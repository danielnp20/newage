using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO.Documentos.Activos_Fijos
{
    public class DAL_acActivoDocu : DAL_Base
    {
        public DAL_acActivoDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn)
            : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega un registro al acActivoDocu
        /// </summary>
        public DTO_TxResult DAL_acActivoDocu_add(DTO_acActivoDocu acCtrlDocu)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "INSERT INTO [acActivoDocu]" +
                    "          ([EmpresaID]," +
                    "           [NumeroDoc]," +
                    "           [MvtoTipoActID]," +
                    "           [DocumentoREL]," +
                    "           [LocFisicaID]," +
                    "           [Observacion]," +
                    "           [Valor]," +
                    "           [Iva]," +
                    "           [DatoAdd1]," +
                    "           [DatoAdd2]," +
                    "           [DatoAdd3]," +
                    "           [DatoAdd4]," +
                    "           [DatoAdd5]," +
                    "           [eg_acMovimientoTipo]," +
                    "           [eg_glLocFisica])" +
                    "       VALUES" +
                    "           (@EmpresaID" +
                    "           ,@NumeroDoc" +
                    "           ,@MvtoTipoActID" +
                    "           ,@DocumentoREL" +
                    "           ,@LocFisicaID" +
                    "           ,@Observacion" +
                    "           ,@Valor" +
                    "           ,@Iva" +
                    "           ,@DatoAdd1" +
                    "           ,@DatoAdd2" +
                    "           ,@DatoAdd3" +
                    "           ,@DatoAdd4" +
                    "           ,@DatoAdd5" +
                    "           ,@eg_acMovimientoTipo" +
                    "           ,@eg_glLocFisica)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MvtoTipoActID", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@DocumentoREL", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@LocFisicaID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.VarChar, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.VarChar, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.VarChar, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@eg_acMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLocFisica", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de Valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = acCtrlDocu.NumeroDoc.Value;
                mySqlCommand.Parameters["@MvtoTipoActID"].Value = acCtrlDocu.MvtoTipoActID.Value;
                mySqlCommand.Parameters["@DocumentoREL"].Value = acCtrlDocu.DocumentoREL.Value;
                mySqlCommand.Parameters["@LocFisicaID"].Value = acCtrlDocu.LocFisicaID.Value;
                mySqlCommand.Parameters["@Observacion"].Value = acCtrlDocu.Observacion.Value;
                mySqlCommand.Parameters["@Valor"].Value = acCtrlDocu.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = acCtrlDocu.Iva.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = acCtrlDocu.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = acCtrlDocu.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = acCtrlDocu.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = acCtrlDocu.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = acCtrlDocu.DatoAdd5.Value;
                mySqlCommand.Parameters["@eg_acMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLocFisica, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glLocFisica"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acEstado, this.Empresa, egCtrl);
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
                result.Result = ResultValue.NOK;
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoDocu_add");
                throw exception;
            }
            return result;
        }



        /// <summary>
        /// Consulta el documento asociado al alta de activos
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>Documento asocioado al activo</returns>
        public DTO_acActivoDocu DAL_acActivoDocu_Get(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from acActivoDocu with(nolock) where cpCuentaXPagar.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                DTO_acActivoDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_acActivoDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoDocu_Get");
                throw exception;
            }
        }

      
        #endregion
    }
}
