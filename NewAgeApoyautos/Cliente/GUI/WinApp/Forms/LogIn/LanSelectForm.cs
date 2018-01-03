using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;

namespace NewAge.Cliente.GUI.WinApp.Forms
{

    public partial class LanSelectForm : Form
    {

        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private List<DTO_seLAN> _lans;
        private bool _retornar = false;

        #endregion

        #region Propiedades

        public DTO_seLAN SelectedConfig
        {
            get
            {
                object o = this.cmbLans.SelectedItem;
                if (o != null && o is DTO_seLAN)
                    return (o as DTO_seLAN);
                else
                    return null;
            }
        }

        public bool SelectedAsDefault
        {
            get
            {
                return this.chkDefault.Checked;
            }
        }

        #endregion

        public LanSelectForm(string iniLan)
        {
            InitializeComponent();
            _lans = _bc.AdministrationModel.seLAN_GetLanAll();
            this.cmbLans.Items.AddRange(_lans.ToArray());
            if (!string.IsNullOrWhiteSpace(iniLan))
            {
                IEnumerable<DTO_seLAN> fLans = _lans.Where(x => x.ID.Value == iniLan);
                if(fLans.Count() > 0)
                    this.cmbLans.SelectedItem = fLans.First();
                else
                {
                    this.cmbLans.Items.Add("Seleccione");
                    this.cmbLans.SelectedItem = "Seleccione";
                }
            }
            else
            {
                this.cmbLans.Items.Add("Seleccione");
                this.cmbLans.SelectedItem = "Seleccione";
            }
            //DTO_seLAN dummyForceWCF = DTO_seLAN.GetForceWCFDummy();
            // this.cmbLans.Items.Add(dummyForceWCF);
        }

        #region Eventos

        /// <summary>
        /// Se cierra la aplicacion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void LanSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_retornar)
                System.Environment.Exit(0);
        }

        /// <summary>
        /// Se realiza cuando se elige un item del combo de lans
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbLans_SelectedValueChanged(object sender, EventArgs e)
        {
            object o = cmbLans.SelectedItem;
            bool correct = false;
            DTO_seLAN config = null;
            if (o is DTO_seLAN)
            {
                config = (DTO_seLAN)o;
                correct = Program.TestConnection(config.CadenaConnLogger.Value, config.CadenaConnLogger.Value);
            }

            if (correct)
            {
                if (config!=null)
                    this.chkDefault.Enabled = true;

                this.chkDefault.Checked = false;
                this.btnAceptar.Enabled = true;
            }
            else
            {
                if (config != null)
                    MessageBox.Show("Conexión LAN " + config.ID.Value + " no disponible, por favor intente con otra o comuniquese con el administrador");
                
                this.chkDefault.Enabled = false;
                this.chkDefault.Checked = false;
                this.btnAceptar.Enabled = false;
            }

        }

        /// <summary>
        /// Se cierra la aplicacion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Ingresa a la aplicacion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this._retornar = true;
            this.Close();
        }

        #endregion
    }
}
