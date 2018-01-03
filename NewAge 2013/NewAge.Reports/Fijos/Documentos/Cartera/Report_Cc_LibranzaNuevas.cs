﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Linq;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Cc_LibranzaNuevas : ReportBase
    {
        #region Variables
        #endregion
        public Report_Cc_LibranzaNuevas()
        {
            
        }

        public Report_Cc_LibranzaNuevas(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Créditos Nuevos");          
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime? fechaIni, DateTime fechaFin, string cliente)
        {
            try
            {
                #region Asigna Info Header
                this.lblPEriodo.Text = fechaIni.Value.ToString(FormatString.Period) + " hasta " + fechaFin.ToString(FormatString.Period);
                #endregion

                #region Asigna Filtros
                this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDatasource.Queries[0].Parameters[1].Value = fechaFin;
                this.QueriesDatasource.Queries[0].Parameters[2].Value = fechaIni;
                this.QueriesDatasource.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(cliente) ? cliente : null;
                #endregion        

                base.ConfigureConnection(this.QueriesDatasource);
                base.CreateReport();
                return base.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
