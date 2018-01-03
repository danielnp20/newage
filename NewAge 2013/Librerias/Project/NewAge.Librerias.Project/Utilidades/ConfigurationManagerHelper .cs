using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;

namespace NewAge.Librerias.Project
{
    public static class ConfigurationManagerHelper 
    { 
        public static bool Exists() 
        { 
            return Exists(System.Reflection.Assembly.GetEntryAssembly()); 
        } 
        
        public static bool Exists(Assembly assembly)     
        {
            return System.IO.File.Exists(assembly.Location + ".config");    
        }

        public static string GetKey(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key];
            }
            else
            {
                if (ConfigurationManager.AppSettings["ERR_CFG_0001"] != null)
                {
                    string msg = ConfigurationManager.AppSettings["ERR_CFG_0001"];
                    throw new Exception(String.Format(msg, key));
                }
                else
                {
                    throw new Exception(String.Format("Las llaves {0} y {1} no existen en el archivo config", "ERR_CFG_0001", key));
                }
            }
        }
    } 
}
