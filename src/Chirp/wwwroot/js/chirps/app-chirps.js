(function () {
    "use strict";
   
    //Creating the module
    angular.module("app-chirps", ["chirpControls"]);

    // SignalR's hub object.
    var chirpPostHub = $.connection.chirpPostHub;

    $(function () {
        $.connection.hub.logging = true;
        $.connection.hub.start();
    });

    angular.module('app-chirps').value('chirpPostHub', chirpPostHub);
})();