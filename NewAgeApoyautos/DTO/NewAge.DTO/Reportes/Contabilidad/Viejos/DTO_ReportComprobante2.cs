using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Attributes;
using System.Data;
using System.Reflection;

namespace NewAge.DTO.Reportes
{
    /// <summary>
    /// Clase del reporte Comprobante
    /// </summary>
    #region Properties
        
    public class DTO_ReportComprobante2 : DTO_Comprobante
    {
        /// <summary>
        /// Comprobante Footer
        /// </summary>
        public List<DTO_ReportComprobanteFooter> footerReport
        {
            get;
            set;
        }

        /// <summary>
        /// Descripcion del Tercero
        /// </summary>
        public string DescTercero
        {
            get;
            set;
        }

        /// <summary>
        /// Descripcion de la Cuenta
        /// </summary>    
        public string DescCuenta
        {
            get;
            set;
        }

        /// <summary>
        /// Descripcion del Proyecto
        /// </summary>
        public string DescProyecto
        {
            get;
            set;
        }

        /// <summary>
        /// Descripcion de la Linea Presupuestal
        /// </summary>
        public string DescLineaPresupuestal
        {
            get;
            set;
        }

        /// <summary>
        /// Descripcion del ConceptoCargo
        /// </summary>
        public string DescConceptoCargo
        {
            get;
            set;
        }

        /// <summary>
        /// Estado del comprobante
        /// </summary>
        public string Estado
        {
            get;
            set;
        }
    }
    #endregion

    #region Constructor
    public class DTO_ReportComprobanteFooter : DTO_ComprobanteFooter
    {
        /// <summary>
        /// Constructor getting data from the datareader
        /// </summary>
        public DTO_ReportComprobanteFooter(IDataReader dr)
            : base(dr)
        {
            short s = Convert.ToInt16(dr["Naturaleza"]);
            if (s == (short)NaturalezaCuenta.Debito)
                Debito = true;
            else
                Debito = false;
        }

        /// <summary>
        /// Constructor getting data from the parent
        /// </summary>
        public DTO_ReportComprobanteFooter(DTO_ComprobanteFooter parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportComprobanteFooter()
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
