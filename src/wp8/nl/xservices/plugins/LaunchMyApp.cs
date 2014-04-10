using com.aireuropa.phonegap.Plugins.nl.x_services.plugins.launchmyapp;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
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

        public void getUriData(string options)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                string uriData = GetUriDataFromCustomUriMatcher();
                DispatchCommandResult(new PluginResult(PluginResult.Status.OK, uriData));   
            });    
        }

        private string GetUriDataFromCustomUriMatcher()
        {
            var frame = Application.Current.RootVisual as PhoneApplicationFrame;     
            var customUriMapper = frame.UriMapper as CustomUriMapper;
            return customUriMapper.UriData;   
        }       
    }
}
