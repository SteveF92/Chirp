(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-user")
        .controller("userController", userController);

    function userController($http, $scope, chirpPostHub) {
        var vm = this;
        vm.user = {};
        vm.pageUserName = pageUserName;
        vm.chirpPosts = [];
        vm.errorMessage = "";

        vm.getUser = function () {
            vm.isBusy = true;
            var userUrl = "/api/user/";
            userUrl = userUrl.concat(vm.pageUserName);
            $http.get(userUrl)
            .then(function (response) {
                //Success
                angular.copy(response.data, vm.user);
            }, function (error) {
                //Failure
                vm.errorMessage = "Failed to get User Info: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });
        };

        vm.getChirps = function () {
            vm.isBusy = true;
            var userUrl = "/api/chirpposts/";
            userUrl = userUrl.concat(vm.pageUserName);
            $http.get(userUrl)
            .then(function (response) {
                //Success
                angular.copy(response.data, vm.chirpPosts);
            }, function (error) {
                //Failure
                vm.errorMessage = "Failed to get Chirps: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });
        };

        vm.getUser();
        vm.getChirps();

        // Method which receives data.
        chirpPostHub.client.refreshChirps = function () {
            // Method which handles messages.
            vm.getChirps();
            $scope.$apply();
        };
    }
})();