using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Mask;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class coImpDeclaracionCalendario : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public coImpDeclaracionCalendario() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coImpDeclaracionCalendario;
            base.InitForm();
        }

        #region Validaciones Del formulario

        /// <summary>
        /// Asigna un editor de celda(button, check, textbox..) a la celda relacionada del index
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvRecordEdit_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            base.gvRecordEdit_CustomRowCellEditForEditing(sender, e);

            int impDec = AppMasters.coImpuestoDeclaracion;
            DTO_coImpDeclaracionCalendario dtoData = (DTO_coImpDeclaracionCalendario)gvModule.GetRow( this.gvModule.FocusedRowHandle);
            if (dtoData != null)
            {
                DTO_coImpuestoDeclaracion imp = (DTO_coImpuestoDeclaracion)_bc.GetMasterDTO(AppMasters.MasterType.Simple, impDec, false, dtoData.ImpuestoDeclID.Value, true);
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("Periodo");
                if (newFC1 != null && imp != null)
                {
                    if (newFC1 is IntRankConfiguration)
                    {
                        IntRankConfiguration cfc = (IntRankConfiguration)newFC1;
                        cfc.Regex = string.Empty;
                        this.cmbRank.Items.Clear();
                        for (int i = 1; i < imp.PeriodoDeclaracion.Value + 1; i++)
                        {
                            cfc.Regex += i != 1 ? "|" + i.ToString() : i.ToString();
                            this.cmbRank.Items.Add(i);
                        }
                        this.cmbRank.Mask.MaskType = MaskType.RegEx;
                        this.cmbRank.Mask.EditMask = cfc.Regex;
                    }
                } 
            }
        }

        /// <summary>
        /// Carga los datos de la grilla de edicion
        /// </summary>
        /// <param name="isNew">Dice si el registro es nuevo</param>
        /// <param name="rowIndex">Numero de fila</param>
        protected override void LoadEditGridData(bool isNew, int rowIndex)
        {
            base.LoadEditGridData(isNew, rowIndex);
            if (rowIndex >= 0)
            {
                if (!isNew)
                {
                    DTO_coImpDeclaracionCalendario impCalendario = this.gvModule.GetRow(rowIndex) as DTO_coImpDeclaracionCalendario;
                    if (impCalendario != null && impCalendario.NumeroDoc.Value.HasValue)
                    {
                        this.grlControlRecordEdit.Enabled = false;
                        FormProvider.Master.itemDelete.Enabled = false;
                    }
                    else
                    {
                        this.grlControlRecordEdit.Enabled = true;
                        FormProvider.Master.itemDelete.Enabled = true;
                    }
                }
                else
                {
                    this.grlControlRecordEdit.Enabled = true;
                    FormProvider.Master.itemDelete.Enabled = true;
                }
            }
        }

        #endregion

    }
}
