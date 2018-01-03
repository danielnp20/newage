using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.DTO.Negocio;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QueryOrdenLegalizacion : ModalQueryDocument
    {
        //public QueryOrdenLegalizacion()
        //{
        //    this.InitializeComponent();
        //}

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        protected override void LoadData()
        {
            base.LoadData();
            foreach (DTO_glDocumentoControl item in this._listDocuments)
	        {
		        
	        }
            if (this._listDocuments != null && this._listDocuments.Count > 0)
            {
                this.gcDocument.RefreshDataSource();
                this._docCtrl = this._listDocuments[0];
            }
        }
    }
}
