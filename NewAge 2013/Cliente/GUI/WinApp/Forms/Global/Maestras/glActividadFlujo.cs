using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.API.Native;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glActividadFlujo
    /// </summary>
    public partial class glActividadFlujo : MasterSimpleForm
    {
        private int entradas = 0;
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        ///<summary>
        /// Constructor 
        /// </summary>
        public glActividadFlujo()
            : base()
        {
            try
            {
                entradas = 1;
                FormProvider.Master.itemSave.Enabled = true;
                bool IsCurrent = false;
                for (int i = 0; i < gvModule.DataRowCount; i++)
                {
                    FieldConfiguration newFC9 = this.GetFieldConfigByFieldName("DocumentoID");
                    FieldConfiguration newFC10 = this.GetFieldConfigByFieldName("ModuloID");
                    FieldConfiguration newFC12 = this.GetFieldConfigByFieldName("ModuloDesc");
                    FieldConfiguration newFC11 = this.GetFieldConfigByFieldName("UnidadTiempo");
                    newFC11.Editable = true;
                    string v = (this.GetEditRow(newFC9.ColumnIndex).Valor).ToString();
                    DTO_glDocumento glDocumento = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, v, true);

                    if (glDocumento != null)
                    {
                        this.SetEditGridValue(newFC10.ColumnIndex, glDocumento.ModuloID.Value);
                        this.SetEditGridValue(newFC12.ColumnIndex, glDocumento.ModuloDesc.Value);
                    }
                    string valor = gvModule.GetRowCellValue(i, "SistemaInd").ToString();

                    if (Convert.ToBoolean(valor))
                    {
                        FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("ActividadFlujoID");
                        FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("Descriptivo");
                        FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("ProcedimientoID");
                        //FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("TipoDocumento");
                        FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("LLamadaID");
                        FieldConfiguration newFC6 = this.GetFieldConfigByFieldName("SistemaInd");
                        newFC1.Editable = false;
                        newFC2.Editable = false;
                        newFC3.Editable = false;
                        //newFC4.Editable = false;
                        newFC5.Editable = true;
                        newFC6.Editable = false;

                        break;
                    }
                }
                if (!IsCurrent)
                    this.TBNew();
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glActividadFlujo;
            base.InitForm();

        }

        #region Eventos MDI
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            FormProvider.Master.itemImport.Enabled = false;
            FormProvider.Master.itemNew.Enabled = false;
            FormProvider.Master.itemSave.Enabled = false;
            FormProvider.Master.itemExport.Enabled = false;
            FormProvider.Master.itemDelete.Enabled = false;
        }
        #endregion

        #region Validaciones Del formulario

        //<summary>
        //Sobrecargar para procesar el cambio en un campo especifico de la grilla de edición
        //</summary>
        //<param name="Field">Nombre del campo en la grilla de edición (caption)</param>
        //<param name="Value">Valor ingresado</param>
        //<param name="RowIndex">Numero de la fila en la grilla de edición</param>
        protected override bool FieldValidate(string Field, object Value, int RowIndex, out string err)
        {
            bool res = true;
            err = string.Empty;
            FieldConfiguration fc = this.GetFieldConfigByCaption(Field);
            
            #region DocumentoID
            if (fc.FieldName == "DocumentoID")
            {
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("ModuloID");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("ModuloDesc");
                string v = Value.ToString();
                DTO_glDocumento glDocumento = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, v, true);
                if (glDocumento != null)
                {
                    if (glDocumento.ModuloID.Value != "CC")
                    {
                        string msgNO = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Gl_ActividadFlujoModuloDiferent);
                        MessageBox.Show(msgNO);
                    }
                    else
                    {
                        this.SetEditGridValue(newFC2.ColumnIndex, glDocumento.ModuloID.Value);
                        this.SetEditGridValue(newFC3.ColumnIndex, glDocumento.ModuloDesc.Value);
                        //this.gvRecordEdit.SetRowCellValue(newFC2.RowIndex, "ModuloID", glDocumento.ModuloID.Value);   
                    }
                }
  
            }
               
            #endregion
            return res;
        }

        /// <summary>
        /// Evento para eliminar un detalle
        /// </summary>
        public override void TBDelete()
        {
            FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("SistemaInd");
            bool value = Convert.ToBoolean(this.GetEditRow(newFC1.ColumnIndex).Valor);

            if (value)
            {
                string msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.GL_ActividadFlujoBorrar);
                MessageBox.Show(msg);
                return;
            }
            base.TBDelete();
        }

        /// <summary>
        /// Sobrecargar para modificar alguna configuración de campo de la maestra
        /// </summary>
        public override void CustomizeFieldsConfig()
        {
            string imp = string.Empty;
            try
            {
                ButtonEditFKConfiguration fc = (ButtonEditFKConfiguration)this.GetFieldConfigByFieldName("DocumentoTipo");
                if (fc != null)
                {
                    DTO_glConsultaFiltro fil = new DTO_glConsultaFiltro()
                           {
                               CampoFisico = "DocumentoTipo",
                               OperadorFiltro = OperadorFiltro.Igual,
                               ValorFiltro = ((int)FormTypes.Activities).ToString()
                           };
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    filtros.Add(fil);
                    fc.Filtros = filtros; 
                }
            }
            catch (Exception)
            {
                ;
            }
        }

        /// <summary>
        /// Evento para validar elcambio de row en la grilla superior de la maestra
        /// </summary>
        /// <param name="sender">Evento que envia el evento</param>
        /// <param name="e"></param>
        protected override void gvModule_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            base.gvModule_FocusedRowChanged(sender,e);
            DTO_glActividadFlujo glActFlu = (DTO_glActividadFlujo)this.selectedDto;
            if (glActFlu != null)
            {
                if (this.entradas != 0)
                {
                    if (glActFlu.DocumentoID.Value != null)
                    {
                        DTO_glDocumento glDocumento = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, glActFlu.DocumentoID.Value, true);

                        if (glDocumento != null)
                            glActFlu.ModuloID.Value = glDocumento.ModuloID.Value;
                    }
                }

                if (glActFlu.TipoActividad.Value == 2)
                {
                    FormProvider.Master.itemNew.Enabled = true;
                    FormProvider.Master.itemSave.Enabled = true;
                    FormProvider.Master.itemDelete.Enabled = true;
                }
                else
                {
                    FormProvider.Master.itemNew.Enabled = false;
                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemDelete.Enabled = false;
                }
            }
        }

        public override void TBNew()
        {
            FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("ActividadFlujoID");
            FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("Descriptivo");
            FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("ProcedimientoID");
            //FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("TipoDocumento");
            FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("LLamadaID");
            FieldConfiguration newFC6 = this.GetFieldConfigByFieldName("SistemaInd");
            FieldConfiguration newFC7 = this.GetFieldConfigByFieldName("ModuloID");
            FieldConfiguration newFC8 = this.GetFieldConfigByFieldName("ModuloDesc");
            newFC1.Editable = true;
            newFC2.Editable = true;
            newFC3.Editable = true;
            //newFC4.Editable = true;
            newFC5.Editable = true;
            newFC6.Editable = true;
            newFC7.Editable = true;
            newFC8.Editable = true;
            base.TBNew();
        }
        #endregion
    }//clase
}//namespace


