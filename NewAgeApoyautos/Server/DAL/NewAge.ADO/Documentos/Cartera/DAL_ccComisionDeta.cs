using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using System.ComponentModel;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_ccComisionDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccComisionDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public DTO_ccComisionDeta DAL_ccComisionDeta_GetByID(int NumeroDoc)
        {
            try
            {
                DTO_ccComisionDeta result = new DTO_ccComisionDeta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccComisionDeta with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if(dr.Read())
                    result = new DTO_ccComisionDeta(dr);

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccComisionDeta_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccComisionDeta_Add(DTO_ccComisionDeta comisionDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  INSERT INTO ccComisionDeta   " +
                                              "  ([NumeroDoc] "+ 
                                              "   ,[NumDocCredito] "+
                                              "   ,[NumDocCxP] " + 
                                              "   ,[AsesorID] "+ 
                                              "   ,[VlrBase] "+ 
                                              "   ,[Porcentaje] "+
                                              "   ,[VlrComision] " + 
                                              "   ,[eg_ccAsesor] ) " +
                                              "  VALUES " +
                                              "  (@NumeroDoc "+
                                              "   ,@NumDocCredito "+
                                              "   ,@NumDocCxP " +
                                              "   ,@AsesorID "+
                                              "   ,@VlrBase "+
                                              "   ,@Porcentaje "+
                                              "   ,@VlrComision " +
                                              "   ,@eg_ccAsesor ) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCxP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrBase", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrComision", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_ccAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = comisionDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = comisionDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumDocCxP"].Value = comisionDeta.NumDocCxP.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = comisionDeta.AsesorID.Value;
                mySqlCommandSel.Parameters["@VlrBase"].Value = comisionDeta.VlrBase.Value;
                mySqlCommandSel.Parameters["@Porcentaje"].Value = comisionDeta.Porcentaje.Value;
                mySqlCommandSel.Parameters["@VlrComision"].Value = comisionDeta.VlrComision.Value;
                mySqlCommandSel.Parameters["@eg_ccAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccComisionDeta_Add");
                throw exception;
            }
        }
        
        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccComisionDeta_Update(DTO_ccComisionDeta comisionDeta)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCxP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrBase", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrComision", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_ccAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = comisionDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = comisionDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumDocCxP"].Value = comisionDeta.NumDocCxP.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = comisionDeta.AsesorID.Value;
                mySqlCommandSel.Parameters["@VlrBase"].Value = comisionDeta.VlrBase.Value;
                mySqlCommandSel.Parameters["@Porcentaje"].Value = comisionDeta.Porcentaje.Value;
                mySqlCommandSel.Parameters["@VlrComision"].Value = comisionDeta.VlrComision.Value;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE ccComisionDeta SET" +
                    "  NumDocCredito = @NumDocCredito "+
                    "  NumDocCxP = @NumDocCxP " +
                    "  AsesorID = @AsesorID "+
                    "  VlrBase = @VlrBase "+
                    "  Porcentaje = @Porcentaje "+
                    "  VlrComision = @VlrComision "+
                    "WHERE  NumeroDoc = @NumeroDoc ";
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccComisionDeta_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public List<DTO_ccComisionDeta> DAL_ccComisionDeta_GetForLiquidacion(DateTime periodo)
        {
            try
            {
                List<DTO_ccComisionDeta> result = new List<DTO_ccComisionDeta>();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                #endregion
                #region CommandText
                mySqlCommand.CommandText =
                    "SELECT DISTINCT credito.NumeroDoc as NumDocCredito, credito.Libranza, credito.FechaLiquida, credito.VlrLibranza, "+
                    "                credito.VlrPrestamo, credito.VlrGiro, asesor.AsesorID, "+
                    "                asesor.Descriptivo as Nombre, asesor.PorcComision "+
                    "FROM ccCierreDia cierre  with(nolock) "+
                    "       INNER JOIN ccCreditoDocu credito with(nolock) ON cierre.AsesorID = cierre.AsesorID "+
                    "       INNER JOIN ccAsesor asesor with(nolock) ON asesor.AsesorID = credito.AsesorID "+
                    "       INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = credito.NumeroDoc " +
                    "WHERE	cierre.TipoDato = 1 AND ctrl.Fecha <= @Periodo AND "+
		            "        credito.NumeroDoc NOT IN "+
		            "        ( "+
                    "            select NumDocCredito from ccComisionDeta " +
		            "        )";
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    string asesorID = dr["AsesorID"].ToString();
                    bool nuevo = true;
                    DTO_ccComisionDeta dto = new DTO_ccComisionDeta();
                    DTO_ccCreditoDocu credito = new DTO_ccCreditoDocu();

                    List<DTO_ccComisionDeta> list = result.Where(x => x.AsesorID.Value == asesorID.Trim()).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto.Aprobado.Value = false;
                        dto.AsesorID.Value = dr["AsesorID"].ToString();
                        dto.Nombre.Value = dr["Nombre"].ToString();
                        dto.Porcentaje.Value = Convert.ToDecimal(dr["PorcComision"]);
                    }
                    credito.NumeroDoc.Value = Convert.ToInt32(dr["NumDocCredito"]);
                    credito.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    credito.FechaLiquida.Value = Convert.ToDateTime(dr["FechaLiquida"]);
                    credito.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                    credito.VlrPrestamo.Value = Convert.ToDecimal(dr["VlrPrestamo"]);
                    credito.VlrGiro.Value = Convert.ToDecimal(dr["VlrGiro"]);
                    dto.Detalle.Add(credito);
                    if(nuevo)
                        result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccComisionDeta_GetForLiquidacion");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public List<DTO_ccComisionDeta> DAL_ccComisionDeta_GetForAprobacion(string actFlujoID)
        {
            try
            {
                List<DTO_ccComisionDeta> result = new List<DTO_ccComisionDeta>();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                #endregion
                #region CommandText
                mySqlCommand.CommandText =
                    "SELECT comiDeta.*, credito.Libranza, credito.FechaLiquida, credito.VlrLibranza, " +
                    "       credito.VlrPrestamo, credito.VlrGiro, asesor.Descriptivo as Nombre " +
                    "FROM ccComisionDeta comiDeta with(nolock) "+
	                "    INNER JOIN ccAsesor asesor ON asesor.AsesorID = comiDeta.AsesorID "+ 
                    "    INNER JOIN ccCreditoDocu credito with(nolock) ON credito.NumeroDoc = comiDeta.NumDocCredito "+
	                "    INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = comiDeta.NumeroDoc "+  
		            "        AND ctrl.Estado = @Estado "+  
	                "    INNER JOIN glactividadEstado act with(nolock) on act.NumeroDoc = comiDeta.NumeroDoc "+  
	                "        AND act.actividadFlujoID = @ActividadFlujoID AND act.CerradoInd = @CerradoInd "+
                    "WHERE comiDeta.NumDocCxP is null ";
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    string asesorID = dr["AsesorID"].ToString();
                    bool nuevo = true;
                    DTO_ccComisionDeta dto = new DTO_ccComisionDeta();
                    DTO_ccCreditoDocu credito = new DTO_ccCreditoDocu();

                    List<DTO_ccComisionDeta> list = result.Where(x => x.AsesorID.Value == asesorID.Trim()).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto.Aprobado.Value = false;
                        dto.Rechazado.Value = false;
                        dto.AsesorID.Value = dr["AsesorID"].ToString();
                        dto.Nombre.Value = dr["Nombre"].ToString();
                        dto.Porcentaje.Value = Convert.ToDecimal(dr["Porcentaje"]);
                    }
                    credito.NumeroDoc.Value = Convert.ToInt32(dr["NumDocCredito"]);
                    credito.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    credito.FechaLiquida.Value = Convert.ToDateTime(dr["FechaLiquida"]);
                    credito.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                    credito.VlrPrestamo.Value = Convert.ToDecimal(dr["VlrPrestamo"]);
                    credito.VlrGiro.Value = Convert.ToDecimal(dr["VlrGiro"]);
                    dto.Detalle.Add(credito);
                    if (nuevo)
                        result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccComisionDeta_GetForAprobacion");
                throw exception;
            }
        }

        #endregion
    }

}
    