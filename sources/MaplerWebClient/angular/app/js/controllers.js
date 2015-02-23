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

        var mapItems = RestClient.getAll('mapItem')
            .success(function(data) {
            alert(data.length);
            })
            .error(function (data, status) {
                alert('error');
            })
            .finally(function () {
                // Execute logic independent of success/error
                alert('done');
            })
            .catch(function (error) {
                // Catch and handle exceptions from success/error/finally functions
                alert('error');
            });

        //$scope.testData = data1;
        $scope.isLoggedIn = UserStatus;
    }]);

maplerControllers.controller('MyItemsCtrl', ['$scope', 'UserStatus',
    function($scope, UserStatus) {
        $scope.isLoggedIn = UserStatus;
    }]);