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
    /// Models DTO_noEmpleadoFamilia
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noEmpleadoFamilia : DTO_MasterComplex
    {
        #region DTO_noEmpleadoFamilia
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noEmpleadoFamilia(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.EmpleadoDesc.Value = dr["EmpleadoDesc"].ToString();
                }
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();                
                this.Nombre.Value = dr["Nombre"].ToString();
                this.TipoFamiliar.Value = Convert.ToByte(dr["TipoFamiliar"]);
                this.FechaNac.Value = Convert.ToDateTime(dr["FechaNac"]);
                this.Observacion.Value = (dr["Observacion"].ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noEmpleadoFamilia()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpleadoID = new UDT_BasicID();
            this.EmpleadoDesc = new UDT_Descriptivo();
            this.Nombre = new UDTSQL_char(30);
            this.TipoFamiliar = new UDTSQL_tinyint();
            this.FechaNac = new UDTSQL_smalldatetime();
            this.Observacion = new UDT_DescripTBase();
        }

        public DTO_noEmpleadoFamilia(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noEmpleadoFamilia(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID EmpleadoID { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpleadoDesc { get; set; }

        [DataMember]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoFamiliar { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNac { get; set; }
        
        [DataMember]
        public UDT_DescripTBase Observacion { get; set; }
    }
}
