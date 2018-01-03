using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del Reporte Facturas Por Pagar
    /// </summary>
    public class DTO_ReportAnticiposViaje
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportAnticiposViaje(IDataReader dr)
        {
            this.InitCols();

            //Valores Genericos
            if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
            if (!string.IsNullOrWhiteSpace(dr["Area"].ToString()))
                this.Area.Value = Convert.ToString(dr["Area"]);
            if (!string.IsNullOrWhiteSpace(dr["TipoViaje"].ToString()))
                this.TipoViaje.Value = Convert.ToByte(dr["TipoViaje"]);
            if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
            if (!string.IsNullOrWhiteSpace(dr["DocumentoTercero"].ToString()))
                this.DocumentoTercero.Value = Convert.ToString(dr["DocumentoTercero"]);
            if (!string.IsNullOrWhiteSpace(dr["NombreTercero"].ToString()))
                this.NombreTercero.Value = Convert.ToString(dr["NombreTercero"]);
            if (!string.IsNullOrWhiteSpace(dr["DiasAlojamiento"].ToString()))
                this.DiasAlojamiento.Value = Convert.ToByte(dr["DiasAlojamiento"]);
            else
                this.DiasAlojamiento.Value = 0;
            if (!string.IsNullOrWhiteSpace(dr["ValorAlojamiento"].ToString()))
                this.ValorAlojamiento.Value = Convert.ToDecimal(dr["ValorAlojamiento"]);
            else
                this.ValorAlojamiento.Value = 0;
            if (!string.IsNullOrWhiteSpace(dr["DiasAlimentacion"].ToString()))
                this.DiasAlimentacion.Value = Convert.ToByte(dr["DiasAlimentacion"]);
            else
                this.DiasAlimentacion.Value = 0;
            if (!string.IsNullOrWhiteSpace(dr["ValorAlimentacion"].ToString()))
                this.ValorAlimentacion.Value = Convert.ToDecimal(dr["ValorAlimentacion"]);
            else
                this.ValorAlimentacion.Value = 0;
            if (!string.IsNullOrWhiteSpace(dr["DiasTransporte"].ToString()))
                this.DiasTransporte.Value = Convert.ToByte(dr["DiasTransporte"]);
            else
                this.DiasTransporte.Value = 0;
            if (!string.IsNullOrWhiteSpace(dr["ValorTransporte"].ToString()))
                this.ValorTransporte.Value = Convert.ToDecimal(dr["ValorTransporte"]);
            else
                this.ValorTransporte.Value = 0;
            if (!string.IsNullOrWhiteSpace(dr["DiasOtrosGastos"].ToString()))
                this.DiasOtrosGastos.Value = Convert.ToByte(dr["DiasOtrosGastos"]);
            else
                this.DiasOtrosGastos.Value = 0;
            if (!string.IsNullOrWhiteSpace(dr["ValorOtrosGastos"].ToString()))
                this.ValorOtrosGastos.Value = Convert.ToDecimal(dr["ValorOtrosGastos"]);
            else
                this.ValorOtrosGastos.Value = 0;
            if (!string.IsNullOrWhiteSpace(dr["ValorTiquetes"].ToString()))
                this.ValorTiquetes.Value = Convert.ToDecimal(dr["ValorTiquetes"]);
            else
                this.ValorTiquetes.Value = 0;
            this.AnticipoTipoID.Value = Convert.ToString(dr["AnticipoTipoID"]);
            this.AnticipoTipoDesc.Value = Convert.ToString(dr["AnticipoTipoDesc"]);
            this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
            this.Observacion.Value = Convert.ToString(dr["Observacion"]);
        }

       
         /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportAnticiposViaje()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Fecha = new UDTSQL_datetime();
            this.Area = new UDT_Descriptivo();
            this.TipoViaje = new UDTSQL_tinyint();
            this.Valor = new UDT_Valor();
            this.DocumentoTercero = new UDT_TerceroID();
            this.NombreTercero = new UDT_Descriptivo();
            this.DiasAlojamiento = new UDTSQL_tinyint();
            this.ValorAlojamiento = new UDT_Valor();
            this.DiasAlimentacion = new UDTSQL_tinyint();
            this.ValorAlimentacion = new UDT_Valor();
            this.DiasTransporte = new UDTSQL_tinyint();
            this.ValorTransporte = new UDT_Valor();
            this.DiasOtrosGastos = new UDTSQL_tinyint();
            this.ValorOtrosGastos = new UDT_Valor();
            this.ValorTiquetes = new UDT_Valor();
            this.AnticipoTipoID = new UDT_AnticipoTipoID();
            this.AnticipoTipoDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.Observacion = new UDT_DescripTExt();
        }
        #region Propiedades

        /// <summary>
        /// Moneda origen
        /// </summary>
        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }
        
        [DataMember]
        public UDT_Descriptivo Area { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoViaje { get; set; }
        
        [DataMember]
        public UDT_Valor Valor { get; set; }
        
        [DataMember]
        public UDT_TerceroID DocumentoTercero { get; set; }
        
        [DataMember]
        public UDT_Descriptivo NombreTercero { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint DiasAlojamiento { get; set; }
        
        [DataMember]
        public UDT_Valor ValorAlojamiento { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint DiasAlimentacion { get; set; }
        
        [DataMember]
        public UDT_Valor ValorAlimentacion { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint DiasTransporte { get; set; }
        
        [DataMember]
        public UDT_Valor ValorTransporte { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint DiasOtrosGastos { get; set; }
        
        [DataMember]
        public UDT_Valor ValorOtrosGastos { get; set; }

        [DataMember]
        public UDT_Valor ValorTiquetes { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Descriptivo AnticipoTipoDesc { get; set; }

        [DataMember]
        public UDT_AnticipoTipoID AnticipoTipoID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        #endregion
    }
}
