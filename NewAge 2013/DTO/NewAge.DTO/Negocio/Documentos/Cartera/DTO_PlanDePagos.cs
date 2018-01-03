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
    /// Models DTO_LiquidacionCartera
    [Serializable]
    [DataContract]
    public class DTO_PlanDePagos : DTO_SerializedObject
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_PlanDePagos()
        {
            this.CompradorCarteraID = new UDT_BasicID();
            this.ComponentesUsuario = new List<DTO_ccSolicitudComponentes>();
            this.ComponentesFijos = new Dictionary<string, bool>();
            this.ComponentesAll = new List<DTO_ccSolicitudComponentes>();
            this.Cuotas = new List<DTO_Cuota>();
            this.Tasas = new Dictionary<string, decimal>();
            this.VlrPoliza = 0;

            //Propiedades de operación
            this.CuotasCredito = new List<DTO_Cuota>();
            this.CuotasPoliza = new List<DTO_Cuota>();
            this.CuotasExtras = new List<DTO_Cuota>();
        }

        #region Propiedades

        [DataMember]
        public List<DTO_ccSolicitudComponentes> ComponentesUsuario
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccSolicitudComponentes> ComponentesAll
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_Cuota> Cuotas
        {
            get;
            set;
        }

        [DataMember]
        public Dictionary<string, decimal> Tasas
        {
            get;
            set;
        }

        [DataMember]
        public Dictionary<string, bool> ComponentesFijos
        {
            get;
            set;
        }

        [DataMember]
        public int VlrAdicional
        {
            get;
            set;
        }

        [DataMember]
        public int VlrCompra
        {
            get;
            set;
        }

        [DataMember]
        public int VlrDescuento
        {
            get;
            set;
        }

        [DataMember]
        public int VlrGiro
        {
            get;
            set;
        }

        [DataMember]
        public int VlrLibranza
        {
            get;
            set;
        }

        [DataMember]
        public int VlrPoliza
        {
            get;
            set;
        }

        [DataMember]
        public int VlrCuotaPoliza
        {
            get;
            set;
        }

        [DataMember]
        public int VlrCuota
        {
            get;
            set;
        }

        [DataMember]
        public int VlrPrestamo
        {
            get;
            set;
        }

        [DataMember]
        public int VlrPrestamoPoliza
        {
            get;
            set;
        }

        [DataMember]
        public decimal TasaTotal
        {
            get;
            set;
        }

        [DataMember]
        public UDT_BasicID CompradorCarteraID { get; set; }

        #endregion

        #region Propiedades de operación

        [DataMember]
        public List<DTO_Cuota> CuotasCredito
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_Cuota> CuotasPoliza
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_Cuota> CuotasExtras
        {
            get;
            set;
        }

        #endregion

    }
}
