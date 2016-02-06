var applicationModule = angular.module('application', []);

// SignalR's hub object.
var chirpPostHub = $.connection.chirpPostHub;

$(function () {
    $.connection.hub.logging = true;
    $.connection.hub.start();
});

angular.module('application').value('chirpPostHub', chirpPostHub);