using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using System.Runtime.Serialization;

namespace NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters
{
    /// <summary>
    /// Tipos de fuentes de datos automaticas para las listas
    /// </summary>
    public enum ReportParameterListSource
    {
        Moneda,
        MonedaExcl,
        MonedaOrigen,
        Ordenamiento,
       // Rompimiento,
        //TipoReporte,
        TipoBalance,
        //BalanceDePruebaTipo
        BancoCuenta,
        Caja,
        DeclaracionImpuesto
    }

    public delegate void ListValueChangedHandler(object sender, EventArgs e);

    /// <summary>
    /// Clase para una lista de ociones para parámetros
    /// </summary>
    public partial class ReportParameterList : UserControl, IMultiReportParameter
    {
        public event ListValueChangedHandler ValueChangedEvent;

        protected virtual void OnValueChangedEvent(EventArgs e)
        {
            ValueChangedEvent(this, e);
        }

        BaseController _bc = null;

        protected bool _mandatory=false;

        /// <summary>
        /// Indica si es obligatoria
        /// </summary>
        public bool Mandatory
        {
            get
            {
                return _mandatory;
            }
            set
            {
                this._mandatory = value;
                RefreshList();
            }
        }

        protected ReportParameterListSource _source = ReportParameterListSource.Moneda;

        /// <summary>
        /// Fuente de datos. Si se asigna se llenan los items automaticamente
        /// </summary>
        public ReportParameterListSource Source
        {
            get
            {
                return _source;
            }
            set
            {
                this.lbl.Text = _bc.GetResource(LanguageTypes.Forms, AppForms.ReportForm + "_" + value.ToString());
                this.lbl.Width = 300;
                _source = value;
                RefreshList();
            }
        }

        protected List<ReportParameterListItem> _items = new List<ReportParameterListItem>();

        /// <summary>
        /// Asigna una lista de items
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="value"></param>
        public void SetItems(string nombre, List<ReportParameterListItem> value)
        {
            this.lbl.Text = _bc.GetResource(LanguageTypes.Forms, nombre);
            _items = value;
            RefreshList();
        }

        protected string _defaultKey = string.Empty;

        /// <summary>
        /// Asigna la llave por defecto
        /// </summary>
        public string DefaultKey
        {
            get { return _defaultKey; }
            set
            {
                _defaultKey = value;
                RefreshList();
            }
        }

        /// <summary>
        /// Remove item from ComboBox items list
        /// </summary>
        /// <param name="itemToRemove"> Receives the key of the item to remove </param>
        public void RemoveItem(string itemToRemove)
        {
            List<ReportParameterListItem> itemsList = new List<ReportParameterListItem>();
            itemsList =  this.comboEdit.Properties.Items.Cast<ReportParameterListItem>().ToList();
            foreach (ReportParameterListItem item in itemsList)
                if (item.Key == itemToRemove) this.comboEdit.Properties.Items.Remove(item);
        }

        /// <summary>
        /// Checks if ComboBox contains an item with specified key
        /// </summary>
        /// <param name="itemToRemove"> Receives the key of the item to check </param>
        public bool ContainsItem(string itemToCheck)
        {
            List<ReportParameterListItem> itemsList = new List<ReportParameterListItem>();
            itemsList = this.comboEdit.Properties.Items.Cast<ReportParameterListItem>().ToList();
            foreach (ReportParameterListItem item in itemsList)
                if (item.Key == itemToCheck) return true;
            return false;
        }

        /// <summary>
        /// Actualiza la lista y llena los valores si es una lista con fuente (Source)
        /// </summary>
        public void RefreshList()
        {
            try
            {
                this.comboEdit.Properties.Items.Clear();
                if (string.IsNullOrWhiteSpace(DefaultKey) && Mandatory)
                {
                    this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = "", Desc = "*" });
                }
                if (!Mandatory)
                    this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = "", Desc = "" });
                if (_items.Count > 0)
                {
                    this.comboEdit.Properties.Items.AddRange(_items);
                }
                else
                {
                    long count = 0;
                    switch (this.Source)
                    {
                        case ReportParameterListSource.Moneda:
                            this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = TipoMoneda.Local.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) });
                            if (this._bc.AdministrationModel.MultiMoneda) {
                                this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = TipoMoneda.Foreign.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) });
                                this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = TipoMoneda.Both.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth) }); };
                            break;
                        case ReportParameterListSource.MonedaExcl:
                            this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = TipoMoneda.Local.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) });
                            this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = TipoMoneda.Foreign.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) });
                            break;
                        case ReportParameterListSource.MonedaOrigen:
                            count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.glMoneda, null, null, true);
                            IEnumerable<DTO_MasterBasic> Monedas = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.glMoneda, count, 1, null, null, true);
                            foreach (var mda in Monedas)
                                this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Desc = mda.ID.ToString() + " - " + mda.Descriptivo.ToString()});
                            break;
                        case ReportParameterListSource.Ordenamiento:
                            this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = "CuentaID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account) });
                            this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = "ComprobanteID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Voucher) });
                            break;                      
                        case ReportParameterListSource.BancoCuenta:
                            count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.tsBancosCuenta, null, null, true);
                            IEnumerable<DTO_MasterBasic> BancoCuentas = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.tsBancosCuenta, count, 1, null, null, true);
                            foreach (var cuenta in BancoCuentas)
                                this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = cuenta.ID.ToString(), Desc = cuenta.ID.ToString() + " - " + cuenta.Descriptivo.ToString() });
                            break;
                        case ReportParameterListSource.Caja:
                            count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.tsCaja, null, null, true);
                            IEnumerable<DTO_MasterBasic> Cajas = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.tsCaja, count, 1, null, null, true);
                            foreach (var caja in Cajas)
                                this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = caja.ID.ToString(), Desc = caja.ID.ToString() + " - " + caja.Descriptivo.ToString() });
                            break;
                        case ReportParameterListSource.DeclaracionImpuesto:
                            count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coImpuestoDeclaracion, null, null, true);
                            IEnumerable<DTO_MasterBasic> DeclaracionesImpuesto = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coImpuestoDeclaracion, count, 1, null, null, true);
                            foreach (var declar in DeclaracionesImpuesto)
                                this.comboEdit.Properties.Items.Add(new ReportParameterListItem() { Key = declar.ID.ToString(), Desc = declar.ID.ToString() + " - " + declar.Descriptivo.ToString() });
                            break;
                        default:
                            break;
                    }
                }
                int defaultIndex = 0;
                for (int i=0;i<this.comboEdit.Properties.Items.Count;i++)
                {
                    object o=this.comboEdit.Properties.Items[i];
                    if ((o as ReportParameterListItem).Key.Equals(DefaultKey))
                    {
                        defaultIndex = i;
                        break;
                    }
                }
                if (this.comboEdit.Properties.Items.Count > defaultIndex)
                    this.comboEdit.SelectedIndex = defaultIndex;
            }
            catch (Exception)
            {
                ;
            }
        }

        /// <summary>
        /// Devuelve el item selecionado
        /// </summary>
        public ReportParameterListItem SelectedListItem
        {
            get
            {
                object o=comboEdit.SelectedItem;
                ReportParameterListItem li = (o as ReportParameterListItem);
                if (Mandatory && string.IsNullOrEmpty(li.Key))
                    return null;
                return li;
            }
        }

        /// <summary>
        /// Devuelve la llave del item selecionado
        /// </summary>
        /// <returns></returns>
        public string[] GetSelectedValue(){
            List<string> res = new List<string>();
            if (SelectedListItem!=null)
                res.Add(SelectedListItem.Key);
            else
                res.Add(string.Empty);
            return res.ToArray();
            //return SelectedListItem.Key;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ReportParameterList()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboEdit_SelectedValueChanged(object sender, EventArgs e)
        {
            this.OnValueChangedEvent(e);
        }
    }

    /// <summary>
    /// Item de una lista de llave,valor para listas y radiobutons
    /// </summary>
    public class ReportParameterListItem
    {
        public string Key;

        public string Desc;

        public override string ToString()
        {
            return Desc;
        }
    }
}
