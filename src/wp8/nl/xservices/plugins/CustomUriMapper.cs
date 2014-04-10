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

        public CustomUriMapper(string uriToMatch, string navigateTo)
        {
            UriToMatch = uriToMatch;
            NavigateTo = navigateTo;
        }

        public override Uri MapUri(Uri uri)
        {
            string receivedUri = System.Net.HttpUtility.UrlDecode(uri.ToString());
            if (receivedUri.Contains(UriToMatch))
            {
                return handleCustomUrl(receivedUri);
            }
            return uri;
        }

        private Uri handleCustomUrl(string receivedUri)
        {
            string formattedUri = NavigateTo + URI_PARAM + normalizeUri(receivedUri);
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
}
