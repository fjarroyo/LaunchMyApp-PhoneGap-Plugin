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

        public void initializeApp(string options)
        {
            StartMyAppConfig config = deserialize(options);
            DispatchCommandResult(new PluginResult(PluginResult.Status.OK, "Received callback on native code"));
        }

        private StartMyAppConfig deserialize(String options)
        {
            string jsonConfig = JsonHelper.Deserialize<string[]>(options)[0];
            return JsonHelper.Deserialize<StartMyAppConfig>(jsonConfig);
        }

    }


    [DataContract]
    class StartMyAppConfig
    {
        [DataMember(Name = "navigateTo", IsRequired = false)]
        public string NavigateTo { get; set; }

        [DataMember(Name = "uriToMatch", IsRequired = false)]
        public string UriToMatch { get; set;  }

    }

}
