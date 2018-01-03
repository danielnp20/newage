using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;

namespace NewAge.DTO.Resultados
{
    /// <summary>
    /// Clase que contiene la configuración inicial enviada al cliente para saber en que modo se encuantra
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_aplConfiguracionLan
    {
        public DTO_aplConfiguracionLan(SqlDataReader dr)
        {
            this.LanNombre = Convert.ToString(dr["LanNombre"]);
            this.CadenaConn = Convert.ToString(dr["CadenaConn"]);
            this.CadenaConnLogger = Convert.ToString(dr["CadenaConnLogger"]);
            this.IndRemoto = Convert.ToBoolean(dr["IndRemoto"]);
        }

        private DTO_aplConfiguracionLan()
        {
            ;
        }

        public static DTO_aplConfiguracionLan GetForceWCFDummy()
        {
            DTO_aplConfiguracionLan dummy = new DTO_aplConfiguracionLan();
            dummy.LanNombre = "Remoto Sin LAN (más lento)";
            dummy.IndRemoto = true;
            dummy.ForceWCF = true;
            return dummy;
        }

        /// <summary>
        /// IP de la base de datos a la cual se debe conectar
        /// </summary>
        [DataMember]
        public string LanNombre = string.Empty;

        /// <summary>
        /// IP de la base de datos a la cual se debe conectar
        /// </summary>
        [DataMember]
        public string CadenaConn = string.Empty;

        /// <summary>
        /// IP de la base de datos a la cual se debe conectar para las tablas temporales
        /// </summary>
        [DataMember]
        public string CadenaConnLogger = string.Empty;

        /// <summary>
        /// Indica si se encuantra en una lan remota y debe usar el modelo de broker.
        /// </summary>
        [DataMember]
        public bool IndRemoto=true;

        /// <summary>
        /// Indica que se debe hacer toda la comunicación a través de WCF porque no hay conexión LAN
        /// </summary>
        [DataMember]
        public bool ForceWCF = false;

        public override string ToString()
        {
            return this.LanNombre;
        }
    }
}
