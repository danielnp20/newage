using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Collections;
using System.Collections.Generic;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glTabla
    /// </summary>
    public partial class glTabla : MasterSimpleForm 
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glTabla() : base() {}

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            MDI mdi = FormProvider.Master;
            if (FormProvider.Master.LoadFormTB)
            {
                mdi.itemNew.Enabled = false;
                mdi.itemDelete.Enabled = false;
                mdi.itemImport.Enabled = false;
                mdi.itemGenerateTemplate.Enabled = false;
                mdi.itemSearch.Enabled = false;
                mdi.itemFilter.Enabled = false;
                mdi.itemFilterDef.Enabled = false;
            }
            mdi.itemNew.Visible = false;
            this.pgGrid.Visible = false;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glTabla;
            base.InitForm();
        }

        /// <summary>
        /// Sobrecargar para procesar el cambio en un campo especifico de la grilla de edición
        /// </summary>
        /// <param name="Field">Nombre del campo en la grilla de edición (caption)</param>
        /// <param name="Value">Valor ingresado</param>
        /// <param name="RowIndex">Numero de la fila en la grilla de edición</param>
        protected override bool FieldValidate(string Field, object Value, int RowIndex, out string err)
        {
            bool res = true; 
            err = string.Empty;
            FieldConfiguration fc = this.GetFieldConfigByCaption(Field);
            //Valida que la linea presupuestal sea el de la tabla de control
            if (fc.FieldName.StartsWith("lonNivel"))
            {
                if (Value != null && !string.IsNullOrWhiteSpace(Convert.ToString(Value)))
                {
                    try
                    {
                        int val = Convert.ToInt32(Value);
                        if (val < 1)
                        {
                            err = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_Tabla_LengthNotZero);
                            res = false;
                        }
                    }
                    catch (Exception e)
                    {
                        ;
                    }
                }
            }
            return res;
        }

        #region Sobrecarga métodos de la maestar simple

        /// <summary>
        /// Carga los datos de la grilla de edicion
        /// </summary>
        /// <param name="isNew">Dice si el registro es nuevo</param>
        /// <param name="rowIndex">Numero de fila</param>
        protected override void LoadEditGridData(bool isNew, int rowIndex)
        {
            try
            {
                base.LoadEditGridData(isNew, rowIndex);
                DTO_glTabla tabla = this.gvModule.GetRow(rowIndex) as DTO_glTabla;
                if (_bc.AdministrationModel.glTabla_HasData(tabla.TablaNombre.Value, _bc.GetMaestraEmpresaGrupoByDocumentID(tabla.DocumentoID.Value.Value)))
                    this.grlControlRecordEdit.Enabled = false;
                else
                    this.grlControlRecordEdit.Enabled = true;

                this.GetFieldConfigByFieldName("Descriptivo").Editable = false;
                this.GetFieldConfigByFieldName("DocumentoID").Editable = false;
                this.GetFieldConfigByFieldName("ActivoInd").Editable = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-glTabla.cs", "LoadEditGridData"));
            }
        }

        /// <summary>
        /// Trae los datos
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Número de página</param>
        /// <returns></returns>
        protected override IEnumerable GetPagedData(int pageNum)
        {
            Dictionary<int, string> empGrupo = new Dictionary<int, string>();
            DTO_glEmpresa emp = _bc.AdministrationModel.Empresa;

            empGrupo.Add((int)GrupoEmpresa.Automatico, emp.ID.Value);
            empGrupo.Add((int)GrupoEmpresa.Individual, emp.EmpresaGrupoID_.Value);
            empGrupo.Add((int)GrupoEmpresa.General, _bc.GetControlValue(AppControl.GrupoEmpresaGeneral));

            this.dtoList = _bc.AdministrationModel.glTabla_GetAllByEmpresaGrupo(empGrupo,true);
            return this.dtoList;
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
                gc.MainView.PostEditor();

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

                int rowNumber = -1;
                if (validDTO.Count == 0)
                {
                    rowNumber = this.gvModule.FocusedRowHandle;
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
                if (result.Result.Equals(ResultValue.OK))
                {
                    this.LoadGridData(false, false, false);
                    if(rowNumber!=-1)
                        this.gvModule.MoveBy(rowNumber);


                    //Trae la lista de maestras con su configuracion
                    var masters = _bc.AdministrationModel.aplMaestraPropiedad_GetAll();
                    this._bc.AdministrationModel.MasterProperties.Clear();
                    foreach (DTO_aplMaestraPropiedades p in masters)
                        _bc.AdministrationModel.MasterProperties.Add(p.DocumentoID, p);

                    //Carga las propiedades de las tables
                    Dictionary<int, string> empGrupo = new Dictionary<int, string>();

                    empGrupo.Add((int)GrupoEmpresa.Automatico, _bc.AdministrationModel.Empresa.ID.Value);
                    empGrupo.Add((int)GrupoEmpresa.Individual, _bc.AdministrationModel.Empresa.EmpresaGrupoID_.Value);
                    empGrupo.Add((int)GrupoEmpresa.General, _bc.GetControlValue(AppControl.GrupoEmpresaGeneral));

                    _bc.AssignTablesByCompany(empGrupo);

                    this.Insertando = insertandoTemp;
                    frm.ShowDialog();
                }
                else
                {
                    frm.ShowDialog();
                    return;
                }

                if (this.Insertando)
                    this.TBNew();
                else
                    this.Insertando = false;

                this.gvRecordEdit.PostEditor();
                foreach (GridControl gc in this.extraGrids.Values)
                    gc.MainView.PostEditor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }//clase
}//namespace
       

     