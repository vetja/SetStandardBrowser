using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace SetStandardBrowser
{
    class Program
    {
        private static RegistryLookup _registryLookup;

        static void Main(string[] args)
        {
            _registryLookup = new RegistryLookup();

            var browsers = _registryLookup.GetInstalledBrowsers();

            _registryLookup.ChangeDefaultBrowser("IE.HTTP", "IE.HTTPS");
        }

    }
}
