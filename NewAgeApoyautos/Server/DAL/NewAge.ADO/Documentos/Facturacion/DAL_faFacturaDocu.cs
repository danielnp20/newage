using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_FacturaDocu
    /// </summary>
    public class DAL_faFacturaDocu : DAL_Base
    {
       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_faFacturaDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Consulta una Factura segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_faFacturaDocu DAL_faFacturaDocu_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from faFacturaDocu with(nolock) where faFacturaDocu.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_faFacturaDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_faFacturaDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_faFacturaDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla faFacturaDocu 
        /// </summary>
        /// <param name="fact">Factura</param>
        /// <returns></returns>
        public void DAL_faFacturaDocu_Add(DTO_faFacturaDocu fact)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO faFacturaDocu " +
                                           "    (EmpresaID " +
                                           "    ,NumeroDoc " +
                                           "    ,AsesorID " +
                                           "    ,DocumentoREL " +
                                           "    ,NotaEnvioREL " +
                                           "    ,FacturaREL " +
                                           "    ,ClienteID " +
                                           "    ,ListaPrecioID " +
                                           "    ,ZonaID " +
                                           "    ,FacturaTipoID " +
                                           "    ,Porcentaje1 " +
                                           "    ,Porcentaje2 " +
                                           "    ,MonedaPago " +
                                           "    ,TasaPago " +
                                           "    ,FechaVto " +
                                           "    ,ObservacionENC " +
                                           "    ,ObservacionPIE " +
                                           "    ,FormaPago " +
                                           "    ,Valor " +
                                           "    ,Iva " +
                                           "    ,Bruto " +
                                           "    ,PorcPtoPago " +
                                           "    ,PorcRteGarantia " +
                                           "    ,FechaPtoPago " +
                                           "    ,ValorPtoPago " +
                                           "    ,Retencion1 " +
                                           "    ,Retencion2 " +
                                           "    ,Retencion3 " +
                                           "    ,Retencion4 " +
                                           "    ,Retencion5 " +
                                           "    ,Retencion6 " +
                                           "    ,Retencion7 " +
                                           "    ,Retencion8 " +
                                           "    ,Retencion9 " +
                                           "    ,Retencion10 " +
                                           "    ,DatoAdd1 " +
                                           "    ,DatoAdd2 " +
                                           "    ,DatoAdd3 " +
                                           "    ,DatoAdd4 " +
                                           "    ,DatoAdd5 " +
                                           "    ,FacturaFijaInd " +
                                           "    ,RteGarantiaIvaInd " +
                                           "    ,PorcAnticipo " +
                                           "    ,IncluyeIVAInd " +
                                           "    ,eg_faAsesor " +
                                           "    ,eg_faCliente " +
                                           "    ,eg_faListaPrecio " +
                                           "    ,eg_glZona " +
                                           "    ,eg_faFacturaTipo) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@AsesorID " +
                                           "    ,@DocumentoREL " +
                                           "    ,@NotaEnvioREL " +
                                           "    ,@FacturaREL " +
                                           "    ,@ClienteID " +
                                           "    ,@ListaPrecioID " +
                                           "    ,@ZonaID " +
                                           "    ,@FacturaTipoID " +
                                           "    ,@Porcentaje1 " +
                                           "    ,@Porcentaje2 " +
                                           "    ,@MonedaPago " +
                                           "    ,@TasaPago " +
                                           "    ,@FechaVto " +
                                           "    ,@ObservacionENC " +
                                           "    ,@ObservacionPIE " +
                                           "    ,@FormaPago " +
                                           "    ,@Valor " +
                                           "    ,@Iva " +
                                           "    ,@Bruto " +
                                           "    ,@PorcPtoPago " +
                                           "    ,@PorcRteGarantia " +                                         
                                           "    ,@FechaPtoPago " +
                                           "    ,@ValorPtoPago " +
                                           "    ,@Retencion1 " +
                                           "    ,@Retencion2 " +
                                           "    ,@Retencion3 " +
                                           "    ,@Retencion4 " +
                                           "    ,@Retencion5 " +
                                           "    ,@Retencion6 " +
                                           "    ,@Retencion7 " +
                                           "    ,@Retencion8 " +
                                           "    ,@Retencion9 " +
                                           "    ,@Retencion10 " +
                                           "    ,@DatoAdd1 " +
                                           "    ,@DatoAdd2 " +
                                           "    ,@DatoAdd3 " +
                                           "    ,@DatoAdd4 " +
                                           "    ,@DatoAdd5 " +
                                           "    ,@FacturaFijaInd " +
                                           "    ,@RteGarantiaIvaInd " +
                                           "    ,@PorcAnticipo " +
                                           "    ,@IncluyeIVAInd " +
                                           "    ,@eg_faAsesor " +
                                           "    ,@eg_faCliente " +
                                           "    ,@eg_faListaPrecio " +
                                           "    ,@eg_glZona " +
                                           "    ,@eg_faFacturaTipo) ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoREL", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NotaEnvioREL", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FacturaREL", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@ListaPrecioID", SqlDbType.Char, UDT_ListaPrecioID.MaxLength);
                mySqlCommand.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommand.Parameters.Add("@FacturaTipoID", SqlDbType.Char, UDT_FacturaTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@Porcentaje1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Porcentaje2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@TasaPago", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechaVto", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ObservacionENC", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@ObservacionPIE", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@FormaPago", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Bruto", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorcPtoPago", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorcRteGarantia", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FechaPtoPago", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ValorPtoPago", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion5", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion6", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion7", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion8", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion9", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Retencion10", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@FacturaFijaInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@RteGarantiaIvaInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@PorcAnticipo", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IncluyeIVAInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@eg_faAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faListaPrecio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glZona", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faFacturaTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = fact.NumeroDoc.Value;
                mySqlCommand.Parameters["@AsesorID"].Value = fact.AsesorID.Value;
                mySqlCommand.Parameters["@DocumentoREL"].Value = fact.DocumentoREL.Value;
                mySqlCommand.Parameters["@NotaEnvioREL"].Value = fact.NotaEnvioREL.Value;
                mySqlCommand.Parameters["@FacturaREL"].Value = fact.FacturaREL.Value;
                mySqlCommand.Parameters["@ClienteID"].Value = fact.ClienteID.Value;
                mySqlCommand.Parameters["@ListaPrecioID"].Value = fact.ListaPrecioID.Value;
                mySqlCommand.Parameters["@ZonaID"].Value = fact.ZonaID.Value;
                mySqlCommand.Parameters["@FacturaTipoID"].Value = fact.FacturaTipoID.Value;
                mySqlCommand.Parameters["@Porcentaje1"].Value = fact.Porcentaje1.Value;
                mySqlCommand.Parameters["@Porcentaje2"].Value = fact.Porcentaje2.Value;
                mySqlCommand.Parameters["@MonedaPago"].Value = fact.MonedaPago.Value;
                mySqlCommand.Parameters["@TasaPago"].Value = fact.TasaPago.Value;
                mySqlCommand.Parameters["@FechaVto"].Value = fact.FechaVto.Value;
                mySqlCommand.Parameters["@ObservacionENC"].Value = fact.ObservacionENC.Value;
                mySqlCommand.Parameters["@ObservacionPIE"].Value = fact.ObservacionPIE.Value;
                mySqlCommand.Parameters["@FormaPago"].Value = fact.FormaPago.Value;
                mySqlCommand.Parameters["@Valor"].Value = fact.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = fact.Iva.Value;
                mySqlCommand.Parameters["@Bruto"].Value = fact.Bruto.Value;
                mySqlCommand.Parameters["@PorcPtoPago"].Value = fact.PorcPtoPago.Value;
                mySqlCommand.Parameters["@PorcRteGarantia"].Value = fact.PorcRteGarantia.Value;
                mySqlCommand.Parameters["@FechaPtoPago"].Value = fact.FechaPtoPago.Value;
                mySqlCommand.Parameters["@ValorPtoPago"].Value = fact.ValorPtoPago.Value;
                mySqlCommand.Parameters["@Retencion1"].Value = fact.Retencion1.Value;
                mySqlCommand.Parameters["@Retencion2"].Value = fact.Retencion2.Value;
                mySqlCommand.Parameters["@Retencion3"].Value = fact.Retencion3.Value;
                mySqlCommand.Parameters["@Retencion4"].Value = fact.Retencion4.Value;
                mySqlCommand.Parameters["@Retencion5"].Value = fact.Retencion5.Value;
                mySqlCommand.Parameters["@Retencion6"].Value = fact.Retencion6.Value;
                mySqlCommand.Parameters["@Retencion7"].Value = fact.Retencion7.Value;
                mySqlCommand.Parameters["@Retencion8"].Value = fact.Retencion8.Value;
                mySqlCommand.Parameters["@Retencion9"].Value = fact.Retencion9.Value;
                mySqlCommand.Parameters["@Retencion10"].Value = fact.Retencion10.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = fact.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = fact.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = fact.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = fact.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = fact.DatoAdd5.Value;
                mySqlCommand.Parameters["@FacturaFijaInd"].Value = fact.FacturaFijaInd.Value;
                mySqlCommand.Parameters["@RteGarantiaIvaInd"].Value = fact.RteGarantiaIvaInd.Value;
                mySqlCommand.Parameters["@PorcAnticipo"].Value = fact.PorcAnticipo.Value;
                mySqlCommand.Parameters["@IncluyeIVAInd"].Value = fact.IncluyeIVAInd.Value;
                mySqlCommand.Parameters["@eg_faAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faAsesor, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faCliente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faListaPrecio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faListaPrecio, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faFacturaTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faFacturaTipo, this.Empresa, egCtrl);
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
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@NumeroDoc"].Value);

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_faFacturaDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar la factura en tabla faFacturaDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">factura</param>
        public void DAL_faFacturaDocu_Upd(DTO_faFacturaDocu fact, bool OnlyFacturaFija)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                if (!OnlyFacturaFija)
                {
                    //Actualiza Tabla faFacturaDocu
                    #region CommandText
                    mySqlCommand.CommandText = "    UPDATE faFacturaDocu " +
                                               "    SET EmpresaID  = @EmpresaID  " +
                                               "    ,AsesorID  = @AsesorID " +
                                               "    ,DocumentoREL  = @DocumentoREL " +
                                               "    ,NotaEnvioREL  = @NotaEnvioREL " +
                                               "    ,FacturaREL  = @FacturaREL " +
                                               "    ,ClienteID  = @ClienteID " +
                                               "    ,ListaPrecioID  = @ListaPrecioID " +
                                               "    ,ZonaID  = @ZonaID " +
                                               "    ,FacturaTipoID  = @FacturaTipoID " +
                                               "    ,Porcentaje1  = @Porcentaje1 " +
                                               "    ,Porcentaje2  = @Porcentaje2 " +
                                               "    ,MonedaPago  = @MonedaPago " +
                                               "    ,TasaPago  = @TasaPago " +
                                               "    ,FechaVto  = @FechaVto " +
                                               "    ,ObservacionENC  = @ObservacionENC " +
                                               "    ,ObservacionPIE  = @ObservacionPIE " +
                                               "    ,FormaPago  = @FormaPago " +
                                               "    ,Valor  = @Valor " +
                                               "    ,Iva  = @Iva " +
                                               "    ,Bruto  = @Bruto " +
                                               "    ,PorcPtoPago  = @PorcPtoPago " +
                                               "    ,PorcRteGarantia  = @PorcRteGarantia " +
                                               "    ,FechaPtoPago  = @FechaPtoPago " +
                                               "    ,ValorPtoPago  = @ValorPtoPago " +
                                               "    ,Retencion1  = @Retencion1 " +
                                               "    ,Retencion2  = @Retencion2 " +
                                               "    ,Retencion3  = @Retencion3 " +
                                               "    ,Retencion4  = @Retencion4 " +
                                               "    ,Retencion5  = @Retencion5 " +
                                               "    ,Retencion6  = @Retencion6 " +
                                               "    ,Retencion7  = @Retencion7 " +
                                               "    ,Retencion8  = @Retencion8 " +
                                               "    ,Retencion9  = @Retencion9 " +
                                               "    ,Retencion10  = @Retencion10 " +
                                               "    ,DatoAdd1  = @DatoAdd1 " +
                                               "    ,DatoAdd2  = @DatoAdd2 " +
                                               "    ,DatoAdd3  = @DatoAdd3 " +
                                               "    ,DatoAdd4  = @DatoAdd4 " +
                                               "    ,DatoAdd5  = @DatoAdd5 " +
                                               "    ,FacturaFijaInd  = @FacturaFijaInd " +
                                               "    ,RteGarantiaIvaInd  = @RteGarantiaIvaInd " +
                                               "    ,PorcAnticipo  = @PorcAnticipo " +
                                               "    ,IncluyeIVAInd  = @IncluyeIVAInd " +
                                               "    WHERE NumeroDoc = @NumeroDoc";

                    #endregion
                    #region Creacion de parametros
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                    mySqlCommand.Parameters.Add("@DocumentoREL", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@NotaEnvioREL", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@FacturaREL", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommand.Parameters.Add("@ListaPrecioID", SqlDbType.Char, UDT_ListaPrecioID.MaxLength);
                    mySqlCommand.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                    mySqlCommand.Parameters.Add("@FacturaTipoID", SqlDbType.Char, UDT_FacturaTipoID.MaxLength);
                    mySqlCommand.Parameters.Add("@Porcentaje1", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Porcentaje2", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                    mySqlCommand.Parameters.Add("@TasaPago", SqlDbType.TinyInt);
                    mySqlCommand.Parameters.Add("@FechaVto", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters.Add("@ObservacionENC", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                    mySqlCommand.Parameters.Add("@ObservacionPIE", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                    mySqlCommand.Parameters.Add("@FormaPago", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                    mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Bruto", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@PorcPtoPago", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@PorcRteGarantia", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@FechaPtoPago", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters.Add("@ValorPtoPago", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion1", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion2", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion3", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion4", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion5", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion6", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion7", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion8", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion9", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@Retencion10", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                    mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                    mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                    mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                    mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                    mySqlCommand.Parameters.Add("@FacturaFijaInd", SqlDbType.Bit);
                    mySqlCommand.Parameters.Add("@RteGarantiaIvaInd", SqlDbType.Bit);
                    mySqlCommand.Parameters.Add("@PorcAnticipo", SqlDbType.Decimal);
                    mySqlCommand.Parameters.Add("@IncluyeIVAInd", SqlDbType.Bit);
                    #endregion
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@NumeroDoc"].Value = fact.NumeroDoc.Value;
                    mySqlCommand.Parameters["@AsesorID"].Value = fact.AsesorID.Value;
                    mySqlCommand.Parameters["@DocumentoREL"].Value = fact.DocumentoREL.Value;
                    mySqlCommand.Parameters["@NotaEnvioREL"].Value = fact.NotaEnvioREL.Value;
                    mySqlCommand.Parameters["@FacturaREL"].Value = fact.FacturaREL.Value;
                    mySqlCommand.Parameters["@ClienteID"].Value = fact.ClienteID.Value;
                    mySqlCommand.Parameters["@ListaPrecioID"].Value = fact.ListaPrecioID.Value;
                    mySqlCommand.Parameters["@ZonaID"].Value = fact.ZonaID.Value;
                    mySqlCommand.Parameters["@FacturaTipoID"].Value = fact.FacturaTipoID.Value;
                    mySqlCommand.Parameters["@Porcentaje1"].Value = fact.Porcentaje1.Value;
                    mySqlCommand.Parameters["@Porcentaje2"].Value = fact.Porcentaje2.Value;
                    mySqlCommand.Parameters["@MonedaPago"].Value = fact.MonedaPago.Value;
                    mySqlCommand.Parameters["@TasaPago"].Value = fact.TasaPago.Value;
                    mySqlCommand.Parameters["@FechaVto"].Value = fact.FechaVto.Value;
                    mySqlCommand.Parameters["@ObservacionENC"].Value = fact.ObservacionENC.Value;
                    mySqlCommand.Parameters["@ObservacionPIE"].Value = fact.ObservacionPIE.Value;
                    mySqlCommand.Parameters["@FormaPago"].Value = fact.FormaPago.Value;
                    mySqlCommand.Parameters["@Valor"].Value = fact.Valor.Value;
                    mySqlCommand.Parameters["@Iva"].Value = fact.Iva.Value;
                    mySqlCommand.Parameters["@Bruto"].Value = fact.Bruto.Value;
                    mySqlCommand.Parameters["@PorcPtoPago"].Value = fact.PorcPtoPago.Value;
                    mySqlCommand.Parameters["@PorcRteGarantia"].Value = fact.PorcRteGarantia.Value;
                    mySqlCommand.Parameters["@FechaPtoPago"].Value = fact.FechaPtoPago.Value;
                    mySqlCommand.Parameters["@ValorPtoPago"].Value = fact.ValorPtoPago.Value;
                    mySqlCommand.Parameters["@Retencion1"].Value = fact.Retencion1.Value;
                    mySqlCommand.Parameters["@Retencion2"].Value = fact.Retencion2.Value;
                    mySqlCommand.Parameters["@Retencion3"].Value = fact.Retencion3.Value;
                    mySqlCommand.Parameters["@Retencion4"].Value = fact.Retencion4.Value;
                    mySqlCommand.Parameters["@Retencion5"].Value = fact.Retencion5.Value;
                    mySqlCommand.Parameters["@Retencion6"].Value = fact.Retencion6.Value;
                    mySqlCommand.Parameters["@Retencion7"].Value = fact.Retencion7.Value;
                    mySqlCommand.Parameters["@Retencion8"].Value = fact.Retencion8.Value;
                    mySqlCommand.Parameters["@Retencion9"].Value = fact.Retencion9.Value;
                    mySqlCommand.Parameters["@Retencion10"].Value = fact.Retencion10.Value;
                    mySqlCommand.Parameters["@DatoAdd1"].Value = fact.DatoAdd1.Value;
                    mySqlCommand.Parameters["@DatoAdd2"].Value = fact.DatoAdd2.Value;
                    mySqlCommand.Parameters["@DatoAdd3"].Value = fact.DatoAdd3.Value;
                    mySqlCommand.Parameters["@DatoAdd4"].Value = fact.DatoAdd4.Value;
                    mySqlCommand.Parameters["@DatoAdd5"].Value = fact.DatoAdd5.Value;
                    mySqlCommand.Parameters["@FacturaFijaInd"].Value = fact.FacturaFijaInd.Value;
                    mySqlCommand.Parameters["@RteGarantiaIvaInd"].Value = fact.RteGarantiaIvaInd.Value;
                    mySqlCommand.Parameters["@PorcAnticipo"].Value = fact.PorcAnticipo.Value;
                    mySqlCommand.Parameters["@IncluyeIVAInd"].Value = fact.IncluyeIVAInd.Value;
                    #endregion
                }
                else
                {
                    #region CommandText
                    mySqlCommand.CommandText = "    UPDATE faFacturaDocu " +
                                               "    SET EmpresaID  = @EmpresaID  " +                                              
                                               "    ,FacturaFijaInd  = @FacturaFijaInd " +
                                               "    WHERE NumeroDoc = @NumeroDoc";

                    #endregion
                    #region Creacion de parametros
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);       
                    mySqlCommand.Parameters.Add("@FacturaFijaInd", SqlDbType.Bit);
                    #endregion
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@NumeroDoc"].Value = fact.NumeroDoc.Value;     
                    mySqlCommand.Parameters["@FacturaFijaInd"].Value = fact.FacturaFijaInd.Value;
                    #endregion 
                }
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_faFacturaDocu_Upd");
                throw exception;
            }

        }

        /// <summary>
        /// Trae un listado de facturas pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_faFacturacionAprobacion> DAL_faFacturaDocu_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID, string usuarioID)
        {
            try
            {
                List<DTO_faFacturacionAprobacion> result = new List<DTO_faFacturacionAprobacion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "   select distinct ctrl.*, ctrl.PeriodoDoc as PeriodoID, ctrl.Observacion as ObservacionDoc, ctrl.ComprobanteIDNro as ComprobanteNro, " +
                    "   cl.ClienteID,cl.Descriptivo as ClienteDesc, usr.UsuarioID as UsuarioID,fact.FacturaTipoID, fact.Valor as vlrBruto, fact.Iva, fact.Retencion1, fact.Retencion2, " +
                    "	fact.Retencion3, fact.Retencion4, fact.Retencion5, fact.Retencion6, fact.Retencion7, fact.Retencion8, fact.Retencion9, fact.Retencion10 " +
                    "from glDocumentoControl ctrl with(nolock)  " +
                    "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "	inner join faFacturaDocu fact with(nolock) on ctrl.NumeroDoc = fact.NumeroDoc  " +
                    "	inner join faCliente cl with(nolock) on fact.ClienteID = cl.ClienteID and fact.eg_faCliente = cl.EmpresaGrupoID " +
                    "	inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                    "	inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "       and perm.UsuarioID = @UsuarioID and perm.AreaFuncionalID = ctrl.AreaFuncionalID  " +
                    "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and perm.ActividadFlujoID = @ActividadFlujoID ";

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
                    DTO_faFacturacionAprobacion dto = new DTO_faFacturacionAprobacion(dr);
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_faFacturaDocu_GetPendientesByModulo");
                throw exception;
            }
        } 

        #endregion

        #region OTRAS

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las facturas</param>
        /// <param name="terceroID">Tercero</param>
        /// <returns>Retorna una lista de facturas</returns>
        public List<DTO_faFacturacionResumen> DAL_faFacturaDocu_GetResumen(DateTime periodo, TipoMoneda tipoMoneda, string terceroID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.FacturaVenta;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;

                mySqlCommand.CommandText =
                    "select * from " +
                    "( " +
                    "	select doc.DocumentoID, doc.FechaDoc Fecha, doc.PrefijoID, doc.DocumentoNro, doc.MonedaID, cta.OrigenMonetario, " +
                    "		saldo.CuentaID, saldo.TerceroID, doc.DocumentoTercero, saldo.ProyectoID, saldo.CentroCostoID, " +
                    "		saldo.LineaPresupuestoID, saldo.ConceptoSaldoID, saldo.IdentificadorTR, saldo.ConceptoCargoID, " +
                    "		( " +
                    "			saldo.DbOrigenLocML + saldo.DbOrigenExtML + saldo.CrOrigenLocML + saldo.CrOrigenExtML + " +
                    "			saldo.DbSaldoIniLocML + saldo.DbSaldoIniExtML + saldo.CrSaldoIniLocML + saldo.CrSaldoIniExtML " +
                    "		) as ML, " +
                    "		( " +
                    "			saldo.DbOrigenLocME + saldo.DbOrigenExtME + saldo.CrOrigenLocME + saldo.CrOrigenExtME + " +
                    "			saldo.DbSaldoIniLocME + saldo.DbSaldoIniExtME + saldo.CrSaldoIniLocME + saldo.CrSaldoIniExtME	 " +
                    "		) as ME " +
                    "	from glDocumentoControl doc with(nolock) " +
                    "		inner join coCuentaSaldo saldo with(nolock) on doc.NumeroDoc = saldo.IdentificadorTR " +
                    "		inner join coPlanCuenta cta with(nolock) on saldo.CuentaID = cta.CuentaID " +
                    "	where doc.EmpresaID = @EmpresaID and doc.DocumentoID = @DocumentoID and saldo.PeriodoID = @PeriodoDoc " +
                    "       and saldo.TerceroID = @TerceroID " +
                    ") as res ";

                if (tipoMoneda == TipoMoneda.Local)
                    mySqlCommand.CommandText += "where ML != 0 ";
                else if (tipoMoneda == TipoMoneda.Foreign)
                    mySqlCommand.CommandText += "where ME != 0 ";
                else
                    mySqlCommand.CommandText += "where ML != 0 and ME != 0 ";

                mySqlCommand.CommandText += "order by IdentificadorTR ";

                List<DTO_faFacturacionResumen> result = new List<DTO_faFacturacionResumen>();
                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_faFacturacionResumen fact = new DTO_faFacturacionResumen(dr);
                    result.Add(fact);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_faFacturaDocu_GetResumen");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="clienteID">Periodo de consulta</param>
        /// <param name="NotaEnvioEmptyInd">filtrar por nota de envio</param>
        /// <returns>Retorna una lista de facturas</returns>
        public List<DTO_faFacturaDocu> DAL_faFacturaDocu_GetByCliente(string clienteID, bool NotaEnvioEmptyInd)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_TerceroID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ClienteID"].Value = clienteID;

                mySqlCommand.CommandText =
                    "select * from faFacturaDocu where EmpresaID = @EmpresaID and ClienteID = @ClienteID ";
                if (NotaEnvioEmptyInd)
                    mySqlCommand.CommandText += " and NotaEnvioREL is NULL";


                List<DTO_faFacturaDocu> result = new List<DTO_faFacturaDocu>();
                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_faFacturaDocu fact = new DTO_faFacturaDocu(dr);
                    result.Add(fact);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_faFacturaDocu_GetResumen");
                throw exception;
            }
        }


        public List<DTO_QueryHeadFactura> Consultar_Facturas(DateTime Ano, string terceroId, int tipoConsulta, string asesorID, string zonaID, string proyectoID, int TipoFact, string NumFact, string prefijoID, bool facturaFijaInd)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                List<DTO_QueryHeadFactura> results = new List<DTO_QueryHeadFactura>();
                
                #region Filtros

                string where = string.Empty;
                string tipoFactOP1 = " AND (saldo.DbOrigenLocML + saldo.DbOrigenExtML + saldo.CrOrigenLocML + saldo.CrOrigenExtML + saldo.DbSaldoIniLocML + saldo.DbSaldoIniExtML + saldo.CrSaldoIniLocML + saldo.CrSaldoIniExtML) * (-1) = 0";
                string tipoFactOP2 = " AND (saldo.DbOrigenLocML + saldo.DbOrigenExtML + saldo.CrOrigenLocML + saldo.CrOrigenExtML + saldo.DbSaldoIniLocML + saldo.DbSaldoIniExtML + saldo.CrSaldoIniLocML + saldo.CrSaldoIniExtML) * (-1) <> 0";

                switch (tipoConsulta)
                {
                    case 1:
                        where += " AND (docc.DocumentoID=@FacturaVenta OR docc.DocumentoID=@NotaCredito) ";
                        break;
                    case 2:
                        where = " AND docc.DocumentoID=@FacturaVenta ";
                        break;
                    case 3:
                        where = " AND docc.DocumentoID=@NotaCredito ";
                        break;
                }
                
                if (TipoFact == 1)
                    where += tipoFactOP1;
                if (TipoFact == 2)
                    where += tipoFactOP2;
                if (TipoFact == 3)
                    where += " AND (" + tipoFactOP1.Replace(" AND", "") + " OR " + tipoFactOP2.Replace(" AND", "") + ")";


                if (!string.IsNullOrEmpty(asesorID))
                    where = " AND fac.AsesorID=@AsesorID ";
                if (!string.IsNullOrEmpty(zonaID))
                    where = " AND fac.ZonaID=@ZonaID ";
                if (!string.IsNullOrEmpty(proyectoID))
                    where = " AND fac.ProyectoID=@ProyectoID ";
                if (!string.IsNullOrEmpty(terceroId))
                    where = " AND docc.TerceroID=@TerceroID ";
                if (Ano.Year!=0)
                    where = " AND YEAR(saldo.PeriodoID) = @ano ";
                if (!string.IsNullOrEmpty(NumFact))
                    where = " AND docc.DocumentoNro = @DocumentoNro ";
                if (!string.IsNullOrEmpty(prefijoID))
                    where = " AND docc.PrefijoID = @PrefijoID ";

                if(facturaFijaInd)
                    where = " AND fac.FacturaFijaInd = @FacturaFijaInd ";

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                "SELECT docc.NumeroDoc , docc.TerceroID, RTRIM(docc.PrefijoID) + '-' + cast(docc.DocumentoNro as VARCHAR(100)) as PrefDoc, " +
                    "ter.Descriptivo AS Nombre, fac.DatoAdd1,fac.FacturaFijaInd, " +
                    "fac.Bruto AS ValorBruto,fac.MonedaPago AS MdaPago,fac.Iva AS IVA,docc.Fecha,docc.Observacion, " +
                    "(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) * (-1) as SaldoLoc, " +
                    "docc.Valor, docc.PrefijoID AS Prefijo " +
                "FROM faFacturaDocu AS fac WITH (NOLOCK) " +
                "INNER JOIN glDocumentoControl AS docc WITH (NOLOCK) ON docc.NumeroDoc=fac.NumeroDoc " +
                "INNER JOIN coTercero AS ter WITH (NOLOCK) ON ter.TerceroID=docc.TerceroID AND ter.EmpresaGrupoID=docc.eg_coTercero " +
                "INNER JOIN coCuentaSaldo as saldo WITH (NOLOCK) ON  saldo.IdentificadorTR = docc.NumeroDoc " +
                "WHERE fac.EmpresaID = @EmpresaID " +  where;

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FacturaVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NotaCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char,UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FacturaFijaInd", SqlDbType.Bit);
                
                #endregion
                #region Asignacion de valores a parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FacturaVenta"].Value = AppDocuments.FacturaVenta;
                mySqlCommandSel.Parameters["@NotaCredito"].Value = AppDocuments.NotaCredito;
                mySqlCommandSel.Parameters["@AsesorID"].Value = asesorID;
                mySqlCommandSel.Parameters["@ZonaID"].Value = zonaID;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = proyectoID;
                mySqlCommandSel.Parameters["@ano"].Value = Ano.Year;
                mySqlCommandSel.Parameters["@TerceroID"].Value = terceroId;
                mySqlCommandSel.Parameters["@DocumentoNro"].Value = NumFact;
                mySqlCommandSel.Parameters["@PrefijoID"].Value = prefijoID;
                mySqlCommandSel.Parameters["@FacturaFijaInd"].Value = facturaFijaInd; 

                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                DTO_QueryHeadFactura doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_QueryHeadFactura(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_faFacturaDocu_GetResumen");
                throw exception;
            }

        }

        public List<DTO_QueryDetailFactura> Consultar_Facturas_Detalle(int idTr, DateTime periodo)
        {
            try
            {
                List<DTO_QueryDetailFactura> results = new List<DTO_QueryDetailFactura>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                    " SELECT ctrl.DocumentoID as DocumentoTipo, aux.DocumentoCOM, aux.PeriodoID as Fecha, Estado, " +
                    "   aux.ComprobanteID, aux.ComprobanteNro, ABS(aux.vlrMdaLoc) as vlrMdaLoc,  aux.IdentificadorTR, ctrl.NumeroDoc " +
                    " FROM coAuxiliar aux with(nolock)" +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = aux.NumeroDoc " +
                    " WHERE IdentificadorTR = @idTr " +
                    "   AND aux.PeriodoID <= @periodo";

                mySqlCommandSel.Parameters.Add("@idTr", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@periodo", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@idTr"].Value = idTr;
                mySqlCommandSel.Parameters["@periodo"].Value = periodo;

                DTO_QueryDetailFactura doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_QueryDetailFactura(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Consultar_Facturas_Detalle");
                throw exception;
            }

        }

        #endregion

    }
}
