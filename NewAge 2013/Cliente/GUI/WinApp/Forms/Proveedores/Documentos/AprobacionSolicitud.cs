using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using NewAge.Librerias.Project;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Linq;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Aprobacion de Solicitudes
    /// </summary>
    public partial class AprobacionSolicitud : DocumentAprobComplexForm
    {
        //public AprobacionSolicitud()
        //{
        //    this.InitializeComponent();
        //}

        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        //Listas de datos
        private List<DTO_prSolicitudAprobacion> _docs = null;

        private string unboundPrefixCargo = "UnboundPref_";
        private bool _preAprobacionInd = false;  
        private bool _inicialForm = true;

        #endregion

        #region Funciones Virtuales del DocumentAprobForm

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.SolicitudAprob;
            this.frmModule = ModulesPrefix.pr;

            base.SetInitParameters();
        }

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                #region Valida si existe Preaprobacion

                DTO_glConsulta consulta = new DTO_glConsulta();
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                //Trae actividades del documento principal(Solicitud)
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.Solicitud);
                if (actividades.Count > 0)
                {
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ActividadPadre",
                        ValorFiltro = actividades[0],
                        OperadorFiltro = OperadorFiltro.Igual,
                    });
                    consulta.Filtros = filtros;

                    //Obtiene la actividad segun la actividad padre
                    long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.glProcedimientoFlujo, consulta, true);
                    List<DTO_MasterComplex> _lisActividad = this._bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.glProcedimientoFlujo, count, 1, consulta, true).ToList();
                    if (_lisActividad.Count > 0)
                    {
                        DTO_glProcedimientoFlujo _flujo = (DTO_glProcedimientoFlujo)_lisActividad[0];
                        this.actividadFlujoID = _flujo.ActividadHija.Value;
                    }

                    //Valida si es aprobacion directa o existe preaprobacion
                    actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.SolicitudPreAprob);
                    if (actividades.Count > 0 && actividades[0] == this.actividadFlujoID)
                        this._preAprobacionInd = true;
                }

                #endregion

                //Carga el combo de actividades
                this.LookUpDocumentosDataSource();

                //Obtiene los documentos para aprobar
                this.currentRow = -1; 
                this._docs = _bc.AdministrationModel.Solicitud_GetPendientesByModulo(ModulesPrefix.pr, Convert.ToInt32(lookUpDocumentos.EditValue), this.actividadFlujoID, this._bc.AdministrationModel.User);
                foreach (var item in   this._docs )
                    item.FileUrl = string.Empty;
                
                //Llena la grilla
                this.SourceGrid();
                
                this._inicialForm = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        protected override void LoadDetails()
        {
            try
            {
                this.currentDet = null;
                this.currentDetRow = -1;

                DTO_prSolicitudAprobacion doc = (DTO_prSolicitudAprobacion)this.currentDoc;
                string prefijo = doc.PrefijoID.Value;
                int solNro = doc.DocumentoNro.Value.Value;

                DTO_prSolicitud sol = _bc.AdministrationModel.Solicitud_Load(AppDocuments.Solicitud, prefijo, solNro);

                List<DTO_prSolicitudFooter> details = null;
                if (sol != null && sol.Footer.Count != 0)
                {
                    this.detFooterLoaded = false;
                    this.currentDetRow = 0;
                    details = sol.Footer;
                    this.currentDet = sol.Footer[this.currentDetRow];
                    this.LoadDetFooter();
                    this.gvDetails.MoveFirst();
                }
                else 
                {
                    details = new List<DTO_prSolicitudFooter>();
                    this.gcDetFooter.DataSource = null;
                }

                this.gcDetails.DataSource = details;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "LoadDetails"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        protected override void LoadDetFooter()
        {
            try
            {
                DTO_prSolicitudFooter det = (DTO_prSolicitudFooter)this.currentDet;

                List<DTO_prSolicitudCargos> footer = null;
                if (det != null && det.SolicitudCargos.Count > 0)
                    footer = det.SolicitudCargos;
                else
                    footer = new List<DTO_prSolicitudCargos>();

                this.gcDetFooter.DataSource = footer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                base.AddDocumentCols();

                //Prefijo
                GridColumn pref = new GridColumn();
                pref.FieldName = this.unboundPrefix + "PrefijoID";
                pref.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefijoID");
                pref.UnboundType = UnboundColumnType.String;
                pref.VisibleIndex = 3;
                pref.Width = 50;
                pref.Visible = true;
                pref.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(pref);

                //Documento Numero
                GridColumn docNro = new GridColumn();
                docNro.FieldName = this.unboundPrefix + "DocumentoNro";
                docNro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoNro");
                docNro.UnboundType = UnboundColumnType.String;
                docNro.VisibleIndex = 4;
                docNro.Width = 50;
                docNro.Visible = true;
                docNro.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(docNro);

                //FechaEntrega 
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "FechaEntrega";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaEntrega");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 5;
                fecha.Width = 50;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(fecha);

                //Descripcion
                GridColumn obsDoc = new GridColumn();
                obsDoc.FieldName = this.unboundPrefix + "ObservacionDoc";
                obsDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ObservacionDoc");
                obsDoc.UnboundType = UnboundColumnType.String;
                obsDoc.VisibleIndex = 6;
                obsDoc.Visible = true;
                obsDoc.Width = 100 ;
                obsDoc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(obsDoc);

                //Archivo
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.OptionsColumn.ShowCaption = false;
                file.UnboundType = UnboundColumnType.String;
                file.Width = 50;
                file.VisibleIndex = 7;
                file.Visible = true;
                this.gvDocuments.Columns.Add(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        protected override void AddDetailCols()
        {
            try
            {
                #region Columnas basicas
                //CodigoServicios
                GridColumn codBS = new GridColumn();
                codBS.FieldName = this.unboundPrefix + "CodigoBSID";
                codBS.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 1;
                codBS.Width = 80;
                codBS.Visible = true;
                codBS.Fixed = FixedStyle.Left;
                codBS.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codBS);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 80;
                codRef.Visible = true;
                codRef.Fixed = FixedStyle.Left;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codRef);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 3;
                desc.Width = 200;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(desc);

                //MarcaInvID
                GridColumn MarcaInvID = new GridColumn();
                MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
                MarcaInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MarcaInvID");
                MarcaInvID.UnboundType = UnboundColumnType.String;
                MarcaInvID.VisibleIndex = 4;
                MarcaInvID.Width = 50;
                MarcaInvID.Visible = true;
                MarcaInvID.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(MarcaInvID);

                //RefProveedor
                GridColumn RefProveedor = new GridColumn();
                RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
                RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RefProveedor");
                RefProveedor.UnboundType = UnboundColumnType.String;
                RefProveedor.VisibleIndex = 5;
                RefProveedor.Width = 60;
                RefProveedor.Visible = true;
                RefProveedor.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(RefProveedor);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro1");
                param1.UnboundType = UnboundColumnType.String;
                param1.VisibleIndex = 6;
                param1.Width = 40;
                param1.Visible = true;
                param1.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";
                param2.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro2");
                param2.UnboundType = UnboundColumnType.String;
                param2.VisibleIndex = 7;
                param2.Width = 40;
                param2.Visible = true;
                param2.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(param2);
                #endregion
                #region Columnas extras
                #region Columnas Visible
                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefix + "UnidadInvID";
                unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
                unidad.UnboundType = UnboundColumnType.String;
                unidad.VisibleIndex = 8;
                unidad.Width = 40;
                unidad.Visible = true;
                unidad.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(unidad);

                //Cantidad Solicitud
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefix + "CantidadSol";
                cant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadSol");
                cant.UnboundType = UnboundColumnType.Integer;
                cant.VisibleIndex = 9;
                cant.Width = 50;
                cant.Visible = true;
                cant.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(cant);             
                #endregion
                #region Columnas No Visibles

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDetails.Columns.Add(numDoc);

                //SolicitudDocuID
                GridColumn solDocu = new GridColumn();
                solDocu.FieldName = this.unboundPrefix + "SolicitudDocuID";
                solDocu.UnboundType = UnboundColumnType.Integer;
                solDocu.Visible = false;
                this.gvDetails.Columns.Add(solDocu);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefix + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetails.Columns.Add(consDeta);

                //SolicitudDetaID
                GridColumn solDeta = new GridColumn();
                solDeta.FieldName = this.unboundPrefix + "SolicitudDetaID";
                solDeta.UnboundType = UnboundColumnType.Integer;
                solDeta.Visible = false;
                this.gvDetails.Columns.Add(solDeta);


                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDetails.Columns.Add(colIndex);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "AddDetailsCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del footer del detalle
        /// </summary>
        protected override void AddDetFooterCols()
        {
            try
            {
                #region Columnas Visibles
                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefixCargo + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 1;
                proyecto.Width = 100;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = true;
                this.gvDetFooter.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefixCargo + "CentroCostoID";
                ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 2;
                ctoCosto.Width = 100;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = true;
                this.gvDetFooter.Columns.Add(ctoCosto);

                //Centro de costo
                GridColumn percent = new GridColumn();
                percent.FieldName = this.unboundPrefixCargo + "PorcentajeID";
                percent.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorcentajeID");
                percent.UnboundType = UnboundColumnType.Decimal;
                percent.VisibleIndex = 3;
                percent.Width = 100;
                percent.Visible = true;
                percent.OptionsColumn.AllowEdit = true;
                this.gvDetFooter.Columns.Add(percent);
                #endregion
                #region Columnas No Visibles

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefixCargo + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDetFooter.Columns.Add(numDoc);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefixCargo + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetFooter.Columns.Add(consDeta);

                //Indice de la fila de la grilla de los cargos
                GridColumn cargoColIndex = new GridColumn();
                cargoColIndex.FieldName = this.unboundPrefixCargo + "Index";
                cargoColIndex.UnboundType = UnboundColumnType.Integer;
                cargoColIndex.Visible = false;
                this.gvDetFooter.Columns.Add(cargoColIndex);

                //Indice de la fila la grilla principal
                GridColumn detColIndex = new GridColumn();
                detColIndex.FieldName = this.unboundPrefixCargo + "IndexDet";
                detColIndex.UnboundType = UnboundColumnType.Integer;
                detColIndex.Visible = false;
                this.gvDetFooter.Columns.Add(detColIndex);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "AddDetFooter"));
            }
        }

        /// <summary>
        /// Carga la Informacion del Combo de Documentos
        /// </summary>
        protected virtual void LookUpDocumentosDataSource()
        {
            Dictionary<string, string> documentos = new Dictionary<string, string>();
            DTO_glDocumento dtoDoc = null;
            if (this._preAprobacionInd)
            {
                dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.SolicitudPreAprob.ToString(), true);
                documentos.Add(dtoDoc.ID.Value, dtoDoc.Descriptivo.Value); 
            }

            dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.SolicitudAprob.ToString(), true);
            documentos.Add(dtoDoc.ID.Value, dtoDoc.Descriptivo.Value);
            this.lookUpDocumentos.Properties.ValueMember = "Key";
            this.lookUpDocumentos.Properties.DisplayMember = "Value";
            this.lookUpDocumentos.Properties.DataSource = documentos;
            if (this._preAprobacionInd)
            {
                this.documentID = AppDocuments.SolicitudPreAprob;
                this.lookUpDocumentos.EditValue = AppDocuments.SolicitudPreAprob.ToString();
            }
            else
            {
                this.documentID = AppDocuments.SolicitudAprob;
                this.lookUpDocumentos.EditValue = AppDocuments.SolicitudAprob.ToString();
            }
        }

        /// <summary>
        /// Asigna la fuente de datos y la variables
        /// </summary>
        private void SourceGrid()
        {
            this.currentDoc = null;
            this.currentRow = -1;
            this.gcDocuments.DataSource = null;
            if (this._docs != null && this._docs.Count > 0)
            {
                this.detailsLoaded = false;
                this.allowValidate = false;
                this.currentRow = 0;
                this.gcDocuments.DataSource = this._docs;
                this.allowValidate = true;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Approve);

                if (!detailsLoaded)
                {
                    this.currentDoc = this.gvDocuments.GetRow(this.currentRow);
                    this.LoadDetails();
                }

                this.gvDocuments.MoveFirst();
            }
            else
            {
                this.gcDetails.DataSource = null;
                this.gcDetFooter.DataSource = null;
            }
        }

        #endregion

        #region Eventos Virtuales grillas

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            
            #region Generales
            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value)
                    this._docs[e.RowHandle].Rechazado.Value = false;
            }
            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                    this._docs[e.RowHandle].Aprobado.Value = false;
            }
            #endregion

            this.gcDocuments.RefreshDataSource();
            this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDetails_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDetFooter_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

        }

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            base.riPopup_QueryPopUp(sender, e);

            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Descriptivo")
            {
                this.richEditControl.ReadOnly = true;
                this.richEditControl.Document.Text = this.gvDocuments.GetFocusedRowCellValue(fieldName).ToString();
            }
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Descriptivo")
                this.richEditControl.ReadOnly = false;
            else
                base.riPopup_QueryResultValue(sender, e);
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Se realiza cuando cambia el valor del combo 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void lookUpDocumentos_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(Convert.ToInt32(lookUpDocumentos.EditValue));
                if (actividades.Count > 0 && !this._inicialForm)
                {
                    this.actividadFlujoID = actividades[0];
                    this._docs = _bc.AdministrationModel.Solicitud_GetPendientesByModulo(ModulesPrefix.pr,Convert.ToInt32(lookUpDocumentos.EditValue), this.actividadFlujoID, this._bc.AdministrationModel.User);
                    foreach (var item in this._docs)
                        item.FileUrl = string.Empty;
                    this.SourceGrid();                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "lookUpDocumentos_EditValueChanged"));
            }
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void editLink_Click(object sender, EventArgs e) 
        {
            try
            {
                int fila = this.gvDocuments.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._docs[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "editLink_Click"));
            }
        }
        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                if (this._docs != null && this._docs.Count != 0)
                {
                    if (this.ValidateDocRow(this.gvDocuments.FocusedRowHandle))
                    {
                        Thread process = new Thread(this.ApproveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "TBSave"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        protected override void ApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
               
                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

               //List<DTO_TxResult> results = null;

               List<DTO_SerializedObject> results = _bc.AdministrationModel.Solicitud_AprobarRechazar(AppDocuments.SolicitudAprob, this.actividadFlujoID, this._docs, false);
               FormProvider.Master.StopProgressBarThread(this.documentID);
               int i = 0;
               int percent = 0;
               List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

               FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
               foreach (object obj in results)
               {
                   #region Funciones de progreso
                   FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                   percent = ((i + 1) * 100) / (results.Count);

                   if (FormProvider.Master.ProcessCanceled(this.documentID))
                   {
                       MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                       break;
                   }
                   #endregion

                   if (this._docs[i].Aprobado.Value.Value || this._docs[i].Rechazado.Value.Value)
                   {
                       MailType mType = this._docs[i].Aprobado.Value.Value ? MailType.Approve : MailType.Reject;
                       bool isOK = _bc.SendDocumentMail(mType, this.documentID, this.actividadDTO.seUsuarioID.Value, obj, false);
                       if (!isOK)
                       {
                           DTO_TxResult r = (DTO_TxResult)obj;
                           resultsNOK.Add(r);
                       }
                   }
                   i++;
               }
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}