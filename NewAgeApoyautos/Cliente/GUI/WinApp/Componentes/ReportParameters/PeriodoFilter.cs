using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters
{
    public partial class PeriodoFilter : UserControl
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private PeriodFilterOptions _filterOptions;

        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad para asiganr las opciones del periodo
        /// </summary>
        public PeriodFilterOptions FilterOptions
        {
            get { return _filterOptions; }
            set {
                _filterOptions = value;
                this.Visible = true;
                switch (value)
                {
                    case PeriodFilterOptions.None:
                        this.Visible = false;
                        break;
                    case PeriodFilterOptions.Year:
                        this.ShowRow(0);
                        this.Years.Visible = false;
                        this.lblFromYear.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear"); 
                        this.lblUntilYear.Visible = false;
                        this.txtYear1.Visible = false;
                        this.HideRow(1);
                        break;
                    case PeriodFilterOptions.YearMonth:
                        this.ShowRow(0);
                        this.Years.Visible = false;
                        this.lblFromYear.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear"); 
                        this.lblUntilYear.Visible = false;
                        this.txtYear1.Visible = false;
                        this.ShowRow(1);
                        this.lblMonths.Visible = false;
                        this.lblFromMonth.Text =  _bc.GetResource(LanguageTypes.Forms,(AppForms.ReportForm).ToString() + "_lblMonth"); 
                        this.lblUntilMonth.Visible = false;
                        this.monthCB1.Visible = false;
                        this.monthCB1.Name = "monthCB1";
                        break;
                    case PeriodFilterOptions.YearMonthSpan:
                        this.ShowRow(0);
                        this.Years.Visible = false;
                        this.lblFromYear.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear"); 
                        this.lblUntilYear.Visible = false;
                        this.txtYear1.Visible = false;
                        this.lblMonths.Visible = true;
                        this.lblFromMonth.Text =  _bc.GetResource(LanguageTypes.Forms,(AppForms.ReportForm).ToString() + "_lblFrom"); 
                        this.lblUntilMonth.Visible = true;
                        this.monthCB1.Visible = true;
                        this.monthCB1.Name = "monthCB1";
                        this.ShowRow(1);
                        break;
                    case PeriodFilterOptions.YearMonthDay:
                        this.ShowRow(0);
                        this.Years.Visible = false;
                        this.lblFromYear.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear"); 
                        this.lblUntilYear.Visible = false;
                        this.txtYear1.Visible = false;
                        this.ShowRow(1);
                        this.lblMonths.Visible = false;
                        this.lblFromMonth.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblMonth"); 
                        this.lblUntilMonth.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblDay");
                        this.monthCB1.Visible = true;
                        this.monthCB1.Name = "dayCB";
                        break;
                    case PeriodFilterOptions.YearSpanMonthSpan:
                        this.ShowRow(0);
                        this.txtYear1.Visible = true;
                        this.ShowRow(1);
                        this.monthCB1.Visible = true;
                        this.monthCB1.Name = "monthCB1";
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Año seleccionado
        /// </summary>
        public int[] Year
        {
            get
            {
                List<int> resYear = new List<int>();
                try
                {                    
                    switch (this._filterOptions)
                    {
                        case PeriodFilterOptions.Year:
                        case PeriodFilterOptions.YearMonth:
                        case PeriodFilterOptions.YearMonthSpan:
                        case PeriodFilterOptions.YearMonthDay:
                            resYear.Add(Convert.ToInt32(this.txtYear.Text));
                            break;
                        case PeriodFilterOptions.YearSpanMonthSpan:
                            resYear.Add(Convert.ToInt32(this.txtYear.Text));
                            resYear.Add(Convert.ToInt32(this.txtYear1.Text));
                            break;
                        default:
                            break;
                    }
                    return resYear.ToArray();
                }
                catch (Exception)
                {
                    resYear.Add(0);
                    return resYear.ToArray();
                }
            }

        }

        /// <summary>
        /// Meses seleccionados. Si la opcion es de solo un mes el arreglo solo tiene un item
        /// </summary>
        public int[] Months
        {
            get
            {
                List<int> resMonth = new List<int>();
                switch (this._filterOptions)
                {
                    case PeriodFilterOptions.YearMonth:
                        resMonth.Add((int)this.monthCB.SelectedItem);
                        break;
                    case PeriodFilterOptions.YearMonthSpan:
                    case PeriodFilterOptions.YearSpanMonthSpan:
                    case PeriodFilterOptions.YearMonthDay:
                        resMonth.Add((int)this.monthCB.SelectedItem);
                        resMonth.Add((int)this.monthCB1.SelectedItem);
                        break;
                    default:
                        break;
                }
                return resMonth.ToArray();
            }
        }

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public PeriodoFilter()
        {
            InitializeComponent();
        }

        #region Funciones Privadas

        /// <summary>
        /// Oculta una fila
        /// </summary>
        /// <param name="row"></param>
        private void HideRow(int row){
            foreach (Control c in this.tbLyPn.Controls){
                if (this.tbLyPn.GetRow(c) == row)
                {
                    c.Visible = false;
                }
            }
        }

        /// <summary>
        /// Muestra una fila
        /// </summary>
        /// <param name="row"></param>
        private void ShowRow(int row)
        {
            foreach (Control c in this.tbLyPn.Controls)
            {
                if (this.tbLyPn.GetRow(c) == row)
                {
                    c.Visible = true;
                }
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            
        }

        private void monthCB_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.monthCB1.Name.Contains("day"))
            {
                if (this.monthCB.SelectedItem != null)
                {
                    int daysQty = DateTime.DaysInMonth(Convert.ToInt32(this.txtYear.Text), Convert.ToInt32(this.monthCB.SelectedItem));
                    this.monthCB1.Items.Clear();
                    for (int i = 1; i <= daysQty; i++)
                        this.monthCB1.Items.Add(i);
                    this.monthCB1.SelectedItem = 1;
                }
            };
        }

        #endregion

    }

    /// <summary>
    /// Diferentes opciones
    /// </summary>
    public enum PeriodFilterOptions
    {
        /// <summary>
        /// No hay filtro de periodo
        /// </summary>
        None,
        /// <summary>
        /// Solo muetra el año
        /// </summary>
        Year,
        /// <summary>
        /// Muestra el año y un combo para selecionar un mes
        /// </summary>
        YearMonth,
        /// <summary>
        /// Muestra el año y dos combos para selecionar un rango de meses
        /// </summary>
        YearMonthSpan,
        /// <summary>
        /// Muestra el año, combo para selecionar el mes y combo para selecionar el dia
        /// </summary>
        YearMonthDay,
        /// <summary>
        /// Muestra dos años y dos combos para selecionar un rango de meses
        /// </summary>
        YearSpanMonthSpan,
    }
}
