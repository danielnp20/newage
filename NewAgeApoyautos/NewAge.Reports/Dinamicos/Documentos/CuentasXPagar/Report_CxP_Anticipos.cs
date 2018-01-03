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
    public partial class Report_CxP_Anticipos : ReportBase
    {
        #region Variable del formulario
        //Variables para convertir a positivos los valores negativos
        decimal retefuente = 0;
        decimal reteIva = 0;
        decimal reteIca = 0;
        decimal anticipo = 0;

        //Variables para calculto total
        decimal vlrBruto = 0;
        decimal iva = 0;
        decimal retIva = 0;
        decimal retFuente = 0;
        decimal retIca = 0;
        decimal antici = 0;
        decimal total = 0;
        #endregion

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public Report_CxP_Anticipos() { }

        public Report_CxP_Anticipos(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int numeroDoc)
            : base(loggerConn, c, tx, empresa, userId, formatType, numeroDoc)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "SOLICITUD DE ANTICIPOS");            
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_Fecha");            
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numeroDoc, bool isAprobada)
        {
            try
            {
                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                #region Verifica si la causacion esta en estado aprobada  o Preaprobar
                //(Si es para aprobadar colocar el preliminar, si es aprobada le quita el Preliminar)
                if (!isAprobada)
                {
                    this.Watermark.Font = new System.Drawing.Font("Arial", 144F);
                    this.Watermark.ForeColor = System.Drawing.Color.Gainsboro;
                    this.Watermark.Text = "PRELIMINAR";
                    this.Watermark.TextTransparency = 119;

                    this.lblUserSolicita.Text = this.lblUserName.Text;
                }
                else 
                {
                    this.lblUserAprueba.Visible = true;
                    this.lineAprueba.Visible = true;
                    this.lblApruebaTit.Visible = true;
                    var userSolici = this._moduloGlobal.seUsuario_GetUserByReplicaID(ctrl.seUsuarioID.Value.Value);
                    this.lblUserSolicita.Text = userSolici != null ? userSolici.Descriptivo.Value : string.Empty;
                    this.lblUserAprueba.Text = this.lblUserName.Text;
                }
                #endregion

                ModuloCuentasXPagar _moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ReportCxPTotales> data = _moduloCxP.ReportesCuentasXPagar_DocumentoAnticipo(numeroDoc);
                this.DataSource = data;

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
