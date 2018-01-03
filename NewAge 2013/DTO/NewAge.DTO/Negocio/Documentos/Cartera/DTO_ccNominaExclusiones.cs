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
    /// Class comprobante para aprobacion:
    /// Models DTO_ccNominaExcluciones
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccNominaExclusiones
    {
        #region DTO_ccNominaExcluciones

        public DTO_ccNominaExclusiones(IDataReader dr)
        {
            InitCols();
            try
            {
                this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                this.FechaNomina.Value = Convert.ToDateTime(dr["FechaNomina"]);
                this.ValorNomina.Value = Convert.ToDecimal(dr["ValorNomina"]);
                this.Renglon.Value = Convert.ToInt32(dr["Renglon"]);
                this.Cedula.Value = dr["Cedula"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                this.Libranza.Value = dr["Libranza"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.Observacion.Value = dr["Observacion"].ToString();
                this.MensajeError.Value = dr["MensajeError"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccNominaExclusiones()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.PagaduriaID = new UDT_PagaduriaID();
            this.FechaNomina = new UDTSQL_smalldatetime();
            this.ValorNomina = new UDT_Valor();
            this.Renglon = new  UDTSQL_int();
            this.Cedula = new UDTSQL_varchar(20);
            this.Nombre = new UDTSQL_varchar(100);
            this.Libranza = new UDTSQL_varchar(20);
            this.Valor = new UDT_Valor();
            this.Observacion = new UDT_DescripTExt();
            this.MensajeError = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
        }
        
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNomina { get; set; }

        [DataMember]
        public UDT_Valor ValorNomina { get; set; }

        [DataMember]
        public UDTSQL_int Renglon { get; set; }

        [DataMember]
        public UDTSQL_varchar Cedula { get; set; }

        [DataMember]
        public UDTSQL_varchar Nombre { get; set; }

        [DataMember]
        public UDTSQL_varchar Libranza { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_DescripTExt MensajeError { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion
    }
}
