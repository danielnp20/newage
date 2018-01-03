using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ProcessForm : Form
    {
        BaseController _bc = BaseController.GetInstance();

        #region Hilos

        #region Variables y Propiedades

        /// <summary>
        /// Hilo con la barra de progreso
        /// </summary>
        public Thread ProgressBarThread
        {
            get;
            set;
        }

        /// <summary>
        /// Funcion que se ejecuta ver el estado 
        /// Dependiendo si es maestra, app, etc
        /// </summary>   
        public Func<int> FuncProgressBarThread
        {
            get;
            set;
        }

        #endregion

        #region Delegados

        protected delegate void UpdateProgress(int progress);
        protected UpdateProgress UpdateProgressDelegate;
        /// <summary>
        /// Delegado que actualiza la barra de progreso
        /// </summary>
        /// <param name="progress"></param>
        private void UpdateProgressBar(int progress)
        {
            this.pbProcess.Position = progress;
            this.pbProcess.Update();
        }

        protected delegate void ShowResultDialog(MessageForm frm);
        protected ShowResultDialog ShowResultDialogDelegate;
        /// <summary>
        /// Delegado que muestra un resultado dentro de un dialogo
        /// </summary>
        protected void ShowResultDialogMethod(MessageForm frm)
        {
            frm.ShowDialog();
        }

        #endregion

        #endregion

        #region Variables

        protected DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();

        #endregion

        #region Propiedades

        /// <summary>
        /// Identificador del documento
        /// </summary>
        protected int documentID { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessForm()
        {
            //this.InitializeComponent();
            this.InitForm();

            #region Carga la info de las actividades
            List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

            if (actividades.Count != 1)
            {
                string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                MessageBox.Show(string.Format(msg, this.documentID.ToString()));
            }
            else
            {
                string actividadFlujoID = actividades[0];
                this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
            }
            #endregion

            this.UpdateProgressDelegate = new UpdateProgress(this.UpdateProgressBar);
            this.ShowResultDialogDelegate = new ShowResultDialog(this.ShowResultDialogMethod);
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected virtual void InitForm() { }

        #endregion

        #region Funciones Protected (Hilos)

        /// <summary>
        /// Revisa el estado del proceso que se este corriendo en el servidor
        /// </summary>
        protected void CheckServerProcessStatus()
        {
            try
            {
                this.Invoke(this.UpdateProgressDelegate, new object[] { 0 });
                while (true)
                {
                    //Thread.Sleep(1000);
                    int progress = this.FuncProgressBarThread.Invoke();
                    this.Invoke(this.UpdateProgressDelegate, new object[] { progress });
                }
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Cancela el hilo que se este ejecutando con la barra de estado
        /// </summary>
        protected void StopProgressBarThread()
        {
            try
            {
                this.ProgressBarThread.Abort();
                this.Invoke(this.UpdateProgressDelegate, new object[] { 0 });
            }
            catch (Exception e)
            {
                ;
            }
        }

        #endregion
    }
}
