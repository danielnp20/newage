using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_No_CertificadoIngresos : XtraReport
    {
        #region Variables

        private string _mes;

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

        public Report_No_CertificadoIngresos()
        {
        }

        public Report_No_CertificadoIngresos(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_No_CertificadoIngresos(string loggerConn, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, ExportFormatType formatType, int? numDoc = null)
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
        } 
        #endregion

        #region Funciones Virtuales

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
            

            #region Header
            if (this.lbl_Empresa != null)
                this.lbl_Empresa.Text = this.Empresa.Descriptivo.Value;
            if (this.xrLblEmpresa != null)
                this.xrLblEmpresa.Text = this.Empresa.Descriptivo.Value;

            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
                this.logoFactura = logoImage;
                if(this.pct_Logo != null)
                    this.pct_Logo.Image = logoImage;
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
        }

        /// <summary>
        /// Inicializa objetos y parametros iniciales
        /// </summary>
        protected virtual void SetInitParameters()
        {
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);

            #region Header

            if (this.lbl_Empresa != null)
                this.lbl_Empresa.Text = this.Empresa.Descriptivo.Value;
            if (this.xrLblEmpresa != null)
                this.xrLblEmpresa.Text = this.Empresa.Descriptivo.Value;

            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
                if (this.pct_Logo != null)
                    this.pct_Logo.Image = logoImage;
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
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string numDoc, bool isAprobada, decimal valorAnticipo, decimal valorRteGarantia, decimal? porcRteGarantia)
        {
            try
            {
                if (!isAprobada)
                {
                    //this.Watermark.Font = new System.Drawing.Font("Arial", 120F);
                    //this.Watermark.ForeColor = System.Drawing.Color.WhiteSmoke;
                    //this.Watermark.Text = "PRE-FACTURA";
                    //this.Watermark.TextTransparency = 119;
                }

                ModuloFacturacion _moduloFacturacion = new ModuloFacturacion(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                List<DTO_FacturacionTotales> data = _moduloFacturacion.ReportesFacturacion_FacturaVenta(numDoc, isAprobada);

                foreach (DTO_FacturacionTotales d in data)
                {
                    List<DTO_ReportFacturaVenta> tmp = new List<DTO_ReportFacturaVenta>();
                    List<DTO_ReportFacturaVenta> visibles = d.DetalleFacturaVenta.FindAll(x => x.ImprimeInd.Value.Value || x.ImprimeInd.Value == null);

                    #region Recorre los items visibles y agrupa por NroItem los valores
                    foreach (DTO_ReportFacturaVenta det in visibles)
                    {
                        DTO_ReportFacturaVenta f = ObjectCopier.Clone(d.DetalleFacturaVenta.Find(x => x.NroItem.Value == det.NroItem.Value));
                        //Valida si ya agrego el nroItem
                        if (!tmp.Exists(x => x.NroItem.Value == det.NroItem.Value))
                        {
                            f.VlrBruto.Value = d.DetalleFacturaVenta.FindAll(x => x.NroItem.Value == det.NroItem.Value).Sum(y => y.VlrBruto.Value);
                            f.ValorUNI.Value = f.CantidadUNI.Value != 0 ? f.VlrBruto.Value / f.CantidadUNI.Value : 0;
                        }
                        else
                        {
                            f.VlrBruto.Value =0;
                            f.ValorUNI.Value = 0;
                        }
                        f.VlrTotal.Value = d.DetalleFacturaVenta.FindAll(x => x.NroItem.Value == det.NroItem.Value).First().VlrTotal.Value;
                        f.VlrAnticipo.Value = valorAnticipo;
                        f.VlrRteGarantia.Value = valorRteGarantia;
                        f.PorcRteGarantia.Value = porcRteGarantia;   
                        tmp.Add(f);
                    }
                    #endregion

                    //asigna el valor de la factura en letras
                    if (this.lbl_CurrentLet != null)
                        d.LetrasValor = CurrencyFormater.GetCurrencyString("ES1", "COP", data[0].DetalleFacturaVenta[0].VlrTotal.Value.Value);

                    d.DetalleFacturaVenta = tmp;
                } 
                this.DataSource = data;
                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Funciones Privadas

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

    }
}