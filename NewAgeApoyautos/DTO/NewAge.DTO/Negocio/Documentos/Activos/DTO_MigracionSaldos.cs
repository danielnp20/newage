using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_acActivoControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigracionSaldos
    {
        #region Constructor
                
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigracionSaldos()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PlaquetaID = new UDT_PlaquetaID();
            this.SerialID = new UDTSQL_char(25);
            this.Fecha = new UDTSQL_smalldatetime();
            this.ReferenciaID = new UDT_ReferenciaID();
            this.LocFisicaID = new UDT_LocFisicaID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.ProyectoID = new UDT_ProyectoID();
            this.ActivoGrupoID = new UDT_ActivoGrupoID();
            this.ActivoClaseID = new UDT_ActivoClaseID();
            this.ActivoTipoID = new UDT_ActivoTipoID();
            this.VidaUtilLOC = new UDTSQL_int();
            this.VidaUtilIFRS = new UDTSQL_int();
            this.VidaUtilUSG = new UDTSQL_int();
            this.TipoDepreLOC = new UDTSQL_tinyint();
            this.TipoDepreIFRS = new UDTSQL_tinyint();
            this.TipoDepreUSG = new UDTSQL_tinyint();
            this.ValorSalvamentoLOC = new UDT_Valor();
            this.ValorSalvamentoIFRS = new UDT_Valor();
            this.ValorSalvamentoUSG = new UDT_Valor();
            this.ValorSalvamentoIFRSUS = new UDT_Valor();
            this.CostoLOC = new UDT_Valor();
            this.CostoEXT = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_PlaquetaID PlaquetaID { get; set; }
       
        [DataMember]
        [AllowNull]
        public UDTSQL_char SerialID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ReferenciaID ReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_LocFisicaID LocFisicaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ActivoGrupoID ActivoGrupoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ActivoClaseID ActivoClaseID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ActivoTipoID ActivoTipoID { get; set; }
        
        [DataMember]
        [AllowNull]
        public UDTSQL_int VidaUtilLOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_int VidaUtilIFRS { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_int VidaUtilUSG { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoDepreLOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoDepreIFRS { get; set; } 

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoDepreUSG { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorSalvamentoLOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorSalvamentoUSG { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorSalvamentoIFRS { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorSalvamentoIFRSUS { get; set; }
        

        #region Campos de Extras  Valor

        [DataMember]
        public UDT_Valor CostoLOC { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoEXT { get; set; }

        #endregion

       
        #endregion

    }   
}
