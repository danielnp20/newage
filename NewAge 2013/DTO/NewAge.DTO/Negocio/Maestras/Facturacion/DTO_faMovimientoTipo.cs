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
    /// Models DTO_faMovimientoTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faMovimientoTipo : DTO_MasterBasic
    {
        #region DTO_faMovimientoTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faMovimientoTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString();
                }

                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.TipoMovimiento.Value = Convert.ToByte(dr["TipoMovimiento"]);
                this.Transaccion.Value = Convert.ToByte(dr["Transaccion"]);
                this.SubtotalesInd.Value = Convert.ToBoolean(dr["SubtotalesInd"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faMovimientoTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.TipoMovimiento = new UDTSQL_tinyint();
            this.Transaccion = new UDTSQL_tinyint();
            this.SubtotalesInd = new UDT_SiNo();
        }

        public DTO_faMovimientoTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_faMovimientoTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoMovimiento { get; set; }

        [DataMember]
        public UDTSQL_tinyint Transaccion { get; set; }

        [DataMember]
        public UDT_SiNo SubtotalesInd { get; set; }

    }
}


