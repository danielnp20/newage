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
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra basica
    /// </summary>
    public abstract partial class MasterSimpleForm : MasterForm
    {
        #region variables

        BaseController _bc = BaseController.GetInstance();
        protected IEnumerable<DTO_MasterBasic> dtoList;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MasterSimpleForm(string mod = null) : base(mod) { }

        #region Funciones Privadas

        /// <summary>
        /// Carga el dto correspondiente a la mestra selecionada
        /// </summary>
        /// <param name="id">Codigo</param>
        /// <param name="desc">Descripcion</param>
        /// <param name="act">Indicador de activo</param>
        /// <param name="version">Version del registro</param>
        /// <param name="replicaId">Valor actual de la version del registro</param>
        /// <returns>Retorna el DTO que corresponde a la maestra actual</returns>
        private DTO_MasterBasic LoadDTO(string id, string desc, bool act, short version, int replicaId, Dictionary<string, Object> extraValues, bool useReflection = false)
        {
            DTO_MasterBasic dto = new DTO_MasterBasic();
            try
            {
                dto = new DTO_MasterBasic(this.FrmProperties);
                dto.IdName = this.colIdName;

                try { dto.ID.Value = id.ToUpper(); }
                catch (MentorDataParametersException e1) { e1.FieldName = this.fields[this.editrow_code].FieldName; throw e1; }
                try { dto.Descriptivo.Value = desc; }
                catch (MentorDataParametersException e1) { e1.FieldName = this.fields[this.editrow_desc].FieldName; throw e1; }
                try { dto.ActivoInd.Value = act; }
                catch (MentorDataParametersException e1) { e1.FieldName = this.fields[this.editrow_act].FieldName; throw e1; }
                dto.CtrlVersion.Value = version;
                dto.ReplicaID.Value = replicaId;

                if (this.FrmProperties.GrupoEmpresaInd)
                {
                    try { dto.EmpresaGrupoID.Value = this.empresaGrupoID; }
                    catch (MentorDataParametersException e1)
                    {
                        e1.FieldName = this.fields[this.editrow_company].FieldName;
                        throw e1;
                    }
                }

                if (useReflection)
                {
                    #region carga los campos del DTO

                    object simpleDto = Activator.CreateInstance(this.frmType, new object[1] { dto });
                    bool isEmp = simpleDto.GetType() == typeof(DTO_glEmpresa);
                    bool isUsr = simpleDto.GetType() == typeof(DTO_seUsuario);
                    bool isTerc = simpleDto.GetType() == typeof(DTO_coTercero);
                    string key = string.Empty;

                    //Se recorren cada uno de los elementos de la grilla de edicion extras y se tratan de asignar
                    foreach (string extraValueKey in extraValues.Keys)
                    {
                        if (extraValueKey.Equals(this.FrmProperties.ColumnaID))
                            continue;
                        Object value = extraValues[extraValueKey];
                        key = extraValueKey;

                        if (isEmp && extraValueKey.Equals("EmpresaGrupoID"))
                            key = "EmpresaGrupoID_";

                        if ((isUsr || isTerc) && extraValueKey.Equals("Contrasena"))
                            continue;

                        PropertyInfo pi = this.frmType.GetProperty(key);
                        if (pi != null)
                        {
                            #region Carga las propiedades
                            UDT udt = (UDT)pi.GetValue(simpleDto, null);
                            try
                            {
                                if (!(udt is UDT_Imagen))
                                    udt.SetValueFromString(extraValues[extraValueKey].ToString());
                                else
                                {
                                    if (!string.IsNullOrEmpty(extraValues[extraValueKey].ToString()))
                                    {
                                        byte[] arr = Utility.ObjectToByteArray(extraValues[extraValueKey]);
                                        (udt as UDT_Imagen).Value = arr;
                                    }
                                }
                            }
                            catch (MentorDataParametersException mdpe)
                            {
                                mdpe.FieldName = key;
                                throw mdpe;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Carga los campos
                            FieldInfo fi = this.frmType.GetField(extraValueKey);
                            if (fi != null)
                            {
                                UDT udt = (UDT)fi.GetValue(simpleDto);
                                try
                                {
                                    if (!(udt is UDT_Imagen))
                                        udt.SetValueFromString(extraValues[extraValueKey].ToString());
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(extraValues[extraValueKey].ToString()))
                                        {
                                            byte[] arr = Utility.ObjectToByteArray(extraValues[extraValueKey]);
                                            (udt as UDT_Imagen).Value = arr;
                                        }
                                    }
                                }
                                catch (MentorDataParametersException mdpe)
                                {
                                    mdpe.FieldName = extraValueKey;
                                    throw mdpe;
                                }
                            }
                            else
                            {
                                Exception ex = new MentorDataException("", extraValueKey, Enums.MentorDataExceptionType.Reflection, "LoadDTO");
                                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterSimpleForm", "LoadDTO"));
                            }
                            #endregion
                        }
                    }
                    #endregion
                    return (DTO_MasterBasic)simpleDto;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dto;
        }

        #endregion

        #region Funciones Virtuales (Implementacion MasterForm)

        #region Funciones de datos

        /// <summary>
        /// Metodo encargado de encapsular la insercion de registros de maestras simples
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="insertList">Lista de dtos</param>
        /// <param name="userId">usuario logueado</param>
        /// <param name="documentId">docuemnto</param>
        /// <param name="accion">accion (insercion/importar)</param>
        /// <returns></returns>
        protected virtual DTO_TxResult DataAdd(List<DTO_MasterBasic> insertList, int accion)
        {
            byte[] bItems = CompressedSerializer.Compress<IEnumerable<DTO_MasterBasic>>(insertList);
            return _bc.AdministrationModel.MasterSimple_Add(this.DocumentID, bItems, accion);
        }

        /// <summary>
        /// Metodo que encapsula la funcion de actualizar
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="dto">dto</param>
        /// <param name="userId">usuario</param>
        /// <param name="documentId">documento</param>
        /// <returns></returns>
        protected virtual DTO_TxResult DataUpdate(DTO_MasterBasic dto)
        {
            return _bc.AdministrationModel.MasterSimple_Update(this.DocumentID, dto);
        }

        #endregion

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.SetInitParameters(false);
        }

        /// <summary>
        /// Cuenta los elementos dado un filtro
        /// </summary>
        /// <returns></returns>
        protected override long CountElements(bool useFastFilter = true)
        {
            List<DTO_glConsultaFiltro> filtrosRapidos = useFastFilter ? this.filtrosConsulta : new List<DTO_glConsultaFiltro>();
            var result = _bc.AdministrationModel.MasterSimple_Count(this.DocumentID, this.consulta, filtrosRapidos, null);
            return result;
        }

        /// <summary>
        /// Trae los datos
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Número de página</param>
        /// <returns></returns>
        protected override IEnumerable GetPagedData(int pageNum)
        {
            //<DTO_glConsultaFiltro> filtrosRapidos = useFastFilter ? this.FiltrosConsulta : new List<DTO_glConsultaFiltro>();
            this.dtoList = _bc.AdministrationModel.MasterSimple_GetPaged(this.DocumentID, this.pageSize, pageNum, this.consulta, this.filtrosConsulta, null);
            return this.dtoList;
        }

        /// <summary>
        /// Actualiza los campos de la grilla de edición
        /// </summary>
        /// <param name="isNew">Indica si se va agregar un nuevo registro</param>
        /// <param name="rowIndex">Indice de la fila</param>
        protected override void LoadEditGridData(bool isNew, int rowIndex)
        {
            List<GridProperty> fillGridEdit = new List<GridProperty>();
            DTO_MasterBasic dtoData = null;
            try
            {
                dtoData = (DTO_MasterBasic)gvModule.GetRow(rowIndex);
                dtoData = (DTO_MasterBasic)this.FillEditData(dtoData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterSimpleForm.cs", "LoadEditGridData"));
            }

            //Obtiene los datos de la segunda columna para la grilla de edición
            this.idSelected = (isNew || dtoData == null) ? string.Empty : dtoData.ID.Value;
            this.basicUDT.Value = this.idSelected;
            string valueDes = (isNew || dtoData == null) ? string.Empty : dtoData.Descriptivo.Value;
            string valueAct = (isNew || dtoData == null) ? true.ToString() : dtoData.ActivoInd.Value.ToString();
            string valueVers = (isNew || dtoData == null) ? "0" : dtoData.CtrlVersion.Value.ToString();
            string valueReplicaID = (isNew || dtoData == null) ? "-1" : dtoData.ReplicaID.Value.ToString();

            //Llena la lista que envia los datos a la grilla de edición
            fillGridEdit.AddRange(new GridProperty[] 
            {
                new GridProperty(this.fields[this.editrow_code].Caption, idSelected.Trim()),
                new GridProperty(this.fields[this.editrow_desc].Caption, valueDes.Trim())      
            });

            List<GridProperty> extra = this.GetExtraGridProperties(this.frmType, dtoData);

            fillGridEdit.AddRange(extra);
            fillGridEdit.AddRange(new GridProperty[] 
            {
                new GridProperty(this.fields[this.editrow_act].Caption, valueAct),     
                new GridProperty(this.fields[this.editrow_vers].Caption, valueVers, false),
                new GridProperty(this.fields[this.editrow_replica].Caption, valueReplicaID, false)           
            });

            this.ConfigureEditGrids(fillGridEdit);
        }

        /// <summary>
        /// Valida los campos de la grilla de edicion y  llena el dto
        /// Si ocurren problemas de validación devuelve el dto en null
        /// </summary>
        /// <returns>El DTO lleno si la validación pasó y el DTO nulo si no pasó</returns>
        protected override object ValidateEditGrid()
        {
            #region Traer strings

            //this.gvRecordEdit.ClearColumnsFilter();
            //Cambie el foco del control para tomar la ultima version de la grilla
            this.gvModule.Focus();

            string valueid = this.GetEditRow(this.editrow_code).Valor.ToString().Trim();
            string valueDes = this.GetEditRow(this.editrow_desc).Valor.ToString().Trim();
            string valueAct = this.GetEditRow(this.editrow_act).Valor.ToString().Trim();
            string valueVers = this.GetEditRow(this.editrow_vers).Valor.ToString().Trim();
            string valueReplicaId = this.GetEditRow(this.editrow_replica).Valor.ToString().Trim();
            Dictionary<string, Object> extraValues = new Dictionary<string, Object>();

            #endregion
            #region Validaciones de no Null y formato

            string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string msgInvalidField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField);
            string msgInvalidFieldLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFieldLength);
            string msgFieldNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FieldNotFound_Col);
            string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather);

            List<GridProperty> aux = (List<GridProperty>)this.grlControlRecordEdit.DataSource;
            for (int i = 0; i < aux.Count; i++)
            {
                if (i != this.editrow_code && i != this.editrow_desc && i != this.editrow_act && i != this.editrow_vers && i != this.editrow_replica)
                {
                    GridProperty gp = this.GetEditRow(i);
                    FieldConfiguration config = this.GetFieldConfigByFieldName(this.fields[i].FieldName);
                    DTO_aplMaestraCampo campo = this.GetFieldByFieldName(this.fields[i].FieldName);

                    string value = gp.Valor.ToString();
                    if (config.FieldName == "PeriodoID")
                    {
                        string[] fSplit = value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        if (fSplit.Count() == 2)
                        {
                            int year = Convert.ToInt32(fSplit[0]);
                            int month = Convert.ToInt32(fSplit[1]);
                            DateTime pDate = new DateTime(year, month, 1);
                            value = pDate.ToString(FormatString.Date);
                        }
                    }

                    //Valida nulls
                    if (String.IsNullOrWhiteSpace(value) && !config.AllowNull)
                    {
                        MessageBox.Show(string.Format(msgEmptyField, this.fields[i].Caption));
                        return null;
                    }

                    //Valida FKS existentes
                    if (campo.FKInd)
                    {
                        object fkDTO = _bc.GetMasterDTO(AppMasters.MasterType.Simple, campo.FKDocumentoID, false, value, true);
                        DTO_MasterBasic basic = null;

                        if (fkDTO is DTO_MasterHierarchyBasic)
                        {
                            if ((fkDTO as DTO_MasterHierarchyBasic).MovInd.Value == false)
                            {
                                MessageBox.Show(string.Format(msgEmptyField, this.fields[i + 1].Caption));
                                return null;
                            }
                        }
                        basic = fkDTO != null ? (DTO_MasterBasic)fkDTO : null;
                        if (fkDTO == null || basic.ID == null || string.IsNullOrEmpty(basic.ID.Value))
                        {
                            //Si la FK tiene datos y acepta nulls
                            if (!String.IsNullOrWhiteSpace(value) && config.AllowNull)
                            {
                                MessageBox.Show(string.Format(msgFieldNotFound, this.fields[i].Caption));
                                return null;
                            }
                        }
                    }

                    if (gp.Valor.GetType() == typeof(System.Drawing.Bitmap))
                        extraValues.Add(this.fields[i].FieldName, gp.Valor);
                    else
                        extraValues.Add(this.fields[i].FieldName, value);
                }
            }

            if (string.IsNullOrEmpty(valueid))
            {
                MessageBox.Show(string.Format(msgEmptyField, this.fields[editrow_code].Caption));
                return null;
            }
            if (string.IsNullOrEmpty(valueDes))
            {
                MessageBox.Show(string.Format(msgEmptyField, this.fields[editrow_desc].Caption));
                return null;
            }
            if (string.IsNullOrEmpty(valueAct))
            {
                MessageBox.Show(string.Format(msgEmptyField, this.fields[editrow_act].Caption));
                return null;
            }
            if (!valueAct.Equals(true.ToString()) && !valueAct.Equals(false.ToString()))
            {
                MessageBox.Show(string.Format(msgInvalidField, this.fields[editrow_act].Caption));
                return null;
            }
            #endregion
            #region Llenar DTO

            short ver = this.Insertando ? (short)1 : Convert.ToInt16(valueVers);
            DTO_MasterBasic dto = this.LoadDTO(valueid, valueDes, Convert.ToBoolean(valueAct), ver, Convert.ToInt32(valueReplicaId), extraValues, true);
            return dto;

            #endregion
        }

        /// <summary>
        /// Función que trae los registros para el reporte
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable GetReportData()
        {
            long count = _bc.AdministrationModel.MasterSimple_Count(this.DocumentID, this.consulta, null, true);
            IEnumerable data = _bc.AdministrationModel.MasterSimple_GetPaged(this.DocumentID, count, 1, this.consulta, null, true);

            return data;
        }

        #endregion

        #region Implementación eventos Master Form

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvModule_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                DTO_MasterBasic dto = (DTO_MasterBasic)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);//Le quita los caracteres de unbound para diferenciarlas de las columnas normales

                if (fieldName == "EmpresaGrupoID")
                {
                    if (e.Row.GetType() == typeof(NewAge.DTO.Negocio.DTO_glEmpresa))
                    {
                        DTO_glEmpresa emp = (DTO_glEmpresa)e.Row;
                        e.Value = emp.EmpresaGrupoID_.Value;
                        return;
                    }
                    if (e.Row.GetType() == typeof(NewAge.DTO.Negocio.DTO_glEmpresaGrupo))
                    {
                        DTO_glEmpresaGrupo emp = (DTO_glEmpresaGrupo)e.Row;
                        e.Value = emp.ID.Value;
                        return;
                    }
                }

                if (fieldName == "Contrasena")
                {
                    e.Value = string.Empty;
                    return;
                }

                try
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }

                    if (fieldName == this.fields[this.editrow_code].FieldName)
                        e.Value = dto.ID.Value;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterSimpleForm.cs", "gvModule_CustomUnboundColumnData"));
                }
            }
        }

        #endregion

        #region Eventos barra de herramientas

        /// <summary>
        /// Sobrecarga de la opcion guardar de la barra de herramientas
        /// </summary>
        public override void TBSave()
        {
            this.gvRecordEdit.PostEditor();
            foreach (GridControl gc in this.extraGrids.Values)
            {
                gc.MainView.PostEditor();
            }
            bool insertandoTemp = this.Insertando;
            try
            {
                DTO_MasterBasic dto = (DTO_MasterBasic)this.ValidateEditGrid();
                if (dto == null)
                    return;

                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                bool res = true;
                string errMsg = string.Empty;
                SortedDictionary<string, string> validDTO = this.ValidateDTORules(dto, out res, out errMsg);
                foreach (var err in validDTO)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + err.Key);
                    rdF.Message = _bc.GetResource(LanguageTypes.Messages, err.Value);
                    rd.DetailsFields.Add(rdF);
                }

                if (validDTO.Count == 0)
                {
                    if (this.Insertando)
                    {
                        List<DTO_MasterBasic> insertList = new List<DTO_MasterBasic>();
                        insertList.Add(dto);
                        result = this.DataAdd(insertList, Convert.ToInt32(FormsActions.Add));
                    }
                    else
                        result = this.DataUpdate(dto);
                }
                else
                {
                    rd.Message = "NOK";
                    result.Result = ResultValue.NOK;
                    result.Details.Add(rd);
                }

                MessageForm frm = new MessageForm(result);
                frm.ShowDialog();

                if (result.Result.Equals(ResultValue.OK))
                {
                    this.pnSearch.Visible = false;
                    this.txtCode.Text = string.Empty;
                    this.txtDescrip.Text = string.Empty;
                    this.filtrosConsulta = null;
                    this.consulta = null;

                    long rowNumber = _bc.AdministrationModel.MasterSimple_Rownumber(this.DocumentID, dto.ID, this.consulta, null);
                    int numPage = (int)(rowNumber / this.pgGrid.PageSize) + 1;
                    this.pgGrid.PageNumber = numPage;

                    if (insertandoTemp)
                        this.saveNew = true;
                    else
                        this.saveNew = false;

                    this.LoadGridData(false, false, false);

                    if (insertandoTemp)
                        this.saveNew = true;
                    else
                        this.saveNew = false;

                    this.gvModule.MoveBy((int)(rowNumber % pgGrid.PageSize) - 1);

                    this.Insertando = insertandoTemp;
                }
                else
                    return;

                if (this.Insertando)
                    this.TBNew();
                else
                    this.Insertando = false;

                base.TBSave();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterSimpleForm.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para eliminar un o un conjunto de registros
        /// </summary>
        public override void TBDelete()
        {
            string msgTitleDelete = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete));
            string msgDeleteCode = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Code);
            string msgDeleteInvalidOp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_InvalidOP);
            msgDeleteCode = string.Format(msgDeleteCode, this.idSelected.Trim());

            if (this.Insertando)
            {
                MessageBox.Show(msgDeleteInvalidOp);
                return;
            }
            try
            {
                if (MessageBox.Show(msgDeleteCode, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (this.gvModule.DataRowCount <= 1 || this.pgGrid.PageCount == 1)
                        this.pgGrid.PageNumber = 1;

                    DTO_TxResult result = _bc.AdministrationModel.MasterSimple_Delete(this.DocumentID, this.basicUDT);

                    MessageForm frm = new MessageForm(result);
                    this.LoadGridData(false, false, false);

                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterSimpleForm.cs", "AddGridCols"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Importación
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    bool sendToServer = true;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    List<DTO_MasterBasic> list = new List<DTO_MasterBasic>();
                    Dictionary<int, List<Tuple<string, bool>>> fks = new Dictionary<int, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    //Posición de las columnas
                    int codigo_pos = 0;
                    int desc_pos = 0;
                    int activo_pos = 0;
                    Dictionary<string, int> colPos = new Dictionary<string, int>();
                    //Valores de las columnas al recorrer los textos
                    string codigo = string.Empty;
                    string descripcion = string.Empty;
                    bool activo = false;
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgImportInvalidLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_InvalidLength);
                    string msg_FkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather);
                    #endregion
                    #region Llena las listas de columnas de acuerdo a los parametros extras
                    foreach (var mf in this.FrmProperties.Campos)
                    {
                        string colName = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                        if (mf.FKInd)
                        {
                            fkNames.Add(colName);
                            try
                            {
                                fks.Add(mf.FKDocumentoID, new List<Tuple<string, bool>>());
                            }
                            catch (Exception ex)
                            { ; }
                        }
                        colPos.Add(colName.ToLower(), -1);
                        colVals.Add(mf.NombreColumna, string.Empty);
                    }
                    #endregion
                    #region Llena información para enviar al servidor
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.DocumentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.DocumentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.DocumentID))
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
                        }
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        #region Llena la info de los titulos de las columnas y sus posiciones
                        //Llena la info de las columnas y verifica la posición del dato
                        if (i == 0)
                        {
                            for (int j = 0; j < line.Length; ++j)
                            {
                                if (line[j].Equals(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.FrmProperties.ColumnaID), StringComparison.InvariantCultureIgnoreCase))
                                    codigo_pos = j;
                                else if (line[j].Equals(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_Descriptivo"), StringComparison.InvariantCultureIgnoreCase))
                                    desc_pos = j;
                                else if (line[j].Equals(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd"), StringComparison.InvariantCultureIgnoreCase))
                                    activo_pos = j;
                                else
                                {
                                    int cPos = 0;
                                    string cName = _bc.GetResource(LanguageTypes.Forms, line[j].Trim().ToLower());
                                    bool colFound = colPos.TryGetValue(cName, out cPos);
                                    if (colFound)
                                    {
                                        colPos[cName] = j;
                                    }
                                }
                            }
                        }
                        #endregion
                        //Recorre todas las filas y verifica que tengan datos y el codigo sea valido
                        if (i > 0 && line.Length > 0)
                        {
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i + 1;
                            rd.Key = codigo;
                            rd.Message = "OK";

                            #region Info básica
                            bool createDTO = true;
                            int max = 0;
                            max = Math.Max(max, codigo_pos);
                            max = Math.Max(max, desc_pos);
                            max = Math.Max(max, activo_pos);
                            foreach (int v in colPos.Values)
                                max = Math.Max(max, v);

                            //Llena los valores de las columnas
                            if (line.Length != (max + 1))
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                continue;
                            }
                            else
                            {
                                codigo = line[codigo_pos].Trim();
                                descripcion = this.DocumentID != (int)AppMasters.coTercero ? line[desc_pos].Trim() : string.Empty;
                                foreach (DTO_aplMaestraCampo mf in this.FrmProperties.Campos)
                                {
                                    string colName = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                    int pos = colPos[colName.ToLower()];
                                    if (pos != -1)
                                    {
                                        colVals[mf.NombreColumna] = line[pos];
                                    }
                                    if (mf.FKInd && !string.IsNullOrWhiteSpace(line[pos]))
                                    {
                                        colVals[mf.NombreColumna] = line[pos].ToUpper();

                                        Tuple<string, bool> tupValid = new Tuple<string, bool>(line[pos], true);
                                        Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[pos], false);

                                        if (fks[mf.FKDocumentoID].Contains(tupValid))
                                        {
                                            continue;
                                        }
                                        else if (fks[mf.FKDocumentoID].Contains(tupInvalid))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                            rdF.Message = string.Format(msg_FkNotFound, line[pos]);
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        else
                                        {
                                            bool isInt = mf.FKDocumentoID == AppMasters.glDocumento ? true : false;

                                            object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, mf.FKDocumentoID, isInt, line[pos], true);
                                            bool hierarchyFather = false;
                                            if (dto is DTO_MasterHierarchyBasic)
                                            {
                                                if ((dto as DTO_MasterHierarchyBasic).MovInd.Value == false)
                                                {
                                                    hierarchyFather = true;
                                                }
                                            }
                                            if (dto != null && !hierarchyFather)
                                            {
                                                fks[mf.FKDocumentoID].Add(new Tuple<string, bool>(line[pos], true));
                                            }
                                            else
                                            {
                                                fks[mf.FKDocumentoID].Add(new Tuple<string, bool>(line[pos], false));

                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                                if (hierarchyFather)
                                                    rdF.Message = string.Format(msgFkHierarchyFather, line[pos]);
                                                else
                                                    rdF.Message = string.Format(msg_FkNotFound, line[pos]);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion
                            #region Validaciones de no Null
                            if (string.IsNullOrEmpty(codigo))
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.colIdName);
                                rdF.Message = msgEmptyField;
                                rd.DetailsFields.Add(rdF);

                                createDTO = false;
                            }
                            if (!this.ValidateCode(codigo))
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.colIdName);
                                rdF.Message = msgInvalidFormat;
                                rd.DetailsFields.Add(rdF);

                                createDTO = false;
                            }
                            if (this.DocumentID != (int)AppMasters.coTercero && string.IsNullOrEmpty(descripcion))
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.fields[this.editrow_desc].FieldName);
                                rdF.Message = msgEmptyField;
                                rd.DetailsFields.Add(rdF);

                                createDTO = false;
                            }
                            foreach (DTO_aplMaestraCampo mf in this.FrmProperties.Campos)
                            {
                                Type t = Type.GetType("NewAge.DTO.UDT." + mf.Tipo + ", NewAge.DTO");
                                PropertyInfo pi = t.GetProperty("Value");
                                bool isBool = pi.PropertyType.Equals(typeof(bool)) || pi.PropertyType.Equals(typeof(Nullable<bool>));

                                if (!mf.VacioInd && mf.ImportacionInd && !isBool)
                                {
                                    string colKey = this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna;
                                    string rsxKey = _bc.GetResource(LanguageTypes.Forms, colKey).ToLower();
                                    int pos = colPos[rsxKey];

                                    if (string.IsNullOrEmpty(line[pos]))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }

                            #endregion
                            #region Validacion de formatos

                            activo = false;
                            if (line[activo_pos].Trim() != string.Empty)
                            {
                                activo = true;

                                if (line[activo_pos].Trim().ToLower() != "x")
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.fields[this.editrow_act].FieldName);
                                    rdF.Message = msgInvalidFormat + " (x)";
                                    rd.DetailsFields.Add(rdF);

                                    createDTO = false;
                                }
                            }

                            foreach (DTO_aplMaestraCampo mf in this.FrmProperties.Campos)
                            {
                                if (mf.ImportacionInd)
                                {
                                    int pos = colPos[_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna).ToLower()];
                                    string colName = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);

                                    Type t = Type.GetType("NewAge.DTO.UDT." + mf.Tipo + ", NewAge.DTO");
                                    PropertyInfo pi = t.GetProperty("Value");

                                    //Comprueba los valores solo para los booleanos
                                    if (pi.PropertyType.Equals(typeof(bool)) || pi.PropertyType.Equals(typeof(Nullable<bool>)))
                                    {
                                        string colVal = "false";
                                        if (line[pos].Trim() != string.Empty)
                                        {
                                            colVal = "true";
                                            if (line[pos].Trim().ToLower() != "x")
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                                rdF.Message = msgInvalidFormat + " (x)";
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                        colVals[mf.NombreColumna] = colVal;
                                    }

                                    //Valida formatos para las otras columnas
                                    if (line[pos].Trim() != string.Empty)
                                    {
                                        if (pi.PropertyType.Equals(typeof(DateTime)) || pi.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                        {
                                            try
                                            {
                                                DateTime val = DateTime.ParseExact(line[pos].Trim(), FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                            }
                                            catch (Exception ex)
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                                rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                        if (pi.PropertyType.Equals(typeof(int)) || pi.PropertyType.Equals(typeof(Nullable<int>)))
                                        {
                                            try
                                            {
                                                int val = Convert.ToInt32(line[pos].Trim());
                                            }
                                            catch (Exception ex)
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                                rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                        if (pi.PropertyType.Equals(typeof(long)) || pi.PropertyType.Equals(typeof(Nullable<long>)))
                                        {
                                            try
                                            {
                                                long val = Convert.ToInt64(line[pos].Trim());
                                            }
                                            catch (Exception ex)
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                                rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                        if (pi.PropertyType.Equals(typeof(short)) || pi.PropertyType.Equals(typeof(Nullable<short>)))
                                        {
                                            try
                                            {
                                                short val = Convert.ToInt16(line[pos].Trim());
                                            }
                                            catch (Exception ex)
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                                rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                        if (pi.PropertyType.Equals(typeof(byte)) || pi.PropertyType.Equals(typeof(Nullable<byte>)))
                                        {
                                            try
                                            {
                                                byte val = Convert.ToByte(line[pos].Trim());
                                            }
                                            catch (Exception ex)
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                                rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                        if (pi.PropertyType.Equals(typeof(decimal)) || pi.PropertyType.Equals(typeof(Nullable<decimal>)))
                                        {
                                            try
                                            {
                                                decimal val = Convert.ToDecimal(line[pos].Trim(), CultureInfo.InvariantCulture);
                                                if (line[pos].Trim().Contains(','))
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mf.NombreColumna);
                                                rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                    } //validacion si no es null
                                }
                            }
                            #endregion
                            #region Crea los DTO
                            if (createDTO)
                            {
                                try
                                {
                                    DTO_MasterBasic dto = this.LoadDTO(codigo, descripcion, activo, 1, 1, colVals, true);

                                    //Valida las reglas [articulares del DTO
                                    bool res = true;
                                    string errMsg = string.Empty;
                                    SortedDictionary<string, string> validDTO = this.ValidateDTORules(dto, out res, out errMsg);
                                    if (!res)
                                    {
                                        sendToServer = false;
                                        foreach (var err in validDTO)
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + err.Key);
                                            rdF.Message = _bc.GetResource(LanguageTypes.Messages, err.Value);
                                            rd.DetailsFields.Add(rdF);
                                        }
                                    }

                                    if (sendToServer)
                                        list.Add(dto);
                                }
                                catch (MentorDataParametersException mdpe)
                                {
                                    if (mdpe.GetType() == typeof(MentorDataParametersException))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + mdpe.FieldName);
                                        rdF.Message = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatMaxLength), mdpe.MaxLenght);
                                        rd.DetailsFields.Add(rdF);
                                        sendToServer = false;
                                    }
                                    else
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Message = mdpe.Message;
                                        rd.DetailsFields.Add(rdF);
                                        sendToServer = false;
                                    }
                                }
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
                    #region Revisa validaciones particulares de una maestra para importar (Sin implementacion aun)

                    if (sendToServer && result.Result == ResultValue.OK)
                    {
                        DTO_TxResult res = this.ValidateDataOnImport(list);
                    }

                    #endregion
                    #region Envia la info al servidor o muestra los errores locales
                    if (sendToServer && result.Result == ResultValue.OK)
                    {
                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.DocumentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                        FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoMaster(this.DocumentID));

                        ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                        FormProvider.Master.ProgressBarThread = new Thread(pth);
                        FormProvider.Master.ProgressBarThread.Start(this.DocumentID);

                        //Envia los DTOs al servidor
                        result = this.DataAdd(list, Convert.ToInt32(FormsActions.Import));
                        FormProvider.Master.StopProgressBarThread(this.DocumentID);

                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                            this.Invoke(this.refreshGrid);

                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterSimpleForm.cs", "ImportThread"));
            }
            finally
            {
                base.ImportThread();
                this.importando = false;
                FormProvider.Master.StopProgressBarThread(this.DocumentID);
            }
        }

        #endregion

    }//clase
}//namespace
