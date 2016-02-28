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
        vm.firstLoad = false;
        vm.gotUser = false;
        vm.gotChirps = false;

        vm.getUser = function () {
            vm.isBusyUser = true;
            var userUrl = "/api/user/";
            userUrl = userUrl.concat(vm.pageUserName);
            $http.get(userUrl)
            .then(function (response) {
                //Success
                angular.copy(response.data, vm.user);
                vm.gotUser = true;
            }, function (error) {
                //Failure
                vm.errorMessage = "Failed to get User Info: " + error;
            })
            .finally(function () {
                vm.isBusyUser = false;
            });
        };

        vm.getChirps = function () {
            vm.isBusyChirps = true;
            var userUrl = "/api/chirpposts/";
            userUrl = userUrl.concat(vm.pageUserName);
            $http.get(userUrl)
            .then(function (response) {
                //Success
                angular.copy(response.data, vm.chirpPosts);
                vm.gotChirps = true;
            }, function (error) {
                //Failure
                vm.errorMessage = "Failed to get Chirps: " + error;
            })
            .finally(function () {
                vm.isBusyChirps = false;
            });
        };

        vm.getUser();
        vm.getChirps();

        $scope.$watch("vm.gotChirps", function () {
            if (vm.gotUser === true) {
                vm.firstLoad = true;
            }
        });

        $scope.$watch("vm.gotUser", function () {
            if (vm.gotChirps === true) {
                vm.firstLoad = true;
            }
        });

        // Method which receives data.
        chirpPostHub.client.refreshChirps = function () {
            // Method which handles messages.
            vm.getChirps();
            $scope.$apply();
        };
    }
})();