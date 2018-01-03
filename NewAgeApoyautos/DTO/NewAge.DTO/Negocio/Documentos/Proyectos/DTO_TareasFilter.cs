using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_TareasFilter
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_TareasFilter
    {
        #region DTO_TareasFilter
        public DTO_TareasFilter(IDataReader dr)
        {
            InitCols();
            try
            {
                this.TareaID.Value = Convert.ToString(dr["TareaID"]);
                this.TareaDesc.Value = Convert.ToString(dr["TareaDesc"]);
                this.ClaseServicioID.Value = Convert.ToString(dr["ClaseServicioID"]);
                this.ClaseServicioDesc.Value = Convert.ToString(dr["ClaseServicioDesc"]);
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.RecursoDesc.Value = Convert.ToString(dr["RecursoDesc"]);
                this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                this.RefProveedor.Value = Convert.ToString(dr["RefProveedor"]);
                this.MarcaInvID.Value = Convert.ToString(dr["MarcaInvID"]);
                this.MarcaDesc.Value = Convert.ToString(dr["MarcaDesc"]);
                this.MaterialInvID.Value = Convert.ToString(dr["MaterialInvID"]);
                this.MaterialDesc.Value = Convert.ToString(dr["MaterialDesc"]);
                this.SerieID.Value = Convert.ToString(dr["SerieID"]);
                this.SerieDesc.Value = Convert.ToString(dr["SerieDesc"]);
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
                this.UnidadDesc.Value = Convert.ToString(dr["UnidadDesc"]);
                this.EmpaqueInvID.Value = Convert.ToString(dr["EmpaqueInvID"]);
                this.EmpaqueDesc.Value = Convert.ToString(dr["EmpaqueDesc"]);
                this.ClaseInvID.Value = Convert.ToString(dr["ClaseInvID"]);
                this.ClaseDesc.Value = Convert.ToString(dr["ClaseDesc"]);
                this.GrupoInvID.Value = Convert.ToString(dr["GrupoInvID"]);
                this.GrupoDesc.Value = Convert.ToString(dr["GrupoDesc"]);
                this.TipoInvID.Value = Convert.ToString(dr["TipoInvID"]);
                this.TipoDesc.Value = Convert.ToString(dr["TipoDesc"]);
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
        public DTO_TareasFilter()
        {
            InitCols();
        }

        public void InitCols() 
        {
            this.TareaID = new UDT_CodigoGrl();
            this.TareaDesc= new UDT_Descriptivo();
            this.ClaseServicioID = new UDT_CodigoGrl();
            this.ClaseServicioDesc = new UDT_Descriptivo();
            this.RecursoID = new UDT_CodigoGrl();
            this.RecursoDesc = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.RefProveedor = new UDT_CodigoGrl20();
            this.MarcaInvID = new UDT_CodigoGrl();
            this.MarcaDesc = new UDT_Descriptivo();
            this.MaterialInvID = new UDT_CodigoGrl();
            this.MaterialDesc = new UDT_Descriptivo();
            this.SerieID = new UDT_CodigoGrl();
            this.SerieDesc = new UDT_Descriptivo();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.UnidadDesc = new UDT_Descriptivo();
            this.EmpaqueInvID = new UDT_EmpaqueInvID();
            this.EmpaqueDesc = new UDT_Descriptivo();
            this.ClaseInvID = new UDT_CodigoGrl();
            this.ClaseDesc = new UDT_Descriptivo();
            this.GrupoInvID = new UDT_CodigoGrl();
            this.GrupoDesc = new UDT_Descriptivo();
            this.TipoInvID = new UDT_CodigoGrl();
            this.TipoDesc = new UDT_Descriptivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDT_Descriptivo TareaDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl ClaseServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseServicioDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        public UDT_Descriptivo RecursoDesc { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        [DataMember]
        public UDT_CodigoGrl MarcaInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo MarcaDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl MaterialInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo MaterialDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl SerieID { get; set; }

        [DataMember]
        public UDT_Descriptivo SerieDesc { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadDesc { get; set; }

        [DataMember]
        public UDT_EmpaqueInvID EmpaqueInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpaqueDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl ClaseInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl GrupoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl TipoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo TipoDesc { get; set; }

        [DataMember]
        public List<DTO_TareasFilter> Detalle { get; set; }

        #endregion

    }
}
