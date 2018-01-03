using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_glEmpresaGrupo : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glEmpresaGrupo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Agrega un nuevo grupo de empresas
        /// </summary>
        /// <param name="grupo">Grupo de embresas</param>
        /// <param name="egCopia">Grupo de empresas del cual se saca la copia</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transacción</param>
        /// <returns></returns>
        public bool DAL_glEmpresaGrupo_Add(DTO_glEmpresaGrupo grupo, string egCopia, bool insideAnotherTx)
        {
            bool result = true;
            try
            {
                if (!insideAnotherTx)
                    base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

                SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                DAL_aplBitacora dal_bit = new DAL_aplBitacora(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int bitaPadre = 0;
               SqlDataReader dr = null;

                #region Consulta si ya existe el grupo de empresa
                bool exist = false;
                mySqlCommand.CommandText = "Select * from glEmpresaGrupo where EmpresaGrupoID = @EmpresaGrupoID";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = grupo.ID.Value; ;

                dr = mySqlCommand.ExecuteReader();

                if(dr.Read())
                    exist = true;
                dr.Close();
                mySqlCommand.Parameters.Clear();
                #endregion

                #region Ingresa el grupo de empresas
                if (!exist)
                {
                    mySqlCommand.CommandText =
                            "INSERT INTO glEmpresaGrupo(EmpresaGrupoID, Descriptivo, CtrlVersion, ActivoInd)values(@EmpresaGrupoID, @Descriptivo, @CtrlVersion, @ActivoInd)";

                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                    mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_Descriptivo.MaxLength);
                    mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = grupo.ID.Value;
                    mySqlCommand.Parameters["@Descriptivo"].Value = grupo.Descriptivo.Value;
                    mySqlCommand.Parameters["@CtrlVersion"].Value = 1;
                    mySqlCommand.Parameters["@ActivoInd"].Value = 1;

                    mySqlCommand.ExecuteNonQuery();
                    mySqlCommand.Parameters.Clear();
                }

                bitaPadre = dal_bit.DAL_aplBitacora_Add(this.Empresa.ID.Value, AppMasters.glEmpresaGrupo, (short)FormsActions.Add, DateTime.Now, this.UserId, grupo.ID.Value,
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
                #endregion              

                List<DTO_glTabla> tables = new List<DTO_glTabla>();
                #region Carga la lista de tablas
                mySqlCommand.CommandText =
                    "Select DocumentoID, Descriptivo, Jerarquica from glTabla where EmpresaGrupoID = @EmpresaGrupoID";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = egCopia;
                
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_glTabla t = new DTO_glTabla();
                    t.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                    t.Descriptivo.Value = dr["Descriptivo"].ToString();
                    t.Jerarquica.Value = Convert.ToBoolean(dr["Jerarquica"]);

                    tables.Add(t);
                }
                dr.Close();
                #endregion
                #region Crea la información en glTabla si se requiere
                if (!exist)
                {
                    mySqlCommand.CommandText =
                    "INSERT INTO glTabla(EmpresaGrupoID, DocumentoID, Descriptivo, Jerarquica, CtrlVersion, ActivoInd)" +
                    "values(@EmpresaGrupoID, @DocumentoID, @Descriptivo, @Jerarquica, @CtrlVersion, @ActivoInd)";

                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = grupo.ID.Value;

                    mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                    mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@Jerarquica", SqlDbType.Bit);
                    mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                    mySqlCommand.Parameters["@CtrlVersion"].Value = 1;
                    mySqlCommand.Parameters["@ActivoInd"].Value = 1;

                    tables.ForEach(t =>
                    {
                        mySqlCommand.Parameters["@DocumentoID"].Value = t.DocumentoID.Value.Value;
                        mySqlCommand.Parameters["@Descriptivo"].Value = t.Descriptivo.Value;
                        mySqlCommand.Parameters["@Jerarquica"].Value = t.Jerarquica.Value;

                        mySqlCommand.ExecuteNonQuery();
                        dal_bit.DAL_aplBitacora_Add(this.Empresa.ID.Value, AppMasters.glTabla, (short)FormsActions.Add, DateTime.Now, this.UserId, grupo.ID.Value,
                            t.DocumentoID.Value.Value.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, bitaPadre, 0, 0);
                    }); 
                }

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result = false;
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glEmpresaGrupo_Add");
                throw exception;
            }
            finally
            {
                if (result)
                {
                    if (!insideAnotherTx)
                        base.MySqlConnectionTx.Commit();
                }
                else
                {
                    if (base.MySqlConnectionTx != null)
                    {
                        base.MySqlConnectionTx.Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// Elimina un grupo de empresas
        /// </summary>
        /// <param name="grupo">Grupo de embresas</param>
        /// <param name="egCopia">Grupo de empresas del cual se saca la copia</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transacción</param>
        /// <returns></returns>
        public bool DAL_glEmpresaGrupo_Delete(string egID, bool insideAnotherTx)
        {
            bool result = true;
            try
            {
                if (!insideAnotherTx)
                    base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

                SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                DAL_aplBitacora dal_bit = new DAL_aplBitacora(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                // Elimina la info de glTabla
                mySqlCommand.CommandText =
                    "DELETE FROM glEmpresaGrupo where EmpresaGrupoID = @EmpresaGrupoID";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = egID;

                mySqlCommand.ExecuteNonQuery();
                dal_bit.DAL_aplBitacora_Add(this.Empresa.ID.Value, AppMasters.glEmpresaGrupo, (short)FormsActions.Delete, DateTime.Now, this.UserId, egID, string.Empty,
                    string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);

                // Elimina el grupo de empresas
                mySqlCommand.CommandText =
                    "DELETE FROM glTabla where EmpresaGrupoID = @EmpresaGrupoID";

                mySqlCommand.ExecuteNonQuery();
                dal_bit.DAL_aplBitacora_Add(this.Empresa.ID.Value, AppMasters.glTabla, (short)FormsActions.Delete, DateTime.Now, this.UserId, null, 0, 0, 0);

                return result;
            }
            catch (Exception ex)
            {
                result = false;
                var exception = new Exception(DictionaryMessages.Err_UpdatePwd, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glEmpresaGrupo_Delete");
                throw exception;
            }
            finally
            {
                if (result)
                {
                    if (!insideAnotherTx)
                        base.MySqlConnectionTx.Commit();
                }
                else
                {
                    if (base.MySqlConnectionTx != null)
                    {
                        base.MySqlConnectionTx.Rollback();
                    }
                }
            }

        }

        #endregion
    }
}
