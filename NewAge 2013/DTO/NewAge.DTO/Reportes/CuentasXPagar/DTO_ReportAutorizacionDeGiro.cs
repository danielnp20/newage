using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del documento Autorizacion del Giro (Causacion Factura)
    /// </summary>
    public class DTO_ReportAutorizacionDeGiro : DTO_BasicReport
    {
        #region Propiedades
        /// <summary>
        /// ID de la Empresa
        /// </summary>
        [DataMember]
        public string EmpresaID { get; set; }

        /// <summary>
        /// Indicador del Estado del documento (sin aprobar - true)
        /// </summary>
        [DataMember]
        public bool EstadoInd { get; set; }

        /// <summary>
        /// Tercero ID (del glDocumentoControl)
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Descripcion del Tercero (A favor de..)
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

        /// <summary>
        /// Factura No.
        /// </summary>
        [DataMember]
        public string DocumentoTercero { get; set; }

        /// <summary>
        /// Fecha de la Factura
        /// </summary>
        [DataMember]
        public DateTime FechaFact { get; set; }

        /// <summary>
        /// Fecha del glDocumentoControl
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Fecha VTO
        /// </summary>
        [DataMember]
        public DateTime FechaVto { get; set; }

        /// <summary>
        /// Observacion del glDocumentoControl
        /// </summary>
        [DataMember]
        public string Descripcion { get; set; }

        /// <summary>
        /// ComprobanteID del glDocumentoControl
        /// </summary>
        [DataMember]
        public string ComprobanteID { get; set; }

        /// <summary>
        /// ComprobanteNro del glDocumentoControl
        /// </summary>
        [DataMember]
        public string ComprobanteNro { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [DataMember]
        public string UsuarioResp { get; set; }

        /// <summary>
        /// Tasa Cambio CONT del glDocumentoControl
        /// </summary>
        [DataMember]
        public decimal TasaCambio { get; set; }

        /// <summary>
        /// Documento ID
        /// </summary>
        [DataMember]
        public string DocumentoID { get; set; }

        #region Valores (Moneda local)
        /// <summary>
        /// Vr.Bruto de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal VrBrutoML { get; set; }

        /// <summary>
        /// IVA de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal IvaML { get; set; }

        /// <summary>
        /// ReteIVA de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal ReteIvaML { get; set; }

        /// <summary>
        /// ReteFuente de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal ReteFuenteML { get; set; }

        /// <summary>
        /// ReteIca de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal ReteIcaML { get; set; }

        /// <summary>
        /// Timbre de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal TimbreML { get; set; }

        /// <summary>
        /// Anticipos de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal AnticiposML { get; set; }

        /// <summary>
        /// OtrosDtos de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal OtrosDtosML { get; set; }

        /// <summary>
        /// ValorNeto de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal VrNetoML{ get; set; }
        #endregion 
        
        #region Valores (Moneda Extranjera)
        /// <summary>
        /// Vr.Bruto de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal VrBrutoME { get; set; }

        /// <summary>
        /// IVA de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal IvaME { get; set; }

        /// <summary>
        /// ReteIVA de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal ReteIvaME { get; set; }

        /// <summary>
        /// ReteFuente de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal ReteFuenteME { get; set; }

        /// <summary>
        /// ReteIca de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal ReteIcaME { get; set; }

        /// <summary>
        /// Timbre de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal TimbreME { get; set; }

        /// <summary>
        /// Anticipos de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal AnticiposME { get; set; }

        /// <summary>
        /// OtrosDtos de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal OtrosDtosME { get; set; }

        /// <summary>
        /// ValorNeto de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal VrNetoME { get; set; }
        #endregion 

        /// <summary>
        /// Detalle de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public List<DTO_AutorizacionDetail> AutorizacionDetail { get; set; }
        #endregion
    }
    /// <summary>
    /// Clase del detalle de la factura (heredado de DTO_ComprobanteFooter)
    /// </summary>
    public class DTO_AutorizacionDetail
    {
        #region Properties
        /// <summary>
        /// CuentaID
        /// </summary>
        [DataMember]
        public string CuentaID { get; set; }

        /// <summary>
        /// CentroCostoID
        /// </summary>
        [DataMember]
        public string CentroCostoID { get; set; }

        /// <summary>
        /// ProyectoID
        /// </summary>
        [DataMember]
        public string ProyectoID { get; set; }

        /// <summary>
        /// LineaPresupuestoID
        /// </summary>
        [DataMember]
        public string LineaPresupuestoID { get; set; }

        /// <summary>
        /// Descriptivo de la Cuenta
        /// </summary>
        [DataMember]
        public string CuentaDesc { get; set; }

        /// <summary>
        /// Tipo del Impuesto (Retefuente,Ica,Iva etc.)
        /// </summary>
        [DataMember]
        public string ImpuestoTipoID { get; set; }

        #region Valores (Moneda local)
        /// <summary>
        /// Valor Base de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal BaseML { get; set; }

        /// <summary>
        /// Valor de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal ValorML { get; set; }

        ///// <summary>
        ///// Valor de la factura (Moneda local)
        ///// </summary>
        //[DataMember]
        //public decimal ValorML_SinSigno { get; set; }

        /// <summary>
        /// Vr.Bruto de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal VrBrutoML { get; set; }

        /// <summary>
        /// IVA de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal IvaML { get; set; }

        /// <summary>
        /// ReteIVA de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal ReteIvaML { get; set; }

        /// <summary>
        /// ReteFuente de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal ReteFuenteML { get; set; }

        /// <summary>
        /// ReteIca de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal ReteIcaML { get; set; }

        /// <summary>
        /// Timbre de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal TimbreML { get; set; }

        /// <summary>
        /// Anticipos de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal AnticiposML { get; set; }

        /// <summary>
        /// OtrosDtos de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal OtrosDtosML { get; set; }

        /// <summary>
        /// ValorNeto de la factura (Moneda local)
        /// </summary>
        [DataMember]
        public decimal VrNetoML { get; set; }
        #endregion

        #region Valores (Moneda Extranjera)
        /// <summary>
        /// Valor de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal ValorME { get; set; }

        ///// <summary>
        ///// Valor de la factura (Moneda extranjera)
        ///// </summary>
        //[DataMember]
        //public decimal ValorME_SinSigno { get; set; }

        /// <summary>
        /// Vr.Bruto de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal VrBrutoME { get; set; }

        /// <summary>
        /// IVA de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal IvaME { get; set; }

        /// <summary>
        /// ReteIVA de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal ReteIvaME { get; set; }

        /// <summary>
        /// ReteFuente de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal ReteFuenteME { get; set; }

        /// <summary>
        /// ReteIca de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal ReteIcaME { get; set; }

        /// <summary>
        /// Timbre de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal TimbreME { get; set; }

        /// <summary>
        /// Anticipos de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal AnticiposME { get; set; }

        /// <summary>
        /// OtrosDtos de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal OtrosDtosME { get; set; }

        /// <summary>
        /// ValorNeto de la factura (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal VrNetoME { get; set; }
        #endregion 

        /// <summary>
        /// (Valor/Base)*100%
        /// </summary>
        [DataMember]
        public string Percent { get; set; }
        #endregion
    }

}
