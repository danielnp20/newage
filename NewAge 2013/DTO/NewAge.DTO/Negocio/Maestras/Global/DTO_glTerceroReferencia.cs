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
    /// Models DTO_glTerceroReferencia
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glTerceroReferencia : DTO_MasterComplex
    {
        #region glAreaFuncionalDocumentoPrefijo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glTerceroReferencia(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                }

                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.TipoReferencia.Value = Convert.ToByte(dr["TipoReferencia"]);
                this.Nombre.Value = dr["Nombre"].ToString();
                this.Relacion.Value = dr["Relacion"].ToString();
                this.Direccion.Value = dr["Direccion"].ToString();
                if (!string.IsNullOrEmpty(dr["Barrio"].ToString()))
                    this.Barrio.Value = dr["Barrio"].ToString();
                this.Telefono.Value = dr["Telefono"].ToString();
                this.Ciudad.Value = dr["Ciudad"].ToString();
                if (!string.IsNullOrEmpty(dr["Correo"].ToString()))
                    this.Correo.Value = dr["Correo"].ToString();
                this.Celular.Value = dr["Celular"].ToString(); 

            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glTerceroReferencia() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.TerceroID=new UDT_BasicID();
            this.TerceroDesc=new UDT_Descriptivo();
            this.TipoReferencia=new UDTSQL_tinyint();
            this.Nombre=new UDTSQL_char(50);
            this.Relacion=new UDTSQL_char(50);
            this.Direccion=new UDTSQL_char(50);
            this.Barrio=new UDTSQL_char(50);
            this.Telefono=new UDTSQL_char(50);
            this.Ciudad=new UDTSQL_char(25);
            this.Correo = new UDTSQL_char(60);
            this.Celular = new UDTSQL_char(50);
            //Campo Adicional
            this.NuevoRegistro = new UDT_SiNo();
            this.NuevoRegistro.Value = false;
            this.CodLLamada = new UDT_CodigoGrl5();
            this.LlamadaREF = new UDTSQL_char(50);
            

        }

        public DTO_glTerceroReferencia(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_glTerceroReferencia(DTO_aplMaestraPropiedades masterProperties) : base(masterProperties)
        {
        }  

        #endregion 

        #region Propiedades

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoReferencia { get; set; }

        [DataMember]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        public UDTSQL_char Relacion { get; set; }

        [DataMember]
        public UDTSQL_char Direccion { get; set; }

        [DataMember]
        public UDTSQL_char Barrio { get; set; }

        [DataMember]
        public UDTSQL_char Telefono { get; set; }

        [DataMember]
        public UDTSQL_char Ciudad { get; set; }

        [DataMember]
        public UDTSQL_char Correo { get; set; }   

        //Campos Extras

        [DataMember]
        public UDT_SiNo NuevoRegistro { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CodLLamada { get; set; }

        [DataMember]
        public UDTSQL_char LlamadaREF { get; set; }

        [DataMember]
        public UDTSQL_char Celular { get; set; }

        #endregion
    }
}
