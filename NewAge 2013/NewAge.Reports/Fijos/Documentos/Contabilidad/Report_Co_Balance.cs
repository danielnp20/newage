using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using System.Windows.Forms;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_Balance : ReportBase
    {

        //private bool colorInd = false;
        private string ItemCombo1 = string.Empty;
        private string ItemCombo2 = string.Empty;

        public Report_Co_Balance(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLbl_Año.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_Año");
            this.xrLblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_Desde");
            this.xrLblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_Hasta");
            this.xrLblMoneda.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_Moneda");
            this.xrLblLibroAux.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_Libro");
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_BalancePrueba");                   
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }


        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int año, int LongitudCuenta, int SaldoIncial, string CuentaInicial,
                                     string CuentaFinal, string libro, string tipoReport, string Moneda, int MesInicial, int MesFinal, byte? Combo1, byte? Combo2)
        {
            try
            {

                #region Colocarle nombre a los meses
                #region Mes Inicial

                string MsIn = string.Empty;

                switch (MesInicial)
                {
                    case 1:
                        MsIn = "Enero";
                        break;
                    case 2:
                        MsIn = "Febrero";
                        break;
                    case 3:
                        MsIn = "Marzo";
                        break;
                    case 4:
                        MsIn = "Abril";
                        break;
                    case 5:
                        MsIn = "Mayo";
                        break;
                    case 6:
                        MsIn = "Junio";
                        break;
                    case 7:
                        MsIn = "Julio";
                        break;
                    case 8:
                        MsIn = "Agosto";
                        break;
                    case 9:
                        MsIn = "Septiembre";
                        break;
                    case 10:
                        MsIn = "Octubre";
                        break;
                    case 11:
                        MsIn = "Noviembre";
                        break;
                    case 12:
                        MsIn = "Diciembre";
                        break;
                    case 13:
                        MsIn = "Diciembre";
                        break;
                }

                #endregion

                #region Mes Final

                string MsFn = string.Empty;

                switch (MesFinal)
                {
                    case 1:
                        MsFn = "Enero";
                        break;
                    case 2:
                        MsFn = "Febrero";
                        break;
                    case 3:
                        MsFn = "Marzo";
                        break;
                    case 4:
                        MsFn = "Abril";
                        break;
                    case 5:
                        MsFn = "Mayo";
                        break;
                    case 6:
                        MsFn = "Junio";
                        break;
                    case 7:
                        MsFn = "Julio";
                        break;
                    case 8:
                        MsFn = "Agosto";
                        break;
                    case 9:
                        MsFn = "Septiembre";
                        break;
                    case 10:
                        MsFn = "Octubre";
                        break;
                    case 11:
                        MsFn = "Noviembre";
                        break;
                    case 12:
                        MsFn = "Diciembre";
                        break;
                    case 13:
                        MsFn = "Diciembre";
                        break;
                }

                #endregion 
                #endregion

                #region Agrupamiento .. Consolidado ... Detalle

                string ItemCombo1 = string.Empty;

                #region Rompimiento 1
                switch (Combo1)
                {
                    case 1:
                        ItemCombo1 = "Centro Costo";
                        break;
                    case 2:
                        ItemCombo1 = "Proyecto";
                        break;
                    case 3:
                        ItemCombo1 = "Linea Presupuesto";
                        break;
                    case 4:
                        ItemCombo1 = "";
                        break;
                }
                #endregion

                #region Rompimiento 2

                string ItemCombo2 = string.Empty;

                switch (Combo2)
                {
                    case 1:
                        ItemCombo2 = "Proyecto";
                        break;
                    case 2:
                        ItemCombo2 = "Centro Costo";
                        break;
                    case 3:
                        ItemCombo2 = "Linea Presupuesto";
                        break;
                    case 4:
                        ItemCombo2 = " - ";
                        break;
                    case 5:
                        ItemCombo1 = "";
                        break;
                }
                #endregion
                #endregion

                if (Combo1  == 0)
                {
                    this.gh1.Visible = false;
                    this.gh2.Visible = false;
                    this.gh1.GroupFields.Clear();
                    this.gh2.GroupFields.Clear();
                    this.ItemCombo1 = string.Empty;
                    this.ItemCombo2 = string.Empty;
                }
        
                this.xrLblAño.Text = año.ToString();
                this.xrLblMesIni.Text = MsIn.ToString();
                this.xrLblFechaFin.Text = MsFn.ToString();
                this.xrLblMonedas.Text = Moneda.ToString();
                this.xrlblLibro.Text = libro.ToString();
                this.LblCombo1.Text = ItemCombo1.ToString() + ":";
                this.lblCombo2.Text = ItemCombo2.ToString() + ":";

                this.BDBalance.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.BDBalance.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(MesInicial.ToString()) ? MesInicial.ToString() : null;
                this.BDBalance.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(MesFinal.ToString()) ? MesFinal.ToString() : null;
                this.BDBalance.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(año.ToString()) ? año.ToString() : null;
                this.BDBalance.Queries[0].Parameters[4].Value = !string.IsNullOrEmpty(LongitudCuenta.ToString()) ? LongitudCuenta.ToString() : null;
                this.BDBalance.Queries[0].Parameters[5].Value = !string.IsNullOrEmpty(SaldoIncial.ToString()) ? SaldoIncial.ToString() : null;
                this.BDBalance.Queries[0].Parameters[6].Value = !string.IsNullOrEmpty(libro) ? libro : null;
                this.BDBalance.Queries[0].Parameters[7].Value = !string.IsNullOrEmpty(CuentaInicial) ? CuentaInicial : null;
                this.BDBalance.Queries[0].Parameters[8].Value = !string.IsNullOrEmpty(CuentaFinal) ? CuentaFinal : null;
                this.BDBalance.Queries[0].Parameters[9].Value = !string.IsNullOrEmpty(ItemCombo1.ToString()) ? ItemCombo1.ToString() : null;
                this.BDBalance.Queries[0].Parameters[10].Value = !string.IsNullOrEmpty(ItemCombo2.ToString()) ? ItemCombo2.ToString() : null;
                this.BDBalance.Queries[0].Parameters[11].Value = !string.IsNullOrEmpty(Moneda.ToString()) ? Moneda.ToString() : null;
                if (Combo1  == 0)
                {
                    this.BDBalance.Queries[0].Parameters[9].Value = string.Empty;
                    this.BDBalance.Queries[0].Parameters[10].Value = string.Empty;
                }
                base.ConfigureConnection(this.BDBalance);
                this.CreateReport();
                return this.ReportName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo generar el reporte por el siguiente error: " + ex);
                throw;
            }
        }
        #region Negrilla en la cuenta Principal
        //<summary>
        //Cuenta principales se les asigna negrilla
        //</summary>
        private void xrTableCell5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //Cb1 = ItemCombo1.ToString();

            XRTableCell cell = (XRTableCell)sender;
            XRTableRow Fila = FindControl("xrTableRow2", true) as XRTableRow;
            XRTableCell cell_MaxLengthInd = FindControl("Negrita", true) as XRTableCell;

            if (!string.IsNullOrEmpty(cell_MaxLengthInd.Text.Trim()))
                if (Convert.ToInt16(cell_MaxLengthInd.Text) == 1)
                    Fila.Font = new Font("Times New Roman", 8, FontStyle.Regular);
                else
                    Fila.Font = new Font("Times New Roman", 8, FontStyle.Bold);

            //if (this.colorInd)
            //{
            //    Fila.BackColor = Color.WhiteSmoke;
            //    this.colorInd = false;
            //}
            //else
            //{
            //    Fila.BackColor = Color.White;
            //    this.colorInd = true;
            //}
        }
    }
}
        #endregion

//xrTableRow2