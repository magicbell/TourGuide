
var simpleControls = angular.module('simpleControls', [])
    .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            templateUrl: "/views/waitCursor.html"
        };
    };