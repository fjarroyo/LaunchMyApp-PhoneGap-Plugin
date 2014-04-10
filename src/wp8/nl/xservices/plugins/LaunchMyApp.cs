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
using WPCordovaClassLib.Cordova.JSON;

namespace Cordova.Extension.Commands
{

    public class LaunchMyApp : BaseCommand
    {
        private bool phoneApplicationInitialized = false;

        private PhoneApplicationFrame frame;

        private UIElement rootVisual;

        public void initializeApp(string options)
        {
            StartMyAppConfig config = JsonHelper.Deserialize<StartMyAppConfig[]>(options)[0];
            SetUpApp(new CustomUriMapper(config.UriToMatch, config.NavigateTo));
            //DispatchCommandResult(new PluginResult(PluginResult.Status.OK, ""));
            DispatchCommandResult();

        }

        private void SetUpApp(CustomUriMapper customUriMapper)
        {
            if (!phoneApplicationInitialized)
            {
                frame = Application.Current.RootVisual as PhoneApplicationFrame;
                rootVisual = Application.Current.RootVisual;

                frame.NavigationFailed += RootFrame_NavigationFailed;

                frame.UriMapper = customUriMapper;
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
        private string PROTOCOL = "/Protocol?encodedLaunchUri";

        private string URI_PARAM = "?uri=";

        private string UriToMatch { get; set; }

        //"/MainPage.xaml"
        private string NavigateTo { get; set; }

        public CustomUriMapper(string uriToMatch, string navigateTo) {
            UriToMatch = uriToMatch;
            NavigateTo = navigateTo;
        }

        public override Uri MapUri(Uri uri)
        {
            String receivedUri = System.Net.HttpUtility.UrlDecode(uri.ToString());
            if (receivedUri.Contains(UriToMatch))
            {
                return handleCustomUrl(receivedUri);
            }
            return uri;
        }

        private Uri handleCustomUrl(string receivedUri)
        {
            String formattedUri = NavigateTo + URI_PARAM + normalizeUri(receivedUri);
            return new Uri(formattedUri, UriKind.Relative);
        }

        //TODO ver formato url
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

    class StartMyAppConfig
    {
        public string NavigateTo { get; set; }
        public string UriToMatch { get; set;  }

    }

}
