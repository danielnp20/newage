using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using NewAge.DTO.Resultados;
using System.Data;
using System.Linq;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_RecaudosMasivos : ReportBaseLandScape
    {
        #region Variables
        private string _mes = string.Empty;
        #endregion
        public Report_Cc_RecaudosMasivos()
        {

        }

        public Report_Cc_RecaudosMasivos(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "1038_RecaudosMasivos");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int documentID, List<DTO_ccIncorporacionDeta> data)
        {
            try
            {
                ModuloCarteraFin modulo = new ModuloCarteraFin(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_RecaudosMasivos> result = new List<DTO_RecaudosMasivos>();
                Dictionary<string, string> columnComp = new Dictionary<string,string>();
                var datatable = modulo.RecaudosMasivos_GetRelacionPagos(documentID, data, DictionaryProgress.BatchProgress, columnComp);

                //Info de componentes
                string componenteCapital = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                string componenteInteres = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
                string componenteSeguro = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                string componenteIntSeguro = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);
                string componenteUsura = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteUsura);
                string componenteSaldoFavor = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSaldosAFavor);
                string componenteMora = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteMora);
                string componenteHonor = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentePrejuridico);
                string componenteGastosLeg = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentePrejuridico);

                #region Asigna datos
                foreach (DataRow row in datatable.Rows)
                {
                    DTO_RecaudosMasivos d = new DTO_RecaudosMasivos();
                    d.Credito.Value = Convert.ToInt32(row["Credito"]);
                    d.ClienteID.Value = row["ClienteID"].ToString();
                    d.Nombre.Value = row["Nombre"].ToString();
                    d.Total.Value = Convert.ToDecimal(row["Total"]);
                    d.Cuota.Value = Convert.ToInt32(row["Cuota"]);
                    d.Capital.Value = Convert.ToDecimal(row[columnComp[componenteCapital]]);
                    d.Interes.Value = Convert.ToDecimal(row[columnComp[componenteInteres]]);

                    #region Llena las columnas de componentes dinamicos
                    //Seguro
                    if (columnComp.Keys.Any(x => x == componenteIntSeguro))
                        d.InteresSeguro.Value = Convert.ToDecimal(row[columnComp[componenteIntSeguro]]);
                    else
                        d.InteresSeguro.Value = 0;
                    if (columnComp.Keys.Any(x => x == componenteSeguro))
                        d.Seguro.Value = Convert.ToDecimal(row[columnComp[componenteSeguro]]) + d.InteresSeguro.Value;
                    else
                        d.Seguro.Value = 0;
                   
                    //Saldo a favor
                    if (columnComp.Keys.Any(x => x == componenteSaldoFavor))
                        d.SaldoAFavor.Value = Convert.ToDecimal(row[columnComp[componenteSaldoFavor]]);
                    else
                        d.SaldoAFavor.Value = 0;
                
                    //Mora
                    if (columnComp.Keys.Any(x => x == componenteMora))
                        d.InteresMora.Value = Convert.ToDecimal(row[columnComp[componenteMora]]);
                    else
                        d.InteresMora.Value = 0;

                    //Honorarios(Prejur)
                    if (columnComp.Keys.Any(x => x == componenteHonor))
                        d.Honorarios.Value = Convert.ToDecimal(row[columnComp[componenteHonor]]);
                    else
                        d.Honorarios.Value = 0;

                    //Gastos Leg
                    d.GastosLegal.Value = d.Total.Value - d.Capital.Value - d.Interes.Value - d.Seguro.Value - d.SaldoAFavor.Value - d.InteresMora.Value - d.Honorarios.Value;
                    #endregion

                    d.RecaudosDet.Add(d);
                    result.Add(d);
                }    
                #endregion            

                this.DataSource = result;
                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
