using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_ccComisionDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccComisionDeta
    {
        #region DTO_ccComisionDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccComisionDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                if(!string.IsNullOrWhiteSpace(dr["NumDocCxP"].ToString()))
                    this.NumDocCxP.Value = Convert.ToInt32(dr["NumDocCxP"]);
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.VlrBase.Value = Convert.ToDecimal(dr["VlrBasePrivado"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocCxP"].ToString()))
                    this.VlrBasePrivado.Value = Convert.ToDecimal(dr["VlrBasePrivado"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrBasePublico"].ToString()))
                    this.VlrBasePublico.Value = Convert.ToDecimal(dr["VlrBasePublico"]);
                this.Porcentaje.Value = Convert.ToInt32(dr["Porcentaje"]);
                this.VlrComision.Value = Convert.ToDecimal(dr["VlrComision"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccComisionDeta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.NumDocCxP = new UDT_Consecutivo();
            this.AsesorID = new UDT_AsesorID();
            this.VlrBase = new UDT_Valor();
            this.VlrBasePrivado = new UDT_Valor();
            this.VlrBasePublico = new UDT_Valor();
            this.Porcentaje = new UDT_PorcentajeID();
            this.VlrComision = new UDT_Valor();

            //Campos Adicionales
            this.Aprobado = new UDT_SiNo();
            this.Detalle = new List<DTO_ccCreditoDocu>();
            this.FechaAprobacion = new UDTSQL_smalldatetime();
            this.Nombre = new UDT_DescripTBase();
            this.NumCreditos = new UDTSQL_int();
            this.Rechazado = new UDT_SiNo();
            this.PagaduriaID = new UDT_PagaduriaID ();
            this.Pagaduria = new UDT_DescripTBase();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCxP { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }	

        [DataMember]
        public UDT_Valor VlrBase { get; set; }	

        [DataMember]
        public UDT_PorcentajeID Porcentaje { get; set; }	

        [DataMember]
        public UDT_Valor VlrComision { get; set; }	

        [DataMember]
        public UDT_Valor VlrBasePrivado { get; set; }	

        [DataMember]
        public UDT_Valor VlrBasePublico { get; set; }	

        
        

        //Campos Adicionales
        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public List<DTO_ccCreditoDocu> Detalle { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAprobacion { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre { get; set; }

        [DataMember]
        public UDTSQL_int NumCreditos { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_DescripTBase Pagaduria { get; set; }


        #endregion
    }
}
