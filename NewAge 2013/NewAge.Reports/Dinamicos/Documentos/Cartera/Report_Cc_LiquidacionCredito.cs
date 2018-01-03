using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_Cc_LiquidacionCredito : XtraReport
    {  
        #region Variables
        protected ModuloGlobal _moduloGlobal;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected DTO_glEmpresa _empresa;
        protected int _userID;
        protected ModuloBase _moduloBase;
        protected ExportFormatType _formatType;

        protected int? numeroDoc;
        protected string loggerConnectionStr;
        protected Image logoFactura = null;
        protected string letrasValor = string.Empty;

        protected ReportProvider reportProvider;

        #endregion

        #region Propiedades

        /// <summary>
        /// Empresa
        /// </summary>
        internal DTO_glEmpresa Empresa
        {
            get { return this._empresa; }
            set { this._empresa = value; }
        }

        /// <summary>
        /// Nombre del Reporte
        /// </summary>
        public string ReportName
        {
            get;
            set;
        }

        /// <summary>
        /// Ruta del reporte
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        public Report_Cc_LiquidacionCredito()
        {
        }

        public Report_Cc_LiquidacionCredito(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_Cc_LiquidacionCredito(string loggerConn, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, ExportFormatType formatType, int? numDoc = null)
        {
            InitializeComponent();

            this._connection = conn;
            this._transaction = tx;
            this._empresa = emp;
            this._userID = userID;
            this._formatType = formatType;
            this.loggerConnectionStr = loggerConn;

            this.numeroDoc = numDoc;
            this.SetInitParameters();

            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_lblLiquidacionCredito");
            this.xrTblDatosBasicos.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_DatosBasicos");
            this.xrTblLibranza.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_libranza");
            this.xrLblFechaLiquida.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_fechaLiqui");
            this.xrTblCedula.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_cc");
            this.xrlTblNombre.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_Nombre");
            this.xrTblPlazo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_Plazo");
            this.xtLblVlrCuota.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_VlrCuota");
            this.xrLblVlrLibranza.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_VlrLibranza");
            this.xrTblInteresSeguro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_Interes/Seguro");
            this.xrTblPagaduria.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_Pagaduria");
            this.xrTblZona.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_zona");
            this.xrLAsesor.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_Asesor");
            this.xrTblResumenLiqui.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_ResumenLiquidacion");
            this.xrTblVlrSolicitado.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_vlrSolicitado");
            this.xrTblMayorVlr.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_MayorVlrSoli");
            this.xrLTotalComp.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_TotalComponentes");
            this.xrTblMayorVlrCapital.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_MayorVlrCapi");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_LblTotal");
            this.xrTblTotalMayorVlr.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35093_TotalMayorVlr");
        } 
        #endregion

        #region Funciones

        /// <summary>
        /// Inicia las variables básicas para el reporte del usuario
        /// </summary>
        /// <param name="conn">conexion a base datos</param>
        /// <param name="tx">transaccion</param>
        /// <param name="emp">empresa</param>
        /// <param name="userID">identificador del usuario</param>
        public void InitUserReport(string loggerConn, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, ExportFormatType formatType, int? numDoc = null)
        {
            this._connection = conn;
            this._transaction = tx;
            this._empresa = emp;
            this._userID = userID;
            this._formatType = formatType;
            this.loggerConnectionStr = loggerConn;

            this.numeroDoc = numDoc;
            this.SetUserParameters();
        }

        /// <summary>
        /// Inicializa objetos y parametros iniciales
        /// </summary>
        protected virtual void SetUserParameters()
        {
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);


            #region Header
            if (usuario != null)
                this.lblUserName.Text = usuario.Descriptivo.Value;
            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
                this.logoFactura = logoImage;
                if(this.imgLogoEmpresa != null)
                    this.imgLogoEmpresa.Image = logoImage;
            }
            catch { ; }

            #endregion
            #region Nombre del reporte

            string repName;
            string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            string ext = this.GetExtention();

            if (this.numeroDoc.HasValue)
            {
                // Reporte de documento
                string fileFormat = this._moduloGlobal.GetControlValue(AppControl.NombreArchivoDocumentos);
                repName = string.Format(fileFormat, this.numeroDoc.ToString());
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaDocumentos);
            }
            else
            {
                // Reporte temporal
                repName = Guid.NewGuid().ToString();
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            }

            this.ReportName = repName.ToString() + ext;
            this.Path = filesPath + docsPath + ReportName;

            #endregion
            #region Recusos
            this.lblPage.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblPage");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblDate");
            this.lblUser.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblUser");
            this.reportProvider = new ReportProvider(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this.reportProvider.LoadResources(this.AllControls<XRControl>());
            #endregion
        }

        /// <summary>
        /// Inicializa objetos y parametros iniciales
        /// </summary>
        protected virtual void SetInitParameters()
        {
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);

            #region Header

            if (usuario != null)
                this.lblUserName.Text = usuario.Descriptivo.Value;

            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
                if (this.imgLogoEmpresa != null)
                    this.imgLogoEmpresa.Image = logoImage;
            }
            catch { ; }

            #endregion
            #region Nombre del reporte

            string repName;
            string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            string ext = this.GetExtention();

            if (this.numeroDoc.HasValue)
            {
                // Reporte de documento
                string fileFormat = this._moduloGlobal.GetControlValue(AppControl.NombreArchivoDocumentos);
                repName = string.Format(fileFormat, this.numeroDoc.ToString());
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaDocumentos);
            }
            else
            {
                // Reporte temporal
                repName = Guid.NewGuid().ToString();
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            }

            this.ReportName = repName.ToString() + ext;
            this.Path = filesPath + docsPath + ReportName;

            #endregion
            #region Recusos
            this.lblPage.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblPage");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblDate");
            this.lblUser.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblUser");

            this.reportProvider = new ReportProvider(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this.reportProvider.LoadResources(this.AllControls<XRControl>());

            #endregion
            
        }

        /// <summary>
        /// Función que exporta de acuerdo al tipo de formato seleccionado por el usuario
        /// </summary>
        protected void CreateReport()
        {
            switch (this._formatType)
            {
                case ExportFormatType.pdf:
                    this.ExportToPdf(Path);
                    break;
                case ExportFormatType.xls:
                    this.ExportToXls(Path);
                    break;
                case ExportFormatType.xlsx:
                    this.ExportToXlsx(Path);
                    break;
                case ExportFormatType.html:
                    this.ExportToHtml(Path);
                    break;
            }

            if (this.numeroDoc.HasValue)
                this.ReportName = this.numeroDoc.ToString();
        }

        /// <summary>
        /// Funcion que de acuerdo al ExportType devuelve la extensión
        /// </summary>
        private string GetExtention()
        {
            string extension = string.Empty;
            switch (this._formatType)
            {
                case ExportFormatType.pdf:
                    extension = ".pdf";
                    break;
                case ExportFormatType.xls:
                    extension = ".xls";
                    break;
                case ExportFormatType.xlsx:
                    extension = ".xlsx";
                    break;
                case ExportFormatType.html:
                    extension = ".html";
                    break;
            }
            return extension;
        }
        #endregion

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int libranza)
        {           
            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_CarteraTotales> data = _moduloCartera.ReportesCartera_Cc_LiquidacionCredito(libranza);

            foreach (var d in data)
            {
                foreach (var liq in d.DetalleLiquidaCredito)
                {
                    if (liq.Plazo.Value != 0)
                        liq.VlrCuotaTotal.Value = d.DetalleLiquidaCredito.Sum(x=>x.VlrCuota.Value)/liq.Plazo.Value;
                }
            }
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
