using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.Librerias.Project;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_Co_DocumentoContable : ReportBase
    {

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public Report_Co_DocumentoContable() { }

        public Report_Co_DocumentoContable(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int numeroDoc)
            : base(loggerConn, c, tx, empresa, userId, formatType, numeroDoc)
        {
            //this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_DocumentoCont");
            this.lblAFavorDe.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_AFavorDe");
            this.lblFacturaNro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_FacturaNro");
            this.lblFechaFactura.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_FechaFactura");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_Fecha");
            this.lblDescripcion.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_Descripcion");
            this.lblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_Total");
            this.lblComprobante.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_Comprobante");            
            this.lblNit.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_Nit");
            this.lblSolicita.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_Solicita");
            this.lblAprueba.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_Aprueba");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numeroDoc, bool isAprovada, int documento)
        {
            try
            {
                #region Verifica si la causacion esta en estado aprobada  o Preaprobar
                //(Si es para aprobadar colocar el preliminar, si es aprobada le quita el Preliminar)
                if (!isAprovada)
                {
                    this.Watermark.Font = new System.Drawing.Font("Arial", 144F);
                    this.Watermark.ForeColor = System.Drawing.Color.Gainsboro;
                    this.Watermark.Text = "PRELIMINAR";
                    this.Watermark.TextTransparency = 119;
                }
                #endregion
                #region Revisa el tipo de documento cargar el nombre

                switch (documento)
                {
                    case(AppDocuments.DocumentoContable):
                        this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "12_DocumentoCont");
                        break;

                    case(AppDocuments.CruceCuentas):
                        this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "18_CruceCuentas");
                        break;
                }
                

              
                    
                #endregion

                ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ReportCxPTotales> data = _moduloContabilidad.ReportesContabilidad_DocumentoContable(numeroDoc, isAprovada);

                if (data.Count != 0)
                {
                    this.DataSource = data;
                    this.CreateReport();
                    return this.ReportName;
                }

                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
