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
    /// Models DTO_pySolServicio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pySolServicioAprob
    {
        #region DTO_pySolServicioAprob

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pySolServicioAprob(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.PrefijoID.Value = Convert.ToString(dr["PrefijoID"]);
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.Observaciones.Value = Convert.ToString(dr["Observacion"]);
                this.DocumentoDesc.Value = Convert.ToString(dr["DocumentoDesc"]);
                this.PeriodoDoc.Value = Convert.ToDateTime(dr["PeriodoDoc"]);
                this.Aprobado.Value = false;
                this.Rechazado.Value = false;                             
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
        public DTO_pySolServicioAprob()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.Observaciones = new UDT_DescripTBase();
            this.DocumentoDesc = new UDT_DescripTBase();
            this.PeriodoDoc = new UDT_PeriodoID();
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
        }

        #endregion
                
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_DescripTBase Observaciones { get; set; }

        [DataMember]
        public UDT_DescripTBase DocumentoDesc { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoDoc { get; set; }
                

        #region Agrupaciones
        

        #endregion

        #region Otras
     
        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        #endregion

    }
}
