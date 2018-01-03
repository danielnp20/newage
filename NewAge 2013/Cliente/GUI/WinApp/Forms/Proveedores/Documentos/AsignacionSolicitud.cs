using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Repository;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class AsignacionSolicitud : FormWithToolbar
    {
        #region Delegados

        private delegate void RefreshData();
        private RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        private void RefreshDataMethod()
        {
            this.CleanHeader();
            this.LoadData();
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        List<DTO_prSolicitudAsignacion> _docs = null;
        List<DTO_prSolicitudAsignacion> _docsFiltered = null;
        private int userID = 0;

        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private string unboundPrefix = "Unbound_";
        private string unboundPrefixDet = "UnboundPrefDet_"; 
        private string unboundPrefixCarg = "UnboundPrefCarg_";        
        private bool multiMoneda;
        private DTO_prSolicitudAsignacion currentDoc = null;
        private DTO_prSolicitudAsignDet currentDet = null;
        private bool detailsLoaded = false;
        private bool detFooterLoaded = false;
        private int numDetails = 0;
        private int currentRow = -1;
        private int currentDetRow = -1;

        //Variables Privadas
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private string actividadFlujoID = string.Empty;
        private DTO_glActividadFlujo actividadDTO = null;

        //Filtros
        private string[] _filterAreaAp;
        private string[] _filterCodigoBS;
        private string[] _filterRefer;
        private string _selectedAreaAp = string.Empty;
        private string _selectedCodigoBS = string.Empty;
        private string _selectedRefer = string.Empty;
        private bool _filtered = false;
        private bool _canSelect = true;
        #endregion

        public AsignacionSolicitud()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                //this.AfterInitialize();

                this.numDetails = Convert.ToInt32(_bc.GetControlValue(AppControl.PaginadorAprobacionDocumentos));

                //Asigna la lista de columnas
                this.AddDocumentCols();
                this.AddDetailCols();
                this.AddDetFooterCols();

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    this.actividadFlujoID = actividades[0];
                    this.LoadData();
                }

                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                #endregion
               
                this.refreshData = new RefreshData(RefreshDataMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AsignacionSolicitud.cs", "AsignacionSolicitud"));
            }
        }

        #region  Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.documentID = AppDocuments.SolicitudAsign;
            this.frmModule = ModulesPrefix.pr;

            this._bc.InitMasterUC(this.masterUsuarioAsign, AppMasters.seUsuario, false, true, true, false);

            this.gcDocuments.ShowOnlyPredefinedDetails = true;
        }

        /// <summary>
        /// Carga la información del formulario
        /// </summary>
        private void LoadData()
        {
            try
            {
                int directAssignInd = Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_IndAsignacionDirectaSolicitudes));
                this._docs = new List<DTO_prSolicitudAsignacion>();
                if (directAssignInd == 0)
                    this.Enabled = false;
                else
                {
                   this._docs = _bc.AdministrationModel.Solicitud_GetPendientesForAssign(ModulesPrefix.pr, AppDocuments.SolicitudAsign,this.actividadFlujoID, this._bc.AdministrationModel.User);
                    
                    //List<DTO_prSolicitudAsignacion> tempAsign = new List<DTO_prSolicitudAsignacion>();
                    //foreach (Object item in temp)
                    //    tempAsign.Add((DTO_prSolicitudAsignacion)item);
                    //this._docs = tempAsign;
                }

                #region Trae los filtros
                if (this._docs != null && this._docs.Count > 0)
                {
                    this._filterAreaAp = (from data in this._docs orderby data.AreaAprobacion.Value ascending select data.AreaAprobacion.Value).Distinct().ToArray();
                    this._filterCodigoBS = (from data in this._docs from dataDet in data.SolicitudAsignDet orderby dataDet.CodigoBSID.Value ascending select dataDet.CodigoBSID.Value).Distinct().ToArray();
                    this._filterRefer = (from data in this._docs from dataDet in data.SolicitudAsignDet orderby dataDet.inReferenciaID.Value ascending select dataDet.inReferenciaID.Value).Distinct().ToArray();

                    this.cmbAreaAprob.Items.Clear();
                    this.cmbCodigoBS.Items.Clear();
                    this.cmbReferencia.Items.Clear();

                    this.cmbAreaAprob.Items.Add(string.Empty);
                    this.cmbCodigoBS.Items.Add(string.Empty);
                    this.cmbReferencia.Items.Add(string.Empty);

                    if (this._filterAreaAp.Length > 0)
                        this.cmbAreaAprob.Items.AddRange(this._filterAreaAp);

                    if (this._filterCodigoBS.Length > 0)
                        this.cmbCodigoBS.Items.AddRange(this._filterCodigoBS);

                    if (this._filterRefer.Length > 0)
                        this.cmbReferencia.Items.AddRange(this._filterRefer);
                }
                #endregion

                this.LoadDocuments();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información en la grilla de decomentos
        /// </summary>
        private void LoadDocuments()
        {
            this.currentDoc = null;
            this.currentRow = -1;
            this.gcDocuments.DataSource = null;

            if (!this._filtered)
            {
                #region Llena la lista de dolcumentos para filtrar
                this._docsFiltered = (from doc in this._docs
                                      select new DTO_prSolicitudAsignacion
                                      {
                                          Asignado = doc.Asignado,
                                          AreaAprobacion = doc.AreaAprobacion,
                                          DocumentoID = doc.DocumentoID,
                                          DocumentoNro = doc.DocumentoNro,
                                          FechaEntrega = doc.FechaEntrega,
                                          FileUrl = doc.FileUrl,
                                          LugarEntrega = doc.LugarEntrega,
                                          NumeroDoc = doc.NumeroDoc,
                                          ObservacionDoc = doc.ObservacionDoc,
                                          PeriodoID = doc.PeriodoID,
                                          PrefijoID = doc.PrefijoID,
                                          Prioridad = doc.Prioridad,
                                          UsuarioID = doc.UsuarioID,
                                          UsuarioSolicita = doc.UsuarioSolicita,
                                          SolicitudAsignDet = (from det in doc.SolicitudAsignDet
                                                               select new DTO_prSolicitudAsignDet
                                                               {
                                                                   Asignado = det.Asignado,
                                                                   CantidadSol = det.CantidadSol,
                                                                   CodigoBSID = det.CodigoBSID,
                                                                   ConsecutivoDetaID = det.ConsecutivoDetaID,
                                                                   DatoAdd1 = det.DatoAdd1,
                                                                   Descriptivo = det.Descriptivo,
                                                                   EstadoInv = det.EstadoInv,
                                                                   inReferenciaID = det.inReferenciaID,
                                                                   NumeroDoc = det.NumeroDoc,
                                                                   Parametro1 = det.Parametro1,
                                                                   Parametro2 = det.Parametro2,
                                                                   SolicitudCargos = det.SolicitudCargos,
                                                                   UnidadInvID = det.UnidadInvID
                                                               }).ToList()
                                      }).ToList();
                #endregion
            }

            if (this._docsFiltered != null && this._docsFiltered.Count > 0)
            {
                this.detailsLoaded = false;
                this.currentRow = 0;

                this._docsFiltered.ForEach(docF=> docF.Asignado.Value = docF.SolicitudAsignDet.TrueForAll(detF => detF.Asignado.Value.Value));
                this.AsignadoChanged();

                this.gcDocuments.DataSource = this._docsFiltered;

                if (!detailsLoaded)
                {
                    this.currentDoc = (DTO_prSolicitudAsignacion)this.gvDocuments.GetRow(this.currentRow);
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

        /// <summary>
        /// Carga la información sn la grilla del detalle
        /// </summary>
        private void LoadDetails()
        {
            try
            {
                this.currentDet = null;
                this.currentDetRow = -1;

                DTO_prSolicitudAsignacion doc = this.currentDoc;
                string prefijo = doc.PrefijoID.Value;
                int solNro = doc.DocumentoNro.Value.Value;

                //List<DTO_prSolicitudAsignDet> details = null;
                if (doc != null && this._docsFiltered.Exists(d => d.NumeroDoc.Value == doc.NumeroDoc.Value) 
                    && this._docsFiltered.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).SolicitudAsignDet.Count != 0)
                {
                    this.detFooterLoaded = false;
                    this.currentDetRow = 0;
                    //details = this._docsFiltered.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).SolicitudAsignDet;
                    this.gcDetails.DataSource = this._docsFiltered.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).SolicitudAsignDet;
                    this.currentDet = this._docsFiltered.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).SolicitudAsignDet[this.currentDetRow];
                    this.LoadDetFooter();
                    this.gvDetails.MoveFirst();
                }
                else
                {
                    //details = new List<DTO_prSolicitudAsignDet>();
                    this.gcDetails.DataSource = null;
                    this.gcDetFooter.DataSource = null;
                }

                //this.gcDetails.DataSource = details;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        private void LoadDetFooter()
        {
            try
            {
                DTO_prSolicitudAsignDet det = this.currentDet;

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
        private void AddDocumentCols()
        {
            try
            {
                //Asignado
                GridColumn selectDoc = new GridColumn();
                selectDoc.FieldName = this.unboundPrefix + "Asignado";
                selectDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Asignado");
                selectDoc.UnboundType = UnboundColumnType.Boolean;
                selectDoc.VisibleIndex = 0;
                selectDoc.Width = 50;
                selectDoc.Visible = true;
                selectDoc.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(selectDoc);

                //Prefijo
                GridColumn pref = new GridColumn();
                pref.FieldName = this.unboundPrefix + "PrefijoID";
                pref.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefijoID");
                pref.UnboundType = UnboundColumnType.String;
                pref.VisibleIndex = 1;
                pref.Width = 100;
                pref.Visible = true;
                pref.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(pref);

                //Documento Numero
                GridColumn docNro = new GridColumn();
                docNro.FieldName = this.unboundPrefix + "DocumentoNro";
                docNro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoNro");
                docNro.UnboundType = UnboundColumnType.String;
                docNro.VisibleIndex = 2;
                docNro.Width = 100;
                docNro.Visible = true;
                docNro.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(docNro);

                //FechaEntrega 
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "FechaEntrega";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaEntrega");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 3;
                fecha.Width = 100;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(fecha);

                //Descripcion
                GridColumn obsDoc = new GridColumn();
                obsDoc.FieldName = this.unboundPrefix + "ObservacionDoc";
                obsDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ObservacionDoc");
                obsDoc.UnboundType = UnboundColumnType.String;
                obsDoc.VisibleIndex = 4;
                obsDoc.Width = 450;
                obsDoc.Visible = true;
                obsDoc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(obsDoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AsignacionSolicitud.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        private void AddDetailCols()
        {
            try
            {
                #region Columnas basicas
                //Asignado
                GridColumn selectDet = new GridColumn();
                selectDet.FieldName = this.unboundPrefixDet + "Asignado";
                selectDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Asignado");
                selectDet.UnboundType = UnboundColumnType.Boolean;
                selectDet.VisibleIndex = 0;
                selectDet.Width = 50;
                selectDet.Visible = true;
                selectDet.Fixed = FixedStyle.Left;
                selectDet.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(selectDet);

                //CodigoServicios
                GridColumn codBS = new GridColumn();
                codBS.FieldName = this.unboundPrefixDet + "CodigoBSID";
                codBS.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 1;
                codBS.Width = 100;
                codBS.Visible = true;
                codBS.Fixed = FixedStyle.Left;
                codBS.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codBS);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefixDet + "inReferenciaID";
                codRef.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 100;
                codRef.Visible = true;
                codRef.Fixed = FixedStyle.Left;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codRef);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefixDet + "Parametro1";
                param1.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro1");
                param1.UnboundType = UnboundColumnType.String;
                param1.VisibleIndex = 3;
                param1.Width = 50;
                param1.Visible = true;
                param1.Fixed = FixedStyle.Left;
                param1.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefixDet + "Parametro2";
                param2.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro2");
                param2.UnboundType = UnboundColumnType.String;
                param2.VisibleIndex = 4;
                param2.Width = 50;
                param2.Visible = true;
                param2.Fixed = FixedStyle.Left;
                param2.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(param2);
                #endregion
                #region Columnas extras
                #region Columnas Visible
                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefixDet + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 5;
                desc.Width = 300;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(desc);

                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefixDet + "UnidadInvID";
                unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
                unidad.UnboundType = UnboundColumnType.String;
                unidad.VisibleIndex = 6;
                unidad.Width = 100;
                unidad.Visible = true;
                unidad.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(unidad);

                //Cantidad Solicitud
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefixDet + "CantidadSol";
                cant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadSol");
                cant.UnboundType = UnboundColumnType.Integer;
                cant.VisibleIndex = 7;
                cant.Width = 100;
                cant.Visible = true;
                cant.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(cant);

                #endregion
                #region Columnas No Visibles

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefixDet + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDetails.Columns.Add(numDoc);

                //SolicitudDocuID
                GridColumn solDocu = new GridColumn();
                solDocu.FieldName = this.unboundPrefixDet + "SolicitudDocuID";
                solDocu.UnboundType = UnboundColumnType.Integer;
                solDocu.Visible = false;
                this.gvDetails.Columns.Add(solDocu);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefixDet + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetails.Columns.Add(consDeta);

                //SolicitudDetaID
                GridColumn solDeta = new GridColumn();
                solDeta.FieldName = this.unboundPrefixDet + "SolicitudDetaID";
                solDeta.UnboundType = UnboundColumnType.Integer;
                solDeta.Visible = false;
                this.gvDetails.Columns.Add(solDeta);


                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefixDet + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDetails.Columns.Add(colIndex);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AsignacionSolicitud.cs", "AprobacionSolicitud.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del footer del detalle
        /// </summary>
        private void AddDetFooterCols()
        {
            try
            {
                #region Columnas Visibles
                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefixCarg + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 1;
                proyecto.Width = 200;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = false;
                this.gvDetFooter.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefixCarg + "CentroCostoID";
                ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 2;
                ctoCosto.Width = 200;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = false;
                this.gvDetFooter.Columns.Add(ctoCosto);

                //Centro de costo
                GridColumn percent = new GridColumn();
                percent.FieldName = this.unboundPrefixCarg + "PorcentajeID";
                percent.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorcentajeID");
                percent.UnboundType = UnboundColumnType.Decimal;
                percent.VisibleIndex = 3;
                percent.Width = 200;
                percent.Visible = true;
                percent.OptionsColumn.AllowEdit = false;
                this.gvDetFooter.Columns.Add(percent);
                #endregion
                #region Columnas No Visibles
                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefixCarg + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDetFooter.Columns.Add(numDoc);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefixCarg + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetFooter.Columns.Add(consDeta);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AsignacionSolicitud.cs", "AprobacionSolicitud.cs-AddGridCols"));
            }
        }
        
        /// <summary>
        /// Aplica los filtros asignados por usuario a las grillas
        /// </summary>
        private void FilterGrilla()
        {
            bool filteredDoc = false;
            bool filteredDet = false;

            #region Refresca la lista de los documentos
            this._docsFiltered = (from doc in this._docs
                                  select new DTO_prSolicitudAsignacion
                                  {
                                      Asignado = doc.Asignado,
                                      AreaAprobacion = doc.AreaAprobacion,
                                      DocumentoID = doc.DocumentoID,
                                      DocumentoNro = doc.DocumentoNro,
                                      FechaEntrega = doc.FechaEntrega,
                                      FileUrl = doc.FileUrl,
                                      LugarEntrega = doc.LugarEntrega,
                                      NumeroDoc = doc.NumeroDoc,
                                      ObservacionDoc = doc.ObservacionDoc,
                                      PeriodoID = doc.PeriodoID,
                                      PrefijoID = doc.PrefijoID,
                                      Prioridad = doc.Prioridad,
                                      UsuarioID = doc.UsuarioID,
                                      UsuarioSolicita = doc.UsuarioSolicita,
                                      SolicitudAsignDet = (from det in doc.SolicitudAsignDet
                                                           select new DTO_prSolicitudAsignDet
                                                           {
                                                               Asignado = det.Asignado,
                                                               CantidadSol = det.CantidadSol,
                                                               CodigoBSID = det.CodigoBSID,
                                                               ConsecutivoDetaID = det.ConsecutivoDetaID,
                                                               DatoAdd1 = det.DatoAdd1,
                                                               Descriptivo = det.Descriptivo,
                                                               EstadoInv = det.EstadoInv,
                                                               inReferenciaID = det.inReferenciaID,
                                                               NumeroDoc = det.NumeroDoc,
                                                               Parametro1 = det.Parametro1,
                                                               Parametro2 = det.Parametro2,
                                                               SolicitudCargos = det.SolicitudCargos,
                                                               UnidadInvID = det.UnidadInvID
                                                           }).ToList()
                                  }).ToList();
            #endregion
            #region Filtra por Documento
            if (!string.IsNullOrEmpty(this._selectedAreaAp.Trim()))
            {
                this._docsFiltered = (from data in this._docsFiltered
                                      where data.AreaAprobacion.Value == this._selectedAreaAp.Trim()
                                      select data).ToList();
                filteredDoc = true;
            }
            #endregion
            #region Filtra por Detalle
            if (!string.IsNullOrEmpty(this._selectedCodigoBS.Trim()) || !string.IsNullOrEmpty(this._selectedRefer.Trim()))
            {
                this._docsFiltered.ForEach(doc => 
                    {
                        doc.SolicitudAsignDet = doc.SolicitudAsignDet.Where(det =>
                            det.CodigoBSID.Value == (!string.IsNullOrEmpty(this._selectedCodigoBS.Trim()) ? this._selectedCodigoBS : det.CodigoBSID.Value) &&
                            det.inReferenciaID.Value == (!string.IsNullOrEmpty(this._selectedRefer.Trim()) ? this._selectedRefer : det.inReferenciaID.Value)).ToList();
                    });

                this._docsFiltered = this._docsFiltered.Where(doc => doc.SolicitudAsignDet.Count > 0).ToList();  
            
                filteredDet = true;
            }
            #endregion

            this._canSelect = false;
            this.cbSelectAll.Checked = this._docsFiltered.TrueForAll(docF => docF.SolicitudAsignDet.TrueForAll(detF => detF.Asignado.Value.Value));
            this._canSelect = true;

            this._filtered = filteredDoc || filteredDet;

            this.LoadDocuments();    
        }

        /// <summary>
        /// Guarda informacion temporal de las asignaciones
        /// </summary>
        private void AsignadoChanged()
        {
            this._docsFiltered.ForEach(docF =>
            {
                if (this._docs.Exists(doc => doc.NumeroDoc.Value.Value == docF.NumeroDoc.Value.Value))
                {
                    this._docs.Find(doc => doc.NumeroDoc.Value.Value == docF.NumeroDoc.Value.Value).Asignado.Value = docF.Asignado.Value;
                    docF.SolicitudAsignDet.ForEach(detF =>
                        this._docs.ForEach(doc =>
                        {
                            if (doc.SolicitudAsignDet.Exists(det => det.ConsecutivoDetaID.Value.Value == detF.ConsecutivoDetaID.Value.Value))
                                doc.SolicitudAsignDet.Find(det => det.ConsecutivoDetaID.Value.Value == detF.ConsecutivoDetaID.Value.Value).Asignado.Value = detF.Asignado.Value;
                        }));
                }
            });

            this.gcDocuments.RefreshDataSource();
            this.gcDetails.RefreshDataSource();

            this._canSelect = false;
            this.cbSelectAll.Checked = this._docsFiltered.TrueForAll(docF => docF.SolicitudAsignDet.TrueForAll(detF => detF.Asignado.Value.Value));
            this._canSelect = true;
        }

        /// <summary>
        /// Verifica si el documento es valido para procesar
        /// </summary>
        private bool ValidateAsignacion()
        {
            if (this._docs.TrueForAll(doc => doc.SolicitudAsignDet.TrueForAll(det => !det.Asignado.Value.Value)))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                return false;
            }

            if (!this.masterUsuarioAsign.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterUsuarioAsign.CodeRsx);
                MessageBox.Show(msg);
                this.masterUsuarioAsign.Focus();

                this.AsignadoChanged();

                return false;
            }

            return true;
        }

        private void CleanHeader()
        {
            this.masterUsuarioAsign.Value = string.Empty;
            this.cmbAreaAprob.SelectedIndex = -1;
            this.cmbCodigoBS.SelectedIndex = -1;
            this.cmbReferencia.SelectedIndex = -1;
            this.cbSelectAll.Checked = false;
        }
        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AsignacionSolicitud.cs", "DocumentAprobComplexForm.cs-Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AsignacionSolicitud.cs", "DocumentAprobComplexForm.cs-Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AsignacionSolicitud.cs", "DocumentAprobComplexForm.cs-Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AsignacionSolicitud.cs", "DocumentAprobComplexForm.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            Type dataType = dto.GetType();
            int unboundPrefixLen = this.unboundPrefix.Length;
            if (dataType == typeof(DTO_prSolicitudCargos))
                unboundPrefixLen = this.unboundPrefixCarg.Length;
            if (dataType == typeof(DTO_prSolicitudAsignDet))
                unboundPrefixLen = this.unboundPrefixDet.Length;

            string fieldName = e.Column.FieldName.Substring(unboundPrefixLen);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
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
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
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
                }
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                    this.currentRow = e.FocusedRowHandle;

                this.currentDoc = (DTO_prSolicitudAsignacion)this.gvDocuments.GetRow(this.currentRow);
                this.LoadDetails();
                this.detailsLoaded = true;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Asignado")
                this._docsFiltered[e.RowHandle].SolicitudAsignDet.ForEach(detF => detF.Asignado.Value = (bool)e.Value);

            this.gvDetails.PostEditor();
            this.AsignadoChanged();
        }

        #endregion

        #region Eventos grilla de Detalles

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los detalles
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetails_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentDetRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDetails.RowCount - 1)
                    this.currentDetRow = e.FocusedRowHandle;

                this.currentDet = (DTO_prSolicitudAsignDet)this.gvDetails.GetRow(this.currentDetRow);
                this.LoadDetFooter();
                this.detFooterLoaded = true;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetails_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //this.gvDetails.PostEditor();  

            string fieldName = e.Column.FieldName.Substring(this.unboundPrefixDet.Length);

            if (fieldName == "Asignado")
            {
                this._docsFiltered.Find(docF => docF.NumeroDoc.Value.Equals(this.currentDet.NumeroDoc.Value)).SolicitudAsignDet[e.RowHandle].Asignado.Value = (bool)e.Value;
                if ((bool)e.Value)
                {
                    if (this._docsFiltered.Find(docF => docF.NumeroDoc.Value.Equals(this.currentDet.NumeroDoc.Value)).
                        SolicitudAsignDet.Where(detF => !detF.Asignado.Value.Value).Count() == 0)
                        this._docsFiltered.Find(docF => docF.NumeroDoc.Value.Equals(this.currentDet.NumeroDoc.Value)).Asignado.Value = true;
                }
                else
                    this._docsFiltered.Find(docF => docF.NumeroDoc.Value.Equals(this.currentDet.NumeroDoc.Value)).Asignado.Value = false;
            }       

            this.AsignadoChanged();
        }
                
        #endregion

        #region Eventos Controles

        /// <summary>
        /// Se realiza cuando el usuario elige una Area Aprobacion 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbAreaAprob_SelectedValueChanged(object sender, EventArgs e) 
        {
            System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)sender;
            this._selectedAreaAp = cmb.Text;
            this.FilterGrilla();
        }

        /// <summary>
        /// Se realiza cuando el usuario elige un CodigoBS 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbCodigoBS_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)sender;
            this._selectedCodigoBS = cmb.Text;
            this.FilterGrilla();
        }

        /// <summary>
        /// Se realiza cuando el usuario elige un Referencia 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbReferencia_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)sender;
            this._selectedRefer = cmb.Text;
            this.FilterGrilla();
        }

        /// <summary>
        /// Se realiza cuando el usuario cambia el estado de SelectAll CheckBox 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb = (System.Windows.Forms.CheckBox)sender;

            if (this._canSelect)
            {
                this._docsFiltered.ForEach(docF => { docF.Asignado.Value = cb.Checked; docF.SolicitudAsignDet.ForEach(detF => detF.Asignado.Value = cb.Checked); });              
                this.AsignadoChanged();
            }
        }
        #endregion
        
        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            this.gvDetails.PostEditor();
            try
            {
                if (this._docs != null && this._docs.Count != 0)
                {
                    if (this.ValidateAsignacion())
                    {
                        this._docs.ForEach(doc => doc.SolicitudAsignDet.ForEach(det => det.DatoAdd1.Value = this.masterUsuarioAsign.Value));

                        Thread process = new Thread(this.AssignThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al asignar
        /// </summary>
        private void AssignThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                //List<DTO_TxResult> results = null;

                string estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                List<DTO_SerializedObject> results = _bc.AdministrationModel.Solicitud_Asignar(AppDocuments.SolicitudAsign, this.actividadFlujoID, this._docs);

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

                    if (this._docs[i].Asignado.Value.Value)
                    {
                        MailType mType = MailType.Assign;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AsignacionSolicitud.cs", "AssignThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
