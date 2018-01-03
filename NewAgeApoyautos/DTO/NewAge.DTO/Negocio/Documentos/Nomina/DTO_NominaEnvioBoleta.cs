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
    /// Class Nomina para aprobacion:
    /// Models DTO_NominaEnvioBoleta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_NominaEnvioBoleta
    {
        #region DTO_NominaEnvioBoleta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_NominaEnvioBoleta(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                this.Nombre.Value = Convert.ToString(dr["Nombre"]);
                this.CorreoElectronico.Value = Convert.ToString(dr["CorreoElectronico"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.Seleccionar.Value = false;
            }
            catch (Exception e)
            {
                throw e;
            }          
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaEnvioBoleta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Seleccionar = new UDT_SiNo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.PeriodoID = new UDT_PeriodoID();
            this.TerceroID = new UDT_TerceroID();
            this.Nombre = new UDT_Descriptivo();
            this.CorreoElectronico = new UDTSQL_char(60);
            this.Valor = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; } 

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_char CorreoElectronico { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        #endregion
    }
}
