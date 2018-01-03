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

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_py_PlaneacionCostosCompar : XtraReport
    {
        #region Variables
        private DTO_SolicitudTrabajo solicitud = new DTO_SolicitudTrabajo();
        private DTO_pyClaseProyecto _claseProyecto = new DTO_pyClaseProyecto();

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

        public Report_py_PlaneacionCostosCompar()
        {

        }

        public Report_py_PlaneacionCostosCompar(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_py_PlaneacionCostosCompar(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int? numDoc = null)
        {
            this.InitializeComponent();

            this._connection = c;
            this._transaction = tx;
            this._empresa = empresa;
            this._userID = userId;
            this._formatType = formatType;
            this.loggerConnectionStr = loggerConn;

            this.numeroDoc = numDoc;
            this.SetInitParameters();
        }

        #region Funciones publicas
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
        public void SetUserParameters()
        {
            //this.InitializeComponent();
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);

            #region Nombre del reporte

            string repName;
            string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            //string ext = this.GetExtention();

            //if (this.numeroDoc.HasValue)
            //{
            //    // Reporte de documento
            //    string fileFormat = this._moduloGlobal.GetControlValue(AppControl.NombreArchivoDocumentos);
            //    repName = string.Format(fileFormat, this.numeroDoc.ToString());
            //    docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaDocumentos);
            //}
            //else
            //{
            //    // Reporte temporal
            //    repName = Guid.NewGuid().ToString();
            //    docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            //}

            //this.ReportName = repName.ToString() + ext;
            //this.Path = filesPath + docsPath + ReportName;

            #endregion
            #region Header

            //this.lblNombreEmpresa.Text = this.Empresa.Descriptivo.Value;
            //this.lblUserName.Text = usuario.Descriptivo.Value;

            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
                this.imgLogoEmpresa.Image = logoImage;
            }
            catch { ; }

            #endregion
            #region Recusos
            this.lblPage.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblPage");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblDate");
            this.lblUser.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblUser");
            #endregion
        }

        /// <summary>
        /// Inicializa objetos y parametros iniciales
        /// </summary>
        public virtual void SetInitParameters()
        {
            //this.InitializeComponent();
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);

            #region Nombre del reporte

            //string repName;
            //string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            //string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            //string ext = this.GetExtention();

            //if (this.numeroDoc.HasValue)
            //{
            //    // Reporte de documento
            //    string fileFormat = this._moduloGlobal.GetControlValue(AppControl.NombreArchivoDocumentos);
            //    repName = string.Format(fileFormat, this.numeroDoc.ToString());
            //    docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaDocumentos);
            //}
            //else
            //{
            //    // Reporte temporal
            //    repName = Guid.NewGuid().ToString();
            //    docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            //}

            //this.ReportName = repName.ToString() + ext;
            //this.Path = filesPath + docsPath + ReportName;

            #endregion
            #region Header

            this.lblNombreEmpresa.Text = this.Empresa.Descriptivo.Value;
            this.lblUserName.Text = usuario.Descriptivo.Value;

            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
                this.imgLogoEmpresa.Image = logoImage;
            }
            catch (Exception ex)
            { }

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
        private void CreateReport()
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
        #endregion

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DTO_SolicitudTrabajo sol)
        {
            List<DTO_SolicitudTrabajo> list = new List<DTO_SolicitudTrabajo>();
            this.solicitud = sol;

            #region Asigna Valores Generales
            DTO_faCliente cliente = (DTO_faCliente)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, this.solicitud.Header.ClienteID.Value, true, false);
            this.solicitud.Header.ClienteDesc.Value = cliente != null ? cliente.Descriptivo.Value : this.solicitud.Header.EmpresaNombre.Value;
            this.solicitud.Header.Valor.Value = this.solicitud.Header.Valor.Value + this.solicitud.Header.ValorIVA.Value + this.solicitud.Header.ValorOtros.Value;

            DTO_seUsuario user = (DTO_seUsuario)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.seUsuario, this.solicitud.Header.ResponsableEMP.Value, true, false);
            this.solicitud.CorreoUsuario.Value = user != null ? user.CorreoElectronico.Value : string.Empty;
            this.solicitud.TelefonoUsuario.Value = user != null ? user.Telefono.Value : string.Empty;
            #endregion
            
            #region Asigna Otros valores segun Tipo Presupuesto
            this._claseProyecto = (DTO_pyClaseProyecto)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, this.solicitud.Header.ClaseServicioID.Value, true, false);
            if (this._claseProyecto != null && this._claseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) // Interno
            {
                string marcaxDef = this._moduloBase.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_MarcaxDef);
                this.solicitud.Detalle = this.solicitud.Detalle.OrderBy(x => x.CapituloTareaID.Value).ToList(); //Ordena por CapituloID
                int count = 1;
                foreach (var tarea in this.solicitud.Detalle)
                {
                    DTO_inReferencia refer = (DTO_inReferencia)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, tarea.TareaID.Value, true, false);
                    DTO_MasterBasic marca = (DTO_MasterBasic)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMarca, refer != null ? refer.MarcaInvID.Value : string.Empty, true, false);
                    tarea.RefProveedor.Value = refer != null ? refer.RefProveedor.Value : string.Empty;
                    if (refer != null && !refer.MarcaInvID.Value.Equals(marcaxDef))
                        tarea.MarcaInvID.Value = marca != null ? marca.Descriptivo.Value : string.Empty;
                    tarea.TareaCliente.Value = count.ToString();
                    count++;
                }
            }
            #endregion            
            #region Calcula valores del Multiplicador
            if (this._claseProyecto != null && this._claseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion && solicitud.Header.PorMultiplicadorPresup.Value != 100)//calcula el valor del multiplicador si es Proy Cliente
            {
                foreach (DTO_pyPreProyectoTarea tarea in this.solicitud.Detalle)
                {
                    tarea.CostoLocalCLI.Value = tarea.CostoLocalCLI.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                    tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                }
                foreach (DTO_pyPreProyectoTarea tarea in solicitud.DetalleTareasAdic)
                {
                    tarea.CostoLocalCLI.Value = tarea.CostoLocalCLI.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                    tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                }
            }
            #endregion
            list.Add(solicitud);

            if (this.solicitud.Header.APUIncluyeAIUInd.Value.Value)
            {
                this.tblRowAdmin.Visible = false;
                this.tblRowImpr.Visible = false;
                this.tblRowUtil.Visible = false;
                this.tblRowSubTotalInd.Visible = false;
                this.tblRowIndirectos.Visible = false;
            }

            this.DataSource = list;
            this.ShowPreview();
            return this.ReportName;
        }

        #region Eventos

        /// <summary>
        /// valida datos antes de imprimar para configurar diseño
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblCellDetalleInd_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                //Se valida si es Detalle para no resaltar
                if (this.tblCellDetalleInd.Text.Equals("True") || this.tblCellDetalleInd.Text.Equals("true"))
                    this.tblRowTareas.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                else
                {
                    this.tblRowTareas.Font = new Font("Tahoma", 8, FontStyle.Bold);
                    this.tblCellUnd.Text = string.Empty;
                    this.tblCellCant.Text = string.Empty;
                    this.tblCellCostoUnit.Text = string.Empty;

                    //Valida si es Titulo
                    if (this.tblCellNivel.Text.Equals("0"))
                    {
                        this.tblCellTarea.Text = string.Empty;
                        this.tblCellCostoTotal.Text = string.Empty;
                    }
                }
                //Valida si oculta el codigo de Tarea Cliente
                if (this.tblCellImprimirTarea.Text.Equals("False") || this.tblCellImprimirTarea.Text.Equals("false"))
                    this.tblCellTarea.Text = string.Empty;

                //Valida si resalta el item como Titulo
                if (this.tblCellTituloNeg.Text.Equals("True") || this.tblCellTituloNeg.Text.Equals("true"))
                {
                    this.tblRowTareas.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                    this.tblCellDesc.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                    //this.tblCellUnd.Text = string.Empty;
                    //this.tblCellCant.Text = string.Empty;
                    //this.tblCellCostoUnit.Text = string.Empty;
                }
                else
                {
                    this.tblRowTareas.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                    this.tblCellDesc.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// valida datos antes de imprimar para configurar diseño y totales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblCellVlrTareaCli_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            #region Declara variables
            decimal totalTareasCliente = 0;
            decimal totalTareasEmpresa = 0;
            decimal totalTareasAdicCliente = 0;
            decimal totalTareasAdicEmpresa = 0;
            //decimal vlrAdminCli = 0;
            decimal vlrAdminEmp = 0;
            //decimal vlrImprCli = 0;
            decimal vlrImprEmp = 0;
            decimal vlrUtilCli = 0;
            decimal vlrUtilEmp = 0;
            decimal vlrIVACli = 0;
            decimal vlrIVAEmp = 0;
            decimal porcIVA = 0;
            #endregion

            #region Calcula Totales
            if (this.solicitud.Detalle.Count > 0)
            {
                totalTareasCliente = this.solicitud.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value);
                totalTareasEmpresa = this.solicitud.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoTotalML.Value.Value);
            }
            if (this.solicitud.DetalleTareasAdic.Count > 0)
            {
                totalTareasAdicCliente = this.solicitud.DetalleTareasAdic.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value);
                totalTareasAdicEmpresa = this.solicitud.DetalleTareasAdic.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoTotalML.Value.Value);
            }
            if (!this.solicitud.Header.APUIncluyeAIUInd.Value.Value)
            {
                vlrAdminEmp = totalTareasEmpresa * (this.solicitud.Header.PorEmpresaADM.Value.Value / 100);
                vlrImprEmp = totalTareasEmpresa * (this.solicitud.Header.PorEmpresaIMP.Value.Value / 100);
                vlrUtilEmp = totalTareasEmpresa * (this.solicitud.Header.PorEmpresaUTI.Value.Value / 100);
                vlrUtilCli = totalTareasCliente * (this.solicitud.Header.PorEmpresaUTI.Value.Value / 100);
            }

            #endregion

            #region Asigna valores a controles
            // Total Tareas
            this.tblCellVlrTareaCli.Text = totalTareasCliente.ToString("n2");
            this.tblCellVlrTareaEmp.Text = totalTareasEmpresa.ToString("n2");
            //Otros
            this.tblCellVlrOtrosCli.Text = totalTareasAdicCliente.ToString("n2");
            this.tblCellVlrOtrosEmp.Text = totalTareasAdicEmpresa.ToString("n2");
            if (this._claseProyecto != null && this._claseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion)
            {
                //Calcula IVA por AIU
                vlrIVACli = vlrUtilCli * Convert.ToDecimal(16 * 0.01);
                vlrIVAEmp = vlrUtilEmp * Convert.ToDecimal(16 * 0.01);

                //AIU
                this.tblCellVlrAdmin.Text = vlrAdminEmp.ToString("n2");
                this.tblCellVlrImpr.Text = vlrImprEmp.ToString("n2");
                this.tblCellVlrUtil.Text = vlrUtilEmp.ToString("n2");
                this.tblCellSubTotalIndEmpres.Text = (vlrAdminEmp + vlrImprEmp + vlrUtilEmp).ToString("n2");

            }
            else
            {
                //Calcula IVA por Total
                vlrIVACli = (totalTareasCliente + totalTareasAdicCliente) * Convert.ToDecimal(16 * 0.01);
                vlrIVAEmp = (totalTareasEmpresa + totalTareasAdicEmpresa) * Convert.ToDecimal(16 * 0.01);

                this.tblRowIndirectos.Visible = false;
                this.tblRowAdmin.Visible = false;
                this.tblRowImpr.Visible = false;
                this.tblRowUtil.Visible = false;
                this.tblRowSubTotalInd.Visible = false;
            }
            //IVA
            this.tblCellVlrIVACli.Text = vlrIVACli.ToString("n2");
            this.tblCellVlrIVAEmp.Text = vlrIVAEmp.ToString("n2");
            //TOTAL
            this.tblCellTOTALCliente.Text = (totalTareasCliente + totalTareasAdicCliente + vlrAdminEmp + vlrImprEmp + vlrUtilEmp + vlrIVACli).ToString("n2");
            this.tblCellTOTALEmp.Text = (totalTareasEmpresa + totalTareasAdicEmpresa + vlrAdminEmp + vlrImprEmp + vlrUtilEmp + vlrIVAEmp).ToString("n2");
            //Diferencias
            this.tblCellVlrTareaDif.Text = (totalTareasCliente - totalTareasEmpresa).ToString("n2");
            this.tblCellVlrOtrosDif.Text = (totalTareasAdicCliente - totalTareasAdicEmpresa).ToString("n2");
            this.tblCellIVADif.Text = (vlrIVACli - vlrIVAEmp).ToString("n2");
            this.tblCellTOTALDiferencia.Text = (totalTareasCliente + totalTareasAdicCliente + vlrAdminEmp + vlrImprEmp + vlrUtilEmp + vlrIVACli -
                                               (totalTareasEmpresa + totalTareasAdicEmpresa + vlrAdminEmp + vlrImprEmp + vlrUtilEmp + vlrIVAEmp)).ToString("n2");
            #endregion
        }
        
        #endregion
    }
}
