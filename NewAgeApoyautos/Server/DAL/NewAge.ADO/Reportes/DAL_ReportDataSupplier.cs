using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.ReportesComunes;
using NewAge.Librerias.Project;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;

namespace NewAge.ADO.Reportes
{
    public class DAL_ReportDataSupplier : DAL_Base, CommonReportDataSupplier
    {
        #region Propiedades
        public string NombreEmpresa
        {
            get;
            set;
        }

        public byte[] LogoEmpresa
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        private string _idioma
        {
            get;
            set;
        }

        public string NitEmpresa
        {
            get;
            set;
        }

        private Dictionary<LanguageTypes, Dictionary<string, string>> rsx = new Dictionary<LanguageTypes, Dictionary<string, string>>();
	    #endregion


        public DAL_ReportDataSupplier(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string idioma, string loggerConn)
            : base(c, tx, empresa, userId, loggerConn)
        {
            this._idioma = idioma;
        }

        #region Implementacion de CommonReportDataSupplier
        public string GetNitEmpresa()
        {
            return this.NitEmpresa;
        }

        public string GetNombreEmpresa()
        {
            return this.NombreEmpresa;
        }

        public byte[] GetLogoEmpresa()
        {
            return this.LogoEmpresa;
        }

        public string GetUserName()
        {
            return this.UserName;
        }

        public string GetResource(LanguageTypes t, string v)
        {
            DAL_aplIdiomaTraduccion dal = new DAL_aplIdiomaTraduccion(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            Dictionary<string, string> d;
            if (rsx.TryGetValue(t, out d))
            {
                string res="";
                if (d.TryGetValue(v, out res))
                {
                    return res;
                }
                else
                {
                    Dictionary<string, string> temp = dal.DAL_aplIdiomaTraduccion_GetRsxByKeysDict(this._idioma, t, new List<string>() { v });
                    if (temp.TryGetValue(v, out res))
                    {
                        d.Add(v, res);
                        return res;
                    }
                }
            }
            else
            {
                d = dal.DAL_aplIdiomaTraduccion_GetRsxByKeysDict(this._idioma, t, new List<string>() { v });
                rsx.Add(t, d);
                string res = "";
                if (d.TryGetValue(v, out res))
                {
                    return res;
                }
            }

            return v;

        }

        #endregion

    }
}
