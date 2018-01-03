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
    /// DAL Bancos
    /// </summary>
    public class DAL_tsBancosDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_tsBancosDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones públicas

        /// <summary>
        /// Consulta un registro de la tabla auxiliar
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_tsBancosDocu DAL_tsBancosDocu_Get(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from tsBancosDocu with(nolock) where NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                DTO_tsBancosDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_tsBancosDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_tsBancosDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta los bancos con su nro de cheque
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_tsBancosDocu DAL_tsBancosDocu_GetByBancoCheque(string bancoCuentaID,int? nroCheque)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string filter = string.Empty;
                if(nroCheque != null)
                {
                    mySqlCommand.Parameters.Add("@NroCheque", SqlDbType.Int);
                    mySqlCommand.Parameters["@NroCheque"].Value = nroCheque;
                    filter = " and NroCheque = @NroCheque ";
                }

                mySqlCommand.CommandText = "select * from tsBancosDocu with(nolock) " +
                                           " where EmpresaID = @EmpresaID and BancoCuentaID = @BancoCuentaID  " + filter;

                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char,UDT_BancoCuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char,UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@BancoCuentaID"].Value = bancoCuentaID;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                DTO_tsBancosDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_tsBancosDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_tsBancosDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega datos a la tabla auxiliar
        /// </summary>
        /// <param name="doc">documento</param>
        public void DAL_tsBancosDocu_Add(DTO_tsBancosDocu doc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "INSERT INTO tsBancosDocu(EmpresaID, NumeroDoc, BancoCuentaID, NroCheque, Valor, Iva, MonedaPago, ValorLocal, ValorExtra, " +
                                            "Beneficiario,BeneficiarioID, Dato1, Dato2, Dato3, Dato4, Dato5, Dato6, Dato7, Dato8, Dato9, Dato10, eg_tsBancosCuenta,eg_coTercero)" +
                    "VALUES (@EmpresaID, @NumeroDoc, @BancoCuentaID, @NroCheque, @Valor, @Iva, @MonedaPago, @ValorLocal, @ValorExtra," +
                            "@Beneficiario,@BeneficiarioID, @Dato1, @Dato2, @Dato3, @Dato4, @Dato5, @Dato6, @Dato7, @Dato8, @Dato9, @Dato10, @eg_tsBancosCuenta,@eg_coTercero)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char, UDT_BancoCuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@NroCheque", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@ValorLocal", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorExtra", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Beneficiario", SqlDbType.Char,50);
                mySqlCommand.Parameters.Add("@BeneficiarioID", SqlDbType.Char,UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Dato1", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato2", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato3", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato4", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato5", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato6", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato7", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato8", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato9", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato10", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@eg_tsBancosCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = doc.NumeroDoc.Value;
                mySqlCommand.Parameters["@BancoCuentaID"].Value = doc.BancoCuentaID.Value;
                mySqlCommand.Parameters["@NroCheque"].Value = doc.NroCheque.Value;
                mySqlCommand.Parameters["@Valor"].Value = doc.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = doc.IVA.Value;
                mySqlCommand.Parameters["@MonedaPago"].Value = doc.MonedaPago.Value;
                mySqlCommand.Parameters["@ValorLocal"].Value = doc.ValorLocal.Value;
                mySqlCommand.Parameters["@ValorExtra"].Value = doc.ValorExtra.Value;
                mySqlCommand.Parameters["@Beneficiario"].Value = doc.Beneficiario.Value;
                mySqlCommand.Parameters["@BeneficiarioID"].Value = doc.BeneficiarioÌD.Value;
                mySqlCommand.Parameters["@Dato1"].Value = doc.Dato1.Value;
                mySqlCommand.Parameters["@Dato2"].Value = doc.Dato2.Value;
                mySqlCommand.Parameters["@Dato3"].Value = doc.Dato3.Value;
                mySqlCommand.Parameters["@Dato4"].Value = doc.Dato4.Value;
                mySqlCommand.Parameters["@Dato5"].Value = doc.Dato5.Value;
                mySqlCommand.Parameters["@Dato6"].Value = doc.Dato6.Value;
                mySqlCommand.Parameters["@Dato7"].Value = doc.Dato7.Value;
                mySqlCommand.Parameters["@Dato8"].Value = doc.Dato8.Value;
                mySqlCommand.Parameters["@Dato9"].Value = doc.Dato9.Value;
                mySqlCommand.Parameters["@Dato10"].Value = doc.Dato10.Value;
                mySqlCommand.Parameters["@eg_tsBancosCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBancosCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                #endregion

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_tsBancosDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los datos en la tabla auxiliar
        /// </summary>
        /// <param name="doc">documento</param>
        public void DAL_tsBancosDocu_Upd(DTO_tsBancosDocu doc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                //Actualiza Tabla tsBancosDocu
                #region CommandText
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "UPDATE tsBancosDocu " +
                                            "SET	EmpresaID = @EmpresaID " +
                                            "	, BancoCuentaID = @BancoCuentaID " +
                                            "	, NroCheque = @NroCheque " +
                                            "	, Valor = @Valor " +
                                            "	, Iva = @Iva " +
                                            "	, MonedaPago = @MonedaPago " +
                                            "	, ValorLocal = @ValorLocal " +
                                            "	, ValorExtra = @ValorExtra " +
                                            "   ,Beneficiario = @Beneficiario "+
                                            "   ,BeneficiarioID = @BeneficiarioID " +
                                            "	, NumeroDocCXP = @NumeroDocCXP " +
                                            "	, Dato1 = @Dato1 " +
                                            "	, Dato2 = @Dato2 " +
                                            "	, Dato3 = @Dato3 " +
                                            "	, Dato4 = @Dato4 " +
                                            "	, Dato5 = @Dato5 " +
                                            "	, Dato6 = @Dato6 " +
                                            "	, Dato7 = @Dato7 " +
                                            "	, Dato8 = @Dato8 " +
                                            "	, Dato9 = @Dato9 " +
                                            "	, Dato10 = @Dato10 " +
                                            "	, eg_tsBancosCuenta = @eg_tsBancosCuenta " +
                                            "   , eg_coTercero= @eg_coTercero"+
                                            " WHERE NumeroDoc = @NumeroDoc";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char, 5);
                mySqlCommand.Parameters.Add("@NroCheque", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@ValorLocal", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorExtra", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Beneficiario", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@BeneficiarioID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Dato1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato6", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato7", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato8", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato9", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato10", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@eg_tsBancosCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = doc.NumeroDoc.Value;
                mySqlCommand.Parameters["@BancoCuentaID"].Value = doc.BancoCuentaID.Value;
                mySqlCommand.Parameters["@NroCheque"].Value = doc.NroCheque.Value;
                mySqlCommand.Parameters["@Valor"].Value = doc.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = doc.IVA.Value;
                mySqlCommand.Parameters["@MonedaPago"].Value = doc.MonedaPago.Value;
                mySqlCommand.Parameters["@ValorLocal"].Value = doc.ValorLocal.Value;
                mySqlCommand.Parameters["@ValorExtra"].Value = doc.ValorExtra.Value;
                mySqlCommand.Parameters["@Beneficiario"].Value = doc.Beneficiario.Value;
                mySqlCommand.Parameters["@BeneficiarioID"].Value = doc.BeneficiarioÌD.Value;
                mySqlCommand.Parameters["@NumeroDocCXP"].Value = doc.NumeroDocCXP.Value;
                mySqlCommand.Parameters["@Dato1"].Value = doc.Dato1.Value;
                mySqlCommand.Parameters["@Dato2"].Value = doc.Dato2.Value;
                mySqlCommand.Parameters["@Dato3"].Value = doc.Dato3.Value;
                mySqlCommand.Parameters["@Dato4"].Value = doc.Dato4.Value;
                mySqlCommand.Parameters["@Dato5"].Value = doc.Dato5.Value;
                mySqlCommand.Parameters["@Dato6"].Value = doc.Dato6.Value;
                mySqlCommand.Parameters["@Dato7"].Value = doc.Dato7.Value;
                mySqlCommand.Parameters["@Dato8"].Value = doc.Dato8.Value;
                mySqlCommand.Parameters["@Dato9"].Value = doc.Dato9.Value;
                mySqlCommand.Parameters["@Dato10"].Value = doc.Dato10.Value;
                mySqlCommand.Parameters["@eg_tsBancosCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBancosCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                #endregion

                mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_tsBancosDocu_Upd");
                throw exception;
            }

        }

        #endregion

    }
}
