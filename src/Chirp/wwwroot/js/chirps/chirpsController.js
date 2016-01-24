(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-chirps")
        .controller("chirpsController", chirpsController);

    function chirpsController($http) {
        var vm = this;
        vm.chirpPosts = [];
        vm.newChirpPost = {};
        vm.errorMessage = "";
        

        vm.getChirps = function () {
            $http.get("/api/chirpposts")
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

        vm.isBusy = true;
        vm.getChirps();
        
        vm.addChirpPost = function () {
            vm.errorMessage = "";
            vm.isBusy = true;
            vm.newChirpPost.postTime = new Date();

            $http.get("/auth/currentuser")
            .then(function (response) {
                //Success
                vm.newChirpPost.user = response.data;

                $http.post("/api/chirpposts", vm.newChirpPost)
                    .then(function (response) {
                        //Success
                        vm.chirpPosts.push(response.data);
                        vm.newChirpPost.message = "";
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