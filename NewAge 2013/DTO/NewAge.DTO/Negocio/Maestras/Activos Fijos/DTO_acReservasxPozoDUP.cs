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
    /// Models DTO_acReservasxPozoDUP
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acReservasxPozoDUP : DTO_MasterComplex
    {
        #region DTO_acReservasxPozoDUP

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acReservasxPozoDUP(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.LocFisicaDesc.Value = dr["LocFisicaDesc"].ToString();
                }

                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.LocFisicaID.Value = dr["LocFisicaID"].ToString();
                this.ResProbable.Value = Convert.ToDecimal(dr["ResProbable"]);
                this.ReservaProbada.Value = Convert.ToDecimal(dr["ReservaProbada"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acReservasxPozoDUP()
            : base()
        {
            InitCols();
        }

        /// <summary>
        /// Construye el DTO_ a partir de una consulta hecha en la bd
        /// </summary>
        /// <param name="dr">IDataReader</param>
        public DTO_acReservasxPozoDUP(IDataReader dr)
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoID = new UDTSQL_smalldatetime();
            this.LocFisicaID = new UDT_BasicID();
            this.LocFisicaDesc = new UDT_Descriptivo();
            this.ResProbable = new UDT_Valor();
            this.ReservaProbada = new UDT_Valor();
        }

        public DTO_acReservasxPozoDUP(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_acReservasxPozoDUP(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_smalldatetime PeriodoID { get; set; }

        [DataMember]
        public UDT_BasicID LocFisicaID { get; set; }

        [DataMember]
        public UDT_Descriptivo LocFisicaDesc { get; set; }
        
        [DataMember]
        public UDT_Valor ResProbable { get; set; }

        [DataMember]
        public UDT_Valor ReservaProbada { get; set; }

    }
  }