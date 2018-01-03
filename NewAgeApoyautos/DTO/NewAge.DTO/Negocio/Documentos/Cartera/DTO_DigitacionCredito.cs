using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_DigitacionCredito
    [Serializable]
    [DataContract]
    public class DTO_DigitacionCredito
    /// </summary>
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_DigitacionCredito()
        {
            this.Header = new DTO_ccSolicitudDocu();
            this.PlanPagos = new DTO_PlanDePagos();
            this.DocCtrl = new DTO_glDocumentoControl();
            this.Poliza = new DTO_ccPolizaEstado();
            this.Componentes = new List<DTO_ccSolicitudComponentes>();
            this.CompraCartera = new List<DTO_ccSolicitudCompraCartera>();
            this.DetaPagos = new List<DTO_ccSolicitudDetallePago>();
        }

        /// <summary>
        /// Crea el DTO con todos sus atributos
        /// </summary>
        /// <param name="dc">glDocumento Control del credito</param>
        /// <param name="h">Informaicon del credito</param>
        /// <param name="f1">Plan de pagos del credito</param>
        /// <param name="f2">Componentes del credito</param>
        /// <param name="f3">Compras asociadas a ese credito</param>
        public void AddData(DTO_glDocumentoControl dc, DTO_ccSolicitudDocu h, DTO_PlanDePagos f1, List<DTO_ccSolicitudComponentes> f2, List<DTO_ccSolicitudCompraCartera> f3)
        {
            this.DocCtrl = dc;
            this.Header = h;
            this.PlanPagos = f1;
            this.Componentes = f2;
            this.CompraCartera = f3;
        }

        /// <summary>
        /// Crea el DTO con todos sus atributos
        /// </summary>
        /// <param name="dc">glDocumento Control del credito</param>
        /// <param name="h">Informaicon del credito</param>
        /// <param name="f1">Plan de pagos del credito</param>
        /// <param name="f2">Componentes del credito</param>
        /// <param name="f3">Compras asociadas a ese credito</param>
        /// <param name="f4">Detalles de los beneficiarios asociados al credito</param>
        public void AddData(DTO_glDocumentoControl dc, DTO_ccSolicitudDocu h, DTO_ccPolizaEstado pol, DTO_PlanDePagos f1,
            List<DTO_ccSolicitudComponentes> f2, List<DTO_ccSolicitudCompraCartera> f3, List<DTO_ccSolicitudDetallePago> f4)
        {
            this.DocCtrl = dc;
            this.Header = h;
            this.Poliza = pol;
            this.PlanPagos = f1;
            this.Componentes = f2;
            this.CompraCartera = f3;
            this.DetaPagos = f4;
        }

        #region Propiedades

        [DataMember]
        public DTO_glDocumentoControl DocCtrl
        {
            get;
            set;
        }

        [DataMember]
        public DTO_ccSolicitudDocu Header
        {
            get;
            set;
        }

        [DataMember]
        public DTO_PlanDePagos PlanPagos
        {
            get;
            set;
        }

        [DataMember]
        public DTO_ccPolizaEstado Poliza
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccSolicitudComponentes> Componentes
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

        [DataMember]
        public List<DTO_ccSolicitudDetallePago> DetaPagos
        {
            get;
            set;
        }

        #endregion
    }
}