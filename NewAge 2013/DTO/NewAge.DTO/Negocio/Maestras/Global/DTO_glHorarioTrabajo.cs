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
    /// Models DTO_glHorarioTrabajo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glHorarioTrabajo : DTO_MasterBasic 
    {
        #region DTO_glHorarioTrabajo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// </summary>
        /// <param name="?"></param>
        public DTO_glHorarioTrabajo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.EntradaHora.Value = Convert.ToInt32(dr["EntradaHora"]);
                this.EntradaMinutos.Value = Convert.ToInt32(dr["EntradaMinutos"]);
                this.SalidaHora.Value = Convert.ToInt32(dr["SalidaHora"]);
                this.SalidaMinutos.Value = Convert.ToInt32(dr["SalidaMinutos"]);
                this.Almuerzo.Value = Convert.ToByte(dr["Almuerzo"]);  
            }
            catch (Exception e)
            {
              throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glHorarioTrabajo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EntradaHora = new UDTSQL_int();
            this.EntradaMinutos = new UDTSQL_int();
            this.SalidaHora = new UDTSQL_int();
            this.SalidaMinutos = new UDTSQL_int();
            this.Almuerzo = new UDTSQL_tinyint(); 
        }

        public DTO_glHorarioTrabajo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glHorarioTrabajo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_int EntradaHora { get; set; }

        [DataMember]
        public UDTSQL_int EntradaMinutos { get; set; }

        [DataMember]
        public UDTSQL_int SalidaHora { get; set; }

        [DataMember]
        public UDTSQL_int SalidaMinutos { get; set; }

        [DataMember]
        public UDTSQL_tinyint Almuerzo { get; set; }    
    }

}
