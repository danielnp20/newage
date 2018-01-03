using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters
{
    public class MonthComboBox:ComboBox
    {
        #region Funciones Publicas
        public MonthComboBox():base()
        {
            this.Width = 35;
            this.DropDownStyle = ComboBoxStyle.DropDownList;         
            this.Enter += new System.EventHandler(this.CBEnter_Enter);
            this.DropDownClosed+=new EventHandler(MonthComboBox_DropDownClosed);
        }
        #endregion

        #region Eventos
        private void CBEnter_Enter(object sender, EventArgs e)
        {
            if (this.Name.Contains("month"))
            {
                this.Items.Clear();

                if (this.Name.Contains("_evenMonths"))
                {
                    for (int i = 1; i <= 12; i++)
                        if (i % 2 == 0) this.Items.Add(i);
                }
                else if (this.Name.Contains("_Anual"))
                {
                    this.Items.Add(1);
                }
                else if (this.Name.Contains("_Semestral"))
                {
                    this.Items.Add(1);
                    this.Items.Add(2);
                }
                else if (this.Name.Contains("_Bimestral"))
                {
                    for (int i = 1; i <= 6; i++)
                        this.Items.Add(i);
                }
                else if (this.Name.Contains("_Trimestral"))
                {
                    for (int i = 1; i <= 4; i++)
                        this.Items.Add(i);
                }
                else if (this.Name.Contains("_13"))
                {
                    for (int i = 1; i <= 13; i++)
                        this.Items.Add(i);
                }
                else
                {
                    for (int i = 1; i <= 12; i++)
                        this.Items.Add(i);
                };
            };
        }

        private void MonthComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (this.SelectedItem==null)
            { 
                this.SelectedIndex++;
            }
        }
        
        #endregion
    }
}
