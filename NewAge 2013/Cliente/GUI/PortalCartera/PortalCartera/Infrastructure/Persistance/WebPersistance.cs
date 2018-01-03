using NewAge.DTO.Negocio;
using NewAge.Web.Common;
using NewAge.Web.Common.StorageObjects;
using System.Configuration;
using System.Data.SqlClient;

namespace NewAge.Cliente.GUI.PortalCartera.Infrastructure
{
    /// <summary>
    /// Class WebPersistance: In charge of web persistance
    /// </summary>
    public class WebPersistance
    {
        /// <summary>
        /// Persistance
        /// </summary>
        private readonly dynamic _persistance;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public WebPersistance()
        {
            _persistance = new AbstractSession(new WebStorage());
            ConnectionsManager.ConnectionString = ConfigurationManager.AppSettings["Server.DB.Connection"];
        }

        #region Public properties

        /// <summary>
        /// Empresa en la que se esta trabajando
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                var conn = ConnectionsManager.ADO_ConnectDB();
                return conn; 
            }
        }

        /// <summary>
        /// Empresa en la que se esta trabajando
        /// </summary>
        public DTO_glEmpresa Empresa
        {
            get { return _persistance.Empresa; }
            set { _persistance.Empresa = value; }
        }

        /// <summary>
        /// Correo del usuario
        /// </summary>
        public string UserMail
        {
            get { return _persistance.UserMail; }
            set { _persistance.UserMail = value; }
        }

        /// <summary>
        /// Nombre del usuario
        /// </summary>
        public string UserName
        {
            get { return _persistance.UserName; }
            set { _persistance.UserName = value; }
        }

        /// <summary>
        /// Cédula del usuario
        /// </summary>
        public string UserId
        {
            get { return _persistance.UserId; }
            set { _persistance.UserId = value; }
        }


        #endregion
    }
}