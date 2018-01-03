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
    /// Models DTO_prRecibidoDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prRecibidoDocu
    {
        #region DTO_prRecibidoDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prRecibidoDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                //this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.LugarEntrega.Value = dr["LugarEntrega"].ToString();
                this.BodegaID.Value = dr["BodegaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ConformidadInd"].ToString()))
                    this.ConformidadInd.Value = Convert.ToBoolean(dr["ConformidadInd"]);
                if (!string.IsNullOrWhiteSpace(dr["Calificacion"].ToString()))
                    this.Calificacion.Value = Convert.ToByte(dr["Calificacion"]);
                this.ObsRechazo.Value = dr["ObsRechazo"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prRecibidoDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.LugarEntrega = new UDT_LocFisicaID();
            this.BodegaID = new UDT_BodegaID();
            this.ConformidadInd = new UDT_SiNo();
            this.Calificacion = new UDTSQL_tinyint();
            this.ObsRechazo = new UDT_DescripTExt();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        //[DataMember]
        //public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_LocFisicaID LugarEntrega { get; set; }

        [DataMember]
        public UDT_BodegaID BodegaID { get; set; }

        [DataMember]
        public UDT_SiNo ConformidadInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint Calificacion { get; set; }

        [DataMember]
        public UDT_DescripTExt ObsRechazo { get; set; }
    
        #endregion
    }
}
