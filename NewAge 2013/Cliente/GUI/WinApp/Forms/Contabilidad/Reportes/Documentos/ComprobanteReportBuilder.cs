using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.Resultados;
using NewAge.DTO.Attributes;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Reportes;
using NewAge.DTO.Negocio.Reportes;
using System.Collections;
using NewAge.Cliente.GUI.WinApp.Forms;
using NewAge.ReportesComunes;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    
    public class ComprobanteReportBuilder : IFiltrable
    {
        #region Variables
        protected DTO_glConsulta Consulta = null;
        protected bool MultiMonedaInd;
        public ComprobanteReport CompReport; 
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Builder for Comprobante Report (collects and organises report data)
        /// </summary>
        /// <param name="documentId">ID of the current document allowing to get information about it</param>
        /// <param name="multimoneda">MultiMoneda property of the document (true - MultiMoneda; false - not MultiMoneda)</param>
        /// <param name="data">data for the report</param>
        /// <param name="isPre">true - comprobante no esta aprobado</param>
        /// <param name="show">Indica si se debe mostrar o solo generar</param>
        /// <param name="allFields">Indica si se debe mostrar todos los campos</param>
        public ComprobanteReportBuilder(int documentId, bool multimoneda, List<DTO_ReportComprobante2> data, bool isPre , bool show, bool allFields = false)
        {
            BaseController _bc = BaseController.GetInstance();

            this.MultiMonedaInd = multimoneda;
            List<DTO_ReportComprobante2> l = new List<DTO_ReportComprobante2>();

            ArrayList fieldList = new ArrayList();

            #region Report field list
            #region Obtener la lista de los campos a travez MasterQuery (currently isn't used)
            if (!allFields)
            {
                #region Crear IgnoreList
                List<string> ignore = new List<string>();

                ignore.Add("Index");
                ignore.Add("CentroCostoID");
                ignore.Add("LugarGeograficoID");
                ignore.Add("PrefijoCOM");
                ignore.Add("DocumentoCOM");
                ignore.Add("ActivoCOM");
                ignore.Add("ConceptoSaldoID");
                ignore.Add("IdentificadorTR");
                ignore.Add("TasaCambio");
                ignore.Add("vlrBaseME");
                ignore.Add("vlrMdaOtr");
                ignore.Add("DatoAdd1");
                ignore.Add("DatoAdd2");
                ignore.Add("DatoAdd3");
                ignore.Add("DatoAdd4");
                ignore.Add("Consecutivo");
                if (!this.MultiMonedaInd)
                {
                    ignore.Add("vlrMdaExt");
                }
                #endregion

                //MasterQuery mq = new MasterQuery(this, AppDocuments.ComprobanteManual, _bc.AdministrationModel.User.ReplicaID.Value.Value, typeof(DTO_ReportComprobanteFooter), ignore);
                //mq.ShowDialog();
            }
            #region Crear FieldList
            //if (this.Consulta != null && this.Consulta.Selecciones != null && !allFields)
            //{
            //    foreach (DTO_glConsultaSeleccion sel in this.Consulta.Selecciones)
            //    {
            //        if (sel.CampoFisico == "vlrMdaLoc")
            //        {
            //            fieldList.Add("DebitoML");
            //            fieldList.Add("CreditoML");
            //        }
            //        else
            //        {
            //            if (sel.CampoFisico == "vlrMdaExt")
            //            {
            //                fieldList.Add("DebitoME");
            //                fieldList.Add("CreditoME");
            //            }
            //            else
            //            {
            //                if (sel.CampoFisico != "Debito")
            //                {
            //                    fieldList.Add(sel.CampoFisico);
            //                };
            //            };
            //        };
            //    }
            //}
            //else
            //{

            #endregion
#endregion
            
            fieldList.AddRange(ColumnsInfo.ComprobanteFields);            
            if (MultiMonedaInd)
                {
                    fieldList.Add("DebitoME");
                    fieldList.Add("CreditoME");
                };
            #endregion
            
            //ComprobanteReportSigned compReport = new ComprobanteReportSigned(documentId, l, this.MultiMonedaInd, fieldList);
            CompReport = new ComprobanteReport(documentId, data, this.MultiMonedaInd, fieldList, isPre, _bc, new List<string>());
            if (show)
                CompReport.ShowPreview();
        } 
       
        #endregion

        #region Funciones privadas
        /// <summary>
        /// Asigna una consulta desde MasterQuery para hacer el filtrado de datos
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        void IFiltrable.SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            try
            {
                this.Consulta = consulta;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
        #endregion
    }
}
