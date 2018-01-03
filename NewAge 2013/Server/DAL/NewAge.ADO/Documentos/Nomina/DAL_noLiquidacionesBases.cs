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
    public class DAL_noLiquidacionesBases : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noLiquidacionesBases(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
              
        /// <summary>
        /// Ingresa un registro de base al sistema
        /// </summary>
        /// <param name="detalle">detalle de la base</param>
        /// <returns></returns>
        public bool DAL_noLiquidacionesBases_Add(DTO_noLiquidacionesBases detalle)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "    INSERT INTO noLiquidacionesBases   " +
                                               "    ([EmpresaID]   " +
                                               "    ,[NumeroDoc]   " +
                                               "    ,[ConceptoNOID]   " +
                                               "    ,[ConceptoBase]   " +
                                               "    ,[Dias]   " +
                                               "    ,[Valor]   " +
                                               "    ,[eg_noConceptoNOM])   " +
                                               "  VALUES    " +
                                               "  (@EmpresaID    " +
                                               "  ,@NumeroDoc    " +
                                               "  ,@ConceptoNOID    " +
                                               "  ,@ConceptoBase    " +
                                               "  ,@Dias    " +
                                               "  ,@Valor    " +
                                               "  ,@eg_noConceptoNOM)   ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoBase", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Dias", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);             
                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
              
             
                mySqlCommandSel.Parameters["@EmpresaID"].Value = detalle.EmpresaID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = detalle.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = detalle.ConceptoNOID.Value;
                mySqlCommandSel.Parameters["@ConceptoBase"].Value = detalle.ConceptoBase.Value;
                mySqlCommandSel.Parameters["@Dias"].Value = detalle.Dias.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = detalle.Valor.Value;            
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = egCtrl;
           

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesBases_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Selecciona las liquidaciones Base asociadas al numero de documento
        /// </summary>
        /// <param name="numDoc">numero documento</param>
        /// <returns></returns>
        public List<DTO_noLiquidacionesBases> DAL_noLiquidacionesBases_Get(int numDoc)
        {

            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   "   SELECT [EmpresaID]  "   +
                                                "   ,[NumeroDoc] "   +
                                                "   ,[ConceptoNOID] "   +
                                                "   ,[ConceptoBase] "   +
                                                "   ,[Dias] "   +
                                                "   ,[Valor] "   +
                                                "   FROM noLiquidacionesBases " +
                                                "   WHERE noLiquidacionesPreliminar.NumeroDoc =  @NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;

                List<DTO_noLiquidacionesBases> result = new List<DTO_noLiquidacionesBases>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noLiquidacionesBases dto = new DTO_noLiquidacionesBases(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesBases_Get");
                throw exception;
            }

        }
               
        /// <summary>
        /// Elimina los registros de la tabla de Bases
        /// </summary>
        /// <param name="numDoc">numero de documento</param>
        public void DAL_noLiquidacionesBases_Delete(int numDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "     delete from noLiquidacionesBases   " +
                                               "    where NumeroDoc = @NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesBases_Delete");
                throw exception;
            }
       
        }
    }
}
