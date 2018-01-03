using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_pyProyectoDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Agrega docu Solproyecto
        /// </summary>
        /// <param name="documento">documento</param>
        /// <returns></returns>
        public void DAL_pyProyectoDocu_Add(DTO_pyProyectoDocu documento)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "   INSERT INTO pyProyectoDocu " +
                                           "      ([NumeroDoc]   " +
                                           "      ,[EmpresaID]   " +
                                           "      ,[DocSolicitud]   " +
                                           "      ,[ClienteID]   " +
                                           "      ,[EmpresaNombre]   " +
                                           "      ,[ResponsableEMP]   " +
                                           "      ,[ResponsableCLI]   " +
                                           "      ,[ResponsableCorreo]   " +
                                           "      ,[ResponsableTelefono]   " +
                                           "      ,[ContratoID]   " +
                                           "      ,[ClaseServicioID]   " +
                                           "      ,[PropositoProyecto]   " +
                                           "      ,[TipoSolicitud]   " +
                                           "      ,[RecursosXTrabajoInd]   " +
                                           "      ,[DescripcionSOL]   " +
                                           "      ,[Licitacion]   " +
                                           "      ,[TasaCambio]   " +
                                           "      ,[MonedaPresupuesto]   " +
                                           "      ,[PorAjusteCambio]   " +
                                           "      ,[TipoAjusteCambio]   " +
                                           "      ,[PorIPC]   " +
                                           "      ,[APUIncluyeAIUInd]   " +
                                           "      ,[Jerarquia]   " +
                                           "      ,[PorClienteADM]   " +
                                           "      ,[PorClienteIMP]   " +
                                           "      ,[PorClienteUTI]   " +
                                           "      ,[PorEmpresaADM]   " +
                                           "      ,[PorEmpresaIMP]   " +
                                           "      ,[PorEmpresaUTI]   " +
                                           "      ,[PorMultiplicadorPresup]   " +
                                           "      ,[MultiplicadorActivoInd]   " +
                                           "      ,[PorIVA]   " +
                                           "      ,[VersionPrevia]   " +
                                           "      ,[Version]   " +
                                           "      ,[TipoRedondeo]   " +
                                           "      ,[EquipoCantidadInd]   " +
                                           "      ,[PersonalCantidadInd]   " +
                                           "      ,[FechaFijaEntregas]   " +
                                           "      ,[UsuarioFijaEntregas]   " +
                                           "      ,[FechaInicio]   " +
                                           "      ,[eg_faCliente]   " +
                                           "      ,[eg_pyContrato]   " +
                                           "      ,[eg_pyClaseProyecto])   " +
                                            "   VALUES " +
                                             "      (@NumeroDoc " +
                                             "      ,@EmpresaID " +
                                             "      ,@DocSolicitud " +
                                             "      ,@ClienteID " +
                                             "      ,@EmpresaNombre " +
                                             "      ,@ResponsableEMP " +
                                             "      ,@ResponsableCLI " +
                                             "      ,@ResponsableCorreo " +
                                             "      ,@ResponsableTelefono " +
                                             "      ,@ContratoID " +
                                             "      ,@ClaseServicioID " +
                                             "      ,@PropositoProyecto " +
                                             "      ,@TipoSolicitud " +
                                             "      ,@RecursosXTrabajoInd " +
                                             "      ,@DescripcionSOL " +
                                             "      ,@Licitacion   " +
                                             "      ,@TasaCambio   " +
                                             "      ,@MonedaPresupuesto   " +
                                             "      ,@PorAjusteCambio   " +
                                             "      ,@TipoAjusteCambio   " +
                                             "      ,@PorIPC   " +
                                             "      ,@APUIncluyeAIUInd   " +
                                             "      ,@Jerarquia   " +
                                             "      ,@PorClienteADM   " +
                                             "      ,@PorClienteIMP   " +
                                             "      ,@PorClienteUTI   " +
                                             "      ,@PorEmpresaADM   " +
                                             "      ,@PorEmpresaIMP   " +
                                             "      ,@PorEmpresaUTI  " +
                                             "      ,@PorMultiplicadorPresup   " +
                                             "      ,@MultiplicadorActivoInd   " +
                                             "      ,@PorIVA   " +
                                             "      ,@VersionPrevia   " +
                                             "      ,@Version   " +
                                             "      ,@TipoRedondeo   " +
                                             "      ,@EquipoCantidadInd   " +
                                             "      ,@PersonalCantidadInd   " +
                                             "      ,@FechaFijaEntregas   " +
                                             "      ,@UsuarioFijaEntregas   " +
                                             "      ,@FechaInicio   " +
                                             "      ,@eg_faCliente " +
                                             "      ,@eg_pyContrato " +
                                             "      ,@eg_pyClaseProyecto ) ";
                #endregion
                #region Creacion de parametros

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocSolicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClaseServicioID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaNombre", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableEMP", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableCLI", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableCorreo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableTelefono", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoID", SqlDbType.Char, UDT_ContratoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecursosXTrabajoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PropositoProyecto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoSolicitud", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DescripcionSOL", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@Licitacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MonedaPresupuesto", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PorAjusteCambio", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoAjusteCambio", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PorIPC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@APUIncluyeAIUInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Jerarquia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PorClienteADM", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorClienteIMP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorClienteUTI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorEmpresaADM", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorEmpresaIMP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorEmpresaUTI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorMultiplicadorPresup", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MultiplicadorActivoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PorIVA", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VersionPrevia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoRedondeo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EquipoCantidadInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PersonalCantidadInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@FechaFijaEntregas", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioFijaEntregas", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@eg_faCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyContrato", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyClaseProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = documento.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocSolicitud"].Value = documento.DocSolicitud.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = documento.ClienteID.Value;
                mySqlCommandSel.Parameters["@ClaseServicioID"].Value = documento.ClaseServicioID.Value;
                mySqlCommandSel.Parameters["@EmpresaNombre"].Value = documento.EmpresaNombre.Value;
                mySqlCommandSel.Parameters["@ResponsableEMP"].Value = documento.ResponsableEMP.Value;
                mySqlCommandSel.Parameters["@ResponsableCLI"].Value = documento.ResponsableCLI.Value;
                mySqlCommandSel.Parameters["@ResponsableCorreo"].Value = documento.ResponsableCorreo.Value;
                mySqlCommandSel.Parameters["@ResponsableTelefono"].Value = documento.ResponsableTelefono.Value;
                mySqlCommandSel.Parameters["@ContratoID"].Value = documento.ContratoID.Value;
                mySqlCommandSel.Parameters["@RecursosXTrabajoInd"].Value = documento.RecursosXTrabajoInd.Value;
                mySqlCommandSel.Parameters["@PropositoProyecto"].Value = documento.PropositoProyecto.Value;
                mySqlCommandSel.Parameters["@TipoSolicitud"].Value = documento.TipoSolicitud.Value;
                mySqlCommandSel.Parameters["@DescripcionSOL"].Value = documento.DescripcionSOL.Value;
                mySqlCommandSel.Parameters["@Licitacion"].Value = documento.Licitacion.Value;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = documento.TasaCambio.Value;
                mySqlCommandSel.Parameters["@MonedaPresupuesto"].Value = documento.MonedaPresupuesto.Value;
                mySqlCommandSel.Parameters["@PorAjusteCambio"].Value = documento.PorAjusteCambio.Value;
                mySqlCommandSel.Parameters["@TipoAjusteCambio"].Value = documento.TipoAjusteCambio.Value;
                mySqlCommandSel.Parameters["@PorIPC"].Value = documento.PorIPC.Value;
                mySqlCommandSel.Parameters["@APUIncluyeAIUInd"].Value = documento.APUIncluyeAIUInd.Value;
                mySqlCommandSel.Parameters["@Jerarquia"].Value = documento.Jerarquia.Value;
                mySqlCommandSel.Parameters["@PorClienteADM"].Value = documento.PorClienteADM.Value;
                mySqlCommandSel.Parameters["@PorClienteIMP"].Value = documento.PorClienteIMP.Value;
                mySqlCommandSel.Parameters["@PorClienteUTI"].Value = documento.PorClienteUTI.Value;
                mySqlCommandSel.Parameters["@PorEmpresaADM"].Value = documento.PorEmpresaADM.Value;
                mySqlCommandSel.Parameters["@PorEmpresaIMP"].Value = documento.PorEmpresaIMP.Value;
                mySqlCommandSel.Parameters["@PorEmpresaUTI"].Value = documento.PorEmpresaUTI.Value;
                mySqlCommandSel.Parameters["@PorMultiplicadorPresup"].Value = documento.PorMultiplicadorPresup.Value;
                mySqlCommandSel.Parameters["@MultiplicadorActivoInd"].Value = documento.MultiplicadorActivoInd.Value;
                mySqlCommandSel.Parameters["@PorIVA"].Value = documento.PorIVA.Value;
                mySqlCommandSel.Parameters["@VersionPrevia"].Value = documento.VersionPrevia.Value;
                mySqlCommandSel.Parameters["@Version"].Value = documento.Version.Value;
                mySqlCommandSel.Parameters["@TipoRedondeo"].Value = documento.TipoRedondeo.Value;
                mySqlCommandSel.Parameters["@EquipoCantidadInd"].Value = documento.EquipoCantidadInd.Value;
                mySqlCommandSel.Parameters["@PersonalCantidadInd"].Value = documento.PersonalCantidadInd.Value;
                mySqlCommandSel.Parameters["@FechaFijaEntregas"].Value = documento.FechaFijaEntregas.Value;
                mySqlCommandSel.Parameters["@UsuarioFijaEntregas"].Value = documento.UsuarioFijaEntregas.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = documento.FechaInicio.Value;
                mySqlCommandSel.Parameters["@eg_faCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faCliente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyContrato"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyContrato, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyClaseProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyClaseProyecto, this.Empresa, egCtrl);
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
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza docu Solproyecto
        /// </summary>
        /// <param name="documento">documento</param>
        /// <returns></returns>
        public void DAL_pyProyectoDocu_Upd(DTO_pyProyectoDocu documento)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                                "   UPDATE pyProyectoDocu SET					" +
                                                "   ClienteID			=	@ClienteID				" +
                                                "  ,DocSolicitud		=	@DocSolicitud			" +
                                                "  ,EmpresaNombre		=	@EmpresaNombre			" +
                                                "  ,ResponsableEMP		=	@ResponsableEMP			" +
                                                "  ,ResponsableCLI		=	@ResponsableCLI			" +
                                                "  ,ResponsableCorreo	=	@ResponsableCorreo		" +
                                                "  ,ResponsableTelefono	=	@ResponsableTelefono	" +
                                                "  ,ContratoID			=	@ContratoID				" +
                                                "  ,ClaseServicioID		=	@ClaseServicioID		" +
                                                "  ,PropositoProyecto	=	@PropositoProyecto		" +
                                                "  ,TipoSolicitud		=	@TipoSolicitud			" +
                                                "  ,RecursosXTrabajoInd	=	@RecursosXTrabajoInd	" +
                                                "  ,DescripcionSOL		=	@DescripcionSOL			" +
                                                "  ,Licitacion		=	@Licitacion			" +
                                                "  ,TasaCambio		=	@TasaCambio			" +
                                                "  ,Jerarquia		=	@Jerarquia			" +
                                                "  ,APUIncluyeAIUInd	=	@APUIncluyeAIUInd	" +
                                                "  ,PorClienteADM  =	@PorClienteADM " +
                                                "  ,PorClienteIMP  =	@PorClienteIMP " +
                                                "  ,PorClienteUTI  =	@PorClienteUTI " +
                                                "  ,PorEmpresaADM  =	@PorEmpresaADM " +
                                                "  ,PorEmpresaIMP  =	@PorEmpresaIMP " +
                                                "  ,PorEmpresaUTI  =	@PorEmpresaUTI " +
                                                "  ,PorMultiplicadorPresup		=	@PorMultiplicadorPresup	" +
                                                "  ,VersionPrevia		=	@VersionPrevia	" +
                                                "  ,Version		=	@Version	" +
                                                "  ,TipoRedondeo		=	@TipoRedondeo	" +
                                                "  ,EquipoCantidadInd		=	@EquipoCantidadInd	" +
                                                "  ,PersonalCantidadInd		=	@PersonalCantidadInd	" +
                                                "  ,FechaFijaEntregas		=	@FechaFijaEntregas	" +
                                                "  ,UsuarioFijaEntregas		=	@UsuarioFijaEntregas	" +
                                                "  ,MultiplicadorActivoInd		=	@MultiplicadorActivoInd	" +
                                                "  ,PorIVA		=	@PorIVA	" +
                                                "  ,FechaInicio		=	@FechaInicio	" +
                                                "   WHERE  NumeroDoc	=	@NumeroDoc				";

                #endregion
                #region Creacion de parametros

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocSolicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaNombre", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableEMP", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableCLI", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableCorreo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableTelefono", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoID", SqlDbType.Char, UDT_ContratoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClaseServicioID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@PropositoProyecto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoSolicitud", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@RecursosXTrabajoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DescripcionSOL", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@Licitacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@APUIncluyeAIUInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Jerarquia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PorClienteADM", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorClienteIMP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorClienteUTI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorEmpresaADM", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorEmpresaIMP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorEmpresaUTI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorMultiplicadorPresup", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MultiplicadorActivoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PorIVA", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VersionPrevia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoRedondeo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EquipoCantidadInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PersonalCantidadInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@FechaFijaEntregas", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioFijaEntregas", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                #endregion
                #region Asignacion de valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = documento.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = documento.ClienteID.Value;
                mySqlCommandSel.Parameters["@DocSolicitud"].Value = documento.DocSolicitud.Value;
                mySqlCommandSel.Parameters["@EmpresaNombre"].Value = documento.EmpresaNombre.Value;
                mySqlCommandSel.Parameters["@ResponsableEMP"].Value = documento.ResponsableEMP.Value;
                mySqlCommandSel.Parameters["@ResponsableCLI"].Value = documento.ResponsableCLI.Value;
                mySqlCommandSel.Parameters["@ResponsableCorreo"].Value = documento.ResponsableCorreo.Value;
                mySqlCommandSel.Parameters["@ResponsableTelefono"].Value = documento.ResponsableTelefono.Value;
                mySqlCommandSel.Parameters["@ContratoID"].Value = documento.ContratoID.Value;
                mySqlCommandSel.Parameters["@ClaseServicioID"].Value = documento.ClaseServicioID.Value;
                mySqlCommandSel.Parameters["@PropositoProyecto"].Value = documento.PropositoProyecto.Value;
                mySqlCommandSel.Parameters["@TipoSolicitud"].Value = documento.TipoSolicitud.Value;
                mySqlCommandSel.Parameters["@RecursosXTrabajoInd"].Value = documento.RecursosXTrabajoInd.Value;
                mySqlCommandSel.Parameters["@DescripcionSOL"].Value = documento.DescripcionSOL.Value;
                mySqlCommandSel.Parameters["@Licitacion"].Value = documento.Licitacion.Value;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = documento.TasaCambio.Value;
                mySqlCommandSel.Parameters["@APUIncluyeAIUInd"].Value = documento.APUIncluyeAIUInd.Value;
                mySqlCommandSel.Parameters["@Jerarquia"].Value = documento.Jerarquia.Value;
                mySqlCommandSel.Parameters["@PorClienteADM"].Value = documento.PorClienteADM.Value;
                mySqlCommandSel.Parameters["@PorClienteIMP"].Value = documento.PorClienteIMP.Value;
                mySqlCommandSel.Parameters["@PorClienteUTI"].Value = documento.PorClienteUTI.Value;
                mySqlCommandSel.Parameters["@PorEmpresaADM"].Value = documento.PorEmpresaADM.Value;
                mySqlCommandSel.Parameters["@PorEmpresaIMP"].Value = documento.PorEmpresaIMP.Value;
                mySqlCommandSel.Parameters["@PorEmpresaUTI"].Value = documento.PorEmpresaUTI.Value;
                mySqlCommandSel.Parameters["@PorMultiplicadorPresup"].Value = documento.PorMultiplicadorPresup.Value;
                mySqlCommandSel.Parameters["@MultiplicadorActivoInd"].Value = documento.MultiplicadorActivoInd.Value;
                mySqlCommandSel.Parameters["@PorIVA"].Value = documento.PorIVA.Value;
                mySqlCommandSel.Parameters["@VersionPrevia"].Value = documento.VersionPrevia.Value;
                mySqlCommandSel.Parameters["@Version"].Value = documento.Version.Value;
                mySqlCommandSel.Parameters["@TipoRedondeo"].Value = documento.TipoRedondeo.Value;
                mySqlCommandSel.Parameters["@EquipoCantidadInd"].Value = documento.EquipoCantidadInd.Value;
                mySqlCommandSel.Parameters["@PersonalCantidadInd"].Value = documento.PersonalCantidadInd.Value;
                mySqlCommandSel.Parameters["@FechaFijaEntregas"].Value = documento.FechaFijaEntregas.Value;
                mySqlCommandSel.Parameters["@UsuarioFijaEntregas"].Value = documento.UsuarioFijaEntregas.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = documento.FechaInicio.Value;
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
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDocu_Add");
                throw exception;
            }
        }        

        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_pyProyectoDocu DAL_pyProyectoDocu_Get(int numeroDoc)
        {
            try
            {

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "   SELECT pyProyectoDocu.*   " +
                                                  "   FROM pyProyectoDocu   " +
                                                  "   INNER JOIN glDocumentoControl ON pyProyectoDocu.NumeroDoc = glDocumentoControl.NumeroDoc   " +
                                                  "   WHERE glDocumentoControl.EmpresaID = @EmpresaID AND  glDocumentoControl.NumeroDoc = @NumeroDoc ";


                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;


                DTO_pyProyectoDocu result = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_pyProyectoDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDocu_Get");
                throw exception;
            }
        }

    }
}
