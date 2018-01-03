using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_coPlanillaConsolidacion : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coPlanillaConsolidacion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae la lista de empresas a consilidar
        /// </summary>
        /// <param name="compID">Identificador del comprobante</param>
        /// <param name="periodo">Periodo de consulta del auxiliar</param>
        /// <returns>Retorna la lista de empresas a consilidar</returns>
        public List<DTO_ComprobanteConsolidacion> DAL_coPlanillaConsolidacion_GetEmpresas(string compID, DateTime periodo)
        {
            try
            {
                List<DTO_ComprobanteConsolidacion> results = new List<DTO_ComprobanteConsolidacion>();

                #region Query
                
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@ActivoInd"].Value = true;

                mySqlCommandSel.CommandText =
                    "select * from coPlanillaConsolidacion with(nolock) " +
                    "where EmpresaGrupoID=@EmpresaGrupoID and eg_coCentroCosto=@eg_coCentroCosto and ActivoInd=@ActivoInd";

                #endregion
                #region Trae la lista de empresas
                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ComprobanteConsolidacion r = new DTO_ComprobanteConsolidacion();
                    r.EmpresaID.Value = dr["EmpresaID"].ToString();
                    r.CentroCostoID.Value = dr["CentroCostoID"].ToString();

                    results.Add(r);
                }

                dr.Close();
                #endregion
                #region Revisa si ya fueron consolidadas

                mySqlCommandSel.Parameters.Clear();

                mySqlCommandSel.CommandText =
                    "select COUNT(*) from coAuxiliar with(nolock) " +
                    "where EmpresaID=@EmpresaID and PeriodoID=@PeriodoID and " +
                    "	ComprobanteID=@ComprobanteID and CentroCostoID=@CentroCostoID";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = compID;

                foreach (DTO_ComprobanteConsolidacion r in results)
                {
                    mySqlCommandSel.Parameters["@CentroCostoID"].Value = r.CentroCostoID.Value;
                    int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                    if (count > 0)
                        r.Procesado = true;
                }
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coPlanillaConsolidacion_GetEmpresas");
                throw exception;
            }
        }
        
        #endregion
  }
}
