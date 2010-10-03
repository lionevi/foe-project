using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Win32;

namespace ServerInstallation
{
    class ServerInstallation
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Configuring Foe Server...");
            LoadConfig();
            Console.WriteLine("Completed configuring Foe Server.");
        }

        static void LoadConfig()
        {
            try
            {
                // Load XML configuration from file
                Console.WriteLine("Loading configuration file...");
                XmlDocument doc = new XmlDocument();
                doc.Load("foe.config");
                XmlNode node = doc.GetElementsByTagName("Configuration")[0];
                string _dbServer = node["DbServer"].InnerText;
                string _dbUser = node["DbUser"].InnerText;
                string _dbPass = node["DbPass"].InnerText;
                string _dbName = node["DbName"].InnerText;
                string _connStr = "Data Source=" + _dbServer + ";Initial Catalog=" + _dbName +
                    ";Persist Security Info=True;User ID=" + _dbUser + ";Password=" + _dbPass;

                // Write configuration to Windows Registry
                Console.WriteLine("Writing registry...");
                string subKey = "Software\\BBG\\Foe";

                RegistryKey key = Registry.LocalMachine.CreateSubKey(subKey);
                key.SetValue("DbServer", _dbServer);
                key.SetValue("DbUser", _dbUser);
                key.SetValue("DbPass", _dbPass);
                key.SetValue("DbName", _dbName);
                key.Close();
            }
            catch (Exception except)
            {
                throw except;
            }     
        }
    }
}
