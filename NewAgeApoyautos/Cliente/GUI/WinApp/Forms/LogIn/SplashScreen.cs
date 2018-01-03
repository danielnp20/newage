using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario que se muestra mientras carga la aplicación
    /// </summary>
    public partial class SplashScreen : Form
    {
        /// <summary>
        /// Contructor
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();
        }

        #region Funciones Publicas

        /// <summary>
        /// Función que permite desde el metodo que hace el cargue
        /// poner mensajes de estado . por ejemplo "Cargando idiomas"
        /// Utiliza el delegado para procesar los cambios desde otros hilos
        /// </summary>
        /// <param name="text">Cadena a mostrar en la pantalla de cargue</param>
        public void StateChanged(string text){
            if (this.label1.InvokeRequired)
            {
                StateChangedD d = new StateChangedD(StateChanged);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label1.Text = text;
            }
            
        }

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado para poder invocar StateChanged desde otro hilo
        /// </summary>
        /// <param name="text">Texto del SplashScreen</param>
        delegate void StateChangedD(string text);

        #endregion
    }
}
