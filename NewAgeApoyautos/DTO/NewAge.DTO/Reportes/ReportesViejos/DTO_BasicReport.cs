using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using NewAge.DTO.Reportes;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    [KnownType(typeof(DTO_repBitacora))]
    [KnownType(typeof(DTO_ReportBalanceDePrueba))]
    [KnownType(typeof(DTO_ReportBalanceDePruebaPorMeses))]
    [KnownType(typeof(DTO_ReportBalanceDePruebaPorQ))]
    [KnownType(typeof(DTO_ReportComprobante))]
    [KnownType(typeof(DTO_ReportLibroMayor))]
    [KnownType(typeof(DTO_ReportSaldos1))]
    [KnownType(typeof(DTO_Formularios))]
    [KnownType(typeof(DTO_FormulariosDetail))]
    [KnownType(typeof(DTO_FormulariosCuenta))]
    [KnownType(typeof(DTO_ReportInventariosBalance))]
    [KnownType(typeof(DTO_ReportRelacionDocumentos))]
    [KnownType(typeof(DTO_ReportSaldosDocumentos))]
    [KnownType(typeof(DTO_ReportAnticipo))]
    [KnownType(typeof(DTO_ReportAnticipoViaje))]
    [KnownType(typeof(DTO_ReportAutorizacionDeGiro))]
    [KnownType(typeof(DTO_ReportReciboCaja))]
    [KnownType(typeof(DTO_ReportBalanceDePruebaComparativo))]
    [KnownType(typeof(DTO_ReportFacturasPorPagar))]
    [KnownType(typeof(DTO_CertificatesDetailedReport))]
    [KnownType(typeof(DTO_ReportReciboCaja))]
    [KnownType(typeof(DTO_ReportProgramacionPagos))]
    [KnownType(typeof(DTO_ReportCajaMenor))]
    [KnownType(typeof(DTO_ReportPagoFacturas))]
    [KnownType(typeof(DTO_ReportLiquidacionCredito2))]
    [KnownType(typeof(DTO_ReportInventariosSaldos))]
    [KnownType(typeof(DTO_ReportProveedoresSolicitudes))]

    // <summary>
    // Clase generico para todos los reportes
    // </summary>
    public abstract class DTO_BasicReport : DTO_SerializedObject
    {
        /// <summary>
        /// Rompimiento del reporte - nivel 1
        /// </summary>
        public Rompimiento ReportRompimiento1
        {
            get;
            set;
        }

        /// <summary>
        /// Rompimiento del reporte - nivel 2
        /// </summary>
        public Rompimiento ReportRompimiento2
        {
            get;
            set;
        }

        /// <summary>
        /// Rompimiento del reporte - nivel 3
        /// </summary>
        public Rompimiento ReportRompimiento3
        {
            get;
            set;
        }

        /// <summary>
        /// Rompimiento del reporte - nivel 4
        /// </summary>
        public Rompimiento ReportRompimiento4
        {
            get;
            set;
        }
     

        /// <summary>
        /// Llena los rompimientos dados los nombres de las propiedades desde las cuales sacarlos
        /// </summary>
        /// <param name="reportDtos">datos (para el reporte) a llenar</param>
        /// <param name="property1">propiedad para llenar el primer rompimiento</param>
        /// <param name="property2">propiedad para llenar el segundo rompimiento</param>
        /// <param name="property3">propiedad para llenar el tercer rompimiento</param>
        /// <param name="property4">propiedad para llenar el cuarto rompimiento</param>
        public static void FillRompimiento(List<DTO_BasicReport> reportDtos, string property1, string property2="", string property3="", string property4=""){
            try
            {
                if (reportDtos == null || reportDtos.Count == 0)
                    return;
                PropertyInfo pi1 = reportDtos.First().GetType().GetProperty(property1);
                PropertyInfo pi2 = reportDtos.First().GetType().GetProperty(property2);
                PropertyInfo pi3 = reportDtos.First().GetType().GetProperty(property3);
                PropertyInfo pi4 = reportDtos.First().GetType().GetProperty(property4);
                if (pi1 == null)
                    return;
                foreach (DTO_BasicReport r in reportDtos)
                {
                    if (pi1 != null)
                    {
                        r.ReportRompimiento1 = new Rompimiento();
                        r.ReportRompimiento1.GroupFieldValue = pi1.GetValue(r, null).ToString().TrimEnd();
                        r.ReportRompimiento1.GroupFieldName = property1;
                        r.ReportRompimiento1.GroupFieldDesc = string.Empty;
                        if (property1.Contains("ID"))
                        {
                            PropertyInfo pi1_desc = reportDtos.First().GetType().GetProperty(property1.Replace("ID", "Desc"));
                            if (pi1_desc != null)
                                r.ReportRompimiento1.GroupFieldDesc = pi1_desc.GetValue(r, null).ToString();
                        };
                    }
                    else
                    {
                        r.ReportRompimiento1 = new Rompimiento();
                        r.ReportRompimiento1.GroupFieldValue = string.Empty;
                        r.ReportRompimiento1.GroupFieldName = string.Empty;
                        r.ReportRompimiento1.GroupFieldDesc = string.Empty;
                    };

                    if (pi2 != null)
                    {
                        r.ReportRompimiento2 = new Rompimiento();
                        r.ReportRompimiento2.GroupFieldValue = pi2.GetValue(r, null).ToString();
                        r.ReportRompimiento2.GroupFieldName = property2;
                        r.ReportRompimiento2.GroupFieldDesc = string.Empty;
                        if (property2.Contains("ID"))
                        {
                            PropertyInfo pi2_desc = reportDtos.First().GetType().GetProperty(property2.Replace("ID", "Desc"));
                            if (pi2_desc != null)
                                r.ReportRompimiento2.GroupFieldDesc = pi2_desc.GetValue(r, null).ToString();
                        };
                    }
                    else
                    {
                        r.ReportRompimiento2 = new Rompimiento();
                        r.ReportRompimiento2.GroupFieldValue = string.Empty;
                        r.ReportRompimiento2.GroupFieldName = string.Empty;
                        r.ReportRompimiento2.GroupFieldDesc = string.Empty;
                    };

                    if (pi3 != null)
                    {
                        r.ReportRompimiento3 = new Rompimiento();
                        r.ReportRompimiento3.GroupFieldValue = pi3.GetValue(r, null).ToString();
                        r.ReportRompimiento3.GroupFieldName = property3;
                        r.ReportRompimiento3.GroupFieldDesc = string.Empty;
                        if (property4.Contains("ID"))
                        {
                            PropertyInfo pi3_desc = reportDtos.First().GetType().GetProperty(property3.Replace("ID", "Desc"));
                            if (pi3_desc != null)
                                r.ReportRompimiento3.GroupFieldDesc = pi3_desc.GetValue(r, null).ToString();
                        };
                    }
                    else
                    {
                        r.ReportRompimiento3 = new Rompimiento();
                        r.ReportRompimiento3.GroupFieldValue = string.Empty;
                        r.ReportRompimiento3.GroupFieldName = string.Empty;
                        r.ReportRompimiento3.GroupFieldDesc = string.Empty;
                    };

                    if (pi4 != null)
                    {
                        r.ReportRompimiento4 = new Rompimiento();
                        r.ReportRompimiento4.GroupFieldValue = pi4.GetValue(r, null).ToString();
                        r.ReportRompimiento4.GroupFieldName = property4;
                        r.ReportRompimiento4.GroupFieldDesc = string.Empty;
                        if (property4.Contains("ID"))
                        {
                            PropertyInfo pi4_desc = reportDtos.First().GetType().GetProperty(property4.Replace("ID", "Desc"));
                            if (pi4_desc != null)
                                r.ReportRompimiento4.GroupFieldDesc = pi4_desc.GetValue(r, null).ToString();
                        };
                    }
                    else
                    {
                        r.ReportRompimiento4 = new Rompimiento();
                        r.ReportRompimiento4.GroupFieldValue = string.Empty;
                        r.ReportRompimiento4.GroupFieldName = string.Empty;
                        r.ReportRompimiento4.GroupFieldDesc = string.Empty;
                    };
                }
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        [Serializable]
        public class Rompimiento
        {
            /// <summary>
            /// el nombre del tipo del rompimineto por el cual se realiza
            /// </summary>
            public string GroupFieldName { get; set; }

            /// <summary>
            /// el valor del tipo del rompimineto por el cual se realiza
            /// </summary>
            public string GroupFieldValue { get; set; }

            /// <summary>
            /// la descripcion del tipo del rompimineto por el cual se realiza
            /// </summary>
            public string GroupFieldDesc { get; set; }
        }
        
    }
}
