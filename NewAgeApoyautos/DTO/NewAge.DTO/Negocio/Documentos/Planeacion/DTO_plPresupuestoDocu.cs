using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_plPresupuestoDocu
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plPresupuestoDocu()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_plPresupuestoDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);              
                this.NumeroDocPresup.Value = Convert.ToInt32(dr["NumeroDocPresup"]);
                if(!string.IsNullOrEmpty(dr["NumeroDocOrdTrabajo"].ToString()))
                    this.NumeroDocOrdTrabajo.Value = Convert.ToInt32(dr["NumeroDocOrdTrabajo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDocPresup = new UDT_Consecutivo();
            this.NumeroDocOrdTrabajo = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades


        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocPresup { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocOrdTrabajo { get; set; }

        #endregion
    }
}
