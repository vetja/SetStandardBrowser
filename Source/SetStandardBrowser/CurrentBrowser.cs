using Microsoft.Win32;

namespace SetStandardBrowser
{
    public class CurrentBrowser
    {

        public string GetCurrentBrowser()
        {
            RegistryKey regkeyhttp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\shell\\Associations\\UrlAssociations\\http\\UserChoice", true);
            return regkeyhttp.GetValue("Progid").ToString();
        }

    }
}