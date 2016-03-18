(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-chirp")
        .controller("resetPasswordController", resetPasswordController);

    function resetPasswordController($scope, $http, $window) {
        var vm = this;
        vm.isBusy = false;
        vm.pageReady = false;

        vm.credentials = {
            email: "",
            code: ""
        };

        vm.init = function (email, code) {
            vm.credentials.email = email;
            vm.credentials.code = code;
            vm.pageReady = true;
        };

        vm.resetPassword = function () {
            vm.errorMessage = "";
            vm.isBusy = true;

            $http.post("/api/user/resetPassword", vm.credentials)
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
                    vm.credentials.password = "";
                    vm.credentials.confirmPassword = "";
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }
})();