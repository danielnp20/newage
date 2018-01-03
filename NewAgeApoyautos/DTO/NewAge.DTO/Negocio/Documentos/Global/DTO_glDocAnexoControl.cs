using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_glDocAnexoControl
    {

        #region Constructor

        public DTO_glDocAnexoControl(IDataReader dr)
        {
            try
            {
                this.InitCols();

                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);

                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.ConsReplica.Value = Convert.ToInt32(dr["ConsReplica"]);
                this.ArchivoNombre.Value = dr["ArchivoNombre"].ToString();

                this.Nuevo.Value = false;
                this.Eliminar.Value = false;
                this.Actualizar.Value = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DTO_glDocAnexoControl()
        {
            this.InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ConsReplica = new UDT_Consecutivo();
            this.ArchivoNombre = new UDTSQL_char(50);
            //Extra
            this.DocumentoID = new UDT_Consecutivo();
            this.Descriptivo = new UDT_DescripTBase();
            this.Nuevo = new UDT_SiNo();
            this.Eliminar = new UDT_SiNo();
            this.Actualizar = new UDT_SiNo();
        }


        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsReplica { get; set; }

        [DataMember]
        public UDTSQL_char ArchivoNombre { get; set; }

        //Extras

        [DataMember]
        public UDT_Consecutivo DocumentoID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public byte[] Archivo { get; set; }

        [DataMember]
        public UDT_SiNo Nuevo { get; set; }

        [DataMember]
        public UDT_SiNo Eliminar { get; set; }

        [DataMember]
        public UDT_SiNo Actualizar { get; set; }

        //Solo temporal 
        [DataMember]
        public string Extension { get; set; }


        #endregion
    }
}
