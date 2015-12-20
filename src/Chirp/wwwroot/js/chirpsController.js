(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-chirps")
        .controller("chirpsController", chirpsController);

    function chirpsController($http) {
        var vm = this;
        vm.chirpPosts = [{
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

        vm.newChirpPost = {};

        vm.errorMessage = "";
        vm.isBusy = true;

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
                        vm.newChirp = {};
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