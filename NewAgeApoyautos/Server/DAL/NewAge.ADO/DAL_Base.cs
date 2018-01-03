using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using NewAge.DTO.Negocio;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    /// <summary>
    /// Clase base que maneja la conexión a la base de datos
    /// </summary>
    public class DAL_Base
    {
        #region Variables
        
        /// <summary>
        /// Prefijo para hacer referencia en los queries a FK con grupos de empresas
        /// </summary>
        /// 
        public const string EgFkPrefix = "eg_";

        #endregion

        #region Propiedades

        /// <summary>
        /// Get or sets the connection
        /// </summary>
        protected SqlConnection MySqlConnection
        {
            get;
            set;
        }

        /// <summary>
        /// Get or sets the Trasaction
        /// </summary>
        public SqlTransaction MySqlConnectionTx
        {
            get;
            set;
        }

        /// <summary>
        /// empresa relacionada con la solicutud
        /// </summary>
        protected virtual DTO_glEmpresa Empresa
        {
            get;
            set;
        }

        /// <summary>
        /// Usuario de la aplicación
        /// </summary>
        internal int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Empresa Grupo para las solicitudes
        /// </summary>
        protected virtual string EmpresaGrupoID
        {
            get;
            set;
        }


        /// <summary>
        /// Get or sets the connection
        /// </summary>
        internal string loggerConnectionStr;
        protected SqlConnection MySqlConnection_Logger
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_Base(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn)
        {
            this.MySqlConnection = c;
            this.MySqlConnectionTx = tx;
            this.UserId = userId;
            this.Empresa = empresa;

            this.loggerConnectionStr = loggerConn;
        }

        #endregion

        #region Funciones públicas

        /// <summary>
        /// Inicializa los parámetros para un llamado
        /// </summary>
        /// <param name="documentoID"></param>
        /// <param name="empresa"></param>
        public void SetCallParameters(int? documentoID, DTO_glEmpresa empresa, int? userId=null)
        {
            try
            {
                this.Empresa = empresa;
                if (empresa != null && empresa.EmpresaGrupoID_.Value != null && empresa.ID.Value != null)
                {
                    if (documentoID != null && documentoID.Value != 0)
                    {
                        try
                        {
                            string egCtrl = StaticMethods.GetGrupoEmpresasControl(this.MySqlConnection, this.MySqlConnectionTx, this.loggerConnectionStr);
                            this.EmpresaGrupoID = this.GetMaestraEmpresaGrupoByDocumentID(documentoID.Value, this.Empresa, egCtrl);
                        }
                        catch (Exception ex)
                        {
                            var exception = new Exception(DictionaryMessages.Err_Data, ex);
                            Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "SetCallParameters");
                            throw exception;
                        }
                    }
                }

                if (userId != null && userId != 0)
                    this.UserId = userId.Value;
                else
                    this.UserId = 0;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_Data, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "SetCallParameters");
                throw exception;
            }
        }
        
        /// <summary>
        /// Trae el grupo de empresas de acuerdo al un documento
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <returns>Retorna el grupo de empresas</returns>
        internal string GetMaestraEmpresaGrupoByDocumentID(int documentID, DTO_glEmpresa emp, string egControl)
        {
            DTO_aplMaestraPropiedades prop = StaticMethods.GetParameters(this.MySqlConnection, this.MySqlConnectionTx, documentID, this.loggerConnectionStr);
            GrupoEmpresa seguridad = prop.TipoSeguridad;

            string empGrupo = string.Empty;
            if (seguridad == GrupoEmpresa.Automatico)
            {
                empGrupo = emp.ID.Value;
            }
            else if (seguridad == GrupoEmpresa.Individual)
            {
                empGrupo = emp.EmpresaGrupoID_.Value;
            }
            else
            {
                empGrupo = egControl;
            }

            return empGrupo;
        }

        /// <summary>
        /// Exporta un archivo a csv
        /// </summary>
        /// <param name="tipo">Tipo de dato</param>
        /// <param name="list">Lista de datos a exportar</param>
        internal string GetCvsName(out string fileName)
        {
            try
            {
                DAL_glControl glControl = new DAL_glControl(this.MySqlConnection, this.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string filesPath = glControl.DAL_glControl_GetById(AppControl.RutaFisicaArchivos).Data.Value;
                string tempPath = glControl.DAL_glControl_GetById(AppControl.RutaTemporales).Data.Value;
                
                fileName = Guid.NewGuid().ToString() + ".csv";

                string file = filesPath + tempPath + fileName;

                return file;
            }
            catch (Exception ex1)
            {
                var exception = new Exception(DictionaryMessages.Err_ReportCreate, ex1);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Base.cs-GetCvsName");
                throw exception;
            }
        }
        #endregion
    }
}
