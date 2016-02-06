applicationModule.controller('chirpsController',
    function ($scope, chirpPostHub) {
        var vm = this;
        vm.test = 0;

        // Method which receives data.
        chirpPostHub.client.refreshChirps = function (message) {
            // Method which handles messages.
            vm.test += 1;
            $scope.$apply();
        };

        vm.woop = function () {
            chirpPostHub.server.refreshChirps("WOOP");
        };
    });