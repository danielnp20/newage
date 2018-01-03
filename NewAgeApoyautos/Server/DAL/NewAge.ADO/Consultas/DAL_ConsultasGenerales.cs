using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Configuration;
using System.Reflection;
using SentenceTransformer;
using NewAge.DTO.Reportes;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_ConsultasGenerales
    /// </summary>
    public class DAL_ConsultasGenerales : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ConsultasGenerales(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Consultas generales
        /// </summary>
        /// <param name="vista">Nombre de la vista</param>
        /// <param name="dtoType">Tipo de DTO</param>
        /// <param name="consulta">Consulta con filtros</param>
        /// <returns></returns>
        public DataTable DAL_ConsultasGenerales_Consultar(string vista, Type dtoType, DTO_glConsulta consulta)
        {
            try
            {
                DataTable results = new DataTable();

                string selectedFields = "*";
                if(consulta.Selecciones.Count > 0)
                {
                    selectedFields = "";// "EmpresaID, NumeroDoc";
                    for (int i = 0; i < consulta.Selecciones.Count; ++i)
                    {
                        if (i != 0)
                            selectedFields += ",";

                        selectedFields += consulta.Selecciones[i].CampoFisico;
                    }
                } 

                string where = " WHERE EmpresaID = @EmpresaID";
                string whereConsulta = (consulta == null || consulta.Filtros == null) ? string.Empty : Transformer.WhereSql(consulta.Filtros, dtoType);
                string query = "SELECT " + selectedFields + " FROM " + vista + where;
                if(!string.IsNullOrWhiteSpace(whereConsulta))
                    query += " AND " + whereConsulta;

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.CommandText =  query;
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                mySqlCommandSel.CommandTimeout = 5000;
                sqlAdapter.SelectCommand = mySqlCommandSel;
                sqlAdapter.Fill(results);

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ConsultasCartera_Consultar");
                return null;
            }
        }

    }
}

