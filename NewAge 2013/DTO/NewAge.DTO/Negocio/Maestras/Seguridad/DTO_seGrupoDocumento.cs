using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_seGrupoForma
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_seGrupoDocumento
    {
        #region Constructor
		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_seGrupoDocumento(IDataReader dr)
        {
            InitCols();
            try
            {
                this.seGrupoID.Value = Convert.ToString(dr["seGrupoID"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.AccionesPerm.Value = Convert.ToInt64(dr["AccionesPerm"]);
                this.CtrlVersion.Value = Convert.ToInt16(dr["CtrlVersion"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_seGrupoDocumento(IDataReader dr, string grupo)
        {
            InitCols();
            try
            {
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.ModuloID.Value = dr["ModuloID"].ToString();
                this.DocumentoTipo.Value = Convert.ToByte(dr["DocumentoTipo"]);

                if (dr["seGrupoID"] != null && dr["seGrupoID"].ToString() != string.Empty)
                    this.seGrupoID.Value = Convert.ToString(dr["seGrupoID"]);
                else
                    this.seGrupoID.Value = grupo;
                if (dr["AccionesPerm"] != null && dr["seGrupoID"].ToString() != string.Empty)
                    this.AccionesPerm.Value = Convert.ToInt64(dr["AccionesPerm"]);
                else
                    this.AccionesPerm.Value = 0;
                if (dr["CtrlVersion"] != null && dr["seGrupoID"].ToString() != string.Empty)
                    this.CtrlVersion.Value = Convert.ToInt16(dr["CtrlVersion"]);
                else
                    this.CtrlVersion.Value = 0;
            }
            catch (Exception e)
            { ; }
        }


        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_seGrupoDocumento()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.seGrupoID = new UDT_seGrupoID();
            this.DocumentoID = new UDT_DocumentoID();
            this.AccionesPerm = new UDTSQL_bigint();
            this.CtrlVersion = new UDT_CtrlVersion();
            this.ModuloID = new UDTSQL_varchar(3);
            this.DocumentoTipo = new UDTSQL_tinyint();
        }
	    #endregion

        /// <summary>
        /// Gets or sets the seGrupoID
        /// </summary>
        [DataMember]
        public UDT_seGrupoID seGrupoID { get; set; }

        /// <summary>
        /// Gets or sets the DocumentoID
        /// </summary>
        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        /// <summary>
        /// Gets or sets the AccionesPerm
        /// </summary>
        [DataMember]
        public UDTSQL_bigint AccionesPerm { get; set; }


        /// <summary>
        /// Gets or sets the CtrlVersion
        /// </summary>
        [DataMember]
        public UDT_CtrlVersion CtrlVersion { get; set; }

        /// <summary>
        /// Gets or sets the ModuloID
        /// </summary>
        [DataMember]
        public UDTSQL_varchar ModuloID { get; set; }

        /// <summary>
        /// Gets or sets the DocumentoTipo
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint DocumentoTipo { get; set; }

    }
}
