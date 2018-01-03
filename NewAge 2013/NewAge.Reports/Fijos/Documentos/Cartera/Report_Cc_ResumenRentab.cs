using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Linq;
using DevExpress.DataAccess.Sql.Native;
using DevExpress.DataAccess.Sql;
using System.Data;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_ResumenRentab : ReportBase
    {
        protected ModuloContabilidad _moduloContab;
        private DateTime periodo;
        #region Variables
        private string cesionario = string.Empty;
        private List<decimal> valorRecInteres = new List<decimal>();
        private List<decimal> valorSdoCarteraAnt = new List<decimal>();
        private List<decimal> valorSaldoPrimaComp = new List<decimal>();
        #endregion
        public Report_Cc_ResumenRentab()
        {
            
        }

        public Report_Cc_ResumenRentab(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            base.lblReportName.Text = "";
            base.lblNombreEmpresa.Text = "";   
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime? fechaIni,  string compCartera)
        {
            try
            {
                #region Asigna Filtros
                this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDatasource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(compCartera)? compCartera : null;
                this.QueriesDatasource.Queries[0].Parameters[2].Value = fechaIni;
                #endregion        

                base.ConfigureConnection(this.QueriesDatasource);
                this.CreateReport();
                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #region Eventos
        /// <summary>
        /// Antesde imprimir el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbAcumulado_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                if (false)
                {
                    string tipo = this.tableDeta.Rows[0].Cells[0].Lines[0];
                    #region Valida cada cesionario para limpiar datos
                    if (!cesionario.Equals(this.lblCesionario.Text))
                    {
                        valorRecInteres.Clear();
                        valorSdoCarteraAnt.Clear();
                        valorSaldoPrimaComp.Clear();
                        cesionario = this.lblCesionario.Text;
                    }
                    #endregion
                    if (tipo.Equals("04RECINT"))
                    {
                        #region Obtiene los Recursos adicionales
                        long Mes1 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[2].Lines[0].Replace(",", ""));
                        long Mes2 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[3].Lines[0].Replace(",", ""));
                        long Mes3 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[4].Lines[0].Replace(",", ""));
                        long Mes4 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[5].Lines[0].Replace(",", ""));
                        long Mes5 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[6].Lines[0].Replace(",", ""));
                        long Mes6 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[7].Lines[0].Replace(",", ""));
                        long Mes7 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[8].Lines[0].Replace(",", ""));
                        long Mes8 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[9].Lines[0].Replace(",", ""));
                        long Mes9 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[10].Lines[0].Replace(",", ""));
                        long Mes10 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[11].Lines[0].Replace(",", ""));
                        long Mes11 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[12].Lines[0].Replace(",", ""));
                        long Mes12 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[13].Lines[0].Replace(",", ""));
                        valorRecInteres.Add(Mes1);
                        valorRecInteres.Add(Mes2);
                        valorRecInteres.Add(Mes3);
                        valorRecInteres.Add(Mes4);
                        valorRecInteres.Add(Mes5);
                        valorRecInteres.Add(Mes6);
                        valorRecInteres.Add(Mes7);
                        valorRecInteres.Add(Mes8);
                        valorRecInteres.Add(Mes9);
                        valorRecInteres.Add(Mes10);
                        valorRecInteres.Add(Mes11);
                        valorRecInteres.Add(Mes12);
                        #endregion
                    }
                    else if (tipo.Equals("05SDOCAR"))
                    {
                        #region Obtiene el saldo de Cartera
                        long Mes1 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[2].Lines[0].Replace(",", ""));
                        long Mes2 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[3].Lines[0].Replace(",", ""));
                        long Mes3 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[4].Lines[0].Replace(",", ""));
                        long Mes4 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[5].Lines[0].Replace(",", ""));
                        long Mes5 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[6].Lines[0].Replace(",", ""));
                        long Mes6 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[7].Lines[0].Replace(",", ""));
                        long Mes7 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[8].Lines[0].Replace(",", ""));
                        long Mes8 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[9].Lines[0].Replace(",", ""));
                        long Mes9 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[10].Lines[0].Replace(",", ""));
                        long Mes10 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[11].Lines[0].Replace(",", ""));
                        long Mes11 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[12].Lines[0].Replace(",", ""));
                        long Mes12 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[13].Lines[0].Replace(",", ""));
                        valorSdoCarteraAnt.Add(Mes1);
                        valorSdoCarteraAnt.Add(Mes2);
                        valorSdoCarteraAnt.Add(Mes3);
                        valorSdoCarteraAnt.Add(Mes4);
                        valorSdoCarteraAnt.Add(Mes5);
                        valorSdoCarteraAnt.Add(Mes6);
                        valorSdoCarteraAnt.Add(Mes7);
                        valorSdoCarteraAnt.Add(Mes8);
                        valorSdoCarteraAnt.Add(Mes9);
                        valorSdoCarteraAnt.Add(Mes10);
                        valorSdoCarteraAnt.Add(Mes11);
                        valorSdoCarteraAnt.Add(Mes12);
                        #endregion
                    }
                    else if (tipo.Equals("06SDOPRI"))
                    {
                        #region Obtiene el Saldo Prima Compra
                        long Mes1 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[2].Lines[0].Replace(",", ""));
                        long Mes2 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[3].Lines[0].Replace(",", ""));
                        long Mes3 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[4].Lines[0].Replace(",", ""));
                        long Mes4 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[5].Lines[0].Replace(",", ""));
                        long Mes5 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[6].Lines[0].Replace(",", ""));
                        long Mes6 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[7].Lines[0].Replace(",", ""));
                        long Mes7 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[8].Lines[0].Replace(",", ""));
                        long Mes8 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[9].Lines[0].Replace(",", ""));
                        long Mes9 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[10].Lines[0].Replace(",", ""));
                        long Mes10 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[11].Lines[0].Replace(",", ""));
                        long Mes11 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[12].Lines[0].Replace(",", ""));
                        long Mes12 = Convert.ToInt64(this.tableDeta.Rows[0].Cells[13].Lines[0].Replace(",", ""));
                        valorSaldoPrimaComp.Add(Mes1);
                        valorSaldoPrimaComp.Add(Mes2);
                        valorSaldoPrimaComp.Add(Mes3);
                        valorSaldoPrimaComp.Add(Mes4);
                        valorSaldoPrimaComp.Add(Mes5);
                        valorSaldoPrimaComp.Add(Mes6);
                        valorSaldoPrimaComp.Add(Mes7);
                        valorSaldoPrimaComp.Add(Mes8);
                        valorSaldoPrimaComp.Add(Mes9);
                        valorSaldoPrimaComp.Add(Mes10);
                        valorSaldoPrimaComp.Add(Mes11);
                        valorSaldoPrimaComp.Add(Mes12);
                        #endregion

                        #region Asigna valores de Rentabilidad
                        //this.tbRenta01.Text = (valorSdoCarteraAnt[0] + valorSaldoPrimaComp[0]) != 0 ? (valorRecInteres[0] / (valorSdoCarteraAnt[0] + valorSaldoPrimaComp[0])).ToString("p2") : "0.00%";
                        //this.tbRenta02.Text = (valorSdoCarteraAnt[1] + valorSaldoPrimaComp[1]) != 0 ? (valorRecInteres[1] / (valorSdoCarteraAnt[1] + valorSaldoPrimaComp[1])).ToString("p2") : "0.00%";
                        //this.tbRenta03.Text = (valorSdoCarteraAnt[2] + valorSaldoPrimaComp[2]) != 0 ? (valorRecInteres[2] / (valorSdoCarteraAnt[2] + valorSaldoPrimaComp[2])).ToString("p2") : "0.00%";
                        //this.tbRenta04.Text = (valorSdoCarteraAnt[3] + valorSaldoPrimaComp[3]) != 0 ? (valorRecInteres[3] / (valorSdoCarteraAnt[3] + valorSaldoPrimaComp[3])).ToString("p2") : "0.00%";
                        //this.tbRenta05.Text = (valorSdoCarteraAnt[4] + valorSaldoPrimaComp[4]) != 0 ? (valorRecInteres[4] / (valorSdoCarteraAnt[4] + valorSaldoPrimaComp[4])).ToString("p2") : "0.00%";
                        //this.tbRenta06.Text = (valorSdoCarteraAnt[5] + valorSaldoPrimaComp[5]) != 0 ? (valorRecInteres[5] / (valorSdoCarteraAnt[5] + valorSaldoPrimaComp[5])).ToString("p2") : "0.00%";
                        //this.tbRenta07.Text = (valorSdoCarteraAnt[6] + valorSaldoPrimaComp[6]) != 0 ? (valorRecInteres[6] / (valorSdoCarteraAnt[6] + valorSaldoPrimaComp[6])).ToString("p2") : "0.00%";
                        //this.tbRenta08.Text = (valorSdoCarteraAnt[7] + valorSaldoPrimaComp[7]) != 0 ? (valorRecInteres[7] / (valorSdoCarteraAnt[7] + valorSaldoPrimaComp[7])).ToString("p2") : "0.00%";
                        //this.tbRenta09.Text = (valorSdoCarteraAnt[8] + valorSaldoPrimaComp[8]) != 0 ? (valorRecInteres[8] / (valorSdoCarteraAnt[8] + valorSaldoPrimaComp[8])).ToString("p2") : "0.00%";
                        //this.tbRenta10.Text = (valorSdoCarteraAnt[9] + valorSaldoPrimaComp[9]) != 0 ? (valorRecInteres[9] / (valorSdoCarteraAnt[9] + valorSaldoPrimaComp[9])).ToString("p2") : "0.00%";
                        //this.tbRenta11.Text = (valorSdoCarteraAnt[10] + valorSaldoPrimaComp[10]) != 0 ? (valorRecInteres[10] / (valorSdoCarteraAnt[10] + valorSaldoPrimaComp[10])).ToString("p2") : "0.00%";
                        //this.tbRenta12.Text = (valorSdoCarteraAnt[11] + valorSaldoPrimaComp[11]) != 0 ? (valorRecInteres[11] / (valorSdoCarteraAnt[11] + valorSaldoPrimaComp[11])).ToString("p2") : "0.00%";
                        #endregion
                    }
                }
            }
            catch (Exception)
            {
                ;
            }
        }

        /// <summary>
        /// Antesde imprimir el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDescripcion_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                string tipotmp = this.tableDeta.Rows[0].Cells[0].Lines[0];
                if (tipotmp.Equals("10RENTAB"))
                {
                    this.tbDeta01.Text = this.tbDeta01.Text + "%";
                    this.tbDeta02.Text = this.tbDeta02.Text + "%";
                    this.tbDeta03.Text = this.tbDeta03.Text + "%";
                    this.tbDeta04.Text = this.tbDeta04.Text + "%";
                    this.tbDeta05.Text = this.tbDeta05.Text + "%";
                    this.tbDeta06.Text = this.tbDeta06.Text + "%";
                    this.tbDeta07.Text = this.tbDeta07.Text + "%";
                    this.tbDeta08.Text = this.tbDeta08.Text + "%";
                    this.tbDeta09.Text = this.tbDeta09.Text + "%";
                    this.tbDeta10.Text = this.tbDeta10.Text + "%";
                    this.tbDeta11.Text = this.tbDeta11.Text + "%";
                    this.tbDeta12.Text = this.tbDeta12.Text + "%";
                    this.tbDetaAcumulado.Text = "";

                }
                else
                {
                    this.tbDeta01.Text = this.tbDeta01.Text.Substring(0, this.tbDeta01.Text.Length - 3);
                    this.tbDeta02.Text = this.tbDeta02.Text.Substring(0, this.tbDeta02.Text.Length - 3);
                    this.tbDeta03.Text = this.tbDeta03.Text.Substring(0, this.tbDeta03.Text.Length - 3);
                    this.tbDeta04.Text = this.tbDeta04.Text.Substring(0, this.tbDeta04.Text.Length - 3);
                    this.tbDeta05.Text = this.tbDeta05.Text.Substring(0, this.tbDeta05.Text.Length - 3);
                    this.tbDeta06.Text = this.tbDeta06.Text.Substring(0, this.tbDeta06.Text.Length - 3);
                    this.tbDeta07.Text = this.tbDeta07.Text.Substring(0, this.tbDeta07.Text.Length - 3);
                    this.tbDeta08.Text = this.tbDeta08.Text.Substring(0, this.tbDeta08.Text.Length - 3);
                    this.tbDeta09.Text = this.tbDeta09.Text.Substring(0, this.tbDeta09.Text.Length - 3);
                    this.tbDeta10.Text = this.tbDeta10.Text.Substring(0, this.tbDeta10.Text.Length - 3);
                    this.tbDeta11.Text = this.tbDeta11.Text.Substring(0, this.tbDeta11.Text.Length - 3);
                    this.tbDeta12.Text = this.tbDeta12.Text.Substring(0, this.tbDeta12.Text.Length - 3);                  
                    this.tbDetaAcumulado.Text = this.tbDetaAcumulado.Text.Substring(0, this.tbDetaAcumulado.Text.Length - 3);
                    if (tipotmp.Equals("05SDOCAR") || tipotmp.Equals("06SDOPRI"))
                         this.tbDetaAcumulado.Text = "0";
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }    
        #endregion
    }
}
