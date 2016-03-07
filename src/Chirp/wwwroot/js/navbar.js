(function () {
    "use strict";

    // enabling to open the overflow menu as the pure css link
    // would toggle a scroll and therefore hide the menu
    document.getElementById("paradeiser-dropdown").addEventListener("click", function (event) {
        // stopping the scroll
        event.preventDefault();
        // toggling the class
        document.getElementById("paradeiser-more").classList.toggle("open");
    });

    // hide the menu on click onto greybox
    document.getElementById("greybox").addEventListener("click", function (event) {
        // stopping the scroll
        event.preventDefault();
        // toggling the class
        document.getElementById("paradeiser-more").classList.remove("open");
    });

    // enabling headroom
    var myElement = document.querySelector(".paradeiser");
    var headroom = new Headroom(myElement, {
        tolerance: 5,
        onUnpin: function () {
            document.getElementById("paradeiser-more").classList.remove("open");
        }
    });
    headroom.init();
})();