'use strict';

// Declare app level module which depends on views, and components
//var maplerWebClient = angular.module('maplerWebClient', [
//    'ngRoute',
//    'maplerWebClient.login'
//]);
//
//maplerWebClient.config(['$routeProvider', function($routeProvider) {
//    $routeProvider.otherwise({redirectTo: '/login'});
//}]);
//
//maplerWebClient.controller('LayoutController', ['$scope', function($scope) {
//    $scope.greeting = 'Hola!';
//    //$scope.isActive = function (viewLocation) {
//    //  return viewLocation === $location.path();
//    //};
//}]);

var maplerWebClient = angular.module('maplerWebClient', [
    'ngRoute',

    'maplerServices',
    'maplerControllers'
//    'maplerFilters',
]);

maplerWebClient.config(['$routeProvider',
    function($routeProvider) {
        $routeProvider.
            when('/login', {
                templateUrl: 'partials/login.html',
                controller: 'LoginCtrl'
            }).
            when('/overview', {
                templateUrl: 'partials/mapItems.html',
                controller: 'MapOverviewCtrl'
            }).
//            when('/phones/:phoneId', {
//                templateUrl: 'partials/phone-detail.html',
//                controller: 'PhoneDetailCtrl'
//            }).
            otherwise({
                redirectTo: '/overview'
            });
    }]);

maplerWebClient.controller('UserStatusCtrl', ['$scope', 'UserStatus',
    function($scope, UserStatus) {
        $scope.loginStatus = UserStatus;

    }]);