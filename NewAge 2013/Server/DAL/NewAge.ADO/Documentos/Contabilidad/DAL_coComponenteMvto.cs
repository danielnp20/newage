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
    /// Operaciones sobre la tabla de los movimientos de un componente
    /// </summary>
    public class DAL_coComponenteMvto : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_coComponenteMvto(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId) : base(c, tx, empresa, userId) { }

        /// <summary>
        /// Agrega un nuevo registro
        /// </summary>
        /// <param name="comp">Componente de movimiento</param>
        public void DAL_coComponenteMvto_Add(DTO_coComponenteMvto comp)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompLocal1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal9", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal13", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal14", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal15", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal16", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal17", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal18", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal19", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompLocal20", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra9", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra13", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra14", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra15", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra16", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra17", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra18", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra19", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompExtra20", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = comp.NumeroDoc.Value.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = comp.BalanceTipoID.Value;
                mySqlCommandSel.Parameters["@ModuloID"].Value = comp.ModuloID.Value;
                mySqlCommandSel.Parameters["@IdentificadorTR"].Value = comp.IdentificadorTR.Value;
                mySqlCommandSel.Parameters["@CompLocal1"].Value = comp.CompLocal1.Value;
                mySqlCommandSel.Parameters["@CompLocal2"].Value = comp.CompLocal2.Value;
                mySqlCommandSel.Parameters["@CompLocal3"].Value = comp.CompLocal3.Value;
                mySqlCommandSel.Parameters["@CompLocal4"].Value = comp.CompLocal4.Value;
                mySqlCommandSel.Parameters["@CompLocal5"].Value = comp.CompLocal5.Value;
                mySqlCommandSel.Parameters["@CompLocal6"].Value = comp.CompLocal6.Value;
                mySqlCommandSel.Parameters["@CompLocal7"].Value = comp.CompLocal7.Value;
                mySqlCommandSel.Parameters["@CompLocal8"].Value = comp.CompLocal8.Value;
                mySqlCommandSel.Parameters["@CompLocal9"].Value = comp.CompLocal9.Value;
                mySqlCommandSel.Parameters["@CompLocal10"].Value = comp.CompLocal10.Value;
                mySqlCommandSel.Parameters["@CompLocal11"].Value = comp.CompLocal11.Value;
                mySqlCommandSel.Parameters["@CompLocal12"].Value = comp.CompLocal12.Value;
                mySqlCommandSel.Parameters["@CompLocal13"].Value = comp.CompLocal13.Value;
                mySqlCommandSel.Parameters["@CompLocal14"].Value = comp.CompLocal14.Value;
                mySqlCommandSel.Parameters["@CompLocal15"].Value = comp.CompLocal15.Value;
                mySqlCommandSel.Parameters["@CompLocal16"].Value = comp.CompLocal16.Value;
                mySqlCommandSel.Parameters["@CompLocal17"].Value = comp.CompLocal17.Value;
                mySqlCommandSel.Parameters["@CompLocal18"].Value = comp.CompLocal18.Value;
                mySqlCommandSel.Parameters["@CompLocal19"].Value = comp.CompLocal19.Value;
                mySqlCommandSel.Parameters["@CompLocal20"].Value = comp.CompLocal20.Value;
                mySqlCommandSel.Parameters["@CompExtra1"].Value = comp.CompExtra1.Value;
                mySqlCommandSel.Parameters["@CompExtra2"].Value = comp.CompExtra2.Value;
                mySqlCommandSel.Parameters["@CompExtra3"].Value = comp.CompExtra3.Value;
                mySqlCommandSel.Parameters["@CompExtra4"].Value = comp.CompExtra4.Value;
                mySqlCommandSel.Parameters["@CompExtra5"].Value = comp.CompExtra5.Value;
                mySqlCommandSel.Parameters["@CompExtra6"].Value = comp.CompExtra6.Value;
                mySqlCommandSel.Parameters["@CompExtra7"].Value = comp.CompExtra7.Value;
                mySqlCommandSel.Parameters["@CompExtra8"].Value = comp.CompExtra8.Value;
                mySqlCommandSel.Parameters["@CompExtra9"].Value = comp.CompExtra9.Value;
                mySqlCommandSel.Parameters["@CompExtra10"].Value = comp.CompExtra10.Value;
                mySqlCommandSel.Parameters["@CompExtra11"].Value = comp.CompExtra11.Value;
                mySqlCommandSel.Parameters["@CompExtra12"].Value = comp.CompExtra12.Value;
                mySqlCommandSel.Parameters["@CompExtra13"].Value = comp.CompExtra13.Value;
                mySqlCommandSel.Parameters["@CompExtra14"].Value = comp.CompExtra14.Value;
                mySqlCommandSel.Parameters["@CompExtra15"].Value = comp.CompExtra15.Value;
                mySqlCommandSel.Parameters["@CompExtra16"].Value = comp.CompExtra16.Value;
                mySqlCommandSel.Parameters["@CompExtra17"].Value = comp.CompExtra17.Value;
                mySqlCommandSel.Parameters["@CompExtra18"].Value = comp.CompExtra18.Value;
                mySqlCommandSel.Parameters["@CompExtra19"].Value = comp.CompExtra19.Value;
                mySqlCommandSel.Parameters["@CompExtra20"].Value = comp.CompExtra20.Value;
                mySqlCommandSel.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO coComponenteMvto(" +
                    "	NumeroDoc,EmpresaID,BalanceTipoID,ModuloID,IdentificadorTR," +
                    "   CompLocal1,CompLocal2,CompLocal3,CompLocal4,CompLocal5,CompLocal6,CompLocal7,CompLocal8,CompLocal9,CompLocal10," +
                    "   CompLocal11,CompLocal12,CompLocal13,CompLocal14,CompLocal15,CompLocal16,CompLocal17,CompLocal18,CompLocal19,CompLocal20," +
                    "   CompExtra1,CompExtra2,CompExtra3,CompExtra4,CompExtra5,CompExtra6,CompExtra7,CompExtra8,CompExtra9,CompExtra10," +
                    "   CompExtra11,CompExtra12,CompExtra13,CompExtra14,CompExtra15,CompExtra16,CompExtra17,CompExtra18,CompExtra19,CompExtra20," +
                    "   eg_coBalanceTipo" +
                    ")VALUES(" +
                    "	@NumeroDoc,@EmpresaID,@BalanceTipoID,@ModuloID,@IdentificadorTR," +
                    "   @CompLocal1,@CompLocal2,@CompLocal3,@CompLocal4,@CompLocal5,@CompLocal6,@CompLocal7,@CompLocal8,@CompLocal9,@CompLocal10," +
                    "   @CompLocal11,@CompLocal12,@CompLocal13,@CompLocal14,@CompLocal15,@CompLocal16,@CompLocal17,@CompLocal18,@CompLocal19,@CompLocal20," +
                    "   @CompExtra1,@CompExtra2,@CompExtra3,@CompExtra4,@CompExtra5,@CompExtra6,@CompExtra7,@CompExtra8,@CompExtra9,@CompExtra10," +
                    "   @CompExtra11,@CompExtra12,@CompExtra13,@CompExtra14,@CompExtra15,@CompExtra16,@CompExtra17,@CompExtra18,@CompExtra19,@CompExtra20," +
                    "   @eg_coBalanceTipo" +
                    ")";
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null)
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_coImpDeclaracionDocu_Add", false);
                throw exception;
            }
        }

        /// <summary>
        /// Elimina todos los movimientos de un comprobante
        /// </summary>
        /// <param name="numeroDoc">Identificador de la consulta</param>
        public void DAL_coComponenteMvto_Delete(int numeroDoc)
        {

            SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
            mySqlCommandSel.Transaction = base.MySqlConnectionTx;

            try
            {
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommandSel.CommandText = "delete from coComponenteMvto where EmpresaID=@EmpresaID and NumeroDoc=@NumeroDoc";
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_coComponenteMvto_HasData", false);
                throw exception;
            }
        }

    }
}
