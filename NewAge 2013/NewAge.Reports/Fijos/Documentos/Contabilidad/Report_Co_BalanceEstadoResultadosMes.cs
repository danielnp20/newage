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
    public partial class Report_Co_BalanceEstadoResultadosMes : ReportBaseLandScape
    {
        //private bool colorInd = false;
        private string ItemCombo1 = string.Empty;
        private string ItemCombo2 = string.Empty;
        private decimal suma = 0;
        private decimal sumam01 = 0;
        private decimal sumam02 = 0;
        private decimal sumam03 = 0;
        private decimal sumam04 = 0;
        private decimal sumam05 = 0;
        private decimal sumam06 = 0;
        private decimal sumam07 = 0;
        private decimal sumam08 = 0;
        private decimal sumam09 = 0;
        private decimal sumam10 = 0;
        private decimal sumam11 = 0;
        private decimal sumam12 = 0;
        private int factor = -1;


        public Report_Co_BalanceEstadoResultadosMes(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = "Estado de Resultados"; this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_BalancePrueba");
            this.lblReportName.Visible = false;
            this.lblNombreEmpresa.Visible = false;
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
                                     string CuentaFinal, string libro, string tipoReport, string Moneda, int MesInicial, int MesFinal, byte? Combo1, byte? Combo2, string proyecto, string centroCto)
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
                this.lblFechas.Text = "De " + MsIn + " A " + MsFn + " de " + año.ToString();
                string tercero = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                DTO_coTercero terceroDTO = (DTO_coTercero)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, tercero, true, false);
                if (terceroDTO != null)
                {
                    this.lblNomEmpresa.Text = terceroDTO.Descriptivo.Value;
                    this.lblNItEmpresa.Text = "Nit: " + terceroDTO.ID.Value;
                }

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
                this.LblCombo1.Text = ItemCombo1.ToString() + ":";
                this.lblCombo2.Text = ItemCombo2.ToString() + ":";
                #endregion

                if (Combo1 == 0)
                {
                    this.gh1.Visible = false;
                    this.gh2.Visible = false;
                    this.gh1.GroupFields.Clear();
                    this.gh2.GroupFields.Clear();
                    this.ItemCombo1 = string.Empty;
                    this.ItemCombo2 = string.Empty;
                    this.titleCell.Visible = false;
                }

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
                this.BDBalance.Queries[0].Parameters[12].Value = !string.IsNullOrEmpty(proyecto) ? proyecto : null;
                this.BDBalance.Queries[0].Parameters[13].Value = !string.IsNullOrEmpty(centroCto) ? centroCto : null;
                if (Combo1 == 0)
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
            XRTableCell cell = (XRTableCell)sender;          
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.suma += Convert.ToDecimal(cell.Text);

            //XRTableRow Fila = FindControl("xrTableRow2", true) as XRTableRow; 
            //XRTableCell cell_MaxLengthInd = FindControl("Negrita", true) as XRTableCell;
            //    Fila.Font = new Font("Times New Roman", 10, FontStyle.Regular);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCellTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                //if (this.suma >= 0)
                //{
                //    this.xrLblTotal.Text = "Pérdida";
                //    this.factor = 1;
                //}
                //else
                //{
                   this.xrLblTotal.Text = "Utilidad";
                //    this.factor = -1;
                //}

                this.tbCellTotal.Text = Math.Round(this.suma,2).ToString("n0");
            }
            catch (Exception ex)
            {;
            }
        }

        private void xrTableCell32_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam01+= Convert.ToDecimal(cell.Text);

        }

        private void xrTableCell34_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam02 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell35_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam03 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell36_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam04 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell37_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam05 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell38_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam06 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell39_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam07 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell40_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam08 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell41_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam09 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell42_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam10 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell43_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam11 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell33_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableCell cell_Cuenta = FindControl("lblCuentaID", true) as XRTableCell;
            if (!string.IsNullOrEmpty(cell_Cuenta.Text.Trim()) && cell_Cuenta.Text.TrimEnd().Length == 1)
                this.sumam12 += Convert.ToDecimal(cell.Text);
        }

        private void xrTableCell51_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            if (this.suma >= 0)
            {
                this.factor = 1;
            }
            else
            {
                this.factor = 1;
            }
            this.xrTableCell51.Text = Math.Round(this.sumam01 * this.factor, 2).ToString("n0");

        }

        private void xrTableCell52_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell52.Text = Math.Round(this.sumam02 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell47_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell47.Text = Math.Round(this.sumam03 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell50_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell50.Text = Math.Round(this.sumam04 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell53_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell53.Text = Math.Round(this.sumam05 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell45_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell45.Text = Math.Round(this.sumam06 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell49_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell49.Text = Math.Round(this.sumam07 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell54_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell54.Text = Math.Round(this.sumam08 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell46_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell46.Text = Math.Round(this.sumam09 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell48_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell48.Text = Math.Round(this.sumam10 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell55_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell55.Text = Math.Round(this.sumam11 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell44_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell44.Text = Math.Round(this.sumam12 * this.factor, 2).ToString("n0");
        }

        private void xrTableCell56_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell56.Text = Math.Round((this.sumam01 ) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell57_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell57.Text = Math.Round((this.sumam01 + this.sumam02) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell58_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell58.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell59_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell59.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03+this.sumam04) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell60_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell60.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03 + this.sumam04 + this.sumam05) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell61_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell61.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03 + this.sumam04 + this.sumam05 + this.sumam06) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell62_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell62.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03 + this.sumam04 + this.sumam05 + this.sumam06 + this.sumam07) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell63_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell63.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03 + this.sumam04 + this.sumam05 + this.sumam06 + this.sumam07 + this.sumam08) * this.factor, 2).ToString("n0");

        }

        private void xrTableCell64_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell64.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03 + this.sumam04 + this.sumam05 + this.sumam06 + this.sumam07 + this.sumam08 + this.sumam09) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell65_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell65.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03 + this.sumam04 + this.sumam05 + this.sumam06 + this.sumam07 + this.sumam08 + this.sumam09 + this.sumam10) * this.factor, 2).ToString("n0");

        }

        private void xrTableCell66_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell66.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03 + this.sumam04 + this.sumam05 + this.sumam06 + this.sumam07 + this.sumam08 + this.sumam09 + this.sumam10 + this.sumam11) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell67_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell67.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03 + this.sumam04 + this.sumam05 + this.sumam06 + this.sumam07 + this.sumam08 + this.sumam09 + this.sumam10 + this.sumam11 + this.sumam12) * this.factor, 2).ToString("n0");
        }

        private void xrTableCell68_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrTableCell68.Text = Math.Round((this.sumam01 + this.sumam02 + this.sumam03 + this.sumam04 + this.sumam05 + this.sumam06 + this.sumam07 + this.sumam08 + this.sumam09 + this.sumam10 + this.sumam11 + this.sumam12) * this.factor, 2).ToString("n0");
        }


    }
    #endregion
}


//xrTableRow2