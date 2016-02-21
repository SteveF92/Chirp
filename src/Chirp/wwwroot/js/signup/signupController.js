(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-signup")
        .controller("signupController", signupController);

    function signupController($http, $window) {
        var vm = this;

        vm.newUser = {};

        vm.errorMessage = "";
        vm.isBusy = false;

        vm.signup = function () {
            vm.errorMessage = "";
            vm.isBusy = true;

            $http.post("/api/user", vm.newUser)
                .then(function (response) {
                    //Success
                    if (typeof response.data.error === 'undefined') {
                        $window.location.href = response.data.url;
                    }
                    else {
                        //Returned error condition
                        vm.errorMessage = response.data.error;
                    }
                }, function (error) {
                    //Failure
                    vm.errorMessage = error.message;
                    vm.newUser.Password = "";
                    vm.newUser.ConfirmPassword = "";
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }
})();