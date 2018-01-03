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
    /// Models DTO_seUsuarioPrefijo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_seUsuarioPrefijo : DTO_MasterComplex 
    {
        #region DTO_seUsuarioPrefijo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_seUsuarioPrefijo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    //this.EmpresaDesc.Value = dr["EmpresaDesc"].ToString();
                    this.UsuarioDesc.Value = dr["UsuarioDesc"].ToString();
                    this.PrefijoDesc.Value = dr["PrefijoDesc"].ToString();
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                }

                //this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.seUsuarioID.Value = dr["seUsuarioID"].ToString();
                this.DocumentoID.Value = dr["DocumentoID"].ToString();
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_seUsuarioPrefijo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //this.EmpresaID = new UDT_BasicID();
            //this.EmpresaDesc = new UDT_Descriptivo();
            this.seUsuarioID = new UDT_BasicID();
            this.UsuarioDesc = new UDT_Descriptivo();
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
            this.PrefijoID = new UDT_BasicID();
            this.PrefijoDesc = new UDT_Descriptivo();         
        }

        public DTO_seUsuarioPrefijo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_seUsuarioPrefijo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        //[DataMember]
        //public UDT_BasicID EmpresaID { get; set; }

        //[DataMember]
        //public UDT_Descriptivo EmpresaDesc { get; set; }

        [DataMember]
        public UDT_BasicID seUsuarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioDesc { get; set; }

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDT_BasicID PrefijoID { get; set; }

        [DataMember]
        public UDT_Descriptivo PrefijoDesc { get; set; }

    }

}
