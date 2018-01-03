using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_cpAnticipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpAnticipo
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpAnticipo(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.RadicaFecha.Value = Convert.ToDateTime(dr["RadicaFecha"]);
                this.AnticipoTipoID.Value = dr["AnticipoTipoID"].ToString();
                this.Plazo.Value = Convert.ToByte(dr["Plazo"]);
                this.ConceptoCxPID.Value = dr["ConceptoCxPID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocCXP"].ToString()))
                    this.NumeroDocCXP.Value = Convert.ToInt32(dr["NumeroDocCXP"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaSalida"].ToString()))
                    this.FechaSalida.Value = Convert.ToDateTime(dr["FechaSalida"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaRetorno"].ToString()))
                    this.FechaRetorno.Value = Convert.ToDateTime(dr["FechaRetorno"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoViaje"].ToString()))
                    this.TipoViaje.Value = Convert.ToByte(dr["TipoViaje"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasAlojamiento"].ToString()))
                    this.DiasAlojamiento.Value = Convert.ToByte(dr["DiasAlojamiento"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["IVA"].ToString()))
                    this.IVA.Value = Convert.ToDecimal(dr["IVA"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorAlojamiento"].ToString()))
                    this.ValorAlojamiento.Value = Convert.ToDecimal(dr["ValorAlojamiento"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasAlimentacion"].ToString()))
                    this.DiasAlimentacion.Value = Convert.ToByte(dr["DiasAlimentacion"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorAlimentacion"].ToString()))
                    this.ValorAlimentacion.Value = Convert.ToDecimal(dr["ValorAlimentacion"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasTransporte"].ToString()))
                    this.DiasTransporte.Value = Convert.ToByte(dr["DiasTransporte"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorTransporte"].ToString()))
                    this.ValorTransporte.Value = Convert.ToDecimal(dr["ValorTransporte"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasOtrosGastos"].ToString()))
                    this.DiasOtrosGastos.Value = Convert.ToByte(dr["DiasOtrosGastos"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorOtrosGastos"].ToString()))
                    this.ValorOtrosGastos.Value = Convert.ToDecimal(dr["ValorOtrosGastos"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorTiquetes"].ToString()))
                    this.ValorTiquetes.Value = Convert.ToDecimal(dr["ValorTiquetes"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpAnticipo()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.RadicaFecha = new UDTSQL_smalldatetime();
            this.AnticipoTipoID = new UDT_AnticipoTipoID();
            this.Plazo = new UDTSQL_tinyint();
            this.ConceptoCxPID = new UDT_ConceptoCxPID();
            this.NumeroDocCXP = new UDT_Consecutivo();
            this.FechaSalida = new UDTSQL_smalldatetime();
            this.FechaRetorno = new UDTSQL_smalldatetime();
            this.TipoViaje = new UDTSQL_tinyint();
            this.DiasAlojamiento = new UDTSQL_tinyint();
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
            this.ValorAlojamiento = new UDT_Valor();
            this.DiasAlimentacion = new UDTSQL_tinyint();
            this.ValorAlimentacion = new UDT_Valor();
            this.DiasTransporte = new UDTSQL_tinyint();
            this.ValorTransporte = new UDT_Valor();
            this.DiasOtrosGastos = new UDTSQL_tinyint();
            this.ValorOtrosGastos = new UDT_Valor();
            this.ValorTiquetes = new UDT_Valor();
        }
        #endregion

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime RadicaFecha { get; set; }

        [DataMember]
        public UDT_AnticipoTipoID AnticipoTipoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint Plazo { get; set; }

        [DataMember]
        public UDT_ConceptoCxPID ConceptoCxPID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocCXP { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaSalida  { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRetorno { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoViaje  { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasAlojamiento { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        public UDT_Valor ValorAlojamiento { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasAlimentacion { get; set; }

        [DataMember]
        public UDT_Valor ValorAlimentacion { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasTransporte  { get; set; }

        [DataMember]
        public UDT_Valor ValorTransporte  { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasOtrosGastos { get; set; }

        [DataMember]
        public UDT_Valor ValorOtrosGastos { get; set; }

        [DataMember]
        public UDT_Valor ValorTiquetes { get; set; }

    }
}
