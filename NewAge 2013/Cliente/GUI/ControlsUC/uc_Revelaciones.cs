using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.ControlsUC
{
    public partial class uc_Revelaciones : UserControl
    {
        public uc_Revelaciones()
        {
            InitializeComponent();
        }
         
        /// <summary>
        /// Retorna el Texto Fomateado 
        /// </summary>
        public string ValueXML 
        {
            set { txtTexto.WordMLText = value; }
            get { return this.txtTexto.WordMLText; }
        }

        /// <summary>
        /// Retorna el Texto Fomateado 
        /// </summary>
        public string ValueHTML
        {
            set { txtTexto.HtmlText = value; }
            get { return this.txtTexto.HtmlText; }
        }

        /// <summary>
        /// Retorna el Texto Fomateado 
        /// </summary>
        public string Text
        {
            get { return this.txtTexto.Text; }
        }
      
    }
}
