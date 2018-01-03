using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Reports;
using SentenceTransformer;
using NewAge.Librerias.Project;
using DevExpress.XtraEditors;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.DTO.Resultados;
using System.Data;
using NewAge.Cliente.GUI.WinApp.Forms;
using Microsoft.Office.Interop.Excel;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_OperacionesConjuntas : ReportParametersForm
    {
        #region Variables

        //Varibales para el manejo del formario
        private DateTime _Periodo;
        private Excel.Application app;
        private System.Data.DataTable result = new System.Data.DataTable();

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que se encarga de desahbilitar los controles que no se utilizan
        /// </summary>
        private void EnableControls(bool isEnable)
        {
            this.btnFilter.Enabled = isEnable;
            this.btnResetFilter.Enabled = isEnable;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;
        }

        /// <summary>
        /// Funcion que se encarga de gerenerar los datos de excel
        /// </summary>
        private void GenerateExcel()
        {
            try
            {
                this.GenerateExcelDatCapexUSD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_OperacionesConjuntas.cs", "GenerateExcel"));
            }
        }

        /// <summary>
        /// Funcion que se encarga de generar los datos para el reporte de DatCapexUSD
        /// </summary>
        private void GenerateExcelDatCapexUSD()
        {
            try
            {
                #region Variables

                string proyID = string.Empty;
                string proyTemp = string.Empty;

                int proyFila = 1;
                decimal totalEnero = 0;
                decimal totalFebrero = 0;
                decimal totalMarzo = 0;
                decimal totalAbril = 0;
                decimal totalMayo = 0;
                decimal totalJunio = 0;
                decimal totalJulio = 0;
                decimal totalAgosto = 0;
                decimal totalSeptiembre = 0;
                decimal totalOctubre = 0;
                decimal totalNoviembre = 0;
                decimal totalDiciembre = 0; 

                #endregion

                #region Genera los resultado para pintarlos en el excel
                this.app.Range["A3"].Select();

                for (int i = 0; i < this.result.Rows.Count; i++)
                {
                    //Verifica si el proyecto es nuevo para pintarlo por primera vez
                    proyTemp = this.result.Rows[i]["ProyectoID"].ToString();
                    if (proyID != proyTemp)
                    {
                        #region Datos con la suma del proyecto por mes
                        if (!string.IsNullOrWhiteSpace(proyID))
                        {
                            this.app.ActiveCell.Offset[proyFila * -1, 2].Value = totalEnero;
                            this.app.ActiveCell.Offset[proyFila * -1, 2].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 3].Value = totalFebrero;
                            this.app.ActiveCell.Offset[proyFila * -1, 3].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 4].Value = totalMarzo;
                            this.app.ActiveCell.Offset[proyFila * -1, 4].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 5].Value = totalAbril;
                            this.app.ActiveCell.Offset[proyFila * -1, 5].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 6].Value = totalMayo;
                            this.app.ActiveCell.Offset[proyFila * -1, 6].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 7].Value = totalJunio;
                            this.app.ActiveCell.Offset[proyFila * -1, 7].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 8].Value = totalJulio;
                            this.app.ActiveCell.Offset[proyFila * -1, 8].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 9].Value = totalAgosto;
                            this.app.ActiveCell.Offset[proyFila * -1, 9].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 10].Value = totalSeptiembre;
                            this.app.ActiveCell.Offset[proyFila * -1, 10].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 11].Value = totalOctubre;
                            this.app.ActiveCell.Offset[proyFila * -1, 11].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 12].Value = totalNoviembre;
                            this.app.ActiveCell.Offset[proyFila * -1, 12].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 13].Value = totalDiciembre;
                            this.app.ActiveCell.Offset[proyFila * -1, 13].Font.Bold = true;

                            this.app.ActiveCell.Offset[proyFila * -1, 14].FormulaR1C1 = "=SUM(RC[-12]:RC[-1])";
                            this.app.ActiveCell.Offset[proyFila * -1, 14].Font.Bold = true;
                        }
                        #endregion
                        #region Pinta el proyecto para el rompiento del excel
                        proyID = proyTemp;
                        this.app.ActiveCell.Value = proyID;
                        this.app.ActiveCell.Font.Bold = true;

                        proyFila = 1;
                        totalEnero = 0;
                        totalFebrero = 0;
                        totalMarzo = 0;
                        totalAbril = 0;
                        totalMayo = 0;
                        totalJunio = 0;
                        totalJulio = 0;
                        totalAgosto = 0;
                        totalSeptiembre = 0;
                        totalOctubre = 0;
                        totalNoviembre = 0;
                        totalDiciembre = 0;

                        this.app.ActiveCell.Offset[1, 0].Select(); 
                        #endregion
                    }

                    #region Calcula el total del proyecto por mes
                    ++proyFila;
                    totalEnero += Convert.ToDecimal(this.result.Rows[i]["Enero"]);
                    totalFebrero += Convert.ToDecimal(this.result.Rows[i]["Febrero"]);
                    totalMarzo += Convert.ToDecimal(this.result.Rows[i]["Marzo"]);
                    totalAbril += Convert.ToDecimal(this.result.Rows[i]["Abril"]);
                    totalMayo += Convert.ToDecimal(this.result.Rows[i]["Mayo"]);
                    totalJunio += Convert.ToDecimal(this.result.Rows[i]["Junio"]);
                    totalJulio += Convert.ToDecimal(this.result.Rows[i]["Julio"]);
                    totalAgosto += Convert.ToDecimal(this.result.Rows[i]["Agosto"]);
                    totalSeptiembre += Convert.ToDecimal(this.result.Rows[i]["Septiembre"]);
                    totalOctubre += Convert.ToDecimal(this.result.Rows[i]["Octubre"]);
                    totalNoviembre += Convert.ToDecimal(this.result.Rows[i]["Noviembre"]);
                    totalDiciembre += Convert.ToDecimal(this.result.Rows[i]["Diciembre"]); 
                    #endregion
                    #region Pinta los datos en el excel
                    this.app.ActiveCell.Offset[0, 1].Value = this.result.Rows[i]["LineaPresupuestoID"].ToString();
                    this.app.ActiveCell.Offset[0, 2].Value = this.result.Rows[i]["Enero"].ToString();
                    this.app.ActiveCell.Offset[0, 3].Value = this.result.Rows[i]["Febrero"].ToString();
                    this.app.ActiveCell.Offset[0, 4].Value = this.result.Rows[i]["Marzo"].ToString();
                    this.app.ActiveCell.Offset[0, 5].Value = this.result.Rows[i]["Abril"].ToString();
                    this.app.ActiveCell.Offset[0, 6].Value = this.result.Rows[i]["Mayo"].ToString();
                    this.app.ActiveCell.Offset[0, 7].Value = this.result.Rows[i]["Junio"].ToString();
                    this.app.ActiveCell.Offset[0, 8].Value = this.result.Rows[i]["Julio"].ToString();
                    this.app.ActiveCell.Offset[0, 9].Value = this.result.Rows[i]["Agosto"].ToString();
                    this.app.ActiveCell.Offset[0, 10].Value = this.result.Rows[i]["Septiembre"].ToString();
                    this.app.ActiveCell.Offset[0, 11].Value = this.result.Rows[i]["Octubre"].ToString();
                    this.app.ActiveCell.Offset[0, 12].Value = this.result.Rows[i]["Noviembre"].ToString();
                    this.app.ActiveCell.Offset[0, 13].Value = this.result.Rows[i]["Diciembre"].ToString();
                    //Calucla el total por año de cada linea
                    this.app.ActiveCell.Offset[0, 14].FormulaR1C1 = "=SUM(RC[-12]:RC[-1])";
                    this.app.ActiveCell.Offset[0, 14].Font.Bold = true;

                    //Pasa a la siguiente fila 
                    this.app.ActiveCell.Offset[1, 0].Select(); 
                    #endregion
                } 
                #endregion

                #region Pinta el valor del total del proyecto por mes
                if (this.result.Rows.Count > 0)
                {
                    this.app.ActiveCell.Offset[proyFila * -1, 2].Value = totalEnero;
                    this.app.ActiveCell.Offset[proyFila * -1, 2].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 3].Value = totalFebrero;
                    this.app.ActiveCell.Offset[proyFila * -1, 3].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 4].Value = totalMarzo;
                    this.app.ActiveCell.Offset[proyFila * -1, 4].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 5].Value = totalAbril;
                    this.app.ActiveCell.Offset[proyFila * -1, 5].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 6].Value = totalMayo;
                    this.app.ActiveCell.Offset[proyFila * -1, 6].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 7].Value = totalJunio;
                    this.app.ActiveCell.Offset[proyFila * -1, 7].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 8].Value = totalJulio;
                    this.app.ActiveCell.Offset[proyFila * -1, 8].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 9].Value = totalAgosto;
                    this.app.ActiveCell.Offset[proyFila * -1, 9].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 10].Value = totalSeptiembre;
                    this.app.ActiveCell.Offset[proyFila * -1, 10].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 11].Value = totalOctubre;
                    this.app.ActiveCell.Offset[proyFila * -1, 11].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 12].Value = totalNoviembre;
                    this.app.ActiveCell.Offset[proyFila * -1, 12].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 13].Value = totalDiciembre;
                    this.app.ActiveCell.Offset[proyFila * -1, 13].Font.Bold = true;

                    this.app.ActiveCell.Offset[proyFila * -1, 14].FormulaR1C1 = "=SUM(RC[-12]:RC[-1])";
                    this.app.ActiveCell.Offset[proyFila * -1, 14].Font.Bold = true;
                } 
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_OperacionesConjuntas.cs", "GenerateExcelDatCapexUSD"));
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructor
        /// </summary>
        public Reports_OperacionesConjuntas()
        {
            this.Module = ModulesPrefix.oc;
            this.ReportForm = AppReportParametersForm.ocOperaciones;
        }

        #endregion

        #region Funciones protected

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.oc;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = false;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ocOperaciones).ToString());
            
            #region Configurar Filtros del periodo

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();

            this.EnableControls(false);

            #endregion
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                this.EnableControls(true);
                this.periodoFilter1.txtYear1.Visible = false;
                this.btnExportToXLS.Enabled = false;

                int year = Convert.ToInt16(this.periodoFilter1.Year[0].ToString());
                int Month = DateTime.Now.Month;
                this._Periodo = new DateTime(year, Month, DateTime.DaysInMonth(year, Month));

                //Traigo la informacion
                this.result = this._bc.AdministrationModel.ReportesOpereacionesConjuntas_Legalizaciones(this.periodo);

                //Verifico si la consulta devuelve resultados
                if (this.result.Rows.Count != 0)
                {
                    this.app = new Excel.Application();

                    string fileName = _bc.UrlDocumentFile(TipoArchivo.Plantillas, null, null, AppTemplates.oc_Legalizaciones);
                    //this.app.Workbooks.Add("C:/Users/kmilo/Desktop/Prueba");
                    this.app.Workbooks.Add(fileName);
                    
                    this.GenerateExcel();
                    this.app.Visible = true;
                    this.EnableControls(true);
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_OperacionesConjuntas.cs", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

    }
}

