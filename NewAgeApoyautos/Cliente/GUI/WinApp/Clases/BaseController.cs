using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.Cliente.Proxy.Model;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Text.RegularExpressions;
using NewAge.Cliente.GUI.WinApp.Forms;
using System.Reflection;
using NewAge.DTO.Attributes;
using NewAge.ReportesComunes;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.Resultados;
using SentenceTransformer;
using System.Net.NetworkInformation;
using System.Text;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Controlador base de la aplicación 
    /// </summary>
    public class BaseController : CommonReportDataSupplier
    {
        #region Variables

        /// <summary>
        /// Instancia única del base controller
        /// </summary>
        private static BaseController _instance;

        /// <summary>
        /// Numero de referencias del controlador base
        /// </summary>
        private static int _numOfReference;

        /// <summary>
        /// Administration model (fachada del negocio)
        /// </summary>
        private AdministrationModel _administrationModel;

        /// <summary>
        /// Configuración inicial traida del servidor central
        /// </summary>
        private DTO_seLAN _basicConfig;

        /// <summary>
        /// Expresion regular que acepta numeros y letras
        /// </summary>
        public string RegExpNoSymbols = "^[a-zA-Z0-9_.,]*$";

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene la instancia del administration model
        /// </summary>
        public AdministrationModel AdministrationModel
        {
            get { return this._administrationModel; }
        }

        /// <summary>
        /// Obtiene la configuracion de la LAN a la cual se esta conectando el usuario
        /// </summary>
        public DTO_seLAN BasicConfig
        {
            get { return _basicConfig; }
            set
            {
                _basicConfig = value;
                this._administrationModel.BasicConfig = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        private BaseController()
        {
            //Revisa si esta en conexion local 
            string ipLocal = ConfigurationManager.AppSettings["Server.Name"];
            string connStr = ConfigurationManager.AppSettings["Server.DB.Connection"];
            Ping pingSender = new Ping();
            bool isLocal = false;
            try
            {
                PingReply reply = pingSender.Send(ipLocal);
                isLocal = reply.Status == IPStatus.Success ? true : false;
            }
            catch (Exception){ }

            _numOfReference = 0;
            //Se instancia el AM de la Lan que sobrecarga el AdministrationModel
            this._administrationModel = new AdministrationModel(isLocal, connStr);
            this._administrationModel.FormsSecurity = new Dictionary<string, string>();
            this._administrationModel.Config = new Dictionary<string, string>();
            this._administrationModel.Modules = new Dictionary<string, string>();
            this._administrationModel.Languages = new Dictionary<string, Dictionary<string, string>>();
            this._administrationModel.Menus = new Dictionary<string, MainMenu>();
            this._administrationModel.Tables = new Dictionary<Tuple<int, string>, DTO_glTabla>();
            this._administrationModel.MasterProperties = new Dictionary<int, DTO_aplMaestraPropiedades>();

            if (isLocal)
                this.AdministrationModel.Start(true);
        }

        /// <summary>
        /// Obtiene la única instancia del controlador base
        /// </summary>
        /// <returns>Retorna la única instancia del controlador base</returns>
        public static BaseController GetInstance()
        {
            try
            {
                if (_instance == null)
                    _instance = new BaseController();

                _numOfReference++;
                return _instance;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Metodos públicos

        #region Archivos

        /// <summary>
        /// Retorna la url para consultar un archivo
        /// </summary>
        /// <param name="fileType">Tipo de archivo</param>
        /// <param name="docIdentifier">Identificador del documento</param>
        /// <param name="local">Indica para el instalador si es local, remoto o no aplica</param>
        /// <param name="docName">Nombre del documento o del reporte. Para anexos viene dado por Modulo&&NombreArchivo.Ext (Ej. Cc&&MiArch.pdf)</param>
        /// <returns>Retorna la url del archivo</returns>
        internal string UrlDocumentFile(TipoArchivo fileType, int? docIdentifier, bool? local, string docName = null)
        {
            int ruta;
            string fileName = string.Empty;
            string ext = string.Empty;

            string filesPath = this.GetControlValue(AppControl.RutaVirtualArchivos);
            switch (fileType)
            {
                case TipoArchivo.AnexosDocumentos:
                    #region Documentos
                    string[] fileFormat = docName.Split('&');

                    string anexosFormat = this.GetControlValue(AppControl.RutaAnexos);
                    anexosFormat = anexosFormat.Replace("\\", "/");

                    string anexoPath = filesPath + string.Format(anexosFormat, fileFormat.ElementAt(0));

                    return anexoPath + fileFormat.ElementAt(1);

                    break; 
                    #endregion
                case TipoArchivo.Documentos:
                    #region Documentos
                    fileName = /*docName;*/string.Format(this.GetControlValue(AppControl.NombreArchivoDocumentos), docIdentifier.ToString());
                    ruta = AppControl.RutaDocumentos;
                    ext = this.GetControlValue(AppControl.ExtensionDocumentos);
                    break;
                    #endregion
                case TipoArchivo.Temp:
                    #region Documentos
                    fileName = docName;
                    ruta = AppControl.RutaTemporales;
                    break;
                    #endregion
                case TipoArchivo.Plantillas:
                    #region Plantillas
                    fileName = docName;
                    ruta = AppControl.RutaPlantillas;
                    break;
                    #endregion
                default:
                    MessageBox.Show("Sin implementacion (BaseController - UrlDocumentFile)");
                    return string.Empty;
            }

            string docsPath = this.GetControlValue(ruta);

            string filePath = filesPath + docsPath.Replace("\\", "/") + fileName;

            return filePath + ext;
        }

        #endregion

        #region Control

        /// <summary>
        /// Obtiene el valor de un item de control deacuerdo al ID
        /// </summary>
        /// <param name="ctrlID">Identificador del control</param>
        /// <param name="isFromControl">Indica si se esta consultando desde una pantalla de control</param>
        /// <returns>Retorna el valor de la tabla glControl</returns>
        internal string GetControlValue(int ctrlID, bool isFromControl = false)
        {
            string error = this.GetResource(LanguageTypes.Error, DictionaryMessages.Err_ControlNoData);
            var val = from c in this.AdministrationModel.ControlList where c.glControlID.Value == ctrlID select c;
            if (val.Count() <= 0)
            {
                MessageBox.Show(string.Format(error, ctrlID.ToString(), string.Empty));
                return string.Empty;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(val.First().Data.Value) && !isFromControl)
                    MessageBox.Show(string.Format(error, ctrlID.ToString(), val.First().Descriptivo.Value));

                return val.First().Data.Value;
            }
        }

        /// <summary>
        /// Obtiene el valor de un item de control deacuerdo al ID
        /// </summary>
        /// <param name="ctrlID">Identificador del control</param>
        /// <param name="isFromControl">Indica si se esta consultando desde una pantalla de control</param>
        /// <returns>Retorna el valor de la tabla glControl</returns>
        internal string GetControlValueAllowEmpty(int ctrlID, bool isFromControl = false)
        {
            string error = this.GetResource(LanguageTypes.Error, DictionaryMessages.Err_ControlNoData);
            var val = from c in this.AdministrationModel.ControlList where c.glControlID.Value == ctrlID select c;
            if (val.Count() <= 0)
                return string.Empty;
            else
                return val.First().Data.Value;
        }

        /// <summary>
        /// Devuelve in valor segun la empresa
        /// </summary>
        /// <param name="mod">Modulo del sistema</param>
        /// <param name="ctrl">Control que se desea consultar</param>
        /// <param name="isFromControl">Indica si se esta consultando desde una pantalla de control</param>
        /// <returns>Retorna el valor buscado</returns>
        internal string GetControlValueByCompany(ModulesPrefix mod, string ctrl, bool isFromControl = false)
        {
            string modValue = ((int)mod).ToString();

            if (modValue.Length == 1)
                modValue = "0" + modValue;

            string empValue = this.AdministrationModel.Empresa.NumeroControl.Value;
            string key = empValue + modValue + ctrl;

            string result = this.GetControlValue(Convert.ToInt32(key), isFromControl);
            return result;
        }
        /// <summary>
        /// Devuelve in valor segun la empresa
        /// </summary>
        /// <param name="mod">Modulo del sistema</param>
        /// <param name="ctrl">Control que se desea consultar</param>
        /// <returns>Retorna la descripcion del valor buscado</returns>
        internal string GetControlDescripcionValueByCompany(ModulesPrefix mod, string ctrl, bool isFromControl = false)
        {
            string modValue = ((int)mod).ToString();

            if (modValue.Length == 1)
                modValue = "0" + modValue;

            string empValue = this.AdministrationModel.Empresa.NumeroControl.Value;
            string key = empValue + modValue + ctrl;

            string result = this.GetControlValue(Convert.ToInt32(key), isFromControl);
            return result;
        }
        /// <summary>
        /// Devuelve in valor segun la empresa
        /// </summary>
        /// <param name="mod">Modulo del sistema</param>
        /// <param name="ctrl">Control que se desea consultar</param>
        /// <param name="isFromControl">Indica si se esta consultando desde una pantalla de control</param>
        /// <returns>Retorna el valor buscado</returns>
        internal string GetControlValueByCompanyAllowEmpty(ModulesPrefix mod, string ctrl, bool isFromControl = false)
        {
            string modValue = ((int)mod).ToString();

            if (modValue.Length == 1)
                modValue = "0" + modValue;

            string empValue = this.AdministrationModel.Empresa.NumeroControl.Value;
            string key = empValue + modValue + ctrl;

            string result = this.GetControlValueAllowEmpty(Convert.ToInt32(key), isFromControl);
            return result;
        }

        #endregion
        
        #region Controles de usuario

        /// <summary>
        /// Inicia el control de herencia
        /// </summary>
        /// <param name="uc_Hierarchy">Control de herencia</param>
        /// <param name="f">Formulario</param>
        /// <param name="t">Tabla del sistema</param>
        /// <param name="tcod">Texto del titulo Codigo</param>
        /// <param name="tdesc">Texto del titulo Descripcion</param>
        /// <param name="e_addC">Evento para agregar columna</param>
        /// <param name="e_fl">Evento para llenar niveles</param>
        /// <param name="e_cl">Evento para verificar nicel</param>
        internal void Hierarchy_Init(uc_Hierarchy hControl, Type f, DTO_glTabla t, string tcod, string tdesc,
            uc_Hierarchy.AddColumnHandler e_addC, uc_Hierarchy.FillLevelHandler e_fl, uc_Hierarchy.CheckLevelNewHandler e_cl, uc_Hierarchy.UpdateEditGridHandler e_ec, int docId, string colId, string colDesc)
        {
            hControl.TextCode = tcod;
            hControl.TextDescr = tdesc;
            hControl.InitControl(f, t, docId, colId, colDesc);
            hControl.AddHierarchyCol += new uc_Hierarchy.AddColumnHandler(e_addC);
            hControl.FillLevel += new uc_Hierarchy.FillLevelHandler(e_fl);
            hControl.CheckLevelNew += new uc_Hierarchy.CheckLevelNewHandler(e_cl);
            hControl.UpdateEditGrid += new uc_Hierarchy.UpdateEditGridHandler(e_ec);
        }

        /// <summary>
        /// inicializa el control de usuarios de maestra
        /// </summary>
        /// <param name="ctrl">Control de maestra</param>
        /// <param name="masterDocID">Documento de la maestra</param>
        /// <param name="hasLabel">Indica si se debe mostrar el label que tiene por defecto</param>
        /// <param name="hasDesc">Identifica si el control tiene descripcion</param>
        /// <param name="onlyRoots">Indica si solo acepta raices para una jerarquica</param>
        /// <param name="mainControl">Indica si es un control principal, para ponerle un color diferente</param>
        internal void InitMasterUC(uc_MasterFind ctrl, int masterDocID, bool hasLabel, bool hasDesc, bool onlyRoots, bool mainControl, List<DTO_glConsultaFiltro> filtros = null)
        {
            DTO_glTabla fktable;
            DTO_aplMaestraPropiedades props;
            Type modalType;

            string empGrupo = this.GetMaestraEmpresaGrupoByDocumentID(masterDocID);
            Tuple<int, string> tup = new Tuple<int, string>(masterDocID, empGrupo);

            fktable = this.AdministrationModel.Tables[tup];
            props = this.AdministrationModel.MasterProperties[masterDocID];
            modalType = fktable.Jerarquica.Value.Value ? typeof(MasterHierarchyFind) : typeof(ModalMaster);

            ctrl.Table = fktable;
            ctrl.ModalType = typeof(ModalMaster);
            ctrl.TableName = props.NombreTabla;
            ctrl.IsHierarchical = fktable.Jerarquica.Value.Value;
            ctrl.DocId = props.DocumentoID;
            ctrl.ColId = props.ColumnaID;
            ctrl.GrupoEmpresa = this.GetMaestraEmpresaGrupoByDocumentID(props.DocumentoID);
            ctrl.HasDesc = hasDesc;
            ctrl.HasLabel = hasLabel;
            ctrl.Filtros = filtros;

            string rsx = this.GetResource(LanguageTypes.Forms, props.DocumentoID + "_" + props.ColumnaID);

            ctrl.InitControl(props.IDLongitudMax, rsx, onlyRoots, mainControl);
            ctrl.GetMasterDescriptionByID += new uc_MasterFind.GetByIDHandler(this.GetMasterDescriptionByID);
            ctrl.AutoSize = true;

        }

        /// <summary>
        /// inicializa el control de usuarios de maestra
        /// </summary>
        /// <param name="ctrl">Control de periodo</param>
        /// <param name="extraPeriod">Numero de periodoso extras que permite</param>
        internal void InitPeriodUC(uc_PeriodoEdit ctrl, int extraPeriod)
        {
            string tit = GeneralResources.PeriodTitFrm;
            string lbl = GeneralResources.PeriodSelect;
            string btn = GeneralResources.PeriodAccept;

            ctrl.InitControl(extraPeriod, tit, lbl, btn);
        }

        #endregion

        #region Documentos

        /// <summary>
        /// Retorna al prefijo de un documento
        /// </summary>
        /// <param name="af">Area Funcional</param>
        /// <param name="documentoID">Identificador del documento</param>
        /// <returns>Retorna el prefijo</returns>
        internal string GetPrefijo(string af, int documentoID)
        {
            string prefijo = string.Empty;
            try
            {
                DTO_glDocumento dtoDoc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, documentoID.ToString(), true);
                TipoPrefijo_Documento tipoPref = (TipoPrefijo_Documento)Enum.Parse(typeof(TipoPrefijo_Documento), dtoDoc.PrefijoTipo.Value.Value.ToString());
                switch (tipoPref)
                {
                    case TipoPrefijo_Documento.Fijo:
                        prefijo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                        break;
                    case TipoPrefijo_Documento.AreaFuncional:
                        prefijo = this.AdministrationModel.PrefijoDocumento_Get(af, documentoID);
                        break;
                }

                return prefijo;
            }
            catch (Exception)
            {
                MessageBox.Show(this.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidPrefix));
                return prefijo;
            }
        }

        /// <summary>
        /// Obtiene el comprobante de un documento
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="documentID">Identificador del documento</param>
        /// <returns>Retorna el identificador del comprobante</returns>
        internal string GetDocumentComprobante(string prefijoID, int documentID)
        {
            string compID = string.Empty;
            try
            {
                compID = this.AdministrationModel.coComprobantePrefijo_GetComprobanteByDocPref(AppMasters.coComprobantePrefijo, documentID, prefijoID, false);
                if (string.IsNullOrWhiteSpace(compID))
                {
                    MessageBox.Show(this.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CompDocPref));
                }

                return compID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.GetResourceForException(ex, "BaseController", "BaseController-GetDocumentComprobante"));
                return compID;
            }
        }

        /// <summary>
        /// Corrige el ultimo valor calculado de la contrapartida
        /// </summary>
        /// <param name="tc">Tasa de cambio</param>
        /// <param name="mdaLoc">Moneda local por defecto</param>
        /// <param name="mdaComp">Moneda del comprobante</param>
        /// <param name="contra">registro de la contrapartida</param>
        internal void CorregirContrapartida(decimal tc, string mdaLoc, string mdaComp, DTO_ComprobanteFooter contra)
        {
            if (this.AdministrationModel.MultiMoneda)
            {

            }
        }

        /// <summary>
        ///  Envia el primer mail relacionado con un documento
        /// </summary>
        /// <param name="sendToAprob">Indica si es un formulario particular o un formulario que envia a aprobacion</param>
        /// <param name="documentID">Identificador del documento que esta ejecutando la tx</param>
        /// <param name="userID">Identificador del usuario responsable</param>
        /// <param name="objResult">Objeto de resultado (DTO_txResult o DTO_Alarma)</param>
        /// <param name="showResult">Indica si muestra la pantalla de resultados</param>
        /// <param name="showPrefNro">Indica si muestra el prefijo y Nro del doc generado</param>
        /// <returns>retorna true si el objecto de resultado esta libre de errores</returns>
        internal bool SendDocumentMail(MailType mailType, int documentID, string userID, object objResult, bool showResult, bool showPrefNro = false)
        {
            try
            {
                #region Obtiene el tipo de resultado
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                if (objResult.GetType() == result.GetType())
                    result = (DTO_TxResult)objResult;
                else
                {
                    DTO_Alarma alarma = (DTO_Alarma)objResult;
                    if (showPrefNro)
                        result.ResultMessage = DictionaryMessages.DocumentOK + "&&" + alarma.PrefijoID + "&&" +  alarma.Consecutivo;
                    #region Variables para el mail
                    DTO_seUsuario user = this.AdministrationModel.seUsuario_GetUserbyID(userID);
                    string email = user.CorreoElectronico.Value;
                    string formName = this.GetResource(LanguageTypes.Forms, documentID.ToString());

                    string subject = string.Empty;
                    string subjectRsx = string.Empty;
                    string body = string.Empty;
                    string bodyRsx = string.Empty;
                    #endregion
                    #region Envia el mail
                    switch (mailType)
                    {
                        case MailType.Approve:
                            #region Documento de aprobacion
                            subjectRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Approved_Subject);
                            bodyRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Approved_Body);

                            subject = string.Format(subjectRsx, formName);
                            body = string.Format(bodyRsx, alarma.DocumentoID, alarma.DocumentoDesc, alarma.TerceroID, alarma.TerceroDesc, alarma.PrefijoID, alarma.Consecutivo, alarma.FileName);

                            this.SendMail(documentID, subject, body, email);
                            #endregion
                            break;
                        case MailType.Reject:
                            #region Documento de rechazo
                            subjectRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Rejected_Subject);
                            bodyRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Rejected_Body);

                            subject = string.Format(subjectRsx, formName);
                            body = string.Format(bodyRsx, alarma.DocumentoID, alarma.DocumentoDesc, alarma.TerceroID, alarma.TerceroDesc, alarma.PrefijoID, alarma.Consecutivo, alarma.FileName);

                            this.SendMail(documentID, subject, body, email);
                            #endregion
                            break;
                        case MailType.NewDoc:
                            #region Nuevo documento
                            subjectRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocCreated_Subject);
                            bodyRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocCreated_Body);

                            subject = string.Format(subjectRsx, formName);
                            body = string.Format(bodyRsx, alarma.DocumentoID, alarma.DocumentoDesc, alarma.TerceroID, alarma.TerceroDesc, alarma.PrefijoID, alarma.Consecutivo);

                            this.SendMail(documentID, subject, body, email);
                            #endregion 
                            break;
                        case MailType.SendToApprove:
                            #region Documento que envia para aprobacion
                            subjectRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
                            bodyRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Body);

                            subject = string.Format(subjectRsx, formName);
                            body = string.Format(bodyRsx, alarma.DocumentoID, alarma.DocumentoDesc, alarma.TerceroID, alarma.TerceroDesc, alarma.PrefijoID, alarma.Consecutivo, alarma.FileName);

                            this.SendMail(documentID, subject, body, email);
                            #endregion 
                            break;
                        case MailType.Assign:
                            #region Asignacion
                            subjectRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Assigned_Subject);
                            bodyRsx = this.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Assigned_Body);

                            subject = string.Format(subjectRsx, documentID == AppDocuments.SolicitudAsign? this.GetResource(LanguageTypes.Forms, AppDocuments.Solicitud.ToString()) : formName);
                            body = string.Format(bodyRsx, formName, formName, alarma.PrefijoID, alarma.Consecutivo,DateTime.Now.ToString());
                            this.SendMail(documentID, subject, body, email);
                            #endregion
                            break;
                    }
                    #endregion
                }

                if (showResult)
                {
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                }
                #endregion

                return result.Result == ResultValue.OK ? true : false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        #endregion

        #region Empresas

        /// <summary>
        /// Asigna al administration model la informacion de las tablas segun el grupo de empresas en el que se encuentre
        /// </summary>
        /// <param name="empGrupo">Grupo de empresas</param>
        internal void AssignTablesByCompany(Dictionary<int, string> empGrupo)
        {
            try
            {
                this.AdministrationModel.Tables = new Dictionary<Tuple<int, string>, DTO_glTabla>();

                var tables = this.AdministrationModel.glTabla_GetAllByEmpresaGrupo(empGrupo);
                foreach (var t in tables)
                {
                    Tuple<int, string> tup = new Tuple<int, string>(t.DocumentoID.Value.Value, t.EmpresaGrupoID.Value);
                    this.AdministrationModel.Tables.Add(tup, t);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.GetResourceForException(ex, "WinApp-BaseController.cs", "AssignTablesByCompany"));
            }
        }

        /// <summary>
        /// Trae el grupo de empresas de acuerdo al un documento
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <returns>Retorna el grupo de empresas</returns>
        internal string GetMaestraEmpresaGrupoByDocumentID(int documentID)
        {
            GrupoEmpresa seguridad = this.AdministrationModel.MasterProperties[documentID].TipoSeguridad;

            string empGrupo = string.Empty;
            if (seguridad == GrupoEmpresa.Automatico)
            {
                empGrupo = this.AdministrationModel.Empresa.ID.Value;
            }
            else if (seguridad == GrupoEmpresa.Individual)
            {
                empGrupo = this.AdministrationModel.Empresa.EmpresaGrupoID_.Value;
            }
            else
            {
                empGrupo = this.AdministrationModel.EmpresaGrupoGeneral;
            }

            return empGrupo;
        }

        #endregion

        #region Importacion

        /// <summary>
        /// Genera un formato de importación dado un dto
        /// Utiliza el atributo [NotImportable] Para descartar los campos que no se deben poner en el formato
        /// </summary>
        /// <param name="dtoType">Tipo del documento a exportar</param>
        /// <param name="docId">Identificador del documento que se esta exportando</param>
        /// <param name="separator">Caracter que separa las columnas</param>
        /// <returns></returns>
        internal string GetImportExportFormat(Type dtoType, int docId, string separator = "\t", bool resources = true)
        {
            string format = string.Empty;
            PropertyInfo[] pis = dtoType.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                {
                    string pName = pi.Name;
                    if (resources)
                    {
                        string colID = docId.ToString() + "_" + pi.Name;
                        pName = this.GetResource(LanguageTypes.Forms, colID);
                    }
                    if (string.IsNullOrWhiteSpace(format))
                    {
                        format = pName;
                    }
                    else
                    {
                        format += separator;
                        format += pName;
                    }
                }
            }
            return format;
        }

        #endregion

        #region Maestras

        /// <summary>
        /// Obtiene la descripcion de una maestra
        /// </summary>
        /// <param name="onlyRoots">Indica si solo acepta raices para una jerarquica</param>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="colIDName">Npmbre de la PK</param>
        /// <param name="colIDVal">Valor de la PK</param>
        /// <returns>Retorna la descripcion del elemento dado el codigo</returns>
        private Tuple<string, string> GetMasterDescriptionByID(bool onlyRoots, int documentID, string colIDName, string colIDVal, List<DTO_glConsultaFiltro> filtros = null)
        {
            bool valueInt = documentID == AppMasters.glDocumento ? true : false;

            //Si es numérico revisa que no permita letras
            if (valueInt)
            {
                int val = 0;
                bool isInt = Int32.TryParse(colIDVal, out val);
                if(!isInt)
                    return new Tuple<string, string>(string.Empty, string.Empty); 
            }

            object fkDTO = this.GetMasterDTO(AppMasters.MasterType.Simple, documentID, valueInt, colIDVal, true, filtros);
            DTO_MasterBasic basic = null;
            bool validRoot = true;
            string newCode = colIDVal;

            //Si es jerarquica
            if (fkDTO != null && fkDTO is DTO_MasterHierarchyBasic)
            {
                if ((fkDTO as DTO_MasterHierarchyBasic).MovInd.Value == false && onlyRoots)
                    validRoot = false;
                if (fkDTO is DTO_coPlanCuenta)
                {
                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)fkDTO;
                    int mask = cta.MascaraCta.Value.Value;

                    if (colIDVal.Length == mask)
                    {
                        string maskStr = string.Empty;
                        Tuple<int, string> tup = new Tuple<int, string>(documentID, cta.EmpresaGrupoID.Value);
                        DTO_glTabla tabla = this._administrationModel.Tables[tup];
                        mask = tabla.CodeLength(tabla.LevelsUsed()) - mask;
                        for (int i = 0; i < mask; ++i)
                        {
                            maskStr += "0";
                        }
                        newCode = colIDVal + maskStr;
                        validRoot = true;
                        fkDTO = this.GetMasterDTO(AppMasters.MasterType.Simple, documentID, valueInt, newCode, true, filtros);
                    }
                }
            }

            //Carga el valor 
            basic = fkDTO != null ? (DTO_MasterBasic)fkDTO : null;
            if (fkDTO == null || basic.ID == null || string.IsNullOrEmpty(basic.ID.Value) || !validRoot)
            {
                return new Tuple<string, string>(string.Empty, string.Empty);
            }
            else
            {
                return new Tuple<string, string>(basic.Descriptivo.Value, newCode);
            }
        }

        /// <summary>
        /// Trae un DTO de una maestra simple o jerarquica
        /// </summary>
        /// <param name="tipo">Tipo de maestra</param>
        /// <param name="docId">Id del documento</param>
        /// <param name="isValueInt">Identifica si el id es entero</param>
        /// <param name="value">Valor del DTO</param>
        /// <returns>Retorna el DTO requerido</returns>
        internal Object GetMasterDTO(AppMasters.MasterType tipo, int docId, bool isValueInt, string value, bool active, List<DTO_glConsultaFiltro> filtros = null)
        {
            Object obj = null;

            if (tipo == AppMasters.MasterType.Simple)
            {
                UDT_BasicID udt = new UDT_BasicID() { Value = value, IsInt = isValueInt };
                obj = AdministrationModel.MasterSimple_GetByID(docId, udt, active, filtros);
            }
            else if (tipo == AppMasters.MasterType.Hierarchy)
            {
                UDT_BasicID udt = new UDT_BasicID() { Value = value, IsInt = isValueInt };
                obj = AdministrationModel.MasterHierarchy_GetByID(docId, udt, active, filtros);
            }

            return obj;
        }

        /// <summary>
        /// Trae un DTO de una maestra simple o jerarquica
        /// </summary>
        /// <param name="tipo">Tipo de maestra</param>
        /// <param name="docId">Id del documento</param>
        /// <param name="isValueInt">Identifica si el id es entero</param>
        /// <param name="value">Valor del DTO</param>
        /// <returns>Retorna el DTO requerido</returns>
        internal DTO_MasterComplex GetMasterComplexDTO(int docId, Dictionary<string, string> pks, bool active)
        {
            return AdministrationModel.MasterComplex_GetByID(docId, pks, active);
        }

        /// <summary>
        /// Devuelve la informacion de una maestar basado en el nombre de la llave primaria
        /// </summary>
        /// <param name="colId"></param>
        /// <returns></returns>
        internal DTO_aplMaestraPropiedades GetMasterPropertyByColId(string colId)
        {
            DTO_aplMaestraPropiedades mp = new DTO_aplMaestraPropiedades();

            List<DTO_aplMaestraPropiedades> list = (from prop in this.AdministrationModel.MasterProperties.Values where prop.ColumnaID == colId select prop).ToList();
            if (list.Count > 0)
                mp = list.First();

            return mp;
        }

        /// <summary>
        /// Crea una configuración de FK con una tabla
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        internal ButtonEditFKConfiguration CreateFKConfig(int docId)
        {
            string empGrupo = this.GetMaestraEmpresaGrupoByDocumentID(docId);
            Tuple<int, string> tup = new Tuple<int, string>(docId, empGrupo);
            DTO_aplMaestraPropiedades prop = null;
            DTO_glTabla table = null;
            if (this.AdministrationModel.Tables.TryGetValue(tup, out table) && (prop = this.AdministrationModel.MasterProperties[docId]) != null)
            {
                ForeignKeyFieldConfig fk = new ForeignKeyFieldConfig()
                {
                    CountMethod = "MasterSimple_Count",
                    DataMethod = "MasterSimple_GetPaged",
                    DataRowMethod = "MasterSimple_GetByID",
                    DescField = "Descriptivo",
                    KeyField = prop.ColumnaID,
                    ModalFormCode = docId.ToString()
                };

                fk.TableName = table.TablaNombre.Value;

                //Asignacion del campo
                ButtonEditFKConfiguration beFk = new ButtonEditFKConfiguration(typeof(string), this.AdministrationModel.MasterProperties[docId].IDLongitudMax, CharacterCasing.Normal, TextFieldType.Letters, true, fk);

                beFk.Casing = CharacterCasing.Upper;
                return beFk;
            }
            return null;
        }

        #endregion

        #region Mails

        /// <summary>
        /// Envia un mensaje segun la configuracion del sistema
        /// Mira si se envia desde el outlook del usuario o desde la cuenta de AMP
        /// </summary>
        /// <param name="subject">Asunto</param>
        /// <param name="body">Cuerpo</param>
        /// <param name="recipients">Destinatarios</param>
        /// <param name="emailSender">Indica el numero de la cuenta de correo origen que se usara para el envio(1-5)</param>
        internal DTO_TxResult SendMail(int document, string subject, string body, string recipients,byte emailSender = 1, List<string> attachedFiles = null)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                string outook = this.GetControlValue(AppControl.IndicadorOutlook);
                if (outook == "1")
                    MailUtility.SendOutlookMail(subject, body, recipients, attachedFiles);
                else
                {
                    //Parametros cuenta Email sender General
                    string smtp = ConfigurationManager.AppSettings["Mail.Smtp"];
                    string smtpUser = ConfigurationManager.AppSettings["Mail.Sender"];
                    string smtpMailFrom = ConfigurationManager.AppSettings["Mail.Sender"];
                    string smtpPassword = ConfigurationManager.AppSettings["Mail.PasswordSender"];
                    int port = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Mail.Port"]) ? Convert.ToInt32(ConfigurationManager.AppSettings["Mail.Port"]) : 25;
                    bool ssl = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Mail.SSL"]) ? Convert.ToBoolean(ConfigurationManager.AppSettings["Mail.SSL"]) : false;

                    //Valida la cuenta de Email sender (Para enviar desde la cuenta de correo correspondiente)
                    if (emailSender != 1)
                    {
                        smtp = ConfigurationManager.AppSettings["Mail.Smtp" + emailSender.ToString()];
                        smtpUser = ConfigurationManager.AppSettings["Mail.Sender" + emailSender.ToString()];
                        smtpMailFrom = ConfigurationManager.AppSettings["Mail.Sender" + emailSender.ToString()];
                        smtpPassword = ConfigurationManager.AppSettings["Mail.PasswordSender" + emailSender.ToString()];
                        port = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Mail.Port" + emailSender.ToString()]) ? Convert.ToInt32(ConfigurationManager.AppSettings["Mail.Port" + emailSender.ToString()]) : 25;
                        ssl = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Mail.SSL" + emailSender.ToString()]) ? Convert.ToBoolean(ConfigurationManager.AppSettings["Mail.SSL" + emailSender.ToString()]) : false;
                    }

                    //Valida si es cuenta de correo generica para asignar seguridad
                    ssl = smtp.Contains(".live.com") || smtp.Contains(".gmail.com") || smtp.Contains(".yahoo.com") ? true : ssl;

                    Random r = new Random(5);
                    r.Next();
                    string userToken = string.Empty; // this.AdministrationModel.User.ID.Value + document.ToString() + r.ToString(); 

                    MailUtility.SendMail(smtp, port, ssl, smtpUser, smtpPassword, smtpMailFrom, recipients, subject, body, userToken, attachedFiles);
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = this.GetResourceForException(ex, "WinApp-BaseController.cs", "SendMail" + "-Destinatarios:" + recipients);

                return result;
            }
        }

        #endregion

        #region Menu

        /// <summary>
        /// Devuelve el menu correspondiente a un modulo
        /// </summary>
        /// <param name="module">Nombre del modulo</param>
        /// <returns>Retorna un menu</returns>
        internal MainMenu GetMenu(ModulesPrefix module)
        {
            return this._administrationModel.Menus[module.ToString()];
        }

        #endregion

        #region Paginacion

        /// <summary>
        /// Inicializa el paginador
        /// </summary>
        /// <param name="pgControl">Control del paginador</param>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="text">Texto del paginador</param>
        /// <param name="records">Texto de los registros</param>
        internal void Pagging_Init(uc_Pagging pgControl, int pageSize)
        {
            pgControl.PageSize = pageSize;
            pgControl.Text = this.GetResource(LanguageTypes.Messages, pgControl.TextKey);
            pgControl.RecordsKey = this.GetResource(LanguageTypes.Messages, pgControl.RecordsKey);
        }

        /// <summary>
        /// Asigna los eventos al paginador
        /// </summary>
        /// <param name="pgControl">Control del paginador</param>
        /// <param name="e">Evento</param>
        internal void Pagging_SetEvent(uc_Pagging pgControl, uc_Pagging.EventHandler e)
        {
            pgControl.FirstClick += new uc_Pagging.EventHandler(e);
            pgControl.PreviewPage_Click += new uc_Pagging.EventHandler(e);
            pgControl.NextPage_Click += new uc_Pagging.EventHandler(e);
            pgControl.LastPage_Click += new uc_Pagging.EventHandler(e);
        }

        #endregion

        #region Recursos

        /// <summary>
        /// Obtiene un recurso
        /// </summary>
        /// <param name="t">Tipo de recurso</param>
        /// <param name="v">Llave </param>
        /// <returns>Retorna el recurso seleccionado</returns>
        public string GetResource(LanguageTypes t, string v)
        {
            return LanguageManager.GetResource(_administrationModel.Languages, t, v);
        }

        /// <summary>
        /// Trae el recurso del mensaje correspondiente, al llegar un TxResult del servidor
        /// </summary>
        /// <param name="keyResource">Nombre de la llave del recurso</param>
        /// <returns>Recurso del mensaje</returns>
        internal string GetResourceFromResult(LanguageTypes t, string keyResource)
        {
            string[] arr = keyResource.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
            string[] args = new string[arr.Count() - 1];

            string key = this.GetResource(t, arr[0]);
            if (arr.Count() > 1)
            {
                for (int i = 1; i < arr.Count(); ++i)
                {
                    args[i - 1] = arr[i];
                }
            }

            string value = string.Format(key, args);
            return value;
        }

        /// <summary>
        /// Retorna el mensaje de error correspondiente para mostrar al usuario
        /// </summary>
        /// <param name="ex">Excepción</param>
        /// <param name="location">Orignen  del eror ej. Application</param>
        /// <param name="method">Método donde se presenta el error</param>
        /// <returns>Mensaje controlado</returns>
        internal string GetResourceForException(Exception ex, string location, string method)
        {
            string error = string.Empty;

            if (this.AdministrationModel.BasicConfig == null ||
                this.AdministrationModel.BasicConfig.RemotoInd == null ||
                !this.AdministrationModel.BasicConfig.RemotoInd.Value.HasValue ||
                this.AdministrationModel.BasicConfig.RemotoInd.Value.Value)
            {
                error = Mentor_Exception.LogException_Service(ex, location, method, false);
            }
            else
            {
                string loggerConn = this.AdministrationModel.BasicConfig.CadenaConnLogger.Value;
                error = Mentor_Exception.LogException_Local(loggerConn, ex, location, method); 
            }


            if (error.StartsWith("ERR_"))
            {
                string ext = "";
                int place = error.Length;

                if (error.Contains("&&"))
                {
                    place = error.IndexOf("&&");
                    ext = error.Substring(place + 2);
                }

                var msg = this.GetResource(LanguageTypes.Error, error.Substring(0, place));

                if (msg.Equals(error.Substring(0, place)))
                {
                    if (ConfigurationManager.AppSettings[error.Substring(0, place)] != null)
                        return String.Format(ConfigurationManager.AppSettings[error.Substring(0, place)], ext);
                    else
                        return msg + String.Format(", La llave {0} hace falta en el archivo de configuración", error.Substring(0, place));
                }
                else
                {
                    string[] vars = Regex.Split(ext, "&&");
                    return String.Format(msg, vars);
                }
            }
            else
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Esta funcionretorna el mesanje estraido de los resources, permite pasar un m,ensaje en formato
        /// mentor Exception 
        /// </summary>
        /// <param name="msg">Mensaje del cual se quiere obtener un valor visible al user</param>
        /// <returns>Mensaje para el user a partir de un msn tipo exception </returns>
        internal string GetResourceError(string error)
        {
            if (error.StartsWith("ERR_"))
            {
                string ext = "";
                int place = error.Length;

                if (error.Contains("&&"))
                {
                    place = error.IndexOf("&&");
                    ext = error.Substring(place + 2);
                }

                var msg = this.GetResource(LanguageTypes.Error, error.Substring(0, place));
                if (msg.Equals(error.Substring(0, place)))
                {
                    if (ConfigurationManager.AppSettings[error.Substring(0, place)] != null)
                        return String.Format(ConfigurationManager.AppSettings[error.Substring(0, place)], ext.Trim());
                    else
                        return msg;
                }
                else
                {
                    var parametros = ext.Split(new string[] { "&&" }, StringSplitOptions.None);
                    return String.Format(msg, parametros);
                }
            }
            else
                return this.GetResource(LanguageTypes.Error, error);
        }

        /// <summary>
        /// Asigna los recursos a un objecto de tipo resultado
        /// </summary>
        /// <param name="result">Resultsdo</param>
        internal void AssignResultResources(int? documentID, DTO_TxResult result)
        {
            try
            {
                if (!string.IsNullOrEmpty(result.ResultMessage))
                    result.ResultMessage = this.GetResourceError(result.ResultMessage);

                if (result.Details == null)
                    result.Details = new List<DTO_TxResultDetail>();

                #region Detalles
                foreach (DTO_TxResultDetail detalle in result.Details)
                {
                    if (!string.IsNullOrWhiteSpace(detalle.Message) && detalle.Message != "OK")
                    {
                        detalle.Message = this.GetResourceError(detalle.Message);
                        if (detalle.Message == detalle.Message)
                            detalle.Message = this.GetResource(LanguageTypes.Messages, detalle.Message);
                    }

                    #region Campos
                    if (detalle.DetailsFields != null)
                    {
                        string ext = "";
                        foreach (DTO_TxResultDetailFields campo in detalle.DetailsFields)
                        {
                            if (campo.Message.ToUpper().StartsWith("ERR"))
                                campo.Message = this.GetResourceError(campo.Message);
                            else
                            {
                                int place = campo.Message.Length;
                                if (campo.Message.Contains("&&"))
                                {
                                    place = campo.Message.IndexOf("&&");
                                    ext = campo.Message.Substring(place + 2);
                                }

                                string msg = this.GetResource(LanguageTypes.Messages, campo.Message.Substring(0, place));
                                var parametros = ext.Split(new string[] { "&&" }, StringSplitOptions.None);
                                campo.Message = String.Format(msg, parametros);
                            }

                            if (documentID.HasValue)
                                campo.Field = this.GetResource(LanguageTypes.Forms, documentID.Value + "_" + campo.Field);
                        }
                    }
                    #endregion

                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err: " + ex.Message);
            }
        }

        /// <summary>
        /// Asigna los recursos a un objecto de tipo resultado
        /// </summary>
        /// <param name="result">Resultsdo</param>
        internal void AssignResultResources(int? documentID, List<DTO_TxResult> results)
        {
            foreach (DTO_TxResult result in results)
            {
                this.AssignResultResources(documentID, result);
            }
        }

        #endregion

        #region Validaciones

        /// <summary>
        /// Valida una celda que tiene una llave Foranea
        /// </summary>
        /// <param name="gv">Gridview</param>
        /// <param name="unbPrefix">Prefijo de la columna, si es la de por defecto se envia vacio</param>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <param name="colName">Nombre de la columna sin el unbound</param>
        /// <param name="acceptNull">Indica si la celda acepta valores vacios o no</param>
        /// <param name="isFK">Indica si la celda corresponde a una llave foranea</param>
        /// <param name="isHierarchy">Indica si es un control de jerarquia</param>
        /// <param name="FKDocID">Documento Id de la FK</param>
        /// <param name="onlyRoot">Para las fks jerarquicas indica si solo puede traer datos de la raiz</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        internal bool ValidGridCell(GridView gv, string unbPrefix, int fila, string colName, bool acceptNull, bool isFK, bool isHierarchy, int? FKDocID, bool onlyRoot = true)
        {
            string unboundPrefix = string.IsNullOrWhiteSpace(unbPrefix) ? "Unbound_" : unbPrefix;

            string rsxEmpty = this.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string rsxFK = this.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
            string rsxNotLeaf = this.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotLeaf);

            string msg;
            GridColumn col = gv.Columns[unboundPrefix + colName];
            var val = gv.GetRowCellValue(fila, col) != null ? gv.GetRowCellValue(fila, col) : string.Empty;
            string colVal = val.ToString();

            if (string.IsNullOrEmpty(colVal) && !acceptNull)
            {
                msg = string.Format(rsxEmpty, col.Caption);
                gv.SetColumnError(col, msg);
                return false;
            }
            else if (!string.IsNullOrEmpty(colVal) && isFK)
            {
                DTO_MasterBasic dto;
                if (isHierarchy)
                    dto = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, FKDocID.Value, false, colVal, true);
                else
                    dto = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, FKDocID.Value, false, colVal, true);

                if (dto == null)
                {
                    msg = string.Format(rsxFK, colVal);
                    gv.SetColumnError(col, msg);
                    return false;
                }
                else if (isHierarchy && onlyRoot)
                {
                    DTO_MasterHierarchyBasic h = (DTO_MasterHierarchyBasic)dto;
                    if (!h.MovInd.Value.Value)
                    {
                        msg = string.Format(rsxNotLeaf, colVal);
                        gv.SetColumnError(col, msg);
                        return false;
                    }
                }
            }

            gv.SetColumnError(col, string.Empty);
            return true;
        }

        /// <summary>
        /// Valida una celda que tiene una llave Foranea
        /// </summary>
        /// <param name="colRsx">Nombre de la columna sin el unbound</param>
        /// <param name="colVal">Valor a validar</param>
        /// <param name="acceptNull">Indica si la celda acepta valores vacios o no</param>
        /// <param name="isFK">Indica si la celda corresponde a una llave foranea</param>
        /// <param name="isHierarchy">Indica si es un control de jerarquia</param>
        /// <param name="FKDocID">Documento Id de la FK</param>
        /// <param name="onlyRoot">Para las fks jerarquicas indica si solo puede traer datos de la raiz</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        internal DTO_TxResultDetailFields ValidGridCell(string colRsx, string colVal, bool acceptNull, bool isFK, bool isHierarchy, int? FKDocID, bool onlyRoot = true)
        {
            DTO_TxResultDetailFields rdf = new DTO_TxResultDetailFields();
            rdf.Field = colRsx;

            string rsxEmpty = this.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string rsxFK = this.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
            string rsxNotLeaf = this.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotLeaf);

            if (string.IsNullOrEmpty(colVal) && !acceptNull)
                rdf.Message = string.Format(rsxEmpty, colRsx);
            else if (!string.IsNullOrEmpty(colVal) && isFK)
            {
                DTO_MasterBasic dto;
                if (isHierarchy)
                    dto = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, FKDocID.Value, false, colVal, true);
                else
                    dto = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, FKDocID.Value, false, colVal, true);

                if (dto == null)
                    rdf.Message = string.Format(rsxFK, colVal);
                else if (isHierarchy && onlyRoot)
                {
                    DTO_MasterHierarchyBasic h = (DTO_MasterHierarchyBasic)dto;
                    if (!h.MovInd.Value.Value)
                        rdf.Message = string.Format(rsxNotLeaf, colVal); 
                }
            }

            if (string.IsNullOrWhiteSpace(rdf.Message))
                rdf = null;

            return rdf;
        }

        /// <summary>
        /// Valida una celda de valor es valida
        /// </summary>
        /// <param name="gv">Gridview</param>
        /// <param name="unbPrefix">Prefijo de la columna, si es la de por defecto se envia vacio</param>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <param name="colName">Nombre de la columna sin el unbound</param>
        /// <param name="acceptNull">Indica si la celda acepta valores vacios o no</param>
        /// <param name="acceptCero">Indica si la celda acepta ceros como valor</param>
        /// <param name="OnlyPositive">Indica si la celda acepta solo numeros positivos</param>
        /// <param name="invalidImp">Indica si hay error en los impuestos</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        internal bool ValidGridCellValue(GridView gv, string unbPrefix, int fila, string colName, bool acceptNull, bool acceptCero, bool OnlyPositive, bool invalidImp)
        {
            string unboundPrefix = string.IsNullOrWhiteSpace(unbPrefix) ? "Unbound_" : unbPrefix;

            string rsxEmpty = this.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string rsxDouble = this.GetResource(LanguageTypes.Messages, DictionaryMessages.DoubleField);
            string rsxCero = this.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField);
            string rsxPositive = this.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
            string rsxInvalidImp = this.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidImpValue);

            string msg;
            GridColumn col = gv.Columns[unboundPrefix + colName];
            var v = gv.GetRowCellValue(fila, col) != null ? gv.GetRowCellValue(fila, col) : string.Empty;
            string colVal = v.ToString();

            if (string.IsNullOrEmpty(colVal) && !acceptNull)
            {
                msg = string.Format(rsxEmpty, col.Caption);
                gv.SetColumnError(col, msg);
                return false;
            }
            else if (!string.IsNullOrEmpty(colVal))
            {
                decimal val = 0;
                try
                {
                    val = Convert.ToDecimal(colVal, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    msg = string.Format(rsxDouble, col.Caption);
                    gv.SetColumnError(col, msg);
                    return false;
                }

                if (!acceptCero && val == 0)
                {
                    msg = string.Format(rsxCero, col.Caption);
                    gv.SetColumnError(col, msg);
                    return false;
                }

                if (OnlyPositive && val < 0)
                {
                    msg = string.Format(rsxPositive, col.Caption);
                    gv.SetColumnError(col, msg);
                    return false;
                }

                if (invalidImp)
                {
                    gv.SetColumnError(col, rsxInvalidImp);
                    return false;
                }
            }

            gv.SetColumnError(col, string.Empty);
            return true;
        }

        /// <summary>
        /// Valida una celda de valor es valida
        /// </summary>
        /// <param name="colRsx">Nombre de la columna sin el unbound</param>
        /// <param name="colVal">Valor a validar</param>
        /// <param name="acceptNull">Indica si la celda acepta valores vacios o no</param>
        /// <param name="acceptCero">Indica si la celda acepta ceros como valor</param>
        /// <param name="OnlyPositive">Indica si la celda acepta solo numeros positivos</param>
        /// <param name="invalidImp">Indica si hay error en los impuestos</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        internal DTO_TxResultDetailFields ValidGridCellValue(string colRsx, string colVal, bool acceptNull, bool acceptCero, bool OnlyPositive, bool invalidImp)
        {
            DTO_TxResultDetailFields rdf = new DTO_TxResultDetailFields();
            rdf.Field = colRsx;
            
            string rsxEmpty = this.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string rsxDouble = this.GetResource(LanguageTypes.Messages, DictionaryMessages.DoubleField);
            string rsxCero = this.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField);
            string rsxPositive = this.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
            string rsxInvalidImp = this.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidImpValue);

            if (string.IsNullOrEmpty(colVal) && !acceptNull)
                rdf.Message = string.Format(rsxEmpty, colRsx);
            else if (!string.IsNullOrEmpty(colVal))
            {
                decimal val = 0;
                try
                {
                    val = Convert.ToDecimal(colVal, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    rdf.Message = string.Format(rsxDouble, colVal);
                    return rdf;
                }

                if (!acceptCero && val == 0)
                    rdf.Message = string.Format(rsxCero, colVal);
                else if (OnlyPositive && val < 0)
                    rdf.Message = string.Format(rsxPositive, colVal);
                else if (invalidImp)
                    rdf.Message = rsxInvalidImp;
            }

            if (string.IsNullOrWhiteSpace(rdf.Message))
                rdf = null;

            return rdf;
        }

        /// <summary>
        /// Valida un campo que corresponde a MasterComplex
        /// </summary>
        /// <param name="gv">Gridview</param>
        /// <param name="unbPrefix">Prefijo de la columna, si es la de por defecto se envia vacio</param>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <param name="colName">Nombre de la columna sin el unbound</param>
        /// <param name="pks">Primary keys que corresponden a MasterComplex</param>
        /// <param name="acceptNull">Indica si la celda acepta valores vacios o no</param>
        /// <param name="FKDocID">Documento Id de la FK</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        internal bool ValidGridCellComplex(GridView gv, string unbPrefix, int fila, string colName, Dictionary<string, string> pks, bool acceptNull, int? FKDocID)
        {
            string unboundPrefix = string.IsNullOrWhiteSpace(unbPrefix) ? "Unbound_" : unbPrefix;

            string rsxEmpty = this.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string rsxFK = this.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
            string rsxFKEmpty = "{0} esta vacío.";//this.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
            
            string msg;
            GridColumn col = gv.Columns[unboundPrefix + colName];
            string colVal = gv.GetRowCellValue(fila, col).ToString();

            if (string.IsNullOrEmpty(colVal) && !acceptNull)
            {
                msg = string.Format(rsxEmpty, col.Caption);
                gv.SetColumnError(col, msg);
                return false;
            }
            else if (!string.IsNullOrEmpty(colVal))
            {
                if (pks.Values.Contains(string.Empty))
                {
                    msg = string.Format(rsxFKEmpty, pks.First(x=>x.Value==string.Empty).Key);
                    gv.SetColumnError(col, msg);
                    return false;
                }
                else
                {
                    DTO_MasterComplex dto = this.GetMasterComplexDTO(FKDocID.Value, pks, true);

                    if (dto == null)
                    {
                        msg = string.Format(rsxFK, colVal);
                        gv.SetColumnError(col, msg);
                        return false;
                    }
                }
            }

            gv.SetColumnError(col, string.Empty);
            return true;
        }

        /// <summary>
        /// Valida un campo que corresponde a MasterComplex
        /// </summary>
        /// <param name="colRsx">Nombre de la columna sin el unbound</param>
        /// <param name="colVal">Valor a validar</param>
        /// <param name="pks">Primary keys que corresponden a MasterComplex</param>
        /// <param name="acceptNull">Indica si la celda acepta valores vacios o no</param>
        /// <param name="FKDocID">Documento Id de la FK</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        internal DTO_TxResultDetailFields ValidGridCellComplex(string colRsx, string colVal, Dictionary<string, string> pks, bool acceptNull, int? FKDocID)
        {
            DTO_TxResultDetailFields rdf = new DTO_TxResultDetailFields();
            rdf.Field = colRsx;

            string rsxEmpty = this.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string rsxFK = this.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
            string rsxFKEmpty = "{0} esta vacío.";

            if (string.IsNullOrEmpty(colVal) && !acceptNull)
                rdf.Message = string.Format(rsxEmpty, colRsx);
            else if (!string.IsNullOrEmpty(colVal))
            {
                if (pks.Values.Contains(string.Empty))
                    rdf.Message = string.Format(rsxFKEmpty, pks.First(x => x.Value == string.Empty).Key);
                else
                {
                    DTO_MasterComplex dto = this.GetMasterComplexDTO(FKDocID.Value, pks, true);

                    if (dto == null)
                        rdf.Message = string.Format(rsxFK, colVal);
                }
            }

            if (string.IsNullOrWhiteSpace(rdf.Message))
                rdf = null;

            return rdf;
        }

        #endregion

        #endregion

        #region Implementacion de CommonReportDataSupplier

        /// <summary>
        /// Campo descriptivo de la empresa
        /// </summary>
        /// <returns>Retorna la descripcion de la empresa sobre la que se esta trabajando</returns>
        public string GetNombreEmpresa()
        {
            return this.AdministrationModel.Empresa.Descriptivo.Value;
        }

        /// <summary>
        /// Logo de la empresa
        /// </summary>
        /// <returns>Retorna un arreglo de bites con la info del logo de la empresa sobre la que se esta trabajando</returns>
        public byte[] GetLogoEmpresa()
        {
            return this.AdministrationModel.glEmpresaLogo();
        }

        /// <summary>
        /// Nombre del usuario
        /// </summary>
        /// <returns>Retorna el descriptivo del usuario que tiene la sesion abierta en el sistema</returns>
        public string GetUserName()
        {
            return this.AdministrationModel.User.Descriptivo.Value;
        }

        /// <summary>
        /// Nit de la empresa
        /// </summary>
        /// <returns>Retorna Nit de la empresa sobre la que se esta trabajando</returns>
        public string GetNitEmpresa()
        {
            return this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
        }

        #endregion
    }
}
