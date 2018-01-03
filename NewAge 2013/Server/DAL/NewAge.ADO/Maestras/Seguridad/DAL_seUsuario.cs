using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using System.Reflection;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    public class DAL_seUsuario : DAL_Base
    {          
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_seUsuario(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : 
            base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae un usuario
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <param name="password">Contraseña de usuario</param>
        /// <returns>Returna un usuario</returns>
        public UserResult DAL_seUsuario_ValidateUserCredentials(string userId, string password, DTO_seUsuario user, DTO_glControl ctrlDTO)
        {
            try
            {
                    SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();
                    mySqlCommand.Transaction = base.MySqlConnectionTx;

                    //Se hace la consulta de nuevo para traer la contrasena
                    mySqlCommand = this.MySqlConnection.CreateCommand();
                    mySqlCommand.CommandText =
                      "SELECT Contrasena FROM seUsuario " +
                      "WHERE UsuarioID = @UsuarioID";

                    mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, 15);
                    mySqlCommand.Parameters["@UsuarioID"].Value = user.ID.Value;

                    SqlDataReader dr;
                    dr = mySqlCommand.ExecuteReader();

                    if (dr.Read())
                    {
                        byte[] buffer = new byte[16];
                        dr.GetBytes(0, 0, buffer, 0, buffer.Length);
                        user.Contrasena = buffer;
                    }
                    dr.Close();

                    var userPass = AESCrypto.Decrypt(user.Contrasena);

                    int numRep = 0;
                    double hDiff = 0;

                    mySqlCommand.CommandText =
                      "UPDATE seUsuario " +
                      "SET ContrasenaRep = @ContrasenaRep, FechaUltimaDig = @FechaUltimaDig " +
                      "WHERE UsuarioID = @UsuarioID";

                    mySqlCommand.Parameters.Add("@ContrasenaRep", SqlDbType.SmallInt);
                    mySqlCommand.Parameters.Add("@FechaUltimaDig", SqlDbType.DateTime);

                    mySqlCommand.Parameters["@FechaUltimaDig"].Value = DateTime.Now;

                    if (password != userPass)// || !userId.Equals(user.ID.Value, StringComparison.OrdinalIgnoreCase))
                    {                      
                        // Revisa si el usuario ya esta bloqueado
                        if ((Convert.ToInt16(ctrlDTO.Data.Value) - user.ContrasenaRep.Value.Value) <= 0)
                        {
                            return UserResult.BlockUser;
                        }

                        if (user.FechaUltimaDig.Value.HasValue)
                            hDiff = (DateTime.Now - user.FechaUltimaDig.Value.Value).TotalHours;

                        if (hDiff <= 6)
                            numRep = user.ContrasenaRep.Value.Value;

                        mySqlCommand.Parameters["@ContrasenaRep"].Value = numRep + 1;
                        int rows = mySqlCommand.ExecuteNonQuery();

                        return UserResult.IncorrectPassword;
                    }

                    mySqlCommand.Parameters["@ContrasenaRep"].Value = numRep;
                    int rows1 = mySqlCommand.ExecuteNonQuery();

                    return UserResult.AlreadyMember;               
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UserValidate, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seUsuario_ValidateUserCredentials");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <param name="oldPwd">Contraseña vieja</param>
        /// <param name="oldPwdDate">Fecha en que fue modificada por ultima vez la contraseña</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public int DAL_seUsuario_UpdatePassword(int userID, string pwd, string oldPwd, string oldPwdDate)
        {
            try
            {
                var encripPass = AESCrypto.Encrypt(pwd);

                SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "UPDATE seUsuario " +
                    "SET Contrasena = @Contrasena, ContrasenaRep = 0, FechaUltimaDig = @FechaUltimaDig, ContrasenaFecCambio = @ContrasenaFecCambio " +
                    "WHERE ReplicaID = @ReplicaID";


                mySqlCommand.Parameters.Add("@Contrasena", SqlDbType.VarBinary, 256);
                mySqlCommand.Parameters.Add("@FechaUltimaDig", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ContrasenaFecCambio", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ReplicaID", SqlDbType.Int);

                mySqlCommand.Parameters["@Contrasena"].Value = encripPass;
                mySqlCommand.Parameters["@FechaUltimaDig"].Value = DateTime.Now;
                mySqlCommand.Parameters["@ContrasenaFecCambio"].Value = DateTime.Now;
                mySqlCommand.Parameters["@ReplicaID"].Value = userID;

                int rows = mySqlCommand.ExecuteNonQuery();

                return rows;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdatePwd, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seUsuario_UpdatePassword");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public bool DAL_seUsuario_ResetPassword(int userID, string pwd, bool insideAnotherTx)
        {
            bool result = true;
            try
            {
                var encripPass = AESCrypto.Encrypt(pwd);

                if (!insideAnotherTx)
                    base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

                SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "UPDATE seUsuario " +
                    "SET Contrasena = @Contrasena, ContrasenaRep = 0, FechaUltimaDig = @FechaUltimaDig, ContrasenaFecCambio = @ContrasenaFecCambio " +
                    "WHERE ReplicaID = @ReplicaID";

                mySqlCommand.Parameters.Add("@Contrasena", SqlDbType.VarBinary, 256);
                mySqlCommand.Parameters.Add("@FechaUltimaDig", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ContrasenaFecCambio", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ReplicaID", SqlDbType.Int);

                mySqlCommand.Parameters["@Contrasena"].Value = encripPass;
                mySqlCommand.Parameters["@FechaUltimaDig"].Value = DateTime.Now;
                mySqlCommand.Parameters["@ContrasenaFecCambio"].Value = DateTime.Now;
                mySqlCommand.Parameters["@ReplicaID"].Value = userID;

                int rows = mySqlCommand.ExecuteNonQuery();

                if (rows == 0)
                    result = false;

                return result;
            }
            catch (Exception ex)
            {
                result = false;
                var exception = new Exception(DictionaryMessages.Err_UpdatePwd, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seUsuario_ResetPassword");
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
        /// Devuelve las empresas a las que tiene permiso un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <returns>Retorna una lista de empresas</returns>
        public IEnumerable<DTO_glEmpresa> DAL_seUsuario_GetUserCompanies(string userID)
        {
            try
            {
                List<DTO_glEmpresa> empresas = new List<DTO_glEmpresa>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct emp.* " +
                    "from seUsuarioGrupo usrG with(nolock)  " +
                    "	inner join seUsuario usr with(nolock) on usrG.seUsuarioID = usr.ReplicaID " +
                    "	inner join seGrupo grp with(nolock) on usrG.seGrupoID = grp.seGrupoID " +
                    "	left outer join glEmpresa emp with(nolock) on usrG.EmpresaID = emp.EmpresaID " +
                    "where usrG.ActivoInd = 1 and usr.ActivoInd = 1 and emp.ActivoInd = 1 and grp.ActivoInd = 1 " +
                    "	and usr.UsuarioID = @UsuarioID";

                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters["@UsuarioID"].Value = userID;

                //Propiedades de la maestra de empresas
                DTO_aplMaestraPropiedades props = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, AppMasters.glEmpresa, this.loggerConnectionStr);

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_glEmpresa emp = new DTO_glEmpresa(dr, props, true);
                    empresas.Add(emp);
                }
                dr.Close();

                return empresas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seUsuario_GetUserCompanies");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un usuario de acuerdo con el id de la replica (pk)
        /// </summary>
        /// <param name="userID">Identificador del usuario (ReplicaID)</param>
        /// <returns>Retorna el Usuario</returns>
        public DTO_seUsuario DAL_seUsuario_GetUserByReplicaID(int userID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                "SELECT * FROM seUsuario WHERE ReplicaID= @ID";

                mySqlCommand.Parameters.Add("@ID", SqlDbType.Int);
                mySqlCommand.Parameters["@ID"].Value = userID;

                DTO_aplMaestraPropiedades props = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, AppMasters.seUsuario, this.loggerConnectionStr);
                DTO_seUsuario usr = null;
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                    usr = new DTO_seUsuario(dr, props, true);
               
                dr.Close();

                return usr;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingUser, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seUsuario_GetUserNameByReplica");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un usuario
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <returns>Retorna un usuario</returns>
        public DTO_seUsuario DAL_seUsuario_GetUserbyID(string userId)
        {
            try
            {
                DAL_MasterSimple usrDAL = new DAL_MasterSimple(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                usrDAL.DocumentID = AppMasters.seUsuario;

                UDT_BasicID udt = new UDT_BasicID() { Value = userId };
                DTO_MasterBasic basic = usrDAL.DAL_MasterSimple_GetByID(udt, true);

                if (basic == null || basic.ID == null || basic.IdName == null)
                    return null;
                else
                    return (DTO_seUsuario)basic;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingUser, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seUsuario_GetUserbyID");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona un usuario
        /// </summary>
        /// <param name="dto">MasterBasic</param>
        /// <returns>Retorna el resultado TxResult de la operacion</returns>
        public void DAL_seUsuario_Add(DTO_seUsuario dto, string egControl)
        {
            try
            {
                DTO_TxResultDetail rd = new DTO_TxResultDetail();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "INSERT INTO seUsuario (" +
                        "UsuarioID, Descriptivo, IdiomaID, EmpresaIDPref, EmpresaID, AreaFuncionalID, eg_glAreaFuncional, ConexionInd, ConexionFec,Telefono, " +
                        "CorreoElectronico, Contrasena, ContrasenaRep, SeccionFuncionalID, eg_glSeccionFuncional, FechaUltimaDig, ContrasenaFecCambio, ActivoInd, CtrlVersion, " +
                        "ResponsableTercerosInd, UsuarioDelegado, FechaDelegaINI, FechaDelegaFIN, DelegacionActivaInd, HorarioID, eg_glHorarioTrabajo " +
                    ") VALUES (" +
                        "@UsuarioID, @Descriptivo, @IdiomaID, @EmpresaIDPref, @EmpresaID, @AreaFuncionalID, @eg_glAreaFuncional, @ConexionInd, @ConexionFec,@Telefono," +
                        "@CorreoElectronico,@Contrasena, @ContrasenaRep,@SeccionFuncionalID, @eg_glSeccionFuncional,@FechaUltimaDig, @ContrasenaFecCambio, @ActivoInd, @CtrlVersion," +
                        "@ResponsableTercerosInd,@UsuarioDelegado, @FechaDelegaINI,@FechaDelegaFIN, @DelegacionActivaInd,@HorarioID, @eg_glHorarioTrabajo " +
                    ")";

                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, dto.ID.MaxLength);
                mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.VarChar, UDT_Descriptivo.MaxLength);
                mySqlCommand.Parameters.Add("@IdiomaID", SqlDbType.VarChar, UDT_IdiomaID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaIDPref", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@AreaFuncionalID", SqlDbType.VarChar, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@ConexionInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ConexionFec", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@CorreoElectronico", SqlDbType.VarChar, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@Telefono", SqlDbType.VarChar, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@Contrasena", SqlDbType.VarBinary, 256);
                mySqlCommand.Parameters.Add("@ContrasenaFecCambio", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaUltimaDig", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ContrasenaRep", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@SeccionFuncionalID", SqlDbType.VarChar, UDT_SeccionFuncional.MaxLength);              
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@ResponsableTercerosInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@UsuarioDelegado",  SqlDbType.Char, dto.ID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaDelegaINI", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaDelegaFIN", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@DelegacionActivaInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@HorarioID", SqlDbType.Char,3);
                mySqlCommand.Parameters.Add("@eg_glAreaFuncional", SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glSeccionFuncional", SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glHorarioTrabajo", SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@UsuarioID"].Value = dto.ID.Value.Trim();
                mySqlCommand.Parameters["@Descriptivo"].Value = dto.Descriptivo.Value.Trim();
                mySqlCommand.Parameters["@IdiomaID"].Value = dto.IdiomaID.Value;
                mySqlCommand.Parameters["@EmpresaIDPref"].Value = dto.EmpresaIDPref.Value;
                mySqlCommand.Parameters["@EmpresaID"].Value = dto.EmpresaID.Value;
                mySqlCommand.Parameters["@AreaFuncionalID"].Value = dto.AreaFuncionalID.Value;
                mySqlCommand.Parameters["@ConexionInd"].Value = false;
                mySqlCommand.Parameters["@ConexionFec"].Value = DBNull.Value;
                mySqlCommand.Parameters["@CorreoElectronico"].Value = dto.CorreoElectronico.Value;
                mySqlCommand.Parameters["@Telefono"].Value = dto.Telefono.Value;
                mySqlCommand.Parameters["@Contrasena"].Value = AESCrypto.Encrypt(dto.ContrasenaLimpia);
                mySqlCommand.Parameters["@ContrasenaFecCambio"].Value = DateTime.Now;
                mySqlCommand.Parameters["@FechaUltimaDig"].Value = DBNull.Value;
                mySqlCommand.Parameters["@ContrasenaRep"].Value = 0;
                mySqlCommand.Parameters["@SeccionFuncionalID"].Value = dto.SeccionFuncionalID.Value;               
                mySqlCommand.Parameters["@ActivoInd"].Value = dto.ActivoInd.Value;
                mySqlCommand.Parameters["@CtrlVersion"].Value = dto.CtrlVersion.Value;
                mySqlCommand.Parameters["@ResponsableTercerosInd"].Value = dto.ResponsableTercerosInd.Value;
                mySqlCommand.Parameters["@UsuarioDelegado"].Value = dto.UsuarioDelegado.Value;
                mySqlCommand.Parameters["@FechaDelegaINI"].Value = dto.FechaDelegaINI.Value;
                mySqlCommand.Parameters["@FechaDelegaFIN"].Value = dto.FechaDelegaFIN.Value;
                mySqlCommand.Parameters["@DelegacionActivaInd"].Value = dto.DelegacionActivaInd.Value;
                mySqlCommand.Parameters["@HorarioID"].Value = dto.HorarioID.Value;
                mySqlCommand.Parameters["@eg_glSeccionFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glSeccionFuncional, this.Empresa, egControl);
                mySqlCommand.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egControl);
                mySqlCommand.Parameters["@eg_glHorarioTrabajo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glHorarioTrabajo, this.Empresa, egControl);

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddUser, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seUsuario_Add");
                throw exception;
            }
        }

        /// <summary>
        ///Actualiza un usuario
        /// </summary>
        /// <param name="dto">MasterBasic</param> 
        /// <returns>Retorna el resultado TxResult de la operacion</returns>
        public void DAL_seUsuario_Update(DTO_seUsuario dto)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                string egControl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                mySqlCommand.CommandText =
                    "UPDATE seUsuario " +
                        "SET Descriptivo = @Descriptivo, IdiomaID = @IdiomaID, EmpresaIDPref = @EmpresaIDPref, EmpresaID = @EmpresaID, AreaFuncionalID = @AreaFuncionalID, Telefono = @Telefono," +
                        "SeccionFuncionalID = @SeccionFuncionalID,eg_glSeccionFuncional = @eg_glSeccionFuncional,CorreoElectronico = @CorreoElectronico, ActivoInd = @ActivoInd, CtrlVersion = @CtrlVersion, " +
                        "ResponsableTercerosInd = @ResponsableTercerosInd,UsuarioDelegado = @UsuarioDelegado, FechaDelegaINI = @FechaDelegaINI,FechaDelegaFIN =@FechaDelegaFIN, " +
                        " DelegacionActivaInd = @DelegacionActivaInd,HorarioID = @HorarioID, eg_glHorarioTrabajo = @eg_glHorarioTrabajo " +
                        "WHERE ReplicaID = @ReplicaID";

                    mySqlCommand.Parameters.Add("@ReplicaID", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.VarChar, UDT_Descriptivo.MaxLength);
                    mySqlCommand.Parameters.Add("@IdiomaID", SqlDbType.VarChar, UDT_IdiomaID.MaxLength);
                    mySqlCommand.Parameters.Add("@EmpresaIDPref", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@AreaFuncionalID", SqlDbType.VarChar, UDT_AreaFuncionalID.MaxLength);
                    mySqlCommand.Parameters.Add("@SeccionFuncionalID", SqlDbType.VarChar, UDT_SeccionFuncional.MaxLength);
                    mySqlCommand.Parameters.Add("@eg_glSeccionFuncional", SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);
                    mySqlCommand.Parameters.Add("@CorreoElectronico", SqlDbType.VarChar, UDT_DescripTBase.MaxLength);
                    mySqlCommand.Parameters.Add("@Telefono", SqlDbType.VarChar, UDT_DescripTBase.MaxLength);
                    mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.SmallInt);
                    mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);
                    mySqlCommand.Parameters.Add("@ResponsableTercerosInd", SqlDbType.TinyInt);
                    mySqlCommand.Parameters.Add("@UsuarioDelegado", SqlDbType.Char, dto.ID.MaxLength);
                    mySqlCommand.Parameters.Add("@FechaDelegaINI", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters.Add("@FechaDelegaFIN", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters.Add("@DelegacionActivaInd", SqlDbType.TinyInt);
                    mySqlCommand.Parameters.Add("@HorarioID", SqlDbType.Char, 3);
                    mySqlCommand.Parameters.Add("@eg_glHorarioTrabajo", SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);

                    mySqlCommand.Parameters["@ReplicaID"].Value = dto.ReplicaID.Value;
                    mySqlCommand.Parameters["@Descriptivo"].Value = dto.Descriptivo.Value.Trim();
                    mySqlCommand.Parameters["@IdiomaID"].Value = dto.IdiomaID.Value;
                    mySqlCommand.Parameters["@EmpresaIDPref"].Value = dto.EmpresaIDPref.Value;
                    mySqlCommand.Parameters["@EmpresaID"].Value = dto.EmpresaID.Value;
                    mySqlCommand.Parameters["@AreaFuncionalID"].Value = dto.AreaFuncionalID.Value;
                    mySqlCommand.Parameters["@SeccionFuncionalID"].Value = dto.SeccionFuncionalID.Value;
                    mySqlCommand.Parameters["@eg_glSeccionFuncional"].Value = !string.IsNullOrEmpty(dto.SeccionFuncionalID.Value)? this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glSeccionFuncional, this.Empresa, egControl) :string.Empty;
                    mySqlCommand.Parameters["@CorreoElectronico"].Value = dto.CorreoElectronico.Value;
                    mySqlCommand.Parameters["@Telefono"].Value = dto.Telefono.Value;
                    mySqlCommand.Parameters["@ActivoInd"].Value = dto.ActivoInd.Value;
                    mySqlCommand.Parameters["@CtrlVersion"].Value = dto.CtrlVersion.Value;
                    mySqlCommand.Parameters["@ResponsableTercerosInd"].Value = dto.ResponsableTercerosInd.Value;
                    mySqlCommand.Parameters["@UsuarioDelegado"].Value = dto.UsuarioDelegado.Value;
                    mySqlCommand.Parameters["@FechaDelegaINI"].Value = dto.FechaDelegaINI.Value;
                    mySqlCommand.Parameters["@FechaDelegaFIN"].Value = dto.FechaDelegaFIN.Value;
                    mySqlCommand.Parameters["@DelegacionActivaInd"].Value = dto.DelegacionActivaInd.Value;
                    mySqlCommand.Parameters["@HorarioID"].Value = dto.HorarioID.Value;
                    mySqlCommand.Parameters["@eg_glHorarioTrabajo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glHorarioTrabajo, this.Empresa, egControl);

                    foreach (SqlParameter param in mySqlCommand.Parameters)
                    {
                        if (param.Direction.Equals(ParameterDirection.Input))
                        {
                            if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                                param.Value = DBNull.Value;
                        }
                    }

                    mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seUsuario_Update");
                throw exception;
            }
        }

        #endregion
    }
}
