using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla DTO_inLiquidacionDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inImportacionDocu
    {
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inImportacionDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.AgenteAduanaID.Value = dr["AgenteAduanaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ModalidadImp"].ToString()))
                    this.ModalidadImp.Value = Convert.ToByte(dr["ModalidadImp"]);
                this.TipoTransporte.Value = Convert.ToByte(dr["TipoTransporte"]);
                this.TasaImport.Value = Convert.ToDecimal(dr["TasaImport"]);
                this.DeclaracionImp.Value = dr["DeclaracionImp"].ToString();
                this.DeclaracionValor.Value = dr["DeclaracionValor"].ToString();
                this.DocImportadora.Value = dr["DocImportadora"].ToString();
                this.DocTransporte.Value = dr["DocTransporte"].ToString();
                this.DocMvtoZonaFranca.Value = dr["DocMvtoZonaFranca"].ToString();
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();           
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inImportacionDocu()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.AgenteAduanaID = new UDT_ProveedorID();
            this.AgenteAduanaDesc = new UDT_Descriptivo();
            this.ModalidadImp = new UDTSQL_tinyint();
            this.TipoTransporte = new UDTSQL_tinyint();
            this.TasaImport = new UDT_TasaID();
            this.DeclaracionImp = new UDTSQL_char(20);
            this.DeclaracionValor = new UDTSQL_char(20);
            this.DocImportadora = new UDTSQL_char(20);
            this.DocTransporte = new UDTSQL_char(20);
            this.DocMvtoZonaFranca = new UDTSQL_char(20);
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.Observacion = new UDT_DescripTExt();
        }
        

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_ProveedorID AgenteAduanaID { get; set; }

        [DataMember]
        public UDT_Descriptivo AgenteAduanaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint ModalidadImp { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoTransporte { get; set; }
    
        [DataMember]
        public UDT_TasaID TasaImport { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DeclaracionImp { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DeclaracionValor { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DocImportadora { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DocTransporte { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DocMvtoZonaFranca { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Observacion { get; set; }


        #endregion
    }
}
