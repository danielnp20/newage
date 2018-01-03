using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_acComparacionLibros
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acComparacionLibros(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                    this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["PlaquetaID"].ToString()))
                    this.PlaquetaID.Value = dr["PlaquetaID"].ToString();
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrEmpty(dr["SerialID"].ToString()))
                    this.SerialID.Value = dr["SerialID"].ToString();
                if (!string.IsNullOrEmpty(dr["LocFisicaID"].ToString()))
                    this.LocFisicaID.Value = dr["LocFisicaID"].ToString();
                if (!string.IsNullOrEmpty(dr["ActivoClaseID"].ToString()))
                    this.ActivoClaseID.Value = dr["ActivoClaseID"].ToString();
                if (!string.IsNullOrEmpty(dr["NombreClase"].ToString()))
                    this.NombreClase.Value = dr["NombreClase"].ToString();
                if (!string.IsNullOrEmpty(dr["SaldoMLFUNC"].ToString()))
                    this.SaldoMLFUNC.Value = Convert.ToDecimal(dr["SaldoMLFUNC"]);
                if (!string.IsNullOrEmpty(dr["SaldoMEFUNC"].ToString()))
                    this.SaldoMEFUNC.Value = Convert.ToDecimal(dr["SaldoMEFUNC"]);
                if (!string.IsNullOrEmpty(dr["VlrMtoMLFUNC"].ToString()))
                    this.VlrMtoMLFUNC.Value = Convert.ToDecimal(dr["VlrMtoMLFUNC"]);
                if (!string.IsNullOrEmpty(dr["VlrMtoMEFUNC"].ToString()))
                    this.VlrMtoMEFUNC.Value = Convert.ToDecimal(dr["VlrMtoMEFUNC"]);
                if (!string.IsNullOrEmpty(dr["SaldoMLIFRS"].ToString()))
                    this.SaldoMLIFRS.Value = Convert.ToDecimal(dr["SaldoMLIFRS"]);
                if (!string.IsNullOrEmpty(dr["SaldoMEIFRS"].ToString()))
                    this.SaldoMEIFRS.Value = Convert.ToDecimal(dr["SaldoMEIFRS"]);
                if (!string.IsNullOrEmpty(dr["VlrMtoMLIFRS"].ToString()))
                    this.VlrMtoMLIFRS.Value = Convert.ToDecimal(dr["VlrMtoMLIFRS"]);
                if (!string.IsNullOrEmpty(dr["VlrMtoMEIFRS"].ToString()))
                    this.VlrMtoMEIFRS.Value = Convert.ToDecimal(dr["VlrMtoMEIFRS"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acComparacionLibros()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoID = new UDTSQL_datetime();
            this.PlaquetaID = new UDT_PlaquetaID();
            this.Observacion = new UDT_DescripTExt();
            this.SerialID = new UDT_SerialID();
            this.LocFisicaID = new UDT_LocFisicaID();
            this.ActivoClaseID = new UDT_ActivoClaseID();
            this.NombreClase = new UDT_Descriptivo();
            this.SaldoMLFUNC = new UDT_Valor();
            this.SaldoMEFUNC = new UDT_Valor();
            this.VlrMtoMLFUNC = new UDT_Valor();
            this.VlrMtoMEFUNC = new UDT_Valor();
            this.SaldoMLIFRS = new UDT_Valor();
            this.SaldoMEIFRS = new UDT_Valor();
            this.VlrMtoMLIFRS = new UDT_Valor();
            this.VlrMtoMEIFRS = new UDT_Valor();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_datetime PeriodoID { get; set; }

        [DataMember]
        public UDT_PlaquetaID PlaquetaID { get; set; }

        [DataMember]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public UDT_LocFisicaID LocFisicaID { get; set; }

        [DataMember]
        public UDT_ActivoClaseID ActivoClaseID { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreClase { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Valor SaldoMLFUNC { get; set; }

        [DataMember]
        public UDT_Valor SaldoMEFUNC { get; set; }

        [DataMember]
        public UDT_Valor VlrMtoMLFUNC { get; set; }

        [DataMember]
        public UDT_Valor VlrMtoMEFUNC { get; set; }

        [DataMember]
        public UDT_Valor SaldoMLIFRS { get; set; }

        [DataMember]
        public UDT_Valor SaldoMEIFRS { get; set; }

        [DataMember]
        public UDT_Valor VlrMtoMLIFRS { get; set; }

        [DataMember]
        public UDT_Valor VlrMtoMEIFRS { get; set; }

        #endregion
    }
}
