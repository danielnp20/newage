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

namespace NewAge.ADO
{
    /// <summary>
    /// Operaciones sobre la tabla del documento de declaracion de impuestos
    /// </summary>
    public class DAL_coImpDeclaracionDocu : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_coImpDeclaracionDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Agrega un nuevo registro
        /// </summary>
        /// <param name="docu"></param>
        public void DAL_coImpDeclaracionDocu_Add(DTO_coImpDeclaracionDocu docu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ImpuestoDeclID", SqlDbType.Char, UDT_ImpuestoDeclID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@eg_coImpuestoDeclaracion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = docu.NumeroDoc.Value.Value;
                mySqlCommandSel.Parameters["@ImpuestoDeclID"].Value = docu.ImpuestoDeclID.Value;
                mySqlCommandSel.Parameters["@Año"].Value = docu.Año.Value.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = docu.Periodo.Value.Value;
                mySqlCommandSel.Parameters["@eg_coImpuestoDeclaracion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coImpuestoDeclaracion, this.Empresa, egCtrl);

                mySqlCommandSel.CommandText =
                    "INSERT INTO coImpDeclaracionDocu(" +
                    "	EmpresaID,NumeroDoc,ImpuestoDeclID,Año,Periodo,eg_coImpuestoDeclaracion" +
                    ")VALUES(" +
                    "	@EmpresaID,@NumeroDoc,@ImpuestoDeclID,@Año,@Periodo,@eg_coImpuestoDeclaracion" +
                    ")";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coImpDeclaracionDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los registros de un documento
        /// </summary>
        /// <param name="numeroDoc"></param>
        public void DAL_coImpDeclaracionDocu_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText =
                    "DELETE from coImpDeclaracionDocu where EmpresaID=@EmpresaID and NumeroDoc=@NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coImpDeclaracionDocu_Delete");
                throw exception;
            }
        }

    }
}
