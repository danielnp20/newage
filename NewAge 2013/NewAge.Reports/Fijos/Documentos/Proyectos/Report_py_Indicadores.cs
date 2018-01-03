using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.Reports.Fijos
{
    public partial class Report_py_Indicadores : ReportBase
    {

        #region Variables        
        protected ModuloPlaneacion _moduloPlaneacion;
        protected ModuloGlobal _moduloGlobal;        
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected DTO_glEmpresa _empresa;
        protected int _userID;
        protected ModuloBase _moduloBase;
        protected ExportFormatType _formatType;
        protected string loggerConnectionStr;
        protected ReportProvider reportProvider;
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Report_py_Indicadores()
        {

        }

        public Report_py_Indicadores(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) 
        {
            this._connection = c;
            this._transaction = tx;
            this._empresa = empresa;
            this._userID = userId;
            this._formatType = formatType;
            this.loggerConnectionStr = loggerConn;

            this.lblReportName.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void SetInitParameters()
        {


            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo)
        {
            try
            {
                


                #region Asigna Info Header
                string mes = Convert.ToString(periodo.Month).TrimEnd();
                string year = Convert.ToString(periodo.Year).TrimEnd();

                this.lblFiltro.Text = "Periodo: " + mes + "/" + year;
                #endregion
                #region Asigna Filtros

                //this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                //this.QueriesDataSource.Queries[0].Parameters[1].Value = periodo;
                this._moduloPlaneacion = new ModuloPlaneacion(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                ModuloPlaneacion modPlaneacion = new ModuloPlaneacion(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);

                List<DTO_QueryIndicadores> Indicadores = modPlaneacion.plIndicadores(periodo);    
                
                #endregion

                DataSource = Indicadores;
                this.ShowPreview();
                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
