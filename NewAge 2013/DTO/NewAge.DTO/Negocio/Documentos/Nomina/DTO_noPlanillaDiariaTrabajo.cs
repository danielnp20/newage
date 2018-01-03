using System;
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
    public class DTO_noPlanillaDiariaTrabajo
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noPlanillaDiariaTrabajo(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.ContratoNOID.Value = Convert.ToInt32(dr["ContratoNOID"]);
                this.FechaPlanilla.Value = Convert.ToDateTime(dr["Fecha"]);
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.ConceptoNOPlanillaID.Value = dr["ConceptoNOPlanillaID"].ToString();
                this.TipoConceptoPlanilla.Value = Convert.ToByte(dr["Tipo"].ToString());
                this.HorasNORDiu.Value = Convert.ToByte(dr["HorasNORDiu"].ToString());
                this.HorasEXTDiu.Value = Convert.ToByte(dr["HorasEXTDiu"].ToString());
                this.HorasEXTNoc.Value = Convert.ToByte(dr["HorasEXTNoc"].ToString());
                this.HorasRECNoc.Value = Convert.ToByte(dr["HorasRECNoc"].ToString());
                if (!string.IsNullOrEmpty("Identificador"))  
                    this.Identificador.Value = Convert.ToInt32(dr["Identificador"]); 
            }
            catch (Exception e)
            {
                throw e;
            }
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noPlanillaDiariaTrabajo()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.ContratoNOID = new UDT_ContratoNOID();
            this.FechaPlanilla = new UDTSQL_smalldatetime();
            this.EmpleadoID = new UDT_EmpleadoID();           
            this.ConceptoNOPlanillaID = new  UDT_ConceptoNOPlanillaID();
            this.TipoConceptoPlanilla = new UDTSQL_tinyint();
            this.HorasNORDiu = new UDTSQL_tinyint();
            this.HorasEXTDiu = new UDTSQL_tinyint();
            this.HorasEXTNoc = new UDTSQL_tinyint();
            this.HorasRECNoc = new UDTSQL_tinyint();
            this.Identificador = new UDT_Consecutivo();
      
        }
        #endregion
        
        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_ContratoNOID ContratoNOID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPlanilla { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_ConceptoNOPlanillaID ConceptoNOPlanillaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint HorasNORDiu { get; set; }
                
        [DataMember]
        public UDTSQL_tinyint HorasEXTDiu { get; set; }
                
        [DataMember]
        public UDTSQL_tinyint HorasEXTNoc { get; set; }
                
        [DataMember]
        public UDTSQL_tinyint HorasRECNoc { get; set; }

        [DataMember]
        public UDT_Consecutivo Identificador { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoConceptoPlanilla { get; set; }
                  
    }
}
