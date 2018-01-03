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
    /// Models DTO_glEmpresa
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glEmpresa : DTO_MasterBasic
    {
        #region DTO_glEmpresa

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// </summary>
        /// <param name="?"></param>
        public DTO_glEmpresa(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.PaisDesc.Value = dr["PaisDesc"].ToString();
                    this.EmpresaGrupoDesc.Value = dr["EmpresaGrupoDesc"].ToString();
                }

                this.EmpresaGrupoID_.Value = Convert.ToString(dr["EmpresaGrupoID"]);
                this.PaisID.Value = Convert.ToString(dr["PaisID"]);
                this.LogoNombre.Value = Convert.ToString(dr["LogoNombre"]);
                this.NumeroControl.Value = Convert.ToString(dr["NumeroControl"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glEmpresa()
            : base()
        {
            this.InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
           //this.EmpresaID = new UDT_BasicID();
            this.EmpresaGrupoID_ = new UDT_BasicID();
            this.EmpresaGrupoDesc = new UDT_Descriptivo();
            this.PaisID = new UDT_BasicID();
            this.PaisDesc = new UDT_Descriptivo();
            this.Logo = new UDT_Imagen();
            this.LogoNombre = new UDTSQL_varchar(100);
            this.NumeroControl = new UDTSQL_varchar(4);
        }

        public DTO_glEmpresa(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glEmpresa(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }

        #endregion

        [DataMember]
        public UDT_BasicID EmpresaGrupoID_ { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpresaGrupoDesc { get; set; }

        [DataMember]
        public UDT_BasicID PaisID { get; set; }

        [DataMember]
        public UDT_Descriptivo PaisDesc { get; set; }

        [DataMember]
        public UDT_Imagen Logo { get; set; }

        [DataMember]
        public UDTSQL_varchar LogoNombre { get; set; }

        /// <summary>
        /// Gets or sets control number (NO BORRAR SE CARGA EN MEMORIA)
        /// </summary>
        [DataMember]
        public UDTSQL_varchar NumeroControl { get; set; }
    }

}
