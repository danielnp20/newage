using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante manual:
    /// Models DTO_ComprobanteHeader
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ComprobanteHeader
    {
        #region DTO_ComprobanteHeader

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ComprobanteHeader(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.MdaTransacc.Value = dr["MdaTransacc"].ToString(); 
                this.MdaOrigen.Value = Convert.ToByte(dr["MdaOrigen"]);
                this.TasaCambioBase.Value = Convert.ToDecimal(dr["TasaCambioBase"]);
                this.TasaCambioOtr.Value = Convert.ToDecimal(dr["TasaCambioOtr"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ComprobanteHeader()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.LibroID = new UDT_BalanceTipoID(); // Solo para migraciones

            this.EmpresaID = new UDT_EmpresaID();
            this.PeriodoID = new UDT_PeriodoID();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.NumeroDoc = new UDT_Consecutivo();
            this.MdaTransacc = new UDT_MonedaID();
            this.MdaOrigen = new UDTSQL_tinyint();
            this.TasaCambioBase = new UDT_TasaID();
            this.TasaCambioOtr = new UDT_TasaID();
        }

        #endregion

        #region Propiedades

        //Propiedad SOLO para migrar comprobantes
        [DataMember]
        public UDT_BalanceTipoID LibroID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteNro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_MonedaID MdaTransacc { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint MdaOrigen { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_TasaID TasaCambioBase { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_TasaID TasaCambioOtr { get; set; }

        #endregion
    }
}
