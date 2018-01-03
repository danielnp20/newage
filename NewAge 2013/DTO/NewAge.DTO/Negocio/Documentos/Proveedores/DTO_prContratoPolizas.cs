using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_prContratoPolizas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prContratoPolizas : DTO_BasicReport
    {
        

        #region Constructores
        public DTO_prContratoPolizas(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                this.CubrimientoPolizaID.Value = Convert.ToString(dr["CubrimientoPolizaID"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoMvto"].ToString()))
                    this.TipoMvto.Value = Convert.ToByte(dr["TipoMvto"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaINI"].ToString()))
                    this.FechaINI.Value = Convert.ToDateTime(dr["FechaINI"]);
                this.PorCubrimiento.Value = Convert.ToDecimal(dr["PorCubrimiento"]);
                this.VlrCubrimiento.Value = Convert.ToDecimal(dr["VlrCubrimiento"]);
                this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prContratoPolizas()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.CubrimientoPolizaID = new UDT_CodigoGrl10();
            this.TipoMvto = new UDTSQL_tinyint();
            this.FechaINI = new UDTSQL_datetime();
            this.FechaVto = new UDTSQL_datetime();
            this.PorCubrimiento = new UDT_PorcentajeID();
            this.VlrCubrimiento = new UDT_Valor();
            this.Observacion = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();

        }
        #endregion

        #region Propriedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 CubrimientoPolizaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoMvto { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaINI { get; set; }
        
        [DataMember]
        public UDTSQL_datetime FechaVto { get; set; }
        
        [DataMember]
        public UDT_PorcentajeID PorCubrimiento { get; set; }
        
        [DataMember]
        public UDT_Valor VlrCubrimiento { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion
    }

}
