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
    /// Models DTO_faFacturaTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faFacturaTipo : DTO_MasterBasic
    {
        #region DTO_faFacturaTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faFacturaTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString();
                    this.coDocumentoNCRDesc.Value = dr["coDocumentoNCRDesc"].ToString();
                }

                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.coDocumentoNCRID.Value = dr["coDocumentoNCRID"].ToString();
                this.FacturaModelo.Value = dr["FacturaModelo"].ToString();
                this.ObservacionENC.Value = dr["ObservacionENC"].ToString();
                this.ObservacionPIE.Value = dr["ObservacionPIE"].ToString();
                this.NotaCreditoInd.Value = Convert.ToBoolean(dr["NotaCreditoInd"]);
                this.TipoControlInventarios.Value = Convert.ToByte(dr["TipoControlInventarios"]);
            }
            catch (Exception e)
            {               
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faFacturaTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.coDocumentoNCRID = new UDT_BasicID();
            this.coDocumentoNCRDesc = new UDT_Descriptivo();
            this.FacturaModelo = new UDTSQL_char(50);
            this.ObservacionENC = new UDT_DescripTExt();
            this.ObservacionPIE = new UDT_DescripTExt();
            this.NotaCreditoInd = new UDT_SiNo();
            this.TipoControlInventarios = new UDTSQL_tinyint();
        }

        public DTO_faFacturaTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_faFacturaTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_SiNo NotaCreditoInd { get; set; }

        [DataMember]
        public UDTSQL_char FacturaModelo { get; set; }

        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo  coDocumentoDesc { get; set; }

        [DataMember]
        public UDT_BasicID coDocumentoNCRID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoNCRDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt ObservacionENC { get; set; }
            
        [DataMember]
        public UDT_DescripTExt ObservacionPIE { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoControlInventarios { get; set; }


    }
}

