using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.IO;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;


namespace NewAge.Cliente.GUI.WinApp.Reports
{
    public partial class BaseReport : DevExpress.XtraReports.UI.XtraReport
    {
        protected BaseController _bc = BaseController.GetInstance();

        /// <summary>
        /// Para guardar la consulta asociada enc aso de que exista
        /// </summary>
        public DTO_glConsulta Consulta = null;

        /// <summary>
        /// Constructor del Base Report (Template for Other reports)
        /// </summary>
        public BaseReport()
        {
            InitializeComponent();
            DateTime localNow = DateTime.Now;
            this.lblParamFecha.Text=localNow.ToString();
            this.lblNombreEmpresa.Text=_bc.AdministrationModel.Empresa.Descriptivo.Value;
            this.lblUserName.Text = _bc.AdministrationModel.User.Descriptivo.Value;
            this.lblUser.Text = _bc.GetResource(LanguageTypes.Messages, this.lblUser.Text);
            this.lblFecha.Text = _bc.GetResource(LanguageTypes.Messages, this.lblFecha.Text);
            this.lblPage.Text = _bc.GetResource(LanguageTypes.Messages, this.lblPage.Text);            
                
            byte[] logo = _bc.AdministrationModel.glEmpresaLogo();
            try
            {
                MemoryStream ms = new MemoryStream(logo);
                Image logoImage = Image.FromStream(ms);
                this.imgLogoEmpresa.Image = logoImage;
            }
            catch (Exception)
            {
                ;
            }
            //this.Margins.Top = 30;
            //this.Margins.Bottom = 30;
            //this.Margins.Left = 30;
            //this.Margins.Right = 30;
        }

    }
}
