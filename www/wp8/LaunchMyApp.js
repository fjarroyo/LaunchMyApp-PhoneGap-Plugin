(function () {
    "use strict";

    function triggerOpenURL() {
        cordova.exec(
            (typeof handleOpenURL == "function" ? handleOpenURL : null),
            null,
            "LaunchMyApp",
            "getUriData",
            []);
    }

    document.addEventListener("deviceready", triggerOpenURL, false);
    document.addEventListener("resume", triggerOpenURL, false);
}());
