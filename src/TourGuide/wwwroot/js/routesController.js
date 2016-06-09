
//    'use strict';

    //var myApp = angular.module('routesApp', []);

    //myApp.controller('routesController', ['$scope', function ($scope) {
var routesController = function ($scope, $http, $location) {
        
    $scope.routes = [];
    $scope.newRoute = {};
    $scope.errorMessage = "";    

    var url = $location.protocol() + "://" + $location.host();
    if ($location.port() != null)
        url += ":" + $location.port();
    url += "/api/routes";

    showRoutes();

    function showRoutes() {
        $scope.isBusy = true;
         
        $http.get(url)
        .then(function (responce) {
            angular.copy(responce.data, $scope.routes);
        }, function (error) {
            $scope.errorMessage = "Failed to load data: " + error;
        })
        .finally(function () {
            $scope.isBusy = false;
        });
    };

    

        $scope.addRoute = function () {
            //$scope.routes.push({ name: $scope.newRoute.name, created: new Date() });
            //$scope.newRoute = {};
            $scope.errorMessage = "";
            $scope.isBusy = true;

            $http.post(url, $scope.newRoute)
            .then(function (responce) {
                $scope.routes.push(responce.data);
            }, function (error) {
                $scope.errorMessage = "Failed to save new route " + error;
            })
            .finally(function () {
                $scope.isBusy = false;
                $scope.newRoute = {};
            });
        };

        $scope.deleteRoute = function (routeId) {

            $scope.errorMessage = "";
            $scope.isBusy = true;

            $http.delete(url + "/" + routeId)
            .then(function (responce) {
                // $scope.points.push(responce.data);
                showRoutes();
                // _showMap($scope.points);
            }, function (error) {
                $scope.errorMessage = "Failed to delete route " + error;
            })
            .finally(function () {
                $scope.isBusy = false;
            });
        }

    };


