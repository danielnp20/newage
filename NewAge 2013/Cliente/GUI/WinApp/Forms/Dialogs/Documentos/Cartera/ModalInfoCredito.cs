using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de consulta maestro (modal)
    /// </summary>
    public partial class ModalInfoCredito : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int documentID = AppQueries.QueryLibranza;
        private DTO_ccCreditoPlanPagos cuota = new DTO_ccCreditoPlanPagos();
        private List<DTO_ccSaldosComponentes> componentes = new List<DTO_ccSaldosComponentes>();
        private List<DTO_ccSaldosComponentes> componentesTemp = new List<DTO_ccSaldosComponentes>();
        #endregion

        /// <summary>
        /// Constructor de la grilla de plan pagos 
        /// </summary>
        /// <param name="numDoc">Num doc</param>
        /// <param name="libranza">Num de la libranza</param>
        /// <param name="fechaCorte">Fecha corte</param>
        public ModalInfoCredito(string clienteID, DTO_ccCreditoDocu credito)
        {
            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, false, false, true, false);
            this._bc.InitMasterUC(this.masterCiudad, AppMasters.glLugarGeografico, false, true, true, false);
            this._bc.InitMasterUC(this.masterCiudadAct, AppMasters.glLugarGeografico, false, true, true, false);
            this._bc.InitMasterUC(this.masterCodeudor1, AppMasters.coTercero, false, true, true, false);
            this._bc.InitMasterUC(this.masterCodeudor2, AppMasters.coTercero, false, true, true, false);
            this._bc.InitMasterUC(this.masterCodeudor3, AppMasters.coTercero, false, true, true, false);
            this._bc.InitMasterUC(this.masterCodeudor4, AppMasters.coTercero, false, true, true, false);
            this._bc.InitMasterUC(this.masterCodeudor5, AppMasters.coTercero, false, true, true, false);
            DTO_ccCliente cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, clienteID, true);
            this.masterCliente.EnableControl(false);
            this.masterCiudad.EnableControl(false);
            this.masterCiudadAct.EnableControl(false);
            this.masterCodeudor1.EnableControl(false);
            this.masterCodeudor2.EnableControl(false);
            this.masterCodeudor3.EnableControl(false);
            this.masterCodeudor4.EnableControl(false);
            this.masterCodeudor5.EnableControl(false);
            this.LoadData(cliente, credito);
        }

        #region Funciones privadas

        /// <summary>
        /// Carga los datos 
        /// </summary>
        private void LoadData(DTO_ccCliente cliente,DTO_ccCreditoDocu credito)
        {
            try
            {
                //Datos Cliente
                this.masterCliente.Value = cliente.ID.Value;
                this.txtNombre.Text = cliente.Descriptivo.Value;
                this.txtDireccion.Text = cliente.ResidenciaDir.Value;
                this.masterCiudad.Value = cliente.ResidenciaCiudad.Value;
                this.masterCiudadAct.Value = cliente.CiudadAct.Value;
                this.txtTel1.Text = cliente.Telefono1.Value;
                this.txtExt1.Text = cliente.Extension1.Value;
                this.txtTel2.Text = cliente.Telefono2.Value;
                this.txtExt2.Text = cliente.Extension2.Value;
                this.txtCel1.Text = cliente.Celular1.Value;
                this.txtCel2.Text = cliente.Celular2.Value;
                this.txtCorreo.Text = cliente.Correo.Value;
                this.txtTel1Act.Text = cliente.Telefono1Act.Value;
                this.txtTel2Act.Text = cliente.Telefono2Act.Value;
                this.txtCel1Act.Text = cliente.Celular1Act.Value;
                this.txtCel2Act.Text = cliente.Celular2Act.Value;
                this.txtCorreoAct.Text = cliente.CorreoAct.Value;
                if (cliente.FechaNacimiento.Value.HasValue)
                {
                    this.dtFechaNac.DateTime = cliente.FechaNacimiento.Value.Value;
                    this.txtEdad.Text = (DateTime.Today.Year - cliente.FechaNacimiento.Value.Value.Year).ToString();
                }
                else
                    this.dtFechaNac.ResetText();
                //Datos Conyuge
                this.txtCedulaEsposa.Text = cliente.CedEsposa.Value;
                this.txtNombreEsposa.Text = cliente.NomEsposa.Value;
                if (cliente.FechEsposa.Value.HasValue)
                {
                    this.dtFechaNacEsposa.DateTime = cliente.FechEsposa.Value.Value;
                    this.txtEdadEsposa.Text = (DateTime.Today.Year - cliente.FechEsposa.Value.Value.Year).ToString();
                }
                else
                    this.dtFechaNacEsposa.ResetText();


                //Datos Credito
                this.txtLibranza.Text = credito.Libranza.Value.ToString();
                this.txtPagare.Text = credito.Pagare.Value;
                this.txtPagarePol.Text = credito.PagarePOL.Value;
                this.masterCodeudor1.Value = credito.Codeudor1.Value;
                this.masterCodeudor2.Value = credito.Codeudor2.Value;
                this.masterCodeudor3.Value = credito.Codeudor3.Value;
                this.masterCodeudor4.Value = credito.Codeudor4.Value;
                this.masterCodeudor5.Value = credito.Codeudor5.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalInfoCredito.cs", "LoadData"));
            }
        }

        #endregion

    }
}
