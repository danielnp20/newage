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
    /// Models DTO_ccConcesionario
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccConcesionario : DTO_MasterBasic
    {
        #region ccConcesionario
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccConcesionario(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.ZonaDesc.Value = dr["ZonaDesc"].ToString();
                    this.CtoCostoNormalDesc.Value = dr["CtoCostoNormalDesc"].ToString();
                    this.CtoCostoJuridicoDesc.Value = dr["CtoCostoJuridicoDesc"].ToString();
                    this.CiudadDesc.Value = dr["CiudadDesc"].ToString();
                }
                    
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.ZonaID.Value = dr["ZonaID"].ToString();
                this.CtoCostoNormal.Value = dr["CtoCostoNormal"].ToString();
                this.CtoCostoJuridico.Value = dr["CtoCostoJuridico"].ToString();
                if (!string.IsNullOrEmpty(dr["Marca"].ToString()))
                    this.Marca.Value = dr["Marca"].ToString();
                if (!string.IsNullOrEmpty(dr["Red"].ToString()))
                    this.Red.Value = dr["Red"].ToString();
                if (!string.IsNullOrEmpty(dr["Tipo"].ToString()))
                   this.Tipo.Value = dr["Tipo"].ToString();
                if (!string.IsNullOrEmpty(dr["Ciudad"].ToString()))
                    this.Ciudad.Value = dr["Ciudad"].ToString();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_ccConcesionario()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();
            this.ZonaDesc = new UDT_Descriptivo();
            this.ZonaID = new UDT_BasicID();
            this.CtoCostoNormal = new UDT_BasicID();
            this.CtoCostoNormalDesc = new UDT_Descriptivo();
            this.CtoCostoJuridico = new UDT_BasicID();
            this.CtoCostoJuridicoDesc = new UDT_Descriptivo();
           
            this.Marca = new UDTSQL_char(30);
            this.Red= new UDTSQL_char(30);
            this.Tipo = new UDTSQL_char(30);
            this.Ciudad = new UDT_LugarGeograficoID();
            this.CiudadDesc = new UDT_Descriptivo();
        }

        public DTO_ccConcesionario(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccConcesionario(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID ZonaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ZonaDesc { get; set; }

        [DataMember]
        public UDT_BasicID CtoCostoNormal { get; set; }

        [DataMember]
        public UDT_Descriptivo CtoCostoNormalDesc { get; set; }

        [DataMember]
        public UDT_BasicID CtoCostoJuridico { get; set; }

        [DataMember]
        public UDT_Descriptivo CtoCostoJuridicoDesc { get; set; }

        [DataMember]
        public UDTSQL_char Marca { get; set; }

       [DataMember]
        public UDTSQL_char Red { get; set; }

       [DataMember] 
       public UDTSQL_char Tipo { get; set; }


       [DataMember]
       public UDT_LugarGeograficoID Ciudad { get; set; }

       [DataMember]
       public UDT_Descriptivo CiudadDesc { get; set; }




    }
}
