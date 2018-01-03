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
    /// Models DTO_glActividadAreaFuncional
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glActividadAreaFuncional : DTO_MasterComplex
    {
        #region DTO_glActividadAreaFuncional
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glActividadAreaFuncional(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ActividadFlujoDesc.Value = dr["ActividadFlujoDesc"].ToString();
                    this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
                    this.Usuario1Desc.Value = dr["Usuario1Desc"].ToString();
                    this.Usuario2Desc.Value = dr["Usuario2Desc"].ToString();
                    this.Usuario3Desc.Value = dr["Usuario3Desc"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["UsuarioID1"].ToString()))
                        this.AlarmaUsuario1.Value = dr["UsuarioID1"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["UsuarioID2"].ToString()))
                        this.AlarmaUsuario2.Value = dr["UsuarioID2"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["UsuarioID3"].ToString()))
                        this.AlarmaUsuario3.Value = dr["UsuarioID3"].ToString();
                }
                else
                {
                    this.AlarmaUsuario1.Value = dr["AlarmaUsuario1"].ToString();
                    this.AlarmaUsuario2.Value = dr["AlarmaUsuario2"].ToString();
                    this.AlarmaUsuario3.Value = dr["AlarmaUsuario3"].ToString();
                }

                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glActividadAreaFuncional()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ActividadFlujoID = new UDT_BasicID();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.AreaFuncionalID = new UDT_BasicID();
            this.AreaFuncionalDesc = new UDT_Descriptivo();
            this.AlarmaUsuario1 = new UDT_BasicID();
            this.Usuario1Desc = new UDT_Descriptivo();
            this.AlarmaUsuario2 = new UDT_BasicID();
            this.Usuario2Desc = new UDT_Descriptivo();
            this.AlarmaUsuario3 = new UDT_BasicID();
            this.Usuario3Desc = new UDT_Descriptivo();
        }

        public DTO_glActividadAreaFuncional(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_glActividadAreaFuncional(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDT_BasicID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFuncionalDesc { get; set; }

        [DataMember]
        public UDT_BasicID AlarmaUsuario1 { get; set; }

        [DataMember]
        public UDT_Descriptivo Usuario1Desc { get; set; }

        [DataMember]
        public UDT_BasicID AlarmaUsuario2 { get; set; }

        [DataMember]
        public UDT_Descriptivo Usuario2Desc { get; set; }

        [DataMember]
        public UDT_BasicID AlarmaUsuario3 { get; set; }

        [DataMember]
        public UDT_Descriptivo Usuario3Desc { get; set; }
    }
}
