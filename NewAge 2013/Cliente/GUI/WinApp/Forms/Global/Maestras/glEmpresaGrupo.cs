using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glEmpresaGrupo
    /// </summary>
    public partial class glEmpresaGrupo : MasterSimpleForm 
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glEmpresaGrupo() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glEmpresaGrupo;
            base.InitForm();
        }

        #region Sobrecarga métodos de la maestar simple

        /// <summary>
        /// Metodo encargado de encapsular la insercion de registros de maestras simples
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="insertList">Lista de dtos</param>
        /// <param name="userId">usuario logueado</param>
        /// <param name="documentId">docuemnto</param>
        /// <param name="accion">accion (insercion/importar)</param>
        /// <returns></returns>
        protected override DTO_TxResult DataAdd(List<DTO_MasterBasic> insertList, int accion)
        {
            byte[] bItems = CompressedSerializer.Compress<IEnumerable<DTO_MasterBasic>>(insertList);
            bool res = _bc.AdministrationModel.glEmpresaGrupo_Add(bItems);

            DTO_TxResult result = new DTO_TxResult();
            if (res)
                result.Result = ResultValue.OK;
            else
                result.Result = ResultValue.NOK;

            return result;
        }

        /// <summary>
        /// Boton para eliminar un o un conjunto de registros
        /// </summary>
        public override void TBDelete()
        {
            string msgTitleDelete = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete));
            string msgDeleteCode = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Code);
            string msgDeleteInvalidOp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_InvalidOP);
            string msgRecordDeleteErr = _bc.GetResource(LanguageTypes.Error, "ERR_SQL_2005");
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

                    bool res = _bc.AdministrationModel.glEmpresaGrupo_Delete(this.basicUDT.Value);

                    DTO_TxResult result = new DTO_TxResult();
                    if (res)
                        result.Result = ResultValue.OK;
                    else
                        result.Result = ResultValue.NOK;

                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    
                    this.LoadGridData(false, false, false);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(msgRecordDeleteErr);
            }
        }

        #endregion

    }//clase
}//namespace
       

     