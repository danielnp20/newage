using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateScripts
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Paths
                string _sourcePath = ConfigurationManager.AppSettings["Path.Root"];
                string _resultsPath = ConfigurationManager.AppSettings["Path.Results"];

                #region Validate Paths

                //Root
                if (!Directory.Exists(_sourcePath))
                {
                    MessageBox.Show("No se encontraron las rutas de los archivos");
                    return;
                }

                //Results
                if (!Directory.Exists(_resultsPath))
                {
                    Directory.CreateDirectory(_resultsPath);
                }

                #endregion

                //Version
                Version _currentVersion = new Version(ConfigurationManager.AppSettings["Version"]);

                //Loop directories
                int sqlIndex = 0;
                foreach (string dirPath in Directory.GetDirectories(_sourcePath, "*", SearchOption.AllDirectories))
                {
                    string dirName = new DirectoryInfo(dirPath).Name;
                    Version _directoryVersion = new Version(dirName);

                    if(_currentVersion < _directoryVersion)
                    {
                        //Copy all the files & Replaces any files with the same name
                        foreach (string origin in Directory.GetFiles(dirPath, "*.sql"))
                        {
                            ++sqlIndex;
                            string destination = _resultsPath + sqlIndex.ToString() + ".sql";
                            File.Copy(origin, destination, true);
                        }
                    }
                }

                //Clean empty directories
                foreach (var directory in Directory.GetDirectories(_resultsPath))
                {
                    if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                    {
                        Directory.Delete(directory, false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error crítico (program.cs - Main): " + ex.Message);
            }
        }
    }
}
