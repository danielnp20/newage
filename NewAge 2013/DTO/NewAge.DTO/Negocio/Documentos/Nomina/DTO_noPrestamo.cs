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
    public class DTO_noPrestamo
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noPrestamo(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
                this.FechaPrestamo.Value = Convert.ToDateTime(dr["Fecha"]);
                this.Numero.Value = Convert.ToInt32(dr["Numero"]);
                this.VlrPrestamo.Value = Convert.ToDecimal(dr["VlrPrestamo"].ToString());
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.DtoPrima.Value = Convert.ToDecimal(dr["DtoPrima"]);
                this.VlrAbono.Value = Convert.ToDecimal(dr["VlrAbono"]);
                this.ActivoInd.Value = Convert.ToBoolean(dr["ActivaInd"]);
                this.QuincenaPagos.Value = Convert.ToByte(dr["QuincenaPagos"]);
                this.DocPrestamo.Value = Convert.ToInt32(dr["DocPrestamo"]);
                this.DocCxP.Value = Convert.ToInt32(dr["DocCxP"]);
                this.ConceptoNODesc.Value = dr["Descriptivo"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noPrestamo()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.ConceptoNOID = new UDT_ConceptoNOID();
            this.FechaPrestamo = new UDTSQL_smalldatetime();
            this.Numero = new UDT_Consecutivo();
            this.VlrPrestamo = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.VlrSaldo = new UDT_Valor();
            this.DtoPrima = new UDT_Valor();
            this.VlrAbono = new UDT_Valor();
            this.ActivoInd = new UDT_SiNo();
            this.QuincenaPagos = new UDTSQL_tinyint();
            this.DocPrestamo = new UDT_Consecutivo();
            this.DocCxP = new UDT_Consecutivo();
            this.ConceptoNODesc = new UDT_Descriptivo();
        }
        #endregion
        
        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_ConceptoNOID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoNODesc { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaPrestamo { get; set; }

        [DataMember]
        public UDT_Consecutivo Numero { get; set; }

        [DataMember]
        public UDT_Valor VlrPrestamo { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldo { get; set; }

        [DataMember]
        public UDT_Valor DtoPrima { get; set; }

        [DataMember]
        public UDT_Valor VlrAbono { get; set; }

        [DataMember]
        public UDT_SiNo ActivoInd { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint QuincenaPagos { get; set; }

        [DataMember]
        public UDT_Consecutivo DocPrestamo { get; set; }

        [DataMember]
        public UDT_Consecutivo DocCxP { get; set; }
                   
    }
}
