using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL Recibo Caja
    /// </summary>
    public class DAL_tsReciboCajaDocu : DAL_Base
    {
       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_tsReciboCajaDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        #region CRUD

        /// <summary>
        /// Consulta una cuenta por pagar segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_tsReciboCajaDocu DAL_tsReciboCajaDocu_Get(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from tsReciboCajaDocu with(nolock) where NumeroDoc = @NumeroDoc ";
                
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                DTO_tsReciboCajaDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_tsReciboCajaDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_tsReciboCajaDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla tsReciboCajaDocu 
        /// </summary>
        /// <param name="doc">documento</param>
        /// <returns></returns>
        public void DAL_tsReciboCajaDocu_Add(DTO_tsReciboCajaDocu doc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "INSERT INTO tsReciboCajaDocu (EmpresaID, NumeroDoc, CajaID, BancoCuentaID, Valor, Iva, ClienteID, TerceroID " +
                                           "   , NumeroDocNomina, FechaConsignacion, FechaAplica,PagaduriaID, eg_tsCaja, eg_tsBancosCuenta, eg_faCliente, eg_coTercero,eg_ccPagaduria) " +
                                           "VALUES (@EmpresaID, @NumeroDoc, @CajaID, @BancoCuentaID, @Valor, @Iva, @ClienteID, @TerceroID, " +
                                           "    @NumeroDocNomina, @FechaConsignacion, @FechaAplica, @PagaduriaID,@eg_tsCaja, @eg_tsBancosCuenta, @eg_faCliente, @eg_coTercero,@eg_ccPagaduria)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CajaID", SqlDbType.Char, 5);
                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char, 5);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDocNomina", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaConsignacion", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAplica", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                
                mySqlCommand.Parameters.Add("@eg_tsCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_tsBancosCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = doc.NumeroDoc.Value;
                mySqlCommand.Parameters["@CajaID"].Value = doc.CajaID.Value;
                mySqlCommand.Parameters["@BancoCuentaID"].Value = doc.BancoCuentaID.Value;
                mySqlCommand.Parameters["@Valor"].Value = doc.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = doc.IVA.Value;
                mySqlCommand.Parameters["@ClienteID"].Value = doc.ClienteID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = doc.TerceroID.Value;
                mySqlCommand.Parameters["@NumeroDocNomina"].Value = doc.NumeroDocNomina.Value;
                mySqlCommand.Parameters["@FechaConsignacion"].Value = doc.FechaConsignacion.Value;
                mySqlCommand.Parameters["@FechaAplica"].Value = doc.FechaAplica.Value;
                mySqlCommand.Parameters["@PagaduriaID"].Value = doc.PagaduriaID.Value;
                mySqlCommand.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);

                mySqlCommand.Parameters["@eg_tsCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsCaja, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_tsBancosCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBancosCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faCliente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);

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
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_tsReciboCajaDocu_Add");
                throw exception;
            }          
        
        }          

        /// <summary>
        /// Actualizar el documento en tabla tsReciboCajaDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="doc">documento</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public void DAL_tsReciboCajaDocu_Upd(DTO_tsReciboCajaDocu doc)
        {            
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                #region CommandText
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =  " UPDATE tsReciboCajaDocu " +
                                            " SET EmpresaID = @EmpresaID " +
                                            "    ,CajaID = @CajaID  " +
                                            "    ,BancoCuentaID = @BancoCuentaID  " +
                                            "    ,Valor = @Valor  " +
                                            "    ,Iva = @Iva  " +
                                            "    ,ClienteID = @ClienteID  " +
                                            "    ,TerceroID = @TerceroID  " +
                                            "    ,NumeroDocNomina = @NumeroDocNomina  " +
                                            "    ,FechaConsignacion = @FechaConsignacion  " +
                                            "    ,FechaAplica = @FechaAplica  " +
                                            "    ,PagaduriaID=@PagaduriaID "+
                                            "    ,eg_ccPagaduria = @eg_ccPagaduria  " +
                                            "    ,eg_tsCaja = @eg_tsCaja  " +
                                            "    ,eg_tsBancosCuenta = @eg_tsBancosCuenta  " +
                                            "    ,eg_faCliente = @eg_faCliente  " +
                                            "    ,eg_coTercero = @eg_coTercero  " +
                                            " WHERE NumeroDoc = @NumeroDoc";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CajaID", SqlDbType.Char, 5);
                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char, 5);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDocNomina", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaConsignacion", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAplica", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters.Add("@eg_tsCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_tsBancosCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);     
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = doc.NumeroDoc.Value;
                mySqlCommand.Parameters["@CajaID"].Value = doc.CajaID.Value;
                mySqlCommand.Parameters["@BancoCuentaID"].Value = doc.BancoCuentaID.Value;
                mySqlCommand.Parameters["@Valor"].Value = doc.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = doc.IVA.Value;
                mySqlCommand.Parameters["@ClienteID"].Value = doc.ClienteID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = doc.TerceroID.Value;
                mySqlCommand.Parameters["@NumeroDocNomina"].Value = doc.NumeroDocNomina.Value;
                mySqlCommand.Parameters["@FechaConsignacion"].Value = doc.FechaConsignacion.Value;
                mySqlCommand.Parameters["@FechaAplica"].Value = doc.FechaAplica.Value;
                mySqlCommand.Parameters["@PagaduriaID"].Value = doc.PagaduriaID.Value;
                mySqlCommand.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);

                mySqlCommand.Parameters["@eg_tsCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsCaja, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_tsBancosCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBancosCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faCliente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                #endregion
                mySqlCommand.ExecuteNonQuery();   
          
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_tsReciboCajaDocu_Upd");
                throw exception;
            }           

        }

        #endregion

    }
}
