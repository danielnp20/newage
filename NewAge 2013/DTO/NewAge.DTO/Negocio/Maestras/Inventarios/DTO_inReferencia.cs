using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_inReferencia
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inReferencia : DTO_MasterBasic
    {
        #region DTO_inReferencia
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inReferencia(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.GrupoInvDesc.Value = dr["GrupoInvDesc"].ToString();
                    this.ClaseInvDesc.Value = dr["ClaseInvDesc"].ToString();
                    this.TipoInvDesc.Value = dr["TipoInvDesc"].ToString();
                    this.SerieDesc.Value = dr["SerieDesc"].ToString();
                    this.MaterialInvDesc.Value = dr["MaterialInvDesc"].ToString();
                    this.MarcaInvDesc.Value = dr["MarcaInvDesc"].ToString();
                    this.ProveedorDesc.Value = dr["ProveedorDesc"].ToString();
                    this.PosicionArancelDesc.Value = dr["PosicionArancelDesc"].ToString();
                    this.EmpaqueInvDesc.Value = dr["EmpaqueInvDesc"].ToString();
                    this.UnidadInvDesc.Value = dr["UnidadInvDesc"].ToString();
                    this.MonCompraDesc.Value = dr["MonCompraDesc"].ToString();
                }

                this.DescrDetallada.Value = dr["DescrDetallada"].ToString();
                this.GrupoInvID.Value = dr["GrupoInvID"].ToString();
                this.ClaseInvID.Value = dr["ClaseInvID"].ToString();
                this.TipoInvID.Value = dr["TipoInvID"].ToString();
                this.SerieID.Value = dr["SerieID"].ToString();
                this.MaterialInvID.Value = dr["MaterialInvID"].ToString();
                this.MarcaInvID.Value = dr["MarcaInvID"].ToString();
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.PosicionArancelID.Value = dr["PosicionArancelID"].ToString();
                this.EmpaqueInvID.Value = dr["EmpaqueInvID"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                this.CostoStandar.Value = Convert.ToDecimal(dr["CostoStandar"]);

                if (!string.IsNullOrWhiteSpace(dr["Volumen"].ToString()))
                    this.Volumen.Value = Convert.ToDecimal(dr["Volumen"]);
                if (!string.IsNullOrWhiteSpace(dr["Peso"].ToString()))
                    this.Peso.Value = Convert.ToDecimal(dr["Peso"]);
                this.MonCompra.Value = dr["MonCompra"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["VlrCompra"].ToString()))
                    this.VlrCompra.Value = Convert.ToDecimal(dr["VlrCompra"]);
                this.RefProveedor.Value = dr["RefProveedor"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inReferencia() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DescrDetallada = new UDT_DescripTExt();
            this.GrupoInvID = new UDT_BasicID();
            this.GrupoInvDesc = new UDT_Descriptivo();
            this.ClaseInvID = new UDT_BasicID();
            this.ClaseInvDesc = new UDT_Descriptivo();
            this.TipoInvID = new UDT_BasicID();
            this.TipoInvDesc = new UDT_Descriptivo();
            this.SerieID = new UDT_BasicID();
            this.SerieDesc = new UDT_Descriptivo();
            this.MaterialInvID = new UDT_BasicID();
            this.MaterialInvDesc = new UDT_Descriptivo();
            this.MarcaInvID = new UDT_BasicID();
            this.MarcaInvDesc = new UDT_Descriptivo();
            this.ProveedorID = new UDT_BasicID();
            this.ProveedorDesc = new UDT_Descriptivo();
            this.PosicionArancelID = new UDT_BasicID();
            this.PosicionArancelDesc = new UDT_Descriptivo();
            this.EmpaqueInvID = new UDT_BasicID();
            this.EmpaqueInvDesc = new UDT_Descriptivo();
            this.UnidadInvID = new UDT_BasicID();
            this.UnidadInvDesc = new UDT_Descriptivo();
            this.CostoStandar = new UDT_Valor();
            this.SaldoExistencia = new UDT_Cantidad();
            this.CostosExistencia = new DTO_inCostosExistencias();
            this.Peso = new UDT_Cantidad();
            this.Volumen = new UDT_Cantidad();
            this.DocCompra = new UDT_Consecutivo();
            this.MonCompra = new UDT_BasicID();
            this.MonCompraDesc = new UDT_Descriptivo();
            this.VlrCompra = new UDT_Valor();
            this.RefProveedor = new UDT_CodigoGrl20();
        }

        public DTO_inReferencia(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inReferencia(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  

        #endregion 

        #region Propiedades

        [DataMember]
        public UDT_DescripTExt DescrDetallada { get; set; }

        [DataMember]
        public UDT_BasicID GrupoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID ClaseInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID TipoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo TipoInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID SerieID  { get; set; }

        [DataMember]
        public UDT_Descriptivo SerieDesc { get; set; }

        [DataMember]
        public UDT_BasicID MaterialInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo MaterialInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID MarcaInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo MarcaInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProveedorID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorDesc { get; set; }

        [DataMember]
        public UDT_BasicID PosicionArancelID  { get; set; }

        [DataMember]
        public UDT_Descriptivo PosicionArancelDesc { get; set; }

        [DataMember]
        public UDT_BasicID EmpaqueInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpaqueInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadInvDesc { get; set; }

        [DataMember]
        public UDT_Valor CostoStandar { get; set; }

        [DataMember]
        public UDT_Cantidad Peso { get; set; }

        [DataMember]
        public UDT_Cantidad Volumen { get; set; }

        [DataMember]
        public UDT_Consecutivo DocCompra { get; set; }

        [DataMember]
        public UDT_BasicID MonCompra { get; set; }

        [DataMember]
        public UDT_Descriptivo MonCompraDesc { get; set; }

        [DataMember]
        public UDT_Valor VlrCompra { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        //Parametros Extras

        [DataMember]
        [AllowNull]
        public string UbicacionID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad SaldoExistencia { get; set; }

        [DataMember]
        [AllowNull]
        public DTO_inCostosExistencias CostosExistencia { get; set; }

        #endregion
    }

}
