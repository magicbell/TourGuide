//(function () {
//    'use strict';

    //Creating module
var myApp = angular.module('routesApp', ['simpleControls', 'ngRoute'])
    .config(function ($routeProvider) {

        $routeProvider.when("/", {
            controller: "routesController",
            templateUrl: "/views/routesView.html"
        });

        $routeProvider.when("/editor/:routeName", {
            controller: "routeEditorController",
            templateUrl: "/views/routeEditorView.html"
        })

        $routeProvider.otherwise({ redirectTo: "/" });
    });

myApp.controller('routesController', ['$scope', '$http', '$location', routesController]);
myApp.controller('routeEditorController', ['$scope', '$http', '$location', '$routeParams', routeEditorController]);

//})();