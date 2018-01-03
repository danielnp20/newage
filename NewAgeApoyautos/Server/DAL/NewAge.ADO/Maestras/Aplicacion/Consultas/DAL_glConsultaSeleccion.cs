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
    public class DAL_glConsultaSeleccion : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glConsultaSeleccion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los aplForma
        /// </summary>
        /// <returns>Lista de glConsultas</returns>
        public IEnumerable<DTO_glConsultaSeleccion> DAL_glConsultaSeleccion_GetAll(DTO_glConsultaSeleccion glConsultaSeleccion, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //var parameters = new List<SqlParameter>();

                List<DTO_glConsultaSeleccion> selecciones = new List<DTO_glConsultaSeleccion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("SELECT  glConsultaSeleccionID, glConsultaID, CampoFisico, CampoDesc, Idx, ");
                sql.Append("        Tipo, OrdenIdx, OrdenTipo, GroupBy ");
                sql.Append(" FROM   glConsultaSeleccion with(nolock)");
                sql.Append(whereBuilder);
                //sql.Append(this.DAL_glConsultaSeleccion_WhereBuilder(glConsultaSeleccion, false, parameters));

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
                    var seleccion = new DTO_glConsultaSeleccion(dr);

                    selecciones.Add(seleccion);
                }
                dr.Close();

                return selecciones;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaSeleccion_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="glConsultaSeleccion">Informacion del filtro de la consulta</param>
        /// <param name="whereBuilder">condiciones para la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public DTO_glConsultaSeleccion DAL_glConsultaSeleccion_Get(DTO_glConsultaSeleccion glConsultaSeleccion, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //var parameters = new List<SqlParameter>();

                DTO_glConsultaSeleccion seleccion = new DTO_glConsultaSeleccion();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("SELECT  glConsultaSeleccionID, glConsultaID, CampoFisico, CampoDesc, Idx, ");
                sql.Append("        Tipo, OrdenIdx, OrdenTipo, GroupBy ");
                sql.Append(" FROM    glConsultaSeleccion with(nolock)");
                sql.Append(whereBuilder);
                //sql.Append(this.DAL_glConsultaSeleccion_WhereBuilder(glConsultaSeleccion, false, parameters));

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
                    seleccion = new DTO_glConsultaSeleccion(dr);
                }
                dr.Close();

                return seleccion;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaSeleccion_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Adicionar una nueva variante
        /// </summary>
        /// <param name="glConsultaSeleccion">Variante a adicionar</param>
        /// <returns>Variante adicionada</returns>
        public DTO_glConsultaSeleccion DAL_glConsultaSeleccion_Add(DTO_glConsultaSeleccion glConsultaSeleccion)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();

                var seleccion = new DTO_glConsultaSeleccion();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("INSERT INTO glConsultaSeleccion(glConsultaID, CampoFisico, CampoDesc, Idx, Tipo, OrdenIdx, OrdenTipo, GroupBy)");
                sql.Append(" VALUES (@glConsultaID, @CampoFisico, @CampoDesc, @Idx, @Tipo, @OrdenIdx, @OrdenTipo, @GroupBy) ");
                sql.Append(" SET @ID = SCOPE_IDENTITY()");

                mySqlCommand.Parameters.Add("@glConsultaID", SqlDbType.Int);
                mySqlCommand.Parameters["@glConsultaID"].Value = glConsultaSeleccion.glConsultaID;

                mySqlCommand.Parameters.Add("@CampoFisico", SqlDbType.VarChar);
                mySqlCommand.Parameters["@CampoFisico"].Value = glConsultaSeleccion.CampoFisico;

                mySqlCommand.Parameters.Add("@CampoDesc", SqlDbType.VarChar);
                mySqlCommand.Parameters["@CampoDesc"].Value = glConsultaSeleccion.CampoDesc;

                mySqlCommand.Parameters.Add("@Idx", SqlDbType.Int);
                mySqlCommand.Parameters["@Idx"].Value = glConsultaSeleccion.Idx;

                mySqlCommand.Parameters.Add("@Tipo", SqlDbType.VarChar);
                mySqlCommand.Parameters["@Tipo"].Value = glConsultaSeleccion.Tipo;

                mySqlCommand.Parameters.Add("@OrdenIdx", SqlDbType.Int);
                mySqlCommand.Parameters["@OrdenIdx"].Value = glConsultaSeleccion.OrdenIdx;

                mySqlCommand.Parameters.Add("@OrdenTipo", SqlDbType.VarChar);
                mySqlCommand.Parameters["@OrdenTipo"].Value = glConsultaSeleccion.OrdenTipo;

                mySqlCommand.Parameters.Add("@GroupBy", SqlDbType.Bit);
                mySqlCommand.Parameters["@GroupBy"].Value = glConsultaSeleccion.GroupBy;

                mySqlCommand.Parameters.Add("@ID", SqlDbType.Int);
                mySqlCommand.Parameters["@ID"].Direction = ParameterDirection.Output;

                mySqlCommand.CommandText = sql.ToString();

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                seleccion.glConsultaSeleccionID = Convert.ToInt32(mySqlCommand.Parameters["@ID"].Value);

                dr.Close();

                return seleccion;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaSeleccion_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar una  variante
        /// </summary>
        /// <param name="glConsultaSeleccion">Variante a adicionar</param>
        /// <param name="UpdateSetBuilder">condiciones para la consulta</param>
        /// <returns>Variante actualizada</returns>
        public DTO_glConsultaSeleccion DAL_glConsultaSeleccion_Update(DTO_glConsultaSeleccion glConsultaSeleccion, string UpdateSetBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //var parameters = new List<SqlParameter>();

                var variante = new DTO_glConsulta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("UPDATE  glConsultaSeleccion");
                sql.Append(UpdateSetBuilder);
                //sql.Append(this.DAL_glConsultaSeleccion_UpdateSetBuilder(glConsultaSeleccion, parameters));

                sql.Append(" WHERE glConsultaSeleccionID = @glConsultaSeleccionID ");
                
                mySqlCommand.CommandText = sql.ToString();

                var parameterId = new SqlParameter("@glConsultaSeleccionID", SqlDbType.Int);
                parameterId.Value = glConsultaSeleccion.glConsultaID;

                parameters.Add(parameterId);

                parameters.ForEach
                    (
                        param =>
                        {
                            mySqlCommand.Parameters.Add(param);
                        }
                    );


                mySqlCommand.ExecuteNonQuery();


                return glConsultaSeleccion;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaSeleccion_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Eliminar una  variante
        /// </summary>
        /// <param name="glConsultaSeleccion">Variante a adicionar</param>
        /// <param name="whereBuilder">condiciones para la consulta</param>
        /// <returns>Variante eliminada</returns>
        public void DAL_glConsultaSeleccion_Delete(DTO_glConsultaSeleccion glConsultaSeleccion, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //var parameters = new List<SqlParameter>();

                bool onlyId = true;
                if (glConsultaSeleccion.glConsultaSeleccionID == null)
                {
                    onlyId = false;
                }

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("DELETE FROM  glConsultaSeleccion");
                sql.Append(whereBuilder);
                //sql.Append(this.DAL_glConsultaSeleccion_WhereBuilder(glConsultaSeleccion, parameters));

                mySqlCommand.CommandText = sql.ToString();
                parameters.ForEach
                    (
                        param =>
                        {
                            mySqlCommand.Parameters.Add(param);
                        }
                    );

                
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaSeleccion_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Eliminar una  variante
        /// </summary>
        /// <param name="queryID">identificador de la consulta</param>
        /// <returns>Variante eliminada</returns>
        public void DAL_glConsultaSeleccion_DeleteByQueryID(int queryID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "DELETE FROM glConsultaSeleccion where glConsultaID = @glConsultaID";
                mySqlCommand.Parameters.Add("@glConsultaID", SqlDbType.Int);
                mySqlCommand.Parameters["@glConsultaID"].Value = queryID;

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsultaSeleccion_DeleteByQueryID");
                throw exception;
            }
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las variantes
        /// </summary>
        /// <param name="glConsultaSeleccion">Filtros de la variantes</param>
        /// <param name="onlyId">Bandera que indica si es solo el ID</param>
        /// <param name="parameters">Parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        public string DAL_glConsultaSeleccion_WhereBuilder(DTO_glConsultaSeleccion glConsultaSeleccion, bool onlyId, List<SqlParameter> parameters)
        {
            var where = new StringBuilder();

            if (glConsultaSeleccion.glConsultaSeleccionID != null)
            {
                if (where.ToString().Length == 0)
                {
                    where.Append(" WHERE ");
                }
                else
                {
                    where.Append(" AND ");
                }

                where.Append(" glConsultaSeleccionID = @glConsultaSeleccionID");

                var parameter = new SqlParameter("@glConsultaSeleccionID", SqlDbType.Int);
                parameter.Value = glConsultaSeleccion.glConsultaSeleccionID;

                parameters.Add(parameter);
            }

            if (!onlyId)
            {
                if (glConsultaSeleccion.glConsultaID != 0)
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
                    parameter.Value = glConsultaSeleccion.glConsultaID;

                    parameters.Add(parameter);
                }

                if (!String.IsNullOrEmpty(glConsultaSeleccion.CampoFisico))
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
                    parameter.Value = glConsultaSeleccion.CampoFisico;

                    parameters.Add(parameter);
                }


                if (!String.IsNullOrEmpty(glConsultaSeleccion.CampoFisico))
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
                    parameter.Value = glConsultaSeleccion.CampoFisico;

                    parameters.Add(parameter);
                }
            }
            return where.ToString();
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las variantes
        /// </summary>
        /// <param name="glConsultaSeleccion">Filtros de la variantes</param>
        /// <param name="onlyId">Bandera que indica si es solo el ID</param>
        /// <param name="parameters">Parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        public string DAL_glConsultaSeleccion_UpdateSetBuilder(DTO_glConsultaSeleccion glConsultaSeleccion, List<SqlParameter> parameters)
        {
            var set = new StringBuilder();

            if (!String.IsNullOrEmpty(glConsultaSeleccion.CampoDesc))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" CampoDesc = @CampoDesc, ");

                var parameter = new SqlParameter("@CampoDesc", SqlDbType.NVarChar, 55);
                parameter.Value = glConsultaSeleccion.CampoDesc;

                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(glConsultaSeleccion.CampoFisico))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }

                set.Append(" CampoFisico = @CampoFisico ");

                var parameter = new SqlParameter("@CampoFisico", SqlDbType.NVarChar, 255);
                parameter.Value = glConsultaSeleccion.CampoFisico;

                parameters.Add(parameter);
            }

            if (glConsultaSeleccion.Idx != 0)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" Idx = @Idx, ");

                var parameter = new SqlParameter("@Idx", SqlDbType.Int);
                parameter.Value = glConsultaSeleccion.Idx;

                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(glConsultaSeleccion.Tipo))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" Tipo = @Tipo, ");

                var parameter = new SqlParameter("@Tipo", SqlDbType.NVarChar, 55);
                parameter.Value = glConsultaSeleccion.Tipo;

                parameters.Add(parameter);
            }

            if (glConsultaSeleccion.OrdenIdx != 0)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" OrdenIdx = @OrdenIdx, ");

                var parameter = new SqlParameter("@OrdenIdx", SqlDbType.Int);
                parameter.Value = glConsultaSeleccion.Idx;

                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(glConsultaSeleccion.OrdenTipo))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" OrdenTipo = @OrdenTipo, ");

                var parameter = new SqlParameter("@OrdenTipo", SqlDbType.NVarChar, 55);
                parameter.Value = glConsultaSeleccion.Tipo;

                parameters.Add(parameter);
            }

            if (glConsultaSeleccion.GroupBy != null)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" GroupBy = @GroupBy, ");

                var parameter = new SqlParameter("@GroupBy", SqlDbType.Bit);
                parameter.Value = glConsultaSeleccion.GroupBy;

                parameters.Add(parameter);
            }

            return set.ToString();
        }
    }
}
