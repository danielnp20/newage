using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Resultados;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Clase para serializar resultados de tipo Object
    /// </summary>
    [DataContract]
    [Serializable]

    [KnownType(typeof(DTO_Alarma))]
    [KnownType(typeof(DTO_CuentaValor))]
    [KnownType(typeof(DTO_PagoFacturas))]
    [KnownType(typeof(DTO_PlanDePagos))]
    [KnownType(typeof(DTO_TxResult))]
    [KnownType(typeof(DTO_BasicReport))]
    [KnownType(typeof(DTO_ccCreditoDocu))]
    [KnownType(typeof(DTO_ReintegrosCartera))]
    public class DTO_SerializedObject
    {
    }
}
