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
    /// Class Activos Fijos:
    /// Models DTO_Activo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_Activo
    {
        #region DTO_Activo

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_Activo(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (dr["PlaquetaID"] != DBNull.Value)
                    this.PlaquetaID.Value = dr["PlaquetaID"].ToString();
                if (dr["SerialID"] != DBNull.Value)
                    this.SerialID.Value = Convert.ToInt32(dr["SerialID"]);
                if (dr["inReferenciaID"] != DBNull.Value)
                    this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                if (dr["EstadoInv"] != DBNull.Value)
                    this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                if (dr["Descripcion"] != DBNull.Value)
                    this.Descripcion.Value = dr["Descripcion"].ToString();
                if (dr["ActivoGrupoID"] != DBNull.Value)
                    this.ActivoGrupoID.Value = dr["ActivoGrupoID"].ToString();
                if (dr["ActivoClaseID"] != DBNull.Value)
                    this.ActivoClaseID.Value = dr["ActivoClaseID"].ToString();
                if (dr["Modelo"] != DBNull.Value)
                    this.Modelo.Value = dr["Modelo"].ToString();
                if (dr["LocFisicaID"] != DBNull.Value)
                    this.LocFisicaID.Value = dr["LocFisicaID"].ToString();
                if (dr["ProyectoID"] != DBNull.Value)
                    this.ProyectoID.Value = dr["ProyectoID"].ToString();
                if (dr["CentroCostoID"] != DBNull.Value)
                    this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                if (dr["Tipo"] != DBNull.Value)
                    this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_Activo()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Marca = new UDT_SiNo();
            this.PlaquetaID = new UDT_PlaquetaID();
            this.SerialID = new UDT_Consecutivo();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.Descripcion = new UDT_DescripTExt();
            this.ActivoGrupoID = new UDT_ActivoGrupoID();
            this.ActivoClaseID = new UDT_ActivoClaseID();
            this.Modelo = new UDTSQL_char(20);
            this.LocFisicaID = new UDT_LocFisicaID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.Tipo = new UDTSQL_tinyint();
        }

        #endregion
        #region Propiedades

        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public UDT_SiNo Marca { get; set; }

        [DataMember]
        public UDT_PlaquetaID PlaquetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo SerialID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_ActivoGrupoID ActivoGrupoID { get; set; }

        [DataMember]
        public UDT_ActivoClaseID ActivoClaseID { get; set; }

        [DataMember]
        public UDTSQL_char Modelo { get; set; }

        [DataMember]
        public UDT_LocFisicaID LocFisicaID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        #endregion
    }
}
