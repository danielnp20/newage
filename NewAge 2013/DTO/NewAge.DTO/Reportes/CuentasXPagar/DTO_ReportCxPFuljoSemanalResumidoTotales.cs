﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ReportCxPFuljoSemanalResumidoTotales
    {
        #region Propiades
        [DataMember]
        public List<DTO_ReportCxPFlujoSemanalResumido> Detalles { get; set; }

        [DataMember]
        public DateTime FechaIni { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }
        #endregion

    }
}















