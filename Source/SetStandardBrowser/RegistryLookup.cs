using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Linq;
using SetStandardBrowser.Model;

namespace SetStandardBrowser
{
    public class RegistryLookup
    {
        public Dictionary<string, string> Browser = new Dictionary<string, string>();
        public CurrentBrowser _current;
        public RegistryLookup()
        {
            Browser.Add("Mozilla Firefox", "FIREFOX.EXE");
            Browser.Add("Google Chrome", "Google Chrome");
            Browser.Add("Intenet Explorer", "IEXPLORE.EXE");
            Browser.Add("Opera", "Opera");
            Browser.Add("Safari", "Safari.exe");

            _current = new CurrentBrowser();
        }
        
        public List<string> GetInstalledBrowsers()
        {
            var installedBrowsers = new List<string>();
            
            const string regKeyPfad = @"Software\Clients\StartMenuInternet\";
            var regKey = Registry.LocalMachine.OpenSubKey(regKeyPfad);

            if (regKey != null)
            {
                var subKeyNames = regKey.GetSubKeyNames();

                installedBrowsers.AddRange(subKeyNames.Select(key => Browser.First(b => b.Value.ToLower().Equals(key.ToLower())).Key));
            }

            return installedBrowsers;
        }

        public List<BrowserInfo> GetBrowsersFromClients()
        {
            var browsers = new List<BrowserInfo>();

            const string regKeyPfad = @"Software\Clients\StartMenuInternet\";
            var regKey = Registry.LocalMachine.OpenSubKey(regKeyPfad);

            if (regKey != null)
            {
                var installedBrowsers = regKey.GetSubKeyNames().ToList();

                foreach (var installedBrowser in installedBrowsers)
                {
                    if (installedBrowser != "IEXPLORE.EXE")
                    {
                        var associations = installedBrowser + @"\Capabilities\URLAssociations\";
                        var regkeyAssociations = Registry.LocalMachine.OpenSubKey(regKeyPfad + associations);

                        var browser = new BrowserInfo();
                        GetName(installedBrowser, ref browser);
                        browser.Source = GetIconFile(installedBrowser);
                        browser.UrlAssociationFTP = regkeyAssociations.GetValue("ftp") != null 
                                                        ? regkeyAssociations.GetValue("ftp").ToString()
                                                        : null ;
                        browser.UrlAssociationHTTP = regkeyAssociations.GetValue("http") != null
                                                        ? regkeyAssociations.GetValue("http").ToString()
                                                        : null;
                        browser.UrlAssociationHTTPS = regkeyAssociations.GetValue("https") != null 
                                                        ? regkeyAssociations.GetValue("https").ToString()
                                                        : null;

                        browsers.Add(browser);
                    }
                    else
                    {
                        var browser = new BrowserInfo();
                        GetName(installedBrowser, ref browser);
                        browser.Source = GetIconFile(installedBrowser);
                        browser.UrlAssociationFTP = "IE.HTTP";
                        browser.UrlAssociationHTTP = "IE.HTTP";
                        browser.UrlAssociationHTTPS = "IE.HTTPs";

                        browsers.Add(browser);
                    }

                }
            }
            return browsers;
        }

        private string GetIconFile(string installedBrowser)
        {
            switch (installedBrowser)
            {
                case "FIREFOX.EXE":
                    return "Recources/firefox.jpg";
                case "Google Chrome":
                    return "Recources/chrome.jpg";
                case "IEXPLORE.EXE":
                    return "Recources/ie.jpg";
                case "Opera":
                    return "Recources/opera.jpg";
                case "Safari.exe":
                    return "Recources/safari.png";
                default:
                    return "";

            }
        }

        private void GetName(string installedBrowser, ref BrowserInfo browser)
        {
            switch (installedBrowser)
            {
                case "FIREFOX.EXE":
                    browser.Name = BrowserEnum.FireFox.ToString();
                    browser.IsActive = IsBrowserActive(BrowserEnum.FireFox);
                    break;
                case "Google Chrome":
                    browser.Name = BrowserEnum.Chrome.ToString();
                    browser.IsActive = IsBrowserActive(BrowserEnum.Chrome);
                    break;
                case "IEXPLORE.EXE":
                    browser.Name = BrowserEnum.InternetExplorer.ToString();
                    browser.IsActive = IsBrowserActive(BrowserEnum.InternetExplorer);
                    break;
                case "Opera":
                    browser.Name = BrowserEnum.Opera.ToString();
                    browser.IsActive = IsBrowserActive(BrowserEnum.Opera);
                    break;
                case "Safari.exe":
                    browser.Name = BrowserEnum.Safari.ToString();
                    browser.IsActive = IsBrowserActive(BrowserEnum.Safari);
                    break;
                default:
                    browser.Name = installedBrowser;
                    browser.IsActive = false;
                    break;
            }
        }

        public bool? IsBrowserActive(BrowserEnum browser)
        {
            BrowserEnum currentBrowser;

            switch (_current.GetCurrentBrowser())
            {
                case "FirefoxURL":
                    currentBrowser = BrowserEnum.FireFox;
                    break;
                case "ChromeHTML":
                    currentBrowser = BrowserEnum.Chrome;
                    break;
                case "SafariURL":
                    currentBrowser = BrowserEnum.Safari;
                    break;
                case "Opera.Protocol":
                    currentBrowser = BrowserEnum.Opera;
                    break;
                case "IE.HTTP":
                    currentBrowser = BrowserEnum.InternetExplorer;
                    break;
                default:
                    return false;
            }

            return currentBrowser == browser;
        }

        public void ChangeDefaultBrowser(string http, string https )
        {
            RegistryKey regkeyhttp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\shell\\Associations\\UrlAssociations\\http\\UserChoice", true);
            string browserhttp = regkeyhttp.GetValue("Progid").ToString();

            if (browserhttp != http)
            {
                regkeyhttp.SetValue("Progid", http);
            }

            RegistryKey regkeyhttps = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\shell\\Associations\\UrlAssociations\\https\\UserChoice", true);
            string browserhttps = regkeyhttps.GetValue("Progid").ToString();

            if (browserhttps != https)
            {
                regkeyhttps.SetValue("Progid", https);
            }

            RegistryKey regkeyftp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\shell\\Associations\\UrlAssociations\\ftp\\UserChoice", true);
            string browserftp = regkeyftp.GetValue("Progid").ToString();

            if (browserftp != http)
            {
                regkeyftp.SetValue("Progid", http);
            }
        }
    }
}