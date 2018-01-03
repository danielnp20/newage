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
    public class DAL_coImpuestoLocal : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coImpuestoLocal(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="actEconomicaID">Identificador de la actividad economica</param>
        /// <param name="regFisTerceroID">Identificador del regimen fiscal del tercero</param>
        /// <param name="lugarGeoID">Identificador del lugar geografico</param>
        /// <param name="regFiscEmpID">Regimen fiscal de la empresa</param>
        /// <param name="impLocales">Lista tipos de impuestos nacionales</param>
        /// <returns>Retorna una lista de tuplas <Cuenta,TipoImpuestoLocal> </returns>
        public List<string> DAL_coImpuestoLocal_GetCuentasByPK(DTO_coTercero tercero, string actEconomicaID, string lugarGeoID, string regFiscEmpID, string reteIVA, string reteFte)
        {
            try
            {
                List<string> results = new List<string>();

                #region Query
                
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@RegimenFiscalEmpresaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RegimenFiscalTerceroID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActEconomicaID", SqlDbType.Char, UDT_ActEconomicaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coRegimenFiscal", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coImpuestoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coActEconomica", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@RegimenFiscalEmpresaID"].Value = regFiscEmpID;
                mySqlCommandSel.Parameters["@RegimenFiscalTerceroID"].Value = tercero.ReferenciaID.Value;
                mySqlCommandSel.Parameters["@ActEconomicaID"].Value = actEconomicaID;
                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coImpuestoLocal, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coRegimenFiscal"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coRegimenFiscal, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coImpuestoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coImpuestoTipo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coActEconomica"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coActEconomica, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                //Query basico
                mySqlCommandSel.CommandText =
                    "select distinct CuentaID " +
                    "from coImpuestoLocal with(nolock) " +
                    "where " +
                    "   RegimenFiscalEmpresaID = @RegimenFiscalEmpresaID  and RegimenFiscalTerceroID = @RegimenFiscalTerceroID " +
                    "   and ActEconomicaID = @ActEconomicaID and ActivoInd = 1";

                //No tiene en cuenta la reteIVA
                if (tercero.AutoRetIVAInd.Value.Value)
                {
                    mySqlCommandSel.Parameters.Add("@ImpuestoTipoID0", SqlDbType.Char, UDT_ImpuestoTipoID.MaxLength);
                    mySqlCommandSel.Parameters["@ImpuestoTipoID0"].Value = reteIVA;
                    mySqlCommandSel.CommandText += "    and ImpuestoTipoID <> @ImpuestoTipoID0";
                }

                //No tiene en cuenta la reteFte
                if (tercero.AutoRetRentaInd.Value.Value)
                {
                    mySqlCommandSel.Parameters.Add("@ImpuestoTipoID1", SqlDbType.Char, UDT_ImpuestoTipoID.MaxLength);
                    mySqlCommandSel.Parameters["@ImpuestoTipoID1"].Value = reteFte;
                    mySqlCommandSel.CommandText += "    and ImpuestoTipoID <> @ImpuestoTipoID1";
                }

                //Grupos de empresas
                mySqlCommandSel.CommandText += 
                    "   and EmpresaGrupoID = @EmpresaGrupoID and eg_coRegimenFiscal = @eg_coRegimenFiscal" +
                    "   and eg_coImpuestoTipo = @eg_coImpuestoTipo and eg_coActEconomica = @eg_coActEconomica and eg_coPlanCuenta = @eg_coPlanCuenta";

                #endregion
                #region Revisa si le asigna el lugar geografico a la consulta
                if (!string.IsNullOrEmpty(lugarGeoID))
                {
                    mySqlCommandSel.Parameters.Add("@LugarGeograficoID", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                    mySqlCommandSel.Parameters["@LugarGeograficoID"].Value = lugarGeoID;
                    mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);

                    mySqlCommandSel.CommandText += " and LugarGeograficoID = @LugarGeograficoID and eg_glLugarGeografico = @eg_glLugarGeografico ";
                }
                #endregion
                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();

                while (dr.Read())
                    results.Add(dr["CuentaID"].ToString());

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coImpuestoLocal_GetCuentasByPK");
                throw exception;
            }
        }
        
        #endregion
    }
}
