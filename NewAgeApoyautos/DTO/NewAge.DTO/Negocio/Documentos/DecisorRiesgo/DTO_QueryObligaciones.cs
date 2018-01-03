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
    /// Models DTO_QueryObligaciones
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryObligaciones
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryObligaciones(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TipoPersona.Value = Convert.ToByte(dr["TipoPersona"]);
                this.NumeroDocCRD.Value = Convert.ToInt32(dr["NumeroDocCRD"]);
                this.Version.Value = Convert.ToByte(dr["Version"]);
                this.Oblig.Value = Convert.ToInt32(dr["Oblig"]);
                this.Pagare.Value = Convert.ToString(dr["Pagare"]); 
                this.LineaCreditoID.Value = Convert.ToString(dr["LineaCreditoID"]);
                this.CuotasMora.Value = Convert.ToInt32(dr["CuotasMora"]);
                this.Altura.Value = Convert.ToString(dr["Altura"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.SdoCapital.Value = Convert.ToDecimal(dr["SdoCapital"]);
                this.SdoVencido.Value = Convert.ToDecimal(dr["SdoVencido"]);
                this.SdoCredito.Value = Convert.ToDecimal(dr["SdoCredito"]);
                this.Cubrimiento.Value = Convert.ToDecimal(dr["Cubrimiento"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.CancelaInd.Value = Convert.ToBoolean(dr["CancelaInd"]);
                this.NivelRiesgo.Value = Convert.ToByte(dr["NivelRiesgo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryObligaciones()
        {
            InitCols();

        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();            
            this.TipoPersona = new UDTSQL_tinyint();
            this.NumeroDocCRD = new UDT_Consecutivo();
            this.Version = new UDTSQL_tinyint();
            this.Oblig = new UDT_Consecutivo();
            this.Pagare = new UDTSQL_char(15);
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.CuotasMora = new UDTSQL_int();
            this.Altura=new UDT_CodigoGrl10();
            this.VlrCuota=new UDT_Valor();
            this.SdoCapital = new UDT_Valor();
            this.SdoVencido = new UDT_Valor();
            this.SdoCredito=new UDT_Valor();
            this.Cubrimiento=new UDT_PorcentajeID();
            this.Consecutivo = new UDT_Consecutivo();
            this.CancelaInd = new UDT_SiNo();
            this.NivelRiesgo = new UDTSQL_tinyint();
        }

	    #endregion
        
        #region Propiedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoPersona { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocCRD { get; set; }

        [DataMember]
        public UDTSQL_tinyint Version { get; set; }

        [DataMember]
        public UDT_Consecutivo Oblig { get; set; }

        [DataMember]
        public UDTSQL_char Pagare { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCreditoID { get; set; }

        [DataMember]
        public UDTSQL_int CuotasMora { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 Altura { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor SdoCapital { get; set; }

        [DataMember]
        public UDT_Valor SdoVencido { get; set; }

        [DataMember]
        public UDT_Valor SdoCredito { get; set; }

        [DataMember]
        public UDT_PorcentajeID Cubrimiento { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_SiNo CancelaInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint NivelRiesgo { get; set; }


        #endregion
    }
}
