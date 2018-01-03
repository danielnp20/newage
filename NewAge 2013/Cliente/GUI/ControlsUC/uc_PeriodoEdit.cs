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
    public partial class uc_PeriodoEdit : UserControl
    {
        #region Variables

        /// <summary>
        /// Cantidad de periodos extra que admite
        /// </summary>
        private int _extraPeriods = 0;

        #endregion

        #region Propiedades

        public int ExtraPeriods
        {
            get
            {
                return _extraPeriods;
            }
            set
            {
                if (value >= 0)
                {
                    this._extraPeriods = value;
                    this.dtPeriod.ExtraPeriods = value;
                }
                else
                    throw new Exception("Periodos extra debe ser un número positivo");
            }
        }

        /// <summary>
        /// Se usa para obtener el valor seleccionado
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return this.dtPeriod.DateTimePeriodo;
            }
            set
            {
                this.dtPeriod.DateTimePeriodo = value;
            }
        }

        /// <summary>
        /// Habilita o desabilita el control
        /// </summary>
        public bool EnabledControl
        {
            get
            {
                return this.dtPeriod.Enabled;
            }
            set
            {
                this.dtPeriod.Enabled = value;
            }
        }

        /// <summary>
        /// Asigna el valor maximo
        /// </summary>
        public DateTime MinValue
        {
            get { return this.dtPeriod.Properties.MinValue; }
            set { this.dtPeriod.Properties.MinValue = value; }
        }

        /// <summary>
        /// Asigna el valor maximo
        /// </summary>
        public DateTime MaxValue
        {
            get { return this.dtPeriod.Properties.MaxValue; }
            set { this.dtPeriod.Properties.MaxValue = value; }
        }

        /// <summary>
        /// Asigna el valor maximo
        /// </summary>
        public string LblExtra
        {
            get { return this.lblExtra.Text; }
        }

        #endregion

        #region Declaracion Handlers

        public delegate void EventHandler();
        public event EventHandler ValueChanged;

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public uc_PeriodoEdit()
        {
            InitializeComponent();
            this.EnabledControl = true;
        }

        #region Funciones Publicas

        /// <summary>
        /// Constructor
        /// <param name="fType">Formulario</param>
        /// <param name="t">Tabla del sistema</param>
        /// <param name="modalArgs">Argumentos para el formulario modal</param>
        /// </summary>
        public void InitControl(int extraPeriods, string tit, string lblSel, string btnAcc)
        {
            this.ExtraPeriods = extraPeriods;
            this.dtPeriod.InitFormResources(tit, lblSel, btnAcc);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento para actualizar el label cuando la fecha cambia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void dtPeriod_EditValueChanged(object sender, EventArgs e)
        {
            DateTime dt = this.dtPeriod.DateTime;
            if (dt.Ticks < new DateTime(1753, 1, 1).Ticks || dt.Ticks > new DateTime(9999, 12, 1).Ticks)
            {
                dt = new DateTime(1753, 1, 1);
                this.dtPeriod.DateTime = dt;
            }
            else
            {
                if (dt != null)
                {
                    if (dt.Day > 3)
                    {
                        dt = new DateTime(dt.Year, dt.Month, 1);
                        this.dtPeriod.DateTime = dt;
                    }

                    if (dt.Month == 12 && dt.Day > 1)
                    {
                        int periodo = dt.Day + 12 - 1;
                        periodo = Math.Min(periodo, ExtraPeriods + 12);
                        this.lblExtra.Text = periodo.ToString();
                    }
                    else
                    {
                        this.lblExtra.Text = dt.Month.ToString();
                    }
                }
            }

            if (this.ValueChanged != null)
                this.ValueChanged();
        }

        #endregion
    }
}
