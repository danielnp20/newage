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
    /// Models DTO_ocCuentaGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ocCuentaGrupo : DTO_MasterBasic
    {
        #region ocConceptoCuenta
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ocCuentaGrupo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.CuentaOrigPesosDesc.Value = dr["CuentaOrigPesosDesc"].ToString();
                    this.CuentaOrigDolarDesc.Value = dr["CuentaOrigDolarDesc"].ToString();
                }
                this.CuentaOrigPesos.Value = dr["CuentaOrigPesos"].ToString();
                this.CuentaOrigDolar.Value = dr["CuentaOrigDolar"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ocCuentaGrupo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaOrigPesos = new UDT_BasicID();
            this.CuentaOrigPesosDesc = new UDT_Descriptivo();
            this.CuentaOrigDolar = new UDT_BasicID();
            this.CuentaOrigDolarDesc = new UDT_Descriptivo();
        }

        public DTO_ocCuentaGrupo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ocCuentaGrupo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID CuentaOrigPesos { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaOrigPesosDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaOrigDolar { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaOrigDolarDesc { get; set; }

    }
}
