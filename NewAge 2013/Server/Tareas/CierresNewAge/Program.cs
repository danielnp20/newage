using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NewAge.Negocio;
using NewAge.DTO.Negocio;
using System.Configuration;
using NewAge.ADO;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;

namespace CierresNewAge
{
    class Program
    {
        #region DBConnection

        /// <summary>
        /// Cadena de conexion a la bd del logger
        /// </summary>
        private static string loggerCon;

        /// <summary>
        /// Get or sets the connection
        /// </summary>
        private static SqlConnection _mySqlConnection
        {
            get;
            set;
        }

        /// <summary>
        /// Conecta al provedor Sql
        /// </summary>
        private static void ADO_ConnectDB()
        {
            try
            {
                if (_mySqlConnection.State == ConnectionState.Broken || _mySqlConnection.State == ConnectionState.Closed)
                {
                    _mySqlConnection.Open();
                }
            }
            catch
            {
                ADO_CloseDBConnection();
                throw;
            }
        }

        /// <summary>
        /// Cierra la conexión
        /// </summary>
        public static void ADO_CloseDBConnection()
        {
            try
            {
                if (_mySqlConnection.State != ConnectionState.Closed)
                {
                    _mySqlConnection.Close();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        static void Main(string[] args)
        {
            try
            {
                InitData();

                DAL_MasterSimple _masterSimple = new DAL_MasterSimple(_mySqlConnection, null, null, 0, loggerCon);
                _masterSimple.DocumentID = AppMasters.glEmpresa;

                long count = _masterSimple.DAL_MasterSimple_Count(null, null, true);
                IEnumerable<DTO_MasterBasic> master = _masterSimple.DAL_MasterSimple_GetPaged(count, 1, null, null, true);
                List<DTO_glEmpresa> empresas = master.Cast<DTO_glEmpresa>().ToList();

                foreach (DTO_glEmpresa empresa in empresas)
                {
                    ModuloFachada facade = new ModuloFachada();
                    ModuloAplicacion modApl = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, _mySqlConnection, null, empresa, 0, loggerCon);
                    List<DTO_aplModulo> mods = modApl.aplModulo_GetByVisible(1, true).ToList();

                    DTO_TxResult result = new DTO_TxResult();

                    ////Cartera
                    //if(mods.Any(x => x.ModuloID.Value.ToLower() == ModulesPrefix.cc.ToString()))
                    //{
                    //    ModuloCartera mod_cc = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, _mySqlConnection, null, empresa, 0, loggerCon);
                    //    result = mod_cc.Proceso_CierreDia(); 
                    //}

                    //Proveedores
                    if (mods.Any(x => x.ModuloID.Value.ToLower() == ModulesPrefix.pr.ToString()))
                    {
                        ModuloProveedores mod_pr = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, _mySqlConnection, null, empresa, 0, loggerCon);
                        result = mod_pr.Proceso_CierreDia();
                    }

                    //Planeacion
                    if (mods.Any(x => x.ModuloID.Value.ToLower() == ModulesPrefix.pl.ToString()) && result.Result == ResultValue.OK)
                    {
                        ModuloPlaneacion mod_pl = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, _mySqlConnection, null, empresa, 0, loggerCon);
                        result = mod_pl.Proceso_CierreDia();
                    }
                }
            }
            catch (Exception ex1)
            { ; }
        }

        /// <summary>
        /// Inicializa las variables 
        /// </summary>
        private static void InitData()
        {
            //Conexion a BD
            loggerCon = ConfigurationManager.ConnectionStrings["sqlLoggerConnectionString"].ToString();
            _mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnectionString"].ToString());
            ADO_ConnectDB();
        }
    }
}
