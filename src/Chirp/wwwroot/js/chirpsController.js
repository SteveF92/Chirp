(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-chirps")
        .controller("chirpsController", chirpsController);

    function chirpsController($http) {
        var vm = this;
        vm.chirpMessages = [{
            message: "WOOP",
            user: {
                userName: "Steve"
            },
            postTime: new Date()
        }, {
            message: "THIS IS IT",
            user: {
                userName: "Shawn"
            },
            postTime: new Date()
        }];

        vm.newChirpMessage = {};

        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/chirpmessages")
            .then(function (response) {
                //Success
                angular.copy(response.data, vm.chirpMessages);
            }, function (error) {
                //Failure
                vm.errorMessage = "Failed to get Chirps: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });


        vm.addChirpMessage = function () {
            vm.errorMessage = "";
            vm.isBusy = true;
            vm.newChirpMessage.postTime = new Date();

            $http.get("/auth/currentuser")
            .then(function (response) {
                //Success
                vm.newChirpMessage.user = response.data;

                $http.post("/api/chirpmessages", vm.newChirpMessage)
                    .then(function (response) {
                        //Success
                        vm.chirpMessages.push(response.data);
                        vm.newChirpMessage = {};
                    }, function (error) {
                        //Failure
                        vm.errorMessage = "Failed to get Chirps: " + error;
                    })
                    .finally(function () {
                        vm.isBusy = false;
                    });
            }, function (error) {
                //Failure
                vm.errorMessage = "Failed to get current user: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });
        };
    }
})();