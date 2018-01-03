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
    /// DAL de DAL_ReportesGlobal
    /// </summary>
    public class DAL_ReportesGlobal : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesGlobal(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Documentos Pendientes

        /// <summary>
        /// Funcion que se encarga de traer los Documentos Pendientes
        /// </summary>
        /// <param name="Periodo">Periodo a consultar los documentos Pendientes </param>
        /// <param name="modulo">Filtrar un modulo especifico</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportDocumentoPendientes> DAL_ReportesGlobal_DocumentosPendiente(DateTime Periodo,byte tipoReport, string modulo,string documentoID)
        {
            try
            {
                List<DTO_ReportDocumentoPendientes> result = new List<DTO_ReportDocumentoPendientes>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                #region Filtros

                string mod = !string.IsNullOrEmpty(modulo) ? " and doc.ModuloID = @Modulo " : string.Empty;
                string docID = !string.IsNullOrEmpty(documentoID) ? " and doc.DocumentoID = @DocumentoID " : string.Empty;
                string estado = tipoReport == 1 ? " and ctrl.Estado in (1,2,5,6) " : " and (ctrl.Estado <> 1 and ctrl.Estado <> 2 and ctrl.Estado <> 5 and ctrl.Estado <> 6) ";

                #endregion
                #region CommandText

                mySqlCommandSel.CommandText =
                    " select NumeroDoc, ctrl.DocumentoID,  doc.Descriptivo DocumentoDesc, PeriodoDoc,  " +
                            " case when(saldo.coSaldoControl = 2) then RTRIM(CAST(ctrl.PrefijoID AS CHAR(10)))+ ' ' +'-' + ' ' + CAST(ctrl.DocumentoNro AS CHAR(10)) " +
                                     " when(saldo.coSaldoControl = 3) then CAST(ctrl.DocumentoTercero AS CHAR(10)) end Documento, " +
                            " ctrl.TerceroID, ter.Descriptivo as TerceroDesc, RTRIM(CAST(ComprobanteID AS CHAR(10)))+ ' ' +'-' + ' ' + CAST(ComprobanteIDNro AS CHAR(10))Comprobante, " +
                            " Descripcion, ctrl.CuentaID,  DocumentoTercero, Valor, DocumentoPadre, Estado, doc.ModuloID " +
                    " from gldocumentocontrol ctrl with(nolock) " +
                         " left join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID  " +
                         " left join coPlanCuenta cta with(nolock) on cta.CuentaID = ctrl.CuentaID and cta.EmpresaGrupoID = ctrl.eg_coPlanCuenta " +
                         " left join glConceptoSaldo saldo   with(nolock) on saldo.ConceptoSaldoID = cta.ConceptoSaldoID and saldo.EmpresaGrupoID = cta.eg_glConceptoSaldo " +
                         " left join coTercero ter with(nolock) on ter.TerceroID = ctrl.TerceroID and ter.EmpresaGrupoID = ctrl.eg_coTercero " +
                    " where ctrl.EmpresaID = @EmpresaID  " +
                        " and DATEPART(YEAR, ctrl.PeriodoDoc) = @Year  " +
                        " and DATEPART(MONTH, ctrl.PeriodoDoc)= @Month " + mod + docID + estado;


                #endregion
                #region Paramentros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Modulo", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);

                #endregion
                #region Asignacion Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Year"].Value = Periodo.Year;
                mySqlCommandSel.Parameters["@Month"].Value = Periodo.Month;
                mySqlCommandSel.Parameters["@Modulo"].Value = modulo;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = !string.IsNullOrEmpty(documentoID)? Convert.ToInt32(documentoID) : 0;
                #endregion

                DTO_ReportDocumentoPendientes presupuesto = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    presupuesto = new DTO_ReportDocumentoPendientes(dr);
                    result.Add(presupuesto);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesGlobal_DocumentosPendiente");
                throw exception;
            }
        }

        #endregion
    }
}


