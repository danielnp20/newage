using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Text.RegularExpressions;
using System.Threading;
using DevExpress.XtraGrid.Columns;
using SentenceTransformer;
using DevExpress.Data;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Transaccion Manual
    /// </summary>
    public partial class RecibidoNotaEnvio : DocumentForm
    {
        public RecibidoNotaEnvio()
        {
            //this.InitializeComponent();
        }

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.gcDocument.DataSource = this.data.Footer;
            this.newDoc = true;
            this.CleanHeader(true);
            this.EnableHeader(0, true);
            //this.EnableFooter(false);
            //this.CleanFooter(true);
        }
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.gcDocument.DataSource = this.data.Footer;
            this.newDoc = true;
            this.CleanHeader(true);
            this.EnableHeader(0, true);
            this.CleanHeader(true);
            this.masterBodegaOrigen.Focus();
        }

        #endregion

        #region Variables privadas
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _ctrl = null;
        private bool validHeader;
        private bool cleanDoc = true;
        private bool _txtNumberFocus;
        private DTO_MvtoInventarios data = new DTO_MvtoInventarios();
        private string actividadFlujoID = string.Empty;
        private decimal _totalUnidades = 0;
        private int _documentoNro = 0;
        private bool _bodegaDestinoPermis = true;
        private bool _bodegaOrigenPermis = true;
        private string param1xDef = string.Empty;
        private string param2xDef = string.Empty;
        private bool _copyData = false;
        //Numero de una fila segun el indice
        private int NumFila
        {
            get
            {
                return this.data.Footer.FindIndex(det => det.Movimiento.Index == this.indexFila);
            }
        }
        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Revisa si una grilla es valida o no
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidGrid()
        {
            if (this.data.Footer != null && this.data.Footer.Count == 0)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                return false;
            }
            if (!this.ValidateRow(this.gvDocument.FocusedRowHandle))
                return false;

            return true;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            base.SetInitParameters();

            this.documentID = AppDocuments.RecibidoNotaEnvio;         
            this.frmModule = ModulesPrefix.@in;          
            this.data = new DTO_MvtoInventarios();
            this._ctrl = new DTO_glDocumentoControl();
            this._ctrl.DocumentoID.Value = this.documentID;
            this.param1xDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
            this.param2xDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);

            this.AddGridCols();

            //Controles del header
            this._bc.InitMasterUC(this.masterBodegaOrigen, AppMasters.inBodega, false, true, true, true);
            this._bc.InitMasterUC(this.masterBodegaDestino, AppMasters.inBodega, false, true, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterCentroCto, AppMasters.coCentroCosto, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, true);
            this.masterBodegaDestino.EnableControl(false);
            this.masterProyecto.EnableControl(false);
            this.masterCentroCto.EnableControl(false);           
            this.tlSeparatorPanel.RowStyles[0].Height = 40;
            this.tlSeparatorPanel.RowStyles[1].Height = 100;
            this.tlSeparatorPanel.RowStyles[2].Height = 60;            
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            FormProvider.Master.itemSendtoAppr.ToolTipText = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_RecibirInd");

            #region Carga la info de las actividades
            List<string> actividades = this._bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
            if (actividades.Count != 1)
            {
                string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                MessageBox.Show(string.Format(msg, this.documentID.ToString()));
            }
            else
            {
                this.actividadFlujoID = actividades[0];
            }

            //if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
            //    this.actividadDTO = (DTO_glActividadFlujo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
            #endregion
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected  void CleanHeader(bool basic)
        {
            if (basic)
            {
                string periodo = this._bc.GetControlValueByCompany(this.frmModule, AppControl.in_Periodo);
                this.dtPeriod.Text = periodo;
                this.txtNumber.Text = string.Empty;
            }
            this.masterBodegaOrigen.Value = string.Empty;
            this.masterBodegaDestino.Value = string.Empty;
            this.txtCedula.Text = string.Empty;
            this.txtConductor.Text = string.Empty;
            this.txtPlacaVeh.Text = string.Empty;
            this.txtTipoVeh.Text = string.Empty;
            this.txtTelConductor.Text = string.Empty;
            this.txtObservacion.Text = string.Empty;
            this.masterCentroCto.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterPrefijo.Value = string.Empty;
            this.masterBodegaOrigen.Focus();
        }

        /// <summary>
        /// Deshabilita los controles del header
        /// </summary>
        protected  void EnableHeader(short TipoMov, bool basic)
        {
            this.grpboxHeader.Enabled = true;
            this.masterBodegaOrigen.EnableControl(!basic);
            this.masterPrefijo.EnableControl(!basic);
            this.txtNumber.Enabled = !basic;
            this.btnQueryDoc.Enabled = !basic; 
            if (basic)
            {
                //if (TipoMov != 0)
                //    this.masterBodegaOrigen.EnableControl(false);
            }
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected Object LoadTempHeader()
        {
            DTO_inMovimientoDocu header = new DTO_inMovimientoDocu();
            header.BodegaOrigenID.Value = this.masterBodegaOrigen.Value;
            header.BodegaDestinoID.Value = this.masterBodegaDestino.Value;
            header.EmpresaID.Value = this.empresaID;
            header.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            header.DatoAdd2.Value = this.txtTipoVeh.Text;
            header.DatoAdd3.Value = this.txtPlacaVeh.Text;
            header.DatoAdd4.Value = this.txtConductor.Text;
            header.DatoAdd5.Value = this.txtCedula.Text;
            header.DatoAdd6.Value = this.txtTelConductor.Text;
            header.Observacion.Value = this.txtObservacion.Text;

            //this._ctrl.TerceroID.Value = this.masterCliente.Value;
            this._ctrl.NumeroDoc.Value = this.data.DocCtrl.NumeroDoc.Value != null ? this.data.DocCtrl.NumeroDoc.Value : 0 ;
            this._ctrl.ProyectoID.Value = this.masterProyecto.Value;
            this._ctrl.CentroCostoID.Value = this.masterCentroCto.Value;
            //this._ctrl.PrefijoID.Value = this.bodegaOrigen != null ? this.bodegaOrigen.PrefijoID.Value : this.txtPrefix.Text;
            this._ctrl.Fecha.Value = DateTime.Now;            
            this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
            //this._ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            //this._ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            this._ctrl.DocumentoID.Value = this.documentID;
            this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
            this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
            this._ctrl.seUsuarioID.Value = this.userID;
            this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
            this._ctrl.ConsSaldo.Value = 0;
            this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
            this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
            this._ctrl.Descripcion.Value = base.txtDocDesc.Text;
            this._ctrl.Valor.Value = 0;
            this._ctrl.Iva.Value = 0;
            //this._ctrl.UsuarioRESP.Value = this._bc.AdministrationModel.User.ID.Value;

            DTO_MvtoInventarios mvto = new DTO_MvtoInventarios();
            mvto.Header = header;
            mvto.DocCtrl = this._ctrl;
            mvto.Footer = new List<DTO_inMovimientoFooter>();
           
            return mvto;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected  bool ValidateHeader()
        {
            bool result = true;
            if (!this.masterBodegaOrigen.ValidID )
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterBodegaOrigen.CodeRsx);

                MessageBox.Show(msg);
                this.masterBodegaOrigen.Focus();
                result = false;
            }
            if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();
                result = false;
            }
            if (string.IsNullOrEmpty(this.txtNumber.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblNumber.Text);

                MessageBox.Show(msg);
                this.txtNumber.Focus();
                result = false;
            }
             return result;
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Columnas basicas

                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "RecibirInd";
                aprob.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_RecibirInd");
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 60;
                aprob.Visible = true;
                aprob.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(aprob);

                //Rechazar
                GridColumn noAprob = new GridColumn();
                noAprob.FieldName = this.unboundPrefix + "DevolverInd";
                noAprob.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_DevolverInd");
                noAprob.UnboundType = UnboundColumnType.Boolean;
                noAprob.VisibleIndex = 1;
                noAprob.Width = 60;
                noAprob.Visible = true;
                noAprob.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(noAprob);

                //CodigoReferencia+Param1+Param2
                GridColumn codRefP1P2 = new GridColumn();
                codRefP1P2.FieldName = this.unboundPrefix + "ReferenciaIDP1P2";
                codRefP1P2.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRefP1P2.UnboundType = UnboundColumnType.String;
                codRefP1P2.VisibleIndex = 2;
                codRefP1P2.Width = 120;
                codRefP1P2.Visible = true;
                codRefP1P2.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codRefP1P2);

                //Estado
                GridColumn estado = new GridColumn();
                estado.FieldName = this.unboundPrefix + "EstadoInv";
                estado.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_EstadoInv");
                estado.UnboundType = UnboundColumnType.Integer;
                estado.VisibleIndex = 3;
                estado.Width = 110;
                estado.Visible = true;
                estado.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(estado);

                //Descripcion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "DescripTExt";
                desc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescripTExt");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 4;
                desc.Width = 110;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(desc);

                //SerialID
                GridColumn serial = new GridColumn();
                serial.FieldName = this.unboundPrefix + "SerialID";
                serial.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serial.UnboundType = UnboundColumnType.String;
                serial.VisibleIndex = 5;
                serial.Width = 110;
                serial.Visible = true;
                serial.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(serial);

                //docSoporte
                GridColumn docSoporte = new GridColumn();
                docSoporte.FieldName = this.unboundPrefix + "DocSoporte";
                docSoporte.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocSoporte");
                docSoporte.UnboundType = UnboundColumnType.Integer;
                docSoporte.VisibleIndex = 6;
                docSoporte.Width = 110;
                docSoporte.Visible = true;
                docSoporte.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(docSoporte);

                //EmpaqueInvID
                GridColumn empaqueID = new GridColumn();
                empaqueID.FieldName = this.unboundPrefix + "EmpaqueInvID";
                empaqueID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpaqueInvID");
                empaqueID.UnboundType = UnboundColumnType.String;
                empaqueID.VisibleIndex = 7;
                empaqueID.Width = 70;
                empaqueID.Visible = true;
                empaqueID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(empaqueID);

                //CantidadEmpaques
                GridColumn cantidadEmpaques = new GridColumn();
                cantidadEmpaques.FieldName = this.unboundPrefix + "CantidadEMP";
                cantidadEmpaques.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadEMP");
                cantidadEmpaques.UnboundType = UnboundColumnType.Integer;
                cantidadEmpaques.VisibleIndex = 8;
                cantidadEmpaques.Width = 110;
                cantidadEmpaques.Visible = true;
                cantidadEmpaques.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cantidadEmpaques);

                //CantidadUNI
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefix + "CantidadUNI";
                cant.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadUNI");
                cant.UnboundType = UnboundColumnType.Decimal;
                cant.VisibleIndex = 9;
                cant.Width = 110;
                cant.Visible = true;
                cant.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cant);

                //cantidadTraslado
                GridColumn cantidadTraslado = new GridColumn();
                cantidadTraslado.FieldName = this.unboundPrefix + "CantidadTraslado";
                cantidadTraslado.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadTraslado");
                cantidadTraslado.UnboundType = UnboundColumnType.Decimal;
                cantidadTraslado.VisibleIndex = 9;
                cantidadTraslado.Width = 110;
                cantidadTraslado.Visible = true;
                cantidadTraslado.OptionsColumn.AllowEdit = true;
                cantidadTraslado.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                cantidadTraslado.AppearanceCell.Options.UseTextOptions = true;
                cantidadTraslado.AppearanceCell.Options.UseFont = true;
                this.gvDocument.Columns.Add(cantidadTraslado);

                #endregion
                #region Columnas No Visibles

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.UnboundType = UnboundColumnType.String;
                param1.Visible = false;
                this.gvDocument.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";
                param2.UnboundType = UnboundColumnType.String;
                param2.Visible = false;
                this.gvDocument.Columns.Add(param2);

                //Unidad
                GridColumn unidadRef = new GridColumn();
                unidadRef.FieldName = this.unboundPrefix + "UnidadRef";
                unidadRef.UnboundType = UnboundColumnType.String;
                unidadRef.Visible = false;
                this.gvDocument.Columns.Add(unidadRef);

                //IdentificadorTr
                GridColumn param3 = new GridColumn();
                param3.FieldName = this.unboundPrefix + "IdentificadorTr";
                param3.UnboundType = UnboundColumnType.Integer;
                param3.Visible = false;
                this.gvDocument.Columns.Add(param3);

                //ValorUnitario
                GridColumn vlrUnitario = new GridColumn();
                vlrUnitario.FieldName = this.unboundPrefix + "ValorUNI";
                vlrUnitario.UnboundType = UnboundColumnType.Decimal;
                vlrUnitario.Visible = false;
                this.gvDocument.Columns.Add(vlrUnitario);



                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-RecibidoNotaEnvio.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            this.gcDocument.DataSource = this.data.Footer;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this.data.Footer.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDocument.MoveFirst();
            this.dataLoaded = true;
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Obtiene un movimiento de Inventarios
        /// </summary>
        private void GetMvtoInventario(DTO_glDocumentoControl ctrl)
        {
            DTO_seUsuarioBodega userBodega = null;
            List<DTO_inMovimientoFooter> footerNew = new List<DTO_inMovimientoFooter>();

            DTO_MvtoInventarios saldoCostos = this._bc.AdministrationModel.Transaccion_Get(this.documentID, ctrl.NumeroDoc.Value.Value, true);
            if (saldoCostos.Header.BodegaOrigenID.Value != this.masterBodegaOrigen.Value)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberxBodega));
                this.masterBodegaOrigen.Focus();
                return;
            }
            if (this._copyData)
            {
                saldoCostos.DocCtrl.NumeroDoc.Value = 0;
                saldoCostos.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                saldoCostos.Header.NumeroDoc.Value = 0;
            }
            #region Permisos Bodega Destino
            Dictionary<string, string> keysUserBodega = new Dictionary<string, string>();
            keysUserBodega.Add("seUsuarioID", this.userID.ToString());
            keysUserBodega.Add("BodegaID", saldoCostos.Header.BodegaDestinoID.Value);
            userBodega = (DTO_seUsuarioBodega)this._bc.GetMasterComplexDTO(AppMasters.seUsuarioBodega, keysUserBodega, true);
            if (userBodega != null && !userBodega.EntradaInd.Value.Value)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UserNotPermittedIN) + " :" + saldoCostos.Header.BodegaDestinoID.Value.ToString());
                this.gvDocument.Columns[this.unboundPrefix + "RecibirInd"].OptionsColumn.AllowEdit = false;
                this._bodegaDestinoPermis = false;
                return;
            }
            else
            {
                this.gvDocument.Columns[this.unboundPrefix + "RecibirInd"].OptionsColumn.AllowEdit = true;
                this._bodegaDestinoPermis = true;
            }
            #endregion
            #region Permisos Bodega Origen
            keysUserBodega = new Dictionary<string, string>();
            keysUserBodega.Add("seUsuarioID", this.userID.ToString());
            keysUserBodega.Add("BodegaID", saldoCostos.Header.BodegaOrigenID.Value);
            userBodega = (DTO_seUsuarioBodega)this._bc.GetMasterComplexDTO(AppMasters.seUsuarioBodega, keysUserBodega, true);
            if (userBodega != null && !userBodega.EntradaInd.Value.Value)
            {
                this._bodegaOrigenPermis = false;
                this.gvDocument.Columns[this.unboundPrefix + "DevolverInd"].OptionsColumn.AllowEdit = false;
            }
            else
            {
                this._bodegaOrigenPermis = true;
                this.gvDocument.Columns[this.unboundPrefix + "DevolverInd"].OptionsColumn.AllowEdit = true;
            }

            #endregion
            #region Asigna valores de la Nota Envio Existente
            this.masterBodegaDestino.Value = saldoCostos.Header.BodegaDestinoID.Value;
            this.txtTipoVeh.Text = saldoCostos.Header.DatoAdd2.Value;
            this.txtPlacaVeh.Text = saldoCostos.Header.DatoAdd3.Value;
            this.txtConductor.Text = saldoCostos.Header.DatoAdd4.Value;
            this.txtCedula.Text = saldoCostos.Header.DatoAdd5.Value;
            this.txtTelConductor.Text = saldoCostos.Header.DatoAdd6.Value;
            this.txtObservacion.Text = saldoCostos.Header.Observacion.Value;
            this.dtFecha.DateTime = saldoCostos.DocCtrl.FechaDoc.Value.Value;
            this.masterProyecto.Value = ctrl.ProyectoID.Value;
            this.masterCentroCto.Value = ctrl.CentroCostoID.Value;
            this._documentoNro = ctrl.DocumentoNro.Value.Value;

            this.data = saldoCostos;
            this._totalUnidades = 0;
            foreach (var footer in this.data.Footer)
            {
                #region Consulta los saldos de cada referencia
                DTO_inCostosExistencias costos = new DTO_inCostosExistencias();
                DTO_inControlSaldosCostos saldos = new DTO_inControlSaldosCostos();
                saldos.inReferenciaID.Value = footer.Movimiento.inReferenciaID.Value;
                saldos.IdentificadorTr.Value = footer.Movimiento.NumeroDoc.Value.Value;
                saldos.Parametro1.Value = footer.Movimiento.Parametro1.Value;
                saldos.Parametro2.Value = footer.Movimiento.Parametro2.Value;
                saldos.EstadoInv.Value = footer.Movimiento.EstadoInv.Value.Value;
                saldos.ActivoID.Value = footer.Movimiento.ActivoID.Value.Value;
                decimal cantidadDisp = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(this.documentID, saldos, ref costos);
                #endregion
                if (cantidadDisp != 0)
                {
                    DTO_inReferencia referencia = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, footer.Movimiento.inReferenciaID.Value, true);
                    footer.UnidadRef.Value = referencia.UnidadInvID.Value;
                    this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
                    footer.RecibirInd.Value = this._bodegaDestinoPermis;
                    footer.DevolverInd.Value = false;
                    footer.Movimiento.EntradaSalida.Value = (byte)EntradaSalida.Salida;
                    footer.Movimiento.CantidadUNI.Value = cantidadDisp;
                    footer.Movimiento.Valor1LOC.Value = costos.CtoLocSaldoIni.Value + costos.CtoLocEntrada.Value - costos.CtoLocSalida.Value;
                    footer.Movimiento.Valor2LOC.Value = costos.FobLocSaldoIni.Value + costos.FobLocEntrada.Value - costos.FobLocSalida.Value;
                    footer.Movimiento.Valor1EXT.Value = costos.CtoExtSaldoIni.Value + costos.CtoExtEntrada.Value - costos.CtoExtSalida.Value;
                    footer.Movimiento.Valor2EXT.Value = costos.FobExtSaldoIni.Value + costos.FobExtEntrada.Value - costos.FobExtSalida.Value;
                    footer.CantidadTraslado.Value = footer.Movimiento.CantidadUNI.Value.Value;
                    footerNew.Add(footer);
                }
            }
            this.data.Footer = footerNew;
            #endregion
            this.LoadData(true);
            this.validHeader = true;
            this.newDoc = false;
            this.EnableHeader((byte)TipoMovimientoInv.Traslados, true);
            FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);         
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Valida que el numero ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumber_Leave(object sender, EventArgs e)
        {
            if (this._txtNumberFocus)
            {
                this._txtNumberFocus = false;

                if (this.txtNumber.Text == string.Empty)
                    this.txtNumber.Text = "0";

                if (this.txtNumber.Text == "0")
                {
                    #region Nueva transaccion
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    this.gcDocument.DataSource = null;
                    #endregion
                }
                else
                {
                    #region Nota de Envio existente
                    try
                    {
                        DTO_glDocumentoControl docCtrlExist = null;
                        docCtrlExist = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.NotaEnvio, this.masterPrefijo.Value, Convert.ToInt32(this.txtNumber.Text));
   
                        #region Si existe el documento
                        if (docCtrlExist != null)
                            this.GetMvtoInventario(docCtrlExist);
                        else
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberTransaccion));
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-RecibidoNotaEnvio.cs", "txtNumber_Leave"));
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado un comprobante existente
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumber_Enter(object sender, EventArgs e)
        {
            if (this.masterPrefijo.ValidID)
            {
                this._txtNumberFocus = true;
                if(this.txtNumber.Text == string.Empty)
                    this.txtNumber.Text = "0";  
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            if (this.masterBodegaOrigen.ValidID)
            {
                List<int> docs = new List<int>();
                docs.Add(AppDocuments.NotaEnvio);
                ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    if (getDocControl.CopiadoInd)
                        this._copyData = true;
                    this.GetMvtoInventario(getDocControl.DocumentoControl);
                }
            }
            else
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidBodegaOrigen));
        }
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            if (this.ValidateHeader())
                this.validHeader = true;
            else
                this.validHeader = false;
            if (this.validHeader)
            {
                if (this.txtNumber.Text == "0")
                {
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }
                #region Si entra al detalle y no tiene datos
                this.EnableHeader(-1, true);
                DTO_MvtoInventarios saldoCosto = new DTO_MvtoInventarios();
                try
                {
                    if (this.data == null || this.data.Footer.Count == 0)
                    {
                        saldoCosto = (DTO_MvtoInventarios)this.LoadTempHeader();
                        this.data = saldoCosto;

                        this.LoadData(true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-RecibidoNotaEnvio.cs", "gcDocument_Enter: " + ex.Message));
                }

                #endregion
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                        else
                        {
                            DTO_inMovimientoFooter dtoM = (DTO_inMovimientoFooter)e.Row;
                            pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM.Movimiento, null);
                                else
                                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoM.Movimiento, null), null);
                            }
                            else
                            {
                                fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                        e.Value = fi.GetValue(dtoM.Movimiento);
                                    else
                                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoM.Movimiento), null);
                                }
                            }
                        }
                    }
                }
            }

            if (e.IsSetData)
            {
                if (fieldName == "Marca")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            {
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                        else
                        {
                            DTO_inMovimientoFooter dtoM = (DTO_inMovimientoFooter)e.Row;
                            pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM.Movimiento, null);
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoM.Movimiento, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    {
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoM.Movimiento);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las +columnas normales
                GridColumn col = this.gvDocument.Columns[e.Column.FieldName];
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "RecibirInd")
                {
                    if ((bool)e.Value)
                        this.data.Footer[e.RowHandle].DevolverInd.Value = false;
                }
                else if (fieldName == "DevolverInd")
                {
                    if ((bool)e.Value && this._bodegaOrigenPermis)                        
                        this.data.Footer[e.RowHandle].RecibirInd.Value = false;
                }
                else if (fieldName == "CantidadTraslado")
                {
                   decimal num = Convert.ToDecimal(e.Value);
                   if (num <= this.data.Footer[e.RowHandle].Movimiento.CantidadUNI.Value)
                   {
                       this.data.Footer[e.RowHandle].CantidadTraslado.Value = Convert.ToDecimal(e.Value);
                       this.data.Footer[e.RowHandle].Movimiento.Valor1LOC.Value = this.data.Footer[e.RowHandle].Movimiento.ValorUNI.Value*num;
                       this.data.Footer[e.RowHandle].Movimiento.Valor2LOC.Value = (this.data.Footer[e.RowHandle].Movimiento.Valor2LOC.Value/
                                                                                   this.data.Footer[e.RowHandle].Movimiento.CantidadUNI.Value) * num;
                       this.data.Footer[e.RowHandle].Movimiento.Valor1EXT.Value = (this.data.Footer[e.RowHandle].Movimiento.Valor1EXT.Value /
                                                                                   this.data.Footer[e.RowHandle].Movimiento.CantidadUNI.Value) * num;
                       this.data.Footer[e.RowHandle].Movimiento.Valor2EXT.Value = (this.data.Footer[e.RowHandle].Movimiento.Valor2EXT.Value /
                                                                                   this.data.Footer[e.RowHandle].Movimiento.CantidadUNI.Value) * num;
                   }
                   else
                   {
                       MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_TraslateQuantityInvalid));
                       this.data.Footer[e.RowHandle].CantidadTraslado.Value = this.data.Footer[e.RowHandle].Movimiento.CantidadUNI.Value;
                   }                    
                }
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-RecibidoNotaEnvio.cs", "gvDocument_CellValueChanged: " + ex.Message));
            }
        }

        #endregion

        #region Eventos del MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemSave.Visible = false;
        }

        #endregion

        #region Eventos Barra de Herramientas
        
        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                base.TBNew();
                if (this.cleanDoc)
                {
                    this.data = new DTO_MvtoInventarios();
                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.data.Footer;
                    this.disableValidate = false;
                    this._documentoNro = 0;                    
                    this.EnableHeader(0,false);
                    this.CleanHeader(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-RecibidoNotaEnvio.cs", "TBNew: " + ex.Message));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                base.TBSave();
                this.gvDocument.PostEditor();
                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.ValidGrid())
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-RecibidoNotaEnvio.cs", "TBSave: " + ex.Message));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {                
                this.gvDocument.PostEditor();
                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.ValidGrid())
                {
                    Thread process = new Thread(this.RecibirDevolverThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-RecibidoNotaEnvio.cs", "TBSendtoAppr: " + ex.Message));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Enviar para Recibir o devolver movimientos de inventarios
        /// </summary>
        private void RecibirDevolverThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoInventarios(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_SerializedObject obj = this._bc.AdministrationModel.NotaEnvio_RecibirDevolver(this.documentID, this.data, this.actividadFlujoID, false);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    this.data = new DTO_MvtoInventarios();
                    this.Invoke(this.sendToApproveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-RecibidoNotaEnvio.cs", "TBSendtoAppr: " + ex.Message));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        } 

        #endregion

    }
}
