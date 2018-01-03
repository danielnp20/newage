using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;


namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalQuerySolicitud : ModalQueryDocument
    {
        public ModalQuerySolicitud(List<int> filterDocument, bool isMulSelection = false, bool enableCopy = true) : base(filterDocument, isMulSelection, enableCopy) { }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        protected override void LoadData()
        {
            try
            {
                base.LoadData();
                if (this.cmbDestino.EditValue != "")
                {
                    List<DTO_glDocumentoControl> docFilter = new List<DTO_glDocumentoControl>();
                    foreach (DTO_glDocumentoControl item in this._listDocuments)
                    {
                        DTO_prSolicitud sol = this._bc.AdministrationModel.Solicitud_Load(AppDocuments.Solicitud, item.PrefijoID.Value, item.DocumentoNro.Value.Value);
                        if (sol.Header.Destino.Value == Convert.ToByte(this.cmbDestino.EditValue))
                            docFilter.Add(item);
                    }
                    this._listDocuments = docFilter;
                    if (this._listDocuments != null)
                    {
                        this.gcDocument.DataSource = this._listDocuments;
                        this._docCtrl = this._listDocuments.Count > 0 ? this._listDocuments[0] : null;
                        this.gvDocument.RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySolicitud.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        protected override void InitControls(List<int> filterDocument)
        {
            base.InitControls(filterDocument);

            #region Controles combo

            Dictionary<string, string> dicDestino = new Dictionary<string, string>();
            dicDestino.Add("", this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField));
            dicDestino.Add("0", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DestinoOrdenCompra));
            dicDestino.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DestinoContrato));
            this.cmbDestino.EditValue = "";
            this.cmbDestino.Properties.DataSource = dicDestino;

            #endregion
        }
    }
}
