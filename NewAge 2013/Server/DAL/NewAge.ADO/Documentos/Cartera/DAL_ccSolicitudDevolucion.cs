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
    public class DAL_ccSolicitudDevolucion : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudDevolucion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDevolucion
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudDevolucion_Add(DTO_ccSolicitudDevolucion devolucion)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDEV", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaDEV", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaRAD", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.VarChar, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glActividadFlujo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna los valores

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = devolucion.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDEV"].Value = devolucion.NumeroDEV.Value;
                mySqlCommandSel.Parameters["@FechaDEV"].Value = devolucion.FechaDEV.Value;
                mySqlCommandSel.Parameters["@FechaRAD"].Value = devolucion.FechaRAD.Value;
                mySqlCommandSel.Parameters["@seUsuarioID"].Value = devolucion.seUsuarioID.Value;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = devolucion.ActividadFlujoID.Value;
                mySqlCommandSel.Parameters["@eg_glActividadFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl);
                #endregion

                mySqlCommandSel.CommandText =
                                            "insert into ccSolicitudDevolucion(    " +
                                                        " NumeroDoc,    " +
                                                        " NumeroDEV,    " +
                                                        " FechaDEV,    " +
                                                        " FechaRAD,    " +
                                                        " seUsuarioID,    " +
                                                        " ActividadFlujoID,    " +
                                                        " eg_glActividadFlujo) " +
                                            "values (   " +
                                                        "   @NumeroDoc,    " +
                                                        "   @NumeroDEV,    " +
                                                        "   @FechaDEV,    " +
                                                        "   @FechaRAD,    " +
                                                        "   @seUsuarioID,    " +
                                                        "   @ActividadFlujoID,    " +
                                                        "   @eg_glActividadFlujo) ";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDevolucion_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la tabla ccSolicitudDevolucion
        /// </summary>
        /// <returns></returns>
        public void DAL_ccSolicitudDevolucion_Update(DTO_ccSolicitudDevolucion devolucion)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDEV", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaDEV", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaRAD", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.VarChar, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);

                #endregion
                #region Asigna los valores

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = devolucion.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDEV"].Value = devolucion.NumeroDEV.Value;
                mySqlCommandSel.Parameters["@FechaDEV"].Value = devolucion.FechaDEV.Value;
                mySqlCommandSel.Parameters["@FechaRAD"].Value = devolucion.FechaRAD.Value;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = devolucion.ActividadFlujoID.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = devolucion.Consecutivo.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccSolicitudDevolucion SET " +
                    "   NumeroDoc=@NumeroDoc, " + 
                    "   NumeroDEV=@NumeroDEV, " + 
                    "   FechaDEV=@FechaDEV, " + 
                    "   FechaRAD=@FechaRAD, " + 
                    "   ActividadFlujoID=@ActividadFlujoID " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDevolucion_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae las devoluciones de una libranza
        /// </summary>
        /// <returns>retorna un registro de DTO_ccSolicitudDevolucion</returns>
        public List<DTO_ccSolicitudDevolucion> DAL_ccSolicitudDevolucion_GetByLibranza(int NumDocSolicitud)
        {
            try
            {
                List<DTO_ccSolicitudDevolucion> results = new List<DTO_ccSolicitudDevolucion>(); ;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumDocSolicitud;

                mySqlCommand.CommandText = "SELECT * FROM ccSolicitudDevolucion with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc order by consecutivo desc";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDevolucion dev = new DTO_ccSolicitudDevolucion(dr);
                    results.Add(dev);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDevolucion_GetByLibranza");
                throw exception;
            }
        }

        #endregion
    }

}
