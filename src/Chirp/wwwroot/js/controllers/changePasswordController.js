(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-chirp")
        .controller("changePasswordController", changePasswordController);

    function changePasswordController($http, $window) {
        var vm = this;
        vm.isBusy = false;
      
        vm.changePassword = function () {
            vm.errorMessage = "";
            vm.isBusy = true;

            $http.post("/api/user/changePassword", vm.credentials)
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
                    vm.credentials.newPassword = "";
                    vm.credentials.confirmPassword = "";
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }
})();