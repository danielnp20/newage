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
    /// Models DTO_glProcedimientoFlujo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glProcedimientoFlujo : DTO_MasterComplex
    {
        #region DTO_glProcedimientoFlujo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glProcedimientoFlujo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ActividadHijaDesc.Value        = dr["ActividadHijaDesc"].ToString();
                    this.ActividadPadreDesc.Value       = dr["ActividadPadreDesc"].ToString();
                    this.ActividadHijaRechazoDesc.Value = dr["ActividadHijaRechazoDesc"].ToString();
                }
                this.ActividadPadre.Value       = dr["ActividadPadre"].ToString();
                this.ActividadHija.Value        = dr["ActividadHija"].ToString();
                this.ActividadHijaRechazo.Value = dr["ActividadHijaRechazo"].ToString();
                if (!string.IsNullOrEmpty(dr["DiasActivacion"].ToString()))
                    this.DiasActivacion.Value = Convert.ToDecimal(dr["DiasActivacion"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glProcedimientoFlujo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ActividadPadreDesc       = new UDT_Descriptivo();
            this.ActividadPadre           = new UDT_BasicID();
            this.ActividadHija            = new UDT_BasicID();
            this.ActividadHijaDesc        = new UDT_Descriptivo();
            this.ActividadHijaRechazo     = new UDT_BasicID();
            this.ActividadHijaRechazoDesc = new UDT_Descriptivo();
            this.DiasActivacion = new UDT_Cantidad();
        }

        public DTO_glProcedimientoFlujo(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_glProcedimientoFlujo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ActividadPadre { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadPadreDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActividadHija { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadHijaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActividadHijaRechazo { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadHijaRechazoDesc { get; set; }

        [DataMember]
        public UDT_Cantidad DiasActivacion { get; set; }
    }
}
