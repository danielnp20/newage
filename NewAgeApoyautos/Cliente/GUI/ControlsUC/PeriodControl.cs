using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.ComponentModel;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Calendar;
using DevExpress.XtraEditors.Popup;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.ControlsUC
{
    /// <summary>
    /// Control que extiende la funcional de un control de fecha
    /// Solo permite seleccionar un periodo (año y mes)
    /// </summary>
    internal class PeriodControl : DateEdit
    {
        #region Variables
        
        private bool _disableCheck = false;
        private RepositoryItemDateEdit _fProperties;

        #endregion

        #region Propiedades

        /// <summary>
        /// Mensaje: "Periodo Extra"
        /// </summary>
        public string PeriodTitFrm { get; set; }

        /// <summary>
        /// Mensaje: "Seleccione un periodo:" 
        /// </summary>
        public string PeriodSelect { get; set; }

        /// <summary>
        /// Mensaje: "Aceptar" 
        /// </summary>
        public string PeriodAccept { get; set; }

        /// <summary>
        /// Propiedad para asignar el periodo
        /// </summary>
        public DateTime DateTimePeriodo
        {
            get
            {
                return this.DateTime;
            }
            set
            {
                this._disableCheck = true;
                this.DateTime = value;
                this._disableCheck = false;
            }
        }

        /// <summary>
        /// Número de periodos extras
        /// </summary>
        private int _extraPeriods = 0;
        public int ExtraPeriods
        {
            get
            {
                return _extraPeriods;
            }
            set
            {
                if (value >= 0)
                    this._extraPeriods = value;
                else
                    throw new Exception("Periodos extra debe ser un número positivo");
            }
        }

        #endregion  

        #region Constructores

        /// <summary>
        /// Inicializa los controles de interfaz
        /// </summary>
        private void InitializeComponent()
        {
            this.EditValue = null;
            this.Name = "dtPeriod";
            this.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Properties.Appearance.Options.UseFont = true;
            this.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Properties.DisplayFormat.FormatString = "yyyy/MM";
            this.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Properties.EditFormat.FormatString = "yyyy/MM";
            this.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Properties.Mask.EditMask = "yyyy/MM";
            this.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.Size = new System.Drawing.Size(98, 20);
            this.DateTimeChanged += new System.EventHandler(this.PeriodControl_DateTimeChanged);
        }

        /// <summary>
        /// Constructor Base
        /// </summary>
        public PeriodControl()
            : base()
        {
            Properties.Mask.EditMask = "y";
            InitializeComponent();
            this.ExtraPeriods = 0;
        }

        /// <summary>
        /// Constructor con el numero de periodos extras
        /// </summary>
        public PeriodControl(int extraPeriods=0)
            : base()
        {
            Properties.Mask.EditMask = "y";
            InitializeComponent();
            this.ExtraPeriods = extraPeriods;
        }

        #endregion

        #region Funciones Protected

        /// <summary>
        /// Sobreescribe la funcion que abre el cuadro Popup para laselección de fecha (periodo)
        /// </summary>
        protected override PopupBaseForm CreatePopupForm()
        {
            if (Properties.VistaDisplayMode == DevExpress.Utils.DefaultBoolean.True || Properties.VistaDisplayMode == DevExpress.Utils.DefaultBoolean.Default)
                return new MyVistaPopupDateEditForm(this);
            return base.CreatePopupForm();
        }

        #endregion

        #region Funciones Internas

        /// <summary>
        /// Inicializa los recursos para el formulario del periodo
        /// </summary>
        /// <param name="tit">Titulo</param>
        /// <param name="lblSel">Texto para selección de periodo</param>
        /// <param name="btnAcc">Texto para boton de selección de periodo</param>
        internal void InitFormResources(string tit, string lblSel, string btnAcc)
        {
            this.PeriodTitFrm = tit;
            this.PeriodSelect = lblSel;
            this.PeriodAccept = btnAcc;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se ejecuta al seleccionar una nueva fecha
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void PeriodControl_DateTimeChanged(object sender, EventArgs e)
        {
            if (!_disableCheck)
            {
                if (this.DateTime.Month == 12 && this.ExtraPeriods != 0)
                {
                    ExtraPeriodsForm form = new ExtraPeriodsForm(this.PeriodTitFrm, this.PeriodSelect, this.PeriodAccept, ExtraPeriods);
                    form.ShowDialog();
                    int period = form.ResultPeriod;
                    int day = period - 12 + 1;
                    _disableCheck = true;
                    this.DateTime = new DateTime(DateTime.Year, this.DateTime.Month, day);
                    _disableCheck = false;
                }
            }
        }

        #endregion

    }

    #region Clases con utilidades para el control

    //The attribute that points to the registration method 
    [UserRepositoryItem("RegisterPeriodControl")]
    public class RepositoryItemPeriodControl : RepositoryItemDateEdit
    {
        static RepositoryItemPeriodControl() { RegisterPeriodControl(); }
        public const string PeriodControlName = "PeriodControl";
        public override string EditorTypeName { get { return PeriodControlName; } }
        public static void RegisterPeriodControl()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(PeriodControlName,
                typeof(PeriodControl), typeof(RepositoryItemPeriodControl), typeof(DateEditViewInfo), new ButtonEditPainter(), true));
        }
    }

    public class MyVistaPopupDateEditForm : VistaPopupDateEditForm
    {
        private int _extraPeriods = 0;

        public int ExtraPeriods
        {
            get
            {
                return _extraPeriods;
            }
        }

        public MyVistaPopupDateEditForm(DateEdit ownerEdit)
            : base(ownerEdit )
        {
            (this.Calendar as MyVistaDateEditCalendar).ExtraPeriods = (ownerEdit as PeriodControl).ExtraPeriods;
            this._extraPeriods = (ownerEdit as PeriodControl).ExtraPeriods;
        }

        protected override DateEditCalendar CreateCalendar()
        { 
            return new MyVistaDateEditCalendar(OwnerEdit.Properties, OwnerEdit.EditValue, this.ExtraPeriods); 
        }
    }

    public class MyVistaDateEditCalendar : VistaDateEditCalendar
    {
        public int ExtraPeriods=0;

        public MyVistaDateEditCalendar(RepositoryItemDateEdit item, object editDate, int extraPeriods) : base(item, editDate) {
            this.ExtraPeriods = extraPeriods;
        }

        protected override void Init()
        {
            base.Init();
            this.View = DateEditCalendarViewType.YearInfo;
        }

        protected override void OnItemClick(DevExpress.XtraEditors.Calendar.CalendarHitInfo hitInfo)
        {
            DayNumberCellInfo cell = hitInfo.HitObject as DayNumberCellInfo;
            if (View == DateEditCalendarViewType.YearInfo)
            {
                DateTime? dt=null;
                //if (cell.Date.Month == 12 && ExtraPeriods > 0)
                //{
                //    //ExtraPeriodsForm form = new ExtraPeriodsForm(ExtraPeriods);
                //    //form.ShowDialog();
                //    //int period = form.ResultPeriod;
                //    //int day=period-12+1;
                //    dt = new DateTime(DateTime.Year, cell.Date.Month, 15);
                //}
                //else
                    dt = new DateTime(DateTime.Year, cell.Date.Month, 1);
                if (dt!=null)
                    OnDateTimeCommit(dt.Value, false);
                else
                    base.OnItemClick(hitInfo);
            }
            else
                base.OnItemClick(hitInfo);
        }
    }

    #endregion
}
