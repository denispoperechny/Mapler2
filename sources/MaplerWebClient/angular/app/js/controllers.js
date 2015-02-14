'use strict';

/* Controllers */

var maplerControllers = angular.module('maplerControllers', []);

//maplerControllers.controller('PhoneDetailCtrl', ['$scope', '$routeParams', 'Phone',
//    function($scope, $routeParams, Phone) {
//        $scope.phone = Phone.get({phoneId: $routeParams.phoneId}, function(phone) {
//            $scope.mainImageUrl = phone.images[0];
//        });
//
//        $scope.setImage = function(imageUrl) {
//            $scope.mainImageUrl = imageUrl;
//        }
//    }]);

maplerControllers.controller('LoginCtrl', ['$scope', 'UserStatus',
    function($scope, UserStatus) {
        $scope.isLoggedIn = UserStatus;
    }]);

maplerControllers.controller('CompanyCtrl', ['$scope', 'UserStatus',
    function($scope, UserStatus) {
        $scope.isLoggedIn = UserStatus;
    }]);

maplerControllers.controller('MapOverviewCtrl', ['$scope', 'UserStatus', 'RestClient',
    function($scope, UserStatus, RestClient) {
        var mapItems = RestClient.getAll('mapItem');
        $scope.testData = mapItems;
        alert(mapItems.length);
        $scope.isLoggedIn = UserStatus;
    }]);

maplerControllers.controller('MyItemsCtrl', ['$scope', 'UserStatus',
    function($scope, UserStatus) {
        $scope.isLoggedIn = UserStatus;
    }]);