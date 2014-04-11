(function () {
    "use strict";

    console.log('declarando triggerOpenUrl');

    function triggerOpenURL() {
          cordova.exec(
              (typeof handleOpenURL == "function" ? handleOpenURL : null),
              null,
              "LaunchMyApp",
              "checkIntent",
              []);
    }

    console.log('DeviceReady');
    document.addEventListener("deviceready", triggerOpenURL, false);
    console.log('resume');
    document.addEventListener("resume", triggerOpenURL, false);
    console.log('done');
}());
