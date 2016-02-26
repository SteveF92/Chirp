(function () {
    "use strict";

    //Creating the module
    angular.module("app-user", []);

    // SignalR's hub object.
    var chirpPostHub = $.connection.chirpPostHub;

    $(function () {
        $.connection.hub.logging = true;
        $.connection.hub.start();
    });

    angular.module('app-user').value('chirpPostHub', chirpPostHub);
})();