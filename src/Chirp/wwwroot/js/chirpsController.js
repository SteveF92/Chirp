(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-chirps")
        .controller("chirpsController", chirpsController);

    function chirpsController() {
        var vm = this;
        vm.chirpMessages = [{
            message: "WOOP",
            username: "Steve",
            postTime: new Date()
        }, {
            message: "THIS IS IT",
            username: "Shawn",
            postTime: new Date()
        }];

    }
})();