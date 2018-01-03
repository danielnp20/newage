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
    /// Clase del reporte Libro Mayor
    /// </summary>
    public class DTO_ReportLibroMayor
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportLibroMayor(IDataReader dr)
        {
            InitCols();
          try 
	        {	        
		        this.PeriodoID = Convert.ToDateTime(dr["PeriodoID"]);
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                this.SaldoInicial.Value = Convert.ToDecimal(dr["SaldoInicial"]);
                this.DebitoLocal.Value = Convert.ToDecimal(dr["DebitoLocal"]);
                this.CreditoLocal.Value = Convert.ToDecimal(dr["CreditoLocal"]);
                this.TOTAL.Value = Convert.ToDecimal(dr["TOTAL"]);
                this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaNaturaleza"].ToString()))
                    this.CuentaNaturaleza.Value = dr["CuentaNaturaleza"].ToString();

	        }
	        catch (Exception)
	        {
		        throw;
	        }

        }       

        public DTO_ReportLibroMayor(IDataReader dr, bool isNullble)
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
        public DTO_ReportLibroMayor()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaNaturaleza = new UDTSQL_char(3);
            this.PeriodoID = new DateTime();
            this.CuentaID = new UDT_CuentaID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.SaldoInicial = new UDT_Valor();
            this.DebitoLocal = new UDT_Valor();
            this.CreditoLocal = new UDT_Valor();
            this.TOTAL = new UDT_Valor();
            this.BalanceTipoID = new UDT_BalanceTipoID();

        }
       
        #region Propiedades

        [DataMember]
        public UDTSQL_char CuentaNaturaleza { get; set; }

        [DataMember]
        public DateTime PeriodoID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_Valor SaldoInicial { get; set; }

        [DataMember]
        public UDT_Valor DebitoLocal { get; set; }
        
        [DataMember]
        public UDT_Valor CreditoLocal { get; set; }

        [DataMember]
        public UDT_Valor TOTAL { get; set; }

        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; } 

        #endregion

    }
}
