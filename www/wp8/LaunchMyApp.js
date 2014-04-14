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

    function getUriData(successCallback, errorCallback) {
        cordova.exec(
            successCallback,
            errorCallback,
            "LaunchMyApp",
            "getUriData",
            []);
    }

    document.addEventListener("deviceready", triggerOpenURL, false);
    document.addEventListener("resume", triggerOpenURL, false);
}());
