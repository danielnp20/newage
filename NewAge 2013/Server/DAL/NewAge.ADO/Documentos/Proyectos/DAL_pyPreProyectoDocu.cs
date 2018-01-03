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
    public class DAL_pyPreProyectoDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyPreProyectoDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Agrega docu Solproyecto
        /// </summary>
        /// <param name="documento">documento</param>
        /// <returns></returns>
        public void DAL_pyPreProyectoDocu_Add(DTO_pyPreProyectoDocu documento)
        {        
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "   INSERT INTO pyPreProyectoDocu " +
                                           "      ([NumeroDoc]   " +
                                           "      ,[EmpresaID]   " +
                                           "      ,[ClienteID]   " +                                           
                                           "      ,[EmpresaNombre]   " +
                                           "      ,[SolicitanteEMP]   " +
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
                                           "      ,[Version]   " +
                                           "      ,[TipoRedondeo]   " +
                                           "      ,[EquipoCantidadInd]   " +
                                           "      ,[PersonalCantidadInd]   " +
                                           "      ,[PorcRteGarantia]   " +
                                           "      ,[RteGarantiaIvaInd]   " +
                                           "      ,[ValorAnticipoInicial]   " +
                                           "      ,[eg_faCliente]   " +
                                           "      ,[eg_pyContrato]   " +
                                           "      ,[eg_pyClaseProyecto])   " +
                                            "   VALUES " +
                                             "      (@NumeroDoc " +
                                             "      ,@EmpresaID " +
                                             "      ,@ClienteID " +
                                             "      ,@EmpresaNombre " +
                                             "      ,@SolicitanteEMP " +
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
                                             "      ,@Version   " +
                                             "      ,@TipoRedondeo   " +
                                             "      ,@EquipoCantidadInd   " +
                                             "      ,@PersonalCantidadInd   " +
                                             "      ,@PorcRteGarantia   " +
                                             "      ,@RteGarantiaIvaInd   " +
                                             "      ,@ValorAnticipoInicial  " +
                                             "      ,@eg_faCliente " +
                                             "      ,@eg_pyContrato " +
                                             "      ,@eg_pyClaseProyecto ) ";
                #endregion
                #region Creacion de parametros

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClaseServicioID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@SolicitanteEMP", SqlDbType.Char, UDT_UsuarioID.MaxLength);
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
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoRedondeo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EquipoCantidadInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PersonalCantidadInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PorcRteGarantia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RteGarantiaIvaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ValorAnticipoInicial", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_faCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyContrato", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyClaseProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = documento.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = documento.ClienteID.Value;
                mySqlCommandSel.Parameters["@ClaseServicioID"].Value = documento.ClaseServicioID.Value;
                mySqlCommandSel.Parameters["@EmpresaNombre"].Value = documento.EmpresaNombre.Value;
                mySqlCommandSel.Parameters["@SolicitanteEMP"].Value = documento.SolicitanteEMP.Value;
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
                mySqlCommandSel.Parameters["@Version"].Value = documento.Version.Value;
                mySqlCommandSel.Parameters["@TipoRedondeo"].Value = documento.TipoRedondeo.Value;
                mySqlCommandSel.Parameters["@EquipoCantidadInd"].Value = documento.EquipoCantidadInd.Value;
                mySqlCommandSel.Parameters["@PersonalCantidadInd"].Value = documento.PersonalCantidadInd.Value;
                mySqlCommandSel.Parameters["@PorcRteGarantia"].Value = documento.PorcRteGarantia.Value;
                mySqlCommandSel.Parameters["@RteGarantiaIvaInd"].Value = documento.RteGarantiaIvaInd.Value;
                mySqlCommandSel.Parameters["@ValorAnticipoInicial"].Value = documento.ValorAnticipoInicial.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDocu_Add");
                throw exception;
            }           
        }

        /// <summary>
        /// Actualiza docu Solproyecto
        /// </summary>
        /// <param name="documento">documento</param>
        /// <returns></returns>
        public void DAL_pyPreProyectoDocu_Upd(DTO_pyPreProyectoDocu documento)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                                "   UPDATE pyPreProyectoDocu SET					" +
                                                "   ClienteID			=	@ClienteID				" +
                                                "  ,EmpresaNombre		=	@EmpresaNombre			" +
                                                "  ,SolicitanteEMP		=	@SolicitanteEMP			" +
                                                "  ,ResponsableEMP		=	@ResponsableEMP			" +
                                                "  ,ResponsableCLI		=	@ResponsableCLI			" +
                                                "  ,ResponsableCorreo	=	@ResponsableCorreo		" +
                                                "  ,ResponsableTelefono	=	@ResponsableTelefono	" +
                                                "  ,ContratoID			=	@ContratoID				" +
                                                "  ,ClaseServicioID		=	@ClaseServicioID		" +                                              
                                                "  ,PropositoProyecto	=	@PropositoProyecto			" +
                                                "  ,TipoSolicitud		=	@TipoSolicitud			" +
                                                "  ,RecursosXTrabajoInd	=	@RecursosXTrabajoInd	" +
                                                "  ,DescripcionSOL		=	@DescripcionSOL			" +
                                                "  ,Licitacion		=	@Licitacion			" +
                                                "  ,TasaCambio		=	@TasaCambio			" +
                                                "  ,MonedaPresupuesto =	@MonedaPresupuesto			" +
                                                "  ,PorAjusteCambio =	@PorAjusteCambio			" +
                                                "  ,TipoAjusteCambio =	@TipoAjusteCambio			" +
                                                "  ,PorIPC =	@PorIPC			" +
                                                "  ,Jerarquia		=	@Jerarquia			" +
                                                "  ,APUIncluyeAIUInd	=	@APUIncluyeAIUInd	" +                                              
                                                "  ,PorClienteADM  =	@PorClienteADM " +
                                                "  ,PorClienteIMP  =	@PorClienteIMP " +
                                                "  ,PorClienteUTI  =	@PorClienteUTI " +
                                                "  ,PorEmpresaADM  =	@PorEmpresaADM " +
                                                "  ,PorEmpresaIMP  =	@PorEmpresaIMP " +
                                                "  ,PorEmpresaUTI  =	@PorEmpresaUTI " +
                                                "  ,PorMultiplicadorPresup		=	@PorMultiplicadorPresup	" +
                                                "  ,MultiplicadorActivoInd		=	@MultiplicadorActivoInd	" +
                                                "  ,PorIVA		=	@PorIVA	" +
                                                "  ,Version		=	@Version	" +
                                                "  ,TipoRedondeo		=	@TipoRedondeo	" +
                                                "  ,EquipoCantidadInd		=	@EquipoCantidadInd	" +
                                                "  ,PersonalCantidadInd		=	@PersonalCantidadInd	" +
                                                "  ,PorcRteGarantia	 =	@PorcRteGarantia	" +   
                                                "  ,RteGarantiaIvaInd	=	@RteGarantiaIvaInd	" +    
                                                "  ,ValorAnticipoInicial =	@ValorAnticipoInicial	" +   
                                                "   WHERE  NumeroDoc	=	@NumeroDoc				";

                #endregion
                #region Creacion de parametros

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaNombre", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@SolicitanteEMP", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableEMP", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableCLI", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableCorreo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ResponsableTelefono", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoID", SqlDbType.Char, UDT_ContratoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClaseServicioID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@PropositoProyecto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoSolicitud", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@RecursosXTrabajoInd", SqlDbType.Bit);
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
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoRedondeo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EquipoCantidadInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PersonalCantidadInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PorcRteGarantia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RteGarantiaIvaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ValorAnticipoInicial", SqlDbType.Decimal);

                #endregion
                #region Asignacion de valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = documento.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = documento.ClienteID.Value;
                mySqlCommandSel.Parameters["@EmpresaNombre"].Value = documento.EmpresaNombre.Value;
                mySqlCommandSel.Parameters["@SolicitanteEMP"].Value = documento.SolicitanteEMP.Value;
                mySqlCommandSel.Parameters["@ResponsableEMP"].Value = documento.ResponsableEMP.Value;
                mySqlCommandSel.Parameters["@ResponsableCLI"].Value = documento.ResponsableCLI.Value;
                mySqlCommandSel.Parameters["@ResponsableCorreo"].Value = documento.ResponsableCorreo.Value;
                mySqlCommandSel.Parameters["@ResponsableTelefono"].Value = documento.ResponsableTelefono.Value;
                mySqlCommandSel.Parameters["@ContratoID"].Value = documento.ContratoID.Value;
                mySqlCommandSel.Parameters["@ClaseServicioID"].Value = documento.ClaseServicioID.Value;
                mySqlCommandSel.Parameters["@TipoSolicitud"].Value = documento.TipoSolicitud.Value;
                mySqlCommandSel.Parameters["@PropositoProyecto"].Value = documento.PropositoProyecto.Value;
                mySqlCommandSel.Parameters["@RecursosXTrabajoInd"].Value = documento.RecursosXTrabajoInd.Value;
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
                mySqlCommandSel.Parameters["@Version"].Value = documento.Version.Value;              
                mySqlCommandSel.Parameters["@TipoRedondeo"].Value = documento.TipoRedondeo.Value;
                mySqlCommandSel.Parameters["@EquipoCantidadInd"].Value = documento.EquipoCantidadInd.Value;
                mySqlCommandSel.Parameters["@PersonalCantidadInd"].Value = documento.PersonalCantidadInd.Value;
                mySqlCommandSel.Parameters["@PorcRteGarantia"].Value = documento.PorcRteGarantia.Value;
                mySqlCommandSel.Parameters["@RteGarantiaIvaInd"].Value = documento.RteGarantiaIvaInd.Value;
                mySqlCommandSel.Parameters["@ValorAnticipoInicial"].Value = documento.ValorAnticipoInicial.Value;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDocu_Add");
                throw exception;
            }
        }        

        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_pyPreProyectoDocu DAL_pyPreProyectoDocu_Get(int numeroDoc)
        {
            try
            {

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "   SELECT pyPreProyectoDocu.*   " +
                                                  "   FROM pyPreProyectoDocu   " +
                                                  "   INNER JOIN glDocumentoControl ON pyPreProyectoDocu.NumeroDoc = glDocumentoControl.NumeroDoc   " +
                                                  "   WHERE glDocumentoControl.EmpresaID = @EmpresaID AND  glDocumentoControl.NumeroDoc = @NumeroDoc ";


                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;


                DTO_pyPreProyectoDocu result = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_pyPreProyectoDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Numero de documento</param>
        /// <param name="actividadFlujoID">Identificador del Documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl DAL_pyProyectoDocu_GetInternalDoc(int documentID, string idPrefijo, int documentoNro, string actividadFlujoID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "   select doc.*, usr.UsuarioID as UsuarioIDDesc    " +
                    "       from glDocumentoControl doc with(nolock) inner join seUsuario usr with(nolock) on doc.seUsuarioID = usr.ReplicaID    " +
                    "       inner join glActividadEstado  on doc.NumeroDoc = glActividadEstado.NumeroDoc    " +
                    "       where doc.EmpresaID = @EmpresaID      " +
                    "       and PrefijoID = @PrefijoID     " +
                    "       and DocumentoNro = @DocumentoNro     " +
                    "       and DocumentoTipo = @DocumentoTipo     " +
                    "       and DocumentoID=@DocumentoID    " +
                    "       and glActividadEstado.ActividadFlujoID = @ActividadFlujoID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PrefijoID"].Value = idPrefijo;
                mySqlCommand.Parameters["@DocumentoNro"].Value = documentoNro;
                mySqlCommand.Parameters["@DocumentoTipo"].Value = (byte)DocumentoTipo.DocInterno;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;

                DTO_glDocumentoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_glDocumentoControl(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyServicioDocu_GetInternalDoc");
                throw exception;
            }
        }
    }
}
