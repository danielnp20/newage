using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_inBodegaRefe
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inBodegaRefe : DTO_MasterComplex
    {
        #region DTO_inBodegaRefe

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inBodegaRefe(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.BodegaDesc.Value = dr["BodegaDesc"].ToString();
                    this.inReferenciaDesc.Value = dr["inReferenciaDesc"].ToString();
                }

                this.BodegaID.Value = dr["BodegaID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.Minimo.Value = Convert.ToDecimal(dr["Minimo"]);
                this.Maximo.Value = Convert.ToDecimal( dr["Maximo"]);
                this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                if (!string.IsNullOrWhiteSpace(dr["UbicacionID"].ToString()))
                    this.UbicacionID.Value = (dr["UbicacionID"].ToString());
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inBodegaRefe() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.BodegaID = new UDT_BasicID();
            this.BodegaDesc = new UDT_Descriptivo();    
            this.inReferenciaID = new UDT_BasicID();
            this.inReferenciaDesc = new UDT_Descriptivo();
            this.Minimo = new UDT_Cantidad();
            this.Maximo = new UDT_Cantidad();  
            this.UbicacionID = new UDT_UbicacionID();
            this.EstadoInv = new UDTSQL_tinyint();
        }

        public DTO_inBodegaRefe(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_inBodegaRefe(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID BodegaID { get; set; }

        [DataMember]
        public UDT_Descriptivo BodegaDesc { get; set; }
      
        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo inReferenciaDesc { get; set; }

        [DataMember]
        public UDT_Cantidad Minimo { get; set; }

        [DataMember]
        public UDT_Cantidad Maximo { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_UbicacionID UbicacionID { get; set; }
    }
}
