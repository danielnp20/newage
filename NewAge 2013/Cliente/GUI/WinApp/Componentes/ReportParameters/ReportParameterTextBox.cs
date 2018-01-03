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
    public partial class ReportParameterTextBox : UserControl,IMultiReportParameter
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private Options _filterOptions;

        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad para asiganr las opciones del periodo
        /// </summary>
        public Options FilterOptions
        {
            get { return _filterOptions; }
            set {
                _filterOptions = value;
                this.Visible = true;
                switch (value)
                {
                    case Options.SingleCondition:
                        this.lblFrom.Visible = false;
                        this.txtFrom.Visible = true;
                        this.lblUntil.Visible = false;
                        this.txtUntil.Visible = false;
                        break;
                    case Options.RangeCondition:
                        this.lblFrom.Visible = true;
                        this.lblFrom.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblFrom");
                        this.txtFrom.Visible = true;
                        this.lblUntil.Visible = true;
                        this.lblUntil.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblUntil");
                        this.txtUntil.Visible = true;
                        break;                   
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Opcion seleccionada
        /// </summary>
        public int[] Option
        {
            get
            {
                List<int> resOption = new List<int>();
                try
                {                    
                    switch (this._filterOptions)
                    {
                        case Options.SingleCondition:
                            resOption.Add(Convert.ToInt32(this.txtFrom.Text));
                            break;
                        case Options.RangeCondition:
                            resOption.Add(Convert.ToInt32(this.txtFrom.Text));
                            resOption.Add(Convert.ToInt32(this.txtUntil.Text));
                            if (resOption[1] < resOption[0] || resOption[1] < 0 || resOption[0] < 0) new ArgumentNullException();
                            break;
                        default:
                            break;
                    }
                    return resOption.ToArray();
                }
                catch (Exception)
                {
                    resOption.Add(0);
                    return resOption.ToArray();
                }
            }
        }

        /// <summary>
        /// Asigna un nombre del Opcion
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="value"></param>
        public void SetItems(string name, List<ReportParameterListItem> items)
        {
            this.lblOption.Text = _bc.GetResource(LanguageTypes.Forms, name);
        }

        /// <summary>
        /// Devuelve valores digitados
        /// </summary>
        /// <returns></returns>
        public string[] GetSelectedValue()
        {
            List<string> res = new List<string>();
            foreach (int item in this.Option)
            {
                res.Add(item.ToString());
            }
            return res.ToArray();
            //return SelectedItemKey;
        }

        /// <summary>
        /// Limpia los campos
        /// </summary>
        public void Refresh()
        {
            switch (this._filterOptions)
            {
                case Options.SingleCondition:
                    this.txtFrom.Text = string.Empty;
                    break;
                case Options.RangeCondition:
                    this.txtFrom.Text = string.Empty;
                    this.txtUntil.Text= string.Empty;
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ReportParameterTextBox()
        {
            InitializeComponent();
        }

        #region Eventos

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtOption_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        //private void monthCB_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    if (this.monthCB1.Name.Contains("day"))
        //    {
        //        if (this.monthCB.SelectedItem != null)
        //        {
        //            int daysQty = DateTime.DaysInMonth(Convert.ToInt32(this.txtFrom.Text), Convert.ToInt32(this.monthCB.SelectedItem));
        //            this.monthCB1.Items.Clear();
        //            for (int i = 1; i <= daysQty; i++)
        //                this.monthCB1.Items.Add(i);
        //            this.monthCB1.SelectedItem = 1;
        //        }
        //    };
        //}

        #endregion
    }

    /// <summary>
    /// Diferentes opciones
    /// </summary>
    public enum Options
    {
        /// <summary>
        /// One value condition
        /// </summary>
        SingleCondition,
        /// <summary>
        /// Range condition
        /// </summary>
        RangeCondition       
    }
}
