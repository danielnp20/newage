using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.IO;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using System.Collections.Generic;

namespace NewAge.ReportesComunes
{
    public partial class BaseCommonReport : XtraReport
    {
        /// <summary>
        /// Para guardar la consulta asociada enc aso de que exista
        /// </summary>
        public DTO_glConsulta Consulta = null;

        protected CommonReportDataSupplier _dataSupplier;

        public BaseCommonReport(CommonReportDataSupplier dataSupplier)
        {
            InitializeComponent();
            DateTime localNow = DateTime.Now;
            this.lblParamFecha.Text = localNow.ToString();
            this._dataSupplier = dataSupplier;
            this.Init();
        }

        /// <summary>
        /// Inicializa algunos valores
        /// </summary>
        protected virtual void Init()
        {
            this.lblNombreEmpresa.Text = _dataSupplier.GetNombreEmpresa();
            this.lblUserName.Text = _dataSupplier.GetUserName();
            this.lblUser.Text = _dataSupplier.GetResource(LanguageTypes.Messages, this.lblUser.Text);
            this.lblFecha.Text = _dataSupplier.GetResource(LanguageTypes.Messages, this.lblFecha.Text);
            this.lblPage.Text = _dataSupplier.GetResource(LanguageTypes.Messages, this.lblPage.Text);

            byte[] logo = _dataSupplier.GetLogoEmpresa();
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
        }

        /// <summary>
        /// Inicializa algunos valores
        /// </summary>
        public virtual void Init(string NombreEmpresa, string UserName, byte[] LogoEmpresa)
        {
            this.lblNombreEmpresa.Text = NombreEmpresa;
            this.lblUserName.Text = UserName;
            this.lblUser.Text = _dataSupplier.GetResource(LanguageTypes.Messages, this.lblUser.Text);
            this.lblFecha.Text = _dataSupplier.GetResource(LanguageTypes.Messages, this.lblFecha.Text);
            this.lblPage.Text = _dataSupplier.GetResource(LanguageTypes.Messages, this.lblPage.Text);

            byte[] logo = LogoEmpresa;
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
        }
    }
}
