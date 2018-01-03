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
using NewAge.DTO.Negocio.Documentos.Activos;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_DocumentOps
    /// </summary>
    public class DAL_acActivoControl : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_acActivoControl(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae los activos fijos para la Empresa actual
        /// </summary>
        /// <returns></returns>
        public List<DTO_acActivoControl> DAL_acActivoControl_Get()
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from acActivoControl with(nolock) where EmpresaID = @EmpresaID and DocumentoID = @DocumentoID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.AltaActivos;

                List<DTO_acActivoControl> res = new List<DTO_acActivoControl>();
                DTO_acActivoControl act = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    act = new DTO_acActivoControl(dr);
                    res.Add(act);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un activo control por segun la llave primaria
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_acActivoControl DAL_acActivoControl_GetByID(int activoID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from acActivoControl with(nolock) where ActivoID = @ActivoID";

                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters["@ActivoID"].Value = activoID;

                DTO_acActivoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_acActivoControl(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un registro al control de documentos
        /// </summary>
        public int DAL_acActivoControl_Add(DTO_acActivoControl acCtrl)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                           "INSERT INTO [acActivoControl]" +
                                    "   ([EmpresaID]    " +
                                    "   ,[PlaquetaID]   " +
                                    "   ,[ActivoPadreID]   " +
                                    "   ,[RecibidoID]   " +
                                    "   ,[Tipo]   " +
                                    "   ,[FechaImportacion]   " +
                                    "   ,[FechaVencimiento]   " +
                                    "   ,[FechaCapitalizacion]   " +
                                    "   ,[DocumentoID]   " +
                                    "   ,[Fecha]   " +
                                    "   ,[Periodo]   " +
                                    "   ,[CodigoBSID]   " +
                                    "   ,[inReferenciaID]   " +
                                    "   ,[EstadoInv]   " +
                                    "   ,[ActivoGrupoID]   " +
                                    "   ,[ActivoClaseID]   " +
                                    "   ,[Modelo]   " +
                                    "   ,[TipoDepreLOC]   " +
                                    "   ,[AjustexInflacionInd]   " +
                                    "   ,[NumeroDocCompra]   " +
                                    "   ,[MonedaID]   " +
                                    "   ,[TerceroID]   " +
                                    "   ,[DocumentoTercero]   " +
                                    "   ,[ProyectoID]   " +
                                    "   ,[CentroCostoID]   " +
                                    "   ,[LocFisicaID]   " +
                                    "   ,[Responsable]   " +
                                    "   ,[Observacion]   " +
                                    "   ,[EstadoActID]   " +
                                    "   ,[DocumentoAnula]   " +
                                    "   ,[VidaUtilUSG]   " +
                                    "   ,[TipoDepreUSG]   " +
                                    "   ,[ValorSalvamentoLOC]   " +
                                    "   ,[ValorSalvamentoUSG]   " +
                                    "   ,[ValorSalvamentoIFRS]   " +
                                    "   ,[DatoAdd1]   " +
                                    "   ,[DatoAdd2]   " +
                                    "   ,[DatoAdd3]   " +
                                    "   ,[DatoAdd4]   " +
                                    "   ,[DatoAdd5]   " +
                                    "   ,[eg_prBienServicio]   " +
                                    "   ,[eg_inReferencia]   " +
                                    "   ,[eg_acGrupo]   " +
                                    "   ,[eg_acClase]   " +
                                    "   ,[eg_coTercero]   " +
                                    "   ,[eg_coProyecto]   " +
                                    "   ,[eg_coCentroCosto]   " +
                                    "   ,[eg_glLocFisica]   " +
                                    "   ,[eg_acEstado]   " +
                                    "   ,[ActivoTipoID]   " +
                                    "   ,[eg_acTipo]   " +
                                    "   ,[SerialID]   " +
                                    "   ,[Propietario]   " +
                                    "   ,[BodegaID]   " +
                                    "   ,[VidaUtilLOC]   " +
                                    "   ,[VidaUtilIFRS]   " +
                                    "   ,[TipoDepreIFRS]   " +
                                    "   ,[ValorSalvamentoIFRSUS]   " +
                                    "   ,[ValorRetiroIFRS]   " +
                                    "   ,[NumeroDocUltMvto]   " +
                                    "   ,[Turnos])   " +
                                    "     VALUES    " +
                                    "   ( @EmpresaID  " +
                                    "   ,@PlaquetaID  " +
                                    "   ,@ActivoPadreID  " +
                                    "   ,@RecibidoID  " +
                                    "   ,@Tipo  " +
                                    "   ,@FechaImportacion  " +
                                    "   ,@FechaVencimiento  " +
                                    "   ,@FechaCapitalizacion  " +
                                    "   ,@DocumentoID  " +
                                    "   ,@Fecha  " +
                                    "   ,@Periodo  " +
                                    "   ,@CodigoBSID  " +
                                    "   ,@inReferenciaID  " +
                                    "   ,@EstadoInv  " +
                                    "   ,@ActivoGrupoID  " +
                                    "   ,@ActivoClaseID  " +
                                    "   ,@Modelo  " +
                                    "   ,@TipoDepreLOC  " +
                                    "   ,@AjustexInflacionInd  " +
                                    "   ,@NumeroDocCompra  " +
                                    "   ,@MonedaID  " +
                                    "   ,@TerceroID  " +
                                    "   ,@DocumentoTercero  " +
                                    "   ,@ProyectoID  " +
                                    "   ,@CentroCostoID  " +
                                    "   ,@LocFisicaID  " +
                                    "   ,@Responsable  " +
                                    "   ,@Observacion  " +
                                    "   ,@EstadoActID  " +
                                    "   ,@DocumentoAnula  " +
                                    "   ,@VidaUtilUSG  " +
                                    "   ,@TipoDepreUSG  " +
                                    "   ,@ValorSalvamentoLOC  " +
                                    "   ,@ValorSalvamentoUSG  " +
                                    "   ,@ValorSalvamentoIFRS  " +
                                    "   ,@DatoAdd1  " +
                                    "   ,@DatoAdd2  " +
                                    "   ,@DatoAdd3  " +
                                    "   ,@DatoAdd4  " +
                                    "   ,@DatoAdd5  " +
                                    "   ,@eg_prBienServicio  " +
                                    "   ,@eg_inReferencia  " +
                                    "   ,@eg_acGrupo  " +
                                    "   ,@eg_acClase  " +
                                    "   ,@eg_coTercero  " +
                                    "   ,@eg_coProyecto  " +
                                    "   ,@eg_coCentroCosto  " +
                                    "   ,@eg_glLocFisica  " +
                                    "   ,@eg_acEstado  " +
                                    "   ,@ActivoTipoID  " +
                                    "   ,@eg_acTipo  " +
                                    "   ,@SerialID  " +
                                    "   ,@Propietario  " +
                                    "   ,@BodegaID  " +
                                    "   ,@VidaUtilLOC  " +
                                    "   ,@VidaUtilIFRS  " +
                                    "   ,@TipoDepreIFRS  " +
                                    "   ,@ValorSalvamentoIFRSUS  " +
                                    "   ,@ValorRetiroIFRS  " +
                                    "   ,@NumeroDocUltMvto" +
                                    "   ,@Turnos)  " +
                                    " SET @ActivoID = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PlaquetaID", SqlDbType.Char, UDT_PlaquetaID.MaxLength);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoPadreID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RecibidoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Tipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechaImportacion", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaVencimiento", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaCapitalizacion", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActivoGrupoID", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@ActivoClaseID", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@ActivoTipoID", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@Modelo", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@VidaUtilLOC", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@VidaUtilIFRS", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoDepreLOC", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TipoDepreIFRS", SqlDbType.TinyInt);
                //mySqlCommand.Parameters.Add("@PorcSalvamentoLOC", SqlDbType.Decimal);
                //mySqlCommand.Parameters.Add("@PorcSalvamentoIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Turnos", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@AjustexInflacionind", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@NumeroDocCompra", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, 3);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@LocFisicaID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@Responsable", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Propietario", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.VarChar, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoActID", SqlDbType.Char, UDT_EstadoActID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoAnula", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@VidaUtilUSG", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoDepreUSG", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorSalvamentoLOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorSalvamentoUSG", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorSalvamentoIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorSalvamentoIFRSUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorRetiroIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acGrupo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acClase", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLocFisica", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acEstado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDocUltMvto", SqlDbType.Int);
                mySqlCommand.Parameters["@ActivoID"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de Valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PlaquetaID"].Value = acCtrl.PlaquetaID.Value;
                mySqlCommand.Parameters["@SerialID"].Value = acCtrl.SerialID.Value;
                mySqlCommand.Parameters["@ActivoPadreID"].Value = acCtrl.ActivoPadreID.Value;
                mySqlCommand.Parameters["@RecibidoID"].Value = acCtrl.ConsecutivoDetaID.Value; //INT
                mySqlCommand.Parameters["@Tipo"].Value = acCtrl.Tipo.Value; // TINYINT
                mySqlCommand.Parameters["@FechaImportacion"].Value = acCtrl.FechaImportacion.Value;
                mySqlCommand.Parameters["@FechaVencimiento"].Value = acCtrl.FechaVencimiento.Value;
                mySqlCommand.Parameters["@FechaCapitalizacion"].Value = acCtrl.FechaCapitalizacion.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = acCtrl.DocumentoID.Value; //INT
                mySqlCommand.Parameters["@Fecha"].Value = acCtrl.Fecha.Value;
                mySqlCommand.Parameters["@Periodo"].Value = acCtrl.Periodo.Value;
                mySqlCommand.Parameters["@CodigoBSID"].Value = acCtrl.CodigoBSID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = acCtrl.inReferenciaID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = acCtrl.EstadoInv.Value; // TINYINT
                mySqlCommand.Parameters["@ActivoGrupoID"].Value = acCtrl.ActivoGrupoID.Value;
                mySqlCommand.Parameters["@ActivoClaseID"].Value = acCtrl.ActivoClaseID.Value;
                mySqlCommand.Parameters["@ActivoTipoID"].Value = acCtrl.ActivoTipoID.Value;
                mySqlCommand.Parameters["@Modelo"].Value = acCtrl.Modelo.Value;
                mySqlCommand.Parameters["@VidaUtilLOC"].Value = acCtrl.VidaUtilLOC.Value;
                mySqlCommand.Parameters["@VidaUtilIFRS"].Value = acCtrl.VidaUtilIFRS.Value;
                mySqlCommand.Parameters["@TipoDepreLOC"].Value = acCtrl.TipoDepreLOC.Value;
                mySqlCommand.Parameters["@TipoDepreIFRS"].Value = acCtrl.TipoDepreIFRS.Value;
                mySqlCommand.Parameters["@Turnos"].Value = acCtrl.Turnos.Value;
                mySqlCommand.Parameters["@AjustexInflacionind"].Value = acCtrl.AjustexInflacionInd.Value; // TINYINT
                mySqlCommand.Parameters["@NumeroDocCompra"].Value = acCtrl.NumeroDocCompra.Value; // INT
                mySqlCommand.Parameters["@MonedaID"].Value = acCtrl.MonedaID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = acCtrl.TerceroID.Value;
                mySqlCommand.Parameters["@DocumentoTercero"].Value = acCtrl.DocumentoTercero.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = acCtrl.ProyectoID.Value;
                mySqlCommand.Parameters["@CentroCostoID"].Value = acCtrl.CentroCostoID.Value;
                mySqlCommand.Parameters["@LocFisicaID"].Value = acCtrl.LocFisicaID.Value;
                mySqlCommand.Parameters["@Responsable"].Value = acCtrl.Responsable.Value;
                mySqlCommand.Parameters["@Propietario"].Value = acCtrl.Propietario.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = acCtrl.BodegaID.Value;
                mySqlCommand.Parameters["@Observacion"].Value = acCtrl.Observacion.Value;
                mySqlCommand.Parameters["@EstadoActID"].Value = acCtrl.EstadoActID.Value;
                mySqlCommand.Parameters["@DocumentoAnula"].Value = acCtrl.DocumentoAnula.Value; // INT
                mySqlCommand.Parameters["@VidaUtilUSG"].Value = acCtrl.VidaUtilUSG.Value;
                mySqlCommand.Parameters["@TipoDepreUSG"].Value = acCtrl.TipoDepreUSG.Value;
                mySqlCommand.Parameters["@ValorSalvamentoLOC"].Value = acCtrl.ValorSalvamentoLOC.Value;
                mySqlCommand.Parameters["@ValorSalvamentoUSG"].Value = acCtrl.ValorSalvamentoUSG.Value;
                mySqlCommand.Parameters["@ValorSalvamentoIFRS"].Value = acCtrl.ValorSalvamentoIFRS.Value;
                mySqlCommand.Parameters["@ValorSalvamentoIFRSUS"].Value = acCtrl.ValorSalvamentoIFRSUS.Value;
                mySqlCommand.Parameters["@ValorRetiroIFRS"].Value = acCtrl.ValorRetiroIFRS.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = acCtrl.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = acCtrl.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = acCtrl.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = acCtrl.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = acCtrl.DatoAdd5.Value;
                mySqlCommand.Parameters["@NumeroDocUltMvto"].Value = acCtrl.NumeroDocUltMvto.Value;
                mySqlCommand.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acGrupo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acClase"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acClase, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glLocFisica"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLocFisica, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acEstado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acEstado, this.Empresa, egCtrl);
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
                int actId = Convert.ToInt32(mySqlCommand.Parameters["@ActivoID"].Value);

                return actId;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro al control de documentos
        /// </summary>
        public void DAL_acActivoControl_Update(DTO_acActivoControl acCtrl, int activoID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                       " UPDATE acActivoControl SET     " +
                       " [PlaquetaID] = @PlaquetaID    " +
                       " ,[ActivoPadreID] = @ActivoPadreID  " +
                       " ,[RecibidoID] = @RecibidoID    " +
                       " ,[Tipo] = @Tipo    " +
                       " ,[FechaImportacion] = @FechaImportacion    " +
                       " ,[FechaVencimiento] = @FechaVencimiento    " +
                       " ,[FechaCapitalizacion] = @FechaCapitalizacion  " +
                       " ,[DocumentoID] = @DocumentoID  " +
                       " ,[Fecha] = @Fecha  " +
                       " ,[Periodo] = @Periodo  " +
                       " ,[CodigoBSID] = @CodigoBSID    " +
                       " ,[inReferenciaID] = @inReferenciaID     " +
                       " ,[EstadoInv] = @EstadoInv  " +
                       " ,[ActivoGrupoID] = @ActivoGrupoID  " +
                       " ,[ActivoClaseID] = @ActivoClaseID   " +
                       " ,[Modelo] = @Modelo     " +
                       " ,[TipoDepreLOC] = @TipoDepreLOC     " +
                       " ,[AjustexInflacionInd] = @AjustexInflacionInd  " +
                       " ,[NumeroDocCompra] = @NumeroDocCompra  " +
                       " ,[MonedaID] = @MonedaID    " +
                       " ,[TerceroID] = @TerceroID  " +
                       " ,[DocumentoTercero] = @DocumentoTercero     " +
                       " ,[ProyectoID] = @ProyectoID     " +
                       " ,[CentroCostoID] = @CentroCostoID   " +
                       " ,[LocFisicaID] = @LocFisicaID   " +
                       " ,[Responsable] = @Responsable  " +
                       " ,[Observacion] = @Observacion   " +
                       " ,[EstadoActID] = @EstadoActID   " +
                       " ,[DocumentoAnula] = @DocumentoAnula    " +
                       " ,[DatoAdd1] = @DatoAdd1     " +
                       " ,[DatoAdd2] = @DatoAdd2     " +
                       " ,[DatoAdd3] = @DatoAdd3     " +
                       " ,[DatoAdd4] = @DatoAdd4     " +
                       " ,[DatoAdd5] = @DatoAdd5    " +
                       " ,[ActivoTipoID] = @ActivoTipoID    " +
                       " ,[SerialID] = @SerialID    " +
                       " ,[Propietario] = @Propietario  " +
                       " ,[BodegaID] = @BodegaID    " +
                       " ,[VidaUtilLOC] = @VidaUtilLOC  " +
                       " ,[VidaUtilIFRS] = @VidaUtilIFRS     " +
                       " ,[TipoDepreIFRS] = @TipoDepreIFRS   " +
                       " ,[Turnos] = @Turnos    " +
                       " ,[VidaUtilUSG] = @VidaUtilUSG  " +
                       " ,[TipoDepreUSG] = @TipoDepreUSG    " +
                       " ,[ValorSalvamentoLOC] = @ValorSalvamentoLOC    " +
                       " ,[ValorSalvamentoUSG] = @ValorSalvamentoUSG    " +
                       " ,[ValorSalvamentoIFRS] = @ValorSalvamentoIFRS  " +
                       " ,[ValorSalvamentoIFRSUS] = @ValorSalvamentoIFRSUS   " +
                       " ,[ValorRetiroIFRS] = @ValorRetiroIFRS   " +
                       " ,[NumeroDocUltMvto] = @NumeroDocUltMvto    " +
                       " WHERE EmpresaID = @EmpresaID AND ActivoID = @ActivoID";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PlaquetaID", SqlDbType.Char, UDT_PlaquetaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoPadreID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RecibidoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Tipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechaImportacion", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaVencimiento", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaCapitalizacion", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActivoGrupoID", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@ActivoClaseID", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@Modelo", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@TipoDepreLOC", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@AjustexInflacionind", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@NumeroDocCompra", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, 3);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@LocFisicaID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@Responsable", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.VarChar, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoActID", SqlDbType.Char, UDT_EstadoActID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoAnula", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@ActivoTipoID", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@Propietario", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@VidaUtilLOC", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@VidaUtilIFRS", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoDepreIFRS", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Turnos", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@VidaUtilUSG", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoDepreUSG", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorSalvamentoLOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorSalvamentoUSG", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorSalvamentoIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorSalvamentoIFRSUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorRetiroIFRS", SqlDbType.Decimal);    
                mySqlCommand.Parameters.Add("@NumeroDocUltMvto", SqlDbType.Int);               
     

                #endregion
                #region Asignacion de Valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = acCtrl.ActivoID.Value;
                mySqlCommand.Parameters["@PlaquetaID"].Value = acCtrl.PlaquetaID.Value;
                mySqlCommand.Parameters["@ActivoPadreID"].Value = acCtrl.ActivoPadreID.Value;
                mySqlCommand.Parameters["@RecibidoID"].Value = acCtrl.ConsecutivoDetaID.Value;
                mySqlCommand.Parameters["@Tipo"].Value = acCtrl.Tipo.Value;
                mySqlCommand.Parameters["@FechaImportacion"].Value = acCtrl.FechaImportacion.Value;
                mySqlCommand.Parameters["@FechaVencimiento"].Value = acCtrl.FechaVencimiento.Value;
                mySqlCommand.Parameters["@FechaCapitalizacion"].Value = acCtrl.FechaCapitalizacion.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = acCtrl.DocumentoID.Value;
                mySqlCommand.Parameters["@Fecha"].Value = acCtrl.Fecha.Value;
                mySqlCommand.Parameters["@Periodo"].Value = acCtrl.Periodo.Value;
                mySqlCommand.Parameters["@CodigoBSID"].Value = acCtrl.CodigoBSID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = acCtrl.inReferenciaID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = acCtrl.EstadoInv.Value;
                mySqlCommand.Parameters["@ActivoGrupoID"].Value = acCtrl.ActivoGrupoID.Value;
                mySqlCommand.Parameters["@ActivoClaseID"].Value = acCtrl.ActivoClaseID.Value;
                mySqlCommand.Parameters["@Modelo"].Value = acCtrl.Modelo.Value;
                mySqlCommand.Parameters["@TipoDepreLOC"].Value = acCtrl.TipoDepreLOC.Value;
                mySqlCommand.Parameters["@AjustexInflacionind"].Value = acCtrl.AjustexInflacionInd.Value;
                mySqlCommand.Parameters["@NumeroDocCompra"].Value = acCtrl.NumeroDocCompra.Value;
                mySqlCommand.Parameters["@MonedaID"].Value = acCtrl.MonedaID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = acCtrl.TerceroID.Value;
                mySqlCommand.Parameters["@DocumentoTercero"].Value = acCtrl.DocumentoTercero.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = acCtrl.ProyectoID.Value;
                mySqlCommand.Parameters["@CentroCostoID"].Value = acCtrl.CentroCostoID.Value;
                mySqlCommand.Parameters["@LocFisicaID"].Value = acCtrl.LocFisicaID.Value;
                mySqlCommand.Parameters["@Responsable"].Value = acCtrl.Responsable.Value;
                mySqlCommand.Parameters["@Observacion"].Value = acCtrl.Observacion.Value;
                mySqlCommand.Parameters["@EstadoActID"].Value = acCtrl.EstadoActID.Value;
                mySqlCommand.Parameters["@DocumentoAnula"].Value = acCtrl.DocumentoAnula.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = acCtrl.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = acCtrl.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = acCtrl.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = acCtrl.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = acCtrl.DatoAdd5.Value;
                mySqlCommand.Parameters["@ActivoTipoID"].Value = acCtrl.ActivoTipoID.Value;
                mySqlCommand.Parameters["@SerialID"].Value = acCtrl.SerialID.Value;
                mySqlCommand.Parameters["@Propietario"].Value = acCtrl.Propietario.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = acCtrl.BodegaID.Value;
                mySqlCommand.Parameters["@VidaUtilLOC"].Value = acCtrl.VidaUtilLOC.Value;
                mySqlCommand.Parameters["@VidaUtilIFRS"].Value = acCtrl.VidaUtilIFRS.Value;
                mySqlCommand.Parameters["@TipoDepreIFRS"].Value = acCtrl.TipoDepreIFRS.Value;
                mySqlCommand.Parameters["@Turnos"].Value = acCtrl.Turnos.Value;
                mySqlCommand.Parameters["@VidaUtilUSG"].Value = acCtrl.VidaUtilUSG.Value;
                mySqlCommand.Parameters["@TipoDepreUSG"].Value = acCtrl.TipoDepreUSG.Value;
                mySqlCommand.Parameters["@ValorSalvamentoLOC"].Value = acCtrl.ValorSalvamentoLOC.Value;
                mySqlCommand.Parameters["@ValorSalvamentoUSG"].Value = acCtrl.ValorSalvamentoUSG.Value;
                mySqlCommand.Parameters["@ValorSalvamentoIFRS"].Value = acCtrl.ValorSalvamentoIFRS.Value;
                mySqlCommand.Parameters["@ValorSalvamentoIFRSUS"].Value = acCtrl.ValorSalvamentoIFRSUS.Value;
                mySqlCommand.Parameters["@ValorRetiroIFRS"].Value = acCtrl.ValorRetiroIFRS.Value;               
                mySqlCommand.Parameters["@NumeroDocUltMvto"].Value = acCtrl.NumeroDocUltMvto.Value;
              
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
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                dr.Close();
                //int actId = Convert.ToInt32(mySqlCommand.Parameters["@ActivoID"].Value);

                //return actId;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Cambia el Estado del activo 
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <param name="estadoInv">estado del activo</param>
        public void DAL_acActivoControl_ChangeStatus(int activoID, byte estadoInv)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                       " UPDATE acActivoControl SET     " +
                       "    [EstadoInv] = @EstadoInv  " +                    
                       "    WHERE EmpresaID = @EmpresaID AND ActivoID = @ActivoID";
                #endregion

                #region Creacion de parametros
                
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
              
                #endregion

                #region Asignacion de Valores
                
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = activoID;
                mySqlCommand.Parameters["@EstadoInv"].Value = estadoInv;               

                #endregion               

                mySqlCommand.ExecuteNonQuery();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                dr.Close();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_ChangeStatus");
                throw exception;
            }
        }

        #endregion

        #region Otras (Get)

        /// <summary>
        /// Trae un activo control
        /// </summary>
        /// <param name="plaqueta">Plaqueta</param>
        /// <returns>Retorna un activo</returns>
        public DTO_acActivoControl DAL_acActivoControl_GetByPlaqueta(string plaquetaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from acActivoControl with(nolock) where PlaquetaID = @PlaquetaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PlaquetaID", SqlDbType.Char, UDT_PlaquetaID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PlaquetaID"].Value = plaquetaID;

                DTO_acActivoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_acActivoControl(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_GetByPlaqueta");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un activo control
        /// </summary>
        /// <param name="plaqueta">Serial</param>
        /// <returns>Retorna un activo</returns>
        public DTO_acActivoControl DAL_acActivoControl_GetBySerial(string serialID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from acActivoControl with(nolock) where SerialID = @SerialID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_PlaquetaID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@SerialID"].Value = serialID;

                DTO_acActivoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_acActivoControl(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_GetBySerial");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de activos por el numero de documento que genero el alta
        /// </summary>
        /// <param name="numDoc">número documento</param>
        /// <returns></returns>
        public List<DTO_acActivoControl> DAL_acActivoControl_GetByDocument(int numDoc)
        {   
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from acActivoControl with(nolock) where NumeroDocCompra = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PlaquetaID", SqlDbType.Char, UDT_PlaquetaID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = numDoc;

                List<DTO_acActivoControl> result = new List<DTO_acActivoControl>();
                DTO_acActivoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    res = new DTO_acActivoControl(dr);
                    result.Add(res);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_GetByDocument");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro siempre y cuando la plaqueta sea null.
        /// </summary>
        /// <param name="acCtrl">dto_AcControl</param>
        /// <returns></returns>
        public List<DTO_acActivoControl> DAL_acActivoControl_GetByParameter(DTO_acActivoControl acCtrl)
        {
            try
            {
                List<DTO_acActivoControl> result = new List<DTO_acActivoControl>();

                string where = "";
                string and = " and ";
                bool add = false;
                string complete = "";

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Tercero
                if (!string.IsNullOrEmpty(acCtrl.TerceroID.Value))
                {
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char);
                    mySqlCommand.Parameters["@TerceroID"].Value = acCtrl.TerceroID.Value;
                    where = "WHERE TerceroID = @TerceroID";
                    add = true;
                }
                #endregion
                #region Plaqueta

                if (!string.IsNullOrEmpty(acCtrl.PlaquetaID.Value))
                {
                    mySqlCommand.Parameters.Add("@PlaquetaID", SqlDbType.Char, UDT_PlaquetaID.MaxLength);
                    mySqlCommand.Parameters["@PlaquetaID"].Value = acCtrl.PlaquetaID.Value;
                    if (add)
                    {
                        complete += complete = " and PlaquetaID = @PlaquetaID";
                    }
                    else
                    {
                        where = "WHERE PlaquetaID = @PlaquetaID";
                        add = true;
                    }
                }
                #endregion
                #region Serial
                if (!string.IsNullOrEmpty(acCtrl.SerialID.Value))
                {
                    mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char);
                    mySqlCommand.Parameters["@SerialID"].Value = acCtrl.SerialID.Value;
                    if (add)
                    {
                        complete += complete = " and SerialID = @SerialID";
                    }
                    else
                    {
                        where = "WHERE SerialID = @SerialID";
                        add = true;
                    }
                }
                #endregion
                #region Documento tercero
                if (!string.IsNullOrEmpty(acCtrl.DocumentoTercero.Value))
                {
                    mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char);
                    mySqlCommand.Parameters["@DocumentoTercero"].Value = acCtrl.DocumentoTercero.Value;
                    if (add)
                    {
                        complete += complete = " and DocumentoTercero = @DocumentoTercero";
                    }
                    else
                    {
                        where = "WHERE DocumentoTercero = @DocumentoTercero";
                        add = true;
                    }
                }
                #endregion
                #region referencia
                if (!string.IsNullOrEmpty(acCtrl.inReferenciaID.Value))
                {
                    mySqlCommand.Parameters.Add("@Referencia", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@Referencia"].Value = acCtrl.inReferenciaID.Value;
                    if (add)
                    {
                        complete += complete = " and inReferenciaID = @Referencia";
                    }
                    else
                    {
                        where = "WHERE inReferenciaID = @Referencia";
                        add = true;
                    }
                }
                #endregion
                #region LocFisica
                if (!string.IsNullOrEmpty(acCtrl.LocFisicaID.Value))
                {
                    mySqlCommand.Parameters.Add("@LocFisicaID", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                    mySqlCommand.Parameters["@LocFisicaID"].Value = acCtrl.LocFisicaID.Value;
                    if (add)
                    {
                        complete += complete = " and LocFisicaID = @LocFisicaID";
                    }
                    else
                    {
                        where = "WHERE LocFisicaID = @LocFisicaID";
                        add = true;
                    }
                }
                #endregion
                #region ActivoClaseID
                if (!string.IsNullOrEmpty(acCtrl.ActivoClaseID.Value))
                {
                    mySqlCommand.Parameters.Add("@ActivoClaseID", SqlDbType.Char, UDT_ActivoClaseID.MaxLength);
                    mySqlCommand.Parameters["@ActivoClaseID"].Value = acCtrl.ActivoClaseID.Value;
                    if (add)
                    {
                        complete += complete = " and ActivoClaseID = @ActivoClaseID";
                    }
                    else
                    {
                        where = "WHERE ActivoClaseID = @ActivoClaseID";
                        add = true;
                    }
                }
                #endregion
                #region ActivoTipoID
                if (!string.IsNullOrEmpty(acCtrl.ActivoTipoID.Value))
                {
                    mySqlCommand.Parameters.Add("@ActivoTipoID", SqlDbType.Char, UDT_ActivoTipoID.MaxLength);
                    mySqlCommand.Parameters["@ActivoTipoID"].Value = acCtrl.ActivoTipoID.Value;
                    if (add)
                    {
                        complete += complete = " and ActivoTipoID = @ActivoTipoID";
                    }
                    else
                    {
                        where = "WHERE ActivoTipoID = @ActivoTipoID";
                        add = true;
                    }
                }

                #endregion
                #region ActivoGrupoID
                if (!string.IsNullOrEmpty(acCtrl.ActivoGrupoID.Value))
                {
                    mySqlCommand.Parameters.Add("@ActivoGrupoID", SqlDbType.Char, UDT_ActivoGrupoID.MaxLength);
                    mySqlCommand.Parameters["@ActivoGrupoID"].Value = acCtrl.ActivoGrupoID.Value;
                    if (add)
                    {
                        complete += complete = " and ActivoGrupoID = @ActivoGrupoID";
                    }
                    else
                    {
                        where = "WHERE ActivoGrupoID = @ActivoGrupoID";
                        add = true;
                    }
                }
                #endregion
                #region Responsable
                if (!string.IsNullOrEmpty(acCtrl.Responsable.Value))
                {
                    mySqlCommand.Parameters.Add("@Responsable", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@Responsable"].Value = acCtrl.Responsable.Value;
                    if (add)
                    {
                        complete += complete = " and Responsable = @Responsable";
                    }
                    else
                    {
                        where = "WHERE Responsable = @Responsable";
                        add = true;
                    }
                }

                #endregion
                #region Proyecto
                if (!string.IsNullOrEmpty(acCtrl.ProyectoID.Value))
                {
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = acCtrl.ProyectoID.Value;
                    if (add)
                    {
                        complete += complete = " and ProyectoID = @ProyectoID";
                    }
                    else
                    {
                        where = "WHERE ProyectoID = @ProyectoID";
                        add = true;
                    }
                }
                #endregion
                #region CentroCostoID
                if (!string.IsNullOrEmpty(acCtrl.CentroCostoID.Value))
                {
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = acCtrl.CentroCostoID.Value;
                    if (add)
                    {
                        complete += complete = " and CentroCostoID = @CentroCostoID";
                    }
                    else
                    {
                        where = "WHERE CentroCostoID = @CentroCostoID";
                        add = true;
                    }
                }
                #endregion

                if (add)
                    mySqlCommand.CommandText = "select * from acActivoControl with(nolock)" + where + complete + " and PlaquetaID is null";
                else
                    mySqlCommand.CommandText = "select * from acActivoControl with(nolock) " + where + " and PlaquetaID is null";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_acActivoControl activo = new DTO_acActivoControl(dr);
                    result.Add(activo);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro para la pantalla traslado
        /// </summary>
        /// <param name="acCtrl"></param>
        /// <returns></returns>
        public List<DTO_acActivoControl> DAL_acActivoControl_GetByParameterForTranfer(DTO_acActivoControl acCtrl)
        {
            try
            {
                List<DTO_acActivoControl> result = new List<DTO_acActivoControl>();

                string where = "";
                string and = " and ";
                bool add = false;
                string complete = "";

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Tercero
                if (!string.IsNullOrEmpty(acCtrl.TerceroID.Value))
                {
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char);
                    mySqlCommand.Parameters["@TerceroID"].Value = acCtrl.TerceroID.Value;
                    where = "WHERE TerceroID = @TerceroID";
                    add = true;
                }
                #endregion
                #region Plaqueta

                if (!string.IsNullOrEmpty(acCtrl.PlaquetaID.Value))
                {
                    mySqlCommand.Parameters.Add("@PlaquetaID", SqlDbType.Char, UDT_PlaquetaID.MaxLength);
                    mySqlCommand.Parameters["@PlaquetaID"].Value = acCtrl.PlaquetaID.Value;
                    if (add)
                    {
                        complete += complete = " and PlaquetaID = @PlaquetaID";
                    }
                    else
                    {
                        where = "WHERE PlaquetaID = @PlaquetaID";
                        add = true;
                    }
                }
                #endregion
                #region Serial
                if (!string.IsNullOrEmpty(acCtrl.SerialID.Value))
                {
                    mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char);
                    mySqlCommand.Parameters["@SerialID"].Value = acCtrl.SerialID.Value;
                    if (add)
                    {
                        complete += complete = " and SerialID = @SerialID";
                    }
                    else
                    {
                        where = "WHERE SerialID = @SerialID";
                        add = true;
                    }
                }
                #endregion
                #region Documento tercero
                if (!string.IsNullOrEmpty(acCtrl.DocumentoTercero.Value))
                {
                    mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char);
                    mySqlCommand.Parameters["@DocumentoTercero"].Value = acCtrl.DocumentoTercero.Value;
                    if (add)
                    {
                        complete += complete = " and DocumentoTercero = @DocumentoTercero";
                    }
                    else
                    {
                        where = "WHERE DocumentoTercero = @DocumentoTercero";
                        add = true;
                    }
                }
                #endregion
                #region Referencia
                if (!string.IsNullOrEmpty(acCtrl.inReferenciaID.Value))
                {
                    mySqlCommand.Parameters.Add("@Referencia", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@Referencia"].Value = acCtrl.inReferenciaID.Value;
                    if (add)
                    {
                        complete += complete = " and inReferenciaID = @Referencia";
                    }
                    else
                    {
                        where = "WHERE inReferenciaID = @Referencia";
                        add = true;
                    }
                }
                #endregion
                #region Bodega
                if (!string.IsNullOrEmpty(acCtrl.BodegaID.Value))
                {
                    mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaID"].Value = acCtrl.BodegaID.Value;
                    if (add)
                    {
                        complete += complete = " and BodegaID = @BodegaID";
                    }
                    else
                    {
                        where = "WHERE BodegaID = @BodegaID";
                        add = true;
                    }
                }
                #endregion
                #region LocFisica
                if (!string.IsNullOrEmpty(acCtrl.LocFisicaID.Value))
                {
                    mySqlCommand.Parameters.Add("@LocFisicaID", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                    mySqlCommand.Parameters["@LocFisicaID"].Value = acCtrl.LocFisicaID.Value;
                    if (add)
                    {
                        complete += complete = " and LocFisicaID = @LocFisicaID";
                    }
                    else
                    {
                        where = "WHERE LocFisicaID = @LocFisicaID";
                        add = true;
                    }
                }
                #endregion
                #region ActivoClaseID
                if (!string.IsNullOrEmpty(acCtrl.ActivoClaseID.Value))
                {
                    mySqlCommand.Parameters.Add("@ActivoClaseID", SqlDbType.Char, UDT_ActivoClaseID.MaxLength);
                    mySqlCommand.Parameters["@ActivoClaseID"].Value = acCtrl.ActivoClaseID.Value;
                    if (add)
                    {
                        complete += complete = " and ActivoClaseID = @ActivoClaseID";
                    }
                    else
                    {
                        where = "WHERE ActivoClaseID = @ActivoClaseID";
                        add = true;
                    }
                }
                #endregion
                #region ActivoTipoID
                if (!string.IsNullOrEmpty(acCtrl.ActivoTipoID.Value))
                {
                    mySqlCommand.Parameters.Add("@ActivoTipoID", SqlDbType.Char, UDT_ActivoTipoID.MaxLength);
                    mySqlCommand.Parameters["@ActivoTipoID"].Value = acCtrl.ActivoTipoID.Value;
                    if (add)
                    {
                        complete += complete = " and ActivoTipoID = @ActivoTipoID";
                    }
                    else
                    {
                        where = "WHERE ActivoTipoID = @ActivoTipoID";
                        add = true;
                    }
                }

                #endregion
                #region ActivoGrupoID
                if (!string.IsNullOrEmpty(acCtrl.ActivoGrupoID.Value))
                {
                    mySqlCommand.Parameters.Add("@ActivoGrupoID", SqlDbType.Char, UDT_ActivoGrupoID.MaxLength);
                    mySqlCommand.Parameters["@ActivoGrupoID"].Value = acCtrl.ActivoGrupoID.Value;
                    if (add)
                    {
                        complete += complete = " and ActivoGrupoID = @ActivoGrupoID";
                    }
                    else
                    {
                        where = "WHERE ActivoGrupoID = @ActivoGrupoID";
                        add = true;
                    }
                }
                #endregion
                #region Responsable
                if (!string.IsNullOrEmpty(acCtrl.Responsable.Value))
                {
                    mySqlCommand.Parameters.Add("@Responsable", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@Responsable"].Value = acCtrl.Responsable.Value;
                    if (add)
                    {
                        complete += complete = " and Responsable = @Responsable";
                    }
                    else
                    {
                        where = "WHERE Responsable = @Responsable";
                        add = true;
                    }
                }

                #endregion
                #region Proyecto
                if (!string.IsNullOrEmpty(acCtrl.ProyectoID.Value))
                {
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = acCtrl.ProyectoID.Value;
                    if (add)
                    {
                        complete += complete = " and ProyectoID = @ProyectoID";
                    }
                    else
                    {
                        where = "WHERE ProyectoID = @ProyectoID";
                        add = true;
                    }
                }
                #endregion
                #region CentroCostoID
                if (!string.IsNullOrEmpty(acCtrl.CentroCostoID.Value))
                {
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = acCtrl.CentroCostoID.Value;
                    if (add)
                    {
                        complete += complete = " and CentroCostoID = @CentroCostoID";
                    }
                    else
                    {
                        where = "WHERE CentroCostoID = @CentroCostoID";
                        add = true;
                    }
                }
                #endregion

                if (add)
                    mySqlCommand.CommandText = "select * from acActivoControl with(nolock)" + where + complete;
                else
                    mySqlCommand.CommandText = "select * from acActivoControl with(nolock)" + where;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_acActivoControl activo = new DTO_acActivoControl(dr);
                    result.Add(activo);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_GetByParameterForTranfer");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene los activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        public List<DTO_acActivoControl> DAL_acActivoControl_GetFilters(string serialID, 
                                                                        string PlaquetaID, 
                                                                        string locFisicaID, 
                                                                        string referenciaID, 
                                                                        string centroCosto,
                                                                        string proyecto,
                                                                        string clase,
                                                                        string tipo,
                                                                        string grupo,
                                                                        string responsable,                
                                                                        bool isContenedor,
                                                                        int pageSize, 
                                                                        int pageNum)
        {
            try
            {

                string querySelect =    "   select temp.* from  "   +
                                        "   (  "   +
                                        "   select  ROW_NUMBER()Over(Order by ActivoID Asc) As RowNum, acActivoControl.*, acTipo.ContenedorInd  " +
                                        "   from acActivoControl with(nolock)     "   +
                                        "   inner join acTipo on acTipo.ActivoTipoID = acActivoControl.ActivoTipoID  and acTipo.EmpresaGrupoID =  @EmpresaID " +
                                        "   where acActivoControl.EmpresaID = @EmpresaID  {0}  "   +
                                        "   ) temp  " +
                                        "     where temp.RowNum BETWEEN (@Pagina - 1) * @RegistrosporPagina + 1 AND @Pagina * @RegistrosporPagina  ";
                
                string queryWhere = string.Empty;

                #region Filtro Serial

                if (!string.IsNullOrEmpty(serialID))
                    queryWhere += " AND  acActivoControl.SerialID = '" + serialID + "'";

                #endregion
                #region Filtro de Plaqueta

                if (!string.IsNullOrEmpty(PlaquetaID))
                    queryWhere += " AND  acActivoControl.PlaquetaID = '" + PlaquetaID + "'";              

                #endregion
                #region Filtro de Localización Fisica
                               
                if (!string.IsNullOrEmpty(locFisicaID))
                    queryWhere += " AND  acActivoControl.LocFisicaID = '" + locFisicaID + "'"; 
      
                #endregion 
                #region Filtro de Referencia

                if (!string.IsNullOrEmpty(referenciaID))
                    queryWhere += " AND  teacActivoControlmp.inReferenciaID = '" + referenciaID + "'";
              
                #endregion
                #region Filtro de Centro Costo

                if (!string.IsNullOrEmpty(centroCosto))
                    queryWhere += " AND  acActivoControl.CentroCostoID = '" + centroCosto + "'";

                #endregion
                #region Filtro de Proyecto

                if (!string.IsNullOrEmpty(proyecto))
                    queryWhere += " AND  acActivoControl.ProyectoID = '" + proyecto + "'";

                #endregion
                #region Filtro de ActivoClase
                
                if (!string.IsNullOrEmpty(clase))
                    queryWhere += " AND  acActivoControl.ActivoClaseID = '" + clase + "'";                

                #endregion
                #region Filtro de ActivoGrupo

                if (!string.IsNullOrEmpty(grupo))
                    queryWhere += " AND  acActivoControl.ActivoGrupoID = '" + grupo + "'";

                #endregion
                #region Filtro de ActivoTipo

                if (!string.IsNullOrEmpty(tipo))
                    queryWhere += " AND  acActivoControl.ActivoTipoID = '" + tipo + "'";
        
                #endregion
                #region Filtro de Responsable

                if (!string.IsNullOrEmpty(responsable))
                    queryWhere += " AND  acActivoControl.Responsable = '" + responsable + "'";

                #endregion
                #region Filtro de Contenedor

                if (isContenedor)
                    queryWhere += " AND  acActivoControl.ContenedorInd = " + Convert.ToInt32(isContenedor).ToString();
                
                #endregion

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = string.Format(querySelect, queryWhere);

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Pagina", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RegistrosporPagina", SqlDbType.Int);               

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Pagina"].Value = pageNum;
                mySqlCommand.Parameters["@RegistrosporPagina"].Value = pageSize;

                List<DTO_acActivoControl> res = new List<DTO_acActivoControl>();
                DTO_acActivoControl act = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    act = new DTO_acActivoControl(dr);
                    res.Add(act);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_GetFilters");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el numero de activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="centroCosto">identificador centro de costo</param>
        /// <param name="proyecto">identificador de proyecto</param>
        /// <param name="clase">identificador de clase</param>
        /// <param name="tipo">identificador de tipo</param>
        /// <param name="grupo">identificador de grupo</param>
        /// <param name="responsable">resposable</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        public int DAL_acActivoControl_GetFiltersCount( string serialID,
                                                        string PlaquetaID,
                                                        string locFisicaID,
                                                        string referenciaID,
                                                        string centroCosto,
                                                        string proyecto,
                                                        string clase,
                                                        string tipo,
                                                        string grupo,
                                                        string responsable,
                                                        bool isContenedor
                                                       )
        {
            try
            {

                string querySelect = "   select count(*) from acActivoControl with(nolock) " +
                                     "   inner join acTipo on acTipo.ActivoTipoID = acActivoControl.ActivoTipoID    " +
                                     "   AND acTipo.EmpresaGrupoID =  @EmpresaID    " +
                                     "   where EmpresaID = @EmpresaID   ";

                string queryWhere = string.Empty;

                #region Filtro Serial

                if (!string.IsNullOrEmpty(serialID))
                    queryWhere += " AND  SerialID = '" + serialID + "'";

                #endregion
                #region Filtro de Plaqueta

                if (!string.IsNullOrEmpty(PlaquetaID))
                    queryWhere += " AND  PlaquetaID = '" + PlaquetaID + "'";

                #endregion
                #region Filtro de Localización Fisica

                if (!string.IsNullOrEmpty(locFisicaID))
                    queryWhere += " AND  LocFisicaID = '" + locFisicaID + "'";

                #endregion
                #region Filtro de Referencia

                if (!string.IsNullOrEmpty(referenciaID))
                    queryWhere += " AND  inReferenciaID = '" + referenciaID + "'";

                #endregion
                #region Filtro de Centro Costo

                if (!string.IsNullOrEmpty(centroCosto))
                    queryWhere += " AND  CentroCostoID = '" + centroCosto + "'";

                #endregion
                #region Filtro de Proyecto

                if (!string.IsNullOrEmpty(proyecto))
                    queryWhere += " AND  ProyectoID = '" + proyecto + "'";

                #endregion
                #region Filtro de ActivoClase

                if (!string.IsNullOrEmpty(clase))
                    queryWhere += " AND  ActivoClaseID = '" + clase + "'";

                #endregion
                #region Filtro de ActivoGrupo

                if (!string.IsNullOrEmpty(grupo))
                    queryWhere += " AND  ActivoGrupoID = '" + grupo + "'";

                #endregion
                #region Filtro de ActivoTipo

                if (!string.IsNullOrEmpty(tipo))
                    queryWhere += " AND  ActivoTipoID = '" + tipo + "'";

                #endregion
                #region Filtro de Responsable

                if (!string.IsNullOrEmpty(responsable))
                    queryWhere += " AND  Responsable = '" + responsable + "'";

                #endregion
                #region Filtro de Contenedor

                if (isContenedor)
                    queryWhere += " AND  acTipo.ContenedorInd = " + Convert.ToInt32(isContenedor).ToString();

                #endregion

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = querySelect + queryWhere;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
     
                int res = (int)mySqlCommand.ExecuteScalar();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_GetFiltersCount");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de activos que tengan referencias con opcion de garantias
        /// </summary>
        /// <param name="numDoc">número documento</param>
        /// <returns></returns>
        public List<DTO_acActivoControl> DAL_acActivoControl_GetForGarantia()
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = " Select (case when tipoAct.TipoAct = 0 then 'P' else 'S' end)  as TipoAct, refer.Descriptivo, act.*" +
                                           " from acActivoControl act with(nolock) " +
                                           " inner join inReferencia refer with(nolock) on refer.inReferenciaID = act.inReferenciaID  and refer.EmpresaGrupoID = act.eg_inReferencia " +
                                           " inner join inRefTipo refTipo with(nolock)  on refTipo.TipoInvID = refer.TipoInvID  and refTipo.EmpresaGrupoID = refer.eg_inRefTipo " +
                                           " inner join acTipo tipoAct with(nolock)  on tipoAct.ActivoTipoID = act.ActivoTipoID  and tipoAct.EmpresaGrupoID = act.eg_acTipo " +
                                           " where act.EmpresaID =@EmpresaID and refTipo.GarantiaInd = 1 ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                List<DTO_acActivoControl> result = new List<DTO_acActivoControl>();
                DTO_acActivoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    res = new DTO_acActivoControl(dr);
                    res.Descriptivo.Value = dr["Descriptivo"].ToString();
                    res.TipoAct.Value = dr["TipoAct"].ToString();
                    result.Add(res);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_GetForGarantia");
                throw exception;
            }
        }

        #endregion

        #region Traslado Activos

        /// <summary>
        /// Actualiza un registro de acuerdo al movimiento
        /// </summary>
        public void DAL_acActivoControl_TrasladoActivos_Update(DTO_acActivoControl acCtrl, int activoID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    " UPDATE acActivoControl SET " +
                         " [PlaquetaID] = @PlaquetaID , " +
                         " [SerialID] = @SerialID, " +
                         " [RecibidoID] = @RecibidoID, " +
                         " [Tipo] = @Tipo, " +
                         " [FechaImportacion] = @FechaImportacion, " +
                         " [FechaVencimiento] = @FechaVencimiento, " +
                         " [FechaCapitalizacion]  = @FechaCapitalizacion, " +
                         " [DocumentoID] = @DocumentoID , " +
                         " [Fecha] = @Fecha, " +
                         " [Periodo] = @Periodo, " +
                         " [CodigoBSID] = @CodigoBSID, " +
                         " [inReferenciaID] = @inReferenciaID, " +
                         " [EstadoInv] = @EstadoInv, " +
                         " [ActivoGrupoID] = @ActivoGrupoID, " +
                         " [ActivoClaseID] = @ActivoClaseID, " +
                         " [ActivoTipoID] = @ActivoTipoID , " +
                         " [Modelo] = @Modelo, " +
                         " [VidaUtilLOC] = @VidaUtilLOC, " +
                         " [VidaUtilIFRS] = VidaUtilIFRS, " +
                         " [TipoDepreLOC] = @TipoDepreLOC, " +
                         " [TipoDepreIFRS] = @TipoDepreIFRS, " +
                    //" [PorcSalvamentoLOC] = @PorcSalvamentoLOC, " +
                    //" [PorcSalvamentoIFRS] = @PorcSalvamentoIFRS, " +
                         " [Turnos] = @Turnos, " +
                         " [AjustexInflacionInd] = @AjustexInflacionInd, " +
                         " [NumeroDocCompra] = @NumeroDocCompra , " +
                         " [MonedaID] = @MonedaID, " +
                         " [TerceroID] =  @TerceroID, " +
                         " [DocumentoTercero] = @DocumentoTercero, " +
                         " [ProyectoID] = @ProyectoID, " +
                         " [CentroCostoID] = @CentroCostoID, " +
                         " [LocFisicaID] = @LocFisicaID, " +
                         " [Responsable] = @Responsable, " +
                         " [Propietario] = @Propietario, " +
                         " [BodegaID] = @BodegaID, " +
                         " [Observacion] = @Observacion, " +
                         " [EstadoActID] = @EstadoActID, " +
                         " [DocumentoAnula] =  @DocumentoAnula, " +
                         " [ValorSalvamentoIFRSUS] =  @ValorSalvamentoIFRSUS, " +
                         " [ValorRetiroIFRS] =  @ValorRetiroIFRS, " +
                         " [DatoAdd1] = @DatoAdd1, " +
                         " [DatoAdd2] = @DatoAdd2, " +
                         " [DatoAdd3] = @DatoAdd3, " +
                         " [DatoAdd4] = @DatoAdd4, " +
                         " [DatoAdd5] = @DatoAdd5 " +
                         " WHERE ActivoID = @ActivoID";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PlaquetaID", SqlDbType.Char, UDT_PlaquetaID.MaxLength);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@RecibidoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Tipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechaImportacion", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaVencimiento", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaCapitalizacion", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActivoGrupoID", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@ActivoClaseID", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@ActivoTipoID", SqlDbType.Int, UDT_ActivoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@Modelo", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@VidaUtilLOC", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@VidaUtilIFRS", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoDepreLOC", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TipoDepreIFRS", SqlDbType.TinyInt);
                //mySqlCommand.Parameters.Add("@PorcSalvamentoLOC", SqlDbType.Int);
                //mySqlCommand.Parameters.Add("@PorcSalvamentoIFRS", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Turnos", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@AjustexInflacionInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@NumeroDocCompra", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, 3);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@LocFisicaID", SqlDbType.Char, 15);
                mySqlCommand.Parameters.Add("@Responsable", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Propietario", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.VarChar, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoActID", SqlDbType.Char, UDT_EstadoActID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoAnula", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@ValorSalvamentoIFRSUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorRetiroIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acGrupo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acClase", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLocFisica", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acEstado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de Valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = acCtrl.ActivoID.Value;
                mySqlCommand.Parameters["@PlaquetaID"].Value = acCtrl.PlaquetaID.Value;
                mySqlCommand.Parameters["@SerialID"].Value = acCtrl.SerialID.Value;
                mySqlCommand.Parameters["@RecibidoID"].Value = acCtrl.ConsecutivoDetaID.Value;
                mySqlCommand.Parameters["@Tipo"].Value = acCtrl.Tipo.Value;
                mySqlCommand.Parameters["@FechaImportacion"].Value = acCtrl.FechaImportacion.Value;
                mySqlCommand.Parameters["@FechaVencimiento"].Value = acCtrl.FechaVencimiento.Value;
                mySqlCommand.Parameters["@FechaCapitalizacion"].Value = acCtrl.FechaCapitalizacion.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = acCtrl.DocumentoID.Value;
                mySqlCommand.Parameters["@Fecha"].Value = acCtrl.Fecha.Value;
                mySqlCommand.Parameters["@Periodo"].Value = acCtrl.Periodo.Value;
                mySqlCommand.Parameters["@CodigoBSID"].Value = acCtrl.CodigoBSID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = acCtrl.inReferenciaID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = acCtrl.EstadoInv.Value;
                mySqlCommand.Parameters["@ActivoGrupoID"].Value = acCtrl.ActivoGrupoID.Value;
                mySqlCommand.Parameters["@ActivoClaseID"].Value = acCtrl.ActivoClaseID.Value;
                mySqlCommand.Parameters["@ActivoTipoID"].Value = acCtrl.ActivoTipoID.Value;
                mySqlCommand.Parameters["@Modelo"].Value = acCtrl.Modelo.Value;
                mySqlCommand.Parameters["@VidaUtilLOC"].Value = acCtrl.VidaUtilLOC.Value;
                mySqlCommand.Parameters["@VidaUtilIFRS"].Value = acCtrl.VidaUtilIFRS.Value;
                mySqlCommand.Parameters["@TipoDepreLOC"].Value = acCtrl.TipoDepreLOC.Value;
                mySqlCommand.Parameters["@TipoDepreIFRS"].Value = acCtrl.TipoDepreIFRS.Value;
                //mySqlCommand.Parameters["@PorcSalvamentoLOC"].Value = acCtrl.PorcSalvamentoLOC.Value;
                //mySqlCommand.Parameters["@PorcSalvamentoIFRS"].Value = acCtrl.PorcSalvamentoIFRS.Value;
                mySqlCommand.Parameters["@Turnos"].Value = acCtrl.Turnos.Value;
                mySqlCommand.Parameters["@AjustexInflacionInd"].Value = acCtrl.AjustexInflacionInd.Value;
                mySqlCommand.Parameters["@NumeroDocCompra"].Value = acCtrl.NumeroDocCompra.Value;
                mySqlCommand.Parameters["@MonedaID"].Value = acCtrl.MonedaID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = acCtrl.TerceroID.Value;
                mySqlCommand.Parameters["@DocumentoTercero"].Value = acCtrl.DocumentoTercero.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = acCtrl.ProyectoID.Value;
                mySqlCommand.Parameters["@CentroCostoID"].Value = acCtrl.CentroCostoID.Value;
                mySqlCommand.Parameters["@LocFisicaID"].Value = acCtrl.LocFisicaID.Value;
                mySqlCommand.Parameters["@Responsable"].Value = acCtrl.Responsable.Value;
                mySqlCommand.Parameters["@Propietario"].Value = acCtrl.Propietario.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = acCtrl.BodegaID.Value;
                mySqlCommand.Parameters["@Observacion"].Value = acCtrl.Observacion.Value;
                mySqlCommand.Parameters["@EstadoActID"].Value = acCtrl.EstadoActID.Value;
                mySqlCommand.Parameters["@DocumentoAnula"].Value = acCtrl.DocumentoAnula.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = acCtrl.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = acCtrl.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = acCtrl.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = acCtrl.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = acCtrl.DatoAdd5.Value;
                mySqlCommand.Parameters["@ValorSalvamentoIFRSUS"].Value = acCtrl.ValorSalvamentoIFRSUS.Value;
                mySqlCommand.Parameters["@ValorRetiroIFRS"].Value = acCtrl.ValorRetiroIFRS.Value;
                mySqlCommand.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acGrupo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acClase"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acClase, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glLocFisica"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLocFisica, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acEstado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acEstado, this.Empresa, egCtrl);
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
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                dr.Close();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_Add");
                throw exception;
            }
        }

        #endregion

        #region Saldos Activos


        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un listado de saldo</returns>
        public List<DTO_acActivoSaldo> DAL_acActivoControl_GetSaldoCompraActivo(string concSaldo, int identificadorTR, string tipoBalance)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                DTO_acActivoSaldo saldo = null;

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
               
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ConceptoSaldoID"].Value = concSaldo;
                mySqlCommandSel.Parameters["@IdentificadorTR"].Value = identificadorTR;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = tipoBalance;
  
                mySqlCommandSel.CommandText =
                        " SELECT    saldo.IdentificadorTR as IdentificadorTR, " +
                        "           saldo.CuentaID AS CuentaID, " +
                        "           @ConceptoSaldoID as acComponenteID, " +
                        "           'Costo' as Descriptivo, " +
                        "           sum(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoMLoc, " +
                        "           sum(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME + DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) AS SaldoMExt, " +
                        "           SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML) as MvtMLoc, " +
                        "           SUM(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) as MvtMExt, " +
                        "           coplan.Naturaleza as Naturaleza    " +
                        " FROM	coCuentaSaldo as saldo with(nolock) " +
                        "           INNER JOIN coPlanCuenta as coplan on coplan.CuentaID = saldo.CuentaID  " +
                        " WHERE EmpresaID = @EmpresaID and saldo.ConceptoSaldoID = @ConceptoSaldoID " +
                        "           and IdentificadorTR = @IdentificadorTR and BalanceTipoID = @BalanceTipoID  " +
                        " GROUP BY saldo.CuentaID, IdentificadorTR, Descriptivo, Naturaleza ";


                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();
                List<DTO_acActivoSaldo> saldos = new List<DTO_acActivoSaldo>();
                
                while (dr.Read())
                {
                    saldo = new DTO_acActivoSaldo(dr);
                    saldos.Add(saldo);
                }                    

                dr.Close();
                return saldos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetSaldoCompraActivo");
                throw exception;
            }
        }


        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un saldo</returns>
        public List<DTO_acActivoSaldo> DAL_acActivoControl_GetSaldo(DateTime periodo, string concSaldo, int identificadorTR, string tipoBalance, string activoClaseID)
        {
            try
            {
                List<DTO_acActivoSaldo> result = new List<DTO_acActivoSaldo>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                DTO_acActivoSaldo saldo = null;

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActivoClaseID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_acContableEmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@ConceptoSaldoID"].Value = concSaldo;
                mySqlCommandSel.Parameters["@IdentificadorTR"].Value = identificadorTR;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = tipoBalance;
                mySqlCommandSel.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@ActivoClaseID"].Value = activoClaseID;
                mySqlCommandSel.Parameters["@eg_acContableEmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acContabiliza, this.Empresa, egCtrl);


                mySqlCommandSel.CommandText =
                   "SELECT  saldo.IdentificadorTR as IdentificadorTR,  " +
                   "        saldo.CuentaID AS CuentaID,  " +
                   "        contabiliza.ConceptoSaldoID as acComponenteID,	 " +
                   "        Concepto.Descriptivo as Descriptivo,    " +
                   "        sum(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoMLoc,  " +
                   "        sum(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME + DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) AS SaldoMExt,  " +
                   "        coplan.Naturaleza as Naturaleza " +
                   "FROM coCuentaSaldo as saldo with(nolock)  " +
                   "         INNER JOIN acContabiliza as contabiliza on contabiliza.CuentaID = saldo.CuentaID  " +
                   "         INNER JOIN glConceptoSaldo as Concepto on Concepto.ConceptoSaldoID = contabiliza.ConceptoSaldoID  " +
                   "         INNER JOIN coPlanCuenta as coplan on coplan.CuentaID = saldo.CuentaID  " +
                   "WHERE EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and saldo.ConceptoSaldoID = @ConceptoSaldoID  " +
                   "         and IdentificadorTR = @IdentificadorTR and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo  " +
                   "         and contabiliza.ActivoClaseID = @ActivoClaseID and contabiliza.EmpresaGrupoID  = @eg_acContableEmpresaGrupoID  " +
                   "GROUP BY saldo.CuentaID, IdentificadorTR, Concepto.Descriptivo, Concepto.Descriptivo, contabiliza.ConceptoSaldoID, Naturaleza ";
                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_acActivoSaldo dtoSaldo = new DTO_acActivoSaldo(dr);
                    result.Add(dtoSaldo);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetSaldo");
                throw exception;
            }
        }

        /// <summary>
        /// Carga una lista de  dto_ActivoControl con los saldos por meses de acuerdo al año del periodo
        /// </summary>
        /// <param name="periodo">Periodo de busqueda</param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <returns>Lista de saldos de un activo</returns>
        public List<DTO_acActivoSaldo> DAL_acActivoControl_GetSaldo_Meses(string año, string concSaldo, int identificadorTR, string tipoBalance, string activoClaseID, bool mLocal)
        {
            try
            {
                List<DTO_acActivoSaldo> result = new List<DTO_acActivoSaldo>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                DTO_acActivoSaldo saldo = null;

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActivoClaseID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_acContableEmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@Año"].Value = año;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ConceptoSaldoID"].Value = concSaldo;
                mySqlCommandSel.Parameters["@IdentificadorTR"].Value = identificadorTR;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = tipoBalance;
                mySqlCommandSel.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@ActivoClaseID"].Value = activoClaseID;
                mySqlCommandSel.Parameters["@eg_acContableEmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acContabiliza, this.Empresa, egCtrl);

                if (mLocal)
                {
                    mySqlCommandSel.CommandText =
                   " SELECT " +
                               " CuentaID, [01] AS 'Enero', [02] AS 'Febrero' , [03] AS 'Marzo' , [04] AS 'Abril', [05] AS 'Mayo', [06] AS 'Junio', " +
                               " [07] AS 'Julio', [08] AS 'Agosto', [09] AS 'Septiembre', [10] AS 'Octubre' , [11] AS 'Noviembre', [12] AS 'Diciembre' " +
                   " FROM( " +
                   " SELECT " +
                               " saldo.CuentaID, " +
                               " MOnth(PeriodoID) [Mes], " +
                               " sum(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoMLoc " +
                   " FROM coCuentaSaldo as saldo " +
                               " INNER JOIN acContabiliza as contabiliza on contabiliza.CuentaID = saldo.CuentaID " +
                               " INNER JOIN glConceptoSaldo as Concepto on Concepto.ConceptoSaldoID = contabiliza.ConceptoSaldoID " +
                   " WHERE Year(PeriodoID) = @Año and EmpresaID = @EmpresaID AND saldo.ConceptoSaldoID = @ConceptoSaldoID " +
                               " AND IdentificadorTR = @IdentificadorTR and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo " +
                               " AND contabiliza.ActivoClaseID = @ActivoClaseID and contabiliza.EmpresaGrupoID  = @eg_acContableEmpresaGrupoID " +
                   " GROUP BY saldo.CuentaID, PeriodoID " +
                               " ) " +
                               " pvt " +
                               " PIVOT (SUM(SaldoMLoc) " +
                   " FOR [Mes] IN ([01], [02] ,[03], [04], [05], [06], [07], [08], [09], [10], [11], [12])) AS Saldo " +
                   " ORDER BY CuentaID ";
                }
                else
                {
                    mySqlCommandSel.CommandText =
                  " SELECT " +
                              " CuentaID, [01] AS 'Enero', [02] AS 'Febrero' , [03] AS 'Marzo' , [04] AS 'Abril', [05] AS 'Mayo', [06] AS 'Junio', " +
                              " [07] AS 'Julio', [08] AS 'Agosto', [09] AS 'Septiembre', [10] AS 'Octubre' , [11] AS 'Noviembre', [12] AS 'Diciembre' " +
                  " FROM( " +
                  " SELECT " +
                              " saldo.CuentaID, " +
                              " MOnth(PeriodoID) [Mes], " +
                              " sum(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME + DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) AS SaldoMExt " +
                  " FROM coCuentaSaldo as saldo " +
                              " INNER JOIN acContabiliza as contabiliza on contabiliza.CuentaID = saldo.CuentaID " +
                              " INNER JOIN glConceptoSaldo as Concepto on Concepto.ConceptoSaldoID = contabiliza.ConceptoSaldoID" +
                  " WHERE Year(PeriodoID) = @Año and EmpresaID = @EmpresaID AND saldo.ConceptoSaldoID = @ConceptoSaldoID " +
                              " AND IdentificadorTR = @IdentificadorTR and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo " +
                              " AND contabiliza.ActivoClaseID = @ActivoClaseID and contabiliza.EmpresaGrupoID  = @eg_acContableEmpresaGrupoID " +
                  " GROUP BY saldo.CuentaID, PeriodoID " +
                              " ) " +
                              " pvt " +
                              " PIVOT (SUM(SaldoMExt) " +
                  " FOR [Mes] IN ([01], [02] ,[03], [04], [05], [06], [07], [08], [09], [10], [11], [12])) AS Saldo " +
                              " ORDER BY CuentaID ";
                }

                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_acActivoSaldo dtoSaldo = new DTO_acActivoSaldo(dr, true);
                    result.Add(dtoSaldo);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetSaldo_Meses");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los movimientos del activo
        /// </summary>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un saldo</returns>
        public List<DTO_acActivoSaldo> DAL_acActivoControl_GetMvtos(DateTime periodo, string concSaldo, int identificadorTR, string tipoBalance, string activoClaseID)
        {
            try
            {
                List<DTO_acActivoSaldo> result = new List<DTO_acActivoSaldo>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                DTO_acActivoSaldo saldo = null;

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActivoClaseID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_acContableEmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@IdentificadorTR"].Value = identificadorTR;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = tipoBalance;
                mySqlCommandSel.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@ActivoClaseID"].Value = activoClaseID;
                mySqlCommandSel.Parameters["@eg_acContableEmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acContabiliza, this.Empresa, egCtrl);

                mySqlCommandSel.CommandText =
                " SELECT  saldo.IdentificadorTR as IdentificadorTR,  " +
                "         saldo.CuentaID AS CuentaID,  " +
                "         Concepto.ConceptoSaldoID as acComponenteID, " +
                "         Concepto.Descriptivo as Descriptivo,    " +
                "         SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML) AS MvtMLoc, " +
                "         SUM(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) AS MvtMExt  " +
                "FROM coCuentaSaldo as saldo with(nolock)  " +
                "         INNER JOIN acContabiliza as contabiliza on contabiliza.CuentaID = saldo.CuentaID  " +
                "         INNER JOIN glConceptoSaldo as Concepto on Concepto.ConceptoSaldoID = contabiliza.ConceptoSaldoID " +
                "WHERE EmpresaID = @EmpresaID and PeriodoID = @PeriodoID  " +
                "         and IdentificadorTR = @IdentificadorTR and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo " +
                "         and contabiliza.ActivoClaseID = @ActivoClaseID and contabiliza.EmpresaGrupoID = @eg_acContableEmpresaGrupoID   " +
                "GROUP BY saldo.CuentaID, IdentificadorTR, contabiliza.acComponenteID, Descriptivo ";
                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_acActivoSaldo dtoSaldo = new DTO_acActivoSaldo(dr);
                    result.Add(dtoSaldo);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetMvtos");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae el comprobante de acuerdo al numeroDoc y el idTR
        /// </summary>
        /// <param name="numeroDoc">Numero del documento</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna una lista de comprobanteFooter</returns>
        public List<DTO_ComprobanteFooter> DAL_acActivoControl_GetByIdentificadorTR(int numeroDoc, int identTR)
        {
            try
            {
                List<DTO_ComprobanteFooter> result = new List<DTO_ComprobanteFooter>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@IdentificadorTR"].Value = identTR;

                mySqlCommandSel.CommandText =
                " SELECT * FROM coAuxiliar as auxiliar with(nolock)     " +
                " INNER JOIN acContabiliza as contabiliza on contabiliza.CuentaID = auxiliar.CuentaID  " +
                " INNER JOIN glConceptoSaldo as Concepto on Concepto.ConceptoSaldoID = contabiliza.ConceptoSaldoID " +
                " WHERE auxiliar.NumeroDoc = @NumeroDoc and auxiliar.IdentificadorTR = @IdentificadorTR ";
                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_ComprobanteFooter comp = new DTO_ComprobanteFooter(dr);
                    result.Add(comp);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetByIdentificadorTR_and_NumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDebito"></param>
        /// <param name="origenMonetarioL"></param>
        /// <param name="valor"></param>
        /// <param name="ismonedaLocal"></param>
        /// <param name="identTR"></param>
        /// <param name="conceptoSaldo"></param>
        public void DAL_acActivoControl_UpdateSaldos(int documentoID, DTO_coCuentaSaldo dto_Saldo, int identTR, string conceptoSaldo)
        {
            bool add = false;
            string where = "WHERE IdentificadorTR = @IdentificadorTR and ConceptoSaldoID = @conceptoSaldo";
            string colDbOrigenLocML = "[DbOrigenLocML] = @DbOrigenLocML ";
            string colDbOrigenLocME = " DbOrigenLocME = @DbOrigenLocME ";

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                if (dto_Saldo.DbOrigenLocML.Value != null)
                {
                    mySqlCommand.CommandText = "UPDATE coCuentaSaldo SET " +
                                               " [DbOrigenLocML] = @DbOrigenLocML " +
                                               where;

                    mySqlCommand.Parameters.Add("@DbOrigenLocML", SqlDbType.Decimal);
                    mySqlCommand.Parameters["@DbOrigenLocML"].Value = dto_Saldo.DbOrigenLocML.Value.Value;

                    add = true;
                }
                if (dto_Saldo.DbOrigenLocME.Value != null)
                {
                    if (add)
                    {
                        mySqlCommand.CommandText = "UPDATE coCuentaSaldo SET " +
                                                   " DbOrigenLocME = @DbOrigenLocME " +

                                                   where;

                    }
                    mySqlCommand.CommandText = "UPDATE coCuentaSaldo SET " +
                                                     " DbOrigenLocME = @DbOrigenLocME " +
                                             " WHERE IdentificadorTR = @IdentificadorTR and ConceptoSaldoID = @conceptoSaldo";

                    mySqlCommand.Parameters.Add("@DbOrigenLocME", SqlDbType.Decimal);
                    mySqlCommand.Parameters["@DbOrigenLocME"].Value = dto_Saldo.DbOrigenLocME.Value.Value;
                    mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;
                    mySqlCommand.Parameters.Add("@conceptoSaldo", SqlDbType.Char);
                    mySqlCommand.Parameters["@conceptoSaldo"].Value = conceptoSaldo;

                    mySqlCommand.ExecuteNonQuery();

                }
                if (dto_Saldo.DbOrigenExtME.Value != null)
                {
                    mySqlCommand.CommandText = "UPDATE coCuentaSaldo SET " +
                                                     " DbOrigenExtME = @DbOrigenExtME " +
                                             " WHERE IdentificadorTR = @IdentificadorTR and ConceptoSaldoID = @conceptoSaldo";
                    mySqlCommand.Parameters.Add("@DbOrigenExtME", SqlDbType.Decimal);
                    mySqlCommand.Parameters["@DbOrigenExtME"].Value = dto_Saldo.DbOrigenExtME.Value.Value;
                    mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;
                    mySqlCommand.Parameters.Add("@conceptoSaldo", SqlDbType.Char);
                    mySqlCommand.Parameters["@conceptoSaldo"].Value = conceptoSaldo;

                    mySqlCommand.ExecuteNonQuery();

                }
                if (dto_Saldo.DbOrigenExtML.Value != null)
                {
                    mySqlCommand.CommandText = "UPDATE coCuentaSaldo SET " +
                                                     " DbOrigenExtML = @DbOrigenExtML " +
                                             " WHERE IdentificadorTR = @IdentificadorTR and ConceptoSaldoID = @conceptoSaldo";
                    mySqlCommand.Parameters.Add("@DbOrigenExtML", SqlDbType.Decimal);
                    mySqlCommand.Parameters["@DbOrigenExtML"].Value = dto_Saldo.DbOrigenExtML.Value.Value;
                    mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;
                    mySqlCommand.Parameters.Add("@conceptoSaldo", SqlDbType.Char);
                    mySqlCommand.Parameters["@conceptoSaldo"].Value = conceptoSaldo;

                    mySqlCommand.ExecuteNonQuery();

                }
                if (dto_Saldo.CrOrigenExtME.Value != null)
                {
                    mySqlCommand.CommandText = "UPDATE coCuentaSaldo SET " +
                                                     " CrOrigenExtME = @CrOrigenExtME " +
                                             " WHERE IdentificadorTR = @IdentificadorTR and ConceptoSaldoID = @conceptoSaldo";

                    mySqlCommand.Parameters.Add("@CrOrigenExtME", SqlDbType.Decimal);
                    mySqlCommand.Parameters["@CrOrigenExtME"].Value = dto_Saldo.CrOrigenExtME.Value.Value;
                    mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;
                    mySqlCommand.Parameters.Add("@conceptoSaldo", SqlDbType.Char);
                    mySqlCommand.Parameters["@conceptoSaldo"].Value = conceptoSaldo;

                    mySqlCommand.ExecuteNonQuery();

                }
                if (dto_Saldo.CrOrigenExtML.Value != null)
                {
                    mySqlCommand.CommandText = "UPDATE coCuentaSaldo SET " +
                                                      " CrOrigenExtML = @CrOrigenExtML " +
                                             " WHERE IdentificadorTR = @IdentificadorTR and ConceptoSaldoID = @conceptoSaldo";

                    mySqlCommand.Parameters.Add("@CrOrigenExtML", SqlDbType.Decimal);
                    mySqlCommand.Parameters["@CrOrigenExtML"].Value = dto_Saldo.CrOrigenExtML.Value.Value;

                    mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;
                    mySqlCommand.Parameters.Add("@conceptoSaldo", SqlDbType.Char);
                    mySqlCommand.Parameters["@conceptoSaldo"].Value = conceptoSaldo;

                    mySqlCommand.ExecuteNonQuery();

                }
                if (dto_Saldo.CrOrigenLocML.Value != null)
                {
                    mySqlCommand.CommandText = "UPDATE coCuentaSaldo SET " +
                                                     " CrOrigenLocML = @CrOrigenLocML " +
                                             " WHERE IdentificadorTR = @IdentificadorTR and ConceptoSaldoID = @conceptoSaldo";

                    mySqlCommand.Parameters.Add("@CrOrigenLocML", SqlDbType.Decimal);
                    mySqlCommand.Parameters["@CrOrigenLocML"].Value = dto_Saldo.CrOrigenLocML.Value.Value;

                    mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;
                    mySqlCommand.Parameters.Add("@conceptoSaldo", SqlDbType.Char);
                    mySqlCommand.Parameters["@conceptoSaldo"].Value = conceptoSaldo;

                    mySqlCommand.ExecuteNonQuery();

                }
                if (dto_Saldo.CrOrigenLocME.Value != null)
                {
                    mySqlCommand.CommandText = "UPDATE coCuentaSaldo SET " +
                                                     " CrOrigenLocME = @CrOrigenLocME " +
                                             " WHERE IdentificadorTR = @IdentificadorTR and ConceptoSaldoID = @conceptoSaldo";

                    mySqlCommand.Parameters.Add("@CrOrigenLocME", SqlDbType.Decimal);
                    mySqlCommand.Parameters["@CrOrigenLocME"].Value = dto_Saldo.CrOrigenLocME.Value.Value;

                    mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;
                    mySqlCommand.Parameters.Add("@conceptoSaldo", SqlDbType.Char);
                    mySqlCommand.Parameters["@conceptoSaldo"].Value = conceptoSaldo;

                    mySqlCommand.ExecuteNonQuery();
                }

                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;
                mySqlCommand.Parameters.Add("@conceptoSaldo", SqlDbType.Char);
                mySqlCommand.Parameters["@conceptoSaldo"].Value = conceptoSaldo;

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoControl", "DAL_acActivoControl_UpdateSaldos");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los compponentes de acContabiliza de acuerdo a la clase del activo
        /// </summary>
        /// <param name="activoClaseID"></param>
        /// <returns>Lista de activosaldo</returns>
        public List<DTO_acActivoSaldo> DAL_acActivoControl_GetComponentesPorClaseActivoID(string activoClaseID)
        {
            try
            {
                List<DTO_acActivoSaldo> result = new List<DTO_acActivoSaldo>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActivoClaseID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ActivoClaseID"].Value = activoClaseID;

                mySqlCommandSel.CommandText =
                   "			  SELECT              " +
                   "                conta.CuentaID,   " +
                   "                conta.ConceptoSaldoID as acComponenteID, " +
                   "                Concepto.Descriptivo as Descriptivo     " +
                   "              FROM acContabiliza as conta with(nolock)   " +
                   "              INNER JOIN glConceptoSaldo as Concepto on Concepto.ConceptoSaldoID = conta.ConceptoSaldoID " +
                   "              WHERE ActivoClaseID = @ActivoClaseID and conta.EmpresaGrupoID = @EmpresaID";
                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_acActivoSaldo dtoSaldo = new DTO_acActivoSaldo(dr);
                    result.Add(dtoSaldo);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetComponentesPorClaseActivoID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los saldos de acuerdo al componente
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="cuenta"></param>
        /// <param name="componenteID"></param>
        /// <param name="descriptivo"></param>
        /// <param name="concSaldo"></param>
        /// <param name="identificadorTR"></param>
        /// <param name="tipoBalance"></param>
        /// <param name="activoClaseID"></param>
        /// <returns></returns>
        public DTO_acActivoSaldo DAL_acActivoControl_GetSaldoXComponente(DateTime periodo, string cuenta, string descriptivo, string concSaldo, int identificadorTR, string tipoBalance, string activoClaseID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Cuenta", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Cuenta"].Value = cuenta;
                mySqlCommand.Parameters["@ConceptoSaldoID"].Value = concSaldo;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = identificadorTR;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = tipoBalance;
                mySqlCommand.Parameters["@Descriptivo"].Value = descriptivo;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                " SELECT    saldo.IdentificadorTR as IdentificadorTR, " +
                "           saldo.CuentaID AS CuentaID, " +
                "           @ConceptoSaldoID as acComponenteID, " +
                "           @Descriptivo as Descriptivo, " +
                "           sum(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoMLoc, " +
                "           sum(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME + DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) AS SaldoMExt, " +
                "           coplan.Naturaleza as Naturaleza    " +
                " FROM	coCuentaSaldo as saldo with(nolock) " +
                "           INNER JOIN coPlanCuenta as coplan on coplan.CuentaID = saldo.CuentaID  " +
                " WHERE EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and saldo.ConceptoSaldoID = @ConceptoSaldoID " +
                "           and IdentificadorTR = @IdentificadorTR and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo " +
                "           and saldo.CuentaID = @cuenta   " +
                " GROUP BY saldo.CuentaID, IdentificadorTR, Descriptivo, Naturaleza ";


                DTO_acActivoSaldo res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_acActivoSaldo(dr);
                }
                dr.Close();


                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetSaldo");
                throw exception;
            }
        }
      

        #endregion

        #region Retiro Activos

        /// <summary>
        /// Listado de Componentes y sus saldos por activo
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <returns></returns>
        public List<DTO_acRetiroActivoComponente> DAL_acActivoControl_GetComponentes(int activoID, string tipoBalance)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = activoID;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = tipoBalance;

                mySqlCommand.CommandText =
                "   SELECT acComponenteActivo.ComponenteActivoID,   " +
                "   acComponenteActivo.Descriptivo,   " +
                "   coCuentaSaldo.*   " +
                "   FROM acActivoControl WITH(NOLOCK)   " +
                "   INNER JOIN acClase WITH(NOLOCK) on acActivoControl.ActivoClaseID = acClase.ActivoClaseID   " +
                "   INNER JOIN acContabiliza WITH(NOLOCK) on acClase.ActivoClaseID = acContabiliza.ActivoClaseID   " +
                "   INNER JOIN acComponenteActivo WITH(NOLOCK) on acContabiliza.ComponenteActivoID = acComponenteActivo.ComponenteActivoID   " +
                "   INNER JOIN coCuentaSaldo WITH(NOLOCK) on acContabiliza.CuentaID = coCuentaSaldo.CuentaID and acActivoControl.ActivoID = coCuentaSaldo.IdentificadorTR   " +
                "   WHERE acActivoControl.EmpresaID = @EmpresaID   AND   coCuentaSaldo.BalanceTipoID = @BalanceTipoID " +
                "   AND acActivoControl.ActivoID = @ActivoID   ";

                List<DTO_acRetiroActivoComponente> res = new List<DTO_acRetiroActivoComponente>();
                DTO_acRetiroActivoComponente doc = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_acRetiroActivoComponente(dr);
                    res.Add(doc);
                }
                dr.Close();


                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetComponenentes");
                throw exception;
            }
        }

        #endregion

        #region Contenedores


        /// <summary>
        /// Obtiene los activos hijos 
        /// </summary>
        /// <param name="activoID">activoID padre</param>
        /// <returns>listado de activos control</returns>
        public List<DTO_acActivoControl> DAL_acActivoControl_GetChildrenActivos(int activoID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Char, UDT_CuentaID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = activoID;

                mySqlCommand.CommandText =  "   Select *  " +
                                            "   from acActivoControl  " +
                                            "   where  acActivoControl.ActivoPadreID = @ActivoID   " +
                                            "   and acActivoControl.ActivoID <> @ActivoID ";

                List<DTO_acActivoControl> res = new List<DTO_acActivoControl>();
                DTO_acActivoControl doc = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_acActivoControl(dr);
                    res.Add(doc);
                }
                dr.Close();

                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetChildrenActivos");
                throw exception;
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Funcion que obtiene la información del activo de acuerdo al filtro
        /// </summary>
        /// <param name="plaqueta">Filtro de PLaquetaID</param>
        /// <param name="serial">Filtro de SerialID</param>
        /// <param name="referencia">Filtro de Referencia</param>
        /// <param name="clase">Filtro de ActivoClaseID</param>
        /// <param name="tipo">Filtro de ActivoTipoID</param>
        /// <param name="grupo">Filtro de ActivoGrupoID</param>
        /// <param name="locFisica">Filtro de LocfisicaIF</param>
        /// <param name="contenedor">Bool que dice si trae o no los activos contenidos</param>
        /// <param name="responsable">Filtro de Responsable - TerceroID</param>
        /// <returns>Lista de detalles para la consulta</returns>
        public List<DTO_acQueryActivoControl> DAL_acActivoControl_GetByParameter(string plaqueta, string serial, string referencia, string clase, string tipo, string grupo, string locFisica, bool contenedor, string responsable)
        {
            #region Variables

            string filPlaqueta = "";
            string filSerial = "";
            string filReferencia = "";
            string filClase = "";
            string filTipo = "";
            string filGrupo = "";
            string filLocFisica = "";
            string filResponsable = "";

            #endregion
            #region Filtros

            if (!string.IsNullOrWhiteSpace(plaqueta))
                filPlaqueta = "AND acCtrl.PlaquetaID = " + " '" + plaqueta + " '";
            if (!string.IsNullOrWhiteSpace(serial))
                filSerial = "AND acCtrl.SerialID = " + " '" + serial + " '";
            if (!string.IsNullOrWhiteSpace(referencia))
                filReferencia = "AND acCtrl.inReferenciaID = " + " '" + referencia + " '";
            if (!string.IsNullOrWhiteSpace(clase))
                filClase = "AND acCtrl.ActivoClaseID = " + " '" + clase + " '";
            if (!string.IsNullOrWhiteSpace(tipo))
                filTipo = "AND acCtrl.ActivoTipoID = " + " '" + tipo + " '";
            if (!string.IsNullOrWhiteSpace(grupo))
                filGrupo = "AND acCtrl.ActivoGrupoID = " + " '" + grupo + " '";
            if (!string.IsNullOrWhiteSpace(locFisica))
                filLocFisica = "AND acCtrl.LocFisicaID = " + " '" + locFisica + " '";
            if (!string.IsNullOrWhiteSpace(responsable))
                filResponsable = "AND acCtrl.Responsable = " + " '" + responsable + " '";

            #endregion

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #region CommandText

                mySqlCommand.CommandText =
                    " SELECT  " +
                    " ActivoID, PlaquetaID, SerialID, Observacion, Fecha as FechaCompra, coter.Descriptivo as Proveedor, acCtrl.DocumentoTercero as Factura, " +
                    " acCtrl.TipoDepreLOC, acCtrl.TipoDepreIFRS, acCtrl.TipoDepreUsG, acCtrl.VidaUtilLOC, acCtrl.VidaUtilIFRS, acCtrl.VidaUtilUSG, " +
                    " acCtrl.ValorSalvamentoLOC, acCtrl.ValorSalvamentoIFRS, acCtrl.ValorSalvamentoUSG,  acCtrl.ValorRetiroIFRS, grupo.Descriptivo as ActivoGrupo, " +
                    " clase.descriptivo as ActivoClase, tipo.Descriptivo as ActivoTipo, acCtrl.Modelo, proy.Descriptivo as Proyecto, costo.Descriptivo as CentroCosto, " +
                    " locFisica.Descriptivo as LocFisica, acCtrl.ActivoPadreID, " +
                    " acCtrl.Responsable , acCtrl.inReferenciaID " +
                    " FROM acActivoControl acCtrl WITH(NOLOCK) " +
                    " INNER JOIN coTercero coter WITH(NOLOCK)ON  coter.TerceroID = acCtrl.TerceroID " +
                    " INNER JOIN acGrupo grupo WITH(NOLOCK)ON grupo.ActivoGrupoID = acCtrl.ActivoGrupoID " +
                    " INNER JOIN acClase clase WITH(NOLOCK)ON clase.ActivoClaseID = acCtrl.ActivoClaseID " +
                    " INNER JOIN acTipo tipo WITH(NOLOCK)ON tipo.ActivoTipoID = acCtrl.ActivoTipoID " +
                    " INNER JOIN coCentroCosto costo WITH(NOLOCK)ON costo.CentroCostoID = acCtrl.CentroCostoID " +
                    " INNER JOIN glLocFisica locFisica WITH(NOLOCK)ON LocFisica.LocFisicaID = acCtrl.LocFisicaID " +
                    " INNER JOIN coProyecto proy WITH(NOLOCK)ON proy.ProyectoID = acctrl.ProyectoID " +
                    " WHERE acCtrl.EmpresaID =  @EmpresaID " +
                    filPlaqueta +
                    filSerial +
                    filReferencia +
                    filClase +
                    filTipo +
                    filGrupo +
                    filLocFisica +
                    filResponsable +
                    " GROUP BY acCtrl.PlaquetaID, acCtrl.SerialID, acCtrl.Observacion,acCtrl.Fecha, coter.Descriptivo, acCtrl.DocumentoTercero " +
                    " , acCtrl.TipoDepreLOC, acCtrl.TipoDepreIFRS, acCtrl.TipoDepreUSG, acCtrl.VidaUtilLOC, acCtrl.VidaUtilIFRS, acCtrl.VidaUtilIFRS, " +
                    " acCtrl.VidaUtilUSG, acCtrl.ValorSalvamentoLOC, acCtrl.ValorSalvamentoIFRS, acCtrl.ValorSalvamentoUSG, acCtrl.ValorRetiroIFRS, " +
                    " costo.Descriptivo, acCtrl.ActivoID,  grupo.Descriptivo ,clase.descriptivo , tipo.Descriptivo , acCtrl.Modelo, proy.Descriptivo, " +
                    " locFisica.Descriptivo , acCtrl.ActivoPadreID, acCtrl.Responsable, acCtrl.inReferenciaID ";

                List<DTO_acQueryActivoControl> res = new List<DTO_acQueryActivoControl>();
                DTO_acQueryActivoControl doc;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_acQueryActivoControl(dr);
                    res.Add(doc);
                }
                dr.Close();

                return res;
                #endregion
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetComponenentes");
                throw exception;
            }

        }

        /// <summary>
        /// Funcionque obrtinee los saldos correspondientes al activo
        /// </summary>
        /// <param name="identificadorTr">Activo ID del activo</param>
        /// <param name="balanceTipoID">Tipo de Libro (Funcional o IFRS)</param>
        /// <param name="año">Año que se va a consultar</param>
        /// <param name="mes">Mes que se quiere consultar</param>
        /// <returns>Lista de saldos por activo</returns>
        public List<DTO_acActivoQuerySaldos> DAL_acActivoControl_GetSaldosByMesYLibro(int identificadorTr, string balanceTipoID, DateTime periodo, string componenteActivoID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                #region Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@identificadorTR", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@componenteID", SqlDbType.Char);
            
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@identificadorTR"].Value = identificadorTr;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = balanceTipoID;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@componenteID"].Value = componenteActivoID;
                
                #endregion
                #region CommandText

                mySqlCommand.CommandText =
                    " SELECT " +
                    " SUM(DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) as SaldoIniML, " +
                    " SUM(DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) as SaldoIniME, " +
                    " SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML) as VlrMtoML, " +
                    " SUM(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) as VlrMtoME, " +
                    " SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoActualML, " +
                    " SUM(DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME + DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) AS SaldoActualME, " +
                    " coCuentaSaldo.ConceptoSaldoID, IdentificadorTR, BalanceTipoID, " +
                    " compAc.Descriptivo as Componente " +
                    " FROM coCuentaSaldo WITH (NOLOCK) " +
                    " INNER JOIN acActivoControl acCtrl with(nolock) ON acCtrl.ActivoID = coCuentaSaldo.IdentificadorTR " +
                    " inner join glConceptoSaldo AS glconsSaldo WITH(NOLOCK) ON  glconsSaldo.ConceptoSaldoID = coCuentaSaldo.ConceptoSaldoID " +
                    " inner join acComponenteActivo AS compAc WITH(NOLOCK) ON  compAc.ConceptoSaldoID = glConsSaldo.ConceptoSaldoID " +
                    " WHERE glconsSaldo.coSaldoControl = 5 " +
                    " AND coCuentaSaldo.EmpresaId = @EmpresaID " +
                    " AND coCuentaSaldo.PeriodoID = @Periodo   " +
                    " AND compAc.tipoComponente <> 3 " +
                    " AND BalanceTipoId = @BalanceTipoID " +
                    " AND compAc.ComponenteActivoID =  @componenteID " +
                    " GROUP BY coCuentaSaldo.ConceptoSaldoID, IdentificadorTR, BalanceTipoID, DbOrigenLocML, CrOrigenLocML, " +
                    " DbSaldoIniLocME, CrOrigenLocME, compAc.Descriptivo ";

                List<DTO_acActivoQuerySaldos> res = new List<DTO_acActivoQuerySaldos>();
                DTO_acActivoQuerySaldos doc;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_acActivoQuerySaldos(dr);
                    res.Add(doc);
                }
                dr.Close();

                return res;
                #endregion
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoControl_GetSaldosByMesYLibro");
                throw exception;
            }
        }

        #endregion
    }
}
