﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using NewAge.DTO.Resultados;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Pr_RecibidoDetallado : ReportBase
    {
        /// <summary>
        /// Contructor por defecto
        /// </summary>
        public Report_Pr_RecibidoDetallado()
        {

        }

        /// <summary>
        /// Constructor con parametros de conexion
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        public Report_Pr_RecibidoDetallado(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35088_RecibidosDetallado");
            this.lblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Año");
            this.lblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Mes");
            this.lblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Total");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Fecha");
            this.lblRecibido.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35088_Recibido");
            this.lblProveedor.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Proveedor");
        }

        /// <summary>
        /// Inicializa el reporte
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(DateTime Periodo, string proveedor, bool isDetallado, Dictionary<int, string> filtros, bool isFacturdo)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                ModuloProveedores _moduloProveedores = new ModuloProveedores(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ProveedoresTotal> data = _moduloProveedores.ReportesProveedores_Recibidos(Periodo, proveedor, isDetallado,null,filtros, isFacturdo);

                if (data.Count != 0)
                {
                    data[0].PeriodoInicial = Periodo;

                    this.DataSource = data;
                    this.ShowPreview();
                    //this.CreateReport();
                    result.ExtraField = this.ReportName;
                    result.Result = ResultValue.OK;
                }
                else
                    result.Result = ResultValue.NOK;

                return result;
            }
            catch (Exception)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                return result;
            }
        }

    }
}