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
    /// Models DTO_glAreaFuncional.cs
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glAreaFuncional : DTO_MasterBasic
    {
        #region DTO_glAreaFuncional
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glAreaFuncional(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.LocFisicaDesc.Value = dr["LocFisicaDesc"].ToString();
                    this.AreaAprobacionAlternaDesc.Value = dr["AreaAprobacionAlternaDesc"].ToString();
                    this.GrupoApruebaDocDesc.Value = dr["GrupoApruebaDocDesc"].ToString();
                    this.UsuarioDirDesc.Value = dr["UsuarioDirDesc"].ToString();
                    this.UsuarioSubDesc.Value = dr["UsuarioSubDesc"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["UsuarioID1"].ToString()))
                        this.DirectorArea.Value = dr["UsuarioID1"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["UsuarioID2"].ToString()))
                        this.SubDirectorArea.Value = dr["UsuarioID2"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                    this.Prefijo1Desc.Value = dr["Prefijo1Desc"].ToString();
                    this.Prefijo2Desc.Value = dr["Prefijo2Desc"].ToString();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(dr["DirectorArea"].ToString()))
                        this.DirectorArea.Value = dr["DirectorArea"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["SubDirectorArea"].ToString()))
                        this.SubDirectorArea.Value = dr["SubDirectorArea"].ToString();
                }
                this.LocFisicaID.Value = dr["LocFisicaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["AreaAprobacionAlternaID"].ToString()))
                    this.AreaAprobacionAlternaID.Value=dr["AreaAprobacionAlternaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["GrupoApruebaDocID"].ToString()))
                    this.GrupoApruebaDocID.Value = dr["GrupoApruebaDocID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.Prefijo1.Value = dr["Prefijo1"].ToString();
                this.Prefijo2.Value = dr["Prefijo2"].ToString();
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glAreaFuncional() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.LocFisicaID = new UDT_BasicID();
            this.LocFisicaDesc = new UDT_Descriptivo();
            this.AreaAprobacionAlternaID = new UDT_BasicID();
            this.AreaAprobacionAlternaDesc = new UDT_Descriptivo();
            this.GrupoApruebaDocID = new UDT_BasicID();
            this.GrupoApruebaDocDesc = new UDT_Descriptivo();
            this.DirectorArea=new UDT_BasicID();
            this.UsuarioDirDesc=new UDT_Descriptivo();
            this.SubDirectorArea=new UDT_BasicID();
            this.UsuarioSubDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.Prefijo1 = new UDT_BasicID();
            this.Prefijo1Desc = new UDT_Descriptivo();
            this.Prefijo2 = new UDT_BasicID();
            this.Prefijo2Desc = new UDT_Descriptivo();
         }

        public DTO_glAreaFuncional(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glAreaFuncional(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
     
        [DataMember]
        public UDT_BasicID LocFisicaID { get; set; }

        [DataMember]
        public UDT_Descriptivo LocFisicaDesc { get; set; }

        [DataMember]
        public UDT_BasicID AreaAprobacionAlternaID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaAprobacionAlternaDesc { get; set; }

        [DataMember]
        public UDT_BasicID GrupoApruebaDocID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoApruebaDocDesc { get; set; }

        [DataMember]
        public UDT_BasicID DirectorArea { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioDirDesc { get; set; }

        [DataMember]
        public UDT_BasicID SubDirectorArea { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioSubDesc { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_BasicID Prefijo1 { get; set; }

        [DataMember]
        public UDT_Descriptivo Prefijo1Desc { get; set; }

        [DataMember]
        public UDT_BasicID Prefijo2 { get; set; }

        [DataMember]
        public UDT_Descriptivo Prefijo2Desc { get; set; }


    }

}