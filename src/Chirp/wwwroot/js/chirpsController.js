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
            vm.chirpMessages.push({
                message: vm.newChirpMessage.message,
                user: {
                    userName: "PERSON"
                },
                postTime: new Date()
            });
            vm.newChirpMessage = {};
        };
    }
})();