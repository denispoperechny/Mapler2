using System.Web;
using System.Web.Mvc;
using Mapler.API.Security;

namespace Mapler.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
