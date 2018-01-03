using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;


namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalFindDocSolicitud : ModalQueryDocument
    {
        public ModalFindDocSolicitud(List<int> filterDocument, bool isMulSelection = false, bool enableCopy = true) : base(filterDocument, isMulSelection, enableCopy) { }

        public ModalFindDocSolicitud()
        {
           // this.InitializeComponent();
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        protected override void LoadData()
        {
            try
            {
                base.LoadData();

                foreach (DTO_glDocumentoControl item in this._listDocuments)
                {
                    if(base.masterDocumento.Value == AppDocuments.PreProyecto.ToString())
                    {
                        DTO_pyPreProyectoDocu docu = this._bc.AdministrationModel.pyPreProyectoDocu_Get(item.NumeroDoc.Value.Value);
                        if (docu != null)
                        {
                            item.TerceroDesc.Value = docu.EmpresaNombre.Value.Length > UDT_DescripTBase.MaxLength ? docu.EmpresaNombre.Value.Substring(0, UDT_DescripTBase.MaxLength) : docu.EmpresaNombre.Value;
                            item.TerceroID.Value = docu.ClienteID.Value;
                            item.Descripcion.Value = docu.ClaseServicioID.Value;
                            item.DocMask.Value = docu.Licitacion.Value.Length > UDT_DescripTBase.MaxLength ? docu.Licitacion.Value.Substring(0, UDT_DescripTBase.MaxLength) : docu.Licitacion.Value;
                            item.DocumentoTipo.Value = docu.TipoSolicitud.Value;
                            item.Observacion.Value = docu.DescripcionSOL.Value;
                            DTO_pyClaseProyecto clase = (DTO_pyClaseProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, false, docu.ClaseServicioID.Value, true);
                            if (clase != null && clase.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros)
                                this.gvDocument.Columns[this.unboundPrefix + "DocMask"].Visible = false;

                        }
                    }
                    else if(base.masterDocumento.Value == AppDocuments.Proyecto.ToString())
                    {
                        DTO_pyProyectoDocu docu = this._bc.AdministrationModel.pyProyectoDocu_Get(item.NumeroDoc.Value.Value);
                        if (docu != null)
                        {
                            item.TerceroDesc.Value = docu.EmpresaNombre.Value.Length > UDT_DescripTBase.MaxLength ? docu.EmpresaNombre.Value.Substring(0, UDT_DescripTBase.MaxLength) : docu.EmpresaNombre.Value;
                            item.TerceroID.Value = docu.ClienteID.Value;
                            item.Descripcion.Value = docu.ClaseServicioID.Value;
                            item.DocMask.Value = docu.Licitacion.Value.Length > UDT_DescripTBase.MaxLength ? docu.Licitacion.Value.Substring(0, UDT_DescripTBase.MaxLength) : docu.Licitacion.Value;
                            item.DocumentoTipo.Value = docu.TipoSolicitud.Value;
                            item.Observacion.Value = docu.DescripcionSOL.Value;
                            DTO_pyClaseProyecto clase = (DTO_pyClaseProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, false, docu.ClaseServicioID.Value, true);
                            if (clase != null && clase.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros)
                                this.gvDocument.Columns[this.unboundPrefix + "DocMask"].Visible = false;
                        }
                    }

                   
                }
                //Filtra por Tipo Solicitud  
                if (this.cmbTipoSol.EditValue != "")
                    this._listDocuments = this._listDocuments.FindAll(x => x.DocumentoTipo.Value == Convert.ToByte(this.cmbTipoSol.EditValue));
                //Filtra por Cliente 
                if (this.masterCliente.ValidID)
                    this._listDocuments = this._listDocuments.FindAll(x => x.TerceroID.Value == this.masterCliente.Value);
                //Filtra por Empresa 
                if (!string.IsNullOrEmpty(this.txtEmpresaNombre.EditValue.ToString()))
                    this._listDocuments = this._listDocuments.FindAll(x => x.TerceroDesc.Value.Contains(this.txtEmpresaNombre.EditValue.ToString().ToUpper()));
                //Filtra por Clase Servicio 
                if (this.masterClaseServicio.ValidID)
                    this._listDocuments = this._listDocuments.FindAll(x => x.Descripcion.Value == this.masterClaseServicio.Value);
                //Filtra por proyecto
                if (this.masterClaseServicio.ValidID)
                    this._listDocuments = this._listDocuments.FindAll(x => x.Descripcion.Value == this.masterClaseServicio.Value);
                //Filtra por licitacion
                if (!string.IsNullOrEmpty(this.txtLicitacion.EditValue.ToString()))
                    this._listDocuments = this._listDocuments.FindAll(x => x.Descripcion.Value == this.txtLicitacion.EditValue);   

                if (this._listDocuments != null)
                {
                    this.gcDocument.DataSource = this._listDocuments;
                    this._docCtrl = this._listDocuments.Count > 0 ? this._listDocuments[0] : null;
                    this.gvDocument.RefreshData();
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySolicitud.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                GridColumn DocumentoPrefijoNro = new GridColumn();
                DocumentoPrefijoNro.FieldName = this.unboundPrefix + "PrefDoc";
                DocumentoPrefijoNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocMask");
                DocumentoPrefijoNro.UnboundType = UnboundColumnType.String;
                DocumentoPrefijoNro.VisibleIndex = 0;
                DocumentoPrefijoNro.Width = 50;
                DocumentoPrefijoNro.Visible = true;
                DocumentoPrefijoNro.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoPrefijoNro);

                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this.unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 0;
                TerceroID.Width = 80;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroID);

                GridColumn EmpresaNombre = new GridColumn();
                EmpresaNombre.FieldName = this.unboundPrefix + "TerceroDesc";
                EmpresaNombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EmpresaNombre");
                EmpresaNombre.UnboundType = UnboundColumnType.String;
                EmpresaNombre.VisibleIndex = 0;
                EmpresaNombre.Width = 120;
                EmpresaNombre.Visible = true;
                EmpresaNombre.OptionsColumn.AllowEdit = false;
                EmpresaNombre.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                EmpresaNombre.AppearanceCell.Options.UseTextOptions = true;
                this.gvDocument.Columns.Add(EmpresaNombre);

                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 0;
                ProyectoID.Width = 45;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProyectoID);

                GridColumn ProyectoDesc = new GridColumn();
                ProyectoDesc.FieldName = this.unboundPrefix + "ProyectoDesc";
                ProyectoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProyectoDesc");
                ProyectoDesc.UnboundType = UnboundColumnType.String;
                ProyectoDesc.VisibleIndex = 0;
                ProyectoDesc.Width = 70;
                ProyectoDesc.Visible = true;
                ProyectoDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProyectoDesc);

                GridColumn DocMask = new GridColumn();
                DocMask.FieldName = this.unboundPrefix + "DocMask";
                DocMask.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Licitacion");
                DocMask.UnboundType = UnboundColumnType.String;
                DocMask.VisibleIndex = 0;
                DocMask.Width = 130;
                DocMask.Visible = true;
                DocMask.OptionsColumn.AllowEdit = false;
                DocMask.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
                DocMask.AppearanceCell.Options.UseTextOptions = true;
                this.gvDocument.Columns.Add(DocMask);

                GridColumn FechaDoc = new GridColumn();
                FechaDoc.FieldName = this.unboundPrefix + "FechaDoc";
                FechaDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                FechaDoc.UnboundType = UnboundColumnType.DateTime;
                FechaDoc.VisibleIndex = 0;
                FechaDoc.Width = 60;
                FechaDoc.Visible = true;
                FechaDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaDoc);

                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.VisibleIndex = 0;
                Valor.Width = 100;
                Valor.Visible = true;
                Valor.OptionsColumn.AllowEdit = false;
                Valor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(Valor);

                #region Columnas no visibles
                GridColumn Estado = new GridColumn();
                Estado.FieldName = this.unboundPrefix + "Estado";
                Estado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Estado");
                Estado.UnboundType = UnboundColumnType.Integer;
                Estado.VisibleIndex = 0;
                Estado.Width = 80;
                Estado.Visible = false;
                Estado.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Estado);

                GridColumn DocumentoID = new GridColumn();
                DocumentoID.FieldName = this.unboundPrefix + "DocumentoID";
                DocumentoID.UnboundType = UnboundColumnType.Integer;
                DocumentoID.Visible = false;
                this.gvDocument.Columns.Add(DocumentoID);

                GridColumn PrefijoID = new GridColumn();
                PrefijoID.FieldName = this.unboundPrefix + "PrefijoID";
                PrefijoID.UnboundType = UnboundColumnType.String;
                PrefijoID.Visible = false;
                this.gvDocument.Columns.Add(PrefijoID);

                GridColumn DocumentoNro = new GridColumn();
                DocumentoNro.FieldName = this.unboundPrefix + "DocumentoNro";
                DocumentoNro.UnboundType = UnboundColumnType.String;
                DocumentoNro.Visible = false;
                this.gvDocument.Columns.Add(DocumentoNro);
                #endregion
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalQueryDocument", "AddGridCols"));
            }
        }

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        protected override void InitControls(List<int> filterDocument)
        {
            base.InitControls(filterDocument);

            this._bc.InitMasterUC(this.masterClaseServicio, AppMasters.pyClaseProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, false);
            #region Controles combo

            Dictionary<string, string> dicTipoSolicitud = new Dictionary<string, string>();
            dicTipoSolicitud.Add("", this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField));
            dicTipoSolicitud.Add(((int)TipoSolicitud.Cotizacion).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Cotizacion));
            dicTipoSolicitud.Add(((int)TipoSolicitud.Licitacion).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Licitacion));
            dicTipoSolicitud.Add(((int)TipoSolicitud.Garantia).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Garantia));
            dicTipoSolicitud.Add(((int)TipoSolicitud.Interna).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Interna));
            dicTipoSolicitud.Add(((int)TipoSolicitud.Otra).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Otra));
            this.cmbTipoSol.EditValue = "";
            this.cmbTipoSol.Properties.DataSource = dicTipoSolicitud;

            this.txtEmpresaNombre.EditValue = "";
            this.txtLicitacion.EditValue = "";

            #endregion

            //Personaliza columnas
           // this.gvDocument.Columns[this.unboundPrefix + "TerceroID"].Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
        }
    }
}
