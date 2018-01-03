using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ExportLibranzas : Form
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
        /// Constructor por defecto
        /// </summary>
        public ExportLibranzas()
        {
            InitializeComponent();

            this.btnExport.Enabled = false;
        }

        #region Funciones privadas

        /// <summary>
        /// Funcion que se encarga de Traer los recuros
        /// </summary>
        private void LoadResourses()
        {
            for (int i = 0; i < this.result.Columns.Count; i++)
                this.result.Columns[i].Caption = this._bc.GetResource(LanguageTypes.Forms, this._idForm + "_" + this.result.Columns[i]);
        }

        #endregion

        #region Eventos Formulario
        /// <summary>
        /// Boton para exportar a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (this.sfdGuardarDoc.ShowDialog(this) == DialogResult.OK)
                {
                    this.app = new Excel.Application();
                    gvExport.ExportToXls(this.sfdGuardarDoc.FileName);

                    
                    this.app.Visible = true;

                    this.app.Workbooks.Open(System.IO.Path.GetFullPath(this.sfdGuardarDoc.FileName));
                }
            }
            catch (Exception)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                return;
            }
        }

        /// <summary>
        /// Boton para buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (this.dtpPeriodoIni.EditValue == null || this.dtpPeriodoFin.EditValue == null)
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidPeriod));
                else
                {
                    DateTime PeriodoInicial = Convert.ToDateTime(this.dtpPeriodoIni.EditValue);
                    DateTime PeriodoFin = Convert.ToDateTime(this.dtpPeriodoFin.EditValue);

                    System.Data.DataTable result = this._bc.AdministrationModel.ReportesCartera_Cc_LibranzasExcel(PeriodoInicial, PeriodoFin, string.Empty, string.Empty, string.Empty, string.Empty);

                    if (result.Rows.Count > 0)
                    {
                        this.btnExport.Enabled = true;
                        this.LoadResourses();
                        gcExport.DataSource = result;
                    }
                    else
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                }
            }
            catch (Exception)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidInputReportData));
                return;
            }
        }
        #endregion
    }
}