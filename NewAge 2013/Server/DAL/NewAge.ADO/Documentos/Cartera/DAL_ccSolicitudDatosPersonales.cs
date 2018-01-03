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
    public class DAL_ccSolicitudDatosPersonales : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudDatosPersonales(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudDatosPersonales que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudDatosPersonales</returns>
        public List<DTO_ccSolicitudDatosPersonales> DAL_ccSolicitudDatosPersonales_GetAll()
        {
            try
            {
                List<DTO_ccSolicitudDatosPersonales> result = new List<DTO_ccSolicitudDatosPersonales>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT Datos.* FROM ccSolicitudDatosPersonales Datos with(nolock)";
                                       
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDatosPersonales Datos;
                    Datos = new DTO_ccSolicitudDatosPersonales(dr);                
                    result.Add(Datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDatosPersonales_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_ccSolicitudDatosPersonales> DAL_ccSolicitudDatosPersonales_GetByNUmeroDoc(int numeroDoc, Int16? versionNro)
        {
            try
            {
                List<DTO_ccSolicitudDatosPersonales> result = new List<DTO_ccSolicitudDatosPersonales>();

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
                    mySqlCommand.CommandText = "select * from ccSolicitudDatosPersonales  with(nolock) " +
                                         " Where NumeroDoc =  @NumeroDoc and Version =  @Version  ";
                else
                    mySqlCommand.CommandText = "select * from ccSolicitudDatosPersonales  with(nolock) " +
                                        " Where NumeroDoc =  @NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDatosPersonales(dr));
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
        /// Agrega informacion a la tabla ccSolicitudDatosPersonales
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_ccSolicitudDatosPersonales_Add(DTO_ccSolicitudDatosPersonales Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccSolicitudDatosPersonales " +
                    "( " +
                    "   NumeroDoc,Version,TipoPersona,TerceroID,TerceroDocTipoID,FechaExpDoc,CiudadExpDoc,FechaNacimiento,Descriptivo,ApellidoPri," +
                    "   ApellidoSdo,NombrePri,NombreSdo,Celular1,Celular2,CorreoElectronico,DirResidencia,BarrioResidencia,CiudadResidencia,AntResidencia,"+
                    "   TipoVivienda,TelResidencia,LugarTrabajo,DirTrabajo,BarrioTrabajo,CiudadTrabajo,TelTrabajo,Cargo,AntTrabajo,TipoContrato,EPS,"+
                    "   Personascargo,Conyugue,NombreConyugue,ActConyugue,AntConyugue,EmpresaConyugue,DirResConyugue,TelefonoConyugue,CelularConyugue,NombreReferencia1," +
                    "   RelReferencia1,TipoReferencia1,DirReferencia1,BarrioReferencia1,TelefonoReferencia1,CelularReferencia1,NombreReferencia2,"+
                    "   RelReferencia2,TipoReferencia2,DirReferencia2,BarrioReferencia2,TelefonoReferencia2,CelularReferencia2,"+
                    "   VlrActivos,VlrPasivos,VlrPatrimonio,VlrEgresosMes,VlrIngresosMes,VlrIngresosNoOpe,DescrOtrosIng,DescrOtrosBinenes,EntCredito1,"+
                    "   Plazo1,Saldo1,EntCredito2,Plazo2,Saldo2,SolicitudInd1,DeclFondos,BR_Direccion,BR_Valor,BR_AfectacionFamInd,BR_HipotecaInd,"+
                    "   BR_HipotecaNombre,VE_Marca,VE_Clase,VE_Modelo,VE_Placa,VE_PignoradoInd,VE_PignoradoNombre,VE_Valor,UsuarioDigita,FechaDigita,"+
                    "   UsuarioVerifica,FechaVerifica,UsuarioConfirma,FechaConfirma,EstadoCivil,Sexo,DataCreditoDireccion,DataCreditoTelefono,DataCreditoCelular,DataCreditoCorreo" +
                    ") " +
                    "VALUES " +
                    "( " +
                    "   @NumeroDoc,@Version,@TipoPersona,@TerceroID,@TerceroDocTipoID,@FechaExpDoc,@CiudadExpDoc,@FechaNacimiento,@Descriptivo,@ApellidoPri," +
                    "   @ApellidoSdo,@NombrePri,@NombreSdo,@Celular1,@Celular2,@CorreoElectronico,@DirResidencia,@BarrioResidencia,@CiudadResidencia,@AntResidencia,"+
                    "   @TipoVivienda,@TelResidencia,@LugarTrabajo,@DirTrabajo,@BarrioTrabajo,@CiudadTrabajo,@TelTrabajo,@Cargo,@AntTrabajo,@TipoContrato,@EPS,"+
                    "   @Personascargo,@Conyugue,@NombreConyugue,@ActConyugue,@AntConyugue,@EmpresaConyugue, @DirResConyugue,@TelefonoConyugue,@CelularConyugue,@NombreReferencia1," +
                    "   @RelReferencia1,@TipoReferencia1,@DirReferencia1,@BarrioReferencia1,@TelefonoReferencia1,@CelularReferencia1,@NombreReferencia2,"+
                    "   @RelReferencia2,@TipoReferencia2,@DirReferencia2,@BarrioReferencia2,@TelefonoReferencia2,@CelularReferencia2,"+
                    "   @VlrActivos,@VlrPasivos,@VlrPatrimonio,@VlrEgresosMes,@VlrIngresosMes,@VlrIngresosNoOpe,@DescrOtrosIng,@DescrOtrosBinenes,@EntCredito1,"+
                    "   @Plazo1,@Saldo1,@EntCredito2,@Plazo2,@Saldo2,@SolicitudInd1,@DeclFondos,@BR_Direccion,@BR_Valor,@BR_AfectacionFamInd,@BR_HipotecaInd,"+
                    "   @BR_HipotecaNombre,@VE_Marca,@VE_Clase,@VE_Modelo,@VE_Placa,@VE_PignoradoInd,@VE_PignoradoNombre,@VE_Valor,@UsuarioDigita,@FechaDigita,"+
                    "   @UsuarioVerifica,@FechaVerifica,@UsuarioConfirma,@FechaConfirma,@EstadoCivil,@Sexo,@DataCreditoDireccion,@DataCreditoTelefono,@DataCreditoCelular,@DataCreditoCorreo" +
                ") SET @Consecutivo = SCOPE_IDENTITY() ";
                #endregion
                #region Creacion de comandos                
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);          
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoPersona", SqlDbType.TinyInt);                
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char,UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroDocTipoID", SqlDbType.Char,UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaExpDoc", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CiudadExpDoc", SqlDbType.Char,UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaNacimiento", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Descriptivo",SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ApellidoPri", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ApellidoSdo", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@NombrePri", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@NombreSdo", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@Celular1", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@Celular2", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@CorreoElectronico", SqlDbType.Char,60);
                mySqlCommandSel.Parameters.Add("@DirResidencia", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BarrioResidencia", SqlDbType.Char,60);
                mySqlCommandSel.Parameters.Add("@CiudadResidencia", SqlDbType.Char,UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AntResidencia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoVivienda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TelResidencia", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@LugarTrabajo", SqlDbType.Char,60);
                mySqlCommandSel.Parameters.Add("@DirTrabajo", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BarrioTrabajo", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@CiudadTrabajo", SqlDbType.Char,UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TelTrabajo", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@Cargo", SqlDbType.Char,40);
                mySqlCommandSel.Parameters.Add("@AntTrabajo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoContrato", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EPS", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@Personascargo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Conyugue", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@NombreConyugue", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@ActConyugue", SqlDbType.Char,40);
                mySqlCommandSel.Parameters.Add("@AntConyugue", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EmpresaConyugue", SqlDbType.VarChar,50);
                mySqlCommandSel.Parameters.Add("@DirResConyugue", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@TelefonoConyugue", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CelularConyugue", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@NombreReferencia1", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@RelReferencia1", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@TipoReferencia1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DirReferencia1", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BarrioReferencia1", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@TelefonoReferencia1", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CelularReferencia1", SqlDbType.Char,20);                
                mySqlCommandSel.Parameters.Add("@NombreReferencia2", SqlDbType.Char,100);   
                mySqlCommandSel.Parameters.Add("@RelReferencia2", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@TipoReferencia2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DirReferencia2", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BarrioReferencia2", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@TelefonoReferencia2", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CelularReferencia2", SqlDbType.Char,20);                
                mySqlCommandSel.Parameters.Add("@VlrActivos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPasivos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPatrimonio", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrEgresosMes", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrIngresosMes", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrIngresosNoOpe", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DescrOtrosIng", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@DescrOtrosBinenes", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@EntCredito1", SqlDbType.VarChar,50);
                mySqlCommandSel.Parameters.Add("@Plazo1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Saldo1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EntCredito2", SqlDbType.VarChar,50);
                mySqlCommandSel.Parameters.Add("@Plazo2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Saldo2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SolicitudInd1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DeclFondos", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BR_Direccion", SqlDbType.VarChar,60);
                mySqlCommandSel.Parameters.Add("@BR_Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@BR_AfectacionFamInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@BR_HipotecaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@BR_HipotecaNombre", SqlDbType.VarChar,60);
                mySqlCommandSel.Parameters.Add("@VE_Marca", SqlDbType.VarChar,30);
                mySqlCommandSel.Parameters.Add("@VE_Clase", SqlDbType.VarChar,30);
                mySqlCommandSel.Parameters.Add("@VE_Modelo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VE_Placa", SqlDbType.Char,6);
                mySqlCommandSel.Parameters.Add("@VE_PignoradoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VE_PignoradoNombre", SqlDbType.VarChar,60);
                mySqlCommandSel.Parameters.Add("@VE_Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@UsuarioDigita", SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaDigita", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioVerifica",SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaVerifica", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioConfirma", SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaConfirma", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@EstadoCivil", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Sexo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);

                mySqlCommandSel.Parameters.Add("@DataCreditoDireccion", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DataCreditoTelefono", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DataCreditoCelular", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DataCreditoCorreo", SqlDbType.TinyInt);

                #endregion
                #region Asigna los valores                
                mySqlCommandSel.Parameters["@NumeroDoc" ].Value= Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version" ].Value= Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoPersona" ].Value= Datos.TipoPersona.Value;                
                mySqlCommandSel.Parameters["@TerceroID" ].Value= Datos.TerceroID.Value;
                mySqlCommandSel.Parameters["@TerceroDocTipoID" ].Value= Datos.TerceroDocTipoID.Value;
                mySqlCommandSel.Parameters["@FechaExpDoc" ].Value= Datos.FechaExpDoc.Value;
                mySqlCommandSel.Parameters["@CiudadExpDoc"].Value = Datos.CiudadExpDoc.Value;                
                mySqlCommandSel.Parameters["@FechaNacimiento" ].Value= Datos.FechaNacimiento.Value;
                mySqlCommandSel.Parameters["@Descriptivo" ].Value= Datos.Descriptivo.Value;
                mySqlCommandSel.Parameters["@ApellidoPri" ].Value= Datos.ApellidoPri.Value;
                mySqlCommandSel.Parameters["@ApellidoSdo" ].Value= Datos.ApellidoSdo.Value;
                mySqlCommandSel.Parameters["@NombrePri" ].Value= Datos.NombrePri.Value;
                mySqlCommandSel.Parameters["@NombreSdo" ].Value= Datos.NombreSdo.Value;
                mySqlCommandSel.Parameters["@Celular1" ].Value= Datos.Celular1.Value;
                mySqlCommandSel.Parameters["@Celular2" ].Value= Datos.Celular2.Value;
                mySqlCommandSel.Parameters["@CorreoElectronico" ].Value= Datos.CorreoElectronico.Value;
                mySqlCommandSel.Parameters["@DirResidencia" ].Value= Datos.DirResidencia.Value;
                mySqlCommandSel.Parameters["@BarrioResidencia" ].Value= Datos.BarrioResidencia.Value;
                mySqlCommandSel.Parameters["@CiudadResidencia" ].Value= Datos.CiudadResidencia.Value;
                mySqlCommandSel.Parameters["@AntResidencia" ].Value= Datos.AntResidencia.Value;
                mySqlCommandSel.Parameters["@TipoVivienda" ].Value= Datos.TipoVivienda.Value;
                mySqlCommandSel.Parameters["@TelResidencia" ].Value= Datos.TelResidencia.Value;
                mySqlCommandSel.Parameters["@LugarTrabajo" ].Value= Datos.LugarTrabajo.Value;
                mySqlCommandSel.Parameters["@DirTrabajo" ].Value= Datos.DirTrabajo.Value;
                mySqlCommandSel.Parameters["@BarrioTrabajo" ].Value= Datos.BarrioTrabajo.Value;
                mySqlCommandSel.Parameters["@CiudadTrabajo" ].Value= Datos.CiudadTrabajo.Value;
                mySqlCommandSel.Parameters["@TelTrabajo" ].Value= Datos.TelTrabajo.Value;
                mySqlCommandSel.Parameters["@Cargo" ].Value= Datos.Cargo.Value;
                mySqlCommandSel.Parameters["@AntTrabajo" ].Value= Datos.AntTrabajo.Value;
                mySqlCommandSel.Parameters["@TipoContrato" ].Value= Datos.TipoContrato.Value;
                mySqlCommandSel.Parameters["@EPS" ].Value= Datos.EPS.Value;
                mySqlCommandSel.Parameters["@Personascargo" ].Value= Datos.Personascargo.Value;
                mySqlCommandSel.Parameters["@Conyugue" ].Value= Datos.Conyugue.Value;
                mySqlCommandSel.Parameters["@NombreConyugue" ].Value= Datos.NombreConyugue.Value;
                mySqlCommandSel.Parameters["@ActConyugue" ].Value= Datos.ActConyugue.Value;
                mySqlCommandSel.Parameters["@AntConyugue" ].Value= Datos.AntConyugue.Value;
                mySqlCommandSel.Parameters["@EmpresaConyugue"].Value = Datos.EmpresaConyugue.Value;
                mySqlCommandSel.Parameters["@DirResConyugue" ].Value= Datos.DirResConyugue.Value;
                mySqlCommandSel.Parameters["@TelefonoConyugue" ].Value= Datos.TelefonoConyugue.Value;
                mySqlCommandSel.Parameters["@CelularConyugue" ].Value= Datos.CelularConyugue.Value;
                mySqlCommandSel.Parameters["@NombreReferencia1" ].Value= Datos.NombreReferencia1.Value;
                mySqlCommandSel.Parameters["@RelReferencia1" ].Value= Datos.RelReferencia1.Value;
                mySqlCommandSel.Parameters["@TipoReferencia1" ].Value= Datos.TipoReferencia1.Value;
                mySqlCommandSel.Parameters["@DirReferencia1" ].Value= Datos.DirReferencia1.Value;
                mySqlCommandSel.Parameters["@BarrioReferencia1" ].Value= Datos.BarrioReferencia1.Value;
                mySqlCommandSel.Parameters["@TelefonoReferencia1" ].Value= Datos.TelefonoReferencia1.Value;
                mySqlCommandSel.Parameters["@CelularReferencia1" ].Value= Datos.CelularReferencia1.Value;                
                mySqlCommandSel.Parameters["@NombreReferencia2"].Value= Datos.NombreReferencia2.Value;
                mySqlCommandSel.Parameters["@RelReferencia2"].Value= Datos.RelReferencia2.Value;
                mySqlCommandSel.Parameters["@TipoReferencia2"].Value= Datos.TipoReferencia2.Value;
                mySqlCommandSel.Parameters["@DirReferencia2"].Value= Datos.DirReferencia2.Value;
                mySqlCommandSel.Parameters["@BarrioReferencia2"].Value= Datos.BarrioReferencia2.Value;
                mySqlCommandSel.Parameters["@TelefonoReferencia2"].Value= Datos.TelefonoReferencia2.Value;
                mySqlCommandSel.Parameters["@CelularReferencia2"].Value= Datos.CelularReferencia2.Value;
                mySqlCommandSel.Parameters["@VlrActivos"].Value= Datos.VlrActivos.Value;
                mySqlCommandSel.Parameters["@VlrPasivos"].Value= Datos.VlrPasivos.Value;
                mySqlCommandSel.Parameters["@VlrPatrimonio"].Value= Datos.VlrPatrimonio.Value;
                mySqlCommandSel.Parameters["@VlrEgresosMes"].Value= Datos.VlrEgresosMes.Value;
                mySqlCommandSel.Parameters["@VlrIngresosMes"].Value= Datos.VlrIngresosMes.Value;
                mySqlCommandSel.Parameters["@VlrIngresosNoOpe"].Value= Datos.VlrIngresosNoOpe.Value;
                mySqlCommandSel.Parameters["@DescrOtrosIng"].Value= Datos.DescrOtrosIng.Value;
                mySqlCommandSel.Parameters["@DescrOtrosBinenes"].Value= Datos.DescrOtrosBinenes.Value;
                mySqlCommandSel.Parameters["@EntCredito1"].Value= Datos.EntCredito1.Value;
                mySqlCommandSel.Parameters["@Plazo1"].Value= Datos.Plazo1.Value;
                mySqlCommandSel.Parameters["@Saldo1"].Value= Datos.Saldo1.Value;
                mySqlCommandSel.Parameters["@EntCredito2"].Value= Datos.EntCredito2.Value;
                mySqlCommandSel.Parameters["@Plazo2"].Value= Datos.Plazo2.Value;
                mySqlCommandSel.Parameters["@Saldo2"].Value= Datos.Saldo2.Value;
                mySqlCommandSel.Parameters["@SolicitudInd1"].Value= Datos.SolicitudInd1.Value;
                mySqlCommandSel.Parameters["@DeclFondos"].Value= Datos.DeclFondos.Value;
                mySqlCommandSel.Parameters["@BR_Direccion"].Value= Datos.BR_Direccion.Value;
                mySqlCommandSel.Parameters["@BR_Valor"].Value= Datos.BR_Valor.Value;
                mySqlCommandSel.Parameters["@BR_AfectacionFamInd"].Value= Datos.BR_AfectacionFamInd.Value;
                mySqlCommandSel.Parameters["@BR_HipotecaInd"].Value= Datos.BR_HipotecaInd.Value;
                mySqlCommandSel.Parameters["@BR_HipotecaNombre"].Value= Datos.BR_HipotecaNombre.Value;
                mySqlCommandSel.Parameters["@VE_Marca"].Value= Datos.VE_Marca.Value;
                mySqlCommandSel.Parameters["@VE_Clase"].Value= Datos.VE_Clase.Value;
                mySqlCommandSel.Parameters["@VE_Modelo"].Value= Datos.VE_Modelo.Value;
                mySqlCommandSel.Parameters["@VE_Placa"].Value= Datos.VE_Placa.Value;
                mySqlCommandSel.Parameters["@VE_PignoradoInd"].Value= Datos.VE_PignoradoInd.Value;
                mySqlCommandSel.Parameters["@VE_PignoradoNombre"].Value= Datos.VE_PignoradoNombre.Value;
                mySqlCommandSel.Parameters["@VE_Valor"].Value= Datos.VE_Valor.Value;
                mySqlCommandSel.Parameters["@UsuarioDigita"].Value= Datos.UsuarioDigita.Value;
                mySqlCommandSel.Parameters["@FechaDigita"].Value= Datos.FechaDigita.Value;
                mySqlCommandSel.Parameters["@UsuarioVerifica"].Value= Datos.UsuarioVerifica.Value;
                mySqlCommandSel.Parameters["@FechaVerifica"].Value= Datos.FechaVerifica.Value;
                mySqlCommandSel.Parameters["@UsuarioConfirma"].Value= Datos.UsuarioConfirma.Value;
                mySqlCommandSel.Parameters["@FechaConfirma"].Value= Datos.FechaConfirma.Value;
                mySqlCommandSel.Parameters["@Sexo"].Value = Datos.Sexo.Value;
                mySqlCommandSel.Parameters["@EstadoCivil"].Value = Datos.EstadoCivil.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                
                mySqlCommandSel.Parameters["@DataCreditoDireccion"].Value = Datos.DataCreditoDireccion.Value;
                mySqlCommandSel.Parameters["@DataCreditoTelefono"].Value = Datos.DataCreditoTelefono.Value;
                mySqlCommandSel.Parameters["@DataCreditoCelular"].Value = Datos.DataCreditoCelular.Value;
                mySqlCommandSel.Parameters["@DataCreditoCorreo"].Value = Datos.DataCreditoCorreo.Value;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDatosPersonales_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDatosPersonales
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccSolicitudDatosPersonales_Update(DTO_ccSolicitudDatosPersonales Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =                 
                                           "UPDATE ccSolicitudDatosPersonales SET  " +
                                                "NumeroDoc=@NumeroDoc,"+
                                                "Version=@Version," +
                                                "TipoPersona=@TipoPersona," +
                                                "TerceroID=@TerceroID," +
                                                "TerceroDocTipoID=@TerceroDocTipoID," +
                                                "FechaExpDoc=@FechaExpDoc," +
                                                "CiudadExpDoc=@CiudadExpDoc,"+
                                                "FechaNacimiento=@FechaNacimiento," +
                                                "Descriptivo=@Descriptivo," +
                                                "ApellidoPri=@ApellidoPri," +
                                                "ApellidoSdo=@ApellidoSdo," +
                                                "NombrePri=@NombrePri," +
                                                "NombreSdo=@NombreSdo," +
                                                "Celular1=@Celular1," +
                                                "Celular2=@Celular2," +
                                                "CorreoElectronico=@CorreoElectronico," +
                                                "DirResidencia=@DirResidencia," +
                                                "BarrioResidencia=@BarrioResidencia," +
                                                "CiudadResidencia=@CiudadResidencia," +
                                                "AntResidencia=@AntResidencia," +
                                                "TipoVivienda=@TipoVivienda," +
                                                "TelResidencia=@TelResidencia," +
                                                "LugarTrabajo=@LugarTrabajo," +
                                                "DirTrabajo=@DirTrabajo," +
                                                "BarrioTrabajo=@BarrioTrabajo," +
                                                "CiudadTrabajo=@CiudadTrabajo," +
                                                "TelTrabajo=@TelTrabajo," +
                                                "Cargo=@Cargo," +
                                                "AntTrabajo=@AntTrabajo," +
                                                "TipoContrato=@TipoContrato," +
                                                "EPS=@EPS," +
                                                "Personascargo=@Personascargo," +
                                                "Conyugue=@Conyugue," +
                                                "NombreConyugue=@NombreConyugue," +
                                                "ActConyugue=@ActConyugue," +
                                                "AntConyugue=@AntConyugue," +
                                                "EmpresaConyugue=@EmpresaConyugue," +
                                                "DirResConyugue=@DirResConyugue," +
                                                "TelefonoConyugue=@TelefonoConyugue," +
                                                "CelularConyugue=@CelularConyugue," +
                                                "NombreReferencia1=@NombreReferencia1," +
                                                "RelReferencia1=@RelReferencia1," +
                                                "TipoReferencia1=@TipoReferencia1," +
                                                "DirReferencia1=@DirReferencia1," +
                                                "BarrioReferencia1=@BarrioReferencia1," +
                                                "TelefonoReferencia1=@TelefonoReferencia1," +
                                                "CelularReferencia1=@CelularReferencia1," +
                                                "NombreReferencia2=@NombreReferencia2," +
                                                "RelReferencia2=@RelReferencia2," +
                                                "TipoReferencia2=@TipoReferencia2," +
                                                "DirReferencia2=@DirReferencia2," +
                                                "BarrioReferencia2=@BarrioReferencia2," +
                                                "TelefonoReferencia2=@TelefonoReferencia2," +
                                                "CelularReferencia2=@CelularReferencia2," +
                                                "VlrActivos=@VlrActivos," +
                                                "VlrPasivos=@VlrPasivos," +
                                                "VlrPatrimonio=@VlrPatrimonio," +
                                                "VlrEgresosMes=@VlrEgresosMes," +
                                                "VlrIngresosMes=@VlrIngresosMes," +
                                                "VlrIngresosNoOpe=@VlrIngresosNoOpe," +
                                                "DescrOtrosIng=@DescrOtrosIng," +
                                                "DescrOtrosBinenes=@DescrOtrosBinenes," +
                                                "EntCredito1=@EntCredito1," +
                                                "Plazo1=@Plazo1," +
                                                "Saldo1=@Saldo1," +
                                                "EntCredito2=@EntCredito2," +
                                                "Plazo2=@Plazo2," +
                                                "Saldo2=@Saldo2," +
                                                "SolicitudInd1=@SolicitudInd1," +
                                                "DeclFondos=@DeclFondos," +
                                                "BR_Direccion=@BR_Direccion," +
                                                "BR_Valor=@BR_Valor," +
                                                "BR_AfectacionFamInd=@BR_AfectacionFamInd," +
                                                "BR_HipotecaInd=@BR_HipotecaInd," +
                                                "BR_HipotecaNombre=@BR_HipotecaNombre," +
                                                "VE_Marca=@VE_Marca," +
                                                "VE_Clase=@VE_Clase," +
                                                "VE_Modelo=@VE_Modelo," +
                                                "VE_Placa=@VE_Placa," +
                                                "VE_PignoradoInd=@VE_PignoradoInd," +
                                                "VE_PignoradoNombre=@VE_PignoradoNombre," +
                                                "VE_Valor=@VE_Valor," +
                                                "UsuarioDigita=@UsuarioDigita," +
                                                "FechaDigita=@FechaDigita," +
                                                "UsuarioVerifica=@UsuarioVerifica," +
                                                "FechaVerifica=@FechaVerifica," +
                                                "UsuarioConfirma=@UsuarioConfirma," +
                                                "FechaConfirma=@FechaConfirma," +
                                                "EstadoCivil=@EstadoCivil," +
                                                "Sexo=@Sexo ," +
                                                "DataCreditoDireccion=@DataCreditoDireccion," +
                                                "DataCreditoTelefono=@DataCreditoTelefono," +
                                                "DataCreditoCelular=@DataCreditoCelular," +
                                                "DataCreditoCorreo=@DataCreditoCorreo"+
                                               " WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos                
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);          
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoPersona", SqlDbType.TinyInt);                
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char,UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroDocTipoID", SqlDbType.Char,UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaExpDoc", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CiudadExpDoc", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaNacimiento", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Descriptivo",SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ApellidoPri", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ApellidoSdo", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@NombrePri", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@NombreSdo", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@Celular1", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@Celular2", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@CorreoElectronico", SqlDbType.Char,60);
                mySqlCommandSel.Parameters.Add("@DirResidencia", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BarrioResidencia", SqlDbType.Char,60);
                mySqlCommandSel.Parameters.Add("@CiudadResidencia", SqlDbType.Char,UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AntResidencia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoVivienda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TelResidencia", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@LugarTrabajo", SqlDbType.Char,60);
                mySqlCommandSel.Parameters.Add("@DirTrabajo", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BarrioTrabajo", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@CiudadTrabajo", SqlDbType.Char,UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TelTrabajo", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@Cargo", SqlDbType.Char,40);
                mySqlCommandSel.Parameters.Add("@AntTrabajo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoContrato", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EPS", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@Personascargo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Conyugue", SqlDbType.Char,15);
                mySqlCommandSel.Parameters.Add("@NombreConyugue", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@ActConyugue", SqlDbType.Char,40);
                mySqlCommandSel.Parameters.Add("@AntConyugue", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EmpresaConyugue", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@DirResConyugue", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@TelefonoConyugue", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CelularConyugue", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@NombreReferencia1", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@RelReferencia1", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@TipoReferencia1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DirReferencia1", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BarrioReferencia1", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@TelefonoReferencia1", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CelularReferencia1", SqlDbType.Char,20);                   
                mySqlCommandSel.Parameters.Add("@NombreReferencia2", SqlDbType.Char,100);
                mySqlCommandSel.Parameters.Add("@RelReferencia2", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@TipoReferencia2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DirReferencia2", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BarrioReferencia2", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@TelefonoReferencia2", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@CelularReferencia2", SqlDbType.Char,20);
                
                mySqlCommandSel.Parameters.Add("@VlrActivos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPasivos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPatrimonio", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrEgresosMes", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrIngresosMes", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrIngresosNoOpe", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DescrOtrosIng", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@DescrOtrosBinenes", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@EntCredito1", SqlDbType.VarChar,50);
                mySqlCommandSel.Parameters.Add("@Plazo1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Saldo1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EntCredito2", SqlDbType.VarChar,50);
                mySqlCommandSel.Parameters.Add("@Plazo2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Saldo2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SolicitudInd1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DeclFondos", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@BR_Direccion", SqlDbType.VarChar,60);
                mySqlCommandSel.Parameters.Add("@BR_Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@BR_AfectacionFamInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@BR_HipotecaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@BR_HipotecaNombre", SqlDbType.VarChar,60);
                mySqlCommandSel.Parameters.Add("@VE_Marca", SqlDbType.VarChar,30);
                mySqlCommandSel.Parameters.Add("@VE_Clase", SqlDbType.VarChar,30);
                mySqlCommandSel.Parameters.Add("@VE_Modelo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VE_Placa", SqlDbType.Char,6);
                mySqlCommandSel.Parameters.Add("@VE_PignoradoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VE_PignoradoNombre", SqlDbType.VarChar,60);
                mySqlCommandSel.Parameters.Add("@VE_Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@UsuarioDigita", SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaDigita", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioVerifica",SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaVerifica", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioConfirma", SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaConfirma", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EstadoCivil", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Sexo", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters.Add("@DataCreditoDireccion", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DataCreditoTelefono", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DataCreditoCelular", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DataCreditoCorreo", SqlDbType.TinyInt);

            
                #endregion
                #region Asigna los valores                
                mySqlCommandSel.Parameters["@NumeroDoc" ].Value= Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version" ].Value= Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoPersona" ].Value= Datos.TipoPersona.Value;                
                mySqlCommandSel.Parameters["@TerceroID" ].Value= Datos.TerceroID.Value;
                mySqlCommandSel.Parameters["@TerceroDocTipoID" ].Value= Datos.TerceroDocTipoID.Value;
                mySqlCommandSel.Parameters["@FechaExpDoc" ].Value= Datos.FechaExpDoc.Value;
                mySqlCommandSel.Parameters["@CiudadExpDoc"].Value = Datos.CiudadExpDoc.Value;                               
                mySqlCommandSel.Parameters["@FechaNacimiento" ].Value= Datos.FechaNacimiento.Value;
                mySqlCommandSel.Parameters["@Descriptivo" ].Value= Datos.Descriptivo.Value;
                mySqlCommandSel.Parameters["@ApellidoPri" ].Value= Datos.ApellidoPri.Value;
                mySqlCommandSel.Parameters["@ApellidoSdo" ].Value= Datos.ApellidoSdo.Value;
                mySqlCommandSel.Parameters["@NombrePri" ].Value= Datos.NombrePri.Value;
                mySqlCommandSel.Parameters["@NombreSdo" ].Value= Datos.NombreSdo.Value;
                mySqlCommandSel.Parameters["@Celular1" ].Value= Datos.Celular1.Value;
                mySqlCommandSel.Parameters["@Celular2" ].Value= Datos.Celular2.Value;
                mySqlCommandSel.Parameters["@CorreoElectronico" ].Value= Datos.CorreoElectronico.Value;
                mySqlCommandSel.Parameters["@DirResidencia" ].Value= Datos.DirResidencia.Value;
                mySqlCommandSel.Parameters["@BarrioResidencia" ].Value= Datos.BarrioResidencia.Value;
                mySqlCommandSel.Parameters["@CiudadResidencia" ].Value= Datos.CiudadResidencia.Value;
                mySqlCommandSel.Parameters["@AntResidencia" ].Value= Datos.AntResidencia.Value;
                mySqlCommandSel.Parameters["@TipoVivienda" ].Value= Datos.TipoVivienda.Value;
                mySqlCommandSel.Parameters["@TelResidencia" ].Value= Datos.TelResidencia.Value;
                mySqlCommandSel.Parameters["@LugarTrabajo" ].Value= Datos.LugarTrabajo.Value;
                mySqlCommandSel.Parameters["@DirTrabajo" ].Value= Datos.DirTrabajo.Value;
                mySqlCommandSel.Parameters["@BarrioTrabajo" ].Value= Datos.BarrioTrabajo.Value;
                mySqlCommandSel.Parameters["@CiudadTrabajo" ].Value= Datos.CiudadTrabajo.Value;
                mySqlCommandSel.Parameters["@TelTrabajo" ].Value= Datos.TelTrabajo.Value;
                mySqlCommandSel.Parameters["@Cargo" ].Value= Datos.Cargo.Value;
                mySqlCommandSel.Parameters["@AntTrabajo" ].Value= Datos.AntTrabajo.Value;
                mySqlCommandSel.Parameters["@TipoContrato" ].Value= Datos.TipoContrato.Value;
                mySqlCommandSel.Parameters["@EPS" ].Value= Datos.EPS.Value;
                mySqlCommandSel.Parameters["@Personascargo" ].Value= Datos.Personascargo.Value;
                mySqlCommandSel.Parameters["@Conyugue" ].Value= Datos.Conyugue.Value;
                mySqlCommandSel.Parameters["@NombreConyugue" ].Value= Datos.NombreConyugue.Value;
                mySqlCommandSel.Parameters["@ActConyugue" ].Value= Datos.ActConyugue.Value;
                mySqlCommandSel.Parameters["@AntConyugue" ].Value= Datos.AntConyugue.Value;
                mySqlCommandSel.Parameters["@EmpresaConyugue"].Value = Datos.EmpresaConyugue.Value;
                mySqlCommandSel.Parameters["@DirResConyugue" ].Value= Datos.DirResConyugue.Value;
                mySqlCommandSel.Parameters["@TelefonoConyugue" ].Value= Datos.TelefonoConyugue.Value;
                mySqlCommandSel.Parameters["@CelularConyugue" ].Value= Datos.CelularConyugue.Value;
                mySqlCommandSel.Parameters["@NombreReferencia1" ].Value= Datos.NombreReferencia1.Value;
                mySqlCommandSel.Parameters["@RelReferencia1" ].Value= Datos.RelReferencia1.Value;
                mySqlCommandSel.Parameters["@TipoReferencia1" ].Value= Datos.TipoReferencia1.Value;
                mySqlCommandSel.Parameters["@DirReferencia1" ].Value= Datos.DirReferencia1.Value;
                mySqlCommandSel.Parameters["@BarrioReferencia1" ].Value= Datos.BarrioReferencia1.Value;
                mySqlCommandSel.Parameters["@TelefonoReferencia1" ].Value= Datos.TelefonoReferencia1.Value;
                mySqlCommandSel.Parameters["@CelularReferencia1" ].Value= Datos.CelularReferencia1.Value;                
                mySqlCommandSel.Parameters["@NombreReferencia2"].Value= Datos.NombreReferencia2.Value;
                mySqlCommandSel.Parameters["@RelReferencia2"].Value= Datos.RelReferencia2.Value;
                mySqlCommandSel.Parameters["@TipoReferencia2"].Value= Datos.TipoReferencia2.Value;
                mySqlCommandSel.Parameters["@DirReferencia2"].Value= Datos.DirReferencia2.Value;
                mySqlCommandSel.Parameters["@BarrioReferencia2"].Value= Datos.BarrioReferencia2.Value;
                mySqlCommandSel.Parameters["@TelefonoReferencia2"].Value= Datos.TelefonoReferencia2.Value;
                mySqlCommandSel.Parameters["@CelularReferencia2"].Value= Datos.CelularReferencia2.Value;
                mySqlCommandSel.Parameters["@VlrActivos"].Value= Datos.VlrActivos.Value;
                mySqlCommandSel.Parameters["@VlrPasivos"].Value= Datos.VlrPasivos.Value;
                mySqlCommandSel.Parameters["@VlrPatrimonio"].Value= Datos.VlrPatrimonio.Value;
                mySqlCommandSel.Parameters["@VlrEgresosMes"].Value= Datos.VlrEgresosMes.Value;
                mySqlCommandSel.Parameters["@VlrIngresosMes"].Value= Datos.VlrIngresosMes.Value;
                mySqlCommandSel.Parameters["@VlrIngresosNoOpe"].Value= Datos.VlrIngresosNoOpe.Value;
                mySqlCommandSel.Parameters["@DescrOtrosIng"].Value= Datos.DescrOtrosIng.Value;
                mySqlCommandSel.Parameters["@DescrOtrosBinenes"].Value= Datos.DescrOtrosBinenes.Value;
                mySqlCommandSel.Parameters["@EntCredito1"].Value= Datos.EntCredito1.Value;
                mySqlCommandSel.Parameters["@Plazo1"].Value= Datos.Plazo1.Value;
                mySqlCommandSel.Parameters["@Saldo1"].Value= Datos.Saldo1.Value;
                mySqlCommandSel.Parameters["@EntCredito2"].Value= Datos.EntCredito2.Value;
                mySqlCommandSel.Parameters["@Plazo2"].Value= Datos.Plazo2.Value;
                mySqlCommandSel.Parameters["@Saldo2"].Value= Datos.Saldo2.Value;
                mySqlCommandSel.Parameters["@SolicitudInd1"].Value= Datos.SolicitudInd1.Value;
                mySqlCommandSel.Parameters["@DeclFondos"].Value= Datos.DeclFondos.Value;
                mySqlCommandSel.Parameters["@BR_Direccion"].Value= Datos.BR_Direccion.Value;
                mySqlCommandSel.Parameters["@BR_Valor"].Value= Datos.BR_Valor.Value;
                mySqlCommandSel.Parameters["@BR_AfectacionFamInd"].Value= Datos.BR_AfectacionFamInd.Value;
                mySqlCommandSel.Parameters["@BR_HipotecaInd"].Value= Datos.BR_HipotecaInd.Value;
                mySqlCommandSel.Parameters["@BR_HipotecaNombre"].Value= Datos.BR_HipotecaNombre.Value;
                mySqlCommandSel.Parameters["@VE_Marca"].Value= Datos.VE_Marca.Value;
                mySqlCommandSel.Parameters["@VE_Clase"].Value= Datos.VE_Clase.Value;
                mySqlCommandSel.Parameters["@VE_Modelo"].Value= Datos.VE_Modelo.Value;
                mySqlCommandSel.Parameters["@VE_Placa"].Value= Datos.VE_Placa.Value;
                mySqlCommandSel.Parameters["@VE_PignoradoInd"].Value= Datos.VE_PignoradoInd.Value;
                mySqlCommandSel.Parameters["@VE_PignoradoNombre"].Value= Datos.VE_PignoradoNombre.Value;
                mySqlCommandSel.Parameters["@VE_Valor"].Value= Datos.VE_Valor.Value;
                mySqlCommandSel.Parameters["@UsuarioDigita"].Value= Datos.UsuarioDigita.Value;
                mySqlCommandSel.Parameters["@FechaDigita"].Value= Datos.FechaDigita.Value;
                mySqlCommandSel.Parameters["@UsuarioVerifica"].Value= Datos.UsuarioVerifica.Value;
                mySqlCommandSel.Parameters["@FechaVerifica"].Value= Datos.FechaVerifica.Value;
                mySqlCommandSel.Parameters["@UsuarioConfirma"].Value= Datos.UsuarioConfirma.Value;
                mySqlCommandSel.Parameters["@FechaConfirma"].Value= Datos.FechaConfirma.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value= Datos.Consecutivo.Value;

                mySqlCommandSel.Parameters["@Sexo"].Value = Datos.Sexo.Value;
                mySqlCommandSel.Parameters["@EstadoCivil"].Value = Datos.EstadoCivil.Value;
                mySqlCommandSel.Parameters["@DataCreditoDireccion"].Value = Datos.DataCreditoDireccion.Value;
                mySqlCommandSel.Parameters["@DataCreditoTelefono"].Value = Datos.DataCreditoTelefono.Value;
                mySqlCommandSel.Parameters["@DataCreditoCelular"].Value = Datos.DataCreditoCelular.Value;
                mySqlCommandSel.Parameters["@DataCreditoCorreo"].Value = Datos.DataCreditoCorreo.Value;


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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDatosPersonales_Update");
                throw exception;
            }
        }

        public void DAL_ccSolicitudDatosPersonales_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudDatosPersonales WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDatosPersonales_Delete");
                throw exception;
            }
        }

        #endregion

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto</returns>
        public bool DAL_ccSolicitudDatosPersonales_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from ccSolicitudDatosPersonales with(nolock) where Consecutivo = @Consecutivo";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDatosPersonales_Exist");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDatosPersonales
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public List<DTO_ccSolicitudDatosPersonales> DAL_ccSolicitudDatosPersonales_GetByCedula(string cedula)
        {
            try
            {
                List<DTO_ccSolicitudDatosPersonales> result = new List<DTO_ccSolicitudDatosPersonales>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char);
                mySqlCommand.Parameters["@TerceroID"].Value = cedula;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.CommandText = "Select * from ccSolicitudDatosPersonales  with(nolock) " +
                                         " Where TerceroID =  @TerceroID ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDatosPersonales(dr));
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDatosPersonales_GetByCedula");
                throw exception;
            }
        }

        /// <summary>
        /// Trae datos por libranza
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public List<DTO_ccSolicitudDatosPersonales> DAL_ccSolicitudDatosPersonales_GetByLibranza(int libranza)
        {
            try
            {
                List<DTO_ccSolicitudDatosPersonales> result = new List<DTO_ccSolicitudDatosPersonales>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Char);
                mySqlCommand.Parameters["@Libranza"].Value = libranza;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommand.CommandText = " Select datos.* from ccSolicitudDatosPersonales datos  with(nolock) " +
                                            " inner join ccSolicitudDocu docu with(nolock) on datos.NumeroDoc = docu.NumeroDoc and datos.Version = docu.VersionNro " +
                                            " Where docu.Libranza =  @Libranza ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDatosPersonales(dr));
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDatosPersonales_GetByLibranza");
                throw exception;
            }
        }

    }
}
