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
    public class DTO_noTraslado
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noTraslado(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.ContratoNOID.Value = Convert.ToInt32(dr["ContratoNOID"]);
                this.FechaTraslado.Value = Convert.ToDateTime(dr["Fecha"]);
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.OperacionNOID.Value = dr["OperacionNOID"].ToString();
                this.Descripcion.Value = dr["DescripTBase"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noTraslado()
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
            this.FechaTraslado = new UDTSQL_smalldatetime();
            this.EmpleadoID = new UDT_EmpleadoID();           
            this.OperacionNOID = new  UDT_OperacionNOID();
            this.Descripcion = new UDT_DescripTBase();
            this.NumeroDoc = new UDT_Consecutivo();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.ProyectoID = new UDT_ProyectoID();
        }
        #endregion
        
        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_ContratoNOID ContratoNOID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaTraslado { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_OperacionNOID OperacionNOID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }
        
    }
}
