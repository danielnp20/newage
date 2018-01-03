using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Librerias.ExceptionHandler;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors.Mask;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de Maestras
    /// </summary>
    public abstract partial class MasterComplexForm : MasterForm
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        protected IEnumerable<DTO_MasterComplex> dtoList;
        protected DTO_MasterComplex complexDto = null;
        #endregion

        #region Propiedades

        /// <summary>
        /// Encapsula el estado de insertando en el formulario 
        /// </summary>
        protected override bool Insertando
        {
            get
            {
                return base.Insertando;
            }
            set
            {
                base.Insertando = value;
                if (value){
                    this.complexDto = null;
                }
            }
        }
        
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public MasterComplexForm(string mod = null) : base(mod) { }

        #region Funciones privadas

        /// <summary>
        /// Carga el dto correspondiente a la mestra selecionada
        /// </summary>
        /// <param name="act">Indicador de activo</param>
        /// <param name="version">Version del registro</param>
        /// <param name="replicaId">Valor actual de la version del registro</param>
        /// <returns>Retorna el DTO que corresponde a la maestra actual</returns>
        private DTO_MasterComplex LoadDTO(bool act, short version, int replicaId, Dictionary<string, Object> extraValues)
        {
            DTO_MasterComplex dto = new DTO_MasterComplex(this.FrmProperties);
            try
            {
                try { dto.ActivoInd.Value = act; }
                catch (MentorDataParametersException e1) { e1.FieldName = this.fields[this.editrow_act].FieldName; throw e1; }
                dto.CtrlVersion.Value = version;
                dto.ReplicaID.Value = replicaId;

                if (this.FrmProperties.GrupoEmpresaInd)
                {
                    try 
                    { 
                        dto.EmpresaGrupoID.Value = this.empresaGrupoID; 
                    }
                    catch (MentorDataParametersException e1) 
                    { 
                        e1.FieldName = this.fields[this.editrow_company].FieldName; 
                        throw e1; 
                    }
                }

                //Se utiliza el constructor del objeto del formulario, basado en 
                object simpleDto = Activator.CreateInstance(this.frmType, new object[1] { dto });

                #region carga los campos del DTO
                foreach (string extraValueKey in extraValues.Keys)
                {
                    Object value = extraValues[extraValueKey];

                    if (dto.PKValues.ContainsKey(extraValueKey))
                         dto.PKValues[extraValueKey] = value.ToString();

                    PropertyInfo pi = this.frmType.GetProperty(extraValueKey);
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
                            mdpe.FieldName = extraValueKey;
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
                            MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterComplexForm", "LoadDTO"));
                        }
                        #endregion
                    }
                }
                #endregion
                return (DTO_MasterComplex)simpleDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dto;
        }

        /// <summary>
        /// Carga los valores delas llaves primarias para un DTO
        /// </summary>
        /// <returns>Retorna elDTO completo</returns>
        private DTO_MasterComplex FillPksValues(DTO_MasterComplex dto)
        {
            Dictionary<string, string> keys = new Dictionary<string, string>();
            foreach (string pk in dto.PKValues.Keys)
            {
                string val = Utility.GetPropertyValueToString(dto, pk);
                keys.Add(pk, val);
            }

            dto.PKValues = keys;
            return dto;
        }

        /// <summary>
        /// Get a list of pks
        /// </summary>
        /// <param name="dto">Dto to get the pks</param>
        /// <returns>Returns the list of pks with their valuesS</returns>
        //private List<UDT_BasicID> GetDtoPks(DTO_MasterComplex dto)
        //{
        //    List<UDT_BasicID> pks = new List<UDT_BasicID>();
        //    foreach (var pk in dto.PKValues)
        //    {
        //        UDT_BasicID udt = new UDT_BasicID();
        //        if(pk.Key=="DocumentoID")
        //        {
        //            udt = new UDT_BasicID(true);
        //        }
        //        udt.Value = pk.Value;
        //        pks.Add(udt);
        //    }

        //    return pks;
        //}

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
        protected virtual DTO_TxResult DataAdd(List<DTO_MasterComplex> insertList, int accion)
        {
            byte[] bItems = CompressedSerializer.Compress<IEnumerable<DTO_MasterComplex>>(insertList);
            return _bc.AdministrationModel.MasterComplex_Add(this.DocumentID, bItems, accion);
        }

        /// <summary>
        /// Metodo que encapsula la funcion de actualizar
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="dto">dto</param>
        /// <param name="userId">usuario</param>
        /// <param name="documentId">documento</param>
        /// <returns></returns>
        protected virtual DTO_TxResult DataUpdate(DTO_MasterComplex dto)
        {
            return _bc.AdministrationModel.MasterComplex_Update(this.DocumentID, dto);
        }

        #endregion

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// <param name="isH">Defini si es jerarquica</param>
        /// </summary>
        protected virtual void SetInitParameters()
        {
            this.FrmProperties = _bc.AdministrationModel.MasterProperties[this.DocumentID];

            //Propiedades de la maestra
            this.frmModule = this.FrmProperties.ModuloID;
            this.frmType = Type.GetType("NewAge.DTO.Negocio." + this.FrmProperties.DTOTipo + ", NewAge.DTO");
            this.sortField = this.FrmProperties.ColumnaID;

            #region Formato de exportar/importar
            List<string> cols = new List<string>();
            foreach (var ef in this.FrmProperties.Campos)
            {
                if (ef.ImportacionInd)
                {
                    cols.Add(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + ef.NombreColumna));
                }
            }
            cols.Add(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd"));
            this.format = TableFormat.FillMasterSimple(cols);
            #endregion

            //Inicia las variables del formulario
            this.empresaGrupoID = _bc.GetMaestraEmpresaGrupoByDocumentID(this.DocumentID);
            this.CreateFieldsConfig(false);
        }

        /// <summary>
        /// Se ejecuta luego del initializeComponents
        /// </summary>
        protected override void AfterInitialize()
        {
            this.gvModule.OptionsFind.FindFilterColumns = "*";
        }

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.SetInitParameters();
        }

        /// <summary>
        /// Cuenta los elementos dado un filtro
        /// </summary>
        /// <returns></returns>
        protected override long CountElements(bool useFastFilter = true)
        {
            var result = _bc.AdministrationModel.MasterComplex_Count(this.DocumentID, this.consulta, null);
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
            this.dtoList = _bc.AdministrationModel.MasterComplex_GetPaged(this.DocumentID, this.pageSize, pageNum, this.consulta, null);
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
            DTO_MasterComplex dtoData = null;
            try
            {
                dtoData = (DTO_MasterComplex)gvModule.GetRow(rowIndex);
                dtoData = (DTO_MasterComplex)FillEditData(dtoData);
            }
            catch (Exception e)
            {
                ;
            }

            //Obtiene los datos de la segunda columna para la grilla de edición
            string valueAct = (isNew || dtoData == null) ? true.ToString() : dtoData.ActivoInd.Value.ToString();
            string valueVers = (isNew || dtoData == null) ? "0" : dtoData.CtrlVersion.Value.ToString();
            string valueReplicaID = (isNew || dtoData == null) ? "-1" : dtoData.ReplicaID.Value.ToString();
            //Llena la lista que envia los datos a la grilla de edición
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
        /// Valida los campos de la grilla de edicion y llena el dto
        /// SI ocurren problemas de validación devuelve el dto en null
        /// </summary>
        /// <returns>El DTO de Grupo lleno si la validación pasó y el DTO nulo si no pasó</returns>
        protected override object ValidateEditGrid()
        {
            #region Traer strings

            //this.gvRecordEdit.ClearColumnsFilter();

            //Cambie el foco del control para tomar la ultima version de la grilla
            this.gvModule.Focus();

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
                if (i != this.editrow_act && i != this.editrow_vers && i != this.editrow_replica)
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
                                MessageBox.Show(string.Format(msgEmptyField ,this.fields[i + 1].Caption));
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
            DTO_MasterComplex dto = this.LoadDTO(Convert.ToBoolean(valueAct), ver, Convert.ToInt32(valueReplicaId), extraValues);
            return dto;

            #endregion
        }

        /// <summary>
        /// Función que trae los registros para el reporte
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable GetReportData()
        {
            long count = this._bc.AdministrationModel.MasterComplex_Count(this.DocumentID, this.consulta, null);
            IEnumerable data = this._bc.AdministrationModel.MasterComplex_GetPaged(this.DocumentID, count, 1, this.consulta, null);

            return data;
        }

        /// <summary>
        /// Crea las configuraciones de los campos
        /// </summary>
        protected override void CreateFieldsConfig(bool isHierarchy)
        {
            fields = new Dictionary<int, FieldConfiguration>();
            int extraF = 0;
            this.FrmProperties.Campos.ForEach(f =>
            {
                this.AddFormField(ref extraF, f);
                extraF = extraF + 1;
            });

            this.editrow_act = extraF;
            this.editrow_vers = extraF + 1;
            this.editrow_replica = extraF + 2;
            this.editrow_company = extraF + 3;

            CheckFieldConfiguration activoConfig = new CheckFieldConfiguration();
            activoConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd");
            activoConfig.FieldName = "ActivoInd";
            activoConfig.ColumnWidth = 320;
            activoConfig.ColumnIndex = this.editrow_act;
            activoConfig.Tab = basicTab;
            this.fields.Add(this.editrow_act, activoConfig);

            TextFieldConfiguration versionConfig = new TextFieldConfiguration(typeof(int), 10, CharacterCasing.Normal, TextFieldType.Numbers, false);
            versionConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_CtrlVersion");
            versionConfig.FieldName = "CtrlVersion";
            versionConfig.GridVisible = false;
            versionConfig.EditVisible = false;
            versionConfig.ColumnWidth = 0;
            versionConfig.ColumnIndex = this.editrow_vers;
            versionConfig.Tab = basicTab;
            this.fields.Add(this.editrow_vers, versionConfig);

            TextFieldConfiguration replicaConfig = new TextFieldConfiguration(typeof(int), 10, CharacterCasing.Normal, TextFieldType.Numbers, false);
            replicaConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ReplicaID");
            replicaConfig.FieldName = "ReplicaID";
            replicaConfig.GridVisible = false;
            replicaConfig.EditVisible = false;
            replicaConfig.ColumnWidth = 0;
            replicaConfig.ColumnIndex = this.editrow_replica;
            replicaConfig.Tab = basicTab;
            this.fields.Add(this.editrow_replica, replicaConfig);

            //Verifica la columna de la empresa
            if (this.FrmProperties.GrupoEmpresaInd)
            {
                TextFieldConfiguration companyConfig = new TextFieldConfiguration(typeof(int), 10, CharacterCasing.Normal, TextFieldType.Numbers, false);
                companyConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_EmpresaID");
                companyConfig.FieldName = "EmpresaGrupoID";
                companyConfig.GridVisible = false;
                companyConfig.EditVisible = false;
                companyConfig.ColumnWidth = 0;
                companyConfig.ColumnIndex = this.editrow_company;
                companyConfig.Tab = basicTab;
                this.fields.Add(this.editrow_company, companyConfig);
            }
            this.RefreshRowIndexFields();
            this.CustomizeFieldsConfig();
        }

        #endregion             

        #region Funciones MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            if (FormProvider.Master.LoadFormTB)
                FormProvider.Master.itemSearch.Enabled = false;
        }

        #endregion

        #region Implementación eventos Master Form

        ///// <summary>
        ///// Evento que se presenta al seleccionar una fila de la grilla
        ///// </summary>
        ///// <param name="sender">Objeto que envia el evento</param>
        ///// <param name="e">Evento</param>
        //protected override void gvModule_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        //{
        //    this.CreateFieldsConfig(false);
        //    this.Insertando = false;
        //    this.complexDto = (DTO_MasterComplex)this.gvModule.GetRow(e.FocusedRowHandle);
        //    this.complexDto = this.FillPksValues(this.complexDto);
        //    this.LoadEditGridData(false, e.FocusedRowHandle);
        //}
        protected override void RowSelected(int row)
        {
            try
            {
                this.CreateFieldsConfig(false);
                this.Insertando = false;
                this.complexDto = (DTO_MasterComplex)this.gvModule.GetRow(row);
                this.complexDto = this.complexDto != null ? this.FillPksValues(this.complexDto) : null;
                this.LoadEditGridData(false, row);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterComplexForm.cs", "RowSelected"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvModule_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                DTO_MasterComplex dto = (DTO_MasterComplex)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);//Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }

                //if (fieldName == this.Fields[this.editrow_code].FieldName)
                //    e.Value = dto.ID.Value;                
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
                DTO_MasterComplex dto = (DTO_MasterComplex)this.ValidateEditGrid();
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
                        List<DTO_MasterComplex> insertList = new List<DTO_MasterComplex>();
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
                    this.consulta = null;

                    long rowNumber = _bc.AdministrationModel.MasterComplex_Rownumber(this.DocumentID, dto.PKValues, this.consulta, null);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterComplexForm.cs", "TBSave"));
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

                //Campos Extras
                this.FrmProperties.Campos.ForEach(f =>
                {
                    if (f.ImportacionInd)
                    {
                        excell_app.AddData(row, col, _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + f.NombreColumna));
                        col++;
                    }
                });

                //Activo
                excell_app.AddData(row, col, _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd"));

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                throw ex;
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
            string msgRecordDeleteErr = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RecordDeleteErr);
            msgDeleteCode = string.Format(msgDeleteCode, string.Empty);

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

                    DTO_TxResult result = _bc.AdministrationModel.MasterComplex_Delete(this.DocumentID, this.complexDto.PKValues);

                    MessageForm frm = new MessageForm(result);
                    this.LoadGridData(false, false, false);
                    frm.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(msgRecordDeleteErr);
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                #region Carga los titulos de las columnas

                string colsRsx = string.Empty;
                string colsDescriptivo = string.Empty;
                string separator = ",";
                int i = 0;
                //Campos Extras
                this.FrmProperties.Campos.ForEach(f =>
                {
                    //Columnas de importacion
                    if (f.ImportacionInd)
                    {
                        if (i != 0)
                            colsRsx += separator;

                        colsRsx += _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + f.NombreColumna);
                        ++i;
                    }
                    else if (f.Tipo == "UDT_Descriptivo") // Columnas paar los descriptivos
                        colsDescriptivo += separator + _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + f.NombreColumna);
                });

                //Activo
                colsRsx += separator + _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd");
                colsRsx += colsDescriptivo;
                #endregion

                string fileName = _bc.AdministrationModel.MasterComplex_Export(this.DocumentID, colsRsx, this.consulta);
                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, fileName);

                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterSimpleForm.cs", "TBExport"));
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
                if (pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    bool sendToServer = true;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    List<DTO_MasterComplex> list = new List<DTO_MasterComplex>();
                    Dictionary<int, List<Tuple<string, bool>>> fks = new Dictionary<int, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    //Posición de las columnas
                    int activo_pos = 0;
                    Dictionary<string, int> colPos = new Dictionary<string, int>();
                    //Valores de las columnas al recorrer los textos
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
                                if (line[j].Equals(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd"), StringComparison.InvariantCultureIgnoreCase))
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
                            rd.Message = "OK";

                            #region Info básica
                            bool createDTO = true;
                            int max = 0;
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
                                    DTO_MasterComplex dto = this.LoadDTO(activo, 1, 1, colVals);

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
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm }); 
                        
                        if (result.Result.Equals(ResultValue.OK))
                            this.Invoke(refreshGrid);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterComplexForm.cs", "ImportThread"));
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
