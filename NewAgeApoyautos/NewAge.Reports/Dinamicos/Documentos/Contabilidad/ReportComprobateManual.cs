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
using NewAge.DTO.Reportes;

namespace NewAge.Reports.Dinamicos
{
    public partial class ReportComprobateManual : ReportBase
    {
        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public ReportComprobateManual() { }

        /// <summary>
        /// Contructor con parametros de conexion
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        /// <param name="numeroDoc"></param>
        public ReportComprobateManual(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int numeroDoc)
            : base(loggerConn, c, tx, empresa, userId, formatType, numeroDoc)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "11_ComprobanteManual");
            this.lblComprobante.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Comprobante");
            this.lblNro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Nro");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Fecha");
            this.lblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Total");
        }

        #region Funciones Protected

        /// <summary>
        /// Funcion que se encarga de iniciar el reporte
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        } 

        #endregion

        #region Funciones  Publicas

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numeroDoc, bool isAprovada, bool moneda)
        {
            try
            {
                #region Verifica si el comprobante esta en estado aprobada  o Preaprobar
                //(Si es para aprobadar colocar el preliminar, si es aprobada le quita el Preliminar)
                if (!isAprovada)
                {
                    this.Watermark.Font = new System.Drawing.Font("Arial", 144F);
                    this.Watermark.ForeColor = System.Drawing.Color.Gainsboro;
                    this.Watermark.Text = "PRELIMINAR";
                    this.Watermark.TextTransparency = 119;
                }
                #endregion
               #region Verifica el tipo de moneda para colocar el recurso

                if (moneda)
                    this.tblMoneda.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_MonedaLocal");
                else
                    this.tblMoneda.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_MonedaExtrajera"); 

                #endregion

                ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ContabilidadTotal> data = _moduloContabilidad.ReportesContabilidad_ComprobanteManual(numeroDoc, isAprovada, moneda);

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

        #endregion

    }
}
