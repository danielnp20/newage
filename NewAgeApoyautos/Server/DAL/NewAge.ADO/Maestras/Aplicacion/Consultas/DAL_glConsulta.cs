using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    public class DAL_glConsulta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glConsulta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public IEnumerable<DTO_glConsulta> DAL_glConsulta_GetAll(DTO_glConsulta glConsulta, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //List<SqlParameter> dalParameters = (List<SqlParameter>)parameters;

                List<DTO_glConsulta> variantes = new List<DTO_glConsulta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                sql.Append("SELECT  v.glConsultaID, v.Nombre, v.DocumentoID, f.Descriptivo AS FormaDesc, v.seUsuarioID, u.UsuarioID, v.Seleccion, v.Filtro, v.Distincion, ");
                sql.Append("        v.Activo, v.CtrlVersion, v.FechaCreacion, v.UsuarioCreacion, v.FechaAct, v.UsuarioAct, v.Prefijada");
                sql.Append(" FROM    glConsulta v LEFT OUTER JOIN ");
                sql.Append("        glDocumento f ON f.DocumentoID = v.DocumentoID LEFT OUTER JOIN ");
                sql.Append("        seUsuario u ON u.ReplicaID = v.seUsuarioID ");
                sql.Append(whereBuilder);
                //sql.Append(this.DAL_glConsulta_WhereBuilder(glConsulta, false, parameters));

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
                    var variante = new DTO_glConsulta(dr);
                    variantes.Add(variante);
                }
                dr.Close();

                return variantes;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsulta_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el numero de variantes
        /// </summary>
        /// <param name="glConsulta">informacion de la consulta</param>
        /// <param name="whereBuilder">string de condiciones de la consulta</param>
        /// <returns>Lista de glConsultas</returns>
        public long DAL_glConsulta_Count(DTO_glConsulta glConsulta, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //var parameters = new List<SqlParameter>();

                int variantes = 0;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("SELECT  v.glConsultaID, v.Nombre, v.FormaID, f.Descriptivo AS FormaDesc, v.seUsuarioID, u.UsuarioID, v.Seleccion, v.Filtro, v.Distincion, ");
                sql.Append("        v.Activo, v.CtrlVersion, v.FechaCreacion, v.UsuarioCreacion, v.FechaAct, v.UsuarioAct, v.Prefijada");
                sql.Append(" FROM    glConsulta v LEFT OUTER JOIN ");
                sql.Append("        aplForma f ON f.FormaID = v.FormaID LEFT OUTER JOIN ");
                sql.Append("        seUsuario u ON u.ReplicaID = v.seUsuarioID ");
                sql.Append(whereBuilder);
                //sql.Append(this.DAL_glConsulta_WhereBuilder(glConsulta, false, parameters));

                mySqlCommand.CommandText = sql.ToString();
                parameters.ForEach
                    (
                        param =>
                        {
                            mySqlCommand.Parameters.Add(param);
                        }
                    );

                variantes = mySqlCommand.ExecuteNonQuery();
                
                return variantes;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsulta_Count");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene una variante especifica
        /// </summary>
        /// <param name="glConsulta">Informacion de la variante</param>
        ///<param name="whereBuilder">string de condiciones de la consulta</param>
        /// <returns>Variante</returns>
        public DTO_glConsulta DAL_glConsulta_Get(DTO_glConsulta glConsulta, string whereBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //var parameters = new List<SqlParameter>();
                
                var variante = new DTO_glConsulta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("SELECT  v.glConsultaID, v.Nombre, v.DocumentoID, f.Descriptivo AS FormaDesc, v.seUsuarioID, u.UsuarioID, v.Seleccion, v.Filtro, v.Distincion, ");
                sql.Append("        v.Activo, v.CtrlVersion, v.FechaCreacion, v.UsuarioCreacion, v.FechaAct, v.UsuarioAct, v.Prefijada");
                sql.Append(" FROM    glConsulta v LEFT OUTER JOIN ");
                sql.Append("        glDocumento f ON f.DocumentoID = v.DocumentoID LEFT OUTER JOIN ");
                sql.Append("        seUsuario u ON u.ReplicaID = v.seUsuarioID ");
                sql.Append(whereBuilder);
                //sql.Append(this.DAL_glConsulta_WhereBuilder(glConsulta, true, parameterss));

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
                    variante = new DTO_glConsulta(dr);
                }
                dr.Close();

                return variante;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsulta_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Adicionar una nueva variante
        /// </summary>
        /// <param name="glConsulta">Variante a adicionar</param>
        /// <returns>Variante adicionada</returns>
        public DTO_glConsulta DAL_glConsulta_Add(DTO_glConsulta glConsulta)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                var parameters = new List<SqlParameter>();

                var variante = new DTO_glConsulta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                
                sql.Append("INSERT INTO glConsulta(Nombre, DocumentoID, seUsuarioID, Seleccion, Filtro, Distincion, Activo, CtrlVersion, FechaCreacion, UsuarioCreacion, FechaAct, UsuarioAct, Prefijada) ");
                sql.Append(" VALUES (@Nombre, @DocumentoID, @seUsuarioID, @Seleccion, @Filtro, @Distincion, @Activo, @CtrlVersion, @FechaCreacion, @UsuarioCreacion, @FechaAct, @UsuarioAct, @Prefijada) ");
                sql.Append(" SET @ID = SCOPE_IDENTITY()");

                mySqlCommand.Parameters.Add("@Nombre", SqlDbType.VarChar, 255);
                mySqlCommand.Parameters["@Nombre"].Value = glConsulta.Nombre;

                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters["@DocumentoID"].Value = glConsulta.DocumentoID;

                mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                mySqlCommand.Parameters["@seUsuarioID"].Value = glConsulta.seUsuarioID;

                mySqlCommand.Parameters.Add("@Seleccion", SqlDbType.NVarChar);
                mySqlCommand.Parameters["@Seleccion"].Value = glConsulta.Seleccion;

                mySqlCommand.Parameters.Add("@Filtro", SqlDbType.NVarChar);
                mySqlCommand.Parameters["@Filtro"].Value = glConsulta.Filtro;

                mySqlCommand.Parameters.Add("@Distincion", SqlDbType.Bit);
                mySqlCommand.Parameters["@Distincion"].Value = (glConsulta.Distincion == null) ? false : glConsulta.Distincion;

                mySqlCommand.Parameters.Add("@Activo", SqlDbType.Bit);
                mySqlCommand.Parameters["@Activo"].Value = glConsulta.Activo;

                mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);
                mySqlCommand.Parameters["@CtrlVersion"].Value = glConsulta.CtrlVersion;

                mySqlCommand.Parameters.Add("@FechaCreacion", SqlDbType.DateTime);
                mySqlCommand.Parameters["@FechaCreacion"].Value = System.DateTime.Now;

                mySqlCommand.Parameters.Add("@UsuarioCreacion", SqlDbType.Int);
                mySqlCommand.Parameters["@UsuarioCreacion"].Value = glConsulta.UsuarioCreacion;

                mySqlCommand.Parameters.Add("@FechaAct", SqlDbType.DateTime);
                mySqlCommand.Parameters["@FechaAct"].Value = System.DateTime.Now;

                mySqlCommand.Parameters.Add("@UsuarioAct", SqlDbType.Int);
                mySqlCommand.Parameters["@UsuarioAct"].Value = glConsulta.UsuarioAct;

                mySqlCommand.Parameters.Add("@Prefijada", SqlDbType.Bit);
                mySqlCommand.Parameters["@Prefijada"].Value = glConsulta.Prefijada;

                mySqlCommand.Parameters.Add("@ID", SqlDbType.Int);
                mySqlCommand.Parameters["@ID"].Direction = ParameterDirection.Output;

                mySqlCommand.CommandText = sql.ToString();

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                dr.Close();

                glConsulta.glConsultaID = Convert.ToInt32(mySqlCommand.Parameters["@ID"].Value);

                return glConsulta;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsulta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar una  variante
        /// </summary>
        /// <param name="glConsulta">Variante a adicionar</param>
        ///<param name="UpdateSetBuilder">string de condiciones de la consulta</param>
        /// <returns>Variante actualizada</returns>
        public DTO_glConsulta DAL_glConsulta_Update(DTO_glConsulta glConsulta, string UpdateSetBuilder, List<SqlParameter> parameters)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();
                //var parameters = new List<SqlParameter>();

                var variante = new DTO_glConsulta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();

                sql.Append("UPDATE  glConsulta SET ");
                sql.Append(UpdateSetBuilder);
                //sql.Append(this.DAL_glConsulta_UpdateSetBuilder(glConsulta, parameters));
                sql.Append(" WHERE glConsultaID = @glConsultaID");
                sql.Append(" AND CtrlVersion = @CtrlVersion");

                mySqlCommand.CommandText = sql.ToString();

                var parameterId = new SqlParameter("@glConsultaID", SqlDbType.Int);
                parameterId.Value = glConsulta.glConsultaID;

                parameters.Add(parameterId);

                var parameterCtrl = new SqlParameter("@CtrlVersion", SqlDbType.Int);
                parameterCtrl.Value = glConsulta.CtrlVersion;

                parameters.Add(parameterCtrl);

                parameters.ForEach
                (
                    param =>
                    {
                        mySqlCommand.Parameters.Add(param);
                    }
                );

                //SqlDataReader dr;
                mySqlCommand.ExecuteNonQuery();

                //dr.Close();

                return glConsulta;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsulta_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Eliminar una  variante
        /// </summary>
        /// <param name="glConsulta">Variante a adicionar</param>
        /// <returns>Variante eliminada</returns>
        public void DAL_glConsulta_Delete(DTO_glConsulta glConsulta)
        {
            try
            {
                var sql = new StringBuilder();
                var where = new StringBuilder();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                sql.Append("DELETE FROM  glConsulta ");
                sql.Append("WHERE glConsultaID = @glConsultaID ");
                sql.Append("AND CtrlVersion = @CtrlVersion");

                mySqlCommand.CommandText = sql.ToString();
                mySqlCommand.Parameters.Add("@glConsultaID", SqlDbType.Int);
                mySqlCommand.Parameters["@glConsultaID"].Value = glConsulta.glConsultaID;

                mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.Int);
                mySqlCommand.Parameters["@CtrlVersion"].Value = glConsulta.CtrlVersion;

                mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsulta_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las variantes
        /// </summary>
        /// <param name="glConsulta">Filtros de la variantes</param>
        /// <param name="onlyId">Bandera que indica si es solo el ID</param>
        /// <param name="parameters">parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        public string DAL_glConsulta_WhereBuilder(DTO_glConsulta glConsulta, bool onlyId, List<SqlParameter> parameters)
        {
            var where = new StringBuilder();

            if (glConsulta.glConsultaID != 0)
            {
                if (where.ToString().Length == 0)
                {
                    where.Append(" WHERE ");
                }
                else
                {
                    where.Append(" AND ");
                }

                where.Append(" glConsultaID = @glConsultaID ");

                var parameter = new SqlParameter("@glConsultaID", SqlDbType.Int);
                parameter.Value = glConsulta.glConsultaID;

                parameters.Add(parameter);
            }

            if (!onlyId)
            {
                if (!String.IsNullOrEmpty(glConsulta.Nombre))
                {
                    if (where.ToString().Length == 0)
                    {
                        where.Append(" WHERE ");
                    }
                    else
                    {
                        where.Append(" AND ");
                    }

                    where.Append(" v.Nombre LIKE '% @Nombre %' ");

                    var parameter = new SqlParameter("@Nombre", SqlDbType.VarChar, 255);
                    parameter.Value = glConsulta.Nombre;

                    parameters.Add(parameter);
                }

                if (glConsulta.DocumentoID != 0)
                {
                    if (where.ToString().Length == 0)
                    {
                        where.Append(" WHERE ");
                    }
                    else
                    {
                        where.Append(" AND ");
                    }

                    where.Append(" v.DocumentoID = @DocumentoID ");

                    var parameter = new SqlParameter("@DocumentoID", SqlDbType.Int);
                    parameter.Value = glConsulta.DocumentoID;

                    parameters.Add(parameter);
                }

                if (glConsulta.seUsuarioID != 0)
                {
                    if (where.ToString().Length == 0)
                    {
                        where.Append(" WHERE ");
                    }
                    else
                    {
                        where.Append(" AND ");
                    }

                    where.Append(" v.seUsuarioID = @seUsuarioID ");

                    var parameter = new SqlParameter("@seUsuarioID", SqlDbType.Int);
                    parameter.Value = glConsulta.seUsuarioID;

                    parameters.Add(parameter);
                }

                if (glConsulta.Activo != null)
                {
                    if (where.ToString().Length == 0)
                    {
                        where.Append(" WHERE ");
                    }
                    else
                    {
                        where.Append(" AND ");
                    }

                    where.Append(" v.Activo = @Activo ");

                    var parameter = new SqlParameter("@Activo", SqlDbType.Int);
                    parameter.Value = glConsulta.Activo;

                    parameters.Add(parameter);
                }
            }
            return where.ToString();
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las variantes
        /// </summary>
        /// <param name="glConsulta">Filtros de la variantes</param>
        /// <param name="onlyId">Bandera que indica si es solo el ID</param>
        /// <param name="parameters">parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        public string DAL_glConsulta_UpdateSetBuilder(DTO_glConsulta glConsulta, List<SqlParameter> parameters)
        {
            var set = new StringBuilder();

            if (!String.IsNullOrEmpty(glConsulta.Nombre))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" Nombre = @Nombre ");

                var parameter = new SqlParameter("@Nombre", SqlDbType.VarChar, 255);
                parameter.Value = glConsulta.Nombre;

                parameters.Add(parameter);
            }

            if (glConsulta.seUsuarioID != 0)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }

                set.Append(" seUsuarioID = @seUsuarioID ");

                var parameter = new SqlParameter("@seUsuarioID", SqlDbType.Int);
                parameter.Value = glConsulta.seUsuarioID;

                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(glConsulta.Seleccion))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" Seleccion = @Seleccion ");

                var parameter = new SqlParameter("@Seleccion", SqlDbType.NVarChar);
                parameter.Value = glConsulta.Seleccion;

                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(glConsulta.Filtro))
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }
                set.Append(" Filtro = @Filtro ");

                var parameter = new SqlParameter("@Filtro", SqlDbType.NVarChar);
                parameter.Value = glConsulta.Filtro;

                parameters.Add(parameter);
            }

            if (glConsulta.Distincion != null)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }

                set.Append(" Distincion = @Distincion ");

                var parameter = new SqlParameter("@Distincion", SqlDbType.Bit);
                parameter.Value = glConsulta.Distincion;

                parameters.Add(parameter);
            }

            if (glConsulta.Activo != null)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }

                set.Append(" Activo = @Activo ");

                var parameter = new SqlParameter("@Activo", SqlDbType.Bit);
                parameter.Value = glConsulta.Activo;

                parameters.Add(parameter);
            }

            if (glConsulta.FechaCreacion != null)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }

                set.Append(" FechaCreacion = @FechaCreacion ");

                var parameter = new SqlParameter("@FechaCreacion", SqlDbType.DateTime);
                parameter.Value = glConsulta.FechaCreacion;

                parameters.Add(parameter);
            }

            if (glConsulta.UsuarioCreacion != 0)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }

                set.Append(" UsuarioCreacion = @UsuarioCreacion ");

                var parameter = new SqlParameter("@UsuarioCreacion", SqlDbType.Int);
                parameter.Value = glConsulta.UsuarioCreacion;
                parameters.Add(parameter);
            }

            if (glConsulta.FechaAct != null)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }

                set.Append(" FechaAct = @FechaAct ");

                var parameter = new SqlParameter("@FechaAct", SqlDbType.DateTime);
                parameter.Value = glConsulta.FechaAct;

                parameters.Add(parameter);
            }

            if (glConsulta.UsuarioAct != 0)
            {
                if (set.ToString().Length != 0)
                {
                    set.Append(", ");
                }

                set.Append(" UsuarioAct = @UsuarioAct ");

                var parameter = new SqlParameter("@UsuarioAct", SqlDbType.Int);
                parameter.Value = glConsulta.UsuarioAct;

                parameters.Add(parameter);
            }

            if (set.ToString().Length != 0)
            {
                set.Append(", CtrlVersion = @CtrlVersionUpd");

                var parameter = new SqlParameter("@CtrlVersionUpd", SqlDbType.Int);
                parameter.Value = glConsulta.CtrlVersion + 1;

                parameters.Add(parameter);

            }

            if (set.ToString().Length != 0)
            {
                set.Append(", Prefijada = @PrefijadaUpd");

                var parameter = new SqlParameter("@PrefijadaUpd", SqlDbType.Bit);
                parameter.Value = glConsulta.Prefijada;

                parameters.Add(parameter);

            }

            return set.ToString();
        }
        
        #endregion
    }
}
