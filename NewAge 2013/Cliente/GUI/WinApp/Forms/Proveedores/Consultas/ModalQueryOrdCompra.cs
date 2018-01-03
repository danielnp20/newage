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
    public partial class ModalQueryOrdCompra : ModalQueryDocument
    {
        public ModalQueryOrdCompra(List<int> filterDocument, bool isMulSelection = false, bool enableCopy = true) : base(filterDocument, isMulSelection, enableCopy) {  }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        protected override void LoadData()
        {
            try
            {
                base.LoadData();
                if (this.masterProveedor.ValidID)
                {
                    List<DTO_glDocumentoControl> docFilter = new List<DTO_glDocumentoControl>();
                    foreach (DTO_glDocumentoControl item in this._listDocuments)
                    {
                        DTO_prOrdenCompra oc = this._bc.AdministrationModel.OrdenCompra_Load(AppDocuments.OrdenCompra, item.PrefijoID.Value, item.DocumentoNro.Value.Value);
                        if (oc.HeaderOrdenCompra.ProveedorID.Value == this.masterProveedor.Value)
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
            this._bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, false);
        }
    }
}
