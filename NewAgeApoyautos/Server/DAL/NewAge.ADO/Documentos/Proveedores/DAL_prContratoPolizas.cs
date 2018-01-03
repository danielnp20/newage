using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_prContratoPolizas
    /// </summary>
    public class DAL_prContratoPolizas : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prContratoPolizas(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region DAL_prContratoPolizas

        #region Funciones publicas

        /// <summary>
        /// Adiciona el registro en tabla prSaldosDocu
        /// </summary>
        /// <param name="footer">Saldos</param>
        public void DAL_prContratoPolizas_Add(List<DTO_prContratoPolizas> polizas)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prContratoPolizas " +
                                           "    (NumeroDoc    " +
                                           "    ,CubrimientoPolizaID   " +
                                           "    ,TipoMvto " +//
                                           "    ,FechaINI " +//
                                           "    ,FechaVto " +
                                           "    ,PorCubrimiento " +
                                           "    ,VlrCubrimiento " +
                                           "    ,Observacion " +
                                           "    ,eg_prPolizaCubrimiento) " +
                                           "    VALUES" +
                                           "    (@NumeroDoc " +
                                           "    ,@CubrimientoPolizaID" +
                                           "    ,@FechaVto " +
                                           "    ,@PorCubrimiento " +
                                           "    ,@VlrCubrimiento " +
                                           "    ,@Observacion " +
                                           "    ,@eg_prPolizaCubrimiento)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CubrimientoPolizaID", SqlDbType.Char, UDT_CubrimientoPolizaID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoMvto", SqlDbType.TinyInt);//
                mySqlCommand.Parameters.Add("@FechaINI", SqlDbType.DateTime);//
                mySqlCommand.Parameters.Add("@FechaVto", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@PorCubrimiento", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrCubrimiento", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prPolizaCubrimiento", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                foreach (var poliza in polizas)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@NumeroDoc"].Value = poliza.NumeroDoc.Value;
                    mySqlCommand.Parameters["@CubrimientoPolizaID"].Value = poliza.CubrimientoPolizaID.Value;
                    mySqlCommand.Parameters["@TipoMvto"].Value = poliza.TipoMvto.Value;//
                    mySqlCommand.Parameters["@FechaINI"].Value = poliza.FechaINI.Value;//
                    mySqlCommand.Parameters["@FechaVto"].Value = poliza.FechaVto.Value;
                    mySqlCommand.Parameters["@PorCubrimiento"].Value = poliza.PorCubrimiento.Value;
                    mySqlCommand.Parameters["@VlrCubrimiento"].Value = poliza.VlrCubrimiento.Value;
                    mySqlCommand.Parameters["@Observacion"].Value = poliza.Observacion.Value;
                    mySqlCommand.Parameters["@eg_prPolizaCubrimiento"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prPolizaCubrimiento, this.Empresa, egCtrl);
                    #endregion

                    foreach (SqlParameter param in mySqlCommand.Parameters)
                    {
                        if (param.Direction.Equals(ParameterDirection.Input))
                        {
                            if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                                param.Value = DBNull.Value;
                        }
                    }
                    mySqlCommand.ExecuteNonQuery();
                }
           
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoPolizas_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar la solicitud en tabla prSolicitudDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">solicitud</param>
        public void DAL_prContratoPolizas_Upd(List<DTO_prContratoPolizas> polizas)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla prContratoPolizas
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prContratoPolizas " +
                                           "    ,CubrimientoPolizaID  = @CubrimientoPolizaID " +
                                           "    ,TipoMvto  = @TipoMvto " +//
                                           "    ,FechaINI  = @FechaINI " +//
                                           "    ,FechaVto  = @FechaVto " +
                                           "    ,PorCubrimiento  = @PorCubrimiento " +
                                           "    ,VlrCubrimiento  = @VlrCubrimiento " +
                                           "    ,Observacion  = @Observacion " +
                                           "     WHERE Consecutivo = @Consecutivo";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CubrimientoPolizaID", SqlDbType.Char, UDT_CubrimientoPolizaID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoMvto", SqlDbType.TinyInt);//
                mySqlCommand.Parameters.Add("@FechaINI", SqlDbType.DateTime);//
                mySqlCommand.Parameters.Add("@FechaVto", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@PorCubrimiento", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrCubrimiento", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);

                #endregion
                foreach (var poliza in polizas)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@Consecutivo"].Value = poliza.Consecutivo.Value;
                    mySqlCommand.Parameters["@CubrimientoPolizaID"].Value = poliza.CubrimientoPolizaID.Value;
                    mySqlCommand.Parameters["@TipoMvto"].Value = poliza.TipoMvto.Value;//
                    mySqlCommand.Parameters["@FechaINI"].Value = poliza.FechaINI.Value;//
                    mySqlCommand.Parameters["@FechaVto"].Value = poliza.FechaVto.Value;
                    mySqlCommand.Parameters["@PorCubrimiento"].Value = poliza.PorCubrimiento.Value;
                    mySqlCommand.Parameters["@VlrCubrimiento"].Value = poliza.VlrCubrimiento.Value;
                    mySqlCommand.Parameters["@Observacion"].Value = poliza.Observacion.Value;
                    #endregion

                    foreach (SqlParameter param in mySqlCommand.Parameters)
                    {
                        if (param.Direction.Equals(ParameterDirection.Input))
                        {
                            if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                                param.Value = DBNull.Value;
                        }
                    }
                    mySqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoPolizas_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un prSaldosDocu segun el ConsecutivoDetaID
        /// </summary>
        /// <param name="NumeroDoc">ID del registro del detalle</param>
        /// <returns>lista de prSaldosDocu</returns>
        public List<DTO_prContratoPolizas> DAL_prContratoPolizas_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prContratoPolizas with(nolock) where NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                List<DTO_prContratoPolizas> list = new List<DTO_prContratoPolizas>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prContratoPolizas detail = new DTO_prContratoPolizas(dr);
                    list.Add(detail);
                    index++;
                }
                dr.Close();
                return list;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoPolizas_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prConvenio
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_prContratoPolizas_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM prContratoPolizas where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoPolizas_Delete");
                throw exception;
            }
        }

        #endregion

        #endregion
    }
}
