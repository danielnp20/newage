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
    /// Models DTO_faPrecioServicio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faPrecioServicio : DTO_MasterComplex
    {
        #region DTO_faPrecioServicio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faPrecioServicio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ListaPrecioDesc.Value = dr["ListaPrecioDesc"].ToString();
                    this.ServicioDesc.Value = dr["ServicioDesc"].ToString();
                    this.inReferenciaDesc.Value = dr["inReferenciaDesc"].ToString();
                    this.UnidadDesc.Value = dr["UnidadDesc"].ToString();
                }

                this.ListaPrecioID.Value = dr["ListaPrecioID"].ToString();
                this.ServicioID.Value = dr["ServicioID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                if (!string.IsNullOrEmpty(dr["CantidadUNI"].ToString()))
                    this.CantidadUNI.Value = Convert.ToInt32(dr["CantidadUNI"]);
                if (!string.IsNullOrEmpty(dr["ValorLocal"].ToString()))
                    this.ValorLocal.Value = Convert.ToDecimal(dr["ValorLocal"]);
                if (!string.IsNullOrEmpty(dr["ValorExtra"].ToString()))
                    this.ValorExtra.Value = Convert.ToDecimal(dr["ValorExtra"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faPrecioServicio() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ListaPrecioID = new UDT_BasicID();
            this.ListaPrecioDesc = new UDT_Descriptivo();
            this.ServicioID = new UDT_BasicID();
            this.ServicioDesc = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_BasicID();
            this.inReferenciaDesc = new UDT_Descriptivo();
            this.UnidadInvID = new UDT_BasicID();
            this.UnidadDesc = new UDT_Descriptivo();
            this.CantidadUNI = new UDT_Cantidad();
            this.ValorLocal = new UDT_Valor();
            this.ValorExtra = new UDT_Valor();

        }

        public DTO_faPrecioServicio(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_faPrecioServicio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID ListaPrecioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ListaPrecioDesc { get; set; }

        [DataMember]
        public UDT_BasicID ServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ServicioDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo inReferenciaDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadDesc { get; set; }
        
        [DataMember]
        public UDT_Cantidad CantidadUNI { get; set; }

        [DataMember]
        public UDT_Valor ValorLocal { get; set; }

        [DataMember]
        public UDT_Valor ValorExtra { get; set; }
        
        
        

    }
}


