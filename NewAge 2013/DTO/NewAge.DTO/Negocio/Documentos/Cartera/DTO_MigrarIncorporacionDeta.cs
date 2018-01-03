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
    /// Class comprobante para aprobacion:
    /// Models DTO_MigrarIncorporacionDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigrarIncorporacionDeta
    {
        #region DTO_MigrarIncorporacionDeta

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public DTO_MigrarIncorporacionDeta(IDataReader dr)
        {
            InitCols();
            try
            {
              
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigrarIncorporacionDeta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.PagaduriaID = new UDT_PagaduriaID();
            this.PagaduriaDesc = new UDT_Descriptivo();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.Libranza = new UDT_LibranzaID();           
            this.PlazoINC = new UDTSQL_smallint();
            this.ValorCuota = new UDT_Valor();
            this.VlrMora = new UDT_Valor();
            this.VlrIncrMora = new UDT_Valor();
            this.VlrDtoMes = new UDT_Valor();
            this.Observacion = new UDT_DescripTExt();

            this.NovedadIncorporaID = new UDTSQL_char(5);
            this.TipoNovedad = new UDTSQL_tinyint();
            this.Aprobado = new UDT_SiNo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.PeriodoNomina = new UDT_PeriodoID();
            this.FechaCuota1 = new UDTSQL_smalldatetime();
            this.FechaNovedad = new UDTSQL_datetime();
            this.ValorNomina = new UDT_Valor();

        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }      

        [DataMember]
        public UDT_Descriptivo PagaduriaDesc { get; set; }   

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }        

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }          

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }         

        [DataMember]
        public UDTSQL_smallint PlazoINC { get; set; }

        [DataMember]
        public UDT_Valor ValorCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrMora { get; set; }               

        [DataMember]
        public UDT_Valor VlrIncrMora { get; set; }            

        [DataMember]
        public UDT_Valor VlrDtoMes { get; set; }             

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        //No importables

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PeriodoID PeriodoNomina { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char NovedadIncorporaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint TipoNovedad { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_datetime FechaNovedad { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaCuota1 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorNomina { get; set; }

        #endregion

    }
}
