using System.Windows.Media;
using Caliburn.Micro;
using SetStandardBrowser;
using System.Linq;
using SetStandardBrowser.Model;

namespace View
{
    public class AppViewModel:PropertyChangedBase
    {
        private RegistryLookup _regLookup;
        public int BrowserCount;
        public IObservableCollection<BrowserInfo> BrowserInfos { get; set; }

        public AppViewModel()
        {
            _regLookup = new RegistryLookup();
            BrowserInfos = new BindableCollection<BrowserInfo>();
            RefreshBrowserList();
        }

        public void RefreshBrowserList()
        {
            BrowserInfos.Clear();

            var browserInfos = _regLookup.GetBrowsersFromClients();
            BrowserInfos.AddRange(browserInfos);
        }

        public void ChangeBrowser(string ftp, string http, string https)
        {
            _regLookup.ChangeDefaultBrowser(http, https);

            RefreshBrowserList();
        }
    }
}