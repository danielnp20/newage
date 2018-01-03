using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_ccSolicitudDataCreditoUbica : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudDataCreditoUbica(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudDataCreditoUbica que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudDataCreditoUbica</returns>
        public List<DTO_ccSolicitudDataCreditoUbica> DAL_ccSolicitudDataCreditoUbica_GetAll()
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoUbica> result = new List<DTO_ccSolicitudDataCreditoUbica>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT Datos.* FROM ccSolicitudDataCreditoUbica Datos with(nolock)";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDataCreditoUbica Datos;
                    Datos = new DTO_ccSolicitudDataCreditoUbica(dr);
                    result.Add(Datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoUbica_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_ccSolicitudDataCreditoUbica> DAL_ccSolicitudDataCreditoUbica_GetByNUmeroDoc(int numeroDoc, Int16? versionNro)
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoUbica> result = new List<DTO_ccSolicitudDataCreditoUbica>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters.Add("@Version", SqlDbType.SmallInt);
                mySqlCommand.Parameters["@Version"].Value = versionNro;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                if (versionNro.HasValue)
                    mySqlCommand.CommandText = "select * from ccSolicitudDataCreditoUbica  with(nolock) " +
                                         " Where NumeroDoc =  @NumeroDoc and Version =  @Version  ";
                else
                    mySqlCommand.CommandText = "select * from ccSolicitudDataCreditoUbica  with(nolock) " +
                                        " Where NumeroDoc =  @NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDataCreditoUbica(dr));
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoTarea_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDataCreditoUbica
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_ccSolicitudDataCreditoUbica_Add(DTO_ccSolicitudDataCreditoUbica Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccSolicitudDataCreditoUbica " +
                    "( " +
                    "   NumeroDoc,Version,TipoId,NumeroId,Nombre,FechaExp,CiudadExp,DeptoExp,Genero,RangoEdad,CiudadTel1,DeptoTel1,CodCiudadTel1," +
                    "   CodDeptoTel1,NumeroTel1,TipoTel1,RepDesdeTel1,FechaActTel1,NumEntidadTel1,CiudadTel2,DeptoTel2,CodCiudadTel2,CodDeptoTel2," +
                    "   NumeroTel2,TipoTel2,RepDesdeTel2,FechaActTel2,NumEntidadTel2,CiudadTel3,DeptoTel3,CodCiudadTel3,CodDeptoTel3,NumeroTel3," +
                    "   TipoTel3,RepDesdeTel3,FechaActTel3,NumEntidadTel3,CiudadDir1,DeptoDir1,CodCiudadDir1,CodDeptoDir1,DireccionDir1,TipoDir1," +
                    "   EstratoDir1,RepDesdeDir1,FechaActDir1,NumEntidadDir1,CiudadDir2,DeptoDir2,CodCiudadDir2,CodDeptoDir2,DireccionDir2,TipoDir2," +
                    "   EstratoDir2,RepDesdeDir2,FechaActDir2,NumEntidadDir2,CiudadDir3,DeptoDir3,CodCiudadDir3,CodDeptoDir3,DireccionDir3,TipoDir3," +
                    "   EstratoDir3,RepDesdeDir3,FechaActDir3,NumEntidadDir3,Email1,RepDesdeMail1,FechaActMail1,NumEntidadMail1,Email2,RepDesdeMail2," +
                    "   FechaActMail2,NumEntidadMail2,Celular1,RepDesdeCel1,FechaActCel1,NumEntidadCel1,Celular2,RepDesdeCel2,FechaActCel2,NumEntidadCel2," +
                    "   Direccion1IND,Direccion2IND,Direccion3IND,DireccionOtraIND,DireccionOtra,Telefono1IND,Telefono2IND,Telefono3IND,TelefonoOtroIND," +
                    "   TelefonoOtro,Celular1IND,Celular2IND,CelularOtraIND,CelularOtro,EMailOtroIND,EMailOtro" +
                    ") " +
                    "VALUES " +
                    " ( "+
                    "   @NumeroDoc,@Version,@TipoId,@NumeroId,@Nombre,@FechaExp,@CiudadExp,@DeptoExp,@Genero,@RangoEdad,@CiudadTel1,@DeptoTel1,@CodCiudadTel1," +
                    "   @CodDeptoTel1,@NumeroTel1,@TipoTel1,@RepDesdeTel1,@FechaActTel1,@NumEntidadTel1,@CiudadTel2,@DeptoTel2,@CodCiudadTel2,@CodDeptoTel2," +
                    "   @NumeroTel2,@TipoTel2,@RepDesdeTel2,@FechaActTel2,@NumEntidadTel2,@CiudadTel3,@DeptoTel3,@CodCiudadTel3,@CodDeptoTel3,@NumeroTel3," +
                    "   @TipoTel3,@RepDesdeTel3,@FechaActTel3,@NumEntidadTel3,@CiudadDir1,@DeptoDir1,@CodCiudadDir1,@CodDeptoDir1,@DireccionDir1,@TipoDir1," +
                    "   @EstratoDir1,@RepDesdeDir1,@FechaActDir1,@NumEntidadDir1,@CiudadDir2,@DeptoDir2,@CodCiudadDir2,@CodDeptoDir2,@DireccionDir2,@TipoDir2," +
                    "   @EstratoDir2,@RepDesdeDir2,@FechaActDir2,@NumEntidadDir2,@CiudadDir3,@DeptoDir3,@CodCiudadDir3,@CodDeptoDir3,@DireccionDir3,@TipoDir3," +
                    "   @EstratoDir3,@RepDesdeDir3,@FechaActDir3,@NumEntidadDir3,@Email1,@RepDesdeMail1,@FechaActMail1,@NumEntidadMail1,@Email2,@RepDesdeMail2," +
                    "   @FechaActMail2,@NumEntidadMail2,@Celular1,@RepDesdeCel1,@FechaActCel1,@NumEntidadCel1,@Celular2,@RepDesdeCel2,@FechaActCel2,@NumEntidadCel2," +
                    "   @Direccion1IND,@Direccion2IND,@Direccion3IND,@DireccionOtraIND,@DireccionOtra,@Telefono1IND,@Telefono2IND,@Telefono3IND,@TelefonoOtroIND," +
                    "   @TelefonoOtro,@Celular1IND,@Celular2IND,@CelularOtraIND,@CelularOtro,@EMailOtroIND,@EMailOtro" +
                    ") SET @Consecutivo = SCOPE_IDENTITY() ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoId", SqlDbType.Char,1);
                mySqlCommandSel.Parameters.Add("@NumeroId", SqlDbType.Char,11);

                mySqlCommandSel.Parameters.Add("@Nombre", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@FechaExp", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CiudadExp", SqlDbType.Char,30);
                mySqlCommandSel.Parameters.Add("@DeptoExp", SqlDbType.Char,30);
                mySqlCommandSel.Parameters.Add("@Genero", SqlDbType.Char,1);
                mySqlCommandSel.Parameters.Add("@RangoEdad", SqlDbType.Char,5);
                mySqlCommandSel.Parameters.Add("@CiudadTel1", SqlDbType.Char,30);
                mySqlCommandSel.Parameters.Add("@DeptoTel1", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CodCiudadTel1", SqlDbType.Char,10);
                mySqlCommandSel.Parameters.Add("@CodDeptoTel1", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@NumeroTel1", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@TipoTel1", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@RepDesdeTel1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActTel1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadTel1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadTel2", SqlDbType.Char,30);
                mySqlCommandSel.Parameters.Add("@DeptoTel2", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CodCiudadTel2", SqlDbType.Char,10);
                mySqlCommandSel.Parameters.Add("@CodDeptoTel2", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@NumeroTel2", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@TipoTel2", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@RepDesdeTel2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActTel2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadTel2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadTel3", SqlDbType.Char,30);
                mySqlCommandSel.Parameters.Add("@DeptoTel3", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CodCiudadTel3", SqlDbType.Char,10);
                mySqlCommandSel.Parameters.Add("@CodDeptoTel3", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@NumeroTel3", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@TipoTel3", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@RepDesdeTel3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActTel3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadTel3", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadDir1", SqlDbType.Char,30);
                mySqlCommandSel.Parameters.Add("@DeptoDir1", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CodCiudadDir1", SqlDbType.Char,10);
                mySqlCommandSel.Parameters.Add("@CodDeptoDir1", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@DireccionDir1", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@TipoDir1", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@EstratoDir1", SqlDbType.Char,5);
                mySqlCommandSel.Parameters.Add("@RepDesdeDir1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActDir1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadDir1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadDir2", SqlDbType.Char,30);
                mySqlCommandSel.Parameters.Add("@DeptoDir2", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CodCiudadDir2", SqlDbType.Char,10);
                mySqlCommandSel.Parameters.Add("@CodDeptoDir2", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@DireccionDir2", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@TipoDir2", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@EstratoDir2", SqlDbType.Char,5);
                mySqlCommandSel.Parameters.Add("@RepDesdeDir2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActDir2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadDir2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadDir3", SqlDbType.Char,30);
                mySqlCommandSel.Parameters.Add("@DeptoDir3", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CodCiudadDir3", SqlDbType.Char,10);
                mySqlCommandSel.Parameters.Add("@CodDeptoDir3", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@DireccionDir3", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@TipoDir3", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@EstratoDir3", SqlDbType.Char,5);
                mySqlCommandSel.Parameters.Add("@RepDesdeDir3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActDir3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadDir3", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Email1", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@RepDesdeMail1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActMail1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadMail1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Email2", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@RepDesdeMail2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActMail2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadMail2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Celular1", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@RepDesdeCel1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActCel1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadCel1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Celular2", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@RepDesdeCel2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActCel2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadCel2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);

                mySqlCommandSel.Parameters.Add("@Direccion1IND", SqlDbType.Bit);                
                mySqlCommandSel.Parameters.Add("@Direccion2IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Direccion3IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DireccionOtra", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@DireccionOtraIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Telefono1IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Telefono2IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Telefono3IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@TelefonoOtroIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@TelefonoOtro", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@Celular1IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Celular2IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CelularOtraIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CelularOtro", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@EMailOtroIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EMailOtro", SqlDbType.Char,100);


                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoId"].Value = Datos.TipoId.Value;
                mySqlCommandSel.Parameters["@NumeroId"].Value = Datos.NumeroId.Value;
                mySqlCommandSel.Parameters["@Nombre"].Value = Datos.Nombre.Value;
                mySqlCommandSel.Parameters["@FechaExp"].Value = Datos.FechaExp.Value;
                mySqlCommandSel.Parameters["@CiudadExp"].Value = Datos.CiudadExp.Value;
                mySqlCommandSel.Parameters["@DeptoExp"].Value = Datos.DeptoExp.Value;
                mySqlCommandSel.Parameters["@Genero"].Value = Datos.Genero.Value;
                mySqlCommandSel.Parameters["@RangoEdad"].Value = Datos.RangoEdad.Value;
                mySqlCommandSel.Parameters["@CiudadTel1"].Value = Datos.CiudadTel1.Value;
                mySqlCommandSel.Parameters["@DeptoTel1"].Value = Datos.DeptoTel1.Value;
                mySqlCommandSel.Parameters["@CodCiudadTel1"].Value = Datos.CodCiudadTel1.Value;
                mySqlCommandSel.Parameters["@CodDeptoTel1"].Value = Datos.CodDeptoTel1.Value;
                mySqlCommandSel.Parameters["@NumeroTel1"].Value = Datos.NumeroTel1.Value;
                mySqlCommandSel.Parameters["@TipoTel1"].Value = Datos.TipoTel1.Value;
                mySqlCommandSel.Parameters["@RepDesdeTel1"].Value = Datos.RepDesdeTel1.Value;
                mySqlCommandSel.Parameters["@FechaActTel1"].Value = Datos.FechaActTel1.Value;
                mySqlCommandSel.Parameters["@NumEntidadTel1"].Value = Datos.NumEntidadTel1.Value;
                mySqlCommandSel.Parameters["@CiudadTel2"].Value = Datos.CiudadTel2.Value;
                mySqlCommandSel.Parameters["@DeptoTel2"].Value = Datos.DeptoTel2.Value;
                mySqlCommandSel.Parameters["@CodCiudadTel2"].Value = Datos.CodCiudadTel2.Value;
                mySqlCommandSel.Parameters["@CodDeptoTel2"].Value = Datos.CodDeptoTel2.Value;
                mySqlCommandSel.Parameters["@NumeroTel2"].Value = Datos.NumeroTel2.Value;
                mySqlCommandSel.Parameters["@TipoTel2"].Value = Datos.TipoTel2.Value;
                mySqlCommandSel.Parameters["@RepDesdeTel2"].Value = Datos.RepDesdeTel2.Value;
                mySqlCommandSel.Parameters["@FechaActTel2"].Value = Datos.FechaActTel2.Value;
                mySqlCommandSel.Parameters["@NumEntidadTel2"].Value = Datos.NumEntidadTel2.Value;
                mySqlCommandSel.Parameters["@CiudadTel3"].Value = Datos.CiudadTel3.Value;
                mySqlCommandSel.Parameters["@DeptoTel3"].Value = Datos.DeptoTel3.Value;
                mySqlCommandSel.Parameters["@CodCiudadTel3"].Value = Datos.CodCiudadTel3.Value;
                mySqlCommandSel.Parameters["@CodDeptoTel3"].Value = Datos.CodDeptoTel3.Value;
                mySqlCommandSel.Parameters["@NumeroTel3"].Value = Datos.NumeroTel3.Value;
                mySqlCommandSel.Parameters["@TipoTel3"].Value = Datos.TipoTel3.Value;
                mySqlCommandSel.Parameters["@RepDesdeTel3"].Value = Datos.RepDesdeTel3.Value;
                mySqlCommandSel.Parameters["@FechaActTel3"].Value = Datos.FechaActTel3.Value;
                mySqlCommandSel.Parameters["@NumEntidadTel3"].Value = Datos.NumEntidadTel3.Value;
                mySqlCommandSel.Parameters["@CiudadDir1"].Value = Datos.CiudadDir1.Value;
                mySqlCommandSel.Parameters["@DeptoDir1"].Value = Datos.DeptoDir1.Value;
                mySqlCommandSel.Parameters["@CodCiudadDir1"].Value = Datos.CodCiudadDir1.Value;
                mySqlCommandSel.Parameters["@CodDeptoDir1"].Value = Datos.CodDeptoDir1.Value;
                mySqlCommandSel.Parameters["@DireccionDir1"].Value = Datos.DireccionDir1.Value;
                mySqlCommandSel.Parameters["@TipoDir1"].Value = Datos.TipoDir1.Value;
                mySqlCommandSel.Parameters["@EstratoDir1"].Value = Datos.EstratoDir1.Value;
                mySqlCommandSel.Parameters["@RepDesdeDir1"].Value = Datos.RepDesdeDir1.Value;
                mySqlCommandSel.Parameters["@FechaActDir1"].Value = Datos.FechaActDir1.Value;
                mySqlCommandSel.Parameters["@NumEntidadDir1"].Value = Datos.NumEntidadDir1.Value;
                mySqlCommandSel.Parameters["@CiudadDir2"].Value = Datos.CiudadDir2.Value;
                mySqlCommandSel.Parameters["@DeptoDir2"].Value = Datos.DeptoDir2.Value;
                mySqlCommandSel.Parameters["@CodCiudadDir2"].Value = Datos.CodCiudadDir2.Value;
                mySqlCommandSel.Parameters["@CodDeptoDir2"].Value = Datos.CodDeptoDir2.Value;
                mySqlCommandSel.Parameters["@DireccionDir2"].Value = Datos.DireccionDir2.Value;
                mySqlCommandSel.Parameters["@TipoDir2"].Value = Datos.TipoDir2.Value;
                mySqlCommandSel.Parameters["@EstratoDir2"].Value = Datos.EstratoDir2.Value;
                mySqlCommandSel.Parameters["@RepDesdeDir2"].Value = Datos.RepDesdeDir2.Value;
                mySqlCommandSel.Parameters["@FechaActDir2"].Value = Datos.FechaActDir2.Value;
                mySqlCommandSel.Parameters["@NumEntidadDir2"].Value = Datos.NumEntidadDir2.Value;
                mySqlCommandSel.Parameters["@CiudadDir3"].Value = Datos.CiudadDir3.Value;
                mySqlCommandSel.Parameters["@DeptoDir3"].Value = Datos.DeptoDir3.Value;
                mySqlCommandSel.Parameters["@CodCiudadDir3"].Value = Datos.CodCiudadDir3.Value;
                mySqlCommandSel.Parameters["@CodDeptoDir3"].Value = Datos.CodDeptoDir3.Value;
                mySqlCommandSel.Parameters["@DireccionDir3"].Value = Datos.DireccionDir3.Value;
                mySqlCommandSel.Parameters["@TipoDir3"].Value = Datos.TipoDir3.Value;
                mySqlCommandSel.Parameters["@EstratoDir3"].Value = Datos.EstratoDir3.Value;
                mySqlCommandSel.Parameters["@RepDesdeDir3"].Value = Datos.RepDesdeDir3.Value;
                mySqlCommandSel.Parameters["@FechaActDir3"].Value = Datos.FechaActDir3.Value;
                mySqlCommandSel.Parameters["@NumEntidadDir3"].Value = Datos.NumEntidadDir3.Value;
                mySqlCommandSel.Parameters["@Email1"].Value = Datos.Email1.Value;
                mySqlCommandSel.Parameters["@RepDesdeMail1"].Value = Datos.RepDesdeMail1.Value;
                mySqlCommandSel.Parameters["@FechaActMail1"].Value = Datos.FechaActMail1.Value;
                mySqlCommandSel.Parameters["@NumEntidadMail1"].Value = Datos.NumEntidadMail1.Value;
                mySqlCommandSel.Parameters["@Email2"].Value = Datos.Email2.Value;
                mySqlCommandSel.Parameters["@RepDesdeMail2"].Value = Datos.RepDesdeMail2.Value;
                mySqlCommandSel.Parameters["@FechaActMail2"].Value = Datos.FechaActMail2.Value;
                mySqlCommandSel.Parameters["@NumEntidadMail2"].Value = Datos.NumEntidadMail2.Value;
                mySqlCommandSel.Parameters["@Celular1"].Value = Datos.Celular1.Value;
                mySqlCommandSel.Parameters["@RepDesdeCel1"].Value = Datos.RepDesdeCel1.Value;
                mySqlCommandSel.Parameters["@FechaActCel1"].Value = Datos.FechaActCel1.Value;
                mySqlCommandSel.Parameters["@NumEntidadCel1"].Value = Datos.NumEntidadCel1.Value;
                mySqlCommandSel.Parameters["@Celular2"].Value = Datos.Celular2.Value;
                mySqlCommandSel.Parameters["@RepDesdeCel2"].Value = Datos.RepDesdeCel2.Value;
                mySqlCommandSel.Parameters["@FechaActCel2"].Value = Datos.FechaActCel2.Value;
                mySqlCommandSel.Parameters["@NumEntidadCel2"].Value = Datos.NumEntidadCel2.Value;

                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;

                mySqlCommandSel.Parameters["@Direccion1IND"].Value = Datos.Direccion1IND.Value;
                mySqlCommandSel.Parameters["@Direccion2IND"].Value = Datos.Direccion2IND.Value;
                mySqlCommandSel.Parameters["@Direccion3IND"].Value = Datos.Direccion3IND.Value;
                mySqlCommandSel.Parameters["@DireccionOtraIND"].Value = Datos.DireccionOtraIND.Value;
                mySqlCommandSel.Parameters["@DireccionOtra"].Value = Datos.DireccionOtra.Value;
                mySqlCommandSel.Parameters["@Telefono1IND"].Value = Datos.Telefono1IND.Value;
                mySqlCommandSel.Parameters["@Telefono2IND"].Value = Datos.Telefono2IND.Value;
                mySqlCommandSel.Parameters["@Telefono3IND"].Value = Datos.Telefono3IND.Value;
                mySqlCommandSel.Parameters["@TelefonoOtroIND"].Value = Datos.TelefonoOtroIND.Value;
                mySqlCommandSel.Parameters["@TelefonoOtro"].Value = Datos.TelefonoOtro.Value;
                mySqlCommandSel.Parameters["@Celular1IND"].Value = Datos.Celular1IND.Value;
                mySqlCommandSel.Parameters["@Celular2IND"].Value = Datos.Celular2IND.Value;
                mySqlCommandSel.Parameters["@CelularOtraIND"].Value = Datos.CelularOtraIND.Value;
                mySqlCommandSel.Parameters["@CelularOtro"].Value = Datos.CelularOtro.Value;
                mySqlCommandSel.Parameters["@EMailOtroIND"].Value = Datos.EMailOtroIND.Value;
                mySqlCommandSel.Parameters["@EMailOtro"].Value = Datos.EMailOtro.Value;
                
                //Eg
                //mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);

                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommandSel.ExecuteNonQuery();
                int consec = Convert.ToInt32(mySqlCommandSel.Parameters["@Consecutivo"].Value);
                return consec;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoUbica_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDataCreditoUbica
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccSolicitudDataCreditoUbica_Update(DTO_ccSolicitudDataCreditoUbica Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                           "UPDATE ccSolicitudDataCreditoUbica SET  " +
                                                " NumeroDoc=@NumeroDoc" +
                                                " ,Version=@Version" +
                                                " ,TipoId=@TipoId" +
                                                " ,NumeroId=@NumeroId" +
                                                " ,Nombre=@Nombre" +
                                                " ,FechaExp=@FechaExp" +
                                                " ,CiudadExp=@CiudadExp" +
                                                " ,DeptoExp=@DeptoExp" +
                                                " ,Genero=@Genero" +
                                                " ,RangoEdad=@RangoEdad" +
                                                " ,CiudadTel1=@CiudadTel1" +
                                                " ,DeptoTel1=@DeptoTel1" +
                                                " ,CodCiudadTel1=@CodCiudadTel1" +
                                                " ,CodDeptoTel1=@CodDeptoTel1" +
                                                " ,NumeroTel1=@NumeroTel1" +
                                                " ,TipoTel1=@TipoTel1" +
                                                " ,RepDesdeTel1=@RepDesdeTel1" +
                                                " ,FechaActTel1=@FechaActTel1" +
                                                " ,NumEntidadTel1=@NumEntidadTel1" +
                                                " ,CiudadTel2=@CiudadTel2" +
                                                " ,DeptoTel2=@DeptoTel2" +
                                                " ,CodCiudadTel2=@CodCiudadTel2" +
                                                " ,CodDeptoTel2=@CodDeptoTel2" +
                                                " ,NumeroTel2=@NumeroTel2" +
                                                " ,TipoTel2=@TipoTel2" +
                                                " ,RepDesdeTel2=@RepDesdeTel2" +
                                                " ,FechaActTel2=@FechaActTel2" +
                                                " ,NumEntidadTel2=@NumEntidadTel2" +
                                                " ,CiudadTel3=@CiudadTel3" +
                                                " ,DeptoTel3=@DeptoTel3" +
                                                " ,CodCiudadTel3=@CodCiudadTel3" +
                                                " ,CodDeptoTel3=@CodDeptoTel3" +
                                                " ,NumeroTel3=@NumeroTel3" +
                                                " ,TipoTel3=@TipoTel3" +
                                                " ,RepDesdeTel3=@RepDesdeTel3" +
                                                " ,FechaActTel3=@FechaActTel3" +
                                                " ,NumEntidadTel3=@NumEntidadTel3" +
                                                " ,CiudadDir1=@CiudadDir1" +
                                                " ,DeptoDir1=@DeptoDir1" +
                                                " ,CodCiudadDir1=@CodCiudadDir1" +
                                                " ,CodDeptoDir1=@CodDeptoDir1" +
                                                " ,DireccionDir1=@DireccionDir1" +
                                                " ,TipoDir1=@TipoDir1" +
                                                " ,EstratoDir1=@EstratoDir1" +
                                                " ,RepDesdeDir1=@RepDesdeDir1" +
                                                " ,FechaActDir1=@FechaActDir1" +
                                                " ,NumEntidadDir1=@NumEntidadDir1" +
                                                " ,CiudadDir2=@CiudadDir2" +
                                                " ,DeptoDir2=@DeptoDir2" +
                                                " ,CodCiudadDir2=@CodCiudadDir2" +
                                                " ,CodDeptoDir2=@CodDeptoDir2" +
                                                " ,DireccionDir2=@DireccionDir2" +
                                                " ,TipoDir2=@TipoDir2" +
                                                " ,EstratoDir2=@EstratoDir2" +
                                                " ,RepDesdeDir2=@RepDesdeDir2" +
                                                " ,FechaActDir2=@FechaActDir2" +
                                                " ,NumEntidadDir2=@NumEntidadDir2" +
                                                " ,CiudadDir3=@CiudadDir3" +
                                                " ,DeptoDir3=@DeptoDir3" +
                                                " ,CodCiudadDir3=@CodCiudadDir3" +
                                                " ,CodDeptoDir3=@CodDeptoDir3" +
                                                " ,DireccionDir3=@DireccionDir3" +
                                                " ,TipoDir3=@TipoDir3" +
                                                " ,EstratoDir3=@EstratoDir3" +
                                                " ,RepDesdeDir3=@RepDesdeDir3" +
                                                " ,FechaActDir3=@FechaActDir3" +
                                                " ,NumEntidadDir3=@NumEntidadDir3" +
                                                " ,Email1=@Email1" +
                                                " ,RepDesdeMail1=@RepDesdeMail1" +
                                                " ,FechaActMail1=@FechaActMail1" +
                                                " ,NumEntidadMail1=@NumEntidadMail1" +
                                                " ,Email2=@Email2" +
                                                " ,RepDesdeMail2=@RepDesdeMail2" +
                                                " ,FechaActMail2=@FechaActMail2" +
                                                " ,NumEntidadMail2=@NumEntidadMail2" +
                                                " ,Celular1=@Celular1" +
                                                " ,RepDesdeCel1=@RepDesdeCel1" +
                                                " ,FechaActCel1=@FechaActCel1" +
                                                " ,NumEntidadCel1=@NumEntidadCel1" +
                                                " ,Celular2=@Celular2" +
                                                " ,RepDesdeCel2=@RepDesdeCel2" +
                                                " ,FechaActCel2=@FechaActCel2" +
                                                " ,NumEntidadCel2=@NumEntidadCel2" +
                                                " ,Direccion1IND=@Direccion1IND" +
                                                " ,Direccion2IND=@Direccion2IND" +
                                                " ,Direccion3IND=@Direccion3IND" +
                                                " ,DireccionOtraIND=@DireccionOtraIND" +
                                                " ,DireccionOtra=@DireccionOtra" +
                                                " ,Telefono1IND=@Telefono1IND" +
                                                " ,Telefono2IND=@Telefono2IND" +
                                                " ,Telefono3IND=@Telefono3IND" +
                                                " ,TelefonoOtroIND=@TelefonoOtroIND" +
                                                " ,TelefonoOtro=@TelefonoOtro" +
                                                " ,Celular1IND=@Celular1IND" +
                                                " ,Celular2IND=@Celular2IND" +
                                                " ,CelularOtraIND=@CelularOtraIND" +
                                                " ,CelularOtro=@CelularOtro" +
                                                " ,EMailOtroIND=@EMailOtroIND" +
                                                " ,EMailOtro=@EMailOtro" +
                                                " WHERE Consecutivo = @Consecutivo";
                #endregion,
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoId", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@NumeroId", SqlDbType.Char, 11);

                mySqlCommandSel.Parameters.Add("@Nombre", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@FechaExp", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CiudadExp", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DeptoExp", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Genero", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@RangoEdad", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@CiudadTel1", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DeptoTel1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@CodCiudadTel1", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@CodDeptoTel1", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@NumeroTel1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@TipoTel1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@RepDesdeTel1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActTel1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadTel1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadTel2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DeptoTel2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@CodCiudadTel2", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@CodDeptoTel2", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@NumeroTel2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@TipoTel2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@RepDesdeTel2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActTel2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadTel2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadTel3", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DeptoTel3", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@CodCiudadTel3", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@CodDeptoTel3", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@NumeroTel3", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@TipoTel3", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@RepDesdeTel3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActTel3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadTel3", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadDir1", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DeptoDir1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@CodCiudadDir1", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@CodDeptoDir1", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@DireccionDir1", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@TipoDir1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@EstratoDir1", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@RepDesdeDir1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActDir1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadDir1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadDir2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DeptoDir2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@CodCiudadDir2", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@CodDeptoDir2", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@DireccionDir2", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@TipoDir2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@EstratoDir2", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@RepDesdeDir2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActDir2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadDir2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CiudadDir3", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DeptoDir3", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@CodCiudadDir3", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@CodDeptoDir3", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@DireccionDir3", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@TipoDir3", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@EstratoDir3", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@RepDesdeDir3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActDir3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadDir3", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Email1", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@RepDesdeMail1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActMail1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadMail1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Email2", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@RepDesdeMail2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActMail2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadMail2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Celular1", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@RepDesdeCel1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActCel1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadCel1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Celular2", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@RepDesdeCel2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaActCel2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumEntidadCel2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);

                mySqlCommandSel.Parameters.Add("@Direccion1IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Direccion2IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Direccion3IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DireccionOtra", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@DireccionOtraIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Telefono1IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Telefono2IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Telefono3IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@TelefonoOtroIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@TelefonoOtro", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@Celular1IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Celular2IND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CelularOtraIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CelularOtro", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@EMailOtroIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EMailOtro", SqlDbType.Char, 100);


                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoId"].Value = Datos.TipoId.Value;
                mySqlCommandSel.Parameters["@NumeroId"].Value = Datos.NumeroId.Value;
                mySqlCommandSel.Parameters["@Nombre"].Value = Datos.Nombre.Value;
                mySqlCommandSel.Parameters["@FechaExp"].Value = Datos.FechaExp.Value;
                mySqlCommandSel.Parameters["@CiudadExp"].Value = Datos.CiudadExp.Value;
                mySqlCommandSel.Parameters["@DeptoExp"].Value = Datos.DeptoExp.Value;
                mySqlCommandSel.Parameters["@Genero"].Value = Datos.Genero.Value;
                mySqlCommandSel.Parameters["@RangoEdad"].Value = Datos.RangoEdad.Value;
                mySqlCommandSel.Parameters["@CiudadTel1"].Value = Datos.CiudadTel1.Value;
                mySqlCommandSel.Parameters["@DeptoTel1"].Value = Datos.DeptoTel1.Value;
                mySqlCommandSel.Parameters["@CodCiudadTel1"].Value = Datos.CodCiudadTel1.Value;
                mySqlCommandSel.Parameters["@CodDeptoTel1"].Value = Datos.CodDeptoTel1.Value;
                mySqlCommandSel.Parameters["@NumeroTel1"].Value = Datos.NumeroTel1.Value;
                mySqlCommandSel.Parameters["@TipoTel1"].Value = Datos.TipoTel1.Value;
                mySqlCommandSel.Parameters["@RepDesdeTel1"].Value = Datos.RepDesdeTel1.Value;
                mySqlCommandSel.Parameters["@FechaActTel1"].Value = Datos.FechaActTel1.Value;
                mySqlCommandSel.Parameters["@NumEntidadTel1"].Value = Datos.NumEntidadTel1.Value;
                mySqlCommandSel.Parameters["@CiudadTel2"].Value = Datos.CiudadTel2.Value;
                mySqlCommandSel.Parameters["@DeptoTel2"].Value = Datos.DeptoTel2.Value;
                mySqlCommandSel.Parameters["@CodCiudadTel2"].Value = Datos.CodCiudadTel2.Value;
                mySqlCommandSel.Parameters["@CodDeptoTel2"].Value = Datos.CodDeptoTel2.Value;
                mySqlCommandSel.Parameters["@NumeroTel2"].Value = Datos.NumeroTel2.Value;
                mySqlCommandSel.Parameters["@TipoTel2"].Value = Datos.TipoTel2.Value;
                mySqlCommandSel.Parameters["@RepDesdeTel2"].Value = Datos.RepDesdeTel2.Value;
                mySqlCommandSel.Parameters["@FechaActTel2"].Value = Datos.FechaActTel2.Value;
                mySqlCommandSel.Parameters["@NumEntidadTel2"].Value = Datos.NumEntidadTel2.Value;
                mySqlCommandSel.Parameters["@CiudadTel3"].Value = Datos.CiudadTel3.Value;
                mySqlCommandSel.Parameters["@DeptoTel3"].Value = Datos.DeptoTel3.Value;
                mySqlCommandSel.Parameters["@CodCiudadTel3"].Value = Datos.CodCiudadTel3.Value;
                mySqlCommandSel.Parameters["@CodDeptoTel3"].Value = Datos.CodDeptoTel3.Value;
                mySqlCommandSel.Parameters["@NumeroTel3"].Value = Datos.NumeroTel3.Value;
                mySqlCommandSel.Parameters["@TipoTel3"].Value = Datos.TipoTel3.Value;
                mySqlCommandSel.Parameters["@RepDesdeTel3"].Value = Datos.RepDesdeTel3.Value;
                mySqlCommandSel.Parameters["@FechaActTel3"].Value = Datos.FechaActTel3.Value;
                mySqlCommandSel.Parameters["@NumEntidadTel3"].Value = Datos.NumEntidadTel3.Value;
                mySqlCommandSel.Parameters["@CiudadDir1"].Value = Datos.CiudadDir1.Value;
                mySqlCommandSel.Parameters["@DeptoDir1"].Value = Datos.DeptoDir1.Value;
                mySqlCommandSel.Parameters["@CodCiudadDir1"].Value = Datos.CodCiudadDir1.Value;
                mySqlCommandSel.Parameters["@CodDeptoDir1"].Value = Datos.CodDeptoDir1.Value;
                mySqlCommandSel.Parameters["@DireccionDir1"].Value = Datos.DireccionDir1.Value;
                mySqlCommandSel.Parameters["@TipoDir1"].Value = Datos.TipoDir1.Value;
                mySqlCommandSel.Parameters["@EstratoDir1"].Value = Datos.EstratoDir1.Value;
                mySqlCommandSel.Parameters["@RepDesdeDir1"].Value = Datos.RepDesdeDir1.Value;
                mySqlCommandSel.Parameters["@FechaActDir1"].Value = Datos.FechaActDir1.Value;
                mySqlCommandSel.Parameters["@NumEntidadDir1"].Value = Datos.NumEntidadDir1.Value;
                mySqlCommandSel.Parameters["@CiudadDir2"].Value = Datos.CiudadDir2.Value;
                mySqlCommandSel.Parameters["@DeptoDir2"].Value = Datos.DeptoDir2.Value;
                mySqlCommandSel.Parameters["@CodCiudadDir2"].Value = Datos.CodCiudadDir2.Value;
                mySqlCommandSel.Parameters["@CodDeptoDir2"].Value = Datos.CodDeptoDir2.Value;
                mySqlCommandSel.Parameters["@DireccionDir2"].Value = Datos.DireccionDir2.Value;
                mySqlCommandSel.Parameters["@TipoDir2"].Value = Datos.TipoDir2.Value;
                mySqlCommandSel.Parameters["@EstratoDir2"].Value = Datos.EstratoDir2.Value;
                mySqlCommandSel.Parameters["@RepDesdeDir2"].Value = Datos.RepDesdeDir2.Value;
                mySqlCommandSel.Parameters["@FechaActDir2"].Value = Datos.FechaActDir2.Value;
                mySqlCommandSel.Parameters["@NumEntidadDir2"].Value = Datos.NumEntidadDir2.Value;
                mySqlCommandSel.Parameters["@CiudadDir3"].Value = Datos.CiudadDir3.Value;
                mySqlCommandSel.Parameters["@DeptoDir3"].Value = Datos.DeptoDir3.Value;
                mySqlCommandSel.Parameters["@CodCiudadDir3"].Value = Datos.CodCiudadDir3.Value;
                mySqlCommandSel.Parameters["@CodDeptoDir3"].Value = Datos.CodDeptoDir3.Value;
                mySqlCommandSel.Parameters["@DireccionDir3"].Value = Datos.DireccionDir3.Value;
                mySqlCommandSel.Parameters["@TipoDir3"].Value = Datos.TipoDir3.Value;
                mySqlCommandSel.Parameters["@EstratoDir3"].Value = Datos.EstratoDir3.Value;
                mySqlCommandSel.Parameters["@RepDesdeDir3"].Value = Datos.RepDesdeDir3.Value;
                mySqlCommandSel.Parameters["@FechaActDir3"].Value = Datos.FechaActDir3.Value;
                mySqlCommandSel.Parameters["@NumEntidadDir3"].Value = Datos.NumEntidadDir3.Value;
                mySqlCommandSel.Parameters["@Email1"].Value = Datos.Email1.Value;
                mySqlCommandSel.Parameters["@RepDesdeMail1"].Value = Datos.RepDesdeMail1.Value;
                mySqlCommandSel.Parameters["@FechaActMail1"].Value = Datos.FechaActMail1.Value;
                mySqlCommandSel.Parameters["@NumEntidadMail1"].Value = Datos.NumEntidadMail1.Value;
                mySqlCommandSel.Parameters["@Email2"].Value = Datos.Email2.Value;
                mySqlCommandSel.Parameters["@RepDesdeMail2"].Value = Datos.RepDesdeMail2.Value;
                mySqlCommandSel.Parameters["@FechaActMail2"].Value = Datos.FechaActMail2.Value;
                mySqlCommandSel.Parameters["@NumEntidadMail2"].Value = Datos.NumEntidadMail2.Value;
                mySqlCommandSel.Parameters["@Celular1"].Value = Datos.Celular1.Value;
                mySqlCommandSel.Parameters["@RepDesdeCel1"].Value = Datos.RepDesdeCel1.Value;
                mySqlCommandSel.Parameters["@FechaActCel1"].Value = Datos.FechaActCel1.Value;
                mySqlCommandSel.Parameters["@NumEntidadCel1"].Value = Datos.NumEntidadCel1.Value;
                mySqlCommandSel.Parameters["@Celular2"].Value = Datos.Celular2.Value;
                mySqlCommandSel.Parameters["@RepDesdeCel2"].Value = Datos.RepDesdeCel2.Value;
                mySqlCommandSel.Parameters["@FechaActCel2"].Value = Datos.FechaActCel2.Value;
                mySqlCommandSel.Parameters["@NumEntidadCel2"].Value = Datos.NumEntidadCel2.Value;

                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;

                mySqlCommandSel.Parameters["@Direccion1IND"].Value = Datos.Direccion1IND.Value;
                mySqlCommandSel.Parameters["@Direccion2IND"].Value = Datos.Direccion2IND.Value;
                mySqlCommandSel.Parameters["@Direccion3IND"].Value = Datos.Direccion3IND.Value;
                mySqlCommandSel.Parameters["@DireccionOtraIND"].Value = Datos.DireccionOtraIND.Value;
                mySqlCommandSel.Parameters["@DireccionOtra"].Value = Datos.DireccionOtra.Value;
                mySqlCommandSel.Parameters["@Telefono1IND"].Value = Datos.Telefono1IND.Value;
                mySqlCommandSel.Parameters["@Telefono2IND"].Value = Datos.Telefono2IND.Value;
                mySqlCommandSel.Parameters["@Telefono3IND"].Value = Datos.Telefono3IND.Value;
                mySqlCommandSel.Parameters["@TelefonoOtroIND"].Value = Datos.TelefonoOtroIND.Value;
                mySqlCommandSel.Parameters["@TelefonoOtro"].Value = Datos.TelefonoOtro.Value;
                mySqlCommandSel.Parameters["@Celular1IND"].Value = Datos.Celular1IND.Value;
                mySqlCommandSel.Parameters["@Celular2IND"].Value = Datos.Celular2IND.Value;
                mySqlCommandSel.Parameters["@CelularOtraIND"].Value = Datos.CelularOtraIND.Value;
                mySqlCommandSel.Parameters["@CelularOtro"].Value = Datos.CelularOtro.Value;
                mySqlCommandSel.Parameters["@EMailOtroIND"].Value = Datos.EMailOtroIND.Value;
                mySqlCommandSel.Parameters["@EMailOtro"].Value = Datos.EMailOtro.Value;
                
                //Eg
                //mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);

                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoUbica_Update");
                throw exception;
            }
        }

        public void DAL_ccSolicitudDataCreditoUbica_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudDataCreditoUbica WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoUbica_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto</returns>
        public bool DAL_ccSolicitudDataCreditoUbica_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from ccSolicitudDataCreditoUbica with(nolock) where Consecutivo = @Consecutivo";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoUbica_Exist");
                throw exception;
            }
        }
        #endregion

       
    }
}
