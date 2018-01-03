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
    /// Models DTO_coDocumentoCargo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coDocumentoCargo : DTO_MasterComplex
    {
        #region DTO_coDocumentoCargo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coDocumentoCargo(IDataReader dr, DTO_aplMaestraPropiedades mp)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                try { this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString(); } catch (Exception e)  { }
                try { this.CargoEspecialDesc.Value = dr["CargoEspecialDesc"].ToString(); } catch (Exception e)  { }

                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.CargoEspecialID.Value = dr["CargoEspecialID"].ToString();
            }
            catch(Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coDocumentoCargo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.CargoEspecialID = new UDT_BasicID();
            this.CargoEspecialDesc = new UDT_Descriptivo();
        }

        public DTO_coDocumentoCargo(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_coDocumentoCargo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CargoEspecialID { get; set; }

        [DataMember]
        public UDT_Descriptivo CargoEspecialDesc { get; set; }        
    }
}
