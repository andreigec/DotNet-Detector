using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace DotNetDetector
{
    public class NetVersion
    {
        public string Version;
        public string SubVersion;
        public string ServicePack;

        public NetVersion(string servicePack, string subVersion, string version)
        {
            ServicePack = servicePack;
            SubVersion = subVersion;
            Version = version;
        }
    }

    public static class Controller
    {

        public static void Refresh(Form1 f)
        {
            var versions = GetVersionFromRegistry();
            versions.ForEach(f.AddVersion);
        }

        private static List<NetVersion> GetVersionFromRegistry()
        {
            var ret = new List<NetVersion>();
            // Opens the registry key for the .NET Framework entry. 
            using (RegistryKey ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5  
                // or later, you can use: 
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,  
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {

                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                            Console.WriteLine(versionKeyName + "  " + name);
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                ret.Add(new NetVersion(sp, name, versionKeyName.Trim(new[] { 'v' })));
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                                Console.WriteLine(versionKeyName + "  " + name);
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    Console.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    ret.Add(new NetVersion("", subKeyName, name));
                                }

                            }

                        }

                    }
                }
            }
            return ret;
        }
    }
}
