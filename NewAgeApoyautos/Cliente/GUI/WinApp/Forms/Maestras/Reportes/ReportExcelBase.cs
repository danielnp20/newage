using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Excel;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ReportExcelBase : Form
    {
        #region Variables

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        // Variable para el manejo de aplicacion 
        private Excel.Application app;
        private string _idForm = AppForms.MasterReportXls.ToString();

        //Variables para resultado
        System.Data.DataTable result = new System.Data.DataTable();

        #endregion

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public ReportExcelBase(System.Data.DataTable _query, int? idFormCustom = null, bool? loadResources = null)
        {
            InitializeComponent();

            this.Text = this._bc.GetResource(LanguageTypes.Forms, (AppForms.MasterReportXls).ToString() + "_ReportXLS");

            try
            {
                this._idForm = idFormCustom != null ? idFormCustom.ToString() : this._idForm;
                this.result = _query;
                if(loadResources == null || loadResources.Value)
                    this.LoadResources();
                this.gcExport.DataSource = this.result;
                this.gvExport.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReportExcelBase.cs", "ReportExcelBase"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Funcion que se encarga de Traer los recuros
        /// </summary>
        private void LoadResources()
        {
            for (int i = 0; i < this.result.Columns.Count; i++)
                this.result.Columns[i].Caption = this._bc.GetResource(LanguageTypes.Forms, this._idForm + "_" + this.result.Columns[i]);
        }

        #endregion

        #region Eventos del Formulario

        /// <summary>
        /// Evento que se encarga de exportar el Archivo a Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.sfdGuardaDoc.ShowDialog(this) == DialogResult.OK)
                {
                    this.gvExport.ExportToXls(this.sfdGuardaDoc.FileName);

                    this.app = new Excel.Application();
                    this.app.Visible = true;

                    this.app.Workbooks.Open(System.IO.Path.GetFullPath(this.sfdGuardaDoc.FileName));
                }
            }
            catch (Exception)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                return;
            }
        }

        #endregion
    }
}
