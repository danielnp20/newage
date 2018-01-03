using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp;

namespace NewAge.Cliente.GUI.WinApp.ControlsUC
{
    /// <summary>
    /// Formulario para seleccionar Periodo extra
    /// </summary>
    public partial class ExtraPeriodsForm : Form
    {
        #region Variables

        internal int ResultPeriod = 12;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tit">Titulo del formulario</param>
        /// <param name="lblSel">Texto de seleccion para un periodo extra</param>
        /// <param name="btnAcc">Boton que esta ejecutando la accion</param>
        /// <param name="ExtraPeriods">Número de periodos extra</param>
        public ExtraPeriodsForm(string tit, string lblSel, string btnAcc, int ExtraPeriods=1)
        {
            InitializeComponent();
            this.Text = tit;
            this.lblSelect.Text = lblSel;
            this.btnSelect.Text = btnAcc;

            for (int i = 0; i <= ExtraPeriods; i++)
            {
                this.rdGroup.Properties.Items.Add(new RadioGroupItem(12+i,(12+i).ToString()));
            }
        }

        #region Eventos 

        /// <summary>
        /// Evento que se ejecuta al cambiar la selección de un periodo extra
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void rdGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnSelect.Enabled = true;
            ResultPeriod = (int)this.rdGroup.EditValue;
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar un periodo extra
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
