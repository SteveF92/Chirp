(function () {
    "use strict";

    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor)
        .directive("compareTo", compareTo);

    function waitCursor() {
        return {
            restrict: "E",
            templateUrl: "/views/waitCursor.html"
        };
    }

    function compareTo() {
        return {
            require: "ngModel",
            scope: {
                otherModelValue: "=compareTo"
            },
            link: function (scope, element, attributes, ngModel) {

                ngModel.$validators.compareTo = function (modelValue) {
                    return modelValue == scope.otherModelValue;
                };

                scope.$watch("otherModelValue", function () {
                    ngModel.$validate();
                });
            }
        };
    };
})();
