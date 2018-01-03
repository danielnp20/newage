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
    public class DAL_ccSolicitudDevolucionDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudDevolucionDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDevolucion
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudDevolucionDeta_Add(DTO_ccSolicitudDevolucionDeta devolucion)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDEV", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DevCausalID", SqlDbType.Char,(5));
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char,UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccDevolucionCausal", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna los valores

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = devolucion.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDEV"].Value = devolucion.NumeroDEV.Value;
                mySqlCommandSel.Parameters["@DevCausalID"].Value = devolucion.DevCausalID.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = devolucion.Observaciones.Value;
                mySqlCommandSel.Parameters["@eg_ccDevolucionCausal"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccDevolucionCausal, this.Empresa, egCtrl);
                #endregion

                mySqlCommandSel.CommandText =
                                            "insert into ccSolicitudDevolucionDeta(    " +
                                                        " NumeroDoc,    " +
                                                        " NumeroDEV,    " +
                                                        " DevCausalID,    " +
                                                        " Observaciones,    " +
                                                        " eg_ccDevolucionCausal) " +
                                            "values (   " +
                                                        "   @NumeroDoc,    " +
                                                        "   @NumeroDEV,    " +
                                                        "   @DevCausalID,    " +
                                                        "   @Observaciones,    " +
                                                        "   @eg_ccDevolucionCausal) ";

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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDevolucionDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la tabla ccSolicitudDevolucion
        /// </summary>
        /// <returns></returns>
        public void DAL_ccSolicitudDevolucionDeta_Update(DTO_ccSolicitudDevolucionDeta devolucion)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDEV", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DevCausalID", SqlDbType.Char,(5));
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);

                #endregion
                #region Asigna los valores

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = devolucion.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDEV"].Value = devolucion.NumeroDEV.Value;
                mySqlCommandSel.Parameters["@DevCausalID"].Value = devolucion.DevCausalID.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = devolucion.Observaciones.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = devolucion.Consecutivo.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                                            "UPDATE ccSolicitudDevolucionDeta SET " +
                                            "   NumeroDoc=@NumeroDoc, " + 
                                            "   NumeroDEV=@NumeroDEV, " +
                                            "   DevCausalID=@DevCausalID, " +
                                            "   Observaciones=@Observaciones " +
                                            "WHERE  Consecutivo = @Consecutivo ";
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDevolucionDeta_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae las devoluciones de una libranza
        /// </summary>
        /// <returns>retorna un registro de DTO_ccSolicitudDevolucionDeta</returns>
        public List<DTO_ccSolicitudDevolucionDeta> DAL_ccSolicitudDevolucionDetaDeta_GetByNumeroDEV(int numDEV, int numDocLibranza)
        {
            try
            {
                List<DTO_ccSolicitudDevolucionDeta> results = new List<DTO_ccSolicitudDevolucionDeta>(); ;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDEV", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDEV"].Value = numDEV;
                 mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                 mySqlCommand.Parameters["@NumeroDoc"].Value = numDocLibranza;
                
                mySqlCommand.CommandText = "SELECT * FROM ccSolicitudDevolucionDeta with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc  and NumeroDEV = @NumeroDEV ";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDevolucionDeta dev = new DTO_ccSolicitudDevolucionDeta(dr);
                    results.Add(dev);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDevolucionDeta_GetByLibranza");
                throw exception;
            }
        }

        #endregion
    }

}
