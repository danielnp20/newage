using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Cliente.GUI.WinApp.Reports;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Reportes;
using NewAge.DTO.Attributes;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de seguridades para los grupos de usuarios
    /// </summary>
    public partial class seGrupoDocumento : FormWithToolbar, IFiltrable
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Para carga de datos
        private IEnumerable<DTO_MasterBasic> Groups;
        private IEnumerable<DTO_seGrupoDocumento> Data;
        private string Group_Copy;
        private List<DTO_seGrupoDocumento> Data_Copy;
        private List<GroupSecurity> S_AllData;
        private List<GroupSecurity> S_Masters; //1
        private List<GroupSecurity> S_Process; //2
        private List<GroupSecurity> S_Bitacora; //3
        private List<GroupSecurity> S_Reports; //4
        private List<GroupSecurity> S_Queries; //5
        private List<GroupSecurity> S_Documents; //6
        private List<GroupSecurity> S_DocumentsAprob; //7
        private List<GroupSecurity> S_Control; //8
        private List<GroupSecurity> S_Activities; //9
        private Dictionary<int, GroupSecurity> UpdateData = new Dictionary<int, GroupSecurity>();
        //Para manejo de propiedades
        protected string Format;
        private FormTypes CurrentType;
        private string UnboundPrefix = "Unbound_";
        private int DocumentID = AppMasters.seGrupoDocumento;
        private ModulesPrefix FrmModule = ModulesPrefix.se;
        private string EmpresaGrupoID;
        private DTO_MasterBasic DtoGroup;
        private GroupSecurity DtoSecurity;
        private List<string> Cols;
        //Internas del formulario
        private FormTypes _frmType = FormTypes.Master;
        private string _frmName;
        private string _colDocName;
        private bool disableValidate = false;

        #endregion

        #region Propiedades

        /// <summary>
        /// Consulta
        /// </summary>
        public DTO_glConsulta Consulta = null;

        /// <summary>
        /// Sentencia de la consulta
        /// </summary>
        public virtual List<DTO_aplMaestraCampo> ConsultaFiels
        {
            get;
            set;
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public seGrupoDocumento()
        {
            try
            {
                //variables
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.DocumentID.ToString());
                this.EmpresaGrupoID = _bc.AdministrationModel.Empresa.EmpresaGrupoID_.Value;
                this.DtoGroup = new DTO_MasterBasic();
                //Inicializa el formulario
                InitializeComponent();

                FormProvider.Master.Form_Load(this, this.FrmModule, this.DocumentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                this.Data_Copy = new List<DTO_seGrupoDocumento>();
                this.LoadGroupData(true);
                this.CurrentType = FormTypes.Master;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "seGrupoDocumento"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Carga los datos de la grilla de grupos 
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        private void LoadGroupData(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    this.AddGroupCols();
                    this.AddSecurityCols(FormTypes.Master);
                    this.AddSecurityCols(FormTypes.Process);
                    this.AddSecurityCols(FormTypes.Bitacora);
                    this.AddSecurityCols(FormTypes.Report);
                    this.AddSecurityCols(FormTypes.Query);
                    this.AddSecurityCols(FormTypes.Document);
                    this.AddSecurityCols(FormTypes.DocumentAprob);
                    this.AddSecurityCols(FormTypes.Control);
                    this.AddSecurityCols(FormTypes.Activities);
                }

                long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.seGrupo, null, null, true);
                this.Groups = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.seGrupo, count, 1, null, null, true);
                this.grlControlGroup.DataSource = this.Groups;
                this.grlControlGroup.RefreshDataSource();
                if(this.Groups.Count() > 0)
                {
                    this.gvGroup.MoveFirst();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "LoadGroupData"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGroupCols()
        {
            GridColumn code = new GridColumn();
            code.FieldName = this.UnboundPrefix + "seGrupoID";
            code.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupo + "_seGrupoID");
            code.UnboundType = UnboundColumnType.String;
            code.VisibleIndex = 0;
            code.Width = 90;
            code.Visible = true;
            code.OptionsColumn.ReadOnly = true;
            this.gvGroup.Columns.Add(code);

            GridColumn desc = new GridColumn();
            desc.FieldName = this.UnboundPrefix + "Descriptivo";
            desc.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupo + "_Descriptivo");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 1;
            desc.Width = 330;
            desc.Visible = true;
            desc.OptionsColumn.ReadOnly = true;
            this.gvGroup.Columns.Add(desc);
        }

        /// <summary>
        /// Carga las columnas de las seguridades segun el documento
        /// <param name="t">Tipo de pestaña para asignar la columna</param>
        /// </summary>
        private void AddSecurityCols(FormTypes t)
        {
            this.Cols = new List<string>();

            #region Columnas Basicas

            //Modulo del documento
            GridColumn mod = new GridColumn();
            mod.FieldName = this.UnboundPrefix + t + "_ModuloID";
            mod.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + "_ModuloID");
            mod.UnboundType = UnboundColumnType.String;
            mod.VisibleIndex = 0;
            mod.Width = 70;
            mod.OptionsColumn.ReadOnly = true;
            mod.Visible = true;

            //Codigo del documento
            this._colDocName = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + "_DocumentoID");
            GridColumn doc = new GridColumn();
            doc.FieldName = this.UnboundPrefix + t + "_DocumentoID";
            doc.Caption = _colDocName;
            doc.UnboundType = UnboundColumnType.Integer;
            doc.VisibleIndex = 1;
            doc.Width = 100;
            doc.OptionsColumn.ReadOnly = true;
            doc.Visible = true;
            this.Cols.Add(doc.Caption);

            //Descripcion del documento
            GridColumn docDesc = new GridColumn();
            docDesc.FieldName = this.UnboundPrefix + t + "_DocumentoDesc";
            docDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + "_DocumentoDesc");
            docDesc.UnboundType = UnboundColumnType.Integer;
            docDesc.VisibleIndex = 2;
            docDesc.Width = 200;
            docDesc.OptionsColumn.ReadOnly = true;
            docDesc.Visible = true;
            this.Cols.Add(docDesc.Caption);

            #endregion
            #region Columnas Seguridades
            string colName = string.Empty;

            //0 - Get
            colName = "_" + FormsActions.Get.ToString();
            GridColumn colGet = new GridColumn();
            colGet.FieldName = this.UnboundPrefix + t + colName;
            colGet.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colGet.UnboundType = UnboundColumnType.Boolean;
            colGet.OptionsColumn.AllowEdit = true;
            colGet.VisibleIndex = 3;
            colGet.Width = 50;
            colGet.Visible = true;
            this.Cols.Add(colGet.Caption);

            //1 - Add
            colName = "_" + FormsActions.Add.ToString();
            GridColumn colAdd = new GridColumn();
            colAdd.FieldName = this.UnboundPrefix + t + colName;
            colAdd.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colAdd.UnboundType = UnboundColumnType.Boolean;
            colAdd.OptionsColumn.AllowEdit = true;
            colAdd.VisibleIndex = 4;
            colAdd.Width = 60;
            colAdd.Visible = true;
            this.Cols.Add(colAdd.Caption);

            //2 - Edit
            colName = "_" + FormsActions.Edit.ToString();
            GridColumn colEdit = new GridColumn();
            colEdit.FieldName = this.UnboundPrefix + t + colName;
            colEdit.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colEdit.UnboundType = UnboundColumnType.Boolean;
            colEdit.OptionsColumn.AllowEdit = true;
            colEdit.VisibleIndex = 5;
            colEdit.Width = 50;
            colEdit.Visible = true;
            this.Cols.Add(colEdit.Caption);

            //3 - Delete
            colName = "_" + FormsActions.Delete.ToString();
            GridColumn colDelete = new GridColumn();
            colDelete.FieldName = this.UnboundPrefix + t + colName;
            colDelete.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colDelete.UnboundType = UnboundColumnType.Boolean;
            colDelete.OptionsColumn.AllowEdit = true;
            colDelete.VisibleIndex = 6;
            colDelete.Width = 50;
            colDelete.Visible = true;
            this.Cols.Add(colDelete.Caption);

            //4 - Print
            colName = "_" + FormsActions.Print.ToString();
            GridColumn colPrint = new GridColumn();
            colPrint.FieldName = this.UnboundPrefix + t + colName;
            colPrint.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colPrint.UnboundType = UnboundColumnType.Boolean;
            colPrint.OptionsColumn.AllowEdit = true;
            colPrint.VisibleIndex = 7;
            colPrint.Width = 70;
            colPrint.Visible = true;
            this.Cols.Add(colPrint.Caption);

            //5 - GenerateTemplate
            colName = "_" + FormsActions.GenerateTemplate.ToString();
            GridColumn colGenerateTemplate = new GridColumn();
            colGenerateTemplate.FieldName = this.UnboundPrefix + t + colName;
            colGenerateTemplate.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colGenerateTemplate.UnboundType = UnboundColumnType.Boolean;
            colGenerateTemplate.OptionsColumn.AllowEdit = true;
            colGenerateTemplate.VisibleIndex = 8;
            colGenerateTemplate.Width = 140;
            colGenerateTemplate.Visible = true;
            this.Cols.Add(colGenerateTemplate.Caption);

            //6 - Copy
            colName = "_" + FormsActions.Copy.ToString();
            GridColumn colCopy = new GridColumn();
            colCopy.FieldName = this.UnboundPrefix + t + colName;
            colCopy.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colCopy.UnboundType = UnboundColumnType.Boolean;
            colCopy.OptionsColumn.AllowEdit = true;
            colCopy.VisibleIndex = 9;
            colCopy.Width = 50;
            colCopy.Visible = true;
            this.Cols.Add(colCopy.Caption);

            //7 - Paste
            colName = "_" + FormsActions.Paste.ToString();
            GridColumn colPaste = new GridColumn();
            colPaste.FieldName = this.UnboundPrefix + t + colName;
            colPaste.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colPaste.UnboundType = UnboundColumnType.Boolean;
            colPaste.OptionsColumn.AllowEdit = true;
            colPaste.VisibleIndex = 10;
            colPaste.Width = 50;
            colPaste.Visible = true;
            this.Cols.Add(colPaste.Caption);

            //8 - Import
            colName = "_" + FormsActions.Import.ToString();
            GridColumn colImport = new GridColumn();
            colImport.FieldName = this.UnboundPrefix + t + colName;
            colImport.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colImport.UnboundType = UnboundColumnType.Boolean;
            colImport.OptionsColumn.AllowEdit = true;
            colImport.VisibleIndex = 11;
            colImport.Width = 70;
            colImport.Visible = true;
            this.Cols.Add(colImport.Caption);

            //9 - Export
            colName = "_" + FormsActions.Export.ToString();
            GridColumn colExport = new GridColumn();
            colExport.FieldName = this.UnboundPrefix + t + colName;
            colExport.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colExport.UnboundType = UnboundColumnType.Boolean;
            colExport.OptionsColumn.AllowEdit = true;
            colExport.VisibleIndex = 12;
            colExport.Width = 70;
            colExport.Visible = true;
            this.Cols.Add(colExport.Caption);

            //10 - ResetPassword
            colName = "_" + FormsActions.SpecialRights.ToString();
            GridColumn colResetPassword = new GridColumn();
            colResetPassword.FieldName = this.UnboundPrefix + t + colName;
            colResetPassword.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colResetPassword.UnboundType = UnboundColumnType.Boolean;
            colResetPassword.OptionsColumn.AllowEdit = true;
            colResetPassword.VisibleIndex = 13;
            colResetPassword.Width = 140;
            colResetPassword.Visible = true;
            this.Cols.Add(colResetPassword.Caption);

            //11 - Revert
            colName = "_" + FormsActions.Revert.ToString();
            GridColumn colRevert = new GridColumn();
            colRevert.FieldName = this.UnboundPrefix + t + colName;
            colRevert.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colRevert.UnboundType = UnboundColumnType.Boolean;
            colRevert.OptionsColumn.AllowEdit = true;
            colRevert.VisibleIndex = 14;
            colRevert.Width = 70;
            colRevert.Visible = true;
            this.Cols.Add(colRevert.Caption);

            //12 - SendtoAppr
            colName = "_" + FormsActions.SendtoAppr.ToString();
            GridColumn colSendtoAppr = new GridColumn();
            colSendtoAppr.FieldName = this.UnboundPrefix + t + colName;
            colSendtoAppr.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colSendtoAppr.UnboundType = UnboundColumnType.Boolean;
            colSendtoAppr.OptionsColumn.AllowEdit = true;
            colSendtoAppr.VisibleIndex = 16;
            colSendtoAppr.Width = 140;
            colSendtoAppr.Visible = true;
            this.Cols.Add(colSendtoAppr.Caption);

            //13 - Approve
            colName = "_" + FormsActions.Approve.ToString();
            GridColumn colApprove = new GridColumn();
            colApprove.FieldName = this.UnboundPrefix + t + colName;
            colApprove.Caption = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + colName);
            colApprove.UnboundType = UnboundColumnType.Boolean;
            colApprove.OptionsColumn.AllowEdit = true;
            colApprove.VisibleIndex = 17;
            colApprove.Width = 70;
            colApprove.Visible = true;
            this.Cols.Add(colApprove.Caption);

            #endregion
            #region Asignacion de columnas
            switch (t)
            {
                case FormTypes.Master:
                    #region Maestras
                    this.gvMaster.Columns.Add(mod);
                    this.gvMaster.Columns.Add(doc);
                    this.gvMaster.Columns.Add(docDesc);

                    this.gvMaster.Columns.Add(colGet);
                    this.gvMaster.Columns.Add(colAdd);
                    this.gvMaster.Columns.Add(colEdit);
                    this.gvMaster.Columns.Add(colDelete);
                    this.gvMaster.Columns.Add(colPrint);
                    this.gvMaster.Columns.Add(colGenerateTemplate);
                    this.gvMaster.Columns.Add(colImport);
                    this.gvMaster.Columns.Add(colExport);
                    this.gvMaster.Columns.Add(colResetPassword);
                    #endregion
                    break;
                case FormTypes.Process:
                    #region Procesos
                    this.gvProcess.Columns.Add(mod);
                    this.gvProcess.Columns.Add(doc);
                    this.gvProcess.Columns.Add(docDesc);

                    this.gvProcess.Columns.Add(colGet);
                    #endregion
                    break;
                case FormTypes.Bitacora:
                    #region Bitacora
                    this.gvBitacora.Columns.Add(mod);
                    this.gvBitacora.Columns.Add(doc);
                    this.gvBitacora.Columns.Add(docDesc);

                    this.gvBitacora.Columns.Add(colGet);
                    this.gvBitacora.Columns.Add(colPrint);
                    this.gvBitacora.Columns.Add(colExport);
                    #endregion
                    break;
                case FormTypes.Report:
                    #region Reportes
                    this.gvReport.Columns.Add(mod);
                    this.gvReport.Columns.Add(doc);
                    this.gvReport.Columns.Add(docDesc);

                    this.gvReport.Columns.Add(colGet);
                    #endregion
                    break;
                case FormTypes.Query:
                    #region Consultas
                    this.gvQuery.Columns.Add(mod);
                    this.gvQuery.Columns.Add(doc);
                    this.gvQuery.Columns.Add(docDesc);

                    this.gvQuery.Columns.Add(colGet);
                    #endregion
                    break;
                case FormTypes.Document:
                    #region Documentos
                    this.gvDocument.Columns.Add(mod);
                    this.gvDocument.Columns.Add(doc);
                    this.gvDocument.Columns.Add(docDesc);

                    this.gvDocument.Columns.Add(colGet);
                    this.gvDocument.Columns.Add(colAdd);
                    this.gvDocument.Columns.Add(colEdit);
                    this.gvDocument.Columns.Add(colDelete);
                    this.gvDocument.Columns.Add(colPrint);
                    this.gvDocument.Columns.Add(colGenerateTemplate);
                    this.gvDocument.Columns.Add(colCopy);
                    this.gvDocument.Columns.Add(colPaste);
                    this.gvDocument.Columns.Add(colImport);
                    this.gvDocument.Columns.Add(colExport);
                    this.gvDocument.Columns.Add(colRevert);
                    this.gvDocument.Columns.Add(colSendtoAppr);
                    this.gvDocument.Columns.Add(colResetPassword);
                    #endregion
                    break;
                case FormTypes.DocumentAprob:
                    #region Documntos de Aprobacion
                    this.gvDocumentAprob.Columns.Add(mod);
                    this.gvDocumentAprob.Columns.Add(doc);
                    this.gvDocumentAprob.Columns.Add(docDesc);

                    this.gvDocumentAprob.Columns.Add(colGet);
                    this.gvDocumentAprob.Columns.Add(colApprove);
                    #endregion
                    break;
                case FormTypes.Control:
                    #region Documntos de control
                    this.gvControl.Columns.Add(mod);
                    this.gvControl.Columns.Add(doc);
                    this.gvControl.Columns.Add(docDesc);

                    this.gvControl.Columns.Add(colGet);
                    #endregion
                    break;
                case FormTypes.Activities:
                    #region Documentos de Actividades
                    this.gvActivities.Columns.Add(mod);
                    this.gvActivities.Columns.Add(doc);
                    this.gvActivities.Columns.Add(docDesc);

                    this.gvActivities.Columns.Add(colGet);
                    this.gvActivities.Columns.Add(colApprove);
                    #endregion
                    break;
            }

            #endregion

            this.Format = TableFormat.FillMasterSimple(this.Cols);
        }

        /// <summary>
        /// Filtra los datos de todas las seguridades segun los tipos de documentos
        /// <param name="docType">Tipo de documento</param>
        /// </summary>
        private List<GroupSecurity> FilterDocuments(FormTypes docType)
        {
            List<GroupSecurity> list = new List<GroupSecurity>();

            IEnumerable<DTO_seGrupoDocumento> filter = new List<DTO_seGrupoDocumento>();
            filter = from f in this.Data where f.DocumentoTipo.Value == (int)docType select f;

            foreach (DTO_seGrupoDocumento f in filter)
            {
                GroupSecurity g = new GroupSecurity()
                {
                    Module = f.ModuloID.Value,
                    Code = f.DocumentoID.Value.Value,
                    Desc = _bc.GetResource(LanguageTypes.Forms, f.DocumentoID.Value.Value.ToString()),
                    Security = SecurityManager.GetDocumentSecurity(f.AccionesPerm.Value.Value)
                };

                list.Add(g);
            }
            return list;
        }

        /// <summary>
        /// Carga un grupo de seguridaes para un documento dado las acciones permitidas 
        /// </summary>
        /// <param name="docId">Identificador del documento</param>
        /// <param name="colVals">valores de las columnas</param>
        /// <returns>Retorna el grupo de seguridades</returns>
        private GroupSecurity CreateGroupSecurity(int docId, List<object> colVals)
        {
            try
            {
                GroupSecurity result = new GroupSecurity();
                result.Code = docId;

                long res = 0;
                for (int i = 0; i < colVals.Count; ++i)
                {
                    if (colVals[i].ToString().ToLower().Trim() == "x")
                    {
                        long t = (long)Math.Pow(2, i);
                        res += t;
                    }
                }

                result.Security = SecurityManager.GetDocumentSecurity(res);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Guarda la version de las actualizaciones segun la pestaña
        /// </summary>
        /// <param name="sec">Fila con seguridad</param>
        /// <param name="docType">Tab (tipo de grupo de seguridad)</param>
        private void SetTabPageUpdates(GroupSecurity sec, FormTypes docType)
        {
            List<GroupSecurity> secList = new List<GroupSecurity>();
            bool listFound = true;

            switch (docType)
            {
                case FormTypes.Master:
                    secList = this.S_Masters;
                    break;
                case FormTypes.Process:
                    secList = this.S_Process;
                    break;
                case FormTypes.Bitacora:
                    secList = this.S_Bitacora;
                    break;
                case FormTypes.Report:
                    secList = this.S_Reports;
                    break;
                case FormTypes.Query:
                    secList = this.S_Queries;
                    break;
                case FormTypes.Document:
                    secList = this.S_Documents;
                    break;
                case FormTypes.DocumentAprob:
                    secList = this.S_DocumentsAprob;
                    break;
                case FormTypes.Control:
                    secList = this.S_Control;
                    break;
                case FormTypes.Activities:
                    secList = this.S_Activities;
                    break;
               
                default:
                    listFound = false;
                    break;
            }

            if (listFound)
            {
                //Revisa la pestaña del documento
                foreach (GroupSecurity gs in secList)
                {
                    if (gs.Code == sec.Code)
                    {
                        gs.Security = sec.Security;
                        break;
                    }
                }
            }
            else
            {
                //Revisa pestaña de maestras
                foreach (GroupSecurity gs in this.S_AllData)
                {
                    if (gs.Code == sec.Code)
                    {
                        gs.Security = sec.Security;
                        break;
                    }
                }

            }
        }

        /// <summary>
        /// Actualiza la fuente de datos de las grillas
        /// </summary>
        private void ReloadGrids()
        {
            this.gcMaster.DataSource = null;
            this.gcProcess.DataSource = null;
            this.gcBitacora.DataSource = null; 
            this.gcReport.DataSource = null;
            this.gcQuery.DataSource = null;
            this.gcDocument.DataSource = null;
            this.gcDocumentAprob.DataSource = null;
            this.gcControl.DataSource = null;
            this.gcActivities.DataSource = null;

            FormTypes currentT = this.CurrentType;

            this.CurrentType = FormTypes.Master;
            this.gcMaster.DataSource = this.S_Masters;

            this.CurrentType = FormTypes.Process;
            this.gcProcess.DataSource = this.S_Process;

            this.CurrentType = FormTypes.Bitacora;
            this.gcBitacora.DataSource = this.S_Bitacora;

            this.CurrentType = FormTypes.Report;
            this.gcReport.DataSource = this.S_Reports;

            this.CurrentType = FormTypes.Query;
            this.gcQuery.DataSource = this.S_Queries;

            this.CurrentType = FormTypes.Document;
            this.gcDocument.DataSource = this.S_Documents;

            this.CurrentType = FormTypes.DocumentAprob;
            this.gcDocumentAprob.DataSource = this.S_DocumentsAprob;

            this.CurrentType = FormTypes.Control;
            this.gcControl.DataSource = this.S_Control;
            
            this.CurrentType = FormTypes.Activities;
            this.gcActivities.DataSource = this.S_Activities;

            this.CurrentType = currentT;

            this.gcMaster.RefreshDataSource();
            this.gcProcess.RefreshDataSource();
            this.gcBitacora.RefreshDataSource();
            this.gcReport.RefreshDataSource();
            this.gcQuery.RefreshDataSource();
            this.gcDocument.RefreshDataSource();
            this.gcDocumentAprob.RefreshDataSource();
            this.gcControl.RefreshDataSource();
            this.gcActivities.RefreshDataSource();
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.DocumentID, this._frmType, this.FrmModule);

                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemPaste.Visible = true;
                FormProvider.Master.itemCopy.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemCopy.Enabled = FormProvider.Master.itemExport.Enabled;
                    FormProvider.Master.itemPaste.Enabled = FormProvider.Master.itemImport.Enabled;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.DocumentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp--seGrupoDocumento.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.DocumentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.FrmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Grillas de Grupos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGroup_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                DTO_MasterBasic dto = (DTO_MasterBasic)e.Row;
                string fieldName=e.Column.FieldName.Substring(this.UnboundPrefix.Length);
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto,null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto),null);
                    }
                }

                if (fieldName == "seGrupoID")
                    e.Value = dto.ID.Value;                
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGroup_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.DtoGroup = (DTO_MasterBasic)this.gvGroup.GetRow(e.FocusedRowHandle);
            this.LoadSecurityData();
            this.UpdateData = new Dictionary<int, GroupSecurity>();
        }

        #endregion

        #region Eventos Grilla de Permisos

        /// <summary>
        /// Actualiza los campos de la grilla de edición
        /// </summary>
        private void LoadSecurityData()
        {
            List<GridProperty> fillGridEdit = new List<GridProperty>();
            try
            {
                this.Data = _bc.AdministrationModel.seGrupoDocumento_GetByType(this.DtoGroup.ID.Value, string.Empty);

                this.S_Masters = new List<GroupSecurity>();
                this.S_Process = new List<GroupSecurity>();
                this.S_Bitacora = new List<GroupSecurity>();
                this.S_Reports = new List<GroupSecurity>();
                this.S_Queries = new List<GroupSecurity>();
                this.S_Documents = new List<GroupSecurity>();
                this.S_DocumentsAprob = new List<GroupSecurity>();
                this.S_Control = new List<GroupSecurity>();
                this.S_Activities = new List<GroupSecurity>();
                this.S_AllData = new List<GroupSecurity>();

                this.S_Masters = this.FilterDocuments(FormTypes.Master);
                this.S_AllData.AddRange(this.S_Masters);
                
                this.S_Process = this.FilterDocuments(FormTypes.Process);
                this.S_AllData.AddRange(this.S_Process);

                this.S_Bitacora = this.FilterDocuments(FormTypes.Bitacora);
                this.S_AllData.AddRange(this.S_Bitacora);

                this.S_Reports = this.FilterDocuments(FormTypes.Report);
                this.S_AllData.AddRange(this.S_Reports);

                this.S_Queries = this.FilterDocuments(FormTypes.Query);
                this.S_AllData.AddRange(this.S_Queries);

                this.S_Documents = this.FilterDocuments(FormTypes.Document);
                this.S_AllData.AddRange(this.S_Documents);

                this.S_DocumentsAprob = this.FilterDocuments(FormTypes.DocumentAprob);
                this.S_AllData.AddRange(this.S_DocumentsAprob);

                this.S_Control = this.FilterDocuments(FormTypes.Control);
                this.S_AllData.AddRange(this.S_Control);

                this.S_Activities = this.FilterDocuments(FormTypes.Activities);
                this.S_AllData.AddRange(this.S_Activities);

                this.ReloadGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "LoadSecurityData"));
            }
        }

        /// <summary>
        /// Se ejecuta antes de cambiar de pestaña
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvSecurity_SelectedPageChanging(object sender, TabPageChangedEventArgs e)
        {
            int pagIndex = e.Page.TabIndex;

            switch (pagIndex)
            {
                case 0:
                    this.CurrentType = FormTypes.Master;
                    break;
                case 1:
                    this.CurrentType = FormTypes.Process;
                    break;
                case 2:
                    this.CurrentType = FormTypes.Bitacora;
                    break;
                case 3:
                    this.CurrentType = FormTypes.Report;
                    break;
                case 4:
                    this.CurrentType = FormTypes.Query;
                    break;
                case 5:
                    this.CurrentType = FormTypes.Document;
                    break;
                case 6:
                    this.CurrentType = FormTypes.DocumentAprob;
                    break;
                case 7:
                    this.CurrentType = FormTypes.Control;
                    break;
                case 8:
                    this.CurrentType = FormTypes.Activities;
                    break;
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvSecurity_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            int pagIndex = e.Page.TabIndex;

            switch (pagIndex)
            {
                case 0:
                    this.gvMaster.MoveFirst();
                    this.gvMaster.Focus();
                    break;
                case 1:
                    this.gvBitacora.MoveFirst();
                    this.gvBitacora.Focus();
                    break;
                case 2:
                    this.gvReport.MoveFirst();
                    this.gvReport.Focus();
                    break;
                case 3:
                    this.gvProcess.MoveFirst();
                    this.gvProcess.Focus();
                    break;
                case 4:
                    this.gvQuery.MoveFirst();
                    this.gvQuery.Focus();
                    break;
                case 5:
                    this.gvDocument.MoveFirst();
                    this.gvDocument.Focus();
                    break;
                case 6:
                    this.gvDocumentAprob.Focus();
                    this.gvDocumentAprob.MoveFirst();
                    break;
                case 7:
                    this.gvControl.Focus();
                    this.gvControl.MoveFirst();
                    break;
                case 8:
                    this.gvActivities.Focus();
                    this.gvActivities.MoveFirst();
                    break;
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvSecurity_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData)
                {
                    GroupSecurity dto = (GroupSecurity)e.Row;
                    string fieldName = e.Column.FieldName.Substring(this.UnboundPrefix.Length + this.CurrentType.ToString().Length + 1);

                    #region Seleccion de Columna
                    switch (fieldName)
                    {
                        case "ModuloID":
                            e.Value = dto.Module;
                            break;
                        case "DocumentoID":
                            e.Value = dto.Code;
                            break;
                        case "DocumentoDesc":
                            e.Value = dto.Desc;
                            break;
                        case "Get":
                            e.Value = dto.Security[(int)FormsActions.Get];
                            break;
                        case "Add":
                            e.Value = dto.Security[(int)FormsActions.Add];
                            break;
                        case "Edit":
                            e.Value = dto.Security[(int)FormsActions.Edit];
                            break;
                        case "Delete":
                            e.Value = dto.Security[(int)FormsActions.Delete];
                            break;
                        case "Print":
                            e.Value = dto.Security[(int)FormsActions.Print];
                            break;
                        case "GenerateTemplate":
                            e.Value = dto.Security[(int)FormsActions.GenerateTemplate];
                            break;
                        case "Copy":
                            e.Value = dto.Security[(int)FormsActions.Copy];
                            break;
                        case "Paste":
                            e.Value = dto.Security[(int)FormsActions.Paste];
                            break;
                        case "Import":
                            e.Value = dto.Security[(int)FormsActions.Import];
                            break;
                        case "Export":
                            e.Value = dto.Security[(int)FormsActions.Export];
                            break;
                        case "SpecialRights":
                            e.Value = dto.Security[(int)FormsActions.SpecialRights];
                            break;
                        case "Revert":
                            e.Value = dto.Security[(int)FormsActions.Revert];
                            break;
                        case "SendtoAppr":
                            e.Value = dto.Security[(int)FormsActions.SendtoAppr];
                            break;
                        case "Approve":
                            e.Value = dto.Security[(int)FormsActions.Approve];
                            break;
                    }

                    #endregion
                }
                if (e.IsSetData)
                {
                    GroupSecurity dto = (GroupSecurity)e.Row;
                    string fieldName = e.Column.FieldName.Substring(this.UnboundPrefix.Length + this.CurrentType.ToString().Length + 1);

                    #region Seleccion de Columna
                    switch (fieldName)
                    {
                        case "ModuloID":
                            dto.Module = e.Value.ToString();
                            break;
                        case "DocumentoID":
                            dto.Code = (int)e.Value;
                            break;
                        case "DocumentoDesc":
                            dto.Desc = e.Value.ToString();
                            break;
                        case "Get":
                            dto.Security[(int)FormsActions.Get] = (bool)e.Value;
                            break;
                        case "Add":
                            dto.Security[(int)FormsActions.Add] = (bool)e.Value;
                            break;
                        case "Edit":
                            dto.Security[(int)FormsActions.Edit] = (bool)e.Value;
                            break;
                        case "Delete":
                            dto.Security[(int)FormsActions.Delete] = (bool)e.Value;
                            break;
                        case "Print":
                            dto.Security[(int)FormsActions.Print] = (bool)e.Value;
                            break;
                        case "GenerateTemplate":
                            dto.Security[(int)FormsActions.GenerateTemplate] = (bool)e.Value;
                            break;
                        case "Copy":
                            dto.Security[(int)FormsActions.Copy] = (bool)e.Value;
                            break;
                        case "Paste":
                            dto.Security[(int)FormsActions.Paste] = (bool)e.Value;
                            break;
                        case "Import":
                            dto.Security[(int)FormsActions.Import] = (bool)e.Value;
                            break;
                        case "Export":
                            dto.Security[(int)FormsActions.Export] = (bool)e.Value;
                            break;
                        case "SpecialRights":
                            dto.Security[(int)FormsActions.SpecialRights] = (bool)e.Value;
                            break;
                        case "Revert":
                            dto.Security[(int)FormsActions.Revert] = (bool)e.Value;
                            break;
                        case "SendtoAppr":
                            dto.Security[(int)FormsActions.SendtoAppr] = (bool)e.Value;
                            break;
                        case "Approve":
                            dto.Security[(int)FormsActions.Approve] = (bool)e.Value;
                            break;
                    }

                    #endregion
                    #region Guardar Cambios

                    if (this.UpdateData.ContainsKey(dto.Code) && this.UpdateData[dto.Code] != new GroupSecurity())
                    {
                        this.UpdateData[dto.Code] = dto;
                    }
                    else
                    {
                        this.UpdateData.Add(dto.Code, dto);
                    }

                    this.SetTabPageUpdates(dto, this.CurrentType);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "gvSecurity_CustomUnboundColumnData"));
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvSecurity_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            switch (this.tcSecurity.SelectedTabPageIndex)
            {
                case 0:
                    this.DtoSecurity = (GroupSecurity)this.gvMaster.GetRow(e.FocusedRowHandle);
                    break;
                case 1:
                    this.DtoSecurity = (GroupSecurity)this.gvProcess.GetRow(e.FocusedRowHandle);
                    break;
                case 2:
                    this.DtoSecurity = (GroupSecurity)this.gvBitacora.GetRow(e.FocusedRowHandle);
                    break;
                case 3:
                    this.DtoSecurity = (GroupSecurity)this.gvReport.GetRow(e.FocusedRowHandle);
                    break;
                case 4:
                    this.DtoSecurity = (GroupSecurity)this.gvQuery.GetRow(e.FocusedRowHandle);
                    break;
                case 5:
                    this.DtoSecurity = (GroupSecurity)this.gvDocument.GetRow(e.FocusedRowHandle);
                    break;
                case 6:
                    this.DtoSecurity = (GroupSecurity)this.gvDocumentAprob.GetRow(e.FocusedRowHandle);
                    break;
                case 7:
                    this.DtoSecurity = (GroupSecurity)this.gvControl.GetRow(e.FocusedRowHandle);
                    break;
                case 8:
                    this.DtoSecurity = (GroupSecurity)this.gvActivities.GetRow(e.FocusedRowHandle);
                    break;
            }
        }

        #endregion

        #region Eventos barra de herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.grlControlGroup.Focus();
                List<DTO_seGrupoDocumento> list = new List<DTO_seGrupoDocumento>();
                long secCode = 0;
                foreach (GroupSecurity item in this.UpdateData.Values)
                {
                    secCode = SecurityManager.GetCodeSecurity(item.Security);

                    DTO_seGrupoDocumento dto = new DTO_seGrupoDocumento()
                    {
                        seGrupoID = new UDT_seGrupoID() { Value = this.DtoGroup.ID.Value },
                        DocumentoID = new UDT_DocumentoID() { Value = item.Code },
                        AccionesPerm = new UDTSQL_bigint() { Value = secCode }
                    };
                    list.Add(dto);
                }

                byte[] bItems = CompressedSerializer.Compress<IEnumerable<DTO_seGrupoDocumento>>(list);
                int userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

                DTO_TxResult result = _bc.AdministrationModel.seGrupoDocumento_UpdateSecurity(bItems, userID);
                MessageForm frm = new MessageForm(result);

                if (result.Result.Equals(ResultValue.OK))
                {
                    this.tcSecurity.SelectedTabPageIndex = 0;
                    this.LoadSecurityData();
                }

                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para exportar la data actual
        /// </summary>
        public override void TBGenerateTemplate()
        {
            try
            {
                ExcelGenerator excell_app = new ExcelGenerator();

                int row = 1;
                int col = 1;

                foreach (string colName in this.Cols)
                {
                    excell_app.AddData(row, col, colName);
                    col++;
                }

                row++;
                int colDocID = 1;
                int colDocDesc = 2;

                foreach (DTO_seGrupoDocumento doc in this.Data)
                {
                    excell_app.AddData(row, colDocID, doc.DocumentoID.Value.Value.ToString());
                    excell_app.AddData(row, colDocDesc, _bc.GetResource(LanguageTypes.Forms, doc.DocumentoID.Value.Value.ToString()));
                    row++;
                }

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para exportar la data actual
        /// </summary>
        public override void TBCopy()
        {
            try
            {
                this.Group_Copy = this.DtoGroup.ID.Value;
                this.Data_Copy = new List<DTO_seGrupoDocumento>();
                foreach (DTO_seGrupoDocumento dto in this.Data)
                {
                    this.Data_Copy.Add(dto);
                }

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessCopy));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBPaste()
        {
            try
            {
                if (this.Data_Copy.Count == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NoData));
                    return;
                }

                string msgTitleImport = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_ImportSecurity));
                string msgImportCode = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_Security);

                string text = string.Format(msgImportCode, this.Group_Copy, this.DtoGroup.ID.Value);

                if (MessageBox.Show(text, msgTitleImport, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DTO_seGrupoDocumento dto in this.Data_Copy)
                    {
                        dto.seGrupoID.Value = this.DtoGroup.ID.Value;
                    }

                    byte[] bItems = CompressedSerializer.Compress<IEnumerable<DTO_seGrupoDocumento>>(this.Data_Copy);
                    int userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

                    DTO_TxResult result = _bc.AdministrationModel.seGrupoDocumento_UpdateSecurity(bItems, userID);
                    MessageForm frm = new MessageForm(result);

                    if (result.Result.Equals(ResultValue.OK))
                    {
                        this.tcSecurity.SelectedTabPageIndex = 0;
                        this.LoadSecurityData();
                    }

                    frm.ShowDialog();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
            try
            {
                PasteOpDTO ret = CopyPasteExtension.PasteFromClipBoard(this.Format);
                if (ret.Success)
                {
                    var text = ret.MsgResult;
                    bool sendToServer = true;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    //Variables de resultado
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Posición de las columnas
                    int documento_pos = 0;
                    int documento_desc = 1;
                    int valuesInitCount = 2;
                    List<object> colVals = new List<object>();
                    List<GroupSecurity> list = new List<GroupSecurity>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    //Mensaje de confirmacion
                    string msgImportDone = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessImport);
                    #endregion
                    #region Llena información para enviar al servidor
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        int docImpID = 0;

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                        }
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        
                        //Recorre todas las filas y verifica que tengan datos y el codigo sea valido
                        if (i > 0 && line.Length > 0)
                        {
                            bool validSecurity = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            colVals = new List<object>();
                            for (int k = valuesInitCount; k < this.Cols.Count; ++k)
                            {
                                colVals.Add(line[k]);
                            }
                            #endregion
                            #region Validaciones de no Null
                            if (string.IsNullOrEmpty(line[documento_pos]))
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = this._colDocName;
                                rdF.Message = msgEmptyField;
                                rd.DetailsFields.Add(rdF);

                                validSecurity = false;
                            }
                            #endregion
                            #region Validacion de formatos
                            try
                            {
                                docImpID = Convert.ToInt32(line[documento_pos].Trim());
                            }
                            catch (Exception ex)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field =this._colDocName;
                                rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                rd.DetailsFields.Add(rdF);

                                validSecurity = false;
                            }

                            for (int k = valuesInitCount; k < this.Cols.Count; ++k)
                            {
                                string col = this.Cols[k];
                                if (col.ToLower() != this._colDocName.ToLower())
                                {
                                    if (line[k].Trim() != string.Empty)
                                    {
                                        if (line[k].Trim().ToLower() != "x")
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = col;
                                            rdF.Message = msgInvalidFormat + " (x)";
                                            rd.DetailsFields.Add(rdF);

                                            validSecurity = false;
                                        }
                                    }
                                }
                            }

                            #endregion
                            #region Crea los DTO de grupos de seguridades
                            if (sendToServer && validSecurity)
                            {
                                GroupSecurity grp = this.CreateGroupSecurity(docImpID, colVals);
                                list.Add(grp);
                            }
                            else
                            {
                                sendToServer = false;
                            }
                            #endregion
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }

                            result.Details.Add(rd);
                        }
                    }
                    #endregion
                    #region Envia la info al servidor o muestra los errores locales
                    if (sendToServer && result.Result == ResultValue.OK)
                    {
                        foreach (GroupSecurity gs in list)
                        {
                            this.SetTabPageUpdates(gs, FormTypes.Other);

                            // Guardar Cambios
                            if (this.UpdateData.ContainsKey(gs.Code) && this.UpdateData[gs.Code] != new GroupSecurity())
                            {
                                this.UpdateData[gs.Code] = gs;
                            }
                            else
                            {
                                this.UpdateData.Add(gs.Code, gs);
                            }
                        }

                        this.ReloadGrids();
                        result.ResultMessage = msgImportDone;
                    }

                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();

                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(ret.MsgResult, MessageType.Error);
                    frm.ShowDialog();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Boton para filtrar la lista de resultados
        /// </summary>
        public override void TBFilter()
        {
            MasterQuery mq = new MasterQuery(this, this.DocumentID, _bc.AdministrationModel.User.ReplicaID.Value.Value, false, typeof(GroupSecurity), typeof(Filtrable));
            mq.SetFK("Module", AppMasters.aplModulo, _bc.CreateFKConfig(AppMasters.aplModulo));
            mq.SetFK("Code", AppMasters.glDocumento, _bc.CreateFKConfig(AppMasters.glDocumento));
            mq.ShowDialog();
        }

        /// <summary>
        /// Boton para asignar un filtro de resultados por defecto
        /// </summary>
        public override void TBFilterDef()
        {
            try
            {
                this.gvMaster.ActiveFilterString = string.Empty;
                this.gvProcess.ActiveFilterString = string.Empty;
                this.gvBitacora.ActiveFilterString = string.Empty;
                this.gvReport.ActiveFilterString = string.Empty;
                this.gvQuery.ActiveFilterString = string.Empty;
                this.gvDocument.ActiveFilterString = string.Empty;
                this.gvDocumentAprob.ActiveFilterString = string.Empty;
                this.gvControl.ActiveFilterString = string.Empty;
                this.gvActivities.ActiveFilterString = string.Empty;

                this.gcMaster.RefreshDataSource();
                this.gcProcess.RefreshDataSource();
                this.gcBitacora.RefreshDataSource();
                this.gcReport.RefreshDataSource();
                this.gcQuery.RefreshDataSource();
                this.gcDocument.RefreshDataSource();
                this.gcDocumentAprob.RefreshDataSource();
                this.gcControl.RefreshDataSource();
                this.gcActivities.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "TBFilterDef"));
            }
        }

        /// <summary>
        /// Boton para generar reportes
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                #region PreReport data
                List<GroupSecurity> preReportDataMasters = new List<GroupSecurity>();
                List<GroupSecurity> preReportDataProcess = new List<GroupSecurity>();
                List<GroupSecurity> preReportDataBitacora = new List<GroupSecurity>();
                List<GroupSecurity> preReportDataReports = new List<GroupSecurity>();
                List<GroupSecurity> preReportDataQueries = new List<GroupSecurity>();
                List<GroupSecurity> preReportDataDocuments = new List<GroupSecurity>();
                List<GroupSecurity> preReportDataDocumentsAprob = new List<GroupSecurity>();

                preReportDataMasters = this.FilterDocuments(FormTypes.Master);
                preReportDataProcess = this.FilterDocuments(FormTypes.Process);
                preReportDataBitacora = this.FilterDocuments(FormTypes.Bitacora);
                preReportDataReports = this.FilterDocuments(FormTypes.Report);
                preReportDataQueries = this.FilterDocuments(FormTypes.Query);
                preReportDataDocuments = this.FilterDocuments(FormTypes.Document);
                preReportDataDocumentsAprob = this.FilterDocuments(FormTypes.DocumentAprob);
                #endregion

                #region Report data
                List<ArrayList> reportFielsList = new List<ArrayList>();
                ArrayList fieldList;
                List<DTO_ReportSeguridad> reportDataList = new List<DTO_ReportSeguridad>();
                DTO_ReportSeguridad reportData = new DTO_ReportSeguridad();

                #region Masters report data
                reportData.Master_Report = new List<DTO_Masters>();
                DTO_Masters mastersData;

                foreach (GroupSecurity g in preReportDataMasters)
                {
                    mastersData = new DTO_Masters()
                    {
                        ModuloID = g.Module,
                        DocumentoID = g.Code,
                        DocumentoDesc = g.Desc,

                        Get = (g.Security[(int)FormsActions.Get]) ? "x" : " ",
                        Add = (g.Security[(int)FormsActions.Add]) ? "x" : " ",
                        Edit = (g.Security[(int)FormsActions.Edit]) ? "x" : " ",
                        Delete = (g.Security[(int)FormsActions.Delete]) ? "x" : " ",
                        Print = (g.Security[(int)FormsActions.Print]) ? "x" : " ",
                        GenerateTemplate = (g.Security[(int)FormsActions.GenerateTemplate]) ? "x" : " ",
                        Import = (g.Security[(int)FormsActions.Import]) ? "x" : " ",
                        Export = (g.Security[(int)FormsActions.Export]) ? "x" : " ",
                        ResetPassword = (g.Security[(int)FormsActions.SpecialRights]) ? "x" : " "
                    };
                    reportData.Master_Report.Add(mastersData);
                };

                fieldList = new ArrayList()
            {
                "ModuloID",
                "DocumentoID",
                "DocumentoDesc",
                "Get",
                "Add",
                "Edit",
                "Delete",
                "Print",
                "GenerateTemplate",
                "Import",
                "Export",
                "ResetPassword"
            };

                reportFielsList.Add(fieldList);
                #endregion

                #region Process report data
                reportData.Process_Report = new List<DTO_Process>();
                DTO_Process processData;
                foreach (GroupSecurity g in preReportDataProcess)
                {
                    processData = new DTO_Process()
                    {
                        ModuloID = g.Module,
                        DocumentoID = g.Code,
                        DocumentoDesc = g.Desc,

                        Get = (g.Security[(int)FormsActions.Get]) ? "x" : " "
                    };
                    reportData.Process_Report.Add(processData);
                };

                fieldList = new ArrayList()
            {
                "ModuloID",
                "DocumentoID",
                "DocumentoDesc",
                "Get"                
            };

                reportFielsList.Add(fieldList);
                #endregion

                #region Bitacora report data
                reportData.Bitacora_Report = new List<DTO_Bitacora>();
                DTO_Bitacora bitacoraData;
                foreach (GroupSecurity g in preReportDataBitacora)
                {
                    bitacoraData = new DTO_Bitacora()
                    {
                        ModuloID = g.Module,
                        DocumentoID = g.Code,
                        DocumentoDesc = g.Desc,

                        Get = (g.Security[(int)FormsActions.Get]) ? "x" : " ",
                        Print = (g.Security[(int)FormsActions.Print]) ? "x" : " ",
                        Export = (g.Security[(int)FormsActions.Export]) ? "x" : " "
                    };
                    reportData.Bitacora_Report.Add(bitacoraData);
                };

                fieldList = new ArrayList()
            {
                "ModuloID",
                "DocumentoID",
                "DocumentoDesc",
                "Get",
                "Print",
                "Export"           
            };

                reportFielsList.Add(fieldList);
                #endregion

                #region Report report data
                reportData.Report_Report = new List<DTO_Reports>();
                DTO_Reports reportsData;
                foreach (GroupSecurity g in preReportDataReports)
                {
                    reportsData = new DTO_Reports()
                    {
                        ModuloID = g.Module,
                        DocumentoID = g.Code,
                        DocumentoDesc = g.Desc,

                        Get = (g.Security[(int)FormsActions.Get]) ? "x" : " "
                    };
                    reportData.Report_Report.Add(reportsData);
                };

                fieldList = new ArrayList()
            {
                "ModuloID",
                "DocumentoID",
                "DocumentoDesc",
                "Get"                
            };

                reportFielsList.Add(fieldList);
                #endregion

                #region Queries report data
                reportData.Query_Report = new List<DTO_Queries>();
                DTO_Queries queriesData;
                foreach (GroupSecurity g in preReportDataQueries)
                {
                    queriesData = new DTO_Queries()
                    {
                        ModuloID = g.Module,
                        DocumentoID = g.Code,
                        DocumentoDesc = g.Desc,

                        Get = (g.Security[(int)FormsActions.Get]) ? "x" : " "
                    };
                    reportData.Query_Report.Add(queriesData);
                };

                fieldList = new ArrayList()
            {
                "ModuloID",
                "DocumentoID",
                "DocumentoDesc",
                "Get"                
            };

                reportFielsList.Add(fieldList);
                #endregion

                #region Documents report data
                reportData.Document_Report = new List<DTO_Documents>();
                DTO_Documents documentsData;
                foreach (GroupSecurity g in preReportDataDocuments)
                {
                    documentsData = new DTO_Documents()
                    {
                        ModuloID = g.Module,
                        DocumentoID = g.Code,
                        DocumentoDesc = g.Desc,

                        Get = (g.Security[(int)FormsActions.Get]) ? "x" : " ",
                        Add = (g.Security[(int)FormsActions.Add]) ? "x" : " ",
                        Edit = (g.Security[(int)FormsActions.Edit]) ? "x" : " ",
                        Delete = (g.Security[(int)FormsActions.Delete]) ? "x" : " ",
                        Print = (g.Security[(int)FormsActions.Print]) ? "x" : " ",
                        GenerateTemplate = (g.Security[(int)FormsActions.GenerateTemplate]) ? "x" : " ",
                        Copy = (g.Security[(int)FormsActions.Copy]) ? "x" : " ",
                        Paste = (g.Security[(int)FormsActions.Paste]) ? "x" : " ",
                        Import = (g.Security[(int)FormsActions.Import]) ? "x" : " ",
                        Export = (g.Security[(int)FormsActions.Export]) ? "x" : " ",
                        Revert = (g.Security[(int)FormsActions.Revert]) ? "x" : " ",
                        SendtoAppr = (g.Security[(int)FormsActions.SendtoAppr]) ? "x" : " "
                    };
                    reportData.Document_Report.Add(documentsData);
                };

                fieldList = new ArrayList()
            {
                "ModuloID",
                "DocumentoID",
                "DocumentoDesc",
                "Get",
                "Add",
                "Edit",
                "Delete",
                "Print",
                "GenerateTemplate",
                "Copy",
                "Paste",
                "Import",
                "Export",
                "Revert",
                "CancelTx",
                "SendtoAppr"           
            };

                reportFielsList.Add(fieldList);
                #endregion

                #region DocumentsAprob report data
                reportData.DocumentAprob_Report = new List<DTO_DocumentsAprob>();
                DTO_DocumentsAprob documentsAprobData;
                foreach (GroupSecurity g in preReportDataDocumentsAprob)
                {
                    documentsAprobData = new DTO_DocumentsAprob()
                    {
                        ModuloID = g.Module,
                        DocumentoID = g.Code,
                        DocumentoDesc = g.Desc,

                        Get = (g.Security[(int)FormsActions.Get]) ? "x" : " "
                    };
                    reportData.DocumentAprob_Report.Add(documentsAprobData);
                };

                fieldList = new ArrayList()
            {
                "ModuloID",
                "DocumentoID",
                "DocumentoDesc",
                "Get"                
            };

                reportFielsList.Add(fieldList);
                #endregion
                #endregion

                reportDataList.Add(reportData);

                SeguridadReport reportView = new SeguridadReport(reportDataList, reportFielsList, this.DocumentID);
                reportView.ShowPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "TBPrint"));
            }
        }

        #endregion  

        /// <summary>
        /// Asigna una consulta desde MasterQuery para hacer el filtrado de datos
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        public void SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            try
            {
                string filtros = Transformer.FiltrosGrilla(consulta.Filtros, fields, typeof(GroupSecurity));
                this.gvMaster.ActiveFilterString = filtros;
                this.gvProcess.ActiveFilterString = filtros;
                this.gvBitacora.ActiveFilterString = filtros;
                this.gvReport.ActiveFilterString = filtros;
                this.gvQuery.ActiveFilterString = filtros;
                this.gvDocument.ActiveFilterString = filtros;
                this.gvDocumentAprob.ActiveFilterString = filtros;
                this.gvControl.ActiveFilterString = filtros;
                this.gvActivities.ActiveFilterString = filtros;

                this.gcMaster.RefreshDataSource();
                this.gcProcess.RefreshDataSource();
                this.gcBitacora.RefreshDataSource();
                this.gcReport.RefreshDataSource();
                this.gcQuery.RefreshDataSource();
                this.gcDocument.RefreshDataSource();
                this.gcDocumentAprob.RefreshDataSource();
                this.gcControl.RefreshDataSource();
                this.gcActivities.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seGrupoDocumento.cs", "SetConsulta"));
            }
        }

    }

    #region GroupSecurity
    
    /// <summary>
    /// Clase que maneja las seguridades de los grupos
    /// </summary>
    public class GroupSecurity
    {

        /// <summary>
        /// Modulo del documento
        /// </summary>
        [Filtrable]
        public string Module
        {
            get;
            set;
        }

        /// <summary>
        /// Codigo del documento
        /// </summary>
        [Filtrable]
        public int Code
        {
            get;
            set;
        }

        /// <summary>
        /// Descripción del documento
        /// </summary>
        internal string Desc;

        /// <summary>
        /// Arreglo de seguridades
        /// </summary>
        internal BitArray Security;
    }

    /// <summary>
    /// Clase que compara 2 listas de grupos de seguridades
    /// </summary>
    internal class GroupSecurityComparer : IEqualityComparer<GroupSecurity>
    {
        public bool Equals(GroupSecurity x, GroupSecurity y)
        {
            return x.Code == y.Code;
        }

        public int GetHashCode(GroupSecurity obj)
        {
            return obj.Code.GetHashCode();
        }
    }

    #endregion
}
