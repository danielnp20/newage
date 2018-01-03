using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalEstadoLiqEmpleado : Form
    {
        #region Variables

        string _empleadoID;
        BaseController _bc = BaseController.GetInstance();
        DTO_noEstadoLiquidaciones _rx = null;

        #endregion

        public ModalEstadoLiqEmpleado(string empleadoID)
        {
            InitializeComponent();
            this._empleadoID = empleadoID;
            this.LoadData();
            this.InitControls();
            FormProvider.LoadResources(this, AppForms.ModalNomina);

        }

        public void InitControls()
        {
            if (this._rx != null)
            {
                this.txtEmpID.Text = this._rx.EmpleadoID.Value;
                this.txtNombreEmp.Text = this._rx.EmpleadoDesc.Value;
                this.dtFechaIniLiq.DateTime = this._rx.FechaIniLiq.Value.Value;
                this.dtFechaFinLiq.DateTime = this._rx.FechaFinLiq.Value.Value;
                this.chkVacaciones.Checked = this._rx.EnVacaciones.Value == 0 ? false : true;
                this.rbtEstado.SelectedIndex = this._rx.EstadoEmpleado.Value.Value - 1 ;
                this.rbtNomina.SelectedIndex = this.LoadIndexByEstado(this._rx.EstadoLiqNomina.Value.Value);
                this.rbtVacaciones.SelectedIndex = this.LoadIndexByEstado(this._rx.EstadoLiqVacaciones.Value.Value);
                this.rbtPrima.SelectedIndex = this.LoadIndexByEstado(this._rx.EstadoLiqPrima.Value.Value);
                this.rbtCesantias.SelectedIndex = this.LoadIndexByEstado(this._rx.EstadoLiqCesantias.Value.Value);
                this.rbtContrato.SelectedIndex = this.LoadIndexByEstado(this._rx.EstadoLiqContrato.Value.Value);
                this.rbtProvisiones.SelectedIndex = this.LoadIndexByEstado(this._rx.EstadoLiqProvisiones.Value.Value);
                this.rbtPlanilla.SelectedIndex = this.LoadIndexByEstado(this._rx.EstadoLiqPlanilla.Value.Value);
            }
        }

        public void LoadData()
        {
            try
            {
                this._rx = this._bc.AdministrationModel.Nomina_GetEstadoLiquidaciones(this._empleadoID);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Retorna el indice dependiendo del estado del documento control
        /// </summary>
        /// <param name="estado">estado</param>
        /// <returns>indice</returns>
        public byte LoadIndexByEstado(byte estado)
        {
            switch (estado)
            { 
                case (byte)EstadoDocControl.ParaAprobacion : 
                    {
                        return ((byte)EstadoDocControl.ParaAprobacion) - 1;            
                    }
                case (byte)EstadoDocControl.Aprobado:
                    {
                        return ((byte)EstadoDocControl.ParaAprobacion) - 1;
                    }
                default :
                    {
                        return 0;
                    };
            }
        }

    }
}
