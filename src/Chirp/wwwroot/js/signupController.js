(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-signup")
        .controller("signupController", signupController);

    function signupController($http) {
        var vm = this;

        vm.newUser = {};

        vm.errorMessage = "";
        vm.isBusy = false;

        vm.signup = function () {
            vm.errorMessage = "";
            vm.isBusy = true;

            $http.post("/auth/signup", vm.newUser)
                .then(function (response) {
                    //Success
                    vm.newUser = {};
                }, function (error) {
                    //Failure
                    vm.errorMessage = "Failed to signup: " + error;
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }
})();