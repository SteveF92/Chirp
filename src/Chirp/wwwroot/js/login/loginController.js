(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-login")
        .controller("loginController", loginController);

    function loginController($http, $window) {
        var vm = this;

        vm.credentials = {};

        vm.errorMessage = "";
        vm.isBusy = false;

        vm.login = function () {
            vm.errorMessage = "";
            vm.isBusy = true;

            $http.post("/auth/login", vm.credentials)
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
                    vm.credentials.Password = "";
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }
})();