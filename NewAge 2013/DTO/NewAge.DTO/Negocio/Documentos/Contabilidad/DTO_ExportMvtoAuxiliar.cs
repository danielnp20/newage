using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class:
    /// Models DTO_ExportMvtoAuxiliar
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ExportMvtoAuxiliar
    {
        #region DTO_ExportMvtoAuxiliar

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ExportMvtoAuxiliar()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Fecha = new UDTSQL_smalldatetime();
            this.CuentaID = new UDT_CuentaID();
            this.TerceroID = new UDT_TerceroID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.Descriptivo = new UDT_DescripTBase();
            this.vlrMdaLoc = new UDT_Valor();
            this.vlrMdaExt = new UDT_Valor();
            this.DocumentoID = new UDT_Consecutivo();
            //descripciones
            //this.DocumentoDes = new UDT_Descriptivo();
            //this.CuentaDes = new UDT_Descriptivo();
            //this.TerceroDes = new UDT_Descriptivo();
            //this.CentroCtoDes = new UDT_Descriptivo();
            //this.ProyectoDes = new UDT_Descriptivo();
            //this.LineaPresDes = new UDT_Descriptivo();
            //this.ConceptoDes = new UDT_Descriptivo();
        }

        #endregion

        #region Propiedades
      
        [DataMember]
        [Filtrable]
        public UDTSQL_smalldatetime Fecha { get; set; }       
      
        [DataMember]
        [Filtrable]
        public UDT_Consecutivo DocumentoID { get; set; }

        [DataMember]
        [Filtrable]
        public string Comprobante { get; set; }       

        [DataMember]
        [Filtrable]
        public UDT_CuentaID CuentaID { get; set; }
        
        [DataMember]
        [Filtrable]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        [Filtrable]
        public string PrefDoc { get; set; }
       
        [DataMember]
        [Filtrable]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Valor vlrMdaLoc { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Valor vlrMdaExt { get; set; }

        //Descripciones
        //[DataMember]
        //[NotImportable]
        //public UDT_Descriptivo  DocumentoDes { get; set; }

        //[DataMember]
        //[NotImportable]
        //public UDT_Descriptivo CuentaDes { get; set; }

        //[DataMember]
        //[NotImportable]
        //public UDT_Descriptivo TerceroDes { get; set; }

        //[DataMember]
        //[NotImportable]
        //public UDT_Descriptivo CentroCtoDes { get; set; }

        //[DataMember]
        //[NotImportable]
        //public UDT_Descriptivo ProyectoDes { get; set; }

        //[DataMember]
        //[NotImportable]
        //public UDT_Descriptivo LineaPresDes { get; set; }

        //[DataMember]
        //[NotImportable]
        //public UDT_Descriptivo ConceptoDes { get; set; }

        #endregion
    }
}
