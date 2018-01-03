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
    /// 
    /// Models DTO_glDocumentoChequeoLista
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(DTO_glDocumentoChequeoLista))]
    public class DTO_glDocumentoChequeoLista
    {
        #region DTO_glDocumentoChequeoLista

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDocumentoChequeoLista(IDataReader dr)
        {
            InitCols();
            try
            {

                //this.ActividadFlujoDesc.Value = dr["ActividadFlujoDesc"].ToString();
                //this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Descripcion.Value = dr["Descripcion"].ToString();
                this.IncluidoInd.Value = Convert.ToBoolean(dr["IncluidoInd"]);
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDocumentoChequeoLista()
        {
            InitCols();
            this.IncluidoInd.Value = false;
            this.IncluidoConyugeInd.Value = false;
            this.IncluidoCodeudor1Ind.Value = false;
            this.IncluidoCodeudor2Ind.Value = false;
            this.IncluidoCodeudor3Ind.Value = false;

            this.IncluidoDeudor.Value = false;
            this.IncluidoConyuge.Value = false;
            this.IncluidoCodeudor1.Value = false;
            this.IncluidoCodeudor2.Value = false;
            this.IncluidoCodeudor3.Value = false;

        }

        public void InitCols()
        {
            //Inicializa las columnas
            this.NumeroDoc = new UDT_Consecutivo();
            this.ActividadFlujoID = new UDT_ActividadFlujoID();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.Descripcion = new UDTSQL_char(200);
            this.IncluidoInd = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
            //Adicionales
            this.IncluidoConyugeInd = new UDT_SiNo();
            this.IncluidoCodeudor1Ind = new UDT_SiNo();
            this.IncluidoCodeudor2Ind = new UDT_SiNo();
            this.IncluidoCodeudor3Ind = new UDT_SiNo();
            this.IncluidoDeudor = new UDT_SiNo();
            this.IncluidoConyuge = new UDT_SiNo();
            this.IncluidoCodeudor1 = new UDT_SiNo();
            this.IncluidoCodeudor2 = new UDT_SiNo();
            this.IncluidoCodeudor3 = new UDT_SiNo();
            this.EmpleadoInd = new UDT_SiNo();
            this.PrestServiciosInd = new UDT_SiNo();
            this.TransportadorInd = new UDT_SiNo();
            this.IndependienteInd = new UDT_SiNo();
            this.PensionadoInd = new UDT_SiNo();
            this.ExcluyeCodInd = new UDT_SiNo();
            this.Incluye = new UDTSQL_char(20);
        }
        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ActividadFlujoID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDTSQL_char Descripcion { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoInd { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Adicionales

        [DataMember]
        public UDT_SiNo IncluidoConyugeInd { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoCodeudor1Ind { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoCodeudor2Ind { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoCodeudor3Ind { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoDeudor { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoConyuge { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoCodeudor1 { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoCodeudor2 { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoCodeudor3 { get; set; }

        [DataMember]
        public UDT_SiNo EmpleadoInd { get; set; }

        [DataMember]
        public UDT_SiNo PrestServiciosInd { get; set; }

        [DataMember]
        public UDT_SiNo TransportadorInd { get; set; }

        [DataMember]
        public UDT_SiNo IndependienteInd { get; set; }

        [DataMember]
        public UDT_SiNo PensionadoInd { get; set; }

        [DataMember]
        public UDT_SiNo ExcluyeCodInd { get; set; }

        [DataMember]
        public UDTSQL_char Incluye { get; set; }

    }
}
