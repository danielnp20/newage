using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Cliente.GUI.WinApp.Clases
{

    public class GridProperty
    {
        #region Variables

        private bool _visible = true;
        private string _tab = string.Empty;

        #endregion

        #region Propiedades

        public string Campo { get; set; }
        public Object Valor { get; set; }
        /// <summary>
        /// Indica si es visible
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        /// <summary>
        /// Nombre del tab en el que se debe mostrar
        /// </summary>
        public string Tab
        {
            get { return ((string.IsNullOrWhiteSpace(_tab) ? string.Empty : _tab)); }
            set { _tab = value; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c">Campo</param>
        /// <param name="v">Valor del campo</param>
        /// <param name="visible">Si es visible</param>
        internal GridProperty(string c, string v, bool visible=true)
        {
            this.Campo = c;
            this.Valor = v;
            this.Visible = visible;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c">Campo</param>
        /// <param name="v">Valor del campo</param>
        /// <param name="visible">Si es visible</param>
        internal GridProperty(string c, Object v, bool visible = true)
        {
            this.Campo = c;
            this.Valor = v;
            this.Visible = visible;
        }
    }

     #region Funciones Internal

        internal class GridFilter
        {
            /// <summary>
            /// Obtiene y crea Columnas de la grilla de Edición
            /// </summary>
            /// <param name="c">Campo</param>
            /// <param name="v">Valor</param>
            internal GridFilter(string c, string mt, string t, bool af, bool asl, string or, int orix, string f)
            {
                this.Campo = c;
                this.Metadata = mt;
                this.Tipo = t;
                this.AplFiltro = af;
                this.AplSelec = asl;
                this.Orden = or;
                this.OrdIndex = orix;
                this.Filtro = f;

            }

            public string Campo { get; set; }
            public string Metadata { get; set; }
            public string Tipo { get; set; }
            public bool AplFiltro { get; set; }
            public bool AplSelec { get; set; }
            public string Orden { get; set; }
            public int OrdIndex { get; set; }
            public string Filtro { get; set; }
       
        }

    #endregion
}
