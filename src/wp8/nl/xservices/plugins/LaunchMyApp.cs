using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;

namespace CreatePlugin.Plugins.uri
{

    class StartMyApp : BaseCommand
    {
        private bool phoneApplicationInitialized = false;

        private PhoneApplicationFrame frame;

        private UIElement rootVisual;

        public void initializeApp(String options)
        {
            SetUpApp();
            DispatchCommandResult(new PluginResult(PluginResult.Status.OK, ""));
        }

        private void SetUpApp()
        {
            if (!phoneApplicationInitialized)
            {
                frame = Application.Current.RootVisual as PhoneApplicationFrame;
                rootVisual = Application.Current.RootVisual;

                frame.NavigationFailed += RootFrame_NavigationFailed;

                frame.UriMapper = new CustomUriMapper();
                phoneApplicationInitialized = true;
            }
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (rootVisual != frame)
                rootVisual = frame;

            // Remove this handler since it is no longer needed
            frame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                frame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            frame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (frame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

    }

    class CustomUriMapper : System.Windows.Navigation.UriMapperBase
    {
        private String PROTOCOL = "/Protocol?encodedLaunchUri";


        //TODO eliminar Hardcoded string
        private String NAVIGATED_TO = "/MainPage.xaml?uri=";

        public override Uri MapUri(Uri uri)
        {
            //TODO eliminar Hardcoded string
            String receivedUri = System.Net.HttpUtility.UrlDecode(uri.ToString());
            if (receivedUri.Contains("aireuropa:"))
            {
                return handleCustomUrl(receivedUri);
            }
            return uri;
        }

        private Uri handleCustomUrl(string receivedUri)
        {
            String formattedUri = NAVIGATED_TO + normalizeUri(receivedUri);
            return new Uri(formattedUri, UriKind.Relative);
        }

        private string normalizeUri(string receivedUri)
        {
            return receivedUri.Replace(PROTOCOL, "")
                .Replace("&", "_")
                .Replace("+", "_")
                .Replace("?", "-")
                .Replace("=", "_")
                .Replace("/", "-")
                .Replace(":", "-");
        }
    }

}
