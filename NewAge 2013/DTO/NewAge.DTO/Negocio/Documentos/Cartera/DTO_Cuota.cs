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
    public class DTO_Cuota
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_Cuota()
        {
            this.Componentes = new List<string>();
            this.ValoresComponentes = new List<int>();
        }

        #region Propiedades

        [DataMember]
        public int NumCuota
        {
            get;
            set;
        }

        [DataMember]
        public DateTime Fecha
        {
            get;
            set;
        }

        [DataMember]
        public int Capital
        {
            get;
            set;
        }

        [DataMember]
        public int Intereses
        {
            get;
            set;
        }

        [DataMember]
        public int Seguro
        {
            get;
            set;
        }

        [DataMember]
        public List<string> Componentes
        {
            get;
            set;
        }

        [DataMember]
        public List<int> ValoresComponentes
        {
            get;
            set;
        }

        [DataMember]
        public int SaldoCuota
        {
            get;
            set;
        }

        [DataMember]
        public int ValorCuota
        {
            get;
            set;
        }

        #endregion
    }
}
