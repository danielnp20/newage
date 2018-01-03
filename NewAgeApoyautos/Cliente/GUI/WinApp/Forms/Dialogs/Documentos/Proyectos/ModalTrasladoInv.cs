using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Configuration;
using SentenceTransformer;
using System.Collections;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalTrasladoInv : Form
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int _documentID;
        private string _unboundPrefix = "Unbound_";
        //Variables de data
        private DTO_inMovimientoFooter _rowCurrent = new DTO_inMovimientoFooter();
        private string _bodegaIni = string.Empty;
        private string _bodegaFin = string.Empty;
        private string _proyectoID = string.Empty;
        List<DTO_prSolicitudResumen> _solicitudesProv = new List<DTO_prSolicitudResumen>();
        List<DTO_inMovimientoFooter> _detalleInv = new List<DTO_inMovimientoFooter>();
        List<DTO_inMovimientoFooter> _detalleInvExist = new List<DTO_inMovimientoFooter>();
        List<DTO_glMovimientoDeta> _detSaldosProyectoFacVenta = new List<DTO_glMovimientoDeta>();
        private string _empaqueInvIdDef = string.Empty;
        private decimal _tasaCambio = 0;

        #endregion

        #region Propiedades

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public List<DTO_inMovimientoFooter> DetalleSelected
        {
            get { return _detalleInv; }
        }

        #endregion

        /// <summary>
       /// Constructor
       /// </summary>
       /// <param name="bodegaIni">Bodega para consultar las existencias</param>
        public ModalTrasladoInv(List<DTO_inMovimientoFooter> detalleExist, string bodegaIni,string bodegaFin, decimal tasaCambio,string proyectoID)
        {
            this.InitializeComponent();
            try
            {
                this._bodegaIni = bodegaIni;
                this._bodegaFin = bodegaFin;
                this._proyectoID = proyectoID;
                this._tasaCambio = tasaCambio;
                this._detalleInvExist = detalleExist;
                this.SetInitParameters();
                this.AddGridCols();   
                this.rbtTipoTraslado.SelectedIndex = 0;                      
                FormProvider.LoadResources(this, this._documentID);
                this.Text = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_frmTraslados");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalTrasladoInv.cs", "ModalTrasladoInv: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppForms.ModalSolicitudesForm;
            #region Inicializa Controles
            this._bc.InitMasterUC(this.masterBodegaFin, AppMasters.inBodega, true, true, false, false);
            this._bc.InitMasterUC(this.masterBodegaIni, AppMasters.inBodega, true, true, false, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, false, false);
            this.masterBodegaIni.EnableControl(false);
            this.masterBodegaFin.EnableControl(false);
            this.masterBodegaIni.Value = this._bodegaIni;
            this.masterBodegaFin.Value = this._bodegaFin;
            this.masterProyecto.Value = this._proyectoID;
            #endregion            
            this._empaqueInvIdDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_EmpaquexDef);

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            //Campo de marca
            GridColumn sel = new GridColumn();
            sel.FieldName = this._unboundPrefix + "SelectInd";
            sel.Caption = "√";
            sel.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Seleccionar");
            sel.AppearanceHeader.ForeColor = Color.Lime;
            sel.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sel.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            sel.AppearanceHeader.Options.UseTextOptions = true;
            sel.AppearanceHeader.Options.UseFont = true;
            sel.AppearanceHeader.Options.UseForeColor = true;
            sel.UnboundType = UnboundColumnType.Boolean;
            sel.VisibleIndex = 0;
            sel.Width = 20;
            sel.Fixed = FixedStyle.Left;
            sel.Visible = true;
            sel.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(sel);

            GridColumn prefDoc = new GridColumn();
            prefDoc.FieldName = this._unboundPrefix + "PrefDoc";
            prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_PrefDoc");
            prefDoc.UnboundType = UnboundColumnType.String;
            prefDoc.VisibleIndex = 1;
            prefDoc.Width = 55;
            prefDoc.Visible = false;
            prefDoc.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(prefDoc);

            //ProyectoID
            GridColumn ProyectoID = new GridColumn();
            ProyectoID.FieldName = this._unboundPrefix + "ProyectoID";
            ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_ProyectoID");
            ProyectoID.UnboundType = UnboundColumnType.String;
            ProyectoID.VisibleIndex = 2;
            ProyectoID.Width = 50;
            ProyectoID.Visible = false;
            ProyectoID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(ProyectoID);

            GridColumn DatoAdd5 = new GridColumn();
            DatoAdd5.FieldName = this._unboundPrefix + "DatoAdd5";
            DatoAdd5.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DatoAdd4");
            DatoAdd5.UnboundType = UnboundColumnType.String;
            DatoAdd5.VisibleIndex = 3;
            DatoAdd5.Width = 50;
            DatoAdd5.Visible = false;
            DatoAdd5.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DatoAdd5);

            GridColumn fecha = new GridColumn();
            fecha.FieldName = this._unboundPrefix + "FechaSol";
            fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_FechaSol");
            fecha.UnboundType = UnboundColumnType.String;
            fecha.VisibleIndex = 4;
            fecha.Width = 50;
            fecha.Visible = false;
            fecha.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(fecha);

            //CodigoServicios
            GridColumn codBS = new GridColumn();
            codBS.FieldName = this._unboundPrefix + "CodigoBSID";
            codBS.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CodigoBSID");
            codBS.UnboundType = UnboundColumnType.String;
            codBS.VisibleIndex = 5;
            codBS.Width = 50;
            codBS.Visible = false;
            codBS.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(codBS);

            //CodigoReferencia
            GridColumn codRef = new GridColumn();
            codRef.FieldName = this._unboundPrefix + "inReferenciaID";
            codRef.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_inReferenciaID");
            codRef.UnboundType = UnboundColumnType.String;
            codRef.VisibleIndex = 6;
            codRef.Width = 55;
            codRef.Visible = true;
            codRef.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(codRef);

            //DescripTExt
            GridColumn DescripTExt = new GridColumn();
            DescripTExt.FieldName = this._unboundPrefix + "DescripTExt";
            DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Descriptivo");
            DescripTExt.UnboundType = UnboundColumnType.String;
            DescripTExt.VisibleIndex = 7;
            DescripTExt.Width = 240;
            DescripTExt.Visible = true;
            DescripTExt.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DescripTExt);

            //MarcaDesc
            GridColumn MarcaDesc = new GridColumn();
            MarcaDesc.FieldName = this._unboundPrefix + "MarcaDesc";
            MarcaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_MarcaInvID");
            MarcaDesc.UnboundType = UnboundColumnType.String;
            MarcaDesc.VisibleIndex = 8;
            MarcaDesc.Width = 35;
            MarcaDesc.Visible = true;
            MarcaDesc.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(MarcaDesc);

            //RefProveedor
            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this._unboundPrefix + "RefProveedor";
            RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.VisibleIndex = 9;
            RefProveedor.Width = 35;
            RefProveedor.Visible = true;
            RefProveedor.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(RefProveedor);

            //UnidadRef
            GridColumn UnidadRef = new GridColumn();
            UnidadRef.FieldName = this._unboundPrefix + "UnidadRef";
            UnidadRef.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_UnidadInvID");
            UnidadRef.UnboundType = UnboundColumnType.String;
            UnidadRef.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadRef.AppearanceCell.Options.UseTextOptions = true;
            UnidadRef.VisibleIndex = 10;
            UnidadRef.Width = 35;
            UnidadRef.Visible = true;
            UnidadRef.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(UnidadRef);

            //OrigenMonetario
            GridColumn OrigenMonetario = new GridColumn();
            OrigenMonetario.FieldName = this._unboundPrefix + "OrigenMonetario";
            OrigenMonetario.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_OrigenMonetario");
            OrigenMonetario.UnboundType = UnboundColumnType.String;
            OrigenMonetario.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            OrigenMonetario.AppearanceCell.Options.UseTextOptions = true;
            OrigenMonetario.VisibleIndex = 11;
            OrigenMonetario.Width = 35;
            OrigenMonetario.Visible = false;
            OrigenMonetario.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(OrigenMonetario);

            //CantidadDisp
            this.editValue.Mask.EditMask = "n3";
            GridColumn cantidadDisp = new GridColumn();
            cantidadDisp.FieldName = this._unboundPrefix + "CantidadDispon";
            cantidadDisp.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CantidadDispon");
            cantidadDisp.UnboundType = UnboundColumnType.Decimal;
            cantidadDisp.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            cantidadDisp.AppearanceCell.Options.UseTextOptions = true;
            cantidadDisp.VisibleIndex = 12;
            cantidadDisp.Width = 60;
            cantidadDisp.Visible = true;
            cantidadDisp.ColumnEdit = this.editValue;
            cantidadDisp.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(cantidadDisp);

            //CantidadPendiente en Solicitud
            GridColumn cantidadSol = new GridColumn();
            cantidadSol.FieldName = this._unboundPrefix + "CantidadPendiente";
            cantidadSol.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CantidadSol");
            cantidadSol.UnboundType = UnboundColumnType.Decimal;
            cantidadSol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            cantidadSol.AppearanceCell.Options.UseTextOptions = true;
            cantidadSol.VisibleIndex = 13;
            cantidadSol.Width = 60;
            cantidadSol.Visible = false;
            cantidadSol.ColumnEdit = this.editValue;
            cantidadSol.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(cantidadSol);

            //CantidadEMP
            GridColumn CantidadEMP = new GridColumn();
            CantidadEMP.FieldName = this._unboundPrefix + "CantidadEMP";
            CantidadEMP.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CantidadEMP");
            CantidadEMP.UnboundType = UnboundColumnType.Decimal;
            CantidadEMP.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadEMP.AppearanceCell.Options.UseTextOptions = true;
            CantidadEMP.VisibleIndex = 14;
            CantidadEMP.Width = 60;
            CantidadEMP.Visible = true;
            CantidadEMP.ColumnEdit = this.editValue;
            CantidadEMP.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(CantidadEMP);

            this.gvDetalle.OptionsView.ColumnAutoWidth = true;
        
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadExistencias()
        {
            try
            {  
                if (!string.IsNullOrEmpty(this._bodegaIni))
                {
                    string parametro1 = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                    string parametro2 = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                    string empresa = this._bc.AdministrationModel.Empresa.ID.Value;
                    List<DTO_inControlSaldosCostos> listSaldosCostos = new List<DTO_inControlSaldosCostos>();
                    this._detalleInv = new List<DTO_inMovimientoFooter>();
                    DTO_inControlSaldosCostos saldos = new DTO_inControlSaldosCostos();                  
                    
                }
                this.gvDetalle.MoveFirst();
                this.gcDetalle.DataSource = null;
                this.gcDetalle.DataSource = this._detalleInv;
                this.gcDetalle.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTrasladoInv.cs", "ModalTrasladoInv(" + this._documentID + ")-LoadGridData: " + ex.Message));
            }
        }

        /// <summary>
        /// Carga las solicitudes de proveedores
        /// </summary>
        private void LoadSolicitudesProv()
        {
        }

        /// <summary>
        /// Carga las facturas de venta
        /// </summary>
        private void LoadDetalleFacturaVenta()
        {
            DateTime periodoInv = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo));
            this._detSaldosProyectoFacVenta = this._bc.AdministrationModel.glMovimientoDetaPRE_GetSaldosInvByProyecto(periodoInv, this._proyectoID,false);
            this._detSaldosProyectoFacVenta = this._detSaldosProyectoFacVenta.FindAll(t => t.CantidadDispon.Value > 0).OrderBy(x => x.Descriptivo.Value).ToList();

            foreach (DTO_glMovimientoDeta mov in _detSaldosProyectoFacVenta)
            {
                DTO_faFacturacionFooter footerDet = new DTO_faFacturacionFooter();

                #region Asigna datos a la fila
                mov.NroItem.Value = 0;
                mov.Valor1LOC.Value = 0;
                mov.Valor2LOC.Value = 0;
                mov.Valor1EXT.Value = 0;
                mov.Valor2EXT.Value = 0;
                mov.ValorUNI.Value = 0;
                footerDet.Movimiento.ImprimeInd.Value = false;
                footerDet.SelectInd.Value = false;
                #endregion
            }

        }

        /// <summary>
        /// Valida la data digitada
        /// </summary>
        private void ValidData(DTO_inMovimientoFooter row, decimal cantidad, GridColumn col)
        {
            decimal? cantTot = this._detalleInv.FindAll(x => x.Movimiento.BodegaID.Value == row.Movimiento.BodegaID.Value && x.Movimiento.inReferenciaID.Value == row.Movimiento.inReferenciaID.Value).Sum(x => x.Movimiento.CantidadEMP.Value);

            row.CantidadPendiente.Value = row.CantidadPendiente.Value ?? 0;
            if (row.SelectInd.Value.Value && (cantTot > row.CantidadDispon.Value || cantTot < 0))
                this.gvDetalle.SetColumnError(col, "Cantidad inválida");
            else if (row.SelectInd.Value.Value && cantTot == 0)
                this.gvDetalle.SetColumnError(col, "Debe digitar una cantidad diferente de 0 si selecciona este ítem");
            else
            {
                this.gvDetalle.SetColumnError(col, string.Empty);
                #region Realiza la conversion de unidad si es necesario
                DTO_inReferencia referenciaInv = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, row.Movimiento.inReferenciaID.Value, true);
                DTO_inEmpaque empaqueEmp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, referenciaInv.EmpaqueInvID.Value, true);
                row.Movimiento.EmpaqueInvID.Value = referenciaInv.EmpaqueInvID.Value;
                if (empaqueEmp != null && empaqueEmp.UnidadInvID.Value == referenciaInv.UnidadInvID.Value)
                    row.Movimiento.CantidadUNI.Value = cantidad * empaqueEmp.Cantidad.Value.Value;
                else
                {
                    if (!empaqueEmp.UnidadInvID.Value.Equals(this._empaqueInvIdDef))
                    {
                        Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                        keysConvert.Add("UnidadInvID", empaqueEmp.UnidadInvID.Value);
                        keysConvert.Add("UnidadBase", referenciaInv.UnidadInvID.Value);
                        DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                        if (conversion != null)
                            row.Movimiento.CantidadUNI.Value = (conversion.Factor.Value.Value * cantidad * empaqueEmp.Cantidad.Value.Value);
                        else
                            row.Movimiento.CantidadUNI.Value = cantidad * empaqueEmp.Cantidad.Value.Value;
                    }
                    else
                        row.Movimiento.CantidadUNI.Value = cantidad * empaqueEmp.Cantidad.Value.Value;
                }
                #endregion
                row.Movimiento.Valor1LOC.Value = row.Movimiento.ValorUNI.Value * row.Movimiento.CantidadUNI.Value;
                #region Realiza la conversion de moneda si es necesario
                if (this._tasaCambio != 0)
                    row.Movimiento.Valor1EXT.Value = row.Movimiento.Valor1LOC.Value / this._tasaCambio;
                #endregion
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!this.gvDetalle.HasColumnErrors)
                //{
                //    this._currentData.ForEach(a =>
                //    {
                //        if (a.Selected.Value.Value)
                //        {
                //            if (a.CantidadTraslado.Value > 0 && a.CantidadTraslado.Value <= a.CantidadSol.Value)
                //            {
                //                this.ReturnList.Add(a);
                //            }
                //            else
                //            {
                //                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_InvalidSolCantidad) + "-" + a.Descriptivo.Value;
                //                MessageBox.Show(string.Format(msg, a.PrefDoc.ToString()));
                //            }
                //        }
                //    });

                //foreach (DTO_prSolicitudResumen exist in ReturnExistente)
                //{
                //    if (!this.ReturnList.Exists(x => x.inReferenciaID.Value == exist.inReferenciaID.Value && x.CodigoBSID.Value == exist.CodigoBSID.Value))
                //        this.ReturnList.Add(exist);
                //}

                 this.Close();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTrasladoInv.cs", "btnReturn_Click"));
            }           
        }

        /// <summary>
        /// Al hacer click para cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._detalleInv = new List<DTO_inMovimientoFooter>();
            this.Close();
        }

        /// <summary>
        /// Se realiza al cambiar el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridColumn col = this.gvDetalle.Columns[this._unboundPrefix + "CantidadEMP"];

                for (int i = 0; i < this.gvDetalle.DataRowCount; i++)
                {
                    DTO_inMovimientoFooter d = (DTO_inMovimientoFooter)this.gvDetalle.GetRow(i);
                    d.SelectInd.Value = this.chkSelect.Checked;
                    if (d.SelectInd.Value.Value)
                        d.Movimiento.CantidadEMP.Value = d.CantidadDispon.Value < d.CantidadPendiente.Value ?
                                                          d.CantidadDispon.Value : d.CantidadPendiente.Value;
                    else
                        d.Movimiento.CantidadEMP.Value = 0;

                    this.ValidData(d, d.Movimiento.CantidadEMP.Value.Value, col);
                }
                this.gvDetalle.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTrasladoInv.cs", "chkSelect_CheckedChanged"));
            }
        }

        /// <summary>
        /// Se realiza al cambiar el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void rbtTipoTraslado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {               
                if (this.rbtTipoTraslado.SelectedIndex == 0) //Traslado Normal
                {
                    this.gvDetalle.Columns[this._unboundPrefix + "PrefDoc"].VisibleIndex = 1;
                    this.gvDetalle.Columns[this._unboundPrefix + "PrefDoc"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].VisibleIndex = 1;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "DatoAdd5"].VisibleIndex = 3;
                    this.gvDetalle.Columns[this._unboundPrefix + "DatoAdd5"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "FechaSol"].VisibleIndex = 4;
                    this.gvDetalle.Columns[this._unboundPrefix + "FechaSol"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "CodigoBSID"].VisibleIndex = 5;
                    this.gvDetalle.Columns[this._unboundPrefix + "CodigoBSID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "OrigenMonetario"].VisibleIndex = 11;
                    this.gvDetalle.Columns[this._unboundPrefix + "OrigenMonetario"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "CantidadPendiente"].VisibleIndex = 12;
                    this.gvDetalle.Columns[this._unboundPrefix + "CantidadPendiente"].Visible = false;
                    this.LoadExistencias();
                    this.masterProyecto.Visible = false;
                }
                else if (this.rbtTipoTraslado.SelectedIndex == 1) //Consumo Interno
                {
                    this.gvDetalle.Columns[this._unboundPrefix + "PrefDoc"].VisibleIndex = 1;
                    this.gvDetalle.Columns[this._unboundPrefix + "PrefDoc"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].VisibleIndex = 2;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "DatoAdd5"].VisibleIndex = 3;
                    this.gvDetalle.Columns[this._unboundPrefix + "DatoAdd5"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "FechaSol"].VisibleIndex = 4;
                    this.gvDetalle.Columns[this._unboundPrefix + "FechaSol"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "CodigoBSID"].VisibleIndex = 5;
                    this.gvDetalle.Columns[this._unboundPrefix + "CodigoBSID"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "OrigenMonetario"].VisibleIndex = 11;
                    this.gvDetalle.Columns[this._unboundPrefix + "OrigenMonetario"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "CantidadPendiente"].VisibleIndex = 12;
                    this.gvDetalle.Columns[this._unboundPrefix + "CantidadPendiente"].Visible = true;
                    this.LoadSolicitudesProv();
                    this.LoadExistencias();
                    this.masterProyecto.Visible = false;
                }
                 else //Salida Ventas
                {
                    this.gvDetalle.Columns[this._unboundPrefix + "PrefDoc"].VisibleIndex = 1;
                    this.gvDetalle.Columns[this._unboundPrefix + "PrefDoc"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].VisibleIndex = 2;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "DatoAdd5"].VisibleIndex = 3;
                    this.gvDetalle.Columns[this._unboundPrefix + "DatoAdd5"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "FechaSol"].VisibleIndex = 4;
                    this.gvDetalle.Columns[this._unboundPrefix + "FechaSol"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "CodigoBSID"].VisibleIndex = 5;
                    this.gvDetalle.Columns[this._unboundPrefix + "CodigoBSID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "OrigenMonetario"].VisibleIndex = 11;
                    this.gvDetalle.Columns[this._unboundPrefix + "OrigenMonetario"].Visible =  false;
                     this.gvDetalle.Columns[this._unboundPrefix + "CantidadPendiente"].VisibleIndex = 12;
                    this.gvDetalle.Columns[this._unboundPrefix + "CantidadPendiente"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "CantidadEMP"].VisibleIndex = 13;
                    this.gvDetalle.Columns[this._unboundPrefix + "CantidadEMP"].Visible = true;
                    this.LoadDetalleFacturaVenta();
                    this.LoadExistencias();
                    this.masterProyecto.Visible = true;
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTrasladoInv.cs", "rbtTipoTraslado_SelectedIndexChanged"));
            }
             
        }

        #endregion        

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencias_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
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

            if (e.IsSetData)
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

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencias_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                    this._rowCurrent = (DTO_inMovimientoFooter)this.gvDetalle.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTrasladoInv.cs", "gvReferencias_FocusedRowChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Cuando selecciona un item de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvReferencia_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                    this._rowCurrent = (DTO_inMovimientoFooter)this.gvDetalle.GetRow(e.RowHandle);               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTrasladoInv.cs", "gvReferencia_RowClick: " + ex.Message));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencia_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            this._rowCurrent = (DTO_inMovimientoFooter)this.gvDetalle.GetRow(e.RowHandle);
            if (this._rowCurrent != null)
            {
                GridColumn col = new GridColumn();
                col = this.gvDetalle.Columns[this._unboundPrefix + "CantidadEMP"];
                if (fieldName == "SelectInd")
                {
                    if (Convert.ToBoolean(e.Value))
                    {
                        if (this.rbtTipoTraslado.SelectedIndex == 0) //Traslado Normal
                            this.gvDetalle.SetRowCellValue(e.RowHandle, col, this._rowCurrent.CantidadDispon.Value);
                        else  //Traslado Proveedores o Salida facturas
                            this.gvDetalle.SetRowCellValue(e.RowHandle, col, this._rowCurrent.CantidadDispon.Value < this._rowCurrent.CantidadPendiente.Value ? 
                                                                             this._rowCurrent.CantidadDispon.Value : this._rowCurrent.CantidadPendiente.Value);
                    }
                    else
                        this.gvDetalle.SetRowCellValue(e.RowHandle, col, 0);

                    this.gvDetalle.FocusedColumn = col;
                }
                if (fieldName == "CantidadEMP")
                {                  
                    this.ValidData(this._rowCurrent, Convert.ToDecimal(e.Value), col);                  
                }
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencia_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.gvDetalle.HasColumnErrors)
                e.Allow = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvReferencia_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string fieldName = this.gvDetalle.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "CantidadEMP")
            {
                if (!this._rowCurrent.SelectInd.Value.Value)
                    e.Cancel = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvReferencia_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this._unboundPrefix + "OrigenMonetario")
                {
                    if (Convert.ToByte(e.Value) == 1)
                        e.DisplayText = "Loc";
                    else if (Convert.ToByte(e.Value) == 2)
                        e.DisplayText = "Ext";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTraslado.cs", "gvReferencia_CustomColumnDisplayText"));
            }
        }

        #endregion          
    }
}
