(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-chirp")
        .controller("resetPasswordController", resetPasswordController);

    function resetPasswordController($http, $window) {
        var vm = this;
        vm.isBusy = false;
        vm.credentials = {
            email: pageEmail,
            code: pageCode
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