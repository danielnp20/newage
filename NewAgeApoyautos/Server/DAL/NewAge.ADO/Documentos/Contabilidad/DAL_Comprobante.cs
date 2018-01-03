using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Configuration;
using System.Reflection;
using SentenceTransformer;
using NewAge.DTO.Reportes;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_Contabilidad
    /// </summary>
    public class DAL_Comprobante : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_Comprobante(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region AuxiliarPre

        /// <summary>
        /// Valida si el auxiliar pre tiene datos para un periodo
        /// </summary>
        /// <returns></returns>
        public int DAL_ComprobantePre_HasData(bool isMensual, DateTime periodo)
        {
            SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
            mySqlCommand.Transaction = base.MySqlConnectionTx;

            mySqlCommand.CommandText = "SELECT Count(*) FROM coAuxiliarPre WHERE EmpresaID=@EmpresaID AND ";

            mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
            mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

            if (isMensual)
            {
                mySqlCommand.CommandText += "PeriodoID=@PeriodoID";

                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
            }
            else
            {
                mySqlCommand.CommandText += "YEAR(PeriodoID)=@Year";

                mySqlCommand.Parameters.Add("@Year", SqlDbType.Int);
                mySqlCommand.Parameters["@Year"].Value = periodo.Year;
            }

            int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
            return count;
        }

        /// <summary>
        /// Indica si hay un auxiliarPre
        /// </summary>
        /// <param name="empresaID">Codigo de la empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public bool DAL_ComprobantePre_Exists(int ctrlDocID, DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from coAuxiliarPre with(nolock) where EmpresaID = @EmpresaID " +
                    " and PeriodoID = @PeriodoID and ComprobanteID = @ComprobanteID and ComprobanteNro = @ComprobanteNro";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@ComprobanteID"].Value = comprobanteID;
                mySqlCommand.Parameters["@ComprobanteNro"].Value = compNro;

                int count = Convert.ToInt16(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_DAL_ComprobantePre_Exists");
                throw exception;
            }
        }

        /// <summary>
        /// trae la cantidad de veces que un comprobante existe
        /// </summary>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <returns>Retorna un entero con la cantidad de comprobantes que hay en aux con un comprobanteID </returns>
        public int DAL_ComprobanteExistsInAuxPre(string comprobanteID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT COUNT(ComprobanteID) FROM coAuxiliarPre WHERE ComprobanteID=@ComprobanteID and EmpresaID = @EmpresaID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ComprobanteID"].Value = comprobanteID;

                int count = Convert.ToInt16(mySqlCommand.ExecuteScalar());
                return count;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_DAL_ComprobanteExistsInAuxPre");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene un auxiliar con correspondiente IdentificadorTR y periodo anterior o igual correspondiente Periodo
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna un auxiliar</returns>
        public bool DAL_Comprobante_ExistByIdentificadorTR(DateTime periodo, long identTR, bool isPre)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                string aux = isPre ? "coAuxiliarPre" : "coAuxiliar";

                mySqlCommand.CommandText = "select count(*) from " + aux + " with(nolock) where EmpresaID = @EmpresaID " +
                    " and PeriodoID = @PeriodoID and IdentificadorTR = @IdentificadorTR";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;

                int count = Convert.ToInt16(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_ExistByIdentificadorTR");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega registros a la tabla de auxiliar
        /// </summary>
        /// <param name="isNew">Indica si el comprobante (pre) es nuevo o si se esta actualizando</param>
        /// <param name="comprobante">Comprobante contable</param>
        public void DAL_Comprobante_AgregarAuxiliar_Pre(bool isNew, DTO_Comprobante comprobante, DTO_glConceptoSaldo saldo, int index)
        {
            DTO_ComprobanteHeader header = comprobante.Header;
            DTO_ComprobanteFooter footer = comprobante.Footer[index];

            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Definicion de parametros y queries
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = header.PeriodoID.Value;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = header.ComprobanteID.Value;
                mySqlCommandSel.Parameters["@ComprobanteNro"].Value = header.ComprobanteNro.Value.Value;

                mySqlCommandSel.CommandText =
                    "INSERT INTO coAuxiliarPre(" +
                    "	EmpresaID,PeriodoID,ComprobanteID,ComprobanteNro,Fecha,NumeroDoc,MdaTransacc,MdaOrigen," +
                    "	TasaCambioBase,TasaCambioOtr,CuentaID,TerceroID,ProyectoID,CentroCostoID,LineaPresupuestoID," +
                    "	ConceptoCargoID,LugarGeograficoID,PrefijoCOM,DocumentoCOM,ActivoCOM," +
                    "	ConceptoSaldoID,IdentificadorTR,Descriptivo,vlrBaseML,vlrBaseME,vlrMdaLoc,vlrMdaExt,vlrMdaOtr," +
                    "   DatoAdd1,DatoAdd2,DatoAdd3,DatoAdd4,DatoAdd5,DatoAdd6,DatoAdd7,DatoAdd8,DatoAdd9,DatoAdd10," +
                    "   eg_coComprobante,eg_coPlanCuenta,eg_coTercero,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto," +
                    "   eg_coConceptoCargo,eg_glLugarGeografico,eg_glPrefijo,eg_glConceptoSaldo" +
                    ")VALUES(" +
                    "	@EmpresaID,@PeriodoID,@ComprobanteID,@ComprobanteNro,@Fecha,@NumeroDoc,@MdaTransacc,@MdaOrigen," +
                    "	@TasaCambioBase,@TasaCambioOtr,@CuentaID,@TerceroID,@ProyectoID,@CentroCostoID,@LineaPresupuestoID," +
                    "	@ConceptoCargoID,@LugarGeograficoID,@PrefijoCOM,@DocumentoCOM,@ActivoCOM," +
                    "	@ConceptoSaldoID,@IdentificadorTR,@Descriptivo,@vlrBaseML,@vlrBaseME,@vlrMdaLoc,@vlrMdaExt,@vlrMdaOtr," +
                    "   @DatoAdd1,@DatoAdd2,@DatoAdd3,@DatoAdd4,@DatoAdd5,@DatoAdd6,@DatoAdd7,@DatoAdd8,@DatoAdd9,@DatoAdd10," +
                    "   @eg_coComprobante,@eg_coPlanCuenta,@eg_coTercero,@eg_coProyecto,@eg_coCentroCosto,@eg_plLineaPresupuesto," +
                    "   @eg_coConceptoCargo,@eg_glLugarGeografico,@eg_glPrefijo,@eg_glConceptoSaldo" +
                    ")";

                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@MdaTransacc", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@MdaOrigen", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TasaCambioBase", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TasaCambioOtr", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LugarGeograficoID", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@PrefijoCOM", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoCOM", SqlDbType.VarChar, 20);
                mySqlCommandSel.Parameters.Add("@ActivoCOM", SqlDbType.VarChar, 20);
                mySqlCommandSel.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommandSel.Parameters.Add("@vlrBaseML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrBaseME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrMdaLoc", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrMdaExt", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrMdaOtr", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd6", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd7", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd8", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd9", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd10", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@eg_coComprobante", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glConceptoSaldo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion

                //Asigna los valores faltantes del header
                mySqlCommandSel.Parameters["@Fecha"].Value = header.Fecha.Value.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = header.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@MdaTransacc"].Value = header.MdaTransacc.Value;
                mySqlCommandSel.Parameters["@MdaOrigen"].Value = header.MdaOrigen.Value;

                #region Asignacion valores
                mySqlCommandSel.Parameters["@TasaCambioBase"].Value = footer.TasaCambio.Value.Value;
                mySqlCommandSel.Parameters["@TasaCambioOtr"].Value = footer.TasaCambio.Value.Value;

                mySqlCommandSel.Parameters["@CuentaID"].Value = footer.CuentaID.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = footer.TerceroID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = footer.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = footer.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = footer.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@ConceptoCargoID"].Value = footer.ConceptoCargoID.Value;
                mySqlCommandSel.Parameters["@PrefijoCOM"].Value = footer.PrefijoCOM.Value;
                mySqlCommandSel.Parameters["@DocumentoCOM"].Value = footer.DocumentoCOM.Value;
                mySqlCommandSel.Parameters["@ActivoCOM"].Value = footer.ActivoCOM.Value;
                mySqlCommandSel.Parameters["@ConceptoSaldoID"].Value = footer.ConceptoSaldoID.Value;
                mySqlCommandSel.Parameters["@IdentificadorTR"].Value = footer.IdentificadorTR.Value.Value;
                mySqlCommandSel.Parameters["@Descriptivo"].Value = footer.Descriptivo.Value;
                mySqlCommandSel.Parameters["@vlrBaseML"].Value = footer.vlrBaseML.Value.Value;
                mySqlCommandSel.Parameters["@vlrBaseME"].Value = footer.vlrBaseME.Value.Value;
                mySqlCommandSel.Parameters["@vlrMdaLoc"].Value = footer.vlrMdaLoc.Value.Value;
                mySqlCommandSel.Parameters["@vlrMdaExt"].Value = footer.vlrMdaExt.Value.Value;
                mySqlCommandSel.Parameters["@eg_coComprobante"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobante, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glConceptoSaldo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glConceptoSaldo, this.Empresa, egCtrl);

                #endregion
                #region Columnas Nullable
                //Revisa si hay datos para el lugar geografico
                if (footer.LugarGeograficoID != null && !string.IsNullOrWhiteSpace(footer.LugarGeograficoID.Value))
                    mySqlCommandSel.Parameters["@LugarGeograficoID"].Value = footer.LugarGeograficoID.Value;
                else
                    mySqlCommandSel.Parameters["@LugarGeograficoID"].Value = DBNull.Value;

                //Revisa si hay datos para el valor de otra moneda
                if (footer.vlrMdaOtr != null && footer.vlrMdaOtr.Value.HasValue)
                    mySqlCommandSel.Parameters["@vlrMdaOtr"].Value = footer.vlrMdaOtr.Value.Value;
                else
                    mySqlCommandSel.Parameters["@vlrMdaOtr"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato1
                if (footer.DatoAdd1 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd1.Value))
                    mySqlCommandSel.Parameters["@DatoAdd1"].Value = footer.DatoAdd1.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd1"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato2
                if (footer.DatoAdd2 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd2.Value))
                    mySqlCommandSel.Parameters["@DatoAdd2"].Value = footer.DatoAdd2.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd2"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato3
                if (footer.DatoAdd3 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd3.Value))
                    mySqlCommandSel.Parameters["@DatoAdd3"].Value = footer.DatoAdd3.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd3"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato4
                if (footer.DatoAdd4 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd4.Value))
                    mySqlCommandSel.Parameters["@DatoAdd4"].Value = footer.DatoAdd4.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd4"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato5
                if (footer.DatoAdd5 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd5.Value))
                    mySqlCommandSel.Parameters["@DatoAdd5"].Value = footer.DatoAdd5.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd5"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato6
                if (footer.DatoAdd6 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd6.Value))
                    mySqlCommandSel.Parameters["@DatoAdd6"].Value = footer.DatoAdd6.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd6"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato7
                if (footer.DatoAdd7 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd7.Value))
                    mySqlCommandSel.Parameters["@DatoAdd7"].Value = footer.DatoAdd7.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd7"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato8
                if (footer.DatoAdd8 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd8.Value))
                    mySqlCommandSel.Parameters["@DatoAdd8"].Value = footer.DatoAdd8.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd8"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato9
                if (footer.DatoAdd9 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd9.Value))
                    mySqlCommandSel.Parameters["@DatoAdd9"].Value = footer.DatoAdd9.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd9"].Value = DBNull.Value;

                //Revisa si hay datos para campos adicionales Dato10
                if (footer.DatoAdd10 != null && !string.IsNullOrWhiteSpace(footer.DatoAdd10.Value))
                    mySqlCommandSel.Parameters["@DatoAdd10"].Value = footer.DatoAdd10.Value;
                else
                    mySqlCommandSel.Parameters["@DatoAdd10"].Value = DBNull.Value;

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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_AgregarAuxiliar_Pre");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega registros a la tabla de auxiliar
        /// </summary>
        /// <param name="comprobante">Comprobante contable</param>
        public void DAL_Comprobante_BorrarAuxiliar_Pre(DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = comprobanteID;
                mySqlCommandSel.Parameters["@ComprobanteNro"].Value = compNro;

                mySqlCommandSel.CommandText = "DELETE FROM coAuxiliarPre  where EmpresaID = @EmpresaID " +
                " and PeriodoID = @PeriodoID and ComprobanteID = @ComprobanteID and ComprobanteNro = @ComprobanteNro";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteCompr, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_BorrarAuxiliar_Pre");
                throw exception; ;
            }
        }

        /// <summary>
        /// Trae un listado de comprobantes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_ComprobanteAprobacion> DAL_ComprobantePre_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID, string usuarioID)
        {
            try
            {
                List<DTO_ComprobanteAprobacion> result = new List<DTO_ComprobanteAprobacion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct ctrl.NumeroDoc, PeriodoDoc as PeriodoID, ComprobanteID, ComprobanteIDNro as ComprobanteNro, ctrl.DocumentoID, DocumentoNro, usr.UsuarioID " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "   inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID " +
                    "   inner join glActividadPermiso perm with(nolock) on Perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "       and perm.UsuarioID = @UsuarioID and Perm.AreaFuncionalID = Ctrl.AreaFuncionalID " +
                    "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and perm.ActividadFlujoID = @ActividadFlujoID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ComprobanteAprobacion dto = new DTO_ComprobanteAprobacion(dr);
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Observacion.Value = string.Empty;
                    result.Add(dto);
                }
                dr.Close();

                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                foreach (DTO_ComprobanteAprobacion dto in result)
                {
                    mySqlCommand.Parameters["@PeriodoID"].Value = dto.PeriodoID.Value.Value;
                    mySqlCommand.Parameters["@ComprobanteID"].Value = dto.ComprobanteID.Value;
                    mySqlCommand.Parameters["@ComprobanteNro"].Value = dto.ComprobanteNro.Value.Value;

                    mySqlCommand.CommandText =
                        "select TOP 1 Descriptivo " +
                        "from coAuxiliarPre with(nolock) " +
                        "where  " +
                        "	EmpresaID = @EmpresaID AND " +
                        "	PeriodoID = @PeriodoID AND " +
                        "	ComprobanteID = @ComprobanteID AND " +
                        "	ComprobanteNro = @ComprobanteNro ";

                    object des = mySqlCommand.ExecuteScalar();
                    dto.Descriptivo.Value = des == null ? string.Empty : des.ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_DAL_ComprobantePre_GetPendientesByModulo");
                throw exception;
            }
        }

        #endregion

        #region Auxiliar

        /// <summary>
        /// Actualiza los consecutivos del comprobante
        /// </summary>
        /// <param name="isPre">Verifica si la actualizacion se esta haciendo en el auxiliar Preliminar o en el real</param>
        /// <param name="numeroDoc">Identificador del documento control</param>
        /// <param name="comprobanteNro">Consecutivo del comprobante</param>
        public void DAL_Comprobante_UpdateConsecutivo(int numeroDoc, int comprobanteNro, bool isPre, string comprobanteID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string table = isPre ? "coAuxiliarPre" : "coAuxiliar";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@ComprobanteNro"].Value = comprobanteNro;

                mySqlCommand.CommandText = "UPDATE " + table + " SET ComprobanteNro = @ComprobanteNro WHERE NumeroDoc=@NumeroDoc";
                
                if (!string.IsNullOrEmpty(comprobanteID))
                {
                    mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char);
                    mySqlCommand.Parameters["@ComprobanteID"].Value = comprobanteID;
                    mySqlCommand.CommandText = mySqlCommand.CommandText + " AND ComprobanteID = @ComprobanteID";                    
                }

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_UpdateConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el numero de registros de un comprobante
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public int DAL_Comprobante_Count(bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, DTO_glConsulta consulta = null)
        {
            try
            {
                string query = string.Empty;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string from = isPre ? "coAuxiliarPre" : "coAuxiliar";
                query = "select * from " + from + " with(nolock) where EmpresaID = @EmpresaID" +
                    " and PeriodoID = @PeriodoID and ComprobanteID = @ComprobanteID and ComprobanteNro = @ComprobanteNro";

                if (!allData)
                    mySqlCommand.CommandText += " and (DatoAdd4 is NULL or DatoAdd4 = '" + AuxiliarDatoAdd4.Contrapartida.ToString() + "')";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@ComprobanteID"].Value = comprobanteID;
                mySqlCommand.Parameters["@ComprobanteNro"].Value = compNro;

                string where = (consulta == null || consulta.Filtros == null) ? string.Empty : Transformer.WhereSql(consulta.Filtros, typeof(DTO_ComprobanteFooter));

                if (!string.IsNullOrWhiteSpace(where))
                    query = "SELECT COUNT(*) FROM (" + query + ") resultTable WHERE (" + where + ") ";
                else
                    query = "SELECT COUNT(*) FROM (" + query + ") resultTable";

                mySqlCommand.CommandText = query;

                int res = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_DAL_Comprobante_Count");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante DAL_Comprobante_Get(bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, int? pageSize, int? pageNum, DTO_glConsulta consulta = null)
        {
            try
            {
                int ini = 0;
                int fin = 0;
                string query = string.Empty;

                if (pageNum.HasValue && pageSize.HasValue)
                {
                    ini = (pageNum.Value - 1) * pageSize.Value + 1;
                    fin = pageNum.Value * pageSize.Value;
                }

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string from = isPre ? "coAuxiliarPre" : "coAuxiliar";
                query = "select * from " + from + " with(nolock) where EmpresaID = @EmpresaID" +
                    " and PeriodoID = @PeriodoID and ComprobanteID = @ComprobanteID and ComprobanteNro = @ComprobanteNro";

                if (!allData)
                    query += " and " +
                        "( " +
                        "   DatoAdd4 is NULL or" +
                        "   (" +
                        "       DatoAdd4 <> '" + AuxiliarDatoAdd4.Contrapartida.ToString() + "' and" +
                        "       DatoAdd4 <> '" + AuxiliarDatoAdd4.AjEnCambio.ToString() + "' and" +
                        "       DatoAdd4 <> '" + AuxiliarDatoAdd4.AjEnCambioContra.ToString() + "'" +
                        "   )" +
                        ")";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@ComprobanteID"].Value = comprobanteID;
                mySqlCommand.Parameters["@ComprobanteNro"].Value = compNro;

                if (pageNum.HasValue && pageSize.HasValue)
                {
                    query = "SELECT ROW_NUMBER()Over(Order by fecha) As RowNum,* FROM (" + query + ") resultTable ";
                    query = "SELECT * FROM (" + query + ") tempRes WHERE RowNum BETWEEN " + ini + " AND " + fin;
                }

                mySqlCommand.CommandText = query;
                DTO_Comprobante comprobante = null;
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                bool isFirst = true;
                int index = 0;
                while (dr.Read())
                {
                    if (isFirst)
                    {
                        comprobante = new DTO_Comprobante();
                        header = new DTO_ComprobanteHeader(dr);
                        isFirst = false;
                    }

                    DTO_ComprobanteFooter detail = new DTO_ComprobanteFooter(dr);
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();

                if (!isFirst)
                    comprobante.AddData(header, footer);

                return comprobante;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_DAL_Comprobante_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si se existe un documento en un periodo y un auxiliar relacionado
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna el estado del ajuste</returns>
        public EstadoAjuste DAL_Comprobante_HasDocument(int documentoID, DateTime periodo, string libroID)
        {
            try
            {
                EstadoAjuste result = EstadoAjuste.NoData;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;

                mySqlCommand.CommandText =
                    "select Top(1) Estado " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "	inner join coAuxiliar aux with(nolock) on ctrl.NumeroDoc = aux.NumeroDoc " +
                    "	left join coCuentaSaldo saldo with(nolock) on ctrl.NumeroDoc = saldo.IdentificadorTR " +
                    "       and saldo.BalanceTipoID = @BalanceTipoID " +
                    "where ctrl.EmpresaID = @EmpresaID and ctrl.PeriodoDoc = @PeriodoDoc and " +
                    "	ctrl.DocumentoID = @DocumentoID " +
                    "order by ctrl.NumeroDoc desc";

                object res = mySqlCommand.ExecuteScalar();
                if (res != null)
                {
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), res.ToString());
                    if (estado == EstadoDocControl.Aprobado)
                        result = EstadoAjuste.Aprobado;
                    else
                        result = EstadoAjuste.Preliminar;
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_HasAjusteEnCambio");
                throw exception;
            }
        }

        /// <summary>
        /// Genera un numero de comprobante
        /// </summary>
        /// <param name="comprobante">Identificador del comprobante</param>
        /// <param name="prefijoID">Identificador del prefijo del documento que se esta trabajando</param>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="DocumentoNro">Numero de documento (DocumentoNro) de glDocumentoControl</param>
        /// <param name="onlyGet">Indica si solo puede traer la info o tambien crear un nuevo numero</param>
        /// <returns>Retorna en numero de comprobante asignado</returns>
        public int DAL_Comprobante_GenerarNumeroComprobante(DTO_coComprobante comprobante, string prefijoID, DateTime periodo, int DocumentoNro, bool onlyGet = false)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                if (comprobante.TipoConsec.Value.Value == 1 || comprobante.TipoConsec.Value.Value == 2)
                {
                    #region Si el tipo del consecutivo es anual o mensual

                    DateTime nPeriodo = comprobante.TipoConsec.Value.Value == 1 ? new DateTime(periodo.Year, 1, 1) : new DateTime(periodo.Year, periodo.Month, 1);
                    if (periodo.Day == 2)
                        nPeriodo = new DateTime(nPeriodo.Year, nPeriodo.Month,2);
                    mySqlCommandSel.CommandText = "select CompNumero from coComprConsecutivo where EmpresaID = @EmpresaID " +
                        " and ComprobanteID = @ComprobanteID and PeriodoID = @PeriodoID";

                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);

                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@ComprobanteID"].Value = comprobante.ID.Value;
                    mySqlCommandSel.Parameters["@PeriodoID"].Value = nPeriodo;

                    var consObj = mySqlCommandSel.ExecuteScalar();
                    if (onlyGet)
                    {
                        if (consObj != null)
                            return Convert.ToInt32(consObj);
                        else
                            return 0;
                    }

                    mySqlCommandSel.Parameters.Add("@CompNumero", SqlDbType.Int);

                    if (consObj != null)
                    {
                        //Si existe lo asigna y actualiza el consecutivo
                        int res = Convert.ToInt32(consObj);
                        res = res + 1;

                        mySqlCommandSel.CommandText = "Update coComprConsecutivo set CompNumero = @CompNumero" +
                            " where EmpresaID = @EmpresaID and ComprobanteID = @ComprobanteID and PeriodoID = @PeriodoID";

                        mySqlCommandSel.Parameters["@CompNumero"].Value = res;
                        mySqlCommandSel.ExecuteNonQuery();

                        return res;
                    }
                    else
                    {
                        //Si el registro no existe lo crea y asigan el primer valor
                        mySqlCommandSel.CommandText = "Insert Into coComprConsecutivo (EmpresaID, ComprobanteID, PeriodoID, CompNumero)" +
                         " values (@EmpresaID, @ComprobanteID, @PeriodoID, @CompNumero)";

                        mySqlCommandSel.Parameters["@CompNumero"].Value = 1;
                        mySqlCommandSel.ExecuteNonQuery();

                        return 1;
                    }
                    #endregion
                }
                else if (comprobante.TipoConsec.Value.Value == 3)
                {
                    //Si el comprobante se maneja por transaccion
                    return DocumentoNro;
                }

                return 0;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GenerarNumeroComprobante");
                throw exception;
            }
        }

        /// <summary>
        /// Genera un numero de comprobante
        /// </summary>
        /// <param name="comprobante">Identificador del comprobante</param>
        /// <param name="prefijoID">Identificador del prefijo del documento que se esta trabajando</param>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="DocumentoNro">Numero de documento (DocumentoNro) de glDocumentoControl</param>
        /// <returns>Retorna en numero de comprobante asignado</returns>
        public void DAL_Comprobante_AgregarConsecutivoMigracion(string compID, DateTime periodo, int consecutivo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "select count(*) from coComprConsecutivo where EmpresaID = @EmpresaID " +
                    " and ComprobanteID = @ComprobanteID and PeriodoID = @PeriodoID";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = compID;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;

                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                mySqlCommandSel.Parameters.Add("@CompNumero", SqlDbType.Int);
                mySqlCommandSel.Parameters["@CompNumero"].Value = consecutivo;

                if (count > 0)
                    mySqlCommandSel.CommandText = "Update coComprConsecutivo set CompNumero = @CompNumero" +
                        " where EmpresaID = @EmpresaID and ComprobanteID = @ComprobanteID and PeriodoID = @PeriodoID";
                else
                    mySqlCommandSel.CommandText = "Insert Into coComprConsecutivo (EmpresaID, ComprobanteID, PeriodoID, CompNumero)" +
                        " values (@EmpresaID, @ComprobanteID, @PeriodoID, @CompNumero)";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_AgregarConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Borra informacio de coAuxiliar
        /// </summary>
        /// <param name="periodo">periodo</param>
        /// <param name="comprobanteID">comprobanteID</param>
        /// <param name="compNro">comprobanteNro</param>
        public void DAL_Comprobante_BorrarAuxiliar(DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = comprobanteID;
                mySqlCommandSel.Parameters["@ComprobanteNro"].Value = compNro;

                mySqlCommandSel.CommandText = "DELETE FROM coAuxiliar where EmpresaID = @EmpresaID " +
                " and PeriodoID = @PeriodoID and ComprobanteID = @ComprobanteID and ComprobanteNro = @ComprobanteNro";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteCompr, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_BorrarAuxiliar_Pre");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega registros a la tabla de auxiliar
        /// </summary>
        /// <param name="comprobante">Comprobante contable</param>
        public void DAL_Comprobante_AgregarAuxiliar(DTO_Comprobante comprobante)
        {
            DTO_ComprobanteHeader header = comprobante.Header;
            List<DTO_ComprobanteFooter> footer = comprobante.Footer;

            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO coAuxiliar(" +
                    "	EmpresaID,PeriodoID,ComprobanteID,ComprobanteNro,Fecha,NumeroDoc,MdaTransacc,MdaOrigen," +
                    "	TasaCambioBase,TasaCambioOtr,CuentaID,CuentaAlternaID,TerceroID,ProyectoID,CentroCostoID,LineaPresupuestoID," +
                    "	ConceptoCargoID,LugarGeograficoID,PrefijoCOM,DocumentoCOM,ActivoCOM," +
                    "	ConceptoSaldoID,IdentificadorTR,Descriptivo,vlrBaseML,vlrBaseME,vlrMdaLoc,vlrMdaExt,vlrMdaOtr," +
                    "   DatoAdd1,DatoAdd2,DatoAdd3,DatoAdd4,DatoAdd5,DatoAdd6,DatoAdd7,DatoAdd8,DatoAdd9,DatoAdd10," +
                    "   eg_coComprobante,eg_coPlanCuenta,eg_coTercero,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto," +
                    "   eg_coConceptoCargo,eg_glLugarGeografico,eg_glPrefijo,eg_glConceptoSaldo" +
                    ")VALUES(" +
                    "	@EmpresaID,@PeriodoID,@ComprobanteID,@ComprobanteNro,@Fecha,@NumeroDoc,@MdaTransacc,@MdaOrigen," +
                    "	@TasaCambioBase,@TasaCambioOtr,@CuentaID,NULL,@TerceroID,@ProyectoID,@CentroCostoID,@LineaPresupuestoID," +
                    "	@ConceptoCargoID,@LugarGeograficoID,@PrefijoCOM,@DocumentoCOM,@ActivoCOM," +
                    "	@ConceptoSaldoID,@IdentificadorTR,@Descriptivo,@vlrBaseML,@vlrBaseME,@vlrMdaLoc,@vlrMdaExt,@vlrMdaOtr," +
                    "   @DatoAdd1,@DatoAdd2,@DatoAdd3,@DatoAdd4,@DatoAdd5,@DatoAdd6,@DatoAdd7,@DatoAdd8,@DatoAdd9,@DatoAdd10," +
                    "   @eg_coComprobante,@eg_coPlanCuenta,@eg_coTercero,@eg_coProyecto,@eg_coCentroCosto,@eg_plLineaPresupuesto," +
                    "   @eg_coConceptoCargo,@eg_glLugarGeografico,@eg_glPrefijo,@eg_glConceptoSaldo" +
                    ")";
                #endregion
                #region parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteNro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@MdaTransacc", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@MdaOrigen", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TasaCambioBase", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TasaCambioOtr", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LugarGeograficoID", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@PrefijoCOM", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoCOM", SqlDbType.VarChar, 20);
                mySqlCommandSel.Parameters.Add("@ActivoCOM", SqlDbType.VarChar, 20);
                mySqlCommandSel.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommandSel.Parameters.Add("@vlrBaseML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrBaseME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrMdaLoc", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrMdaExt", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrMdaOtr", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd6", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd7", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd8", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd9", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd10", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@eg_coComprobante", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glConceptoSaldo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna parametros comunes
                //Header
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = header.PeriodoID.Value;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = header.ComprobanteID.Value;
                mySqlCommandSel.Parameters["@ComprobanteNro"].Value = header.ComprobanteNro.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = header.Fecha.Value.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = header.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@MdaTransacc"].Value = header.MdaTransacc.Value;
                mySqlCommandSel.Parameters["@MdaOrigen"].Value = header.MdaOrigen.Value;
                //Valores fijos
                mySqlCommandSel.Parameters["@eg_coComprobante"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobante, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glConceptoSaldo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glConceptoSaldo, this.Empresa, egCtrl);
                #endregion
                foreach (DTO_ComprobanteFooter det in footer)
                {
                    #region Parametros footer
                    if (det.TasaCambio.Value == null)
                    {
                        mySqlCommandSel.Parameters["@TasaCambioBase"].Value = header.TasaCambioBase.Value.Value;
                        mySqlCommandSel.Parameters["@TasaCambioOtr"].Value = header.TasaCambioBase.Value.Value;
                    }
                    else
                    {
                        mySqlCommandSel.Parameters["@TasaCambioBase"].Value = det.TasaCambio.Value.Value;
                        mySqlCommandSel.Parameters["@TasaCambioOtr"].Value = det.TasaCambio.Value.Value;
                    }

                    mySqlCommandSel.Parameters["@CuentaID"].Value = det.CuentaID.Value;
                    mySqlCommandSel.Parameters["@TerceroID"].Value = det.TerceroID.Value;
                    mySqlCommandSel.Parameters["@ProyectoID"].Value = det.ProyectoID.Value;
                    mySqlCommandSel.Parameters["@CentroCostoID"].Value = det.CentroCostoID.Value;
                    mySqlCommandSel.Parameters["@LugarGeograficoID"].Value = det.LugarGeograficoID.Value;
                    mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = det.LineaPresupuestoID.Value;
                    mySqlCommandSel.Parameters["@ConceptoCargoID"].Value = det.ConceptoCargoID.Value;
                    mySqlCommandSel.Parameters["@PrefijoCOM"].Value = det.PrefijoCOM.Value;
                    mySqlCommandSel.Parameters["@DocumentoCOM"].Value = det.DocumentoCOM.Value;
                    mySqlCommandSel.Parameters["@ActivoCOM"].Value = det.ActivoCOM.Value;
                    mySqlCommandSel.Parameters["@ConceptoSaldoID"].Value = det.ConceptoSaldoID.Value;
                    mySqlCommandSel.Parameters["@IdentificadorTR"].Value = det.IdentificadorTR.Value.Value;
                    mySqlCommandSel.Parameters["@Descriptivo"].Value = det.Descriptivo.Value;
                    mySqlCommandSel.Parameters["@vlrBaseML"].Value = det.vlrBaseML.Value.Value;
                    mySqlCommandSel.Parameters["@vlrBaseME"].Value = det.vlrBaseME.Value.Value;
                    mySqlCommandSel.Parameters["@vlrMdaLoc"].Value = det.vlrMdaLoc.Value.Value;
                    mySqlCommandSel.Parameters["@vlrMdaExt"].Value = det.vlrMdaExt.Value.Value;
                    #endregion
                    #region Columnas Nullable

                    //Revisa si hay datos para el valor de otra moneda
                    if (det.vlrMdaOtr != null && det.vlrMdaOtr.Value.HasValue)
                        mySqlCommandSel.Parameters["@vlrMdaOtr"].Value = det.vlrMdaOtr.Value.Value;
                    else
                        mySqlCommandSel.Parameters["@vlrMdaOtr"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato1
                    if (det.DatoAdd1 != null && !string.IsNullOrWhiteSpace(det.DatoAdd1.Value))
                        mySqlCommandSel.Parameters["@DatoAdd1"].Value = det.DatoAdd1.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd1"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato2
                    if (det.DatoAdd2 != null && !string.IsNullOrWhiteSpace(det.DatoAdd2.Value))
                        mySqlCommandSel.Parameters["@DatoAdd2"].Value = det.DatoAdd2.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd2"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato3
                    if (det.DatoAdd3 != null && !string.IsNullOrWhiteSpace(det.DatoAdd3.Value))
                        mySqlCommandSel.Parameters["@DatoAdd3"].Value = det.DatoAdd3.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd3"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato4
                    if (det.DatoAdd4 != null && !string.IsNullOrWhiteSpace(det.DatoAdd4.Value))
                        mySqlCommandSel.Parameters["@DatoAdd4"].Value = det.DatoAdd4.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd4"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato5
                    if (det.DatoAdd5 != null && !string.IsNullOrWhiteSpace(det.DatoAdd5.Value))
                        mySqlCommandSel.Parameters["@DatoAdd5"].Value = det.DatoAdd5.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd5"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato6
                    if (det.DatoAdd6 != null && !string.IsNullOrWhiteSpace(det.DatoAdd6.Value))
                        mySqlCommandSel.Parameters["@DatoAdd6"].Value = det.DatoAdd6.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd6"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato7
                    if (det.DatoAdd7 != null && !string.IsNullOrWhiteSpace(det.DatoAdd7.Value))
                        mySqlCommandSel.Parameters["@DatoAdd7"].Value = det.DatoAdd7.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd7"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato8
                    if (det.DatoAdd8 != null && !string.IsNullOrWhiteSpace(det.DatoAdd8.Value))
                        mySqlCommandSel.Parameters["@DatoAdd8"].Value = det.DatoAdd8.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd8"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato9
                    if (det.DatoAdd9 != null && !string.IsNullOrWhiteSpace(det.DatoAdd9.Value))
                        mySqlCommandSel.Parameters["@DatoAdd9"].Value = det.DatoAdd9.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd9"].Value = DBNull.Value;

                    //Revisa si hay datos para campos adicionales Dato10
                    if (det.DatoAdd10 != null && !string.IsNullOrWhiteSpace(det.DatoAdd10.Value))
                        mySqlCommandSel.Parameters["@DatoAdd10"].Value = det.DatoAdd10.Value;
                    else
                        mySqlCommandSel.Parameters["@DatoAdd10"].Value = DBNull.Value;

                    #endregion

                    foreach (SqlParameter param in mySqlCommandSel.Parameters)
                    {
                        if (param.Direction.Equals(ParameterDirection.Input))
                        {
                            if (param.Value == null)
                                param.Value = DBNull.Value;
                        }
                    }

                    mySqlCommandSel.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_AgregarAuxiliar");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un comprobante manual de coAuxiliarPre a coAuxiliar
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <param name="obs">Observacion sobre el documento</param>
        /// <param name="userId">Usuario que realiza la transaccion</param>
        /// <returns>Retorna el id de la bitacora que actualizo el estado del documento</returns>
        public void DAL_Comprobante_AgregarAuxFromPre(DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "insert into coAuxiliar(" +
                    "	    EmpresaID,PeriodoID,ComprobanteID,ComprobanteNro,Fecha,NumeroDoc,MdaTransacc,MdaOrigen," +
                    "	    TasaCambioBase,TasaCambioOtr,CuentaID, CuentaAlternaID,TerceroID,ProyectoID,CentroCostoID,LineaPresupuestoID," +
                    "	    ConceptoCargoID,LugarGeograficoID,PrefijoCOM,DocumentoCOM,ActivoCOM,ConceptoSaldoID,IdentificadorTR," +
                    "	    Descriptivo,vlrBaseML,vlrBaseME,vlrMdaLoc,vlrMdaExt,vlrMdaOtr," +
                    "       DatoAdd1,DatoAdd2,DatoAdd3,DatoAdd4,DatoAdd5,DatoAdd6,DatoAdd7,DatoAdd8,DatoAdd9,DatoAdd10," +
                    "       eg_coComprobante,eg_coPlanCuenta,eg_coTercero,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto," +
                    "       eg_coConceptoCargo,eg_glLugarGeografico,eg_glPrefijo,eg_glConceptoSaldo" +
                    ")" +
                    "    select " +
                    "        aux.EmpresaID,aux.PeriodoID,aux.ComprobanteID,aux.ComprobanteNro,aux.Fecha," +
                    "        ctrl.NumeroDoc as NumeroDoc,aux.MdaTransacc,aux.MdaOrigen,aux.TasaCambioBase,aux.TasaCambioOtr,aux.CuentaID,NULL," +
                    "        aux.TerceroID,aux.ProyectoID,aux.CentroCostoID,aux.LineaPresupuestoID,aux.ConceptoCargoID," +
                    "        aux.LugarGeograficoID,aux.PrefijoCOM,aux.DocumentoCOM,aux.ActivoCOM,aux.ConceptoSaldoID," +
                    "        aux.IdentificadorTR,aux.Descriptivo,aux.vlrBaseML,aux.vlrBaseME,aux.vlrMdaLoc,aux.vlrMdaExt," +
                    "        aux.vlrMdaOtr,aux.DatoAdd1,aux.DatoAdd2,aux.DatoAdd3,aux.DatoAdd4,aux.DatoAdd5,aux.DatoAdd6,aux.DatoAdd7,aux.DatoAdd8,aux.DatoAdd9,aux.DatoAdd10, aux.eg_coComprobante," +
                    "        aux.eg_coPlanCuenta,aux.eg_coTercero,aux.eg_coProyecto,aux.eg_coCentroCosto, aux.eg_plLineaPresupuesto," +
                    "        aux.eg_coConceptoCargo,aux.eg_glLugarGeografico,aux.eg_glPrefijo,aux.eg_glConceptoSaldo " +
                    "    from coAuxiliarPre aux with(nolock) inner join glDocumentoControl ctrl with(nolock) on aux.NumeroDoc = ctrl.NumeroDoc" +
                    "    where aux.EmpresaID = @EmpresaID" +
                    "        and aux.PeriodoID = @PeriodoID" +
                    "        and aux.ComprobanteID = @ComprobanteID" +
                    "        and aux.ComprobanteNro = @ComprobanteNro";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@ComprobanteID"].Value = comprobanteID;
                mySqlCommand.Parameters["@ComprobanteNro"].Value = compNro;

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddCompr, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_AgregarAuxiliarAprobado");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene un auxiliar con correspondiente IdentificadorTR y periodo anterior o igual correspondiente Periodo
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_BitacoraSaldo> DAL_Comprobante_GetByIdentificadorTR(DateTime periodo, long identTR)
        {
            try
            {
                List<DTO_BitacoraSaldo> result = new List<DTO_BitacoraSaldo>();

                string query = string.Empty;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                query = 
                    "select * from coAuxiliar with(nolock) where EmpresaID = @EmpresaID" +
                    " and PeriodoID <= @PeriodoID and IdentificadorTR = @IdentificadorTR ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = identTR;

                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_BitacoraSaldo dto = new DTO_BitacoraSaldo(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetByIdentificadorTR");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene toda la info de auxiliares de acuerdo al numeroDoc (glDocumentoControl)
        /// </summary>
        /// <param name="numeroDoc">Periodo</param>
        /// <returns>Retorna los registros de un auxiliar</returns>
        public List<DTO_ComprobanteFooter> DAL_Comprobante_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                List<DTO_ComprobanteFooter> result = new List<DTO_ComprobanteFooter>();

                string query = string.Empty;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                query =
                    "select * from coAuxiliar with(nolock) where EmpresaID = @EmpresaID and NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter(dr);
                    result.Add(footer);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de comprobantes para realizar la distribucion
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="origen">Filtro de origen</param>
        /// <param name="excls">Lista de exclusiones</param>
        /// <returns>Retiorna la lista de auxiliares</returns>
        public DTO_Comprobante DAL_Comprobante_GetForDistribucion(DateTime periodo, DTO_coCompDistribuyeTabla origen, List<DTO_coCompDistribuyeExcluye> excls)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros generales

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);

                #endregion

                bool isFirst = true;
                string query = string.Empty;
                string queryGeneral = "select * from coauxiliar with(nolock) "; 
                string whereGeneral = "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID ";

                #region Where general
                
                //Cuenta
                if (!string.IsNullOrWhiteSpace(origen.CuentaORIG.Value))
                {
                    mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters["@CuentaID"].Value = origen.CuentaORIG.Value;

                    whereGeneral += " and CuentaID LIKE @CuentaID + '%' and eg_coPlanCuenta = @eg_coPlanCuenta ";
                }

                //CentroCosto
                if (!string.IsNullOrWhiteSpace(origen.CtoCostoORIG.Value))
                {
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = origen.CtoCostoORIG.Value;

                    whereGeneral += " and CentroCostoID LIKE @CentroCostoID + '%' and eg_coCentroCosto = @eg_coCentroCosto ";
                }

                //Proyecto
                if (!string.IsNullOrWhiteSpace(origen.ProyectoORIG.Value))
                {
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = origen.ProyectoORIG.Value;

                    whereGeneral += " and ProyectoID LIKE @ProyectoID + '%' and eg_coProyecto = @eg_coProyecto ";
                }
                #endregion
                #region Query con exclusiones
                if (excls.Count == 0)
                    query = queryGeneral + whereGeneral;
                else
                {
                    int i = 0;
                    isFirst = true;
                    string whereExcl = string.Empty;
                    foreach (DTO_coCompDistribuyeExcluye exc in excls)
                    {
                        if (!isFirst)
                            query += " UNION ";

                        #region Where general
                        whereExcl = string.Empty;

                        //Cuenta
                        if (!string.IsNullOrWhiteSpace(origen.CuentaORIG.Value))
                        {
                            mySqlCommand.Parameters.Add("@CuentaID" + i.ToString(), SqlDbType.Char, UDT_CuentaID.MaxLength);
                            mySqlCommand.Parameters["@CuentaID" + i.ToString()].Value = origen.CuentaORIG.Value;

                            whereExcl += " and CuentaID not LIKE @CuentaID" + i.ToString() + " + '%' and eg_coPlanCuenta = @eg_coPlanCuenta ";
                        }

                        //CentroCosto
                        if (!string.IsNullOrWhiteSpace(origen.CtoCostoORIG.Value))
                        {
                            mySqlCommand.Parameters.Add("@CentroCostoID" + i.ToString(), SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                            mySqlCommand.Parameters["@CentroCostoID" + i.ToString()].Value = origen.CtoCostoORIG.Value;

                            whereExcl += " and CentroCostoID not LIKE @CentroCostoID" + i.ToString() + " + '%' and eg_coCentroCosto = @eg_coCentroCosto ";
                        }

                        //Proyecto
                        if (!string.IsNullOrWhiteSpace(origen.ProyectoORIG.Value))
                        {
                            mySqlCommand.Parameters.Add("@ProyectoID" + i.ToString(), SqlDbType.Char, UDT_ProyectoID.MaxLength);
                            mySqlCommand.Parameters["@ProyectoID" + i.ToString()].Value = origen.ProyectoORIG.Value;

                            whereExcl += " and ProyectoID not LIKE @ProyectoID" + i.ToString() + " + '%' and eg_coProyecto = @eg_coProyecto ";
                        }
                        #endregion

                        query += queryGeneral + whereGeneral + whereExcl; 

                        isFirst = false;
                        i++;
                    }
                }
                #endregion

                mySqlCommand.CommandText = query;

                DTO_Comprobante comprobante = null;
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();

                #region Reliza la consulta
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                isFirst = true;
                comprobante = new DTO_Comprobante();
                int index = 0;
                while (dr.Read())
                {
                    if (isFirst)
                    {
                        header = new DTO_ComprobanteHeader(dr);
                        isFirst = false;
                    }

                    DTO_ComprobanteFooter detail = new DTO_ComprobanteFooter(dr);
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();

                if (!isFirst)
                    comprobante.AddData(header, footer);

                #endregion

                return comprobante;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetForDistribucion");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el listado de registros para el iva prorrateable
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna el listado de registros</returns>
        public List<DTO_ComprobanteFooter> DAL_Comprobante_IvaProrrateoIngresos(DateTime periodo, TipoCuentaGrupo tipo)
        {
            try
            {
                List<DTO_ComprobanteFooter> result = new List<DTO_ComprobanteFooter>();

                string query = string.Empty;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TipoCuenta", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@TipoCuenta"].Value = (short)tipo;
                #endregion
                #region query
                query =
                    "select aux.* " +
                    "from coAuxiliar aux " +
                    "	inner join coPlanCuenta cta on aux.CuentaID = cta.CuentaID and aux.eg_coPlanCuenta = cta.EmpresaGrupoID " +
                    "	inner join coCuentaGrupo grp on cta.CuentaGrupoID = grp.CuentaGrupoID and cta.eg_coCuentaGrupo = grp.EmpresaGrupoID " +
                    "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and grp.TipoCuenta = @TipoCuenta";
                #endregion

                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter(dr);
                    result.Add(footer);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_IvaProrrateoIngresos");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el listado de registros para el iva prorrateable
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna el listado de registros</returns>
        public List<DTO_ComprobanteFooter> DAL_Comprobante_IvaProrrateoDescontable(DateTime periodo, TipoCuentaGrupo tipo, string terceroPorDefecto)
        {
            try
            {
                List<DTO_ComprobanteFooter> result = new List<DTO_ComprobanteFooter>();
               

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TerceroEmpresa", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoCuenta", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@TerceroEmpresa"].Value = terceroPorDefecto;
                mySqlCommand.Parameters["@TipoCuenta"].Value = (short)tipo;
                #endregion
                #region query
                mySqlCommand.CommandText =
                    "select aux.CuentaID, aux.TerceroID, aux.ProyectoID, aux.CentroCostoID, aux.ConceptoSaldoID, " +
                    "	sum(aux.vlrBaseML) as vlrBaseML, sum(aux.vlrBaseME) as vlrBaseME, sum(aux.vlrMdaLoc) as vlrMdaLoc, " +
                    "   sum(aux.vlrMdaExt) as vlrMdaExt, sum(aux.vlrMdaOtr) as vlrMdaOtr " + 
                    "from coAuxiliar aux " +
                    "	inner join coPlanCuenta cta on aux.CuentaID = cta.CuentaID and aux.eg_coPlanCuenta = cta.EmpresaGrupoID " +
                    "	inner join coCuentaGrupo grp on cta.CuentaGrupoID = grp.CuentaGrupoID and cta.eg_coCuentaGrupo = grp.EmpresaGrupoID " +
                    "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and aux.TerceroID <> @TerceroEmpresa and grp.TipoCuenta = @TipoCuenta " +
                    "group by aux.CuentaID, aux.TerceroID, aux.ProyectoID, aux.CentroCostoID, aux.ConceptoSaldoID " +
                    "order by aux.CuentaID ";
                #endregion


                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                    footer.CuentaID.Value = dr["CuentaID"].ToString();
                    footer.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                    footer.TerceroID.Value = dr["TerceroID"].ToString();
                    footer.ProyectoID.Value = dr["ProyectoID"].ToString();
                    footer.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                    footer.vlrBaseML.Value = Convert.ToDecimal(dr["vlrBaseML"]) * -1;
                    footer.vlrBaseME.Value = Convert.ToDecimal(dr["vlrBaseME"]) * -1;
                    footer.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]) * -1;
                    footer.vlrMdaExt.Value = Convert.ToDecimal(dr["vlrMdaExt"]) * -1;
                    footer.vlrMdaOtr.Value = Convert.ToDecimal(dr["vlrMdaOtr"]) * -1;

                    result.Add(footer);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_IvaProrrateoDescontable");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de registros del auxiliar para realizar la consolidacion del balance
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoBalFuncional">Tipo de balance funcional</param>
        /// <param name="tipoBalCorporativo">Tipo de balance corporativo</param>
        /// <returns>Retorna la lista de datos (Null si hay comprobantes sin cuenta alterna asignada)</returns>
        public List<DTO_ComprobanteFooter> DAL_Comprobante_GetConsolidacionAntigua(DateTime periodo, string comprobanteID, string ctoCostoID)
        {
            try
            {
                List<DTO_ComprobanteFooter> result = new List<DTO_ComprobanteFooter>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCosotID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@ComprobanteID"].Value = comprobanteID;
                mySqlCommand.Parameters["@CentroCostoID"].Value = ctoCostoID;

                mySqlCommand.CommandText = "SELECT top(1) NumeroDoc FROM coAuxiliar With(NoLock) where EmpresaID = @EmpresaID " +
                    " and PeriodoID = @PeriodoID and ComprobanteID = @ComprobanteID and CentroCostoID = @CentroCostoID";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                    result.Add(footer);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetConsolidacionAntigua");
                throw exception;
            }
        }

        /// <summary>
        /// Borra informacio de coAuxiliar
        /// </summary>
        /// <param name="periodo">periodo</param>
        /// <param name="comprobanteID">comprobanteID</param>
        public int DAL_Comprobante_BorrarConsolidacionBalance(DateTime periodo, string comprobanteID, string ctoCostoID)
        {
            try
            {
                int numDoc = 0;

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCosotID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = comprobanteID;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = ctoCostoID;

                mySqlCommandSel.CommandText = "SELECT top(1) NumeroDoc FROM coAuxiliar With(NoLock) where EmpresaID = @EmpresaID " +
                    " and PeriodoID = @PeriodoID and ComprobanteID = @ComprobanteID and CentroCostoID = @CentroCostoID";

                object obj = mySqlCommandSel.ExecuteScalar();
                if (obj == null)
                    return numDoc;

                numDoc = Convert.ToInt32(obj);

                //Elimina la info del auxiliar
                mySqlCommandSel.CommandText = 
                    "DELETE FROM coAuxiliar where EmpresaID = @EmpresaID " +
                    "   and PeriodoID = @PeriodoID and CentroCostoID = @CentroCostoID";
                mySqlCommandSel.ExecuteNonQuery();

                return numDoc;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_BorrarConsolidacionBalance");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de registros del auxiliar para realizar la consolidacion del balance
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoBalFuncional">Tipo de balance funcional</param>
        /// <param name="tipoBalCorporativo">Tipo de balance corporativo</param>
        /// <returns>Retorna la lista de datos (Null si hay comprobantes sin cuenta alterna asignada)</returns>
        public List<DTO_ComprobanteFooter> DAL_Comprobante_TraerInfoConsolidacion(DateTime periodo, string tipoBalFuncional, string tipoBalCorporativo)
        {
            try
            {
                List<DTO_ComprobanteFooter> result = new List<DTO_ComprobanteFooter>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TipoBalFunc", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoBalCorp", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@TipoBalFunc"].Value = tipoBalFuncional;
                mySqlCommand.Parameters["@TipoBalCorp"].Value = tipoBalCorporativo;
                #endregion
                #region query para asignar las cuentas alternas al comprobante
                mySqlCommand.CommandText =
                    "update aux " +
                    "set aux.CuentaAlternaID = cta.CuentaAlternaID " +
                    "from coAuxiliar aux with(nolock) " +
                    "	inner join coPlanCuenta cta with(nolock) on aux.CuentaID = cta.CuentaID " +
                    "		and aux.eg_coPlanCuenta = cta.EmpresaGrupoID " +
                    "where EmpresaID=@EmpresaID and PeriodoID=@PeriodoID";
                
                #endregion
                #region Verifica si hay registros del auxiliar requeridos sin cuenta alterna
                mySqlCommand.CommandText =
                    "select count(*) " +
                    "from coAuxiliar aux with(nolock) " +
                    "	inner join coComprobante comp on aux.ComprobanteID = comp.ComprobanteID and aux.eg_coComprobante = comp.EmpresaGrupoID " +
                    "where aux.EmpresaID=@EmpresaID and aux.PeriodoID=@PeriodoID and aux.CuentaAlternaID is NULL " +
                    "	and (comp.BalanceTipoID=@TipoBalFunc or comp.BalanceTipoID=@TipoBalCorp)";

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                if (count > 0)
                    return null;
                #endregion
                #region Trae la info de los auxiliares
                mySqlCommand.CommandText =
                    "select aux.* " +
                    "from coAuxiliar aux with(nolock) " +
                    "	inner join coComprobante comp on aux.ComprobanteID = comp.ComprobanteID and aux.eg_coComprobante = comp.EmpresaGrupoID " +
                    "where EmpresaID=@EmpresaID and PeriodoID=@PeriodoID " +
                    "	and (comp.BalanceTipoID=@TipoBalFunc or comp.BalanceTipoID=@TipoBalCorp) ";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                    result.Add(footer);
                }
                dr.Close();

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_TraerInfoConsolidacion");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de registros del auxiliar para realizar la aprobacion del giro
        /// </summary>
        /// <param name="numerosDoc">Lista con los numero doc de cada giro</param>
        /// <returns></returns>
        public List<DTO_ComprobanteFooter> DAL_Comprobante_GetForAprobGiro(List<int> numerosDoc, string ctaID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters["@CuentaID"].Value = ctaID;

                string query = string.Empty;
                string queryInternal =
                    "select * from " + 
                    "( " +
                    "   select TOP 1 * " +
                    "   from coAuxiliar with(nolock) where NumeroDoc = {0} and CuentaID = @CuentaID " +
                    "	    and DatoAdd4 = '" + AuxiliarDatoAdd4.Contrapartida.ToString() + "' order by Consecutivo" + 
                    ") aux{1}";


                for (int i = 1; i <= numerosDoc.Count; ++i)
                {
                    if (i < numerosDoc.Count)
                        query += string.Format(queryInternal, numerosDoc[i - 1], i) + " UNION ";
                    else
                        query += string.Format(queryInternal, numerosDoc[i - 1], i);
                }

                mySqlCommandSel.CommandText = query;

                List<DTO_ComprobanteFooter> result = new List<DTO_ComprobanteFooter>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ComprobanteFooter dto = new DTO_ComprobanteFooter(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetForAprobacion");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el valor de las cuentas de costo
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <returns></returns>
        public decimal DAL_Comprobante_GetValorByCuentaCosto(int numeroDoc)
        {
            try
            {
                decimal result = 0;
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DatoAdd4_1", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@DatoAdd4_2", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@DatoAdd4_3", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCuentaGrupo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@DatoAdd4_1"].Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                mySqlCommand.Parameters["@DatoAdd4_2"].Value = AuxiliarDatoAdd4.AjEnCambio.ToString();
                mySqlCommand.Parameters["@DatoAdd4_3"].Value = AuxiliarDatoAdd4.AjEnCambioContra.ToString();
                mySqlCommand.Parameters["@eg_coCuentaGrupo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "select SUM(vlrMdaLoc) as ML " +
                    "from coauxiliar aux with(nolock) " +
                    "	inner join coPlanCuenta cta with(nolock) on aux.CuentaID = cta.CuentaID and cta.EmpresaGrupoID = @eg_coPlanCuenta " +
                    "	inner join coCuentaGrupo grp with(nolock) on cta.CuentaGrupoID = grp.CuentaGrupoID and grp.EmpresaGrupoID = @eg_coCuentaGrupo " +
                    "		and grp.TipoCuenta = 1 " +
                    "where NumeroDoc = @NumeroDoc  and (DatoAdd4 is null or (DatoAdd4 <> @DatoAdd4_1 and DatoAdd4 <> @DatoAdd4_2 and DatoAdd4 <> @DatoAdd4_3)) ";

                object obj = mySqlCommand.ExecuteScalar();
                result = !string.IsNullOrWhiteSpace(obj.ToString()) ? Convert.ToDecimal(obj) : 0;

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_SaldosByPeriodoCuenta");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de comprobantes relacionados con un solo numeroDoc
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        /// <returns></returns>
        public List<Tuple<string, int>> DAL_Comprobante_GetComprobantesByNumeroDoc(int numeroDoc)
        {
            try
            {
                List<Tuple<string, int>> results = new List<Tuple<string, int>>();
                
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "select distinct ComprobanteID,ComprobanteNro from coAuxiliar with(nolock) where NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    string compID = dr[0].ToString();
                    int compNro = Convert.ToInt32(dr[1]);

                    Tuple<string, int> tupla = new Tuple<string, int>(compID, compNro);
                    results.Add(tupla);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetComprobantesByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Indica si se han hecho movimientos sobre un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento consultado</param>
        /// <param name="estados">Estados por los que se buscan</param>
        /// <param name="equal">Indica si los estados deben ser iguales o diferentes</param>
        /// <returns>Retorna true si encuentra movimientos</returns>
        public int DAL_Comprobante_GetMovimientosByEstados(int numeroDoc, List<EstadoDocControl> estados, bool equal)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string queryEstado = string.Empty;
                if (estados.Count > 0)
                {
                    queryEstado = " and Estado ";
                    if(!equal)
                        queryEstado += "not ";

                    queryEstado += "in (";
                    for (int i = 0; i < estados.Count; ++i)
                    {
                        int est = (int)estados[i];
                        queryEstado += est.ToString();
                        if (i != estados.Count - 1)
                            queryEstado += ",";
                    }
                    queryEstado += ")";
                }

                mySqlCommand.CommandText =
                    "select count(*) " +
                    "from coAuxiliar aux with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on aux.NumeroDoc = ctrl.NumeroDoc and aux.IdentificadorTR <> ctrl.NumeroDoc " +
                    "       and ctrl.Descripcion not like 'REVERSION DE DOCUMENTO:%' " + queryEstado +
                    "WHERE IdentificadorTR = @NumeroDoc";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_PagosAnulados");
                throw exception;
            }

        }

        /// <summary>
        /// Trae las transferencias bancarias por tercero
        /// </summary>
        /// <param name="terceroID">tercero a validar</param>
        /// <param name="DocTercero">numero de la factura</param>
        /// <returns>lista de comp</returns>
        public DTO_Comprobante DAL_Comprobante_GetTransfBancariaByTercero(string terceroID, int identificadorTr)
        {
            try
            {
                DTO_Comprobante comprobante = null;
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();

                string query = string.Empty;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                query =
                "Select * from  coauxiliar aux " +
                " Inner join glDocumentoControl doc on aux.NumeroDoc = doc.NumeroDoc   " +
                "Where aux.EmpresaID = @EmpresaID and aux.TerceroID = @TerceroID and aux.IdentificadorTR = @IdentificadorTr "+
                "      and aux.DatoAdd9 is null and doc.DocumentoID = @DocumentoID and doc.Estado = @Estado  ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = identificadorTr;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.TransferenciasBancarias;
                mySqlCommand.Parameters["@Estado"].Value = (byte)EstadoDocControl.Aprobado;
                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                bool isFirst = true;
              
                int index = 0;

                while (dr.Read())
                {
                    if (isFirst)
                    {
                        header = new DTO_ComprobanteHeader(dr);
                        isFirst = false;
                    }

                    DTO_ComprobanteFooter detail = new DTO_ComprobanteFooter(dr);
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();

                if (!isFirst)
                {
                    comprobante = new DTO_Comprobante();
                    comprobante.AddData(header, footer);
                }               

                return comprobante;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="periodoInicial">periodo Inicial</param>
        /// <param name="periodoFinal">periodo final</param>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de auxiliares</returns>
        public List<DTO_QueryMvtoAuxiliar> DAL_Comprobante_GetAuxByParameter(DateTime? periodoInicial, DateTime? periodoFinal, DTO_QueryMvtoAuxiliar filter)
        {
            try
            {
                List<DTO_QueryMvtoAuxiliar> result = new List<DTO_QueryMvtoAuxiliar>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select aux.*, docCtrl.DocumentoID,docCtrl.DocumentoNro,docCtrl.DocumentoTercero, docCtrl.DocumentoTipo,doc.Descriptivo as DocumentoDes,comp.BalanceTipoID, " +
                        " cta.Descriptivo as CuentaDes,ter.Descriptivo as TerceroDes,cto.Descriptivo as CentroCtoDes,proy.Descriptivo as ProyectoDes,lin.Descriptivo as LineaPresDes,carg.Descriptivo as ConceptoDes  " +
                        " from coAuxiliar aux with(nolock) "+
                        " inner join glDocumentoControl docCtrl with(nolock) on docCtrl.NumeroDoc = aux.NumeroDoc  " +
                        " inner join glDocumento doc with(nolock) on doc.DocumentoID = docCtrl.DocumentoID  " +
                        " left join coPlanCuenta cta with(nolock) on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta  " +
                        " left join coTercero ter with(nolock) on ter.TerceroID = aux.TerceroID and ter.EmpresaGrupoID = aux.eg_coTercero  " +
                        " left join coCentroCosto cto with(nolock) on cto.CentroCostoID = aux.CentroCostoID and cto.EmpresaGrupoID = aux.eg_coCentroCosto " +
                        " left join coProyecto proy with(nolock) on proy.ProyectoID = aux.ProyectoID and proy.EmpresaGrupoID = aux.eg_coProyecto  " +
                        " left join plLineaPresupuesto lin with(nolock) on lin.LineaPresupuestoID = aux.LineaPresupuestoID and lin.EmpresaGrupoID = aux.eg_plLineaPresupuesto  " +
                        " left join coConceptoCargo carg with(nolock) on carg.ConceptoCargoID = aux.ConceptoCargoID and carg.EmpresaGrupoID = aux.eg_coConceptoCargo  " +
                        " left join coComprobante comp with(nolock) on comp.ComprobanteID = aux.ComprobanteID and comp.EmpresaGrupoID = aux.eg_coComprobante  " +
                        " where docCtrl.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
              
                if (!string.IsNullOrEmpty(filter.DocumentoID.Value.ToString()))
                {
                    query += "and docCtrl.DocumentoID = @DocumentoID ";
                    mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoID"].Value = filter.DocumentoID.Value;
                    filterInd = true;
                }
                if (periodoInicial != null && periodoFinal != null)
                {
                    query += "and aux.Fecha between @PeriodoInicial and @PeriodoFinal ";
                    mySqlCommand.Parameters.Add("@PeriodoInicial", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@PeriodoInicial"].Value = periodoInicial;
                    mySqlCommand.Parameters.Add("@PeriodoFinal", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@PeriodoFinal"].Value = periodoFinal;
                    filterInd = true;
                } 
                if (!string.IsNullOrEmpty(filter.PrefijoCOM.Value.ToString()))
                {
                    query += "and docCtrl.PrefijoID = @PrefijoID ";
                    mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                    mySqlCommand.Parameters["@PrefijoID"].Value = filter.PrefijoCOM.Value;
                    filterInd = true;
                }   
                if (!string.IsNullOrEmpty(filter.DocumentoNro.Value.ToString()))
                {
                    query += "and docCtrl.DocumentoNro = @DocumentoNro ";
                    mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoNro"].Value = filter.DocumentoNro.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ComprobanteID.Value.ToString()))
                {
                    query += "and aux.ComprobanteID = @ComprobanteID ";
                    mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                    mySqlCommand.Parameters["@ComprobanteID"].Value = filter.ComprobanteID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ComprobanteNro.Value.ToString()))
                {
                    query += "and aux.ComprobanteNro = @ComprobanteNro ";
                    mySqlCommand.Parameters.Add("@ComprobanteNro", SqlDbType.Int);
                    mySqlCommand.Parameters["@ComprobanteNro"].Value = filter.ComprobanteNro.Value;
                    filterInd = true;
                }       
                if (!string.IsNullOrEmpty(filter.CuentaID.Value.ToString()))
                {
                    query += "and aux.CuentaID = @CuentaID ";
                    mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters["@CuentaID"].Value = filter.CuentaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.TerceroID.Value.ToString()))
                {
                    query += "and aux.TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = filter.TerceroID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CentroCostoID.Value))
                {
                    query += "and aux.CentroCostoID = @CentroCostoID ";
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = filter.CentroCostoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ProyectoID.Value.ToString()))
                {
                    query += "and aux.ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.LineaPresupuestoID.Value.ToString()))
                {
                    query += "and aux.LineaPresupuestoID = @LineaPresupuestoID ";
                    mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = filter.LineaPresupuestoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ConceptoCargoID.Value.ToString()))
                {
                    query += "and aux.ConceptoCargoID = @ConceptoCargoID ";
                    mySqlCommand.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                    mySqlCommand.Parameters["@ConceptoCargoID"].Value = filter.ConceptoCargoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoCOM.Value.ToString()))
                {
                    query += "and aux.DocumentoCOM = @DocumentoCOM ";
                    mySqlCommand.Parameters.Add("@DocumentoCOM", SqlDbType.Char,20);
                    mySqlCommand.Parameters["@DocumentoCOM"].Value = filter.DocumentoCOM.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.BalanceTipoID.Value.ToString()))
                {
                    query += "and comp.BalanceTipoID = @BalanceTipoID ";
                    mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                    mySqlCommand.Parameters["@BalanceTipoID"].Value = filter.BalanceTipoID.Value;
                    filterInd = true;
                }

                query += " order by aux.NumeroDoc desc";
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_QueryMvtoAuxiliar aux = new DTO_QueryMvtoAuxiliar(dr);
                    aux.Comprobante = aux.ComprobanteID.Value + "-" + aux.ComprobanteNro.Value.ToString();
                    result.Add(aux);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetAuxByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los datos secundarios de un auxiliar 
        /// </summary>       
        /// <param name="consecutivoAux">Consecutivo del comprobante</param>
        /// <param name="isPre">Verifica si la actualizacion se esta haciendo en el auxiliar Preliminar o en el real</param>
        public void DAL_Comprobante_Update(int consecutivoAux, DTO_ComprobanteFooter comp, bool isPre)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string table = isPre ? "coAuxiliarPre" : "coAuxiliar";
               
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd6", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd7", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd8", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd9", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd10", SqlDbType.Char, 20);
                mySqlCommand.Parameters["@Consecutivo"].Value = consecutivoAux;
                mySqlCommand.Parameters["@DatoAdd1"].Value = comp.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = comp.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = comp.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = comp.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = comp.DatoAdd5.Value;
                mySqlCommand.Parameters["@DatoAdd6"].Value = comp.DatoAdd6.Value;
                mySqlCommand.Parameters["@DatoAdd7"].Value = comp.DatoAdd7.Value;
                mySqlCommand.Parameters["@DatoAdd8"].Value = comp.DatoAdd8.Value;
                mySqlCommand.Parameters["@DatoAdd9"].Value = comp.DatoAdd9.Value;
                mySqlCommand.Parameters["@DatoAdd10"].Value = comp.DatoAdd10.Value;

                mySqlCommand.CommandText = "UPDATE " + table +
                                           " SET DatoAdd1 = @DatoAdd1,  " +
                                           "     DatoAdd2 = @DatoAdd2, " +
                                           "     DatoAdd3 = @DatoAdd3,  " +
                                           "     DatoAdd4 = @DatoAdd4,  " +
                                           "     DatoAdd5 = @DatoAdd5,  " +
                                           "     DatoAdd6 = @DatoAdd6,  " +
                                           "     DatoAdd7 = @DatoAdd7,  " +
                                           "     DatoAdd8 = @DatoAdd8,  " +
                                           "     DatoAdd9 = @DatoAdd9,  " +
                                           "     DatoAdd10 = @DatoAdd10  " +
                                           " WHERE Consecutivo=@Consecutivo ";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="periodoInicial">periodo</param>
        /// <param name="lugarxDef">lugarGeo</param>
        /// <returns>Lista de auxiliares</returns>
        public List<DTO_PagoImpuesto> DAL_Comprobante_GetAuxForImpuesto(DateTime periodoFilter, DTO_glLugarGeografico lugarxDef)
        {
            try
            {
                List<DTO_PagoImpuesto> result = new List<DTO_PagoImpuesto>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =

                " SELECT distinct aux.PeriodoID, cta.ImpuestoTipoID,imp.Descriptivo as ImpuestoTipoDesc,cta.NITCierreAnual as TerceroID,ter.Descriptivo as TerceroDesc,aux.CuentaID, cta.Descriptivo as CuentaDesc, " +
                "  CASE WHEN(imp.ImpuestoAlcance = 3) THEN aux.LugarGeograficoID ELSE @LugarGeoDef END as LugarGeoID,  " +
                "  CASE WHEN(imp.ImpuestoAlcance = 3) THEN ciudad.Descriptivo ELSE @LugarGeoDefDesc END as LugarGeoDesc, imp.PeriodoPago, " +
                " sum(aux.vlrMdaLoc) as ValorLocal " +
                " FROM coAuxiliar aux  " +
                    " inner join coPlanCuenta cta on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta and cta.ImpuestoTipoID is not null " +
                    " left join coImpuestoTipo imp on imp.ImpuestoTipoID = cta.ImpuestoTipoID and imp.EmpresaGrupoID = cta.eg_coImpuestoTipo " +
                    " left join glLugarGeografico ciudad on ciudad.LugarGeograficoID = aux.LugarGeograficoID and ciudad.EmpresaGrupoID = aux.eg_glLugarGeografico " +
                    " left join cotercero ter on ter.TerceroID = cta.NITCierreAnual and ter.EmpresaGrupoID = cta.eg_coTercero " +
                " WHERE aux.EmpresaID = @EmpresaID and aux.PeriodoID between @PeriodoIDIni and @PeriodoID  and aux.TerceroID <> cta.NITCierreAnual " +
                " Group by aux.PeriodoID,cta.ImpuestoTipoID,imp.Descriptivo,cta.NITCierreAnual,ter.Descriptivo,aux.CuentaID, cta.Descriptivo ,imp.ImpuestoAlcance,aux.LugarGeograficoID, ciudad.Descriptivo,imp.PeriodoPago ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@PeriodoIDIni", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@LugarGeoDef", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarGeoDefDesc", SqlDbType.Char, UDT_Descriptivo.MaxLength);              
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;               
                mySqlCommand.Parameters["@PeriodoID"].Value = periodoFilter;
                mySqlCommand.Parameters["@PeriodoIDIni"].Value = new DateTime(periodoFilter.Year, 1, 1);
                mySqlCommand.Parameters["@LugarGeoDef"].Value = lugarxDef.ID.Value;
                mySqlCommand.Parameters["@LugarGeoDefDesc"].Value = lugarxDef.Descriptivo.Value;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_PagoImpuesto pago = new DTO_PagoImpuesto(dr);
                    result.Add(pago);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetAuxByParameter");
                throw exception;
            }
        }


        #endregion
    }
}
