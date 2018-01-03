using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Configuration;
using System.Reflection;
using SentenceTransformer;
using NewAge.DTO.Reportes;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_ReportesInventarios
    /// </summary>
    public class DAL_ReportesInventarios : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesInventarios(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Saldos

        /// <summary>
        /// Funcion que carga una lista de DTO para el report de saldos, Kardex y Serial
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes por el cual se va a filtrar</param>
        /// <param name="bodega">Bodega por la cual se desea filtrar</param>
        /// <param name="tipoBodega">Tipo de Bodega por e l cual se desea filtrar</param>
        /// <param name="referencia">Referencia por el cual se desea filtrar</param>
        /// <param name="grupo">Grupo por el cual se desea filtrar</param>
        /// <param name="clase">Clase por el cual se desea filtrar</param>
        /// <param name="tipo">Tipo por el cual se desea filtrar</param>
        /// <param name="serie">Serie por el cual se desea filtrar</param>
        /// <param name="material">Material por el cual se desea filtrar</param>
        /// <param name="isSerial">Parametro para imprimir reporte de Serial</param>
        /// <returns>List DTO</returns>
        public List<DTO_ReportInventariosSaldos> DAL_ReportesInventarios_Saldos(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro=null)
        {
            try
            {
                List<DTO_ReportInventariosSaldos> result = new List<DTO_ReportInventariosSaldos>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros
              
                string bod = "", refe = "", tipoBode = "", grup= "", clas = "", tip ="", seri ="", materi = "", serial ="";

                //Verifica los filtros para aplicarlos a la consulta
                if (!string.IsNullOrEmpty(bodega))
                    bod = " AND saldo.BodegaID = " + "'" + bodega + "'";
                if (!string.IsNullOrEmpty(referencia))
                    refe = "  AND costo.inReferenciaID = " + "'" + referencia + "'";
                if (!string.IsNullOrEmpty(tipoBodega))
                    tipoBode = " AND Bodetipo.BodegaTipoID =  " + "'" + tipoBodega + "'";
                if (!string.IsNullOrEmpty(grupo))
                    grup = " AND grupo.Descriptivo = " + "'" + grupo + "'";
                if (!string.IsNullOrEmpty(clase))
                    clas = " AND clase.Descriptivo = " + "'" + clase + "'";
                if (!string.IsNullOrEmpty(tipo))
                    tip = " AND tipo.Descriptivo = " + "'" + tipo + "'";
                if (!string.IsNullOrEmpty(serie))
                    seri = " AND serie.Descriptivo = " + "'" + serie + "'";
                if (!string.IsNullOrEmpty(material))
                    materi = " AND mate.Descriptivo =  " + " ' " + material + "'";
                if (isSerial == true)
                    serial = " AND tipo.SerializadoInd = 1 ";

                #endregion
                #region CommandText


                mySqlCommandSel.CommandText =
                    "  SELECT costo.Periodo , saldo.BodegaID,  bod.Descriptivo AS BodegaDes,costo.inReferenciaID AS Referencia,  " +
                    "   refe.Descriptivo AS Producto, saldo.CantInicial AS Inicial, saldo.CantEntrada AS Entrada, saldo.CantRetiro AS Salidas,  " +
                    "   (saldo.CantInicial + saldo.CantEntrada - saldo.CantRetiro) AS CantidadLoc,  " +
                    "   (costo.CtoLocSaldoIni + costo.CtoLocEntrada - costo.CtoLocSalida) AS ValorLocal,  " +
                    "   Cast((case when costo.CantInicial + costo.CantEntrada - costo.CantRetiro = 0 then 0 else    " +
                    "   (costo.CtoLocSaldoIni + costo.CtoLocEntrada - costo.CtoLocSalida) / (costo.CantInicial + costo.CantEntrada - costo.CantRetiro) end) as money) as VlrUnidadLoc,    " +
                    "   (costo.CtoExtSaldoIni + costo.CtoExtEntrada - costo.CtoExtSalida) AS ValorExt,  " +
                    "   CASE WHEN(costo.CantInicial + costo.CantEntrada - costo.CantRetiro) <> 0 THEN  " +
                    "   (costo.CtoExtSaldoIni + costo.CtoExtEntrada - costo.CtoExtSalida) / (costo.CantInicial + costo.CantEntrada - costo.CantRetiro)  ELSE 0 END AS VlrUnidadExt  " +
                    "   FROM inSaldosExistencias saldo  " +
                    "   left join inBodega bod    on saldo.BodegaID = bod.BodegaID and saldo.eg_inBodega = bod.EmpresaGrupoID  " +
                    "   left join inCostosExistencias costo on saldo.periodo = costo.Periodo and  " +
                    " costo.CosteoGrupoInvID = bod.CosteoGrupoInvID and costo.eg_inCosteoGrupo = bod.eg_inCosteoGrupo and  " +
                    " saldo.inReferenciaID = costo.inReferenciaID and costo.eg_inReferencia = saldo.eg_inReferencia  " +
                    "   LEFT JOIN inReferencia refe WITH(NOLOCK) ON refe.inReferenciaID = saldo.inReferenciaID  " +
                    "   where Year(saldo.Periodo) = @año and Month(saldo.Periodo) = @MesIni  and (saldo.CantInicial + saldo.CantEntrada - saldo.CantRetiro)  > 0 " + bod + refe +
                    " order by refe.Descriptivo   ";

                    if (Libro == "IFRS")
                {
                    mySqlCommandSel.CommandText =
                        "  SELECT costo.Periodo , saldo.BodegaID,  bod.Descriptivo AS BodegaDes,costo.inReferenciaID AS Referencia,  " +
                        "   refe.Descriptivo AS Producto, saldo.CantInicial AS Inicial, saldo.CantEntrada AS Entrada, saldo.CantRetiro AS Salidas,  " +
                        "   (saldo.CantInicial + saldo.CantEntrada - saldo.CantRetiro) AS CantidadLoc,  " +
                        "   (costo.CtoLocSaldoIniIFRS + costo.CtoLocEntradaIFRS - costo.CtoLocSalidaIFRS) AS ValorLocal,  " +
                        "   Cast((case when costo.CantInicial + costo.CantEntrada - costo.CantRetiro = 0 then 0 else    " +
                        "   (costo.CtoLocSaldoIni + costo.CtoLocEntrada - costo.CtoLocSalida) / (costo.CantInicial + costo.CantEntrada - costo.CantRetiro) end) as money) as VlrUnidadLoc,    " +
                        "   (costo.CtoExtSaldoIniIFRS + costo.CtoExtEntradaIFRS - costo.CtoExtSalidaIFRS) AS ValorExt,  " +
                        "   CASE WHEN(costo.CantInicial + costo.CantEntrada - costo.CantRetiro) <> 0 THEN  " +
                        "   (costo.CtoExtSaldoIni + costo.CtoExtEntrada - costo.CtoExtSalida) / (costo.CantInicial + costo.CantEntrada - costo.CantRetiro)  ELSE 0 END AS VlrUnidadExt  " +
                        "   FROM inSaldosExistencias saldo  " +
                        "   left join inBodega bod    on saldo.BodegaID = bod.BodegaID and saldo.eg_inBodega = bod.EmpresaGrupoID  " +
                        "   left join inCostosExistencias costo on saldo.periodo = costo.Periodo and  " +
                        " costo.CosteoGrupoInvID = bod.CosteoGrupoInvID and costo.eg_inCosteoGrupo = bod.eg_inCosteoGrupo and  " +
                        " saldo.inReferenciaID = costo.inReferenciaID and costo.eg_inReferencia = saldo.eg_inReferencia  " +
                        "   LEFT JOIN inReferencia refe WITH(NOLOCK) ON refe.inReferenciaID = saldo.inReferenciaID  " +
                        "   where Year(saldo.Periodo) = @año and Month(saldo.Periodo) = @MesIni and (saldo.CantInicial + saldo.CantEntrada - saldo.CantRetiro)  > 0   " + bod + refe +
                        " order by refe.Descriptivo   ";
                }
                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@MesIni", SqlDbType.Int);

                #endregion
                #region Valores Parametros

                mySqlCommandSel.Parameters["@año"].Value = año;
                mySqlCommandSel.Parameters["@MesIni"].Value = mesIni;

                #endregion

                DTO_ReportInventariosSaldos aux = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    aux = new DTO_ReportInventariosSaldos(dr);
                    result.Add(aux);
                }               
                dr.Close();

                result = result.FindAll(x => x.CantidadLoc.Value > 0).OrderBy(x=>x.Producto.Value).ToList();
                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesInventarios_Saldos");
                throw exception;
            }
        }

        #endregion

        #region Excel

        /// <summary>
        /// Obtiene un datatable con la info de Inventarios segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="mesIni">Fecha Inicial</param>
        /// <param name="mesFin">Fecha Final</param>
        /// <param name="bodega">bodega</param>
        /// <param name="tipoBodega">tipoBodega</param>
        /// <param name="grupo">Grupo</param>
        /// <param name="clase">tipoBodega</param>
        /// <param name="Tipo">Tipo</param>
        /// <param name="serie">serie</param>
        /// <param name="material">material</param>
        /// <param name="isSerial">isSerial</param>
        /// <param name="otroFilter">otroFilter</param>
        /// <param name="agrup">agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable DAL_Reportes_In_InventarioToExcel(int documentoID, DateTime? mesIni, DateTime? mesFin, string bodega, string tipoBodega, string referencia, string grupo, string clase, string tipo,
                                                       string serie, string material, bool isSerial, string libro, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable table = new DataTable();
                string where = string.Empty;

                #region Saldos
                if (documentoID == AppReports.inSaldos)
                {
                    #region Filtros

                    string bod = "", refe = "", tipoBode = "", grup = "", clas = "", tip = "", seri = "", materi = "", serial = "";

                    //Verifica los filtros para aplicarlos a la consulta
                    if (!string.IsNullOrEmpty(bodega))
                        bod = " AND saldo.BodegaID = " + "'" + bodega + "'";
                    if (!string.IsNullOrEmpty(referencia))
                        refe = "  AND costo.inReferenciaID = " + "'" + referencia + "'";
                    if (!string.IsNullOrEmpty(tipoBodega))
                        tipoBode = " AND Bodetipo.BodegaTipoID =  " + "'" + tipoBodega + "'";
                    if (!string.IsNullOrEmpty(grupo))
                        grup = " AND grupo.Descriptivo = " + "'" + grupo + "'";
                    if (!string.IsNullOrEmpty(clase))
                        clas = " AND clase.Descriptivo = " + "'" + clase + "'";
                    if (!string.IsNullOrEmpty(tipo))
                        tip = " AND tipo.Descriptivo = " + "'" + tipo + "'";
                    if (!string.IsNullOrEmpty(serie))
                        seri = " AND serie.Descriptivo = " + "'" + serie + "'";
                    if (!string.IsNullOrEmpty(material))
                        materi = " AND mate.Descriptivo =  " + " ' " + material + "'";
                    if (isSerial == true)
                        serial = " AND tipo.SerializadoInd = 1 ";

                    #endregion
                    #region CommandText

                    mySqlCommandSel.CommandText =
                        "  SELECT costo.Periodo , saldo.BodegaID,  bod.Descriptivo AS BodegaDes,costo.inReferenciaID AS Referencia,  " +
                        "   refe.Descriptivo AS ReferenciaDesc, saldo.CantInicial AS Inicial, saldo.CantEntrada AS Entrada, saldo.CantRetiro AS Salidas,  " +
                        "   (saldo.CantInicial + saldo.CantEntrada - saldo.CantRetiro) AS CantidadLoc,  " +
                        "   (costo.CtoLocSaldoIni + costo.CtoLocEntrada - costo.CtoLocSalida) AS ValorLocal,  " +
                        "   Cast((case when costo.CantInicial + costo.CantEntrada - costo.CantRetiro = 0 then 0 else    " +
                        "   (costo.CtoLocSaldoIni + costo.CtoLocEntrada - costo.CtoLocSalida) / (costo.CantInicial + costo.CantEntrada - costo.CantRetiro) end) as money) as VlrUnidadLoc,    " +
                        "   (costo.CtoExtSaldoIni + costo.CtoExtEntrada - costo.CtoExtSalida) AS ValorExt,  " +
                        "   CASE WHEN(costo.CantInicial + costo.CantEntrada - costo.CantRetiro) <> 0 THEN  " +
                        "   (costo.CtoExtSaldoIni + costo.CtoExtEntrada - costo.CtoExtSalida) / (costo.CantInicial + costo.CantEntrada - costo.CantRetiro)  ELSE 0 END AS VlrUnidadExt  " +
                        "   FROM inSaldosExistencias saldo  " +
                        "   left join inBodega bod    on saldo.BodegaID = bod.BodegaID and saldo.eg_inBodega = bod.EmpresaGrupoID  " +
                        "   left join inCostosExistencias costo on saldo.periodo = costo.Periodo and  " +
                        " costo.CosteoGrupoInvID = bod.CosteoGrupoInvID and costo.eg_inCosteoGrupo = bod.eg_inCosteoGrupo and  " +
                        " saldo.inReferenciaID = costo.inReferenciaID and costo.eg_inReferencia = saldo.eg_inReferencia  " +
                        "   LEFT JOIN inReferencia refe WITH(NOLOCK) ON refe.inReferenciaID = saldo.inReferenciaID  " +
                        "   where Year(saldo.Periodo) = @año and Month(saldo.Periodo) = @MesIni and (saldo.CantInicial + saldo.CantEntrada - saldo.CantRetiro)  > 0 " + bod + refe +
                        " order by refe.Descriptivo   ";
                    if (libro == "IFRS")
                    {
                        mySqlCommandSel.CommandText =
                            "  SELECT costo.Periodo , saldo.BodegaID,  bod.Descriptivo AS BodegaDes,costo.inReferenciaID AS Referencia,  " +
                            "   refe.Descriptivo AS Producto, saldo.CantInicial AS Inicial, saldo.CantEntrada AS Entrada, saldo.CantRetiro AS Salidas,  " +
                            "   (saldo.CantInicial + saldo.CantEntrada - saldo.CantRetiro) AS CantidadLoc,  " +
                            "   (costo.CtoLocSaldoIniIFRS + costo.CtoLocEntradaIFRS - costo.CtoLocSalidaIFRS) AS ValorLocal,  " +
                            "   Cast((case when costo.CantInicial + costo.CantEntrada - costo.CantRetiro = 0 then 0 else    " +
                            "   (costo.CtoLocSaldoIni + costo.CtoLocEntrada - costo.CtoLocSalida) / (costo.CantInicial + costo.CantEntrada - costo.CantRetiro) end) as money) as VlrUnidadLoc,    " +
                            "   (costo.CtoExtSaldoIniIFRS + costo.CtoExtEntradaIFRS - costo.CtoExtSalidaIFRS) AS ValorExt,  " +
                            "   CASE WHEN(costo.CantInicial + costo.CantEntrada - costo.CantRetiro) <> 0 THEN  " +
                            "   (costo.CtoExtSaldoIni + costo.CtoExtEntrada - costo.CtoExtSalida) / (costo.CantInicial + costo.CantEntrada - costo.CantRetiro)  ELSE 0 END AS VlrUnidadExt  " +
                            "   FROM inSaldosExistencias saldo  " +
                            "   left join inBodega bod    on saldo.BodegaID = bod.BodegaID and saldo.eg_inBodega = bod.EmpresaGrupoID  " +
                            "   left join inCostosExistencias costo on saldo.periodo = costo.Periodo and  " +
                            " costo.CosteoGrupoInvID = bod.CosteoGrupoInvID and costo.eg_inCosteoGrupo = bod.eg_inCosteoGrupo and  " +
                            " saldo.inReferenciaID = costo.inReferenciaID and costo.eg_inReferencia = saldo.eg_inReferencia  " +
                            "   LEFT JOIN inReferencia refe WITH(NOLOCK) ON refe.inReferenciaID = saldo.inReferenciaID  " +
                            "   where Year(saldo.Periodo) = @año and Month(saldo.Periodo) = @MesIni and (saldo.CantInicial + saldo.CantEntrada - saldo.CantRetiro)  > 0 " + bod + refe +
                            " order by refe.Descriptivo   ";
                    }
                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength); 
                    mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@MesIni", SqlDbType.Int);
                    #endregion                    
                    #region Asignacion de valores a parametros

                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@MesIni"].Value = mesIni.Value.Month;
                    mySqlCommandSel.Parameters["@Año"].Value = mesIni.Value.Year;
                    #endregion

                }
                #endregion                
                sda.SelectCommand = mySqlCommandSel;
                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                if (!string.IsNullOrEmpty(mySqlCommandSel.CommandText))
                    sda.Fill(table);

                //Filtra los registros
                DataRow[] foundRows = table.Select("CantidadLoc > 0", "ReferenciaDesc asc");
                DataTable result = table.Clone();
                foreach (DataRow r in foundRows)
                    result.ImportRow(r);

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Reportes_In_InventarioToExcel");
                return null;
            }
        }

        #endregion
    }
}


