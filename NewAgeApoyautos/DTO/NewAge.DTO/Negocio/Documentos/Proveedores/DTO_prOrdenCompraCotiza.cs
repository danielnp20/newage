using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class recibidos de bienes y servicios:
    /// Models DTO_prOrdenCompraCotiza
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prOrdenCompraCotiza
    {
        #region DTO_prSolicitudCargos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prOrdenCompraCotiza(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.FechaCotiza.Value = Convert.ToDateTime(dr["FechaCotiza"]);
                this.Observacion.Value = dr["Observacion"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.Direccion.Value = dr["Direccion"].ToString();
                this.Telefono.Value = dr["Telefono"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prOrdenCompraCotiza()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDTSQL_char(50);
            this.NumeroDoc = new UDT_Consecutivo();
            this.FechaCotiza = new  UDTSQL_datetime();
            this.Observacion = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
            this.Direccion = new UDTSQL_char(50);
            this.Telefono = new UDTSQL_char(20);
        }
        #endregion

        #region Propiedades
       
        [DataMember]
        public UDTSQL_char EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaCotiza { get; set; }        

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDTSQL_char Direccion { get; set; }

        [DataMember]
        public UDTSQL_char Telefono { get; set; }

        #endregion
    }
}

