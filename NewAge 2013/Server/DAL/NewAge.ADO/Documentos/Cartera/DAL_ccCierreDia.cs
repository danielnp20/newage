﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;

namespace NewAge.ADO
{
    public class DAL_ccCierreDia : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCierreDia(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega un registro de cierre 
        /// </summary>
        /// <param name="cierre">Cierre nuevo</param>
        /// <returns></returns>
        private void DAL_ccCierreDia_AddItem(DTO_ccCierreDia cierre)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccCierreDia " +
                    "( " +
                        "EmpresaID,Periodo,DocumentoID,LineaCreditoID,AsesorID,CentroPagoID,ZonaID,CompradorCarteraID,Plazo,TipoDato," +
                        "ValorDia01,ValorDia02,ValorDia03,ValorDia04,ValorDia05,ValorDia06,ValorDia07,ValorDia08,ValorDia09,ValorDia10," +
                        "ValorDia11,ValorDia12,ValorDia13,ValorDia14,ValorDia15,ValorDia16,ValorDia17,ValorDia18,ValorDia19,ValorDia20," +
                        "ValorDia21,ValorDia22,ValorDia23,ValorDia24,ValorDia25,ValorDia26,ValorDia27,ValorDia28,ValorDia29,ValorDia30,ValorDia31," +
                        "eg_ccLineaCredito,eg_ccAsesor,eg_ccCentroPagoPAG,eg_glZona,eg_ccCompradorCartera" +
                    ") " +
                    "VALUES " +
                    "( " +
                        "@EmpresaID,@Periodo,@DocumentoID,@LineaCreditoID,@AsesorID,@CentroPagoID,@ZonaID,@CompradorCarteraID,@Plazo,@TipoDato," +
                        "@ValorDia01,@ValorDia02,@ValorDia03,@ValorDia04,@ValorDia05,@ValorDia06,@ValorDia07,@ValorDia08,@ValorDia09,@ValorDia10," +
                        "@ValorDia11,@ValorDia12,@ValorDia13,@ValorDia14,@ValorDia15,@ValorDia16,@ValorDia17,@ValorDia18,@ValorDia19,@ValorDia20," +
                        "@ValorDia21,@ValorDia22,@ValorDia23,@ValorDia24,@ValorDia25,@ValorDia26,@ValorDia27,@ValorDia28,@ValorDia29,@ValorDia30,@ValorDia31," +
                        "@eg_ccLineaCredito,@eg_ccAsesor,@eg_ccCentroPagoPAG,@eg_glZona,@eg_ccCompradorCartera" +
                    ") ";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@TipoDato", SqlDbType.TinyInt);
                //Valores
                mySqlCommandSel.Parameters.Add("@ValorDia01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia13", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia14", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia15", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia16", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia17", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia18", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia19", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia20", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia23", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia24", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia25", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia26", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia27", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia28", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia29", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia30", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorDia31", SqlDbType.Decimal);
                //Eg
                mySqlCommandSel.Parameters.Add("@eg_ccLineaCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glZona", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = cierre.Periodo.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = cierre.DocumentoID.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = cierre.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = cierre.AsesorID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = cierre.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = cierre.ZonaID.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = cierre.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = cierre.Plazo.Value;
                mySqlCommandSel.Parameters["@TipoDato"].Value = cierre.TipoDato.Value;
                //Valores
                mySqlCommandSel.Parameters["@ValorDia01"].Value = cierre.ValorDia01.Value;
                mySqlCommandSel.Parameters["@ValorDia02"].Value = cierre.ValorDia02.Value;
                mySqlCommandSel.Parameters["@ValorDia03"].Value = cierre.ValorDia03.Value;
                mySqlCommandSel.Parameters["@ValorDia04"].Value = cierre.ValorDia04.Value;
                mySqlCommandSel.Parameters["@ValorDia05"].Value = cierre.ValorDia05.Value;
                mySqlCommandSel.Parameters["@ValorDia06"].Value = cierre.ValorDia06.Value;
                mySqlCommandSel.Parameters["@ValorDia07"].Value = cierre.ValorDia07.Value;
                mySqlCommandSel.Parameters["@ValorDia08"].Value = cierre.ValorDia08.Value;
                mySqlCommandSel.Parameters["@ValorDia09"].Value = cierre.ValorDia09.Value;
                mySqlCommandSel.Parameters["@ValorDia10"].Value = cierre.ValorDia10.Value;
                mySqlCommandSel.Parameters["@ValorDia11"].Value = cierre.ValorDia11.Value;
                mySqlCommandSel.Parameters["@ValorDia12"].Value = cierre.ValorDia12.Value;
                mySqlCommandSel.Parameters["@ValorDia13"].Value = cierre.ValorDia13.Value;
                mySqlCommandSel.Parameters["@ValorDia14"].Value = cierre.ValorDia14.Value;
                mySqlCommandSel.Parameters["@ValorDia15"].Value = cierre.ValorDia15.Value;
                mySqlCommandSel.Parameters["@ValorDia16"].Value = cierre.ValorDia16.Value;
                mySqlCommandSel.Parameters["@ValorDia17"].Value = cierre.ValorDia17.Value;
                mySqlCommandSel.Parameters["@ValorDia18"].Value = cierre.ValorDia18.Value;
                mySqlCommandSel.Parameters["@ValorDia19"].Value = cierre.ValorDia19.Value;
                mySqlCommandSel.Parameters["@ValorDia20"].Value = cierre.ValorDia20.Value;
                mySqlCommandSel.Parameters["@ValorDia21"].Value = cierre.ValorDia21.Value;
                mySqlCommandSel.Parameters["@ValorDia22"].Value = cierre.ValorDia22.Value;
                mySqlCommandSel.Parameters["@ValorDia23"].Value = cierre.ValorDia23.Value;
                mySqlCommandSel.Parameters["@ValorDia24"].Value = cierre.ValorDia24.Value;
                mySqlCommandSel.Parameters["@ValorDia25"].Value = cierre.ValorDia25.Value;
                mySqlCommandSel.Parameters["@ValorDia26"].Value = cierre.ValorDia26.Value;
                mySqlCommandSel.Parameters["@ValorDia27"].Value = cierre.ValorDia27.Value;
                mySqlCommandSel.Parameters["@ValorDia28"].Value = cierre.ValorDia28.Value;
                mySqlCommandSel.Parameters["@ValorDia29"].Value = cierre.ValorDia29.Value;
                mySqlCommandSel.Parameters["@ValorDia30"].Value = cierre.ValorDia30.Value;
                mySqlCommandSel.Parameters["@ValorDia31"].Value = cierre.ValorDia31.Value;
                //Eg
                mySqlCommandSel.Parameters["@eg_ccLineaCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccLineaCredito, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAsesor, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreDia_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_ccCierreDia_UpdateItem(DTO_ccCierreDia cierre, int dia)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string diaStr = dia.ToString();
                if (diaStr.Length == 1)
                    diaStr = "0" + diaStr;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE ccCierreDia SET ValorDia" + diaStr + " = @ValorDia " +
                    "WHERE EmpresaID= @EmpresaID AND Periodo= @Periodo AND DocumentoID= @DocumentoID AND LineaCreditoID= @LineaCreditoID " +
                    "   AND AsesorID= @AsesorID AND CentroPagoID= @CentroPagoID AND ZonaID= @ZonaID AND Plazo=@Plazo AND TipoDato= @TipoDato";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@TipoDato", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ValorDia", SqlDbType.Decimal);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = cierre.Periodo.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = cierre.DocumentoID.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = cierre.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = cierre.AsesorID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = cierre.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = cierre.ZonaID.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = cierre.Plazo.Value;
                mySqlCommandSel.Parameters["@TipoDato"].Value = cierre.TipoDato.Value;

                //Valor
                switch (dia)
                {
                    case 1:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia01.Value;
                        break;
                    case 2:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia02.Value;
                        break;
                    case 3:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia03.Value;
                        break;
                    case 4:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia04.Value;
                        break;
                    case 5:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia05.Value;
                        break;
                    case 6:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia06.Value;
                        break;
                    case 7:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia07.Value;
                        break;
                    case 8:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia08.Value;
                        break;
                    case 9:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia09.Value;
                        break;
                    case 10:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia10.Value;
                        break;
                    case 11:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia11.Value;
                        break;
                    case 12:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia12.Value;
                        break;
                    case 13:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia13.Value;
                        break;
                    case 14:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia14.Value;
                        break;
                    case 15:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia15.Value;
                        break;
                    case 16:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia16.Value;
                        break;
                    case 17:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia17.Value;
                        break;
                    case 18:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia18.Value;
                        break;
                    case 19:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia19.Value;
                        break;
                    case 20:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia20.Value;
                        break;
                    case 21:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia21.Value;
                        break;
                    case 22:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia22.Value;
                        break;
                    case 23:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia23.Value;
                        break;
                    case 24:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia24.Value;
                        break;
                    case 25:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia25.Value;
                        break;
                    case 26:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia26.Value;
                        break;
                    case 27:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia27.Value;
                        break;
                    case 28:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia28.Value;
                        break;
                    case 29:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia29.Value;
                        break;
                    case 30:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia30.Value;
                        break;
                    case 31:
                        mySqlCommandSel.Parameters["@ValorDia"].Value = cierre.ValorDia31.Value;
                        break;
                }
                #endregion
                #region Campos null

                if (string.IsNullOrWhiteSpace(cierre.CompradorCarteraID.Value))
                    mySqlCommandSel.CommandText += " AND CompradorCarteraID IS NULL ";
                else
                {
                    mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = cierre.CompradorCarteraID.Value;

                    mySqlCommandSel.CommandText += " AND CompradorCarteraID = @CompradorCarteraID ";
                }

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
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreDia_UpdateItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_ccCierreDia_Add(DTO_ccCierreDia cierre, int dia)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Verifica si tiene datos
                if (dia == 1 && (!cierre.ValorDia01.Value.HasValue || cierre.ValorDia01.Value.Value == 0))
                    return;
                if (dia == 2 && (!cierre.ValorDia02.Value.HasValue || cierre.ValorDia02.Value.Value == 0))
                    return;
                if (dia == 3 && (!cierre.ValorDia03.Value.HasValue || cierre.ValorDia03.Value.Value == 0))
                    return;
                if (dia == 4 && (!cierre.ValorDia04.Value.HasValue || cierre.ValorDia04.Value.Value == 0))
                    return;
                if (dia == 5 && (!cierre.ValorDia05.Value.HasValue || cierre.ValorDia05.Value.Value == 0))
                    return;
                if (dia == 6 && (!cierre.ValorDia06.Value.HasValue || cierre.ValorDia06.Value.Value == 0))
                    return;
                if (dia == 7 && (!cierre.ValorDia07.Value.HasValue || cierre.ValorDia07.Value.Value == 0))
                    return;
                if (dia == 8 && (!cierre.ValorDia08.Value.HasValue || cierre.ValorDia08.Value.Value == 0))
                    return;
                if (dia == 9 && (!cierre.ValorDia09.Value.HasValue || cierre.ValorDia09.Value.Value == 0))
                    return;
                if (dia == 10 && (!cierre.ValorDia10.Value.HasValue || cierre.ValorDia10.Value.Value == 0))
                    return;
                if (dia == 11 && (!cierre.ValorDia11.Value.HasValue || cierre.ValorDia11.Value.Value == 0))
                    return;
                if (dia == 12 && (!cierre.ValorDia12.Value.HasValue || cierre.ValorDia12.Value.Value == 0))
                    return;
                if (dia == 13 && (!cierre.ValorDia13.Value.HasValue || cierre.ValorDia13.Value.Value == 0))
                    return;
                if (dia == 14 && (!cierre.ValorDia14.Value.HasValue || cierre.ValorDia14.Value.Value == 0))
                    return;
                if (dia == 15 && (!cierre.ValorDia15.Value.HasValue || cierre.ValorDia15.Value.Value == 0))
                    return;
                if (dia == 16 && (!cierre.ValorDia16.Value.HasValue || cierre.ValorDia16.Value.Value == 0))
                    return;
                if (dia == 17 && (!cierre.ValorDia17.Value.HasValue || cierre.ValorDia17.Value.Value == 0))
                    return;
                if (dia == 18 && (!cierre.ValorDia18.Value.HasValue || cierre.ValorDia18.Value.Value == 0))
                    return;
                if (dia == 19 && (!cierre.ValorDia19.Value.HasValue || cierre.ValorDia19.Value.Value == 0))
                    return;
                if (dia == 20 && (!cierre.ValorDia20.Value.HasValue || cierre.ValorDia20.Value.Value == 0))
                    return;
                if (dia == 21 && (!cierre.ValorDia21.Value.HasValue || cierre.ValorDia21.Value.Value == 0))
                    return;
                if (dia == 22 && (!cierre.ValorDia22.Value.HasValue || cierre.ValorDia22.Value.Value == 0))
                    return;
                if (dia == 23 && (!cierre.ValorDia23.Value.HasValue || cierre.ValorDia23.Value.Value == 0))
                    return;
                if (dia == 24 && (!cierre.ValorDia24.Value.HasValue || cierre.ValorDia24.Value.Value == 0))
                    return;
                if (dia == 25 && (!cierre.ValorDia25.Value.HasValue || cierre.ValorDia25.Value.Value == 0))
                    return;
                if (dia == 26 && (!cierre.ValorDia26.Value.HasValue || cierre.ValorDia26.Value.Value == 0))
                    return;
                if (dia == 27 && (!cierre.ValorDia27.Value.HasValue || cierre.ValorDia27.Value.Value == 0))
                    return;
                if (dia == 28 && (!cierre.ValorDia28.Value.HasValue || cierre.ValorDia28.Value.Value == 0))
                    return;
                if (dia == 29 && (!cierre.ValorDia29.Value.HasValue || cierre.ValorDia29.Value.Value == 0))
                    return;
                if (dia == 30 && (!cierre.ValorDia30.Value.HasValue || cierre.ValorDia30.Value.Value == 0))
                    return;
                if (dia == 31 && (!cierre.ValorDia31.Value.HasValue || cierre.ValorDia31.Value.Value == 0))
                    return;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT COUNT (*) from ccCierreDia with(nolock) " +
                    "WHERE EmpresaID= @EmpresaID AND Periodo= @Periodo AND DocumentoID= @DocumentoID AND LineaCreditoID= @LineaCreditoID " +
                    "   AND AsesorID= @AsesorID AND CentroPagoID= @CentroPagoID AND ZonaID= @ZonaID AND Plazo=@Plazo AND TipoDato= @TipoDato";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@TipoDato", SqlDbType.TinyInt);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = cierre.Periodo.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = cierre.DocumentoID.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = cierre.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = cierre.AsesorID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = cierre.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = cierre.ZonaID.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = cierre.Plazo.Value;
                mySqlCommandSel.Parameters["@TipoDato"].Value = cierre.TipoDato.Value;

                #endregion
                #region Campos null

                if (string.IsNullOrWhiteSpace(cierre.CompradorCarteraID.Value))
                    mySqlCommandSel.CommandText += " AND CompradorCarteraID IS NULL ";
                else
                {
                    mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = cierre.CompradorCarteraID.Value;

                    mySqlCommandSel.CommandText += " AND CompradorCarteraID = @CompradorCarteraID ";
                }

                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                if (count == 0)
                    this.DAL_ccCierreDia_AddItem(cierre);
                else
                    this.DAL_ccCierreDia_UpdateItem(cierre, dia);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreDia_Add");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Carga la información para hacer un cierre diario
        /// </summary>
        /// <param name="fecha">Fecha de cierre</param>
        /// <param name="balanceFunc">Balance funcional</param>
        /// <returns></returns>
        public List<DTO_ccCierreDia> DAL_ccCierreDia_GetMovimientos(string balanceFunc, DateTime fecha)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_ccCierreDia> cierres = new List<DTO_ccCierreDia>();
                #region Query
                mySqlCommand.CommandText =
                    "select aux.Fecha, mvto.* from " +
                    "( " +
                    "	select " +
                    "       ctrl.NumeroDoc as 'CtrlOrigen', ctrl.DocumentoID as 'DocumentoOrigen',mvto.IdentificadorTR as 'NumDocCredito', " +
                    "       ctrlCredito.DocumentoTercero as 'LibranzaID',cred.LineaCreditoID, cred.AsesorID, cred.CentroPagoID, cred.ZonaID, cred.CompradorCarteraID, " +
                    "       cred.Plazo, CompLocal1,CompLocal2,CompLocal3,CompLocal4,CompLocal5,CompLocal6,CompLocal7,CompLocal8,CompLocal9,CompLocal10, " +
                    "       CompLocal11,CompLocal12,CompLocal13,CompLocal14,CompLocal15,CompLocal16,CompLocal17,CompLocal18,CompLocal19,CompLocal20 " +
                    "	from coComponenteMvto mvto with(nolock) " +
                    "		inner join glDocumentoControl ctrl with(nolock) on mvto.NumeroDoc = ctrl.NumeroDoc " +
                    "		inner join glDocumentoControl ctrlCredito with(nolock) on mvto.IdentificadorTR = ctrlCredito.NumeroDoc and ctrlCredito.DocumentoID = @DocumentoCredito " +
                    "		inner join ccCreditoDocu cred with(nolock) on ctrlCredito.NumeroDoc = cred.NumeroDoc " +
                    "	where mvto.EmpresaID = @EmpresaID and mvto.IdentificadorTR is not null " +
                    "		and ctrl.DocumentoID in(160,161,164,165,166,167,168,180) " +
                    ") as mvto " +
                    "	inner join " +
                    "	( " +
                    "		select distinct NumeroDoc, Fecha from coAuxiliar aux with(nolock) where CAST(Fecha as DATE) = @Fecha " +
                    "	) as aux on mvto.CtrlOrigen = aux.NumeroDoc " +
                    "order by NumDocCredito";

                #endregion
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.Date);
                mySqlCommand.Parameters.Add("@DocumentoCredito", SqlDbType.Int);
                #endregion
                #region Asignación de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Fecha"].Value = fecha;
                mySqlCommand.Parameters["@DocumentoCredito"].Value = AppDocuments.LiquidacionCredito;
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCierreDia cierre = new DTO_ccCierreDia(dr, true);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreDia_GetMovimientos");
                throw exception;
            }

        }

        /// <summary>
        /// Carga la informacion de los timpos de componentes y la ubicacion donde guarda la info en la tabla de movimientos
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<int>> DAL_ccCierreDia_GetComponentes()
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                Dictionary<int, List<int>> results = new Dictionary<int, List<int>>();

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "select distinct comp.TipoComponente, cs.NumeroComp " +
                    "from ccCarteraComponente comp with(nolock) " +
                    "	inner join glConceptoSaldo cs with(nolock) on comp.ConceptoSaldoID = cs.ConceptoSaldoID and comp.eg_glConceptoSaldo = cs.EmpresaGrupoID " +
                    "where comp.EmpresaGrupoID = @EmpresaGrupoID " +
                    "order by TipoComponente ";

                int tipoComp = 0;
                int lastTipoComp = 0;
                List<int> numComps = new List<int>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    tipoComp = Convert.ToInt32(dr["TipoComponente"]);
                    int numComp = Convert.ToInt32(dr["NumeroComp"]);

                    //Si es nuevo agrego la lista anterior
                    if (tipoComp != lastTipoComp)
                    {
                        //Agrega el ultimo
                        if (lastTipoComp != 0)
                            results[lastTipoComp] = numComps;

                        //Actualiza variables
                        lastTipoComp = tipoComp;
                        numComps = new List<int>();

                        //Agrega el registro al diccionario
                        results.Add(tipoComp, numComps);
                    }

                    numComps.Add(numComp);
                }
                dr.Close();

                //Asigna la ultima lista
                if (lastTipoComp != 0)
                    results[tipoComp] = numComps;

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreDia_GetComponentes");
                throw exception;
            }

        }

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public List<DTO_ccCierreDia> DAL_ccCierreDia_GetAll(DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_ccCierreDia> cierres = new List<DTO_ccCierreDia>();
                #region Query
                mySqlCommand.CommandText =
                    "select * " +
                    "from ccCierreDia with(nolock) " +
                    "where EmpresaID = @EmpresaID AND Periodo = @Periodo ";

                #endregion
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.Date);
                #endregion
                #region Asignación de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCierreDia cierre = new DTO_ccCierreDia(dr, true);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreMes_GetAll");
                throw exception;
            }

        }

        /// <summary>
        /// Carga la información para hacer un cierre diario
        /// </summary>
        /// <param name="fecha">Fecha de cierre</param>
        /// <param name="balanceFunc">Balance funcional</param>
        /// <returns></returns>
        public List<DTO_ccCierreDia> DAL_ccCierreDia_GetForCierreMes(DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_ccCierreDia> cierres = new List<DTO_ccCierreDia>();
                #region Query
                mySqlCommand.CommandText =
                    "select EmpresaID,Periodo,DocumentoID,LineaCreditoID,AsesorID,CentroPagoID,ZonaID,CompradorCarteraID,Plazo,TipoDato, " +
                    "	SUM " +
                    "	( " +
                    "		ISNULL(ValorDia01,0)+ISNULL(ValorDia02,0)+ISNULL(ValorDia03,0)+ISNULL(ValorDia04,0)+ISNULL(ValorDia05,0)+ISNULL(ValorDia06,0)+ " +
                    "		ISNULL(ValorDia07,0)+ISNULL(ValorDia08,0)+ISNULL(ValorDia09,0)+ISNULL(ValorDia10,0)+ISNULL(ValorDia11,0)+ISNULL(ValorDia12,0)+ " +
                    "		ISNULL(ValorDia13,0)+ISNULL(ValorDia14,0)+ISNULL(ValorDia15,0)+ISNULL(ValorDia16,0)+ISNULL(ValorDia17,0)+ISNULL(ValorDia18,0)+ " +
                    "		ISNULL(ValorDia19,0)+ISNULL(ValorDia20,0)+ISNULL(ValorDia21,0)+ISNULL(ValorDia22,0)+ISNULL(ValorDia23,0)+ISNULL(ValorDia24,0)+ " +
                    "		ISNULL(ValorDia25,0)+ISNULL(ValorDia26,0)+ISNULL(ValorDia27,0)+ISNULL(ValorDia28,0)+ISNULL(ValorDia29,0)+ISNULL(ValorDia30,0)+ " +
                    "       ISNULL(ValorDia31,0) " +
                    "	) AS 'ValorMes' " +
                    "from ccCierreDia with(nolock) " +
                    "where EmpresaID = @EmpresaID AND Periodo = @Periodo " +
                    "group by EmpresaID,Periodo,DocumentoID,LineaCreditoID,AsesorID,CentroPagoID,ZonaID,CompradorCarteraID,Plazo,TipoDato";

                #endregion
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.Date);
                #endregion
                #region Asignación de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCierreDia cierre = new DTO_ccCierreDia(dr, false);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreMes_GetAll");
                throw exception;
            }

        }

        ///// <summary>
        ///// Carga todos los cierres diarios de un periodo
        ///// </summary>
        ///// <param name="periodo">periodo de consulta</param>
        ///// <returns></returns>
        //public DTO_TxResult DAL_ccCierreDia_Procesar(DateTime periodo, DateTime fechaCierre)
        //{
        //    DTO_TxResult result = new DTO_TxResult();
        //    result.Result = ResultValue.OK;
        //    result.Details = new List<DTO_TxResultDetail>();

        //    try
        //    {
        //        List<DTO_ccCierreDiaCartera> cierres = new List<DTO_ccCierreDiaCartera>();

        //        string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

        //        SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
        //        mySqlCommand.Transaction = base.MySqlConnectionTx;

        //        #region Creación de parametros

        //        mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
        //        mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
        //        mySqlCommand.Parameters.Add("@FechaCierre", SqlDbType.SmallDateTime);
        //        mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Int);
        //        mySqlCommand.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
        //        mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
        //        mySqlCommand.Parameters.Add("@eg_ccCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
        //        mySqlCommand.Parameters.Add("@eg_glActividadFlujo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
        //        mySqlCommand.Parameters.Add("@eg_ccAbogado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
        //        mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
        //        mySqlCommand.Parameters.Add("@eg_glIncumplimientoEtapa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

        //        #endregion
        //        #region Asignacion de parametros

        //        mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
        //        mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
        //        mySqlCommand.Parameters["@FechaCierre"].Value = fechaCierre;
        //        mySqlCommand.Parameters["@seUsuarioID"].Value = this.UserId;
        //        mySqlCommand.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
        //        mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
        //        mySqlCommand.Parameters["@eg_ccCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCliente, this.Empresa, egCtrl);
        //        mySqlCommand.Parameters["@eg_glActividadFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl);
        //        mySqlCommand.Parameters["@eg_ccAbogado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAbogado, this.Empresa, egCtrl);
        //        mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
        //        mySqlCommand.Parameters["@eg_glIncumplimientoEtapa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glIncumplimientoEtapa, this.Empresa, egCtrl);

        //        #endregion

        //        mySqlCommand.CommandType = CommandType.StoredProcedure;
        //        mySqlCommand.CommandText = "Cartera_CierreDiario";
        //        mySqlCommand.CommandTimeout = 0;

        //        SqlDataReader dr = mySqlCommand.ExecuteReader();
        //        DTO_TxResultDetail rd = new DTO_TxResultDetail();
        //        rd.line = 0;
        //        while (dr.Read())
        //        {
        //            if (dr.GetName(0) == "Linea")
        //            {
        //                #region Carga el error
        //                result.Result = ResultValue.NOK;

        //                rd = new DTO_TxResultDetail();
        //                rd.line = Convert.ToInt32(dr["Linea"]);

        //                switch (dr["CodigoError"].ToString())
        //                {
        //                    case "001": // Falta datos en glControl
        //                        rd.Message = DictionaryMessages.Err_ControlNoData + "&&" + dr["Valor"].ToString() + "&&" + string.Empty;
        //                        break;
        //                    default: // (999): Error del SP 
        //                        rd.Message = dr["Valor"].ToString();
        //                        break;
        //                }

        //                #endregion
        //                result.Details.Add(rd);
        //            }
        //        }
        //        dr.Close();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Result = ResultValue.NOK;
        //        result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_ccCierreMes_Procesar");
        //        return result;
        //    }
        //}

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public DTO_TxResult DAL_ccCierreDia_ProcesarFin(DateTime periodo, DateTime fechaCierre)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                List<DTO_ccCierreDiaCartera> cierres = new List<DTO_ccCierreDiaCartera>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creación de parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaCierre", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glActividadFlujo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccAbogado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glIncumplimientoEtapa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo; 
                mySqlCommand.Parameters["@FechaCierre"].Value = fechaCierre;
                mySqlCommand.Parameters["@seUsuarioID"].Value = this.UserId;
                mySqlCommand.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCliente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glActividadFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccAbogado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAbogado, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glIncumplimientoEtapa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glIncumplimientoEtapa, this.Empresa, egCtrl);

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "CarteraFin_CierreDiario";
                mySqlCommand.CommandTimeout = 0;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                while (dr.Read())
                {
                    if (dr.GetName(0) == "Linea")
                    {
                        #region Carga el error
                        result.Result = ResultValue.NOK;

                        rd = new DTO_TxResultDetail();
                        rd.line = Convert.ToInt32(dr["Linea"]);

                        switch (dr["CodigoError"].ToString())
                        {
                            case "001": // Falta datos en glControl
                                rd.Message = DictionaryMessages.Err_ControlNoData + "&&" + dr["Valor"].ToString() + "&&" + string.Empty;
                                break;
                            default: // (999): Error del SP 
                                rd.Message = dr["Valor"].ToString();
                                break;
                        }

                        #endregion
                        result.Details.Add(rd);
                    }
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_ccCierreMes_Procesar");
                return result;
            }
        }

        /// <summary>
        /// Actualiza un registro de ccCierreDiaCartera
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_ccCierreDiaCartera_UpdateConsEstadoIncump(int consecutivo, int consEstadoIncumplimiento)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
          
                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE ccCierreDiaCartera SET ConsEstadoIncumplido =  @ConsEstadoIncumplido" +
                    "WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsEstadoIncumplido", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@Consecutivo"].Value = consecutivo;
                mySqlCommandSel.Parameters["@ConsEstadoIncumplido"].Value = consEstadoIncumplimiento;

                #endregion
               
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreDiaCartera_UpdateConsEstadoIncump");
                throw exception;
            }
        }

        /// <summary>
        /// Carga todos los cierres mes con uno o varios filtros
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns></returns>
        public List<DTO_ccCierreDiaCartera> DAL_ccCierreDiaCartera_GetByParameter(DTO_ccCierreDiaCartera filter)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_ccCierreDiaCartera> cierres = new List<DTO_ccCierreDiaCartera>(); 
                string query;
                bool filterInd = false;

                query = "select * from ccCierreDiaCartera with(nolock) where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.Fecha.Value.ToString()))
                {
                    query += "and Fecha = @Fecha ";
                    mySqlCommand.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@Fecha"].Value = filter.Fecha.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ClienteID.Value))
                {
                    query += "and ClienteID = @ClienteID ";
                    mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommand.Parameters["@ClienteID"].Value = filter.ClienteID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.NumDocControlIncumplido.Value.ToString()))
                {
                    query += "and NumDocControlIncumplido = @NumDocControlIncumplido ";
                    mySqlCommand.Parameters.Add("@NumDocControlIncumplido", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumDocControlIncumplido"].Value = filter.NumDocControlIncumplido.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.EstadoDeuda.Value.ToString()))
                {
                    query += "and EstadoDeuda = @EstadoDeuda ";
                    mySqlCommand.Parameters.Add("@EstadoDeuda", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EstadoDeuda"].Value = filter.EstadoDeuda.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.FechaMora.Value.ToString()))
                {
                    query += "and FechaMora = @FechaMora ";
                    mySqlCommand.Parameters.Add("@FechaMora", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@FechaMora"].Value = filter.FechaMora.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ConsEstadoIncumplido.Value.ToString()))
                {
                    query += "and ConsEstadoIncumplido = @ConsEstadoIncumplido ";
                    mySqlCommand.Parameters.Add("@ConsEstadoIncumplido", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsEstadoIncumplido"].Value = filter.ConsEstadoIncumplido.Value;
                    filterInd = true;
                }
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return cierres;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ccCierreDiaCartera cierre = new DTO_ccCierreDiaCartera(dr);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreDiaCartera_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Carga el ultimo dia cierre con varios filtros
        /// </summary>
        /// <param name="cliente">cliente</param>
        /// <param name="libranza">libranza</param>
        /// <returns>Dto</returns>
        public DTO_ccCierreDiaCartera DAL_ccCierreDiaCartera_GetUltimoDiaCierre(string cliente, int? numDocCredito)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;

                query = " Select top(1)* from ccCierreDiaCartera with(nolock) where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(cliente))
                {
                    query += "and ClienteID = @ClienteID ";
                    mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommand.Parameters["@ClienteID"].Value = cliente;
                }
                if (!string.IsNullOrEmpty(numDocCredito.ToString()))
                {
                    query += "and NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = numDocCredito;
                }
                query += "order by Fecha Desc ";
                mySqlCommand.CommandText = query;
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                DTO_ccCierreDiaCartera cierre = null;
                if (dr.Read())
                {
                    cierre = new DTO_ccCierreDiaCartera(dr);
                }
                dr.Close();

                return cierre;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreDiaCartera_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Carga la información para hacer un cierre diario
        /// </summary>
        /// <param name="fecha">Fecha de cierre</param>
        /// <param name="balanceFunc">Balance funcional</param>
        /// <returns></returns>
        public List<DTO_GestionCobranza> DAL_ccCierreDiaCartera_GetGestionCobranza(string cliente, DateTime fechaCierre, string actividadGestCobranza)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_GestionCobranza> cierres = new List<DTO_GestionCobranza>();
                #region Query
                string query = string.Empty;
                if (!string.IsNullOrEmpty(cliente))
                {
                    query += " and cd.ClienteID = @ClienteID ";
                    mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommand.Parameters["@ClienteID"].Value = cliente;
                }
                mySqlCommand.CommandText =
                " SELECT cr.NumeroDoc, cr.Libranza,cr.Plazo,cr.NumSolicitud,cr.CobranzaEstadoID,cr.CobranzaGestionCierre,cr.SiniestroEstadoID, cd.Consecutivo,cd.CtasVencidas,cd.FechaMora,cd.DiasMora, cd.NumDocControlIncumplido,  " +
                "       cd.SaldoVencido,cd.VlrIntMora,cd.VlrPrejuridico,cd.VlrOtros, SUM(cd.SaldoVencido + cd.VlrIntMora + cd.VlrPrejuridico + cd.VlrOtros) as TotalLibranza,  " +
                "       cd.ClienteID,cl.Descriptivo as ClienteDesc,cl.TerceroID, " +
                "	   0 as Numero1,act.FechaAlarma1 as FechaCompromiso, IsNull(act.Valor, 0) as Valor,act.Observaciones as ObservacionCompromiso,'' as ConsEstadoIncumplido,  " +
                "       ctrl.Observacion as ObservacionHistoria,IsNull(act.cerradoInd, 0) as CumplidoInd " +
                " FROM ccCierreDiaCartera cd WITH(NOLOCK) " +
                "     INNER JOIN ccCreditoDocu cr WITH(NOLOCK) ON cr.NumeroDoc = cd.NumeroDoc " +
                "     INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = cr.NumeroDoc " +
                "     INNER JOIN ccCliente cl WITH(NOLOCK) ON cl.ClienteID = cd.ClienteID " +
                "     INNER JOIN ccProfesion pr WITH(NOLOCK) ON pr.ProfesionID = cl.ProfesionID " +
                "     LEFT JOIN glActividadEstado act WITH(NOLOCK) ON act.NumeroDoc = ctrl.NumeroDoc   and act.ActividadFlujoID = @ActividadFlujoCob " +
                " WHERE  cd.Fecha = @diaCierre   " + query +
                " GROUP BY cr.NumeroDoc, cr.Libranza, cr.Plazo, cr.NumSolicitud, cr.CobranzaEstadoID, cr.CobranzaGestionCierre,cr.SiniestroEstadoID, cd.Consecutivo, cd.CtasVencidas, cd.FechaMora, cd.DiasMora, cd.NumDocControlIncumplido, " +
                "      cd.SaldoVencido, cd.VlrIntMora, cd.VlrPrejuridico, cd.VlrOtros, " +
                "      cd.ClienteID, cl.Descriptivo, cl.TerceroID, act.FechaAlarma1, act.Valor, act.Observaciones, ctrl.Observacion, act.cerradoInd ";
                #endregion
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@diaCierre", SqlDbType.Date);
                mySqlCommand.Parameters.Add("@ActividadFlujoCob", SqlDbType.Char,UDT_ActividadFlujoID.MaxLength);
                #endregion
                #region Asignación de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@diaCierre"].Value = fechaCierre;
                mySqlCommand.Parameters["@ActividadFlujoCob"].Value = actividadGestCobranza;
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_GestionCobranza cierre = new DTO_GestionCobranza(dr);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreDiaCartera_GetGestionCobranza");
                throw exception;
            }

        }

        #endregion

    }
}
