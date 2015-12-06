(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-chirps")
        .controller("chirpsController", chirpsController);

    function chirpsController() {
        var vm = this;
        vm.name = "Steve";

    }
})();