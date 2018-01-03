using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Auxiliar
    /// </summary>
    public class DTO_ReportBalanceComparativo : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportBalanceComparativo(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                    this.PeriodoID = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["libroFunc"].ToString()))
                    this.libroFunc.Value = dr["libroFunc"].ToString();
                if (!string.IsNullOrEmpty(dr["saldoInicialFuncML"].ToString()))
                    this.saldoInicialFuncML.Value = Convert.ToDecimal(dr["saldoInicialFuncML"]);
                if (!string.IsNullOrEmpty(dr["saldoInicialFuncME"].ToString()))
                    this.saldoInicialFuncME.Value = Convert.ToDecimal(dr["saldoInicialFuncME"]);
                if (!string.IsNullOrEmpty(dr["VlrMtoFuncML"].ToString()))
                    this.VlrMtoFuncML.Value = Convert.ToDecimal(dr["VlrMtoFuncML"]);
                if (!string.IsNullOrEmpty(dr["VlrMtoFuncME"].ToString()))
                    this.VlrMtoFuncME.Value = Convert.ToDecimal(dr["VlrMtoFuncME"]);
                if (!string.IsNullOrEmpty(dr["libroAux"].ToString()))
                    this.libroAux.Value = dr["libroAux"].ToString();
                if (!string.IsNullOrEmpty(dr["saldoInicialLibroML"].ToString()))
                    this.saldoInicialLibroML.Value = Convert.ToDecimal(dr["saldoInicialLibroML"]);
                if (!string.IsNullOrEmpty(dr["saldoInicialLibroME"].ToString()))
                    this.saldoInicialLibroME.Value = Convert.ToDecimal(dr["saldoInicialLibroME"]);
                if (!string.IsNullOrEmpty(dr["VlrMtoLibroML"].ToString()))
                    this.VlrMtoLibroML.Value = Convert.ToDecimal(dr["VlrMtoLibroML"]);
                if (!string.IsNullOrEmpty(dr["VlrMtoLibroME"].ToString()))
                    this.VlrMtoLibroME.Value = Convert.ToDecimal(dr["VlrMtoLibroME"]);
                if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["ajusteSaldoML"].ToString()))
                    this.ajusteSaldoML.Value = Convert.ToDecimal(dr["ajusteSaldoML"]);
                if (!string.IsNullOrEmpty(dr["ajusteSaldoME"].ToString()))
                    this.ajusteSaldoME.Value = Convert.ToDecimal(dr["ajusteSaldoME"]);
                if (!string.IsNullOrEmpty(dr["ajusteMovientoML"].ToString()))
                    this.ajusteMovientoML.Value = Convert.ToDecimal(dr["ajusteMovientoML"]);
                if (!string.IsNullOrEmpty(dr["ajusteMovientoME"].ToString()))
                    this.ajusteMovientoME.Value = Convert.ToDecimal(dr["ajusteMovientoME"]);
                if (!string.IsNullOrEmpty(dr["saldoNuevoFuncML"].ToString()))
                    this.saldoNuevoFuncML.Value = Convert.ToDecimal(dr["saldoNuevoFuncML"]);
                if (!string.IsNullOrEmpty(dr["saldoNuevoFuncME"].ToString()))
                    this.saldoNuevoFuncME.Value = Convert.ToDecimal(dr["saldoNuevoFuncME"]);
                if (!string.IsNullOrEmpty(dr["saldoNuevoAuxML"].ToString()))
                    this.saldoNuevoAuxML.Value = Convert.ToDecimal(dr["saldoNuevoAuxML"]);
                if (!string.IsNullOrEmpty(dr["saldoNuevoAuxME"].ToString()))
                    this.saldoNuevoAuxME.Value = Convert.ToDecimal(dr["saldoNuevoAuxME"]);
                if (!string.IsNullOrEmpty(dr["ajusteNuevoML"].ToString()))
                    this.ajusteNuevoML.Value = Convert.ToDecimal(dr["ajusteNuevoML"]);
                if (!string.IsNullOrEmpty(dr["ajusteNuevoME"].ToString()))
                    this.ajusteNuevoME.Value = Convert.ToDecimal(dr["ajusteNuevoME"]);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DTO_ReportBalanceComparativo(IDataReader dr, bool isNullble)
        {
            InitCols();
            try
            {

            }
            catch (Exception e)
            { ; }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportBalanceComparativo()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoID = new DateTime();
            this.libroFunc = new UDT_Descriptivo();
            this.saldoInicialFuncML = new UDT_Valor();
            this.saldoInicialFuncME = new UDT_Valor();
            this.VlrMtoFuncML = new UDT_Valor();
            this.VlrMtoFuncME = new UDT_Valor();
            this.libroAux = new UDT_Descriptivo();
            this.saldoInicialLibroML = new UDT_Valor();
            this.saldoInicialLibroME = new UDT_Valor();
            this.VlrMtoLibroML = new UDT_Valor();
            this.VlrMtoLibroME = new UDT_Valor();
            this.CuentaID = new UDT_CuentaID();
            this.Descriptivo = new UDT_Descriptivo();
            this.ajusteSaldoML = new UDT_Valor();
            this.ajusteSaldoME = new UDT_Valor();
            this.ajusteMovientoML = new UDT_Valor();
            this.ajusteMovientoME = new UDT_Valor();
            this.saldoNuevoFuncML = new UDT_Valor();
            this.saldoNuevoFuncME = new UDT_Valor();
            this.saldoNuevoAuxML = new UDT_Valor();
            this.saldoNuevoAuxME = new UDT_Valor();
            this.ajusteNuevoML = new UDT_Valor();
            this.ajusteNuevoME = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public DateTime PeriodoID { get; set; }

        [DataMember]
        public UDT_Descriptivo libroFunc { get; set; }

        [DataMember]
        public UDT_Valor saldoInicialFuncML { get; set; }

        [DataMember]
        public UDT_Valor saldoInicialFuncME { get; set; }

        [DataMember]
        public UDT_Valor VlrMtoFuncML { get; set; }

        [DataMember]
        public UDT_Valor VlrMtoFuncME { get; set; }

        [DataMember]
        public UDT_Descriptivo libroAux { get; set; }

        [DataMember]
        public UDT_Valor saldoInicialLibroML { get; set; }

        [DataMember]
        public UDT_Valor saldoInicialLibroME { get; set; }

        [DataMember]
        public UDT_Valor VlrMtoLibroML { get; set; }

        [DataMember]
        public UDT_Valor VlrMtoLibroME { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor ajusteSaldoML { get; set; }

        [DataMember]
        public UDT_Valor ajusteSaldoME { get; set; }

        [DataMember]
        public UDT_Valor ajusteMovientoML { get; set; }

        [DataMember]
        public UDT_Valor ajusteMovientoME { get; set; }

        [DataMember]
        public UDT_Valor saldoNuevoFuncML { get; set; }

        [DataMember]
        public UDT_Valor saldoNuevoFuncME { get; set; }

        [DataMember]
        public UDT_Valor saldoNuevoAuxML { get; set; }

        [DataMember]
        public UDT_Valor saldoNuevoAuxME { get; set; }

        [DataMember]
        public UDT_Valor ajusteNuevoML { get; set; }

        [DataMember]
        public UDT_Valor ajusteNuevoME { get; set; }


        #endregion

    }
}
