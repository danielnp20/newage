using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Inventarios de Balance
    /// </summary>
    public class DTO_ReportInventariosBalance
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportInventariosBalance(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["cuentaID"].ToString()))
                    this.cuentaID.Value = dr["cuentaID"].ToString();
                //if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                //    this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["BalanceTipoID"].ToString()))
                    this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaDesc"].ToString()))
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["DebitoML_Cuenta"].ToString()))
                    this.DebitoML_Cuenta.Value = Convert.ToDecimal(dr["DebitoML_Cuenta"]);
                if (!string.IsNullOrEmpty(dr["CreditoML_Cuenta"].ToString()))
                    this.CreditoML_Cuenta.Value = Convert.ToDecimal(dr["CreditoML_Cuenta"]);
                if (!string.IsNullOrEmpty(dr["InicialML_Cuenta"].ToString()))
                    this.InicialML_Cuenta.Value = Convert.ToDecimal(dr["InicialML_Cuenta"]);
                if (!string.IsNullOrEmpty(dr["FinalML_Cuenta"].ToString()))
                    this.FinalML_Cuenta.Value = Convert.ToDecimal(dr["FinalML_Cuenta"]);
                if (!string.IsNullOrEmpty(dr["TerceroInd"].ToString()))
                    this.TerceroInd.Value = Convert.ToBoolean(dr["TerceroInd"]);
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroDesc"].ToString()))
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["DebitoML_Tercero"].ToString()))
                    this.DebitoML_Tercero.Value = Convert.ToDecimal(dr["DebitoML_Tercero"]);
                if (!string.IsNullOrEmpty(dr["CreditoML_Tercero"].ToString()))
                    this.CreditoML_Tercero.Value = Convert.ToDecimal(dr["CreditoML_Tercero"]);
                if (!string.IsNullOrEmpty(dr["InicialML_Tercero"].ToString()))
                    this.InicialML_Tercero.Value = Convert.ToDecimal(dr["InicialML_Tercero"]);
                if (!string.IsNullOrEmpty(dr["FinalML_Tercero"].ToString()))
                    this.FinalML_Tercero.Value = Convert.ToDecimal(dr["FinalML_Tercero"]);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DTO_ReportInventariosBalance(IDataReader dr, bool isNullble)
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
        public DTO_ReportInventariosBalance()
        {
            InitCols();
            this.DebitoML_Cuenta.Value = 0;
            this.CreditoML_Cuenta.Value = 0;
            this.InicialML_Cuenta.Value = 0;
            this.FinalML_Cuenta.Value = 0;
            this.DebitoML_Tercero.Value = 0;
            this.CreditoML_Tercero.Value = 0;
            this.InicialML_Tercero.Value = 0;
            this.FinalML_Tercero.Value = 0; 
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.cuentaID = new UDT_CuentaID();
            this.PeriodoID = new UDT_PeriodoID();
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.DebitoML_Cuenta = new UDT_Valor();
            this.CreditoML_Cuenta = new UDT_Valor();
            this.InicialML_Cuenta = new UDT_Valor();
            this.FinalML_Cuenta = new UDT_Valor();
            this.TerceroInd = new UDT_SiNo();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.DebitoML_Tercero = new UDT_Valor();
            this.CreditoML_Tercero = new UDT_Valor();
            this.InicialML_Tercero = new UDT_Valor();
            this.FinalML_Tercero = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_CuentaID cuentaID { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_Valor DebitoML_Cuenta { get; set; }

        [DataMember]
        public UDT_Valor CreditoML_Cuenta { get; set; }

        [DataMember]
        public UDT_Valor InicialML_Cuenta { get; set; }

        [DataMember]
        public UDT_Valor FinalML_Cuenta { get; set; }

        [DataMember]
        public UDT_SiNo TerceroInd { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_Valor DebitoML_Tercero { get; set; }

        [DataMember]
        public UDT_Valor CreditoML_Tercero { get; set; }

        [DataMember]
        public UDT_Valor InicialML_Tercero { get; set; }

        [DataMember]
        public UDT_Valor FinalML_Tercero { get; set; }

        #region Region
        ////Listado de Terceros
        //[DataMember]
        //public List<DTO_PorTercero> PorTercero { get; set; } 
        #endregion
        #endregion
    }

    #region NO BORRAR
    //[DataContract]
    //[Serializable]
    ///// <summary>
    ///// Clase del Detalle de la Cuneta por Tercero
    ///// </summary>
    //public class DTO_PorTercero
    //{
    //    /// <summary>
    //    /// Constructor con DataReader
    //    /// </summary>
    //    public DTO_PorTercero(IDataReader dr)
    //    {
    //        this.InitCols();
    //        if (!string.IsNullOrEmpty(dr["TerceroID"].ToString().Trim()))
    //        {
    //            this.TerceroID.Value = dr["TerceroID"].ToString();
    //            this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
    //            this.DebitoML_Tercero.Value = Convert.ToDecimal(dr["DebitoML_Tercero"]);
    //            this.CreditoML_Tercero.Value = Convert.ToDecimal(dr["CreditoML_Tercero"]);
    //            this.InicialML_Tercero.Value = Convert.ToDecimal(dr["InicialML_Tercero"]);
    //            this.FinalML_Tercero.Value = Convert.ToDecimal(dr["FinalML_Tercero"]);
    //        }
    //    }

    //    public DTO_PorTercero(IDataReader dr, bool isNullble)
    //    {
    //        InitCols();
    //        try
    //        {

    //        }
    //        catch (Exception e)
    //        { ; }
    //    }

    //    /// <summary>
    //    /// Constructor por defecto
    //    /// </summary>
    //    public DTO_PorTercero()
    //    {
    //        InitCols();
    //    }

    //    /// Inicializa las columnas
    //    /// </summary>
    //    private void InitCols()
    //    {
    //        this.TerceroID = new UDT_TerceroID();
    //        this.TerceroDesc = new UDT_Descriptivo();
    //        this.DebitoML_Tercero = new UDT_Valor();
    //        this.CreditoML_Tercero = new UDT_Valor();
    //        this.InicialML_Tercero = new UDT_Valor();
    //        this.FinalML_Tercero = new UDT_Valor();
    //    }

    //    #region Propiedades
    //    /// <summary>
    //    /// Tercero ID
    //    /// </summary>
    //    [DataMember]
    //    public UDT_TerceroID TerceroID { get; set; }

    //    /// <summary>
    //    /// Descripcion  del Tercero
    //    /// </summary>
    //    [DataMember]
    //    public UDT_Descriptivo TerceroDesc { get; set; }

    //    /// <summary>
    //    /// Debito por Tercero (Moneda Local)
    //    /// </summary>
    //    [DataMember]
    //    public UDT_Valor DebitoML_Tercero { get; set; }

    //    /// <summary>
    //    /// Credito por Tercero (Moneda Local)
    //    /// </summary>
    //    [DataMember]
    //    public UDT_Valor CreditoML_Tercero { get; set; }

    //    /// <summary>
    //    /// Saldo Inicial por Tercero (Moneda Local)
    //    /// </summary>
    //    [DataMember]
    //    public UDT_Valor InicialML_Tercero { get; set; }

    //    /// <summary>
    //    /// Saldo Final por Tercero (Moneda Local)
    //    /// </summary>
    //    [DataMember]
    //    public UDT_Valor FinalML_Tercero { get; set; }
    //    #endregion
    //} 
    #endregion
}
