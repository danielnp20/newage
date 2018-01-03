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
    public class DAL_drSolicitudDatosVehiculo : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_drSolicitudDatosVehiculo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_drSolicitudDatosVehiculo que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_drSolicitudDatosVehiculo</returns>
        public List<DTO_drSolicitudDatosVehiculo> DAL_drSolicitudDatosVehiculo_GetAll()
        {
            try
            {
                List<DTO_drSolicitudDatosVehiculo> result = new List<DTO_drSolicitudDatosVehiculo>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT * FROM drSolicitudDatosVehiculo with(nolock) ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_drSolicitudDatosVehiculo Datos;
                    Datos = new DTO_drSolicitudDatosVehiculo(dr);
                    result.Add(Datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosVehiculo_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_drSolicitudDatosVehiculo que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_drSolicitudDatosVehiculo</returns>
        public DTO_drSolicitudDatosVehiculo DAL_drSolicitudDatosVehiculo_GetByNumeroDoc(int numeroDoc, int version)
        {
            try
            {
                DTO_drSolicitudDatosVehiculo result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("Version", SqlDbType.Int);
                mySqlCommand.Parameters["NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["Version"].Value = version;

                mySqlCommand.CommandText = "SELECT * FROM drSolicitudDatosVehiculo with(nolock) where NumeroDoc = @NumeroDoc and Version = @Version ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_drSolicitudDatosVehiculo(dr);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosVehiculo_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla drSolicitudDatosVehiculo
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_drSolicitudDatosVehiculo_Add(DTO_drSolicitudDatosVehiculo Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO drSolicitudDatosVehiculo " +
                    "( " +
                     "  NumeroDoc,Version,GarantiaID,Placa,DocPrenda,OrigenDocumento,PrefijoPrenda,NumeroPrenda,TipoPrenda,Garante,Marca,"+
                     "  Linea,Referencia,Cilindraje,Tipocaja,Complemento,AireAcondicionado,PuertasNro,Termoking,FasecoldaCod,VlrFasecolda,"+
                     "  Carroceria,Peso,Servicio,CeroKmInd,Modelo,PrecioVenta,PrecioVentaChasis,PrecioVentaComplemento,CuotaInicial,Registrada,ChasisYComplementoIND,Motor,Serie," +
                     "  Chasis,Clase,Color,Tipo,NumeroFactura,TipoFactura,InmuebleTipo,Matricula,Direccion,Ano,FuenteHIP,CodigoGarantia,VlrFuente,"+
                     "  FechaFuente,CodigoGarantia1,VlrGarantia,IndValidado,IndValidaHipoteca,RUNT,Confecamaras,FechaVTO," +
                     "  Ciudad,FechaPredial,ValorPredial,FechaAvaluo,ValorAvaluo,FechaPromesa,ValorCompraventa,ViviendaNuevaInd," +
                     "  PrefijoHipoteca,NumeroHipoteca,FechaInicio,ValorPoliza,Escritura,FechaEscritura,Notaria,Marca_2,Linea_2,"+
                     "  Referencia_2,Cilindraje_2,Tipocaja_2,Complemento_2,AireAcondicionado_2,PuertasNro_2,Termoking_2,FasecoldaCod_2,"+
                     "  VlrFasecolda_2,Carroceria_2,Peso_2,Servicio_2,CeroKmInd_2,Modelo_2,PrecioVenta_2,Registrada_2,ChasisYComplementoIND_2,"+
                     "  IndValidado_2,PrefijoPrenda_2,NumeroPrenda_2,Garante_2,Placa_2,Motor_2,Serie_2,Chasis_2,Clase_2,Color_2,Tipo_2,DocPrenda_2,"+
                     "  NumeroFactura_2,InmuebleTipo_2,Direccion_2,Ciudad_2,Matricula_2,FechaAvaluo_2,ValorAvaluo_2,ViviendaNuevaInd_2,FechaPredial_2,"+
                     "  ValorPredial_2,FechaPromesa_2,ValorCompraventa_2,IndValidaHipoteca_2,PrefijoHipoteca_2,NumeroHipoteca_2,RUNT_2,Confecamaras_2,"+
                     "  FechaRegistro_2,FechaInicio_2,FechaVto_2,ValorPoliza_2,Escritura_2,FechaEscritura_2,Notaria_2,"+
                     "  PagareCRD,PagarePOL,PolizaVEH1,PolizaVEH2,PolizaHIP1,PolizaHIP2,FechaIniVEH1,FechaFinVEH1,FechaIniVEH2,"+
                     "  FechaFinVEH2,FechaIniHIP1,FechaFinHIP1,FechaIniHIP2,FechaFinHIP2,Aseguradora1VEH,Aseguradora2VEH,Aseguradora1HIP,Aseguradora2HIP,"+
                     "  VlrPolizaVEH1,CancelaContadoPolizaIndVEH1,IntermediarioExternoIndVEH1,VlrPolizaVEH2,CancelaContadoPolizaIndVEH2,"+
                     "  IntermediarioExternoIndVEH2,VlrPolizaHIP1,VlrPolizaHIP2,"+
                     "  eg_ccFasecolda,eg_glGarantia" +
                    ") " +
                    "VALUES " +
                    "( " +
                    "  @NumeroDoc,@Version,@GarantiaID,@Placa,@DocPrenda,@OrigenDocumento,@PrefijoPrenda,@NumeroPrenda,@TipoPrenda,"+
                    "  @Garante,@Marca,@Linea,@Referencia,@Cilindraje,@Tipocaja,@Complemento,@AireAcondicionado,@PuertasNro,@Termoking,"+
                    "  @FasecoldaCod,@VlrFasecolda,@Carroceria,@Peso,@Servicio,@CeroKmInd,@Modelo,@PrecioVenta,@PrecioVentaChasis,@PrecioVentaComplemento,@CuotaInicial,@Registrada," +
                    "  @ChasisYComplementoIND,@Motor,@Serie,@Chasis,@Clase,@Color,@Tipo,@NumeroFactura,@TipoFactura,@InmuebleTipo,@Matricula,@Direccion," +
                    "  @Ano,@FuenteHIP,@CodigoGarantia,@VlrFuente,@FechaFuente,@CodigoGarantia1,@VlrGarantia,@IndValidado,@IndValidaHipoteca,@RUNT,@Confecamaras,@FechaVTO," +
                    "  @Ciudad,@FechaPredial,@ValorPredial,@FechaAvaluo,@ValorAvaluo,@FechaPromesa,@ValorCompraventa,@ViviendaNuevaInd," +
                    "  @PrefijoHipoteca,@NumeroHipoteca,@FechaInicio,@ValorPoliza,@Escritura,@FechaEscritura,@Notaria,@Marca_2,@Linea_2," +
                    "  @Referencia_2,@Cilindraje_2,@Tipocaja_2,@Complemento_2,@AireAcondicionado_2,@PuertasNro_2,@Termoking_2,@FasecoldaCod_2," +
                    "  @VlrFasecolda_2,@Carroceria_2,@Peso_2,@Servicio_2,@CeroKmInd_2,@Modelo_2,@PrecioVenta_2,@Registrada_2,@ChasisYComplementoIND_2," +
                    "  @IndValidado_2,@PrefijoPrenda_2,@NumeroPrenda_2,@Garante_2,@Placa_2,@Motor_2,@Serie_2,@Chasis_2,@Clase_2,@Color_2,@Tipo_2,@DocPrenda_2," +
                    "  @NumeroFactura_2,@InmuebleTipo_2,@Direccion_2,@Ciudad_2,@Matricula_2,@FechaAvaluo_2,@ValorAvaluo_2,@ViviendaNuevaInd_2,@FechaPredial_2," +
                    "  @ValorPredial_2,@FechaPromesa_2,@ValorCompraventa_2,@IndValidaHipoteca_2,@PrefijoHipoteca_2,@NumeroHipoteca_2,@RUNT_2,@Confecamaras_2," +
                    "  @FechaRegistro_2,@FechaInicio_2,@FechaVto_2,@ValorPoliza_2,@Escritura_2,@FechaEscritura_2,@Notaria_2," +
                    "  @PagareCRD,@PagarePOL,@PolizaVEH1,@PolizaVEH2,@PolizaHIP1,@PolizaHIP2,@FechaIniVEH1,@FechaFinVEH1,@FechaIniVEH2," +
                    "  @FechaFinVEH2,@FechaIniHIP1,@FechaFinHIP1,@FechaIniHIP2,@FechaFinHIP2,@Aseguradora1VEH,@Aseguradora2VEH,@Aseguradora1HIP,@Aseguradora2HIP," +
                    "  @VlrPolizaVEH1,@CancelaContadoPolizaIndVEH1,@IntermediarioExternoIndVEH1,@VlrPolizaVEH2,@CancelaContadoPolizaIndVEH2," +
                    "  @IntermediarioExternoIndVEH2,@VlrPolizaHIP1,@VlrPolizaHIP2," +
                    "  @eg_ccFasecolda,@eg_glGarantia" +
                    ") SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de comandos         

                    mySqlCommandSel.Parameters.Add("@NumeroDoc",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Version",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@GarantiaID",SqlDbType.VarChar,UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Placa",SqlDbType.Char,6);
                    mySqlCommandSel.Parameters.Add("@DocPrenda", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@OrigenDocumento",SqlDbType.Char,50);
                    mySqlCommandSel.Parameters.Add("@PrefijoPrenda",SqlDbType.VarChar,UDT_PrefijoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@NumeroPrenda",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@TipoPrenda", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Garante", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Marca",SqlDbType.VarChar,100);
                    mySqlCommandSel.Parameters.Add("@Linea",SqlDbType.VarChar,100);
                    mySqlCommandSel.Parameters.Add("@Referencia",SqlDbType.VarChar,100);
                    mySqlCommandSel.Parameters.Add("@Cilindraje",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Tipocaja",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Complemento",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@AireAcondicionado",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@PuertasNro",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Termoking",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@FasecoldaCod",SqlDbType.Char,UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters.Add("@VlrFasecolda",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@Carroceria",SqlDbType.VarChar,30);
                    mySqlCommandSel.Parameters.Add("@Peso",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Servicio",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@CeroKmInd",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@Modelo",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@PrecioVenta",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@PrecioVentaChasis", SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@PrecioVentaComplemento", SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@CuotaInicial",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@Registrada",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@ChasisYComplementoIND",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@Motor",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Serie",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Chasis",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Clase", SqlDbType.Char, 30);
                    mySqlCommandSel.Parameters.Add("@Color",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Tipo",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@NumeroFactura",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@TipoFactura",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@InmuebleTipo",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Matricula",SqlDbType.VarChar,UDT_CodigoGrl20.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Direccion",SqlDbType.VarChar,UDT_DescripTBase.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Ano",SqlDbType.SmallInt);
                    mySqlCommandSel.Parameters.Add("@FuenteHIP",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@CodigoGarantia",SqlDbType.VarChar,UDT_CodigoGrl20.MaxLength);
                    mySqlCommandSel.Parameters.Add("@VlrFuente",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@FechaFuente",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@CodigoGarantia1",SqlDbType.VarChar,UDT_CodigoGrl20.MaxLength);
                    mySqlCommandSel.Parameters.Add("@VlrGarantia",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@IndValidado", SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@IndValidaHipoteca", SqlDbType.Bit);                
                    mySqlCommandSel.Parameters.Add("@eg_ccFasecolda",SqlDbType.Char,UDT_EmpresaGrupoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@eg_glGarantia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@RUNT", SqlDbType.Char, 50);
                    mySqlCommandSel.Parameters.Add("@Confecamaras", SqlDbType.Char, 50);
                    mySqlCommandSel.Parameters.Add("@FechaVTO", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, 50);
                    mySqlCommandSel.Parameters.Add("@FechaPredial", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@ValorPredial", SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@FechaAvaluo", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@ValorAvaluo", SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@FechaPromesa", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@ValorCompraventa", SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@ViviendaNuevaInd", SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                    //
                    #region campos nuevos
                    mySqlCommandSel.Parameters.Add("@PrefijoHipoteca",SqlDbType.Char,UDT_PrefijoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@NumeroHipoteca",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@FechaInicio",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@ValorPoliza",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@Escritura",SqlDbType.VarChar,10);
                    mySqlCommandSel.Parameters.Add("@FechaEscritura",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@Notaria",SqlDbType.VarChar,10);
                    mySqlCommandSel.Parameters.Add("@Marca_2",SqlDbType.VarChar,100);
                    mySqlCommandSel.Parameters.Add("@Linea_2",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Referencia_2",SqlDbType.VarChar,100);
                    mySqlCommandSel.Parameters.Add("@Cilindraje_2",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Tipocaja_2",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Complemento_2",SqlDbType.Char,100);
                    mySqlCommandSel.Parameters.Add("@AireAcondicionado_2",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@PuertasNro_2",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Termoking_2",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@FasecoldaCod_2",SqlDbType.Char,UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters.Add("@VlrFasecolda_2",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@Carroceria_2",SqlDbType.VarChar,30);
                    mySqlCommandSel.Parameters.Add("@Peso_2",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Servicio_2",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@CeroKmInd_2",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@Modelo_2",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@PrecioVenta_2",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@Registrada_2",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@ChasisYComplementoIND_2",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@IndValidado_2",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@PrefijoPrenda_2",SqlDbType.VarChar,UDT_PrefijoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@NumeroPrenda_2",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Garante_2",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Placa_2",SqlDbType.Char,6);
                    mySqlCommandSel.Parameters.Add("@Motor_2",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Serie_2",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Chasis_2",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Clase_2",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Color_2",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@Tipo_2",SqlDbType.Char,30);
                    mySqlCommandSel.Parameters.Add("@DocPrenda_2",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@NumeroFactura_2",SqlDbType.VarChar,30);
                    mySqlCommandSel.Parameters.Add("@InmuebleTipo_2",SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Direccion_2",SqlDbType.VarChar,UDT_DescripTBase.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Ciudad_2",SqlDbType.VarChar,50);
                    mySqlCommandSel.Parameters.Add("@Matricula_2",SqlDbType.VarChar,UDT_CodigoGrl20.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaAvaluo_2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@ValorAvaluo_2",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@ViviendaNuevaInd_2",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@FechaPredial_2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@ValorPredial_2",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@FechaPromesa_2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@ValorCompraventa_2",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@IndValidaHipoteca_2",SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@PrefijoHipoteca_2",SqlDbType.VarChar,UDT_PrefijoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@NumeroHipoteca_2",SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@RUNT_2",SqlDbType.Char,50);
                    mySqlCommandSel.Parameters.Add("@Confecamaras_2",SqlDbType.Char,50);
                    mySqlCommandSel.Parameters.Add("@FechaRegistro_2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaInicio_2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaVto_2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@ValorPoliza_2",SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@Escritura_2",SqlDbType.VarChar,10);
                    mySqlCommandSel.Parameters.Add("@FechaEscritura_2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@Notaria_2",SqlDbType.VarChar,10);

                    mySqlCommandSel.Parameters.Add("@PagareCRD",SqlDbType.Char,15);
                    mySqlCommandSel.Parameters.Add("@PagarePOL",SqlDbType.Char,15);
                    mySqlCommandSel.Parameters.Add("@PolizaVEH1",SqlDbType.Char,20);
                    mySqlCommandSel.Parameters.Add("@PolizaVEH2",SqlDbType.Char,20);
                    mySqlCommandSel.Parameters.Add("@PolizaHIP1",SqlDbType.Char,20);
                    mySqlCommandSel.Parameters.Add("@PolizaHIP2",SqlDbType.Char,20);
                    mySqlCommandSel.Parameters.Add("@FechaIniVEH1",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFinVEH1",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaIniVEH2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFinVEH2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaIniHIP1",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFinHIP1",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaIniHIP2",SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFinHIP2",SqlDbType.SmallDateTime);

                    mySqlCommandSel.Parameters.Add("@Aseguradora1VEH", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Aseguradora2VEH", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Aseguradora1HIP", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Aseguradora2HIP", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);

                    mySqlCommandSel.Parameters.Add("@VlrPolizaVEH1", SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@CancelaContadoPolizaIndVEH1", SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@IntermediarioExternoIndVEH1", SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@VlrPolizaVEH2", SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@CancelaContadoPolizaIndVEH2", SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@IntermediarioExternoIndVEH2", SqlDbType.Bit);
                    mySqlCommandSel.Parameters.Add("@VlrPolizaHIP1", SqlDbType.Decimal);
                    mySqlCommandSel.Parameters.Add("@VlrPolizaHIP2", SqlDbType.Decimal);

                    #endregion
                #endregion
                #region Asigna los valores

                    mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                    mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                    mySqlCommandSel.Parameters["@GarantiaID"].Value = Datos.GarantiaID.Value;
                    mySqlCommandSel.Parameters["@Placa"].Value = Datos.Placa.Value;
                    mySqlCommandSel.Parameters["@DocPrenda"].Value = Datos.DocPrenda.Value;
                    mySqlCommandSel.Parameters["@OrigenDocumento"].Value = Datos.OrigenDocumento.Value;
                    mySqlCommandSel.Parameters["@PrefijoPrenda"].Value = Datos.PrefijoPrenda.Value;
                    mySqlCommandSel.Parameters["@NumeroPrenda"].Value = Datos.NumeroPrenda.Value;
                    mySqlCommandSel.Parameters["@TipoPrenda"].Value = Datos.TipoPrenda.Value;
                    mySqlCommandSel.Parameters["@Garante"].Value = Datos.Garante.Value;
                    mySqlCommandSel.Parameters["@Marca"].Value = Datos.Marca.Value;
                    mySqlCommandSel.Parameters["@Linea"].Value = Datos.Linea.Value;
                    mySqlCommandSel.Parameters["@Referencia"].Value = Datos.Referencia.Value;
                    mySqlCommandSel.Parameters["@Cilindraje"].Value = Datos.Cilindraje.Value;
                    mySqlCommandSel.Parameters["@Tipocaja"].Value = Datos.Tipocaja.Value;
                    mySqlCommandSel.Parameters["@Complemento"].Value = Datos.Complemento.Value;
                    mySqlCommandSel.Parameters["@AireAcondicionado"].Value = Datos.AireAcondicionado.Value;
                    mySqlCommandSel.Parameters["@PuertasNro"].Value = Datos.PuertasNro.Value;
                    mySqlCommandSel.Parameters["@Termoking"].Value = Datos.Termoking.Value;
                    mySqlCommandSel.Parameters["@FasecoldaCod"].Value = Datos.FasecoldaCod.Value;
                    mySqlCommandSel.Parameters["@VlrFasecolda"].Value = Datos.VlrFasecolda.Value;
                    mySqlCommandSel.Parameters["@Carroceria"].Value = Datos.Carroceria.Value;
                    mySqlCommandSel.Parameters["@Peso"].Value = Datos.Peso.Value;
                    mySqlCommandSel.Parameters["@Servicio"].Value = Datos.Servicio.Value;
                    mySqlCommandSel.Parameters["@CeroKmInd"].Value = Datos.CeroKmInd.Value;
                    mySqlCommandSel.Parameters["@Modelo"].Value = Datos.Modelo.Value;
                    mySqlCommandSel.Parameters["@PrecioVenta"].Value = Datos.PrecioVenta.Value;
                    mySqlCommandSel.Parameters["@PrecioVentaChasis"].Value = Datos.PrecioVentaChasis.Value;
                    mySqlCommandSel.Parameters["@PrecioVentaComplemento"].Value = Datos.PrecioVentaComplemento.Value;
                    mySqlCommandSel.Parameters["@CuotaInicial"].Value = Datos.CuotaInicial.Value;
                    mySqlCommandSel.Parameters["@Registrada"].Value = Datos.Registrada.Value;
                    mySqlCommandSel.Parameters["@ChasisYComplementoIND"].Value = Datos.ChasisYComplementoIND.Value;
                    mySqlCommandSel.Parameters["@Motor"].Value = Datos.Motor.Value;
                    mySqlCommandSel.Parameters["@Serie"].Value = Datos.Serie.Value;
                    mySqlCommandSel.Parameters["@Chasis"].Value = Datos.Chasis.Value;
                    mySqlCommandSel.Parameters["@Clase"].Value = Datos.Clase.Value;
                    mySqlCommandSel.Parameters["@Color"].Value = Datos.Color.Value;
                    mySqlCommandSel.Parameters["@Tipo"].Value = Datos.Tipo.Value;
                    mySqlCommandSel.Parameters["@NumeroFactura"].Value = Datos.NumeroFactura.Value;
                    mySqlCommandSel.Parameters["@TipoFactura"].Value = Datos.TipoFactura.Value;
                    mySqlCommandSel.Parameters["@InmuebleTipo"].Value = Datos.InmuebleTipo.Value;
                    mySqlCommandSel.Parameters["@Matricula"].Value = Datos.Matricula.Value;
                    mySqlCommandSel.Parameters["@Direccion"].Value = Datos.Direccion.Value;
                    mySqlCommandSel.Parameters["@Ano"].Value = Datos.Ano.Value;
                    mySqlCommandSel.Parameters["@FuenteHIP"].Value = Datos.FuenteHIP.Value;
                    mySqlCommandSel.Parameters["@CodigoGarantia"].Value = Datos.CodigoGarantia.Value;
                    mySqlCommandSel.Parameters["@VlrFuente"].Value = Datos.VlrFuente.Value;
                    mySqlCommandSel.Parameters["@FechaFuente"].Value = Datos.FechaFuente.Value;
                    mySqlCommandSel.Parameters["@CodigoGarantia1"].Value = Datos.CodigoGarantia1.Value;
                    mySqlCommandSel.Parameters["@VlrGarantia"].Value = Datos.VlrGarantia.Value;
                    mySqlCommandSel.Parameters["@IndValidado"].Value = Datos.IndValidado.Value;
                    mySqlCommandSel.Parameters["@IndValidaHipoteca"].Value = Datos.IndValidaHipoteca.Value;
                
                    mySqlCommandSel.Parameters["@RUNT"].Value = Datos.RUNT.Value;
                    mySqlCommandSel.Parameters["@Confecamaras"].Value = Datos.Confecamaras.Value;
                    mySqlCommandSel.Parameters["@FechaVTO"].Value = Datos.FechaVTO.Value;
                    mySqlCommandSel.Parameters["@Ciudad"].Value = Datos.Ciudad.Value;
                    mySqlCommandSel.Parameters["@FechaPredial"].Value = Datos.FechaPredial.Value;
                    mySqlCommandSel.Parameters["@ValorPredial"].Value = Datos.ValorPredial.Value;
                    mySqlCommandSel.Parameters["@FechaAvaluo"].Value = Datos.FechaAvaluo.Value;
                    mySqlCommandSel.Parameters["@ValorAvaluo"].Value = Datos.ValorAvaluo.Value;
                    mySqlCommandSel.Parameters["@FechaPromesa"].Value = Datos.FechaPromesa.Value;
                    mySqlCommandSel.Parameters["@ValorCompraventa"].Value = Datos.ValorCompraventa.Value;
                    mySqlCommandSel.Parameters["@ViviendaNuevaInd"].Value = Datos.ViviendaNuevaInd.Value;
                    #region campos nuevos
                    mySqlCommandSel.Parameters["@PrefijoHipoteca"].Value = Datos.PrefijoHipoteca.Value;
                    mySqlCommandSel.Parameters["@NumeroHipoteca"].Value = Datos.NumeroHipoteca.Value;
                    mySqlCommandSel.Parameters["@FechaInicio"].Value = Datos.FechaInicio.Value;
                    mySqlCommandSel.Parameters["@ValorPoliza"].Value = Datos.ValorPoliza.Value;
                    mySqlCommandSel.Parameters["@Escritura"].Value = Datos.Escritura.Value;
                    mySqlCommandSel.Parameters["@FechaEscritura"].Value = Datos.FechaEscritura.Value;
                    mySqlCommandSel.Parameters["@Notaria"].Value = Datos.Notaria.Value;
                    mySqlCommandSel.Parameters["@Marca_2"].Value = Datos.Marca_2.Value;
                    mySqlCommandSel.Parameters["@Linea_2"].Value = Datos.Linea_2.Value;
                    mySqlCommandSel.Parameters["@Referencia_2"].Value = Datos.Referencia_2.Value;
                    mySqlCommandSel.Parameters["@Cilindraje_2"].Value = Datos.Cilindraje_2.Value;
                    mySqlCommandSel.Parameters["@Tipocaja_2"].Value = Datos.Tipocaja_2.Value;
                    mySqlCommandSel.Parameters["@Complemento_2"].Value = Datos.Complemento_2.Value;
                    mySqlCommandSel.Parameters["@AireAcondicionado_2"].Value = Datos.AireAcondicionado_2.Value;
                    mySqlCommandSel.Parameters["@PuertasNro_2"].Value = Datos.PuertasNro_2.Value;
                    mySqlCommandSel.Parameters["@Termoking_2"].Value = Datos.Termoking_2.Value;
                    mySqlCommandSel.Parameters["@FasecoldaCod_2"].Value = Datos.FasecoldaCod_2.Value;
                    mySqlCommandSel.Parameters["@VlrFasecolda_2"].Value = Datos.VlrFasecolda_2.Value;
                    mySqlCommandSel.Parameters["@Carroceria_2"].Value = Datos.Carroceria_2.Value;
                    mySqlCommandSel.Parameters["@Peso_2"].Value = Datos.Peso_2.Value;
                    mySqlCommandSel.Parameters["@Servicio_2"].Value = Datos.Servicio_2.Value;
                    mySqlCommandSel.Parameters["@CeroKmInd_2"].Value = Datos.CeroKmInd_2.Value;
                    mySqlCommandSel.Parameters["@Modelo_2"].Value = Datos.Modelo_2.Value;
                    mySqlCommandSel.Parameters["@PrecioVenta_2"].Value = Datos.PrecioVenta_2.Value;
                    mySqlCommandSel.Parameters["@Registrada_2"].Value = Datos.Registrada_2.Value;
                    mySqlCommandSel.Parameters["@ChasisYComplementoIND_2"].Value = Datos.ChasisYComplementoIND_2.Value;
                    mySqlCommandSel.Parameters["@IndValidado_2"].Value = Datos.IndValidado_2.Value;
                    mySqlCommandSel.Parameters["@PrefijoPrenda_2"].Value = Datos.PrefijoPrenda_2.Value;
                    mySqlCommandSel.Parameters["@NumeroPrenda_2"].Value = Datos.NumeroPrenda_2.Value;
                    mySqlCommandSel.Parameters["@Garante_2"].Value = Datos.Garante_2.Value;
                    mySqlCommandSel.Parameters["@Placa_2"].Value = Datos.Placa_2.Value;
                    mySqlCommandSel.Parameters["@Motor_2"].Value = Datos.Motor_2.Value;
                    mySqlCommandSel.Parameters["@Serie_2"].Value = Datos.Serie_2.Value;
                    mySqlCommandSel.Parameters["@Chasis_2"].Value = Datos.Chasis_2.Value;
                    mySqlCommandSel.Parameters["@Clase_2"].Value = Datos.Clase_2.Value;
                    mySqlCommandSel.Parameters["@Color_2"].Value = Datos.Color_2.Value;
                    mySqlCommandSel.Parameters["@Tipo_2"].Value = Datos.Tipo_2.Value;
                    mySqlCommandSel.Parameters["@DocPrenda_2"].Value = Datos.DocPrenda_2.Value;
                    mySqlCommandSel.Parameters["@NumeroFactura_2"].Value = Datos.NumeroFactura_2.Value;
                    mySqlCommandSel.Parameters["@InmuebleTipo_2"].Value = Datos.InmuebleTipo_2.Value;
                    mySqlCommandSel.Parameters["@Direccion_2"].Value = Datos.Direccion_2.Value;
                    mySqlCommandSel.Parameters["@Ciudad_2"].Value = Datos.Ciudad_2.Value;
                    mySqlCommandSel.Parameters["@Matricula_2"].Value = Datos.Matricula_2.Value;
                    mySqlCommandSel.Parameters["@FechaAvaluo_2"].Value = Datos.FechaAvaluo_2.Value;
                    mySqlCommandSel.Parameters["@ValorAvaluo_2"].Value = Datos.ValorAvaluo_2.Value;
                    mySqlCommandSel.Parameters["@ViviendaNuevaInd_2"].Value = Datos.ViviendaNuevaInd_2.Value;
                    mySqlCommandSel.Parameters["@FechaPredial_2"].Value = Datos.FechaPredial_2.Value;
                    mySqlCommandSel.Parameters["@ValorPredial_2"].Value = Datos.ValorPredial_2.Value;
                    mySqlCommandSel.Parameters["@FechaPromesa_2"].Value = Datos.FechaPromesa_2.Value;
                    mySqlCommandSel.Parameters["@ValorCompraventa_2"].Value = Datos.ValorCompraventa_2.Value;
                    mySqlCommandSel.Parameters["@IndValidaHipoteca_2"].Value = Datos.IndValidaHipoteca_2.Value;
                    mySqlCommandSel.Parameters["@PrefijoHipoteca_2"].Value = Datos.PrefijoHipoteca_2.Value;
                    mySqlCommandSel.Parameters["@NumeroHipoteca_2"].Value = Datos.NumeroHipoteca_2.Value;
                    mySqlCommandSel.Parameters["@RUNT_2"].Value = Datos.RUNT_2.Value;
                    mySqlCommandSel.Parameters["@Confecamaras_2"].Value = Datos.Confecamaras_2.Value;
                    mySqlCommandSel.Parameters["@FechaRegistro_2"].Value = Datos.FechaRegistro_2.Value;
                    mySqlCommandSel.Parameters["@FechaInicio_2"].Value = Datos.FechaInicio_2.Value;
                    mySqlCommandSel.Parameters["@FechaVto_2"].Value = Datos.FechaVto_2.Value;
                    mySqlCommandSel.Parameters["@ValorPoliza_2"].Value = Datos.ValorPoliza_2.Value;
                    mySqlCommandSel.Parameters["@Escritura_2"].Value = Datos.Escritura_2.Value;
                    mySqlCommandSel.Parameters["@FechaEscritura_2"].Value = Datos.FechaEscritura_2.Value;
                    mySqlCommandSel.Parameters["@Notaria_2"].Value = Datos.Notaria_2.Value;

                    mySqlCommandSel.Parameters["@PagareCRD"].Value = Datos.PagareCRD.Value;
                    mySqlCommandSel.Parameters["@PagarePOL"].Value = Datos.PagarePOL.Value;
                    mySqlCommandSel.Parameters["@PolizaVEH1"].Value = Datos.PolizaVEH1.Value;
                    mySqlCommandSel.Parameters["@PolizaVEH2"].Value = Datos.PolizaVEH2.Value;
                    mySqlCommandSel.Parameters["@PolizaHIP1"].Value = Datos.PolizaHIP1.Value;
                    mySqlCommandSel.Parameters["@PolizaHIP2"].Value = Datos.PolizaHIP2.Value;
                    mySqlCommandSel.Parameters["@FechaIniVEH1"].Value = Datos.FechaIniVEH1.Value;
                    mySqlCommandSel.Parameters["@FechaFinVEH1"].Value = Datos.FechaFinVEH1.Value;
                    mySqlCommandSel.Parameters["@FechaIniVEH2"].Value = Datos.FechaIniVEH2.Value;
                    mySqlCommandSel.Parameters["@FechaFinVEH2"].Value = Datos.FechaFinVEH2.Value;
                    mySqlCommandSel.Parameters["@FechaIniHIP1"].Value = Datos.FechaIniHIP1.Value;
                    mySqlCommandSel.Parameters["@FechaFinHIP1"].Value = Datos.FechaFinHIP1.Value;
                    mySqlCommandSel.Parameters["@FechaIniHIP2"].Value = Datos.FechaIniHIP2.Value;
                    mySqlCommandSel.Parameters["@FechaFinHIP2"].Value = Datos.FechaFinHIP2.Value;
                    mySqlCommandSel.Parameters["@Aseguradora1VEH"].Value = Datos.Aseguradora1VEH.Value;
                    mySqlCommandSel.Parameters["@Aseguradora2VEH"].Value = Datos.Aseguradora2VEH.Value;
                    mySqlCommandSel.Parameters["@Aseguradora1HIP"].Value = Datos.Aseguradora1HIP.Value;
                    mySqlCommandSel.Parameters["@Aseguradora2HIP"].Value = Datos.Aseguradora2HIP.Value;

                    mySqlCommandSel.Parameters["@VlrPolizaVEH1"].Value = Datos.VlrPolizaVEH1.Value;
                    mySqlCommandSel.Parameters["@CancelaContadoPolizaIndVEH1"].Value = Datos.CancelaContadoPolizaIndVEH1.Value;
                    mySqlCommandSel.Parameters["@IntermediarioExternoIndVEH1"].Value = Datos.IntermediarioExternoIndVEH1.Value;
                    mySqlCommandSel.Parameters["@VlrPolizaVEH2"].Value = Datos.VlrPolizaVEH2.Value;
                    mySqlCommandSel.Parameters["@CancelaContadoPolizaIndVEH2"].Value = Datos.CancelaContadoPolizaIndVEH2.Value;
                    mySqlCommandSel.Parameters["@IntermediarioExternoIndVEH2"].Value = Datos.IntermediarioExternoIndVEH2.Value;
                    mySqlCommandSel.Parameters["@VlrPolizaHIP1"].Value = Datos.VlrPolizaHIP1.Value;
                    mySqlCommandSel.Parameters["@VlrPolizaHIP2"].Value = Datos.VlrPolizaHIP2.Value;

                    #endregion
                    mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;

                    //Eg
                    mySqlCommandSel.Parameters["@eg_ccFasecolda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccFasecolda, this.Empresa, egCtrl);
                    mySqlCommandSel.Parameters["@eg_glGarantia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glGarantia, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosVehiculo_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla drSolicitudDatosVehiculo
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_drSolicitudDatosVehiculo_Update(DTO_drSolicitudDatosVehiculo Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                              "UPDATE drSolicitudDatosVehiculo SET " +
                                                                "GarantiaID=@GarantiaID" +
                                                                ",Placa=@Placa" +
                                                                ",DocPrenda=@DocPrenda" +
                                                                ",OrigenDocumento=@OrigenDocumento" +
                                                                ",PrefijoPrenda=@PrefijoPrenda" +
                                                                ",NumeroPrenda=@NumeroPrenda" +
                                                                ",TipoPrenda=@TipoPrenda" +
                                                                ",Garante=@Garante" +
                                                                ",Marca=@Marca" +
                                                                ",Linea=@Linea" +
                                                                ",Referencia=@Referencia" +
                                                                ",Cilindraje=@Cilindraje" +
                                                                ",Tipocaja=@Tipocaja" +
                                                                ",Complemento=@Complemento" +
                                                                ",AireAcondicionado=@AireAcondicionado" +
                                                                ",PuertasNro=@PuertasNro" +
                                                                ",Termoking=@Termoking" +
                                                                ",FasecoldaCod=@FasecoldaCod" +
                                                                ",VlrFasecolda=@VlrFasecolda" +
                                                                ",Carroceria=@Carroceria" +
                                                                ",Peso=@Peso" +
                                                                ",Servicio=@Servicio" +
                                                                ",CeroKmInd=@CeroKmInd" +
                                                                ",Modelo=@Modelo" +
                                                                ",PrecioVenta=@PrecioVenta" +
                                                                ",PrecioVentaChasis=@PrecioVentaChasis" +
                                                                ",PrecioVentaComplemento=@PrecioVentaComplemento" +
                                                                ",CuotaInicial=@CuotaInicial" +
                                                                ",Registrada=@Registrada" +
                                                                ",ChasisYComplementoIND=@ChasisYComplementoIND" +
                                                                ",Motor=@Motor" +
                                                                ",Serie=@Serie" +
                                                                ",Chasis=@Chasis" +
                                                                ",Clase=@Clase" +
                                                                ",Color=@Color" +
                                                                ",Tipo=@Tipo" +
                                                                ",NumeroFactura=@NumeroFactura" +
                                                                ",TipoFactura=@TipoFactura" +
                                                                ",InmuebleTipo=@InmuebleTipo" +
                                                                ",Matricula=@Matricula" +
                                                                ",Direccion=@Direccion" +
                                                                ",Ano=@Ano" +
                                                                ",FuenteHIP=@FuenteHIP" +
                                                                ",CodigoGarantia=@CodigoGarantia" +
                                                                ",VlrFuente=@VlrFuente" +
                                                                ",FechaFuente=@FechaFuente" +
                                                                ",CodigoGarantia1=@CodigoGarantia1" +
                                                                ",VlrGarantia=@VlrGarantia" +
                                                                ",IndValidado=@IndValidado"+
                                                                ",IndValidaHipoteca=@IndValidaHipoteca"+
                                                                ",RUNT=@RUNT" +
                                                                ",Confecamaras=@Confecamaras" +
                                                                ",FechaVTO=@FechaVTO" +
                                                                ",Ciudad=@Ciudad" +
                                                                ",FechaPredial=@FechaPredial" +
                                                                ",ValorPredial=@ValorPredial" +
                                                                ",FechaAvaluo=@FechaAvaluo" +
                                                                ",ValorAvaluo=@ValorAvaluo" +
                                                                ",FechaPromesa=@FechaPromesa" +
                                                                ",ValorCompraventa=@ValorCompraventa" +
                                                                ",ViviendaNuevaInd=@ViviendaNuevaInd"+
                                                                ",PrefijoHipoteca=@PrefijoHipoteca" +
                                                                ",NumeroHipoteca=@NumeroHipoteca" +
                                                                ",FechaInicio=@FechaInicio" +
                                                                ",ValorPoliza=@ValorPoliza" +
                                                                ",Escritura=@Escritura" +
                                                                ",FechaEscritura=@FechaEscritura" +
                                                                ",Notaria=@Notaria" +
                                                                ",Marca_2=@Marca_2" +
                                                                ",Linea_2=@Linea_2" +
                                                                ",Referencia_2=@Referencia_2" +
                                                                ",Cilindraje_2=@Cilindraje_2" +
                                                                ",Tipocaja_2=@Tipocaja_2" +
                                                                ",Complemento_2=@Complemento_2" +
                                                                ",AireAcondicionado_2=@AireAcondicionado_2" +
                                                                ",PuertasNro_2=@PuertasNro_2" +
                                                                ",Termoking_2=@Termoking_2" +
                                                                ",FasecoldaCod_2=@FasecoldaCod_2" +
                                                                ",VlrFasecolda_2=@VlrFasecolda_2" +
                                                                ",Carroceria_2=@Carroceria_2" +
                                                                ",Peso_2=@Peso_2" +
                                                                ",Servicio_2=@Servicio_2" +
                                                                ",CeroKmInd_2=@CeroKmInd_2" +
                                                                ",Modelo_2=@Modelo_2" +
                                                                ",PrecioVenta_2=@PrecioVenta_2" +
                                                                ",Registrada_2=@Registrada_2" +
                                                                ",ChasisYComplementoIND_2=@ChasisYComplementoIND_2" +
                                                                ",IndValidado_2=@IndValidado_2" +
                                                                ",PrefijoPrenda_2=@PrefijoPrenda_2" +
                                                                ",NumeroPrenda_2=@NumeroPrenda_2" +
                                                                ",Garante_2=@Garante_2" +
                                                                ",Placa_2=@Placa_2" +
                                                                ",Motor_2=@Motor_2" +
                                                                ",Serie_2=@Serie_2" +
                                                                ",Chasis_2=@Chasis_2" +
                                                                ",Clase_2=@Clase_2" +
                                                                ",Color_2=@Color_2" +
                                                                ",Tipo_2=@Tipo_2" +
                                                                ",DocPrenda_2=@DocPrenda_2" +
                                                                ",NumeroFactura_2=@NumeroFactura_2" +
                                                                ",InmuebleTipo_2=@InmuebleTipo_2" +
                                                                ",Direccion_2=@Direccion_2" +
                                                                ",Ciudad_2=@Ciudad_2" +
                                                                ",Matricula_2=@Matricula_2" +
                                                                ",FechaAvaluo_2=@FechaAvaluo_2" +
                                                                ",ValorAvaluo_2=@ValorAvaluo_2" +
                                                                ",ViviendaNuevaInd_2=@ViviendaNuevaInd_2" +
                                                                ",FechaPredial_2=@FechaPredial_2" +
                                                                ",ValorPredial_2=@ValorPredial_2" +
                                                                ",FechaPromesa_2=@FechaPromesa_2" +
                                                                ",ValorCompraventa_2=@ValorCompraventa_2" +
                                                                ",IndValidaHipoteca_2=@IndValidaHipoteca_2" +
                                                                ",PrefijoHipoteca_2=@PrefijoHipoteca_2" +
                                                                ",NumeroHipoteca_2=@NumeroHipoteca_2" +
                                                                ",RUNT_2=@RUNT_2" +
                                                                ",Confecamaras_2=@Confecamaras_2" +
                                                                ",FechaRegistro_2=@FechaRegistro_2" +
                                                                ",FechaInicio_2=@FechaInicio_2" +
                                                                ",FechaVto_2=@FechaVto_2" +
                                                                ",ValorPoliza_2=@ValorPoliza_2" +
                                                                ",Escritura_2=@Escritura_2" +
                                                                ",FechaEscritura_2=@FechaEscritura_2" +
                                                                ",Notaria_2=@Notaria_2" +
                                                                ",PagareCRD=@PagareCRD" +
                                                                ",PagarePOL=@PagarePOL" +
                                                                ",PolizaVEH1=@PolizaVEH1" +
                                                                ",PolizaVEH2=@PolizaVEH2" +
                                                                ",PolizaHIP1=@PolizaHIP1" +
                                                                ",PolizaHIP2=@PolizaHIP2" +
                                                                ",FechaIniVEH1=@FechaIniVEH1" +
                                                                ",FechaFinVEH1=@FechaFinVEH1" +
                                                                ",FechaIniVEH2=@FechaIniVEH2" +
                                                                ",FechaFinVEH2=@FechaFinVEH2" +
                                                                ",FechaIniHIP1=@FechaIniHIP1" +
                                                                ",FechaFinHIP1=@FechaFinHIP1" +
                                                                ",FechaIniHIP2=@FechaIniHIP2" +
                                                                ",FechaFinHIP2=@FechaFinHIP2" +
                                                                ",Aseguradora1VEH=@Aseguradora1VEH" +
                                                                ",Aseguradora2VEH=@Aseguradora2VEH" +
                                                                ",Aseguradora1HIP=@Aseguradora1HIP" +
                                                                ",Aseguradora2HIP=@Aseguradora2HIP" +
                                                                ",VlrPolizaVEH1=@VlrPolizaVEH1" +
                                                                ",CancelaContadoPolizaIndVEH1=@CancelaContadoPolizaIndVEH1" +
                                                                ",IntermediarioExternoIndVEH1=@IntermediarioExternoIndVEH1" +
                                                                ",VlrPolizaVEH2=@VlrPolizaVEH2" +
                                                                ",CancelaContadoPolizaIndVEH2=@CancelaContadoPolizaIndVEH2" +
                                                                ",IntermediarioExternoIndVEH2=@IntermediarioExternoIndVEH2" +
                                                                ",VlrPolizaHIP1=@VlrPolizaHIP1" +
                                                                ",VlrPolizaHIP2=@VlrPolizaHIP2" +
                                                                ",eg_ccFasecolda=@eg_ccFasecolda" +
                                                                ",eg_glGarantia=@eg_glGarantia"+
                                                                " WHERE  Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos

                //mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                //mySqlCommandSel.Parameters.Add("@Version", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@GarantiaID", SqlDbType.VarChar, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@Placa", SqlDbType.Char, 6);
                mySqlCommandSel.Parameters.Add("@DocPrenda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@OrigenDocumento", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@PrefijoPrenda", SqlDbType.VarChar, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroPrenda", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoPrenda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Garante", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Marca", SqlDbType.VarChar, 100);
                mySqlCommandSel.Parameters.Add("@Linea", SqlDbType.VarChar, 100);
                mySqlCommandSel.Parameters.Add("@Referencia", SqlDbType.VarChar, 100);
                mySqlCommandSel.Parameters.Add("@Cilindraje", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Tipocaja", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Complemento", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@AireAcondicionado", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PuertasNro", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Termoking", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@FasecoldaCod", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrFasecolda", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Carroceria", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@Peso", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Servicio", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CeroKmInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Modelo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PrecioVenta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PrecioVentaChasis", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PrecioVentaComplemento", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaInicial", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Registrada", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ChasisYComplementoIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Motor", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Serie", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Chasis", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Clase", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Color", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Tipo", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@NumeroFactura", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@TipoFactura", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@InmuebleTipo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Matricula", SqlDbType.VarChar, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@Direccion", SqlDbType.VarChar, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@FuenteHIP", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CodigoGarantia", SqlDbType.VarChar, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrFuente", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaFuente", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CodigoGarantia1", SqlDbType.VarChar, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrGarantia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IndValidado", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndValidaHipoteca", SqlDbType.Bit);
                
                mySqlCommandSel.Parameters.Add("@RUNT", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@Confecamaras", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@FechaVTO", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@FechaPredial", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorPredial", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaAvaluo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorAvaluo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaPromesa", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorCompraventa", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ViviendaNuevaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@eg_ccFasecolda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glGarantia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                //
                #region campos nuevos
                mySqlCommandSel.Parameters.Add("@PrefijoHipoteca", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroHipoteca", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorPoliza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Escritura", SqlDbType.VarChar, 10);
                mySqlCommandSel.Parameters.Add("@FechaEscritura", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Notaria", SqlDbType.VarChar, 10);
                mySqlCommandSel.Parameters.Add("@Marca_2", SqlDbType.VarChar, 100);
                mySqlCommandSel.Parameters.Add("@Linea_2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Referencia_2", SqlDbType.VarChar, 100);
                mySqlCommandSel.Parameters.Add("@Cilindraje_2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Tipocaja_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Complemento_2", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@AireAcondicionado_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PuertasNro_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Termoking_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@FasecoldaCod_2", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrFasecolda_2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Carroceria_2", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@Peso_2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Servicio_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CeroKmInd_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Modelo_2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PrecioVenta_2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Registrada_2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ChasisYComplementoIND_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndValidado_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PrefijoPrenda_2", SqlDbType.VarChar, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroPrenda_2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Garante_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Placa_2", SqlDbType.Char, 6);
                mySqlCommandSel.Parameters.Add("@Motor_2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Serie_2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Chasis_2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Clase_2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Color_2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@Tipo_2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DocPrenda_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NumeroFactura_2", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@InmuebleTipo_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Direccion_2", SqlDbType.VarChar, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ciudad_2", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@Matricula_2", SqlDbType.VarChar, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaAvaluo_2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorAvaluo_2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ViviendaNuevaInd_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@FechaPredial_2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorPredial_2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaPromesa_2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorCompraventa_2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IndValidaHipoteca_2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PrefijoHipoteca_2", SqlDbType.VarChar, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroHipoteca_2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@RUNT_2", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@Confecamaras_2", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@FechaRegistro_2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaInicio_2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVto_2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorPoliza_2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Escritura_2", SqlDbType.VarChar, 10);
                mySqlCommandSel.Parameters.Add("@FechaEscritura_2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Notaria_2", SqlDbType.VarChar, 10);
                mySqlCommandSel.Parameters.Add("@PagareCRD", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@PagarePOL", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@PolizaVEH1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@PolizaVEH2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@PolizaHIP1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@PolizaHIP2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@FechaIniVEH1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinVEH1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaIniVEH2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinVEH2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaIniHIP1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinHIP1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaIniHIP2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinHIP2", SqlDbType.SmallDateTime);

                mySqlCommandSel.Parameters.Add("@Aseguradora1VEH", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@Aseguradora2VEH", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@Aseguradora1HIP", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@Aseguradora2HIP", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrPolizaVEH1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CancelaContadoPolizaIndVEH1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IntermediarioExternoIndVEH1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VlrPolizaVEH2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CancelaContadoPolizaIndVEH2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IntermediarioExternoIndVEH2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VlrPolizaHIP1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPolizaHIP2", SqlDbType.Decimal);

                #endregion

                #endregion
                #region Asigna los valores


                mySqlCommandSel.Parameters["@GarantiaID"].Value = Datos.GarantiaID.Value;
                mySqlCommandSel.Parameters["@Placa"].Value = Datos.Placa.Value;
                mySqlCommandSel.Parameters["@DocPrenda"].Value = Datos.DocPrenda.Value;
                mySqlCommandSel.Parameters["@OrigenDocumento"].Value = Datos.OrigenDocumento.Value;
                mySqlCommandSel.Parameters["@PrefijoPrenda"].Value = Datos.PrefijoPrenda.Value;
                mySqlCommandSel.Parameters["@NumeroPrenda"].Value = Datos.NumeroPrenda.Value;
                mySqlCommandSel.Parameters["@TipoPrenda"].Value = Datos.TipoPrenda.Value;
                mySqlCommandSel.Parameters["@Garante"].Value = Datos.Garante.Value;
                mySqlCommandSel.Parameters["@Marca"].Value = Datos.Marca.Value;
                mySqlCommandSel.Parameters["@Linea"].Value = Datos.Linea.Value;
                mySqlCommandSel.Parameters["@Referencia"].Value = Datos.Referencia.Value;
                mySqlCommandSel.Parameters["@Cilindraje"].Value = Datos.Cilindraje.Value;
                mySqlCommandSel.Parameters["@Tipocaja"].Value = Datos.Tipocaja.Value;
                mySqlCommandSel.Parameters["@Complemento"].Value = Datos.Complemento.Value;
                mySqlCommandSel.Parameters["@AireAcondicionado"].Value = Datos.AireAcondicionado.Value;
                mySqlCommandSel.Parameters["@PuertasNro"].Value = Datos.PuertasNro.Value;
                mySqlCommandSel.Parameters["@Termoking"].Value = Datos.Termoking.Value;
                mySqlCommandSel.Parameters["@FasecoldaCod"].Value = Datos.FasecoldaCod.Value;
                mySqlCommandSel.Parameters["@VlrFasecolda"].Value = Datos.VlrFasecolda.Value;
                mySqlCommandSel.Parameters["@Carroceria"].Value = Datos.Carroceria.Value;
                mySqlCommandSel.Parameters["@Peso"].Value = Datos.Peso.Value;
                mySqlCommandSel.Parameters["@Servicio"].Value = Datos.Servicio.Value;
                mySqlCommandSel.Parameters["@CeroKmInd"].Value = Datos.CeroKmInd.Value;
                mySqlCommandSel.Parameters["@Modelo"].Value = Datos.Modelo.Value;
                mySqlCommandSel.Parameters["@PrecioVenta"].Value = Datos.PrecioVenta.Value;
                mySqlCommandSel.Parameters["@PrecioVentaChasis"].Value = Datos.PrecioVentaChasis.Value;
                mySqlCommandSel.Parameters["@PrecioVentaComplemento"].Value = Datos.PrecioVentaComplemento.Value;
                mySqlCommandSel.Parameters["@CuotaInicial"].Value = Datos.CuotaInicial.Value;
                mySqlCommandSel.Parameters["@Registrada"].Value = Datos.Registrada.Value;
                mySqlCommandSel.Parameters["@ChasisYComplementoIND"].Value = Datos.ChasisYComplementoIND.Value;
                mySqlCommandSel.Parameters["@Motor"].Value = Datos.Motor.Value;
                mySqlCommandSel.Parameters["@Serie"].Value = Datos.Serie.Value;
                mySqlCommandSel.Parameters["@Chasis"].Value = Datos.Chasis.Value;
                mySqlCommandSel.Parameters["@Clase"].Value = Datos.Clase.Value;
                mySqlCommandSel.Parameters["@Color"].Value = Datos.Color.Value;
                mySqlCommandSel.Parameters["@Tipo"].Value = Datos.Tipo.Value;
                mySqlCommandSel.Parameters["@NumeroFactura"].Value = Datos.NumeroFactura.Value;
                mySqlCommandSel.Parameters["@TipoFactura"].Value = Datos.TipoFactura.Value;
                mySqlCommandSel.Parameters["@InmuebleTipo"].Value = Datos.InmuebleTipo.Value;
                mySqlCommandSel.Parameters["@Matricula"].Value = Datos.Matricula.Value;
                mySqlCommandSel.Parameters["@Direccion"].Value = Datos.Direccion.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = Datos.Ano.Value;
                mySqlCommandSel.Parameters["@FuenteHIP"].Value = Datos.FuenteHIP.Value;
                mySqlCommandSel.Parameters["@CodigoGarantia"].Value = Datos.CodigoGarantia.Value;
                mySqlCommandSel.Parameters["@VlrFuente"].Value = Datos.VlrFuente.Value;
                mySqlCommandSel.Parameters["@FechaFuente"].Value = Datos.FechaFuente.Value;
                mySqlCommandSel.Parameters["@CodigoGarantia1"].Value = Datos.CodigoGarantia1.Value;
                mySqlCommandSel.Parameters["@VlrGarantia"].Value = Datos.VlrGarantia.Value;
                mySqlCommandSel.Parameters["@IndValidado"].Value = Datos.IndValidado.Value;
                mySqlCommandSel.Parameters["@IndValidaHipoteca"].Value = Datos.IndValidaHipoteca.Value;
                
                mySqlCommandSel.Parameters["@RUNT"].Value = Datos.RUNT.Value;
                mySqlCommandSel.Parameters["@Confecamaras"].Value = Datos.Confecamaras.Value;
                mySqlCommandSel.Parameters["@FechaVTO"].Value = Datos.FechaVTO.Value;
                mySqlCommandSel.Parameters["@Ciudad"].Value = Datos.Ciudad.Value;
                mySqlCommandSel.Parameters["@FechaPredial"].Value = Datos.FechaPredial.Value;
                mySqlCommandSel.Parameters["@ValorPredial"].Value = Datos.ValorPredial.Value;
                mySqlCommandSel.Parameters["@FechaAvaluo"].Value = Datos.FechaAvaluo.Value;
                mySqlCommandSel.Parameters["@ValorAvaluo"].Value = Datos.ValorAvaluo.Value;
                mySqlCommandSel.Parameters["@FechaPromesa"].Value = Datos.FechaPromesa.Value;
                mySqlCommandSel.Parameters["@ValorCompraventa"].Value = Datos.ValorCompraventa.Value;
                mySqlCommandSel.Parameters["@ViviendaNuevaInd"].Value = Datos.ViviendaNuevaInd.Value;

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = Datos.Consecutivo.Value;

                #region campos nuevos
                mySqlCommandSel.Parameters["@PrefijoHipoteca"].Value = Datos.PrefijoHipoteca.Value;
                mySqlCommandSel.Parameters["@NumeroHipoteca"].Value = Datos.NumeroHipoteca.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = Datos.FechaInicio.Value;
                mySqlCommandSel.Parameters["@ValorPoliza"].Value = Datos.ValorPoliza.Value;
                mySqlCommandSel.Parameters["@Escritura"].Value = Datos.Escritura.Value;
                mySqlCommandSel.Parameters["@FechaEscritura"].Value = Datos.FechaEscritura.Value;
                mySqlCommandSel.Parameters["@Notaria"].Value = Datos.Notaria.Value;
                mySqlCommandSel.Parameters["@Marca_2"].Value = Datos.Marca_2.Value;
                mySqlCommandSel.Parameters["@Linea_2"].Value = Datos.Linea_2.Value;
                mySqlCommandSel.Parameters["@Referencia_2"].Value = Datos.Referencia_2.Value;
                mySqlCommandSel.Parameters["@Cilindraje_2"].Value = Datos.Cilindraje_2.Value;
                mySqlCommandSel.Parameters["@Tipocaja_2"].Value = Datos.Tipocaja_2.Value;
                mySqlCommandSel.Parameters["@Complemento_2"].Value = Datos.Complemento_2.Value;
                mySqlCommandSel.Parameters["@AireAcondicionado_2"].Value = Datos.AireAcondicionado_2.Value;
                mySqlCommandSel.Parameters["@PuertasNro_2"].Value = Datos.PuertasNro_2.Value;
                mySqlCommandSel.Parameters["@Termoking_2"].Value = Datos.Termoking_2.Value;
                mySqlCommandSel.Parameters["@FasecoldaCod_2"].Value = Datos.FasecoldaCod_2.Value;
                mySqlCommandSel.Parameters["@VlrFasecolda_2"].Value = Datos.VlrFasecolda_2.Value;
                mySqlCommandSel.Parameters["@Carroceria_2"].Value = Datos.Carroceria_2.Value;
                mySqlCommandSel.Parameters["@Peso_2"].Value = Datos.Peso_2.Value;
                mySqlCommandSel.Parameters["@Servicio_2"].Value = Datos.Servicio_2.Value;
                mySqlCommandSel.Parameters["@CeroKmInd_2"].Value = Datos.CeroKmInd_2.Value;
                mySqlCommandSel.Parameters["@Modelo_2"].Value = Datos.Modelo_2.Value;
                mySqlCommandSel.Parameters["@PrecioVenta_2"].Value = Datos.PrecioVenta_2.Value;
                mySqlCommandSel.Parameters["@Registrada_2"].Value = Datos.Registrada_2.Value;
                mySqlCommandSel.Parameters["@ChasisYComplementoIND_2"].Value = Datos.ChasisYComplementoIND_2.Value;
                mySqlCommandSel.Parameters["@IndValidado_2"].Value = Datos.IndValidado_2.Value;
                mySqlCommandSel.Parameters["@PrefijoPrenda_2"].Value = Datos.PrefijoPrenda_2.Value;
                mySqlCommandSel.Parameters["@NumeroPrenda_2"].Value = Datos.NumeroPrenda_2.Value;
                mySqlCommandSel.Parameters["@Garante_2"].Value = Datos.Garante_2.Value;
                mySqlCommandSel.Parameters["@Placa_2"].Value = Datos.Placa_2.Value;
                mySqlCommandSel.Parameters["@Motor_2"].Value = Datos.Motor_2.Value;
                mySqlCommandSel.Parameters["@Serie_2"].Value = Datos.Serie_2.Value;
                mySqlCommandSel.Parameters["@Chasis_2"].Value = Datos.Chasis_2.Value;
                mySqlCommandSel.Parameters["@Clase_2"].Value = Datos.Clase_2.Value;
                mySqlCommandSel.Parameters["@Color_2"].Value = Datos.Color_2.Value;
                mySqlCommandSel.Parameters["@Tipo_2"].Value = Datos.Tipo_2.Value;
                mySqlCommandSel.Parameters["@DocPrenda_2"].Value = Datos.DocPrenda_2.Value;
                mySqlCommandSel.Parameters["@NumeroFactura_2"].Value = Datos.NumeroFactura_2.Value;
                mySqlCommandSel.Parameters["@InmuebleTipo_2"].Value = Datos.InmuebleTipo_2.Value;
                mySqlCommandSel.Parameters["@Direccion_2"].Value = Datos.Direccion_2.Value;
                mySqlCommandSel.Parameters["@Ciudad_2"].Value = Datos.Ciudad_2.Value;
                mySqlCommandSel.Parameters["@Matricula_2"].Value = Datos.Matricula_2.Value;
                mySqlCommandSel.Parameters["@FechaAvaluo_2"].Value = Datos.FechaAvaluo_2.Value;
                mySqlCommandSel.Parameters["@ValorAvaluo_2"].Value = Datos.ValorAvaluo_2.Value;
                mySqlCommandSel.Parameters["@ViviendaNuevaInd_2"].Value = Datos.ViviendaNuevaInd_2.Value;
                mySqlCommandSel.Parameters["@FechaPredial_2"].Value = Datos.FechaPredial_2.Value;
                mySqlCommandSel.Parameters["@ValorPredial_2"].Value = Datos.ValorPredial_2.Value;
                mySqlCommandSel.Parameters["@FechaPromesa_2"].Value = Datos.FechaPromesa_2.Value;
                mySqlCommandSel.Parameters["@ValorCompraventa_2"].Value = Datos.ValorCompraventa_2.Value;
                mySqlCommandSel.Parameters["@IndValidaHipoteca_2"].Value = Datos.IndValidaHipoteca_2.Value;
                mySqlCommandSel.Parameters["@PrefijoHipoteca_2"].Value = Datos.PrefijoHipoteca_2.Value;
                mySqlCommandSel.Parameters["@NumeroHipoteca_2"].Value = Datos.NumeroHipoteca_2.Value;
                mySqlCommandSel.Parameters["@RUNT_2"].Value = Datos.RUNT_2.Value;
                mySqlCommandSel.Parameters["@Confecamaras_2"].Value = Datos.Confecamaras_2.Value;
                mySqlCommandSel.Parameters["@FechaRegistro_2"].Value = Datos.FechaRegistro_2.Value;
                mySqlCommandSel.Parameters["@FechaInicio_2"].Value = Datos.FechaInicio_2.Value;
                mySqlCommandSel.Parameters["@FechaVto_2"].Value = Datos.FechaVto_2.Value;
                mySqlCommandSel.Parameters["@ValorPoliza_2"].Value = Datos.ValorPoliza_2.Value;
                mySqlCommandSel.Parameters["@Escritura_2"].Value = Datos.Escritura_2.Value;
                mySqlCommandSel.Parameters["@FechaEscritura_2"].Value = Datos.FechaEscritura_2.Value;
                mySqlCommandSel.Parameters["@Notaria_2"].Value = Datos.Notaria_2.Value;

                mySqlCommandSel.Parameters["@PagareCRD"].Value = Datos.PagareCRD.Value;
                mySqlCommandSel.Parameters["@PagarePOL"].Value = Datos.PagarePOL.Value;
                mySqlCommandSel.Parameters["@PolizaVEH1"].Value = Datos.PolizaVEH1.Value;
                mySqlCommandSel.Parameters["@PolizaVEH2"].Value = Datos.PolizaVEH2.Value;
                mySqlCommandSel.Parameters["@PolizaHIP1"].Value = Datos.PolizaHIP1.Value;
                mySqlCommandSel.Parameters["@PolizaHIP2"].Value = Datos.PolizaHIP2.Value;
                mySqlCommandSel.Parameters["@FechaIniVEH1"].Value = Datos.FechaIniVEH1.Value;
                mySqlCommandSel.Parameters["@FechaFinVEH1"].Value = Datos.FechaFinVEH1.Value;
                mySqlCommandSel.Parameters["@FechaIniVEH2"].Value = Datos.FechaIniVEH2.Value;
                mySqlCommandSel.Parameters["@FechaFinVEH2"].Value = Datos.FechaFinVEH2.Value;
                mySqlCommandSel.Parameters["@FechaIniHIP1"].Value = Datos.FechaIniHIP1.Value;
                mySqlCommandSel.Parameters["@FechaFinHIP1"].Value = Datos.FechaFinHIP1.Value;
                mySqlCommandSel.Parameters["@FechaIniHIP2"].Value = Datos.FechaIniHIP2.Value;
                mySqlCommandSel.Parameters["@FechaFinHIP2"].Value = Datos.FechaFinHIP2.Value;
                mySqlCommandSel.Parameters["@Aseguradora1VEH"].Value = Datos.Aseguradora1VEH.Value;
                mySqlCommandSel.Parameters["@Aseguradora2VEH"].Value = Datos.Aseguradora2VEH.Value;
                mySqlCommandSel.Parameters["@Aseguradora1HIP"].Value = Datos.Aseguradora1HIP.Value;
                mySqlCommandSel.Parameters["@Aseguradora2HIP"].Value = Datos.Aseguradora2HIP.Value;

                mySqlCommandSel.Parameters["@VlrPolizaVEH1"].Value = Datos.VlrPolizaVEH1.Value;
                mySqlCommandSel.Parameters["@CancelaContadoPolizaIndVEH1"].Value = Datos.CancelaContadoPolizaIndVEH1.Value;
                mySqlCommandSel.Parameters["@IntermediarioExternoIndVEH1"].Value = Datos.IntermediarioExternoIndVEH1.Value;
                mySqlCommandSel.Parameters["@VlrPolizaVEH2"].Value = Datos.VlrPolizaVEH2.Value;
                mySqlCommandSel.Parameters["@CancelaContadoPolizaIndVEH2"].Value = Datos.CancelaContadoPolizaIndVEH2.Value;
                mySqlCommandSel.Parameters["@IntermediarioExternoIndVEH2"].Value = Datos.IntermediarioExternoIndVEH2.Value;
                mySqlCommandSel.Parameters["@VlrPolizaHIP1"].Value = Datos.VlrPolizaHIP1.Value;
                mySqlCommandSel.Parameters["@VlrPolizaHIP2"].Value = Datos.VlrPolizaHIP2.Value;

                #endregion                
                //Eg
                mySqlCommandSel.Parameters["@eg_ccFasecolda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccFasecolda, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glGarantia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glGarantia, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosVehiculo_Update");
                throw exception;
            }
        }

        public void DAL_drSolicitudDatosVehiculo_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM drSolicitudDatosVehiculo WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosVehiculo_Delete");
                throw exception;
            }
        }

        #endregion

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="consec"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_drSolicitudDatosVehiculo_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from drSolicitudDatosVehiculo with(nolock) where Consecutivo = @Consecutivo";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;


                if (mySqlCommand.Parameters["@Consecutivo"].Value == null || ((mySqlCommand.Parameters["@Consecutivo"].Value is string) &&
                    string.IsNullOrWhiteSpace(mySqlCommand.Parameters["@Consecutivo"].Value.ToString())))
                    mySqlCommand.Parameters["@Consecutivo"].Value = DBNull.Value;


                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosVehiculo_Exist");
                throw exception;
            }
        }

    }
}
