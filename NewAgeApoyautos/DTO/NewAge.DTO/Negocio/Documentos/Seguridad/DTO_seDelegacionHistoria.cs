using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Clase con el historico de delegaciones de tareas:
    /// Models DTO_seDelegacionHistoria
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_seDelegacionHistoria
    {
        #region DTO_seDelegacionHistoria

        /// <summary>
        /// Constructor con data reader
        /// </summary>
        public DTO_seDelegacionHistoria(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.UsuarioID.Value = dr["UsuarioID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaInicialAsig"].ToString()))
                    this.FechaInicialAsig.Value = Convert.ToDateTime(dr["FechaInicialAsig"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFinalAsig"].ToString()))
                    this.FechaFinalAsig.Value = Convert.ToDateTime(dr["FechaFinalAsig"]);
                this.UsuarioRemplazo.Value = dr["UsuarioRemplazo"].ToString();
                this.DelegacionActivaInd.Value = Convert.ToBoolean(dr["DelegacionActivaInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_seDelegacionHistoria()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.UsuarioID = new UDT_UsuarioID();
            this.FechaInicialAsig = new UDTSQL_smalldatetime();
            this.FechaFinalAsig = new UDTSQL_smalldatetime();
            this.UsuarioRemplazo = new UDT_UsuarioID();
            this.DelegacionActivaInd = new UDT_SiNo();
            this.EmpresaID = new UDT_EmpresaID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicialAsig { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFinalAsig { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioRemplazo { get; set; }

        [DataMember]
        public UDT_SiNo DelegacionActivaInd { get; set; }

        #endregion
    }
}
