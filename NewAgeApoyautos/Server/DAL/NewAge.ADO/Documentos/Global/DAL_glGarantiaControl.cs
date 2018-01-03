using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_glGarantiaControl
    /// </summary>
    public class DAL_glGarantiaControl : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glGarantiaControl(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Privadas

        /// <summary>
        /// Agrega un registro al control de garantias
        /// </summary>
        private void DAL_glGarantiaControl_AddItem(DTO_glGarantiaControl garCtrl)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "INSERT INTO glGarantiaControl" +
                            "           (EmpresaID" +
                            "           ,TerceroID" +
                            "           ,DocumentoID" +
                            "           ,NumeroDoc" +
                            "           ,GarantiaID" +
                            "           ,FechaINI" +
                            "           ,FechaVTO" +
                            "           ,FechaRegistro" +
                            "           ,CodigoGarantia" +
                            "           ,CodigoGarantia1" +
                            "           ,Modelo " +
                            "           ,Ano" +
                            "           ,FasecoldaID" +
                            "           ,FuentePRE" +
                            "           ,FuenteHIP" +
                            "           ,Direccion" +
                            "           ,VlrFuente" +
                            "           ,FechaFuente" +
                            "           ,VlrAsegurado" +
                            "           ,Valor1" +
                            "           ,Valor2" +
                            "           ,Valor3" +
                            "           ,Dato1" +
                            "           ,Dato2" +
                            "           ,Dato3" +
                            "           ,Descripcion" +
                            "           ,ActivoInd" +
                            "           ,NuevoInd" +
                            "           ,Matricula" +
                            "           ,VehiculoTipo" +
                            "           ,InmuebleTipo" +
                            "           ,VlrGarantia" +
                            "           ,Placa,RetiraInd,PrefijoID,DocumentoNro" +
                            "           ,eg_coTercero" +
                            "           ,eg_glGarantia" +
                            "           ,eg_ccFasecolda)" +
                            "     VALUES" +
                            "           (@EmpresaID" +
                            "           ,@TerceroID" +
                            "           ,@DocumentoID" +
                            "           ,@NumeroDoc" +
                            "           ,@GarantiaID" +
                            "           ,@FechaINI" +
                            "           ,@FechaVTO" +
                            "           ,@FechaRegistro" +                            
                            "           ,@CodigoGarantia" +
                            "           ,@CodigoGarantia1" +
                            "           ,@Modelo " +
                            "           ,@Ano" +
                            "           ,@FasecoldaID" +
                            "           ,@FuentePRE" +
                            "           ,@FuenteHIP" +
                            "           ,@Direccion" +
                            "           ,@VlrFuente" +
                            "           ,@FechaFuente" +
                            "           ,@VlrAsegurado" +
                            "           ,@Valor1" +
                            "           ,@Valor2" +
                            "           ,@Valor3" +
                            "           ,@Dato1" +
                            "           ,@Dato2" +
                            "           ,@Dato3" +
                            "           ,@Descripcion" +
                            "           ,@ActivoInd" +
                            "           ,@NuevoInd" +
                            "           ,@Matricula" +
                            "           ,@VehiculoTipo" +
                            "           ,@InmuebleTipo" +
                            "           ,@VlrGarantia" +
                            "           ,@Placa,@RetiraInd,@PrefijoID,@DocumentoNro" +
                            "           ,@eg_coTercero" +
                            "           ,@eg_glGarantia" +
                            "           ,@eg_ccFasecolda)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@GarantiaID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@FechaINI", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaVTO", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaRegistro", SqlDbType.SmallDateTime);                
                mySqlCommand.Parameters.Add("@CodigoGarantia", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommand.Parameters.Add("@CodigoGarantia1", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommand.Parameters.Add("@Modelo", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@Ano", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@FasecoldaID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@FuentePRE", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FuenteHIP", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Direccion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@VlrFuente", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FechaFuente", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@VlrAsegurado", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Dato1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Descripcion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@NuevoInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@RetiraInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@Matricula", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommand.Parameters.Add("@VehiculoTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@InmuebleTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@VlrGarantia", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Placa", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RetiraInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glGarantia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccFasecolda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = garCtrl.TerceroID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = garCtrl.DocumentoID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = garCtrl.NumeroDoc.Value;
                mySqlCommand.Parameters["@GarantiaID"].Value = garCtrl.GarantiaID.Value;
                mySqlCommand.Parameters["@FechaINI"].Value = garCtrl.FechaINI.Value;
                mySqlCommand.Parameters["@FechaVTO"].Value = garCtrl.FechaVTO.Value;
                mySqlCommand.Parameters["@FechaRegistro"].Value = garCtrl.FechaRegistro.Value;                
                mySqlCommand.Parameters["@CodigoGarantia"].Value = garCtrl.CodigoGarantia.Value;
                mySqlCommand.Parameters["@CodigoGarantia1"].Value = garCtrl.CodigoGarantia1.Value;
                mySqlCommand.Parameters["@Modelo"].Value = garCtrl.Modelo.Value;
                mySqlCommand.Parameters["@Ano"].Value = garCtrl.Ano.Value;
                mySqlCommand.Parameters["@FasecoldaID"].Value = garCtrl.FaseColdaID.Value;
                mySqlCommand.Parameters["@FuentePRE"].Value = garCtrl.FuentePRE.Value;
                mySqlCommand.Parameters["@FuenteHIP"].Value = garCtrl.FuenteHIP.Value;
                mySqlCommand.Parameters["@Direccion"].Value = garCtrl.Direccion.Value;
                mySqlCommand.Parameters["@VlrFuente"].Value = garCtrl.VlrFuente.Value;
                mySqlCommand.Parameters["@FechaFuente"].Value = garCtrl.FechaFuente.Value;
                mySqlCommand.Parameters["@VlrAsegurado"].Value = garCtrl.VlrAsegurado.Value;
                mySqlCommand.Parameters["@Valor1"].Value = garCtrl.Valor1.Value;
                mySqlCommand.Parameters["@Valor2"].Value = garCtrl.Valor2.Value;
                mySqlCommand.Parameters["@Valor3"].Value = garCtrl.Valor3.Value;
                mySqlCommand.Parameters["@Dato1"].Value = garCtrl.Dato1.Value;
                mySqlCommand.Parameters["@Dato2"].Value = garCtrl.Dato2.Value;
                mySqlCommand.Parameters["@Dato3"].Value = garCtrl.Dato3.Value;
                mySqlCommand.Parameters["@Descripcion"].Value = garCtrl.Descripcion.Value;
                mySqlCommand.Parameters["@ActivoInd"].Value = garCtrl.ActivoInd.Value;
                mySqlCommand.Parameters["@RetiraInd"].Value = garCtrl.RetiraInd.Value;
                mySqlCommand.Parameters["@NuevoInd"].Value = garCtrl.NuevoInd.Value;
                mySqlCommand.Parameters["@Matricula"].Value = garCtrl.Matricula.Value;
                mySqlCommand.Parameters["@VehiculoTipo"].Value = garCtrl.VehiculoTipo.Value;
                mySqlCommand.Parameters["@InmuebleTipo"].Value = garCtrl.InmuebleTipo.Value;
                mySqlCommand.Parameters["@VlrGarantia"].Value = garCtrl.VlrGarantia.Value;
                mySqlCommand.Parameters["@Placa"].Value = garCtrl.Placa.Value;
                mySqlCommand.Parameters["@PrefijoID"].Value = garCtrl.PrefijoID.Value;
                mySqlCommand.Parameters["@DocumentoNro"].Value = garCtrl.DocumentoNro.Value;
                mySqlCommand.Parameters["@RetiraInd"].Value = garCtrl.RetiraInd.Value;
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glGarantia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glGarantia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccFasecolda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccFasecolda, this.Empresa, egCtrl);
                #endregion

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
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glGarantiaControl_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Edita un registro al control de documentos
        /// </summary>
        /// <param name="garCtrl">Documento que se va a editar</param>
        private void DAL_glGarantiaControl_UpdateItem(DTO_glGarantiaControl garCtrl)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "UPDATE glGarantiaControl SET " +
                            "TerceroID = @TerceroID," +
                            "DocumentoID = @DocumentoID," +
                            "NumeroDoc = @NumeroDoc," +
                            "GarantiaID = @GarantiaID," +
                            "FechaINI = @FechaINI," +
                            "FechaVTO = @FechaVTO," +
                            "FechaRegistro=@FechaRegistro,"+
                            "CodigoGarantia = @CodigoGarantia," +
                            "CodigoGarantia1 = @CodigoGarantia1," +
                            "Modelo  = @Modelo," +
                            "Ano = @Ano," +
                            "FasecoldaID = @FasecoldaID," +
                            "FuentePRE = @FuentePRE," +
                            "FuenteHIP = @FuenteHIP," +
                            "Direccion = @Direccion," +
                            "VlrFuente = @VlrFuente," +
                            "FechaFuente = @FechaFuente," +
                            "VlrAsegurado = @VlrAsegurado," +
                            "Valor1 = @Valor1," +
                            "Valor2 = @Valor2," +
                            "Valor3 = @Valor3," +
                            "Dato1 = @Dato1," +
                            "Dato2 = @Dato2," +
                            "Dato3 = @Dato3," +
                            "Descripcion = @Descripcion," +
                            "ActivoInd = @ActivoInd, " +
                            "NuevoInd = @NuevoInd, " +
                            "Matricula = @Matricula, " +
                            "VehiculoTipo = @VehiculoTipo, " +
                            "InmuebleTipo = @InmuebleTipo, " +
                            "VlrGarantia = @VlrGarantia, " +
                            "Placa = @Placa," +  
                            "PrefijoID=@PrefijoID,"+
                            "DocumentoNro=@DocumentoNro," +
                            "RetiraInd=@RetiraInd"+
                        "WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@GarantiaID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@FechaINI", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaVTO", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaRegistro", SqlDbType.SmallDateTime);                
                mySqlCommand.Parameters.Add("@CodigoGarantia", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommand.Parameters.Add("@CodigoGarantia1", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommand.Parameters.Add("@Modelo", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@Ano", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@FasecoldaID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@FuentePRE", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FuenteHIP", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Direccion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@VlrFuente", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FechaFuente", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@VlrAsegurado", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Dato1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Dato3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Descripcion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@NuevoInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@Matricula", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommand.Parameters.Add("@VehiculoTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@InmuebleTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@VlrGarantia", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Placa", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RetiraInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glGarantia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccFasecolda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = garCtrl.TerceroID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = garCtrl.DocumentoID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = garCtrl.NumeroDoc.Value;
                mySqlCommand.Parameters["@GarantiaID"].Value = garCtrl.GarantiaID.Value;
                mySqlCommand.Parameters["@FechaINI"].Value = garCtrl.FechaINI.Value;
                mySqlCommand.Parameters["@FechaVTO"].Value = garCtrl.FechaVTO.Value;
                mySqlCommand.Parameters["@FechaRegistro"].Value = garCtrl.FechaRegistro.Value;                
                mySqlCommand.Parameters["@CodigoGarantia"].Value = garCtrl.CodigoGarantia.Value;
                mySqlCommand.Parameters["@CodigoGarantia1"].Value = garCtrl.CodigoGarantia1.Value;
                mySqlCommand.Parameters["@Modelo"].Value = garCtrl.Modelo.Value;
                mySqlCommand.Parameters["@Ano"].Value = garCtrl.Ano.Value;
                mySqlCommand.Parameters["@FasecoldaID"].Value = garCtrl.FaseColdaID.Value;
                mySqlCommand.Parameters["@FuentePRE"].Value = garCtrl.FuentePRE.Value;
                mySqlCommand.Parameters["@FuenteHIP"].Value = garCtrl.FuenteHIP.Value;
                mySqlCommand.Parameters["@Direccion"].Value = garCtrl.Direccion.Value;
                mySqlCommand.Parameters["@VlrFuente"].Value = garCtrl.VlrFuente.Value;
                mySqlCommand.Parameters["@FechaFuente"].Value = garCtrl.FechaFuente.Value;
                mySqlCommand.Parameters["@VlrAsegurado"].Value = garCtrl.VlrAsegurado.Value;
                mySqlCommand.Parameters["@Valor1"].Value = garCtrl.Valor1.Value;
                mySqlCommand.Parameters["@Valor2"].Value = garCtrl.Valor2.Value;
                mySqlCommand.Parameters["@Valor3"].Value = garCtrl.Valor3.Value;
                mySqlCommand.Parameters["@Dato1"].Value = garCtrl.Dato1.Value;
                mySqlCommand.Parameters["@Dato2"].Value = garCtrl.Dato2.Value;
                mySqlCommand.Parameters["@Dato3"].Value = garCtrl.Dato3.Value;
                mySqlCommand.Parameters["@Descripcion"].Value = garCtrl.Descripcion.Value;
                mySqlCommand.Parameters["@ActivoInd"].Value = garCtrl.ActivoInd.Value;
                mySqlCommand.Parameters["@NuevoInd"].Value = garCtrl.NuevoInd.Value;
                mySqlCommand.Parameters["@Matricula"].Value = garCtrl.Matricula.Value;
                mySqlCommand.Parameters["@VehiculoTipo"].Value = garCtrl.VehiculoTipo.Value;
                mySqlCommand.Parameters["@InmuebleTipo"].Value = garCtrl.InmuebleTipo.Value;
                mySqlCommand.Parameters["@VlrGarantia"].Value = garCtrl.VlrGarantia.Value;
                mySqlCommand.Parameters["@Placa"].Value = garCtrl.Placa.Value;
                mySqlCommand.Parameters["@PrefijoID"].Value = garCtrl.PrefijoID.Value;
                mySqlCommand.Parameters["@DocumentoNro"].Value = garCtrl.DocumentoNro.Value;
                mySqlCommand.Parameters["@RetiraInd"].Value = garCtrl.RetiraInd.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = garCtrl.Consecutivo.Value;
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glGarantia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glGarantia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccFasecolda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccFasecolda, this.Empresa, egCtrl);
                #endregion

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
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glGarantiaControl_Update");
                throw exception;
            }
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Actualiza un registro de glGarantiaControl
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_glGarantiaControl_AddOrUpdate(DTO_glGarantiaControl garCtrl)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT COUNT (*) from glGarantiaControl with(nolock) " +
                    "WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@Consecutivo"].Value = garCtrl.Consecutivo.Value;
                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                if (count == 0)
                    this.DAL_glGarantiaControl_AddItem(garCtrl);
                else                
                    this.DAL_glGarantiaControl_UpdateItem(garCtrl);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glGarantiaControl_Add");
                throw exception;
            }
        }

        #endregion       
 
        #region Otras

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"> filtro</param>
        /// <returns>Lista </returns>
        public List<DTO_glGarantiaControl> DAL_glGarantiaControl_GetByParameter(DTO_glGarantiaControl filter)
        {
            try
            {
                List<DTO_glGarantiaControl> result = new List<DTO_glGarantiaControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;

                query = "select * " +
                        "from glGarantiaControl with(nolock) " +
                        "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoID.Value.ToString()))
                {
                    query += "and DocumentoID = @DocumentoID ";
                    mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoID"].Value = filter.DocumentoID.Value;
                }
                if (!string.IsNullOrEmpty(filter.TerceroID.Value.ToString()))
                {
                    query += "and TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = filter.TerceroID.Value;
                }
                if (!string.IsNullOrEmpty(filter.ActivoInd.Value.ToString()))
                {
                    query += "and ActivoInd = @ActivoInd ";
                    mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.Bit);
                    mySqlCommand.Parameters["@ActivoInd"].Value = filter.ActivoInd.Value;

                }
                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_glGarantiaControl ctrl = new DTO_glGarantiaControl(dr);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glGarantiaControl_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"> filtro</param>
        /// <returns>Lista </returns>
        public List<DTO_QueryGarantiaControl> DAL_glGarantiaControl_Decisor(int numerodoc)
        {
            try
            {
                List<DTO_QueryGarantiaControl> result = new List<DTO_QueryGarantiaControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;

                //query = "select GarCtrl.garantiaid as GarantiaID," +
                //        "	rtrim(GarCtrl.prefijoid)+'-'+rtrim(GarCtrl.NumeroDoc) as NroDoc," +
                //        "	Matricula as Propietario ," +
                //        "   CodigoGarantia as CodigoGarantia1 ," +
                //        "	case when Gar.GarantiaTipo=1 then placa else Direccion end as Referencia ," +
                //        "	FechaINI as FechaRegistro," +
                //        "	FechaVTO," +
                //        "	VlrFuente as VlrGarantia,RetiraInd" +
                //        " from glGarantiaControl GarCtrl" +
                //        " inner join glGarantia Gar on GarCtrl.GarantiaID=Gar.GarantiaID" +
                //        " where Gar.GarantiaTipo in(1,2) and garctrl.TerceroID=@ClienteID and garctrl.EmpresaID = @EmpresaID ";
                query =
                    "   select NumeroDoc,TipoPersona,ConsGarantia,Version,"+
                    "   GarantiaID," +
                    "   DocGarantia as NroDoc," +
                    "   Propietario," +
                    "   Documento," +
                    "   Referencia," +
                    "   FechaRegistro," +
                    "   FechaVto," +
                    "   FechaRegistro,"+
                    "   InmuebleTipo," +
                    "   VlrGarantia," +
                    "   Consecutivo," +
                    "   CancelaInd" +
                    "   from drSolicitudGarantias"+
                    "   where NumeroDoc=@NumeroDoc";

                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = numerodoc;
                
                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_QueryGarantiaControl ctrl = new DTO_QueryGarantiaControl(dr);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glGarantiaControl_Decisor");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Identificador del comprobante</param>
        /// <param name="compNro">Numeor de comprobante</param>
        /// <param name="estado">Estado del comprobante</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna </returns>
        public List<DTO_glGarantiaControl> DAL_glGarantiaControl_GetByConsecutivo(int consecutivo)
        {
            try
            {
                List<DTO_glGarantiaControl> result = new List<DTO_glGarantiaControl>();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT * from glGarantiaControl with(nolock) WHERE Consecutivo = @Consecutivo";
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_glGarantiaControl ctrl = new DTO_glGarantiaControl(dr);
                    result.Add(ctrl);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glGarantiaControl_GetByID");
                throw exception;
            }
        }

        #endregion
    }
}
