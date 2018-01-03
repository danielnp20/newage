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
    /// Models DTO_acDotacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acDotacion : DTO_MasterComplex
    {
        #region DTO_acDotacion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acDotacion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.inReferenciaDesc.Value = dr["inReferenciaDesc"].ToString();
                    this.ReferenciaEquipoDesc.Value = dr["ReferenciaEquipoDesc"].ToString();
                }
                
                this.ReferenciaEquipo.Value = dr["ReferenciaEquipo"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.CantidadUNI.Value = Convert.ToDecimal(dr["CantidadUNI"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acDotacion()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ReferenciaEquipo = new UDT_BasicID();
            this.ReferenciaEquipoDesc = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_BasicID();
            this.inReferenciaDesc = new UDT_Descriptivo();
            this.CantidadUNI = new UDT_Cantidad();
        }

        public DTO_acDotacion(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_acDotacion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ReferenciaEquipo { get; set; }

        [DataMember]
        public UDT_Descriptivo ReferenciaEquipoDesc { get; set; }

        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo inReferenciaDesc { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadUNI { get; set; }


    }
  }