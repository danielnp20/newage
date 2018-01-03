using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_pyServicioDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyServicioDetaDetalle
    {
        #region DTO_pyServicioDetaDetalle

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyServicioDetaDetalle(IDataReader dr)
        {
            InitCols();
            try
            {
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                if (!string.IsNullOrWhiteSpace(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToInt32(dr["Cantidad"]);
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);               
                if (!string.IsNullOrWhiteSpace(dr["CostoLocal"].ToString()))
                    this.CostoLocal.Value = Convert.ToDecimal(dr["CostoLocal"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoExtra"].ToString()))
                    this.CostoExtra.Value = Convert.ToDecimal(dr["CostoExtra"]);                
                if (!string.IsNullOrWhiteSpace(dr["inReferenciaIDDesc"].ToString()))
                    this.inReferenciaIDDesc.Value = dr["inReferenciaIDDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["RecursoIDDesc"].ToString()))
                    this.RecursoIDDesc.Value = dr["RecursoIDDesc"].ToString();             
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
        public DTO_pyServicioDetaDetalle()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.RecursoID = new UDT_CodigoGrl();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.Cantidad = new UDT_Cantidad();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CostoLocalPRY = new UDT_Valor();
            this.CostoExtraPRY = new UDT_Valor();
            this.CostoLocalEMP = new UDT_Valor();
            this.CostoExtraEMP = new UDT_Valor();
            this.CostoLocal = new UDT_Valor();
            this.CostoExtra = new UDT_Valor();        
            this.inReferenciaIDDesc = new UDT_DescripTBase();
            this.RecursoIDDesc = new UDT_DescripTBase();
        }

        #endregion

        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }
          
        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }
        
        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalPRY { get; set; }

        [DataMember]
        public UDT_Valor CostoExtraPRY { get; set; }

        [DataMember]
        public UDT_Valor CostoLocal { get; set; }
     
        [DataMember]
        public UDT_Valor CostoExtra { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalEMP { get; set; }

        [DataMember]
        public UDT_Valor CostoExtraEMP { get; set; }
        
        #region Descriptivos

        [DataMember]
        public UDT_DescripTBase inReferenciaIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase RecursoIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase CodigoBSIDDesc { get; set; }

        #endregion 

    }
}
