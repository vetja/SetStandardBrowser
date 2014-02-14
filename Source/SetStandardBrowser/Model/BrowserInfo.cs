using System.Media;

using System.Net.Mime;
using System.Windows.Media;

namespace SetStandardBrowser.Model
{
    public class BrowserInfo
    {
        public string Name { get; set; }

        public string Source { get; set; }

        public bool? IsActive { get; set; }

        public string UrlAssociationFTP { get; set; }

        public string UrlAssociationHTTP { get; set; }

        public string UrlAssociationHTTPS { get; set; }
    }
}