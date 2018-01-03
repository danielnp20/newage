using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_Credito
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_Credito
    {
        public DTO_Credito()
        {
            this.CreditoDocu = new DTO_ccCreditoDocu();
            this.DocControl = new DTO_glDocumentoControl();
            this.PlanPagos = new List<DTO_ccCreditoPlanPagos>();
            this.Cuotas = new List<DTO_Cuota>();
            this.Componentes = new List<DTO_ccCreditoComponentes>();
            this.CompraCartera = new List<DTO_ccSolicitudCompraCartera>();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(DTO_ccCreditoDocu cd, List<DTO_ccCreditoPlanPagos> pp)
        {
            this.CreditoDocu = cd;
            this.PlanPagos = pp;
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(DTO_ccCreditoDocu cd, DTO_glDocumentoControl ctrl, List<DTO_ccCreditoPlanPagos> pp)
        {
            this.CreditoDocu = cd;
            this.DocControl = ctrl;
            this.PlanPagos = pp;
        }

        #region Propiedades

        [DataMember]
        public DTO_ccCreditoDocu CreditoDocu
        {
            get;
            set;
        }

        [DataMember]
        public DTO_glDocumentoControl DocControl
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccCreditoPlanPagos> PlanPagos
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
        public List<DTO_ccCreditoComponentes> Componentes
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccSolicitudCompraCartera> CompraCartera
        {
            get;
            set;
        }

        #endregion
    }
}
