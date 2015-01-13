using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc.Filters;
using DataPersistance.Facade;
using Mapler.DataPersistance.Models;
using Mapler.Security;
using Microsoft.Ajax.Utilities;
using Microsoft.Practices.Unity;

namespace Mapler.API.Security
{
    //Implemented accordingly to manual:
    //http://www.asp.net/web-api/overview/security/authentication-filters
    
    // TODO: Refactor
    // TODO: introduce logging
    // TODO: introduce error messages
    public class MaplerBasicAuthenticationAttribute : Attribute, System.Web.Http.Filters.IAuthenticationFilter
    {
        private readonly IPersistentRepository<User> _userRepo;
        private readonly IPersistentRepository<UserPass> _passRepo;

        public MaplerBasicAuthenticationAttribute()
        {
            _userRepo = (IPersistentRepository<User>)
                GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPersistentRepository<User>));
            _passRepo = (IPersistentRepository<UserPass>)
                GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPersistentRepository<UserPass>));

            if (_userRepo == null)
                throw new InvalidOperationException("Cannot resolve IPersistentRepository<User>");
            if (_passRepo == null)
                throw new InvalidOperationException("Cannot resolve IPersistentRepository<UserPass>");
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                return;
            }

            // 3. If there are credentials but the filter does not recognize the 
            //    authentication scheme, do nothing.
            if (authorization.Scheme != "Basic")
            {
                return;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            Tuple<string, string> userNameAndPasword = ParseBasicAuthParameter(authorization.Parameter);
            if (userNameAndPasword == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
            }

            string userName = userNameAndPasword.Item1;
            string password = userNameAndPasword.Item2;

            IPrincipal principal = await AuthenticateAsync(userName, password, cancellationToken);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
            }

            // 6. If the credentials are valid, set principal.
            else
            {
                context.Principal = principal;
            }

        }

        // TODO: add company specification
        private Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            var worker = Task.Factory.StartNew<IPrincipal>(() =>
            {
                var user =
                    _userRepo.GetAll(x => x.Login.Equals(userName, StringComparison.InvariantCultureIgnoreCase) && x.IsActive)
                        .FirstOrDefault();

                if (user == null)
                {
                    Trace.WriteLine("Error. User not found: " + userName);
                    return null;
                }

                var passHash = _passRepo.GetAll(x => x.UserId == user.Id || x.IsActive).First().PassHash;
                if (passHash != UserPass.GetPassHash(password))
                {
                    Trace.WriteLine("Error. Incorrect password for user: " + userName);
                    return null;
                }

                var identity = new MaplerIdentity(user.Id, user.Login);
                return new MaplerPrincipal(identity, null, user.IsSuperUser);
            });

            return worker;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Basic");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Tuple: login, password</returns>
        protected virtual Tuple<string, string> ParseBasicAuthParameter(string parameter)
        {
            try
            {
                var parsed = Encoding.Default.GetString(Convert.FromBase64String(parameter));
                var authPair = parsed.Split(':');

                return new Tuple<string, string>(authPair[0], authPair[1]);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                return null;
            }
        }

    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return response;
        }
    }

    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }

        public AuthenticationHeaderValue Challenge { get; private set; }

        public IHttpActionResult InnerResult { get; private set; }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Only add one challenge per authentication scheme.
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }
            }

            return response;
        }
    }
}