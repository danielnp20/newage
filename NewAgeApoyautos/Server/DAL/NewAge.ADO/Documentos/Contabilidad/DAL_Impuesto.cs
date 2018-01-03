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
    /// Dal para operaciones de impuestos
    /// </summary>
    public class DAL_Impuesto : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_Impuesto(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trea la lista de declaraciones de impuestos de un periodo
        /// </summary>
        /// <param name="year">Año de busqueda</param>
        /// <param name="month">Mes de busqueda</param>
        /// <returns>Retorna las declaraciones de un periodo</returns>
        public List<DTO_DeclaracionImpuesto> DAL_Impuesto_GetDeclaracionesByPeriodo(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {                
                List<DTO_DeclaracionImpuesto> result = new List<DTO_DeclaracionImpuesto>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select imp.ImpuestoDeclID, imp.Descriptivo, cal.AñoFiscal, cal.Periodo, cal.Fecha, cal.NumeroDoc " +
                    "from coImpuestoDeclaracion imp with(nolock) " +
                    "	inner join coImpDeclaracionCalendario cal with(nolock) on imp.ImpuestoDeclID = cal.ImpuestoDeclID " +
                    "		and imp.EmpresaGrupoID = cal.eg_coImpuestoDeclaracion " +
                    "where imp.EmpresaGrupoID = @EmpresaGrupoID " +
                    "	and (cal.Fecha between @FechaIni and @FechaFin) ";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);

                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coImpuestoDeclaracion, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommand.Parameters["@FechaFin"].Value = fechaFin;

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_DeclaracionImpuesto dto = new DTO_DeclaracionImpuesto(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Impuesto_GetDeclaraciones");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de cuentas de un renglon
        /// </summary>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="renglon">Identificador del renglon</param>
        /// <param name="fechaIni">Fecha inicial de busqueda</param>
        /// <param name="fechaFin">Fecha final de busqueda</param>
        /// <returns>Retorna la lista de cuentas</returns>
        public List<DTO_DetalleRenglon> DAL_Impuesto_GetCuentasByRenglon(DTO_coImpuestoDeclaracion imp, DTO_coImpDeclaracionRenglon renglon, List<string> ctas, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<DTO_DetalleRenglon> result = new List<DTO_DetalleRenglon>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros generales

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@InfExogenaInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommand.Parameters["@FechaFin"].Value = fechaFin;
                mySqlCommand.Parameters["@InfExogenaInd"].Value = true;

                #endregion
                #region Query de las cuentas
                int index = 0;
                string queryCtas = string.Empty;
                string queryCostoInd = renglon.CostosInd.Value.Value ? string.Empty : " and aux.DatoAdd1 = " + AuxiliarDatoAdd1.IVA.ToString() + " ";
                if (ctas.Count > 0)
                {
                    queryCtas = " AND (";
                    foreach (string cta in ctas)
                    {
                        if (index > 0)
                            queryCtas += " OR ";

                        queryCtas += "(aux.CuentaID = '" + cta.Trim() + "'" + " and aux.DatoAdd2 = '" + renglon.Tarifa.Value.Value.ToString() + "'" + queryCostoInd + ")";
                        index++;
                    }
                    queryCtas += ")";
                }

                #endregion
                #region Carga la informacion del lugar geografico
                string queryLG = string.Empty;
                if (imp.MunicipalInd.Value.Value)
                    queryLG = " AND LugarGeograficoID='" + imp.LugarGeograficoID.Value + "'";
                #endregion
                #region Consulta
                mySqlCommand.CommandText =
                    "select aux.CuentaID, aux.comprobanteID, aux.ComprobanteNro, aux.PrefijoCOM, " +
                    "	aux.DocumentoCOM, aux.Fecha, aux.TerceroID, t.Descriptivo as Nombre, aux.VlrBaseML, aux.VlrMdaLoc " +
                    "from coauxiliar aux with(nolock)" +
                    "	inner Join coTercero t with(nolock) on aux.TerceroID = t.TerceroID " +
                    "	left join coPlanCuenta coCta with(nolock) on aux.CuentaID = coCta.CuentaID " +
                    "	left join coComprobante comp with(nolock) on aux.ComprobanteID = comp.ComprobanteID " +
                    "where EmpresaID = @EmpresaID and aux.fecha between @fechaIni and @fechaFin " + queryCtas + queryLG +
                    "   AND (coCta.NITCierreAnual is null or aux.TerceroID <> coCta.NITCierreAnual) " +
                    "   AND comp.InfExogenaInd = @InfExogenaInd " + 
                    "Order By CuentaID";
                #endregion
                #region Realiza la consulta
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                try
                {
                    while (dr.Read())
                    {
                        DTO_DetalleRenglon dto = new DTO_DetalleRenglon(dr);
                        dto.ImpuestoDeclID.Value = imp.ID.Value;
                        dto.Renglon.Value = renglon.Renglon.Value;
                        result.Add(dto);
                    }
                }
                finally
                {
                    dr.Close();
                }

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Impuesto_GetCuentasByRenglon");
                throw exception;
            }
        }
    
    }
}
