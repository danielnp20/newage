using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_seGrupoDocumento : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_seGrupoDocumento(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        #region Funciones Publicas

        /// <summary>
        /// Trae la lista de seguridades de documentos de un usuario dada la empresa
        /// </summary>
        /// <param name="userId">Codigo de seguridad del usuario</param>
        /// <param name="userEmpDef">Empresa</param>
        /// <param name="isGroupActive">Si el grupo de seguridad esta activo</param>
        /// <returns>Retorna las seguridades de un usuario en una empresa</returns>
        public IEnumerable<DTO_seGrupoDocumento> DAL_seGrupoDocumento_GetByUsuarioID(string userEmpDef, int userId, bool isGroupActive)
        {
            try
            {
                List<DTO_seGrupoDocumento> grupoFormas = new List<DTO_seGrupoDocumento>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT GF.seGrupoID, GF.DocumentoID, GF.AccionesPerm, GF.CtrlVersion " + 
                    "FROM dbo.seUsuarioGrupo AS UG, seGrupo AS G, seGrupoDocumento AS GF " +
                    "WHERE UG.seGrupoID = G.seGrupoID AND GF.seGrupoID = G.seGrupoID AND G.ActivoInd=@GActivoInd " +
                    "   AND UG.seUsuarioID = @seUsuarioID AND UG.EmpresaID = @EmpresaID";

                mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Char, 15);
                mySqlCommand.Parameters["@seUsuarioID"].Value = userId;
                mySqlCommand.Parameters.Add("@GActivoInd", SqlDbType.SmallInt);
                mySqlCommand.Parameters["@GActivoInd"].Value = Convert.ToInt16(isGroupActive);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = userEmpDef;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var gForma = new DTO_seGrupoDocumento(dr);

                    grupoFormas.Add(gForma);
                }
                dr.Close();

                return grupoFormas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seGrupoDocumento_GetByUsuarioID");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene las seguridades del sistema para un grupo dado el modulo y el tipo de documento
        /// </summary>
        /// <param name="grupo">Grupo de seguridades</param>
        /// <param name="tipo">Tipo de documento</param>
        /// <returns>Retorna las seguridades de un grupo</returns>
        public IEnumerable<DTO_seGrupoDocumento> DAL_seGrupoDocumento_GetByType(string grupo, string tipo)
        {
            try
            {
                List<DTO_seGrupoDocumento> grupoFormas = new List<DTO_seGrupoDocumento>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select doc.ModuloID, doc.DocumentoID, doc.DocumentoTipo, gd.seGrupoID, gd.AccionesPerm, gd.CtrlVersion " +
                    "from glDocumento doc with(nolock) left join " +
                    "   (select * from seGrupoDocumento with(nolock) where seGrupoID = @seGrupoID) gd on gd.DocumentoID = doc.DocumentoID " +
                    "where doc.DocumentoTipo <> " + (int)FormTypes.Other + " " +
                    "order by doc.DocumentoTipo, doc.ModuloID, doc.DocumentoID";

                mySqlCommand.Parameters.Add("@seGrupoID", SqlDbType.Char, 15);
                mySqlCommand.Parameters["@seGrupoID"].Value = grupo;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var gForma = new DTO_seGrupoDocumento(dr, grupo);

                    grupoFormas.Add(gForma);
                }
                dr.Close();

                return grupoFormas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seGrupoDocumento_GetByType");
                throw exception;
            }
        }
        
        /// <summary>
        /// Verifica si existe una seguridad
        /// </summary>
        /// <param name="grupo">Identificador del grupo de seguridades</param>
        /// <param name="documento">Identificador del documento</param>
        /// <returns>Retorna un Dto de seguridades</returns>
        public DTO_seGrupoDocumento GetSecurity(string grupo, int documento)
        {
            try
            {
                DTO_seGrupoDocumento dto = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from seGrupoDocumento gd with(nolock) " +
                    "where gd.seGrupoID = @seGrupoID AND gd.DocumentoID = @DocumentoID";

                mySqlCommand.Parameters.Add("@seGrupoID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters["@seGrupoID"].Value = grupo;
                mySqlCommand.Parameters["@DocumentoID"].Value = documento;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    dto = new DTO_seGrupoDocumento(dr);
                }
                dr.Close();

                return dto;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seGrupoDocumento_GetSecurity");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona una seguridad
        /// </summary>
        /// <param name="dto">Seguridad</param>
        /// <returns>Retorna una respuesta TxResult</returns>
        public void DAL_seGrupoDocumento_Add(DTO_seGrupoDocumento dto)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string fields = "";
                string commandParams = "";

                
                fields += " ";
                commandParams += " ";

                mySqlCommand.CommandText =
                    "INSERT INTO seGrupoDocumento (seGrupoID , DocumentoID, AccionesPerm, CtrlVersion) " +
                    "VALUES (@seGrupoID, @DocumentoID, @AccionesPerm, @CtrlVersion)";

                mySqlCommand.Parameters.Add("@seGrupoID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@AccionesPerm", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);

                mySqlCommand.Parameters["@seGrupoID"].Value = dto.seGrupoID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = dto.DocumentoID.Value.Value;
                mySqlCommand.Parameters["@AccionesPerm"].Value = dto.AccionesPerm.Value.Value;
                mySqlCommand.Parameters["@CtrlVersion"].Value = 1;

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seGrupoDocumento_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona una seguridad
        /// </summary>
        /// <param name="dto">Seguridad</param>
        /// <returns>Retorna una respuesta TxResult</returns>
        public void DAL_seGrupoDocumento_Update(DTO_seGrupoDocumento dto)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string fields = "";
                string commandParams = "";

                fields += " ";
                commandParams += " ";

                mySqlCommand.CommandText =
                    "UPDATE seGrupoDocumento SET AccionesPerm = @AccionesPerm, CtrlVersion = @CtrlVersion " +
                    "WHERE seGrupoID = @seGrupoID AND DocumentoID = @DocumentoID";

                mySqlCommand.Parameters.Add("@seGrupoID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@AccionesPerm", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);

                mySqlCommand.Parameters["@seGrupoID"].Value = dto.seGrupoID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = dto.DocumentoID.Value.Value;
                mySqlCommand.Parameters["@AccionesPerm"].Value = dto.AccionesPerm.Value.Value;
                mySqlCommand.Parameters["@CtrlVersion"].Value = dto.CtrlVersion.Value.Value + 1;

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seGrupoDocumento_Update");
                throw exception;
            }
        }

        #endregion

    }
}
