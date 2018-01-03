using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Reportes
{
    /// <summary>
    /// Clase del DocumentoContable
    /// </summary>
    public class DTO_ReportDocumentoContable : DTO_ReportComprobante2
    {
        #region Properties
        /// <summary>
        /// Footer del DocumentoContable
        /// </summary>
        public List<DTO_ReportDocumentoContableFooter> footerReport { get; set; }

        /// <summary>
        /// Descripcion del CentroCosto
        /// </summary>
        public string DescCentroCosto { get; set; }

        /// <summary>
        /// Descripcion del LugarGeografico
        /// </summary>
        public string DescLugarGeografico { get; set; }

        /// <summary>
        /// Descripcion del MonedaTransac
        /// </summary>
        public string DescMonedaTransac { get; set; }

        /// <summary>
        /// Descripcion del MonedaOrigen
        /// </summary>
        public string DescMonedaOrigen { get; set; }

        /// <summary>
        /// Descripcion del Documento
        /// </summary>
        public string DescDocumento { get; set; }

        /// <summary>
        /// Documento ID
        /// </summary>
        public string coDocumentoID { get; set; }

        /// <summary>
        /// CentroCosto ID
        /// </summary>
        public string CentroCostoID { get; set; }

        /// <summary>
        /// Cuenta ID
        /// </summary>
        public string CuentaID { get; set; }

        /// <summary>
        /// Documento Tercero
        /// </summary>
        public string DocumentoTercero { get; set; }

        /// <summary>
        /// LineaPresupuesto ID
        /// </summary>
        public string LineaPresupuestoID { get; set; }

        /// <summary>
        /// LugarGeografico ID
        /// </summary>
        public string LugarGeograficoID { get; set; }

        /// <summary>
        /// Observacion del documento
        /// </summary>
        public string Observacion { get; set; }

        /// <summary>
        /// Proyecto ID
        /// </summary>
        public string ProyectoID { get; set; }

        /// <summary>
        /// Tercero ID
        /// </summary>
        public string TerceroID { get; set; }   

        /// <summary>
        /// Valor del Documento
        /// </summary>
        public decimal ValorDoc { get; set; }

        /// <summary>
        /// Numero del Documento
        /// </summary>
        public string DocumentoNro { get; set; }

        /// <summary>
        /// Estado del Documento
        /// </summary>
        public string DocumentoEstado { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        public string UsuarioResp { get; set; }

        /// <summary>
        ///  Documento Ajustado ID 
        /// </summary>
        public string DocAjustadoID { get; set; }

        /// <summary>
        /// Documento Ajustado Descriptivo
        /// </summary>
        public string DocAjustadoDesc { get; set; }
        #endregion
    }       

        #region Constructors
    public class DTO_ReportDocumentoContableFooter : DTO_ComprobanteFooter
    {
        public DTO_ReportDocumentoContableFooter(IDataReader dr)
            : base(dr)
        {
            short s = Convert.ToInt16(dr["Naturaleza"]);
            if (s == (short)NaturalezaCuenta.Debito)
                Debito = true;
            else
                Debito = false;
        }

        public DTO_ReportDocumentoContableFooter(DTO_ComprobanteFooter parent)
            : base(parent)
        {
        }

        public DTO_ReportDocumentoContableFooter()
            : base()
        {
        }

        [Filtrable]
        public bool Debito
        {
            get;
            set;
        }        
    }
    #endregion
}
