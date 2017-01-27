using System;
using Microsoft.Win32;
using System.Text.RegularExpressions;

class DotNetVersion {
    private static Version Get45or451FromRegistry()
    {
        Version version = null;
        using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\")) {
            int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
            var versionString = CheckFor45DotVersion(releaseKey);
            if (versionString != null){
                version = new Version(versionString);
            }
            // Console.WriteLine("Version: " + version);
        }
        return version;
    }

    // Checking the version using >= will enable forward compatibility,  
    // however you should always compile your code on newer versions of 
    // the framework to ensure your app works the same. 
    private static string CheckFor45DotVersion(int releaseKey)
    {
        if (releaseKey >= 393273) {
           // Console.WriteLine("4.6 RC or later");
           return "4.6";
        }
        if ((releaseKey >= 379893)) {
            // Console.WriteLine("4.5.2 or later");
            return "4.5.2";
        }
        if ((releaseKey >= 378675)) {
            // Console.WriteLine("4.5.1 or later");
            return "4.5.1";
        }
        if ((releaseKey >= 378389)) {
            // Console.WriteLine("4.5 or later");
            return "4.5";
        }
        // This line should never execute. A non-null release key should mean 
        // that 4.5 or later is installed.
        // return "No 4.5 or later version detected";
        return null;
    }

    private static Version Get1to4LatestVersionFromRegistry()
    {
        Version version = null;
         // Opens the registry key for the .NET Framework entry. 
            using (RegistryKey ndpKey = 
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\")) //"
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
                        // Console.WriteLine(versionKeyName + "  " + name);
                        version = ComprateVersionToString(version, versionKeyName);
                    else
                    {
                        if (sp != "" && install == "1")
                        {
                            // Console.WriteLine(versionKeyName + "  " + name + "  SP" + sp);
                            version = ComprateVersionToString(version, versionKeyName);
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
                            // Console.WriteLine(versionKeyName + "  " + name);
                            version = ComprateVersionToString(version, versionKeyName);
                        // else
                        // {
                        //     if (sp != "" && install == "1")
                        //     {
                        //         Console.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
                        //     }
                        //     else if (install == "1")
                        //     {
                        //         Console.WriteLine("  " + subKeyName + "  " + name);
                        //     }
                        //
                        // }

                    }

                }
            }
        }
        return version;
    }
  private static Version ComprateVersionToString(Version v, string s) {
      s = Regex.Replace(s, "[^0-9.]", "");
      Version vs;
      try {
          vs = new Version(s);
      } 
      catch(Exception)
      {
          return v;
      }
      if (v == null) {
          return vs;
      }
      if (v.CompareTo(vs) < 0) {
          return vs;
      }
      return v;
  }
  static void Main(string[] args) {
      Version version = Get45or451FromRegistry();

      if (version == null) {
          version = Get1to4LatestVersionFromRegistry();
      }
      if (version != null) {
          Console.WriteLine(version);
      } else {
          Console.WriteLine("0");
      }
      if (args.Length == 0)
      {
          Console.ReadLine();
      }
  }
}
