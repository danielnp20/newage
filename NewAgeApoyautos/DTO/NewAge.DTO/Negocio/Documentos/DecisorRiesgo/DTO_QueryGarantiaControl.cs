using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_QueryGarantiaControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryGarantiaControl
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryGarantiaControl(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TipoPersona.Value = Convert.ToByte(dr["TipoPersona"]);
                this.ConsGarantia.Value = Convert.ToInt32(dr["ConsGarantia"]);
                this.Version.Value = Convert.ToByte(dr["Version"]);
                this.GarantiaID.Value = Convert.ToString(dr["GarantiaID"]);
                this.NroDoc.Value = Convert.ToString(dr["NroDoc"]);
                this.Propietario.Value = Convert.ToString(dr["Propietario"]);
                this.Documento.Value = Convert.ToString(dr["Documento"]);
                this.Referencia.Value = Convert.ToString(dr["Referencia"]);
                this.FechaRegistro.Value = Convert.ToDateTime(dr["FechaRegistro"]);
                this.FechaVTO.Value = Convert.ToDateTime(dr["FechaVTO"]);
                this.VlrGarantia.Value = Convert.ToDecimal(dr["VlrGarantia"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.CancelaInd.Value = Convert.ToBoolean(dr["CancelaInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryGarantiaControl()
        {
            InitCols();

        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.TipoPersona = new UDTSQL_tinyint();
            this.ConsGarantia= new UDT_Consecutivo();
            this.Version = new UDTSQL_tinyint();
            this.GarantiaID = new UDT_CodigoGrl10();
            this.NroDoc =new UDT_CodigoGrl10 ();
            this.Propietario = new UDT_CodigoGrl20();
            this.Documento = new UDT_CodigoGrl20();
            this.Referencia = new UDT_DescripTBase();
            this.FechaRegistro = new UDTSQL_smalldatetime();
            this.FechaVTO = new UDTSQL_smalldatetime();
            this.VlrGarantia = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
            this.CancelaInd = new UDT_SiNo();
            
        }

	    #endregion
        
        #region Propiedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoPersona { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsGarantia { get; set; }

        [DataMember]
        public UDTSQL_tinyint Version { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 GarantiaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 NroDoc { get; set; }
        
        [DataMember]
        public UDT_CodigoGrl20 Propietario { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 Documento { get; set; }

        [DataMember]
        public UDT_DescripTBase Referencia { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRegistro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVTO { get; set; }

        [DataMember]
        public UDT_Valor VlrGarantia { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_SiNo CancelaInd { get; set; }


        #endregion
    }
}
