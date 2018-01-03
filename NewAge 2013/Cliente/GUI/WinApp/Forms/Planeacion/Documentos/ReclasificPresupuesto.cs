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
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.Cliente.GUI.WinApp.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using SentenceTransformer;
using NewAge.DTO.Resultados;
using NewAge.DTO.Attributes;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ReclasificPresupuesto : DocumentPresupuesto
    {
        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ReclasificPresupuesto()
        {
            try
            {
                //InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GenerarPresupuesto.cs", "GenerarPresupuesto"));
            }
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.ReclasifPresupuesto;
                this._frmModule = ModulesPrefix.pl;
                base.SetInitParameters();
             
                this.format = _bc.GetImportExportFormat(typeof(DTO_plPresupuestoDeta), this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasifPresupuesto.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Carga la info del formulario
        /// </summary>
        protected override void LoadData()
        {
            try
            {
                if (this.masterProyecto.ValidID)
                {
                    #region Valida el Proyecto
                    DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.masterProyecto.Value, true);
                    DTO_glLocFisica locFisica = null;
                    if (string.IsNullOrEmpty(proy.LocFisicaID.Value))
                    {
                        MessageBox.Show("El proyecto no cuenta con Loc fisica definida");
                        base.validHeader = false;
                        return;
                    }
                    else
                    {
                        locFisica = (DTO_glLocFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, false, proy.LocFisicaID.Value, true);
                        if (string.IsNullOrEmpty(locFisica.AreaFisica.Value))
                        {
                            MessageBox.Show("La loc fisica del proyecto no cuenta con Area Fisica definida");
                            base.validHeader = false;
                            return;
                        }
                        base.locFisicaID = proy.LocFisicaID.Value;
                        base.areaFisicaID = locFisica.AreaFisica.Value;
                        base.masterCampo.Value = base.areaFisicaID;
                    }
                    if (string.IsNullOrEmpty(proy.ActividadID.Value))
                    {
                        MessageBox.Show("El proyecto no cuenta con Actividad definida");
                        base.validHeader = false;
                        return;
                    }
                    base.actividadID = proy.ActividadID.Value;
                    base.masterActividad.Value = base.actividadID;
                    base.masterContrato.Value = proy.ContratoID.Value;
                    #endregion

                    this.initData = true;
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    this.presupuesto = this._bc.AdministrationModel.Presupuesto_GetConsolidadoTotal(AppDocuments.Presupuesto, this.masterProyecto.Value, this.periodo,
                                            Convert.ToByte(base.cmbTipoProyecto.EditValue), this.masterContrato.Value, this.masterActividad.Value, this.masterCampo.Value);

                    if (presupuesto == null)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_PresupuestoNotExist));
                        FormProvider.Master.itemSave.Enabled = false;
                        this.validHeader = false;
                        this.proyectoID = string.Empty;
                        return;
                    }
                    else
                    {
                        if (this.presupuesto.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EstateInvalid));
                            FormProvider.Master.itemSave.Enabled = false;
                            this.validHeader = false;
                            return;
                        }                      
                        #region Carga Encabezado
                        this.proyectoID = this.masterProyecto.Value;
                        this.dtPeriod.DateTime = this.periodo;
                        this.txtTasaCambio.EditValue = this.presupuesto.DocCtrl.TasaCambioDOCU.Value.Value;
                        base.numeroDocPresup = presupuesto.DocCtrl.NumeroDoc.Value.Value;
                        #endregion
                        #region Verifica si Existen Adiciones
                        DTO_glDocumentoControl filter = new DTO_glDocumentoControl();
                        filter.DocumentoID.Value = AppDocuments.AdicionPresupuesto;
                        filter.ProyectoID.Value = this.masterProyecto.Value;
                        filter.PeriodoDoc.Value = this.dtPeriod.DateTime;
                        List<DTO_glDocumentoControl> listDocControl = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(filter);
                        foreach (DTO_glDocumentoControl doc in listDocControl)
                        {
                            if (doc.Estado.Value != (byte)EstadoDocControl.Aprobado)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_DocAdicionPendientes));
                                this.validHeader = false;
                                return;
                            }
                        } 
                        #endregion
                        #region Carga Detalle(Verifica si existe la reclasificacion)
                        DTO_Presupuesto adicionExist = this._bc.AdministrationModel.Presupuesto_GetNuevo(AppDocuments.ReclasifPresupuesto, this.masterProyecto.Value, this.periodo);
                        if (adicionExist == null || adicionExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado || adicionExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Anulado || adicionExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Revertido)
                        {
                            int i = 1;
                            foreach (DTO_plPresupuestoDeta item in presupuesto.Detalles)
                            {
                                DTO_plPresupuestoDeta presNew = new DTO_plPresupuestoDeta(true);
                                DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, item.LineaPresupuestoID.Value, true);
                                DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, item.CentroCostoID.Value, true);
                                presNew.ProyectoID.Value = item.ProyectoID.Value;
                                presNew.ActividadID.Value = item.ActividadID.Value;
                                presNew.LocFisicaID.Value = item.LocFisicaID.Value;
                                presNew.AreaFisicaID.Value = locFisica != null ? locFisica.AreaFisica.Value : string.Empty;
                                presNew.LineaPresupuestoID.Value = lineaPres.ID.Value;
                                presNew.LineaPresDesc.Value = lineaPres.Descriptivo.Value;
                                presNew.CentroCostoID.Value = centroCto.ID.Value;
                                presNew.CentroCostoDesc.Value = centroCto.Descriptivo.Value;
                                presNew.VlrSaldoAntLoc.Value = item.VlrSaldoAntLoc.Value;
                                presNew.VlrSaldoAntExtr.Value = item.VlrSaldoAntExtr.Value;
                                presNew.VlrNuevoSaldoLoc.Value = item.VlrNuevoSaldoLoc.Value;
                                presNew.VlrNuevoSaldoExtr.Value = item.VlrNuevoSaldoExtr.Value;
                                presNew.Porcentaje01.Value = item.Porcentaje01.Value;
                                presNew.Porcentaje02.Value = item.Porcentaje02.Value;
                                presNew.Porcentaje03.Value = item.Porcentaje03.Value;
                                presNew.Porcentaje04.Value = item.Porcentaje04.Value;
                                presNew.Porcentaje05.Value = item.Porcentaje05.Value;
                                presNew.Porcentaje06.Value = item.Porcentaje06.Value;
                                presNew.Porcentaje07.Value = item.Porcentaje07.Value;
                                presNew.Porcentaje08.Value = item.Porcentaje08.Value;
                                presNew.Porcentaje09.Value = item.Porcentaje09.Value;
                                presNew.Porcentaje10.Value = item.Porcentaje10.Value;
                                presNew.Porcentaje11.Value = item.Porcentaje11.Value;
                                presNew.Porcentaje12.Value = item.Porcentaje12.Value;
                                presNew.LoadParticionLocalInd = false;
                                presNew.LoadParticionExtrInd = false;
                                presNew.NewRowPresup = false;
                                presNew.Consecutivo.Value = i;
                                this.detList.Add(presNew);
                                i++;
                            }
                            this.presupuesto = null;
                        }
                        else
                        {
                            int i = 1;
                            foreach (DTO_plPresupuestoDeta item in adicionExist.Detalles)
                            {
                                DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, item.LineaPresupuestoID.Value, true);
                                DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, item.CentroCostoID.Value, true);
                                item.LineaPresDesc.Value = lineaPres.Descriptivo.Value;
                                item.CentroCostoDesc.Value = centroCto.Descriptivo.Value;
                                //item.VlrSaldoAntLoc.Value = this.presupuesto.Detalles.Where(x => x.LineaPresupuestoID.Value == lineaPres.ID.Value && x.CentroCostoID.Value == centroCto.ID.Value).First().VlrSaldoAntLoc.Value;
                                //item.VlrSaldoAntExtr.Value = this.presupuesto.Detalles.Where(x => x.LineaPresupuestoID.Value == lineaPres.ID.Value && x.CentroCostoID.Value == centroCto.ID.Value).First().VlrSaldoAntExtr.Value;
                                item.VlrMvtoLocal.Value = Math.Round(item.ValorLoc00.Value.Value + item.ValorLoc01.Value.Value + item.ValorLoc02.Value.Value + item.ValorLoc03.Value.Value
                                                          + item.ValorLoc04.Value.Value + item.ValorLoc05.Value.Value + item.ValorLoc06.Value.Value + item.ValorLoc07.Value.Value + item.ValorLoc08.Value.Value
                                                          + item.ValorLoc09.Value.Value + item.ValorLoc10.Value.Value + item.ValorLoc11.Value.Value + item.ValorLoc12.Value.Value);
                                item.VlrMvtoExtr.Value = Math.Round(item.ValorExt00.Value.Value + item.ValorExt01.Value.Value + item.ValorExt02.Value.Value + item.ValorExt03.Value.Value
                                                         + item.ValorExt04.Value.Value + item.ValorExt05.Value.Value + item.ValorExt06.Value.Value + item.ValorExt07.Value.Value + item.ValorExt08.Value.Value
                                                         + item.ValorExt09.Value.Value + item.ValorExt10.Value.Value + item.ValorExt11.Value.Value + item.ValorExt12.Value.Value);
                                item.VlrNuevoSaldoLoc.Value = item.VlrSaldoAntLoc.Value + item.VlrMvtoLocal.Value;
                                item.VlrNuevoSaldoExtr.Value = item.VlrSaldoAntExtr.Value + item.VlrMvtoExtr.Value;
                                item.LoadParticionLocalInd = false;
                                item.LoadParticionExtrInd = false;
                                item.NewRowPresup = false;
                                item.Consecutivo.Value = i;
                                this.detList.Add(item);
                                i++;
                            }
                            this.presupuesto = adicionExist;                            
                        }
                        #endregion

                        this.validHeader = true;
                        this.dtPeriod.Enabled = false;
                        this.masterProyecto.EnableControl(false);
                    }
                    this.gcDetail.DataSource = this.detList;
                    this.gcDetail.RefreshDataSource();
                    this.isValid_Det = true;
                    this.LoadDetails(true);
                    this.initData = false;
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                    MessageBox.Show(string.Format(msg, this.masterProyecto.LabelRsx, this.masterProyecto.Value));
                    FormProvider.Master.itemSave.Enabled = false;
                    this.validHeader = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuesto.cs", "LoadData"));
            }
        }

        #endregion

        #region Barra de herramientas

        /// <summary>
        /// Boton para guardar
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDetail.PostEditor();
                this.gvDetail.Focus();
                if (this.detList.Count > 0)
                {
                    bool isValid = this.ValidateRow_Det();
                    foreach (var item in this.detList)
                    {
                        isValid = item.VlrMvtoLocal.Value != 0 || item.VlrMvtoExtr.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc00.Value != 0 || item.ValorExt00.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc01.Value != 0 || item.ValorExt01.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc02.Value != 0 || item.ValorExt02.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc03.Value != 0 || item.ValorExt03.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc04.Value != 0 || item.ValorExt04.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc05.Value != 0 || item.ValorExt05.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc06.Value != 0 || item.ValorExt06.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc07.Value != 0 || item.ValorExt07.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc08.Value != 0 || item.ValorExt08.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc09.Value != 0 || item.ValorExt09.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc10.Value != 0 || item.ValorExt10.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc11.Value != 0 || item.ValorExt11.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc12.Value != 0 || item.ValorExt12.Value != 0 ? true : false;
                        if (isValid) break;
                    }
                    if (isValid)
                    {
                        decimal vlrMvtoLocal = this.detList.Sum(x => x.VlrMvtoLocal.Value.Value);
                        decimal vlrMvtoExtr = this.detList.Sum(x => x.VlrMvtoExtr.Value.Value);
                        if (vlrMvtoLocal == 0 && vlrMvtoExtr == 0)
                        {
                            Thread process = new Thread(this.SaveThread);
                            process.Start();
                        }
                        else 
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.pl_SaldoMvtoInvalid));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuesto.cs", "TBSendtoAppr"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                this.gvDetail.PostEditor();
                this.gvDetail.Focus();
                if (this.detList.Count > 0)
                {
                    bool isValid = this.ValidateRow_Det();
                    foreach (var item in this.detList)
                    {
                        isValid = item.VlrMvtoLocal.Value != 0 || item.VlrMvtoExtr.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc00.Value != 0 || item.ValorExt00.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc01.Value != 0 || item.ValorExt01.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc02.Value != 0 || item.ValorExt02.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc03.Value != 0 || item.ValorExt03.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc04.Value != 0 || item.ValorExt04.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc05.Value != 0 || item.ValorExt05.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc06.Value != 0 || item.ValorExt06.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc07.Value != 0 || item.ValorExt07.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc08.Value != 0 || item.ValorExt08.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc09.Value != 0 || item.ValorExt09.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc10.Value != 0 || item.ValorExt10.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc11.Value != 0 || item.ValorExt11.Value != 0 ? true : false;
                        if (isValid) break;
                        isValid = item.ValorLoc12.Value != 0 || item.ValorExt12.Value != 0 ? true : false;
                        if (isValid) break;
                    }
                    if (isValid)
                    {
                        decimal vlrMvtoLocal = this.detList.Sum(x => x.VlrMvtoLocal.Value.Value);
                        decimal vlrMvtoExtr = this.detList.Sum(x => x.VlrMvtoExtr.Value.Value);
                        if (vlrMvtoLocal == 0 && vlrMvtoExtr == 0)
                        {
                            Thread process = new Thread(this.SendToApproveThread);
                            process.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuesto.cs", "TBSendtoAppr"));
            }
        } 
        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de importacion
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgInvalidFormat = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    DTO_plPresupuestoDeta presupuestoDet = null;
                    this.detList = new List<DTO_plPresupuestoDeta>();
                    bool createDTO = true;
                    bool validList = true;

                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<PropertyInfo> pisSupplMig = typeof(DTO_plPresupuestoDeta).GetProperties().ToList();

                    //Recorre el DTO de migracion y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pisSupplMig)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_" + pi.Name);
                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }
                        #endregion
                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (manda error si el numero de columnas al importar es menor al necesario)
                            if (line.Length < cols.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            presupuestoDet = new DTO_plPresupuestoDeta(true);
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion Formatos
                                        UDT udt;
                                        PropertyInfo pi = presupuestoDet.GetType().GetProperty(colName);
                                        udt = pi != null ? (UDT)pi.GetValue(presupuestoDet, null) : null;
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        #region Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colValue = colVal;
                                            colVals[colRsx] = colVal;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (colValue != string.Empty)
                                            {
                                                #region Valores de Fecha
                                                if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                                {
                                                    try
                                                    {
                                                        DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                                #region Valores Numericos
                                                else if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                                {
                                                    try
                                                    {
                                                        int val = Convert.ToInt32(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                                {
                                                    try
                                                    {
                                                        long val = Convert.ToInt64(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                                {
                                                    try
                                                    {
                                                        short val = Convert.ToInt16(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                                {
                                                    try
                                                    {
                                                        byte val = Convert.ToByte(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                                {
                                                    try
                                                    {
                                                        decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                        if (colValue.Trim().Contains(','))
                                                        {
                                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                            rdF.Field = colRsx;
                                                            rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                            rd.DetailsFields.Add(rdF);

                                                            createDTO = false;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        //Asigna el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                        #endregion
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "MigracionNewFact.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);
                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                createDTO = false;
                            }

                            if (createDTO)
                                this.detList.Add(presupuestoDet);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares
                    if (validList)
                    {
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        for (int index = 0; index < this.detList.Count; ++index)
                        {
                            #region Variables
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i+1;
                            rd.Message = "OK";
                            #endregion
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (this.detList.Count);
                            i++;
                            #endregion
                            presupuestoDet = this.detList[index];                            
                            #region Valida y Asigna Totales Ambas Monedas
                            //Total del movimiento(Local)
                            decimal TotalMdaLocal = Math.Round(presupuestoDet.ValorLoc00.Value.Value + presupuestoDet.ValorLoc01.Value.Value + presupuestoDet.ValorLoc02.Value.Value + presupuestoDet.ValorLoc03.Value.Value
                                                      + presupuestoDet.ValorLoc04.Value.Value + presupuestoDet.ValorLoc05.Value.Value + presupuestoDet.ValorLoc06.Value.Value + presupuestoDet.ValorLoc07.Value.Value + presupuestoDet.ValorLoc08.Value.Value
                                                      + presupuestoDet.ValorLoc09.Value.Value + presupuestoDet.ValorLoc10.Value.Value + presupuestoDet.ValorLoc11.Value.Value + presupuestoDet.ValorLoc12.Value.Value);
                            if (TotalMdaLocal != 0)
                                presupuestoDet.VlrSaldoAntLoc.Value = TotalMdaLocal;

                            if (this.loadME)
                            {
                                //Total del movimiento(Extr)
                                decimal TotalMdaExtr = Math.Round(presupuestoDet.ValorExt00.Value.Value + presupuestoDet.ValorExt01.Value.Value + presupuestoDet.ValorExt02.Value.Value + presupuestoDet.ValorExt03.Value.Value
                                                        + presupuestoDet.ValorExt04.Value.Value + presupuestoDet.ValorExt05.Value.Value + presupuestoDet.ValorExt06.Value.Value + presupuestoDet.ValorExt07.Value.Value + presupuestoDet.ValorExt08.Value.Value
                                                        + presupuestoDet.ValorExt09.Value.Value + presupuestoDet.ValorExt10.Value.Value + presupuestoDet.ValorExt11.Value.Value + presupuestoDet.ValorExt12.Value.Value);
                                if (TotalMdaExtr != 0)
                                    presupuestoDet.VlrSaldoAntExtr.Value = TotalMdaExtr;
                            }

                            presupuestoDet.VlrNuevoSaldoLoc.Value = presupuestoDet.VlrSaldoAntLoc.Value + presupuestoDet.VlrMvtoLocal.Value;
                            presupuestoDet.VlrNuevoSaldoExtr.Value = presupuestoDet.VlrSaldoAntExtr.Value + presupuestoDet.VlrMvtoExtr.Value; 
                            #endregion                                            
                            #region Valida consistencia datos 
                            this.ValidateDataImport(presupuestoDet, rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "Detalle NOK";
                                result.Result = ResultValue.NOK;
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region Actualiza la información de la grilla
                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                            this.Invoke(this.refreshDataDelegate);

                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                        this.detList = new List<DTO_plPresupuestoDeta>();
                    }
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, 100 });
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(_bc.GetResourceForException(e, "WinApp-DocumentAuxiliarForm.cs", "ImportThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
