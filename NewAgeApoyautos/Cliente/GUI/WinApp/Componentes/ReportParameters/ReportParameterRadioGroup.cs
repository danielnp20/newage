using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;

namespace NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters
{
    public partial class ReportParameterRadioGroup : UserControl, IMultiReportParameter
    {

        #region Variables

        BaseController _bc = null;
        protected ReportParameterListSource source = ReportParameterListSource.Moneda;
        protected List<ReportParameterListItem> items = new List<ReportParameterListItem>();
        protected string defaultKey = string.Empty;
        public event ListValueChangedHandler ValueChangedEvent;

        #endregion

        #region Propiedades

        public ReportParameterListSource Source
        {
            get
            {
                return source;
            }
            set
            {
                this.lblNombre.Text = _bc.GetResource(LanguageTypes.Forms, value.ToString());
                source = value;
                RefreshList();
            }
        }

        public string DefaultKey
        {
            get { return defaultKey; }
            set
            {
                defaultKey = value;
                RefreshList();
            }
        }

        public string SelectedItemKey
        {
            get
            {
                return radioGroup.EditValue.ToString();
            }
        }

        #endregion

        #region Funciones Protected

        protected virtual void OnValueChangedEvent(EventArgs e)
        {
            ValueChangedEvent(this, e);
        }

        #endregion

        #region Funciones Publicas

        public void SetItems(string nombre,List<ReportParameterListItem> value)
        {
            this.lblNombre.Text = _bc.GetResource(LanguageTypes.Forms, nombre);
            items = value;
            RefreshList();
        }

        public void RefreshList()
        {
            try
            {
                this.radioGroup.Properties.Items.Clear();
                if (items.Count > 0)
                {
                    foreach (ReportParameterListItem li in items)
                    {
                        this.radioGroup.Properties.Items.Add(new RadioGroupItem(li.Key, li.Desc));
                    }
                }
                else
                {
                    switch (this.Source)
                    {
                        case ReportParameterListSource.Moneda:
                            this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = TipoMoneda.Local.ToString(), Description = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) });
                            this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = TipoMoneda.Foreign.ToString(), Description = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) });
                            this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = TipoMoneda.Both.ToString(), Description = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth) });
                            break;
                        case ReportParameterListSource.MonedaExcl:
                            this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = TipoMoneda.Local.ToString(), Description = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) });
                            this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = TipoMoneda.Foreign.ToString(), Description = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) });
                            break;
                        case ReportParameterListSource.Ordenamiento:
                            this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = TipoMoneda.Local.ToString(), Description = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account) });
                            this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = TipoMoneda.Foreign.ToString(), Description = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Voucher) });
                            break;
                        //case ReportParameterListSource.TipoReporte:
                        //    this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = TipoMoneda.Local.ToString(), Description = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Consolidated) });
                        //    this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = TipoMoneda.Foreign.ToString(), Description = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detailed) });
                        //    break;
                        case ReportParameterListSource.TipoBalance:
                            long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coBalanceTipo, null, null, true);
                            IEnumerable<DTO_MasterBasic> TiposBalance = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coBalanceTipo, count, 1, null, null, true);
                            foreach (var tipoBal in TiposBalance)
                                this.radioGroup.Properties.Items.Add(new RadioGroupItem() { Value = tipoBal.ID.Value, Description = tipoBal.Descriptivo.Value });
                            break;
                        default:
                            break;
                    }
                }
                int defaultIndex = 0;
                for (int i = 0; i < this.radioGroup.Properties.Items.Count; i++)
                {
                    RadioGroupItem o = this.radioGroup.Properties.Items[i];
                    if (o.Value.Equals(DefaultKey))
                    {
                        defaultIndex = i;
                        break;
                    }
                }
                if (this.radioGroup.Properties.Items.Count > defaultIndex)
                    this.radioGroup.SelectedIndex = defaultIndex;
                int height = this.radioGroup.Properties.Items.Count * 20;
                if (this.radioGroup.Properties.Items.Count > 0)
                    this.Height = height;
            }
            catch (Exception)
            {
                ;
            }
        }

        public string[] GetSelectedValue()
        {
            List<string> res = new List<string>();
            res.Add(SelectedItemKey);
            return res.ToArray();
            //return SelectedItemKey;
        }

        public ReportParameterRadioGroup()
        {
            try
            {
                InitializeComponent();
                _bc = BaseController.GetInstance();
            }
            catch (Exception)
            {
                ;
            }
        }

        #endregion

        #region Eventos

        private void radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnValueChangedEvent(e);
        }

        #endregion

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public List<ReportParameterListItem> Items
        //{
        //    get { return _items; }
        //    set
        //    {
        //        _items = value;
        //        RefreshList();
        //    }
        //}
    }
}
