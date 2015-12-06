(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-chirps")
        .controller("chirpsController", chirpsController);

    function chirpsController() {
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