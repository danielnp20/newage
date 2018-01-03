using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace RunScripts
{
    class Program
    {
        //Paths
        private static string _scriptsPath;
        private static string _resultsPath;
        private static string _resultsFileName;
        //Results file
        private static FileStream _fs;
        private static StreamWriter _writer;

        static void Main(string[] args)
        {
            try
            {
                //Gets paths
                string rootFolder = ConfigurationManager.AppSettings["Path.RootFolder"];
                string serversFolder = ConfigurationManager.AppSettings["Path.Servers"];
                string backUpsFolder = ConfigurationManager.AppSettings["Path.BackUps"];
                string resultsFolder = ConfigurationManager.AppSettings["Path.Results"];
                string scriptsFolder = ConfigurationManager.AppSettings["Path.Scripts"];

                //Function paths
                string serverFile;
                string backUpsPath;

                #region Validate Paths

                //Root
                if (!Directory.Exists(rootFolder)) 
                {
                    MessageBox.Show("No se encontraron las rutas de los archivos");
                    return;
                }

                //Servers
                serverFile = rootFolder + serversFolder + "Servers.txt";
                if (!File.Exists(serverFile))
                {
                    MessageBox.Show("No se encontro el archivo con la información de los servidores");
                    return;
                }

                //Backups
                backUpsPath = rootFolder + backUpsFolder;

                //Scripts
                _scriptsPath = rootFolder + scriptsFolder;

                //Resultsesults
                _resultsPath = rootFolder + resultsFolder;
                _resultsFileName = _resultsPath + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
                if (!Directory.Exists(_resultsPath))
                {
                    Directory.CreateDirectory(_resultsPath);
                }

                if (File.Exists(_resultsFileName))
                {
                    File.Delete(_resultsFileName);
                }

                #endregion
                #region File writer

                _fs = new FileStream(_resultsFileName, FileMode.OpenOrCreate, FileAccess.Write);
                _writer = new StreamWriter(_fs);
                
                #endregion

                int serverNumber = 1;
                string connStr = string.Empty;
                StreamReader file = new StreamReader(serverFile);
                while ((connStr = file.ReadLine()) != null)
                {
                    if (!connStr.StartsWith("//"))
                    {
                        _writer.WriteLine("Servidor " + serverNumber.ToString() + ": " + connStr);

                        bool txOk = true;
                        bool validateServer = true;
                        Server server = null;
                        SqlConnection conn = null;

                        #region Check Connection string

                        try
                        {
                            conn = new SqlConnection(connStr);
                            conn.Open();

                            _writer.WriteLine("\t" + "Connection: Ok");
                        }
                        catch(Exception ex)
                        {
                            validateServer = false;
                            _writer.WriteLine("\t" + "Connection: Error - " + ex.Message);
                            if (server != null && server.ConnectionContext != null)
                                server.ConnectionContext.RollBackTransaction();
                        }

                        #endregion
                        #region BackUp
                        if (validateServer)
                        {
                            try
                            {
                                string backUpName = backUpsPath + conn.Database + ".bak";
                                string query = "Backup database " + conn.Database + " to disk='" + backUpName + "'";
                                SqlCommand cmd = new SqlCommand(query, conn);
                                cmd.CommandTimeout = 5000;
                                cmd.ExecuteNonQuery();

                                _writer.WriteLine("\t" + "BackUp: OK");

                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                validateServer = false;
                                _writer.WriteLine("\t" + "BackUp: Error - " + ex.Message);
                                if (server != null && server.ConnectionContext != null)
                                    server.ConnectionContext.RollBackTransaction();
                            }
                        }
                        #endregion
                        #region Ejecuta los scripts
                        if (validateServer)
                        {
                            try
                            {
                                //Crea la conexió y transacción
                                server = new Server(new ServerConnection(conn));
                                server.ConnectionContext.BeginTransaction();

                                //Recorre y ejecuta los scripts
                                txOk = ProcessDirectory(server, _scriptsPath);
                                if (txOk)
                                    server.ConnectionContext.CommitTransaction();
                                else
                                    server.ConnectionContext.RollBackTransaction();
                            }
                            catch (Exception ex)
                            {
                                _writer.WriteLine("\t" + "Scripts: Error - " + ex.Message);
                                if (server != null && server.ConnectionContext != null)
                                    server.ConnectionContext.RollBackTransaction();
                            }
                        }
                        #endregion
                        #region Cierra la conexión

                        if (conn != null && conn.State == ConnectionState.Open)
                            conn.Close();

                        #endregion

                        _writer.WriteLine();
                        _writer.WriteLine();
                        serverNumber++;
                    }
                }

                file.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error crítico (program.cs - Main): " + ex.Message);
            }
            finally
            {
                _writer.Close();
            }
        }

        /// <summary>
        /// Loops all the folders to find scripts files
        /// </summary>
        /// <param name="folderName"></param>
        private static bool ProcessDirectory(Server server, string targetDirectory)
        {
            // Process the list of files found in the directory. 
            DirectoryInfo di = new DirectoryInfo(targetDirectory);
            FileInfo[] rgFiles = di.GetFiles("*.sql");
            foreach (FileInfo fi in rgFiles)
            {
                try
                {

                    //FileInfo fileInfo = new FileInfo(fi.FullName);
                    string script = File.ReadAllText(fi.FullName, Encoding.Default);
                    server.ConnectionContext.ExecuteNonQuery(script);

                    _writer.WriteLine("\t\t " + fi.Name + ": Ok");
                }
                catch(Exception ex)
                {
                    string errMesage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    _writer.WriteLine("\t\t " + fi.Name + "(Error) " + errMesage);
                    return false;
                }
            }

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                _writer.WriteLine("\t" + "Directorio: " + Path.GetFileName(Path.GetDirectoryName(subdirectory + "\\")) + ": ");
                bool result = ProcessDirectory(server, subdirectory);
                if (!result)
                    return result;
            }

            return true;
        }

    }
}
