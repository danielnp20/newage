using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.ControlsUC
{
    public partial class uc_RegExTextBox : TextBox
    {
        #region Declaraciones

        private string mRegularExpression;

        #endregion

        #region Propiedades

        public string Regular_Expression
        {
            get { return mRegularExpression; }
            set { mRegularExpression = value; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public uc_RegExTextBox()
        {
            InitializeComponent();
        }

        #region Funciones publicas

        /// <summary>
        /// Valida la información ingresada en el control
        /// </summary>
        /// <returns>Retorna true si cumple la validación</returns>
        public bool ValidateControl()
        {
            string TextToValidate;
            Regex expression;

            try
            {
                TextToValidate = this.Text;
                expression = new Regex(Regular_Expression);
            }
            catch
            {
                return false;
            }

            // test text with expression
            if (expression.IsMatch(TextToValidate))
            {
                return true;
            }
            else
            {
                // no match
                return false;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Sobreescribe el evento que pinta el control
        /// </summary>
        /// <param name="pe">Argumentos del evento</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here
            // Calling the base class OnPaint
            base.OnPaint(pe);
        }

        #endregion
    }
}
