'use strict';

/* Services */

var maplerServices = angular.module('maplerServices', [
//    'ngResource'
]);

//maplerServices.factory('Phone', ['$resource',
//    function($resource){
//        return $resource('phones/:phoneId.json', {}, {
//            query: {method:'GET', params:{phoneId:'phones'}, isArray:true}
//        });
//    }]);

maplerServices.factory('ConfigService', [
    function(){
        var configs = {
            dataServicesConnectionString: 'http://localhost:19384/api/',
            dataServiceAuthentication: 'Basic al9kb2U6cGFzcw=='//'j_doe:pass'
        }
        return configs;
    }]);

maplerServices.factory('RestClient', ['$http', 'ConfigService',
    function($http, ConfigService){

        $http.defaults.headers.common['Authorization'] = ConfigService.dataServiceAuthentication;
        delete $http.defaults.headers.common['X-Requested-With'];

        var client = {
            _connectionString: ConfigService.dataServicesConnectionString,
            getAll: function(dataType) {
                var url = String.format("{0}/{1}", this._connectionString, dataType);
                return $http.get(url);
            },

            get: function(dataType, id) {
                var url = String.format("{0}/{1}/?id={2}", this._connectionString, dataType, id);
                return $http.get(url);
            },

            getMultiple: function(dataType, ids) {
                var url = String.format("{0}/{1}/?getMultiple=true", this._connectionString, dataType);
                $http.post(url, {ids: ids});
            },

            create: function(dataType, item) {
                var url = String.format("{0}/{1}", this._connectionString, dataType);
                $http.post(url, {value: item});
            },

            update: function(dataType, item) {
                var url = String.format("{0}/{1}/?id={2}", this._connectionString, dataType, item.id);
                $http.put(url, {value: item});
            },

            remove: function(dataType, id) {

            }
        };
        return client;
    }]);

maplerServices.factory('UserStatus', [
    function(){
        var loginStatus = new Mapler.loginStatus(true, "me1", "Denys");
        return loginStatus;
    }]);