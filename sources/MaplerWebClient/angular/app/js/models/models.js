/**
 * Created by denis on 1/31/2015.
 */

if (!Mapler){
    var Mapler = {};
}

Mapler.loginStatus = function(isLoggedIn, userLogin, userFullName) {
    this.isLoggedIn = isLoggedIn;
    this.userLogin = userLogin;
    this.userFullName = userFullName;
    //this.userCompanies = ...
}