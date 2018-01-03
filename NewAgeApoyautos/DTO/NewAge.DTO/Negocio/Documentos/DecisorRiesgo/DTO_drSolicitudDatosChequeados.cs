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
    /// Models DTO_drSolicitudDatosChequeados
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drSolicitudDatosChequeados
    {
        #region drSolicitudDatosChequeados

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drSolicitudDatosChequeados(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value=Convert.ToInt32(dr["NumeroDoc"]);
                this.Version.Value = Convert.ToByte(dr["Version"]);
                this.TipoPersona.Value = Convert.ToByte(dr["TipoPersona"]);
                if (!string.IsNullOrWhiteSpace(dr["ChequeadoInd"].ToString()))
                    this.ChequeadoInd.Value = Convert.ToBoolean(dr["ChequeadoInd"]);
                else
                    this.ChequeadoInd.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["VerficadoInd"].ToString()))
                    this.VerficadoInd.Value = Convert.ToBoolean(dr["VerficadoInd"]);
                else
                    this.VerficadoInd.Value = false;

                this.NroRegistro.Value = Convert.ToInt32(dr["NroRegistro"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_drSolicitudDatosChequeados()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {   
            this.NumeroDoc=new UDT_Consecutivo();
            this.Version=new UDTSQL_int();
            this.TipoPersona = new UDTSQL_tinyint();
            this.NroRegistro = new UDT_Consecutivo();
            this.ChequeadoInd = new UDT_SiNo();
            this.VerficadoInd = new UDT_SiNo();
            this.Consecutivo = new UDT_Consecutivo();
    
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDTSQL_int Version { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoPersona { get; set; }

        [DataMember]
        public UDT_Consecutivo NroRegistro { get; set; }

        [DataMember]
        public UDT_SiNo ChequeadoInd { get; set; }

        [DataMember]
        public UDT_SiNo VerficadoInd { get; set; }


        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }


        #endregion
    }
}
