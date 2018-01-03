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
    /// <summary>
    /// Control de paginación
    /// </summary>
    public partial class uc_Pagging : UserControl
    {
        #region Propiedades

        /// <summary>
        /// Llave del recurso para poner el texto
        /// </summary>
        public string TextKey = "msg_pagging";

        /// <summary>
        /// Llave del recurso para poner el texto con el numero de registros
        /// </summary>
        public string RecordsKey = "msg_paggingrecords";

        /// <summary>
        /// Tamaño de la página
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Numero total de registros
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// Número de páginas
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Página Actual
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Asigna el texto del paginador
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Declaración Handlers

        // Declaración de delagados y evento click
        public delegate void EventHandler(object sender, System.EventArgs e);
        EventHandler handlerFirst;
        EventHandler handlerPreview;
        EventHandler handlerNext;
        EventHandler handlerLast;

        /// <summary>
        /// Asigna y remueve los handlers
        /// </summary>
        new public event EventHandler FirstClick
        {
            add { this.handlerFirst += value; }
            remove { this.handlerFirst -= value; }
        }

        /// <summary>
        /// Asigna y remueve los handlers
        /// </summary>
        new public event EventHandler FirstPage_Click
        {
            add { this.handlerFirst += value; }
            remove { this.handlerFirst -= value; }
        }

        /// <summary>
        /// Asigna y remueve los handlers
        /// </summary>
        new public event EventHandler PreviewPage_Click
        {
            add { this.handlerPreview += value; }
            remove { this.handlerPreview -= value; }
        }

        /// <summary>
        /// Asigna y remueve los handlers
        /// </summary>
        new public event EventHandler NextPage_Click
        {
            add { this.handlerNext += value; }
            remove { this.handlerNext -= value; }
        }

        /// <summary>
        /// Asigna y remueve los handlers
        /// </summary>
        new public event EventHandler LastPage_Click
        {
            add { this.handlerLast += value; }
            remove { this.handlerLast -= value; }
        }

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public uc_Pagging()
        {
            InitializeComponent();
        }

        #region Funciones Públicas

        /// <summary>
        /// Rutina para obtener el número de página del paginador
        /// </summary>
        /// <param name="count">Número total de resultados</param>
        /// <param name="firstTime">Indica si es </param>
        /// <param name="firstPage">Indica si es la primera página</param>
        /// <param name="lastPage">Indica si es la última página</param>
        public void UpdatePageNumber(long count, bool firstTime, bool firstPage, bool lastPage)
        {
            this.Count = count;
            int pCount = (int)(this.Count / this.PageSize);
            float fCount = ((float)this.Count / (float)this.PageSize);
            float ret = fCount - pCount;

            this.PageCount = ret == 0 ? pCount : pCount + 1;

            if (firstPage || firstTime || this.PageNumber == 0)
                this.PageNumber = 1;
            if (lastPage)
                this.PageNumber = pCount;

            this.EnableButtons();
            this.lblPage.Text = string.Format(this.Text, this.PageNumber, this.PageCount);
            this.lblRecords.Text = string.Format(this.RecordsKey, this.Count);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se ejecuta al hacer click en ir a la primera página
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        public virtual void btnFirstPage_Click(object sender, EventArgs e)
        {
            this.PageNumber = 1;
            if (this.handlerFirst != null)
                this.handlerFirst(sender, e);

            //Habilita los botones
            this.EnableButtons();
        }

        /// <summary>
        /// Evento que se ejecuta al hacer click en ir a la página anterior
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        public virtual void btnPreviewPage_Click(object sender, EventArgs e)
        {
            --this.PageNumber;
            if (this.handlerPreview != null)
                this.handlerPreview(sender, e);

            this.EnableButtons();
        }

        /// <summary>
        /// Evento que se ejecuta al hacer click en ir a la siguiente página
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        public virtual void btnNextPage_Click(object sender, EventArgs e)
        {
            ++this.PageNumber;
            if (this.handlerNext != null)
                this.handlerNext(sender, e);
  
            //Habilita los botones
            this.EnableButtons();
        }

        /// <summary>
        /// Evento que se ejecuta al hacer click en ir a la última página
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        public virtual void btnLastPage_Click(object sender, EventArgs e)
        {
            this.PageNumber = this.PageCount;
            if (this.handlerLast != null)
                this.handlerLast(sender, e);

            //Habilita los botones
            this.EnableButtons();
        }

        /// <summary>
        /// Habilita o deshabilita los botones
        /// </summary>
        private void EnableButtons()
        {
            //Habilita los botones
            if (this.PageCount > this.PageNumber)
            {
                this.btnNextPage.Enabled = true;
                this.btnLastPage.Enabled = true;
            }
            else
            {
                this.btnNextPage.Enabled = false;
                this.btnLastPage.Enabled = false;
            }

            if (this.PageNumber > 1)
            {
                this.btnFirstPage.Enabled = true;
                this.btnPreviewPage.Enabled = true;
            }
            else
            {
                this.btnFirstPage.Enabled = false;
                this.btnPreviewPage.Enabled = false;
            }
        }

        #endregion
    }
}
