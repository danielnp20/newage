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
    /// Models DTO_ccClienteReferencia
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccTipoNomina : DTO_MasterComplex
    {
        #region DTO_ccClienteReferencia
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccTipoNomina(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica) 
                {
                    this.PagaduriaDesc.Value = dr["PagaduriaDesc"].ToString();
                    this.ProfesionDesc.Value = dr["ProfesionDesc"].ToString();
                }
                this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                this.ProfesionID.Value = dr["ProfesionID"].ToString();
                this.TipoNomina.Value =  dr["TipoNomina"].ToString();
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccTipoNomina() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PagaduriaID = new UDT_BasicID();
            this.PagaduriaDesc = new UDT_Descriptivo();
            this.ProfesionID = new UDT_BasicID();
            this.ProfesionDesc = new UDT_Descriptivo();
            this.TipoNomina = new UDTSQL_char(10);
        }

        public DTO_ccTipoNomina(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccTipoNomina(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID PagaduriaID { get; set; }

        [DataMember]
        public UDT_Descriptivo PagaduriaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProfesionID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProfesionDesc { get; set; }

        [DataMember]
        public UDTSQL_char TipoNomina { get; set; }

    }

}
