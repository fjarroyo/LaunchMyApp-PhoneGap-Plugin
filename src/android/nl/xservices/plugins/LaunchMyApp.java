package nl.xservices.plugins;

import android.content.Intent;
import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaActivity;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.PluginResult;
import org.json.JSONArray;
import org.json.JSONException;

public class LaunchMyApp extends CordovaPlugin {

  private static final String ACTION_CHECKINTENT = "checkIntent";

  private static final String ACTION_GETURIDATA = "getUriData";

  private String uriData = "";

  @Override
  public boolean execute(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
    if (ACTION_CHECKINTENT.equalsIgnoreCase(action)) {
        final Intent intent = ((CordovaActivity) this.webView.getContext()).getIntent();
        if (intent.getDataString() != null) {
            uriData = intent.getDataString();
            callbackContext.sendPluginResult(new PluginResult(PluginResult.Status.OK, uriData));
            intent.setData(null);
            return true;
        } else {
            callbackContext.error("App was not started via the launchmyapp URL scheme. Ignoring this errorcallback is the best approach.");
            return false;
        }
    } else if (ACTION_GETURIDATA.equalsIgnoreCase(action)){
        callbackContext.sendPluginResult(new PluginResult(PluginResult.Status.OK, uriData));
        return true;
    } else {
        callbackContext.error("This plugin only responds to the " + ACTION_CHECKINTENT + ", " + ACTION_GETURIDATA + " action.");
        return false;
    }
  }
}