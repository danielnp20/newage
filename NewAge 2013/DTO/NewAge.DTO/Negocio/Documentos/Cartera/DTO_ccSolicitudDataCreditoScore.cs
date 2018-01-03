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
    /// Models DTO_ccSolicitudDataCreditoScore
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDataCreditoScore
    {
        #region ccSolicitudDataCreditoScore

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDataCreditoScore(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Version.Value = Convert.ToInt32(dr["Version"]);
                this.TipoId.Value = Convert.ToString(dr["TipoId"]);
                this.NumeroId.Value = Convert.ToString(dr["NumeroId"]);
                this.Nombre.Value = Convert.ToString(dr["Nombre"]);
                this.Puntaje.Value = Convert.ToString(dr["Puntaje"]);
                this.Razon1.Value = Convert.ToString(dr["Razon1"]);
                this.Razon2.Value = Convert.ToString(dr["Razon2"]);
                this.Razon3.Value = Convert.ToString(dr["Razon3"]);
                this.Razon4.Value = Convert.ToString(dr["Razon4"]);
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
        public DTO_ccSolicitudDataCreditoScore()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Version = new UDTSQL_int();
            this.TipoId=new UDTSQL_varchar(1);
            this.NumeroId=new UDTSQL_varchar(11);           
            this.Nombre=new UDTSQL_varchar(50);
            this.Puntaje = new UDTSQL_varchar(5);
            this.Razon1= new UDTSQL_varchar(2);
            this.Razon2 = new UDTSQL_varchar(2);
            this.Razon3 = new UDTSQL_varchar(2);
            this.Razon4 = new UDTSQL_varchar(2);            
            this.Consecutivo=new UDT_Consecutivo();
          }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        [DataMember]
        public UDTSQL_int Version{ get; set; }

        [DataMember]
        public UDTSQL_varchar TipoId{ get; set; }

        [DataMember]
        public UDTSQL_varchar NumeroId{ get; set;}         
        
        [DataMember]
        public UDTSQL_varchar Nombre{ get; set; }

        [DataMember]
        public UDTSQL_varchar Puntaje { get; set; } 
        
        [DataMember]
        public UDTSQL_varchar Razon1{ get; set;}

        [DataMember]
        public UDTSQL_varchar Razon2 { get; set; }

        [DataMember]
        public UDTSQL_varchar Razon3 { get; set; }

        [DataMember]
        public UDTSQL_varchar Razon4 { get; set; } 
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        #endregion
    }
}
