(function () {
    "use strict";

    // SignalR's hub object.
    var chirpPostHub = $.connection.chirpPostHub;

    $(function () {
        $.connection.hub.logging = true;
        $.connection.hub.start();
    });

    angular.module('app-chirp').value('chirpPostHub', chirpPostHub);
})();