using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_glConsultaFiltro : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glConsultaFiltro(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="glConsultaFiltro">Informacion del filtro de la consulta</param>
        /// <param name="whereBuilder">condiciones para la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public IEnumerable<DTO_glConsultaFiltro> DAL_glConsultaFiltro_GetAll(DTO_glConsultaFiltro glConsultaFiltro, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //var parameters = new List<SqlParameter>();

                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("SELECT  glConsultaFiltroID, glConsultaFiltroGrupo, glConsultaID, CampoFisico, CampoDesc, Idx, ");
                sql.Append("        OperadorFiltro, ValorFiltro, OperadorSentencia, Idx");
                sql.Append(" FROM    glConsultaFiltro with(nolock)");
                sql.Append(whereBuilder);

                mySqlCommand.CommandText = sql.ToString();
                parameters.ForEach
                    (
                        param =>
                        {
                            mySqlCommand.Parameters.Add(param);
                        }
                    );

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var filtro = new DTO_glConsultaFiltro(dr);

                    filtros.Add(filtro);
                }
                dr.Close();

                return filtros;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaFiltro_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene una variante especifica
        /// </summary>
        /// <param name="glConsultaFiltro">Informacion de la variante</param>
        /// <param name="whereBuilder">condiciones para la consulta</param>
        /// <returns>Variante</returns>
        public DTO_glConsultaFiltro DAL_glConsultaFiltro_Get(DTO_glConsultaFiltro glConsultaFiltro, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //var parameters = new List<SqlParameter>();

                var filtro = new DTO_glConsultaFiltro();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("SELECT  glConsultaFiltroID, glConsultaFiltroGrupo, glConsultaID, CampoFisico, CampoDesc, Idx, ");
                sql.Append("        OperadorFiltro, ValorFiltro, OperadorSentencia, Idx ");
                sql.Append("FROM    glConsultaFiltro  with(nolock)");
                sql.Append(whereBuilder);

                mySqlCommand.CommandText = sql.ToString();
                parameters.ForEach
                    (
                        param =>
                        {
                            mySqlCommand.Parameters.Add(param);
                        }
                    );

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    filtro = new DTO_glConsultaFiltro(dr);
                }
                dr.Close();

                return filtro;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaFiltro_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Adicionar una nueva variante
        /// </summary>
        /// <param name="glConsultaFiltro">Variante a adicionar</param>
        /// <returns>Variante adicionada</returns>
        public DTO_glConsultaFiltro DAL_glConsultaFiltro_Add(DTO_glConsultaFiltro glConsultaFiltro)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();

                var filtro = new DTO_glConsultaFiltro();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("INSERT INTO glConsultaFiltro(glConsultaFiltroGrupo, glConsultaID, CampoFisico, CampoDesc, OperadorFiltro, ValorFiltro, OperadorSentencia, Idx)");
                sql.Append(" VALUES (@glConsultaFiltroGrupo, @glConsultaID, @CampoFisico, @CampoDesc, @OperadorFiltro, @ValorFiltro, @OperadorSentencia, @Idx) ");
                sql.Append(" SET @ID = SCOPE_IDENTITY() ");

                mySqlCommand.Parameters.Add("@glConsultaFiltroGrupo", SqlDbType.Int);
                mySqlCommand.Parameters["@glConsultaFiltroGrupo"].Value = glConsultaFiltro.glConsultaFiltroGrupo;

                mySqlCommand.Parameters.Add("@glConsultaID", SqlDbType.Int);
                mySqlCommand.Parameters["@glConsultaID"].Value = glConsultaFiltro.glConsultaID;

                mySqlCommand.Parameters.Add("@CampoFisico", SqlDbType.VarChar);
                mySqlCommand.Parameters["@CampoFisico"].Value = glConsultaFiltro.CampoFisico;

                mySqlCommand.Parameters.Add("@CampoDesc", SqlDbType.VarChar);
                mySqlCommand.Parameters["@CampoDesc"].Value = glConsultaFiltro.CampoDesc;

                mySqlCommand.Parameters.Add("@OperadorFiltro", SqlDbType.VarChar);
                mySqlCommand.Parameters["@OperadorFiltro"].Value = glConsultaFiltro.OperadorFiltro;

                mySqlCommand.Parameters.Add("@ValorFiltro", SqlDbType.VarChar);
                mySqlCommand.Parameters["@ValorFiltro"].Value = glConsultaFiltro.ValorFiltro;

                mySqlCommand.Parameters.Add("@OperadorSentencia", SqlDbType.VarChar);
                mySqlCommand.Parameters["@OperadorSentencia"].Value = glConsultaFiltro.OperadorSentencia;

                mySqlCommand.Parameters.Add("@Idx", SqlDbType.Int);
                mySqlCommand.Parameters["@Idx"].Value = glConsultaFiltro.Idx;

                mySqlCommand.Parameters.Add("@ID", SqlDbType.Int);
                mySqlCommand.Parameters["@ID"].Direction = ParameterDirection.Output;

                mySqlCommand.CommandText = sql.ToString();

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                filtro.glConsultaFiltroID = Convert.ToInt32(mySqlCommand.Parameters["@ID"].Value);

                dr.Close();

                return filtro;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaFiltro_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar una  variante
        /// </summary>
        /// <param name="glConsultaFiltro">Variante a adicionar</param>
        /// <param name="whereBuilder">condiciones para la consulta</param>
        /// <returns>Variante actualizada</returns>
        public DTO_glConsultaFiltro DAL_glConsultaFiltro_Update(DTO_glConsultaFiltro glConsultaFiltro, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                var variante = new DTO_glConsulta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("UPDATE  glConsultaFiltro");
                sql.Append(whereBuilder);
                sql.Append("WHERE glConsultaFiltroID = @glConsultaFiltroID ");

                mySqlCommand.CommandText = sql.ToString();

                var parameterId = new SqlParameter("@glConsultaFiltroID", SqlDbType.Int);
                parameterId.Value = glConsultaFiltro.glConsultaFiltroID;

                parameters.Add(parameterId);

                parameters.ForEach
                    (
                        param =>
                        {
                            mySqlCommand.Parameters.Add(param);
                        }
                    );

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                return glConsultaFiltro;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaFiltro_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Eliminar una  variante
        /// </summary>
        /// <param name="glConsultaFiltro">Variante a adicionar</param>
        /// <param name="whereBuilder">condiciones para la consulta</param>
        /// <returns>Variante eliminada</returns>
        public void DAL_glConsultaFiltro_Delete(DTO_glConsultaFiltro glConsultaFiltro, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
               // var parameters = new List<SqlParameter>();

                bool onlyId = true;
                if (glConsultaFiltro.glConsultaFiltroID == null)
                {
                    onlyId = false;
                }

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("DELETE FROM  glConsultaFiltro");
                sql.Append(whereBuilder);

                mySqlCommand.CommandText = sql.ToString();
                parameters.ForEach
                (
                    param =>
                    {
                        mySqlCommand.Parameters.Add(param);
                    }
                );

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaFiltro_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Eliminar una  variante
        /// </summary>
        /// <param name="queryID">identificador de la consulta</param>
        /// <returns>Variante eliminada</returns>
        public void DAL_glConsultaFiltro_DeleteByQueryID(int queryID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "DELETE FROM glConsultaFiltro where glConsultaID = @glConsultaID";
                mySqlCommand.Parameters.Add("@glConsultaID", SqlDbType.Int);
                mySqlCommand.Parameters["@glConsultaID"].Value = queryID;

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaFiltro_DeleteByQueryID");
                throw exception;
            }
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las variantes
        /// </summary>
        /// <param name="glConsultaFiltro">Filtros de la variantes</param>
        /// <param name="onlyId">Bandera que indica si es solo el ID</param>
        /// <param name="parameters">Parametros para la conssulta</param>
        /// <returns>String correspondiente al where</returns>
        public string DAL_glConsultaFiltro_WhereBuilder(DTO_glConsultaFiltro glConsultaFiltro, bool onlyId, List<SqlParameter> parameters)
        {
            var where = new StringBuilder();

            if (glConsultaFiltro.glConsultaFiltroID != null)
            {
                if (where.ToString().Length == 0)
                {
                    where.Append(" WHERE ");
                }
                else
                {
                    where.Append(" AND ");
                }

                where.Append(" glConsultaFiltroID = @glConsultaFiltroID");

                var parameter = new SqlParameter("@glConsultaFiltroID", SqlDbType.Int);
                parameter.Value = glConsultaFiltro.glConsultaFiltroID;

                parameters.Add(parameter);
            }

            if (!onlyId)
            {
                if (glConsultaFiltro.glConsultaID != 0)
                {
                    if (where.ToString().Length == 0)
                    {
                        where.Append(" WHERE ");
                    }
                    else
                    {
                        where.Append(" AND ");
                    }

                    where.Append(" glConsultaID = @glConsultaID");

                    var parameter = new SqlParameter("@glConsultaID", SqlDbType.Int);
                    parameter.Value = glConsultaFiltro.glConsultaID;

                    parameters.Add(parameter);
                }

                if (glConsultaFiltro.glConsultaFiltroGrupo != 0)
                {
                    if (where.ToString().Length == 0)
                    {
                        where.Append(" WHERE ");
                    }
                    else
                    {
                        where.Append(" AND ");
                    }

                    where.Append(" glConsultaFiltroGrupo = @glConsultaFiltroGrupo");

                    var parameter = new SqlParameter("@glConsultaFiltroGrupo", SqlDbType.Int);
                    parameter.Value = glConsultaFiltro.glConsultaFiltroGrupo;

                    parameters.Add(parameter);
                }

                if (!String.IsNullOrEmpty(glConsultaFiltro.CampoFisico))
                {
                    if (where.ToString().Length == 0)
                    {
                        where.Append(" WHERE ");
                    }
                    else
                    {
                        where.Append(" AND ");
                    }

                    where.Append(" CampoFisico LIKE '% @CampoFisico %'");

                    var parameter = new SqlParameter("@CampoFisico", SqlDbType.VarChar, 255);
                    parameter.Value = glConsultaFiltro.CampoFisico;

                    parameters.Add(parameter);
                }


                if (!String.IsNullOrEmpty(glConsultaFiltro.CampoFisico))
                {
                    if (where.ToString().Length == 0)
                    {
                        where.Append(" WHERE ");
                    }
                    else
                    {
                        where.Append(" AND ");
                    }

                    where.Append(" CampoDesc LIKE '% @CampoDesc %'");

                    var parameter = new SqlParameter("@CampoDesc", SqlDbType.VarChar);
                    parameter.Value = glConsultaFiltro.CampoFisico;

                    parameters.Add(parameter);
                }
            }
            return where.ToString();
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las variantes
        /// </summary>
        /// <param name="glConsultaFiltro">Filtros de la variantes</param>
        /// <param name="parameters">Parametros para la conssulta</param>
        /// <returns>String correspondiente al where</returns>
        public string DAL_glConsultaFiltro_UpdateSetBuilder(DTO_glConsultaFiltro glConsultaFiltro, List<SqlParameter> parameters)
        {
            var set = new StringBuilder();

            if (!String.IsNullOrEmpty(glConsultaFiltro.CampoDesc))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" CampoDesc = @CampoDesc, ");

                var parameter = new SqlParameter("@CampoDesc", SqlDbType.NVarChar, 55);
                parameter.Value = glConsultaFiltro.CampoDesc;

                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(glConsultaFiltro.CampoFisico))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }

                set.Append(" CampoFisico = @CampoFisico ");

                var parameter = new SqlParameter("@CampoFisico", SqlDbType.NVarChar, 255);
                parameter.Value = glConsultaFiltro.CampoFisico;

                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(glConsultaFiltro.OperadorFiltro))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" OperadorFiltro = @OperadorFiltro, ");

                var parameter = new SqlParameter("@OperadorFiltro", SqlDbType.NVarChar, 255);
                parameter.Value = glConsultaFiltro.OperadorFiltro;

                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(glConsultaFiltro.ValorFiltro))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" ValorFiltro = @ValorFiltro, ");

                var parameter = new SqlParameter("@ValorFiltro", SqlDbType.NVarChar);
                parameter.Value = glConsultaFiltro.ValorFiltro;

                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(glConsultaFiltro.OperadorSentencia))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" OperadorSentencia = @OperadorSentencia, ");

                var parameter = new SqlParameter("@OperadorSentencia", SqlDbType.NVarChar, 55);
                parameter.Value = glConsultaFiltro.OperadorSentencia;

                parameters.Add(parameter);
            }

            if (glConsultaFiltro.Idx != 0)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" Idx = @Idx, ");

                var parameter = new SqlParameter("@Idx", SqlDbType.Int);
                parameter.Value = glConsultaFiltro.Idx;

                parameters.Add(parameter);
            }

            return set.ToString();
        }

        #endregion
    }
}
