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
    /// Models DTO_seUsuarioBodega
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_seUsuarioBodega : DTO_MasterComplex 
    {
        #region DTO_seUsuarioBodega

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_seUsuarioBodega(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.UsuarioDesc.Value = dr["UsuarioDesc"].ToString();
                    this.BodegaDesc.Value = dr["BodegaDesc"].ToString();

                    this.seUsuarioID.Value = dr["UsuarioID1"].ToString();
                }
                else
                    this.seUsuarioID.Value = dr["seUsuarioID"].ToString();


                this.BodegaID.Value = dr["BodegaID"].ToString();
                this.EntradaInd.Value = Convert.ToBoolean(dr["EntradaInd"]);
                this.SalidaInd.Value = Convert.ToBoolean(dr["SalidaInd"]);
                this.ConsultaCostosInd.Value = Convert.ToBoolean(dr["ConsultaCostosInd"]);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_seUsuarioBodega() : base()
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
            this.BodegaID = new UDT_BasicID();
            this.BodegaDesc = new UDT_Descriptivo();
            this.EntradaInd = new UDT_SiNo();
            this.SalidaInd = new UDT_SiNo();
            this.ConsultaCostosInd = new UDT_SiNo();
        }

        public DTO_seUsuarioBodega(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_seUsuarioBodega(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  

        #endregion 
 
        [DataMember]
        public UDT_BasicID seUsuarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioDesc { get; set; }

        [DataMember]
        public UDT_BasicID BodegaID { get; set; }

        [DataMember]
        public UDT_Descriptivo BodegaDesc { get; set; }

        [DataMember]
        public UDT_SiNo EntradaInd { get; set; }

        [DataMember]
        public UDT_SiNo SalidaInd { get; set; }

        [DataMember]
        public UDT_SiNo ConsultaCostosInd { get; set; }

    }

}
