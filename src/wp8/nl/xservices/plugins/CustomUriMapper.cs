using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.aireuropa.phonegap.Plugins.nl.x_services.plugins.launchmyapp
{
    public class CustomUriMapper : System.Windows.Navigation.UriMapperBase
    {
        private string PROTOCOL = "/Protocol?encodedLaunchUri";

        private string URI_PARAM = "?uri=";

        public string UriToMatch { get; set; }

        //"/MainPage.xaml"
        public string NavigateTo { get; set; }

        public string UriData { get; set; }

        public CustomUriMapper(string uriToMatch, string navigateTo)
        {
            UriToMatch = uriToMatch;
            NavigateTo = navigateTo;
            UriData = string.Empty;
        }

        public override Uri MapUri(Uri uri)
        {
            string receivedUri = System.Net.HttpUtility.UrlDecode(uri.ToString());
            if (receivedUri.Contains(UriToMatch))
            {
                return handleCustomUrl(receivedUri);
            }
            UriData = string.Empty;
            return uri;
        }

        private Uri handleCustomUrl(string receivedUri)
        {            
            UriData = normalizeUri(receivedUri);
            return new Uri(NavigateTo, UriKind.Relative);
        }

        private string normalizeUri(string receivedUri)
        {
            return receivedUri.Replace(PROTOCOL, "");
        }
    }
}
