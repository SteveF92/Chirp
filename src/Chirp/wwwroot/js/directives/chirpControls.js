(function () {
    "use strict";

    angular.module("chirpControls", [])
        .directive("chirpPostItem", chirpPostItem);

    function chirpPostItem() {
        return {
            scope: {
                chirpPost: "=chirpPost"
            },
            restrict: "E",
            templateUrl: "/views/chirpPostItem.html"
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
