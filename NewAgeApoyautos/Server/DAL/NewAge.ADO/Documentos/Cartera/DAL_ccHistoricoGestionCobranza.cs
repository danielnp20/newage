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
using System.ComponentModel;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_ccHistoricoGestionCobranza : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccHistoricoGestionCobranza(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de ccHistoricoGestionCobranza
        /// </summary>
        /// <returns>retorna una lista de ccHistoricoGestionCobranza</returns>
        public DTO_ccHistoricoGestionCobranza DAL_ccHistoricoGestionCobranza_GetByID(int consecutivo)
        {
            try
            {
                DTO_ccHistoricoGestionCobranza result = new DTO_ccHistoricoGestionCobranza();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;

                mySqlCommand.CommandText = "SELECT * FROM ccHistoricoGestionCobranza with(nolock)  " +
                                           "WHERE Consecutivo = @Consecutivo";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_ccHistoricoGestionCobranza(dr);

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccHistoricoGestionCobranza_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de ccHistoricoGestionCobranza
        /// </summary>
        /// <returns>retorna una lista de ccHistoricoGestionCobranza</returns>
        public List<DTO_ccHistoricoGestionCobranza> DAL_ccHistoricoGestionCobranza_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                 List<DTO_ccHistoricoGestionCobranza> results = new  List<DTO_ccHistoricoGestionCobranza>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccHistoricoGestionCobranza with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccHistoricoGestionCobranza dto = new DTO_ccHistoricoGestionCobranza(dr);
                    results.Add(dto);                
                }     
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccHistoricoGestionCobranza_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccHistoricoGestionCobranza_Add(DTO_ccHistoricoGestionCobranza historia)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText =  "  INSERT INTO ccHistoricoGestionCobranza   " +
                                               "    ([NumeroDoc] " +
                                               "    ,[Fecha] " +
                                               "    ,[CobranzaGestionID] " +
                                               "    ,[CodConfirmacion] " +
                                               "    ,[FechaConfirmacion] " +
                                               "    ,[eg_ccCobranzaGestion] ) " +
                                               "  VALUES " +
                                               "    (@NumeroDoc " +
                                               "    ,@Fecha " +
                                               "    ,@CobranzaGestionID " +
                                               "    ,@CodConfirmacion " +
                                               "    ,@FechaConfirmacion " +
                                               "    ,@eg_ccCobranzaGestion) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@CobranzaGestionID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodConfirmacion", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@FechaConfirmacion", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@eg_ccCobranzaGestion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = historia.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = historia.Fecha.Value;
                mySqlCommandSel.Parameters["@CobranzaGestionID"].Value = historia.CobranzaGestionID.Value;
                mySqlCommandSel.Parameters["@CodConfirmacion"].Value = historia.CodConfirmacion.Value;
                mySqlCommandSel.Parameters["@FechaConfirmacion"].Value = historia.FechaConfirmacion.Value;
                mySqlCommandSel.Parameters["@eg_ccCobranzaGestion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl);
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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccHistoricoGestionCobranza_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccHistoricoGestionCobranza_Update(List<DTO_ccHistoricoGestionCobranza> data)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@CobranzaGestionID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodConfirmacion", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@FechaConfirmacion", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaControl", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Dato1", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@eg_ccCobranzaGestion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                foreach (var g in data)
                {
                    #region Asigna los valores
                    mySqlCommandSel.Parameters["@Consecutivo"].Value = g.Consecutivo.Value;
                    mySqlCommandSel.Parameters["@NumeroDoc"].Value = g.NumeroDoc.Value;
                    mySqlCommandSel.Parameters["@Fecha"].Value = g.Fecha.Value;
                    mySqlCommandSel.Parameters["@CobranzaGestionID"].Value = g.CobranzaGestionID.Value;
                    mySqlCommandSel.Parameters["@CodConfirmacion"].Value = g.CodConfirmacion.Value;
                    mySqlCommandSel.Parameters["@FechaConfirmacion"].Value = g.FechaConfirmacion.Value;
                    mySqlCommandSel.Parameters["@FechaControl"].Value = g.FechaControl.Value;
                    mySqlCommandSel.Parameters["@VlrCuota"].Value = g.VlrCuota.Value;
                    mySqlCommandSel.Parameters["@Dato1"].Value = g.Dato1.Value;
                    mySqlCommandSel.Parameters["@eg_ccCobranzaGestion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCobranzaGestion, this.Empresa, egCtrl);
                    #endregion
                    #region Query Cooperativa
                    mySqlCommandSel.CommandText =
                        " UPDATE ccHistoricoGestionCobranza SET " +
                        "  CobranzaGestionID = @CobranzaGestionID " +
                        "  ,FechaConfirmacion = @FechaConfirmacion " +
                        "  ,CodConfirmacion = @CodConfirmacion " +
                        "  ,FechaControl = @FechaControl " +
                        "  ,VlrCuota = @VlrCuota " +
                        "  ,Dato1 = @Dato1 " +
                        " WHERE  Consecutivo = @Consecutivo ";
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
              
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccHistoricoGestionCobranza_Update");
                throw exception;
            }
        }

        #endregion

        /// <summary>
        /// Obtiene una lista con la info de las cobranzas
        /// </summary>
        /// <param name="fechaCorte">FEcha o dia de consulta</param>
        /// <param name="gestionCobrID">ID de la cobranza</param>
        /// <param name="estado">Estado a consultar</param>
        /// <param name="tipoGestion">Tipo de gestion</param>
        /// <returns>Datatable</returns>
        public DataTable DAL_ccHistoricoGestionCobranza_GetExcel(DateTime fechaCorte, string gestionCobrID,  byte? estado, byte? tipoGestion, string centroCosto)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                string where = string.Empty;
                string filter = string.Empty;
                string fecha = " @fecha ";

                mySqlCommandSel.Parameters.Add("@fecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters["@fecha"].Value = fechaCorte.Date;

                if (!string.IsNullOrEmpty(gestionCobrID))
                {
                    where += " where det.CobranzaGestionID = @CobranzaGestionID ";
                    mySqlCommandSel.Parameters.Add("@CobranzaGestionID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters["@CobranzaGestionID"].Value = gestionCobrID;
                }
                if (!string.IsNullOrEmpty(centroCosto))
                {
                    filter = " and ctrl.CentroCostoID = @CentroCostoID ";
                    mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommandSel.Parameters["@CentroCostoID"].Value = centroCosto;
                }
               
                if (estado == 1)
                    where += (string.IsNullOrEmpty(where)? " where ": " and ") + " det.CodConfirmacion is null ";
                else if (estado == 2)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.CodConfirmacion is not null ";
                if (tipoGestion == 1)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.Correo = 'X' ";
                else if (tipoGestion == 2)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.Carta = 'X' ";
                else if (tipoGestion == 3)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.MsgVoz = 'X' ";
                else if (tipoGestion == 4)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.MsgTexto = 'X' ";
                else if (tipoGestion == 5)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.Llamada = 'X' ";
                else if (tipoGestion == 6)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.Reporte = 'X' ";              
                #region CommanText

                mySqlCommandSel.CommandText =
                        " SELECT * FROM  " +
                        " ( " +
                        "     ( " +
                        "    select his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza,'Deudor' as Destino, " +
		                "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        cli.Descriptivo as Nombre, crd.ClienteID as Documento, cli.ResidenciaDir, cli.Telefono,cli.Celular1,cli.Correo as Mail,  " +
		                "        (case when ges.CartaInd  	= 1  then 'X' else '' end) as Carta,    " +
                        "        (case when ges.CorreoInd 	= 1 and (ges.CartaInd IS NULL or ges.CartaInd = 0) then 'X' else '' end) as Correo,   " +
		                "        (case when ges.MensajeTextoInd 	= 1 then 'X' else '' end) as MsgTexto,   " +
		                "        (case when ges.MensajeVozInd 	= 1 then 'X' else '' end) as MsgVoz,   " +
		                "        (case when ges.ReporteInd  	= 1 then 'X' else '' end) as Reporte,   " +
		                "        (case when ges.LlamadaInd  	= 1 then 'X' else '' end) as Llamada, his.CodConfirmacion,his.FechaConfirmacion  " +
	                    "    from ccHistoricoGestionCobranza his   " +
                        "        left join ccCreditoDocu crd on his.NumeroDoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
		                "        left join ccCliente cli on crd.ClienteID = cli.ClienteID and crd.eg_ccCliente = cli.EmpresaGrupoID " +
		                "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID  " +
		                "        left join glLugarGeografico ciu on cli.ResidenciaCiudad = ciu.LugarGeograficoID and cli.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
		                "        left join glLugarGeografico dep on left(cli.ResidenciaCiudad,2) = dep.LugarGeograficoID and cli.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where CAST(his.fecha as DATE) = " + fecha  + filter +
	                    "    )   " +
	                    "    UNION   " +
	                    "    (   " +
                        "    select his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza,'Conyugue' as Destino, " +
		                "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        cli.NomEsposa as Nombre, cli.cedEsposa as Documento, cli.ResidenciaDir, cli.Telefono,cli.Celular1,cli.Correo as Mail,  " +
                        "        (case when ges.CartaConyugueInd = 1 then 'X' else '' end) as Carta,		    " +
		                "        (case when ges.CorreoInd  	= 1 then 'X' else '' end) as Correo,   " +
		                "        (case when ges.MensajeTextoInd 	= 1 then 'X' else '' end) as MsgTexto,   " +
		                "        (case when ges.MensajeVozInd 	= 1 then 'X' else '' end) as MsgVoz,   " +
		                "        '' as Reporte,   " +
                        "        ''as Llamada, his.CodConfirmacion,his.FechaConfirmacion    " +
	                    "    from ccHistoricoGestionCobranza his   " +
		                "        left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
		                "        left join ccCliente cli on crd.ClienteID = cli.ClienteID   " +
		                "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID   " +
		                "        left join glLugarGeografico ciu on cli.ResidenciaCiudad = ciu.LugarGeograficoID and cli.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
		                "        left join glLugarGeografico dep on left(cli.ResidenciaCiudad,2) = dep.LugarGeograficoID and cli.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where CAST(his.fecha as DATE) = " + fecha + " and ges.ConyugueInd = 1 and cli.CedEsposa is not null and " +
                        "        (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1) " + filter +
	                    "    )   " +
	                    "    UNION   " +
	                    "    (   " +
                        "    select his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza,'Codeudor' as Destino, " +
		                "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        ter.Descriptivo as Nombre, crd.Codeudor1 as Documento, ter.Direccion, ter.Tel1,ter.Celular1,ter.CECorporativo as Mail, " +
                        "        (case when ges.CartaCodeudorInd = 1 then 'X' else '' end) as Carta,	 " +
		                "        (case when ges.CorreoInd  	= 1 then 'X' else '' end) as Correo,   " +
		                "        (case when ges.MensajeTextoInd 	= 1 then 'X' else '' end) as MsgTexto,   " +
		                "        (case when ges.MensajeVozInd 	= 1 then 'X' else '' end) as MsgVoz,   " +
		                "        '' as Reporte,   " +
		                "        ''as Llamada, his.CodConfirmacion,his.FechaConfirmacion    " +
	                    "    from ccHistoricoGestionCobranza his   " +
		                "        left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
		                "        left join ccCliente cli on crd.ClienteID = cli.ClienteID   " +
		                "        left join coTercero ter on crd.Codeudor1 = ter.TerceroID   " +
		                "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID   " +
		                "        left join glLugarGeografico ciu on ter.LugarGeograficoID = ciu.LugarGeograficoID and ter.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
		                "        left join glLugarGeografico dep on left(ter.LugarGeograficoID,2) = dep.LugarGeograficoID and ter.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where CAST(his.fecha as DATE) =  " + fecha + "  and ges.CoDeudorInd = 1 and crd.Codeudor1 is not null and crd.Codeudor1 <> 0 and cli.CedEsposa <> crd.Codeudor1 and   " +
                        "        (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1)   " + filter +
	                    "    )   " +
	                    "    UNION   " +
	                    "    (   " +
                        "    select his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza ,'Codeudor' as Destino, " +
		                "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        ter.Descriptivo as Nombre, crd.Codeudor2 as Documento, ter.Direccion, ter.Tel1,ter.Celular1,ter.CECorporativo as Mail, " +
                        "        (case when ges.CartaCodeudorInd = 1 then 'X' else '' end) as Carta,    " +
		                "        (case when ges.CorreoInd  	= 1 then 'X' else '' end) as Correo,   " +
		                "        (case when ges.MensajeTextoInd 	= 1 then 'X' else '' end) as MsgTexto,   " +
		                "        (case when ges.MensajeVozInd 	= 1 then 'X' else '' end) as MsgVoz,   " +
		                "        '' as Reporte,   " +
		                "        ''as Llamada, his.CodConfirmacion,his.FechaConfirmacion    " +
	                    "    from ccHistoricoGestionCobranza his   " +
		                "        left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
		                "        left join ccCliente cli on crd.ClienteID = cli.ClienteID   " +
		                "        left join coTercero ter on crd.Codeudor2 = ter.TerceroID   " +
		                "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID   " +
		                "        left join glLugarGeografico ciu on ter.LugarGeograficoID = ciu.LugarGeograficoID and ter.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
		                "        left join glLugarGeografico dep on left(ter.LugarGeograficoID,2) = dep.LugarGeograficoID and ter.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where  CAST(his.fecha as DATE) =  " + fecha + "  and ges.CoDeudorInd = 1 and crd.Codeudor2 is not null and crd.Codeudor2 <> 0 and cli.CedEsposa <> crd.Codeudor2 and   " +
                        "        (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1)  " + filter +
	                    "    )   " +
	                    "    UNION   " +
	                    "    (   " +
                        "    select his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza,'Codeudor' as Destino, " +
		                "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        ter.Descriptivo as Nombre, crd.Codeudor3 as Documento, ter.Direccion, ter.Tel1,ter.Celular1,ter.CECorporativo as Mail, " +
                        "        (case when ges.CartaCodeudorInd = 1 then 'X' else '' end) as Carta,    " +
		                "        (case when ges.CorreoInd  = 1 then 'X' else '' end) as Correo,   " +
		                "        (case when ges.MensajeTextoInd = 1 then 'X' else '' end) as MsgTexto,   " +
		                "        (case when ges.MensajeVozInd = 1 then 'X' else '' end) as MsgVoz,   " +
		                "        '' as Reporte,   " +
		                "        ''as Llamada, his.CodConfirmacion,his.FechaConfirmacion    " +
	                    "    from ccHistoricoGestionCobranza his   " +
		                "        left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
		                "        left join ccCliente cli on crd.ClienteID = cli.ClienteID   " +
		                "        left join coTercero ter on crd.Codeudor3 = ter.TerceroID   " +
		                "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID   " +
		                "        left join glLugarGeografico ciu on ter.LugarGeograficoID = ciu.LugarGeograficoID and ter.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
		                "        left join glLugarGeografico dep on left(ter.LugarGeograficoID,2) = dep.LugarGeograficoID and ter.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where CAST(his.fecha as DATE) =  " + fecha + "  and ges.CoDeudorInd = 1 and crd.Codeudor3 is not null and crd.Codeudor3 <> 0 and cli.CedEsposa <> crd.Codeudor3 and  " +
                        "        (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1)   " + filter +
                        "     ) " +
                        " ) det " + where +
                        " ORDER BY CobranzaGestionID, Nombre, destino ";

                #endregion

                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable();

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                if (!string.IsNullOrEmpty(mySqlCommandSel.CommandText))
                    sda.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_CarteraToExcel");
                return null;

            }
        }


        /// <summary>
        /// Obtiene una lista con la info de las cobranzas sin filtrar fecha
        /// </summary>
        /// <param name="fechaCorte">FEcha o dia de consulta</param>
        /// <param name="gestionCobrID">ID de la cobranza</param>
        /// <param name="estado">Estado a consultar</param>
        /// <param name="tipoGestion">Tipo de gestion</param>
        /// <returns>Datatable</returns>
        public DataTable DAL_ccHistoricoGestionCobranza_GetExcelSinFecha(string gestionCobrID, string clienteID, string libranza, byte? estado, byte? tipoGestion, string centroCosto)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                string where = string.Empty;
                string filter = string.Empty;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(gestionCobrID))
                {
                    where += " where det.CobranzaGestionID = @CobranzaGestionID ";
                    mySqlCommandSel.Parameters.Add("@CobranzaGestionID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters["@CobranzaGestionID"].Value = gestionCobrID;
                }
                if (!string.IsNullOrEmpty(centroCosto))
                {
                    filter = " and ctrl.CentroCostoID = @CentroCostoID ";
                    mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommandSel.Parameters["@CentroCostoID"].Value = centroCosto;
                }
                if (!string.IsNullOrEmpty(clienteID))
                {
                    filter += " and crd.ClienteID = @ClienteID ";
                    mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommandSel.Parameters["@ClienteID"].Value = clienteID;
                }
                if (!string.IsNullOrEmpty(libranza))
                {
                    filter += " and crd.Libranza = @Libranza ";
                    mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                }
                if (estado == 1)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.CodConfirmacion is null ";
                else if (estado == 2)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.CodConfirmacion is not null ";
                if (tipoGestion == 1)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.Correo = 'X' ";
                else if (tipoGestion == 2)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.Carta = 'X' ";
                else if (tipoGestion == 3)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.MsgVoz = 'X' ";
                else if (tipoGestion == 4)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.MsgTexto = 'X' ";
                else if (tipoGestion == 5)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.Llamada = 'X' ";
                else if (tipoGestion == 6)
                    where += (string.IsNullOrEmpty(where) ? " where " : " and ") + " det.Reporte = 'X' ";
                #region CommanText

                mySqlCommandSel.CommandText =
                        " SELECT * FROM  " +
                        " ( " +
                        "     ( " +
                        "    select his.Fecha, his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza,'Deudor' as Destino, " +
                        "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        cli.Descriptivo as Nombre, crd.ClienteID as Documento, cli.ResidenciaDir, cli.Telefono,cli.Celular1,cli.Correo as Mail,  " +
                        "        (case when ges.CartaInd  	= 1  then 'X' else '' end) as Carta,    " +
                        "        (case when ges.CorreoInd 	= 1 and (ges.CartaInd IS NULL or ges.CartaInd = 0) then 'X' else '' end) as Correo,   " +
                        "        (case when ges.MensajeTextoInd 	= 1 then 'X' else '' end) as MsgTexto,   " +
                        "        (case when ges.MensajeVozInd 	= 1 then 'X' else '' end) as MsgVoz,   " +
                        "        (case when ges.ReporteInd  	= 1 then 'X' else '' end) as Reporte,   " +
                        "        (case when ges.LlamadaInd  	= 1 then 'X' else '' end) as Llamada, his.CodConfirmacion,his.FechaConfirmacion  " +
                        "    from ccHistoricoGestionCobranza his   " +
                        "        left join ccCreditoDocu crd on his.NumeroDoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "        left join ccCliente cli on crd.ClienteID = cli.ClienteID and crd.eg_ccCliente = cli.EmpresaGrupoID " +
                        "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID  " +
                        "        left join glLugarGeografico ciu on cli.ResidenciaCiudad = ciu.LugarGeograficoID and cli.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
                        "        left join glLugarGeografico dep on left(cli.ResidenciaCiudad,2) = dep.LugarGeograficoID and cli.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where ctrl.EmpresaID = @EmpresaID " + filter +
                        "    )   " +
                        "    UNION   " +
                        "    (   " +
                        "    select his.Fecha, his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza,'Conyugue' as Destino, " +
                        "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        cli.NomEsposa as Nombre, cli.cedEsposa as Documento, cli.ResidenciaDir, cli.Telefono,cli.Celular1,cli.Correo as Mail,  " +
                        "        (case when ges.CartaConyugueInd = 1 then 'X' else '' end) as Carta,		    " +
                        "        (case when ges.CorreoInd  	= 1 then 'X' else '' end) as Correo,   " +
                        "        (case when ges.MensajeTextoInd 	= 1 then 'X' else '' end) as MsgTexto,   " +
                        "        (case when ges.MensajeVozInd 	= 1 then 'X' else '' end) as MsgVoz,   " +
                        "        '' as Reporte,   " +
                        "        ''as Llamada, his.CodConfirmacion,his.FechaConfirmacion    " +
                        "    from ccHistoricoGestionCobranza his   " +
                        "        left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "        left join ccCliente cli on crd.ClienteID = cli.ClienteID   " +
                        "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID   " +
                        "        left join glLugarGeografico ciu on cli.ResidenciaCiudad = ciu.LugarGeograficoID and cli.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
                        "        left join glLugarGeografico dep on left(cli.ResidenciaCiudad,2) = dep.LugarGeograficoID and cli.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where ges.ConyugueInd = 1 and cli.CedEsposa is not null and " +
                        "        (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1) " + filter +
                        "    )   " +
                        "    UNION   " +
                        "    (   " +
                        "    select his.Fecha, his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza,'Codeudor' as Destino, " +
                        "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        ter.Descriptivo as Nombre, crd.Codeudor1 as Documento, ter.Direccion, ter.Tel1,ter.Celular1,ter.CECorporativo as Mail, " +
                        "        (case when ges.CartaCodeudorInd = 1 then 'X' else '' end) as Carta,	 " +
                        "        (case when ges.CorreoInd  	= 1 then 'X' else '' end) as Correo,   " +
                        "        (case when ges.MensajeTextoInd 	= 1 then 'X' else '' end) as MsgTexto,   " +
                        "        (case when ges.MensajeVozInd 	= 1 then 'X' else '' end) as MsgVoz,   " +
                        "        '' as Reporte,   " +
                        "        ''as Llamada, his.CodConfirmacion,his.FechaConfirmacion    " +
                        "    from ccHistoricoGestionCobranza his   " +
                        "        left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "        left join ccCliente cli on crd.ClienteID = cli.ClienteID   " +
                        "        left join coTercero ter on crd.Codeudor1 = ter.TerceroID   " +
                        "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID   " +
                        "        left join glLugarGeografico ciu on ter.LugarGeograficoID = ciu.LugarGeograficoID and ter.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
                        "        left join glLugarGeografico dep on left(ter.LugarGeograficoID,2) = dep.LugarGeograficoID and ter.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where ges.CoDeudorInd = 1 and crd.Codeudor1 is not null and crd.Codeudor1 <> 0 and cli.CedEsposa <> crd.Codeudor1 and   " +
                        "        (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1)   " + filter +
                        "    )   " +
                        "    UNION   " +
                        "    (   " +
                        "    select his.Fecha, his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza ,'Codeudor' as Destino, " +
                        "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        ter.Descriptivo as Nombre, crd.Codeudor2 as Documento, ter.Direccion, ter.Tel1,ter.Celular1,ter.CECorporativo as Mail, " +
                        "        (case when ges.CartaCodeudorInd = 1 then 'X' else '' end) as Carta,    " +
                        "        (case when ges.CorreoInd  	= 1 then 'X' else '' end) as Correo,   " +
                        "        (case when ges.MensajeTextoInd 	= 1 then 'X' else '' end) as MsgTexto,   " +
                        "        (case when ges.MensajeVozInd 	= 1 then 'X' else '' end) as MsgVoz,   " +
                        "        '' as Reporte,   " +
                        "        ''as Llamada, his.CodConfirmacion,his.FechaConfirmacion    " +
                        "    from ccHistoricoGestionCobranza his   " +
                        "        left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "        left join ccCliente cli on crd.ClienteID = cli.ClienteID   " +
                        "        left join coTercero ter on crd.Codeudor2 = ter.TerceroID   " +
                        "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID   " +
                        "        left join glLugarGeografico ciu on ter.LugarGeograficoID = ciu.LugarGeograficoID and ter.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
                        "        left join glLugarGeografico dep on left(ter.LugarGeograficoID,2) = dep.LugarGeograficoID and ter.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where ges.CoDeudorInd = 1 and crd.Codeudor2 is not null and crd.Codeudor2 <> 0 and cli.CedEsposa <> crd.Codeudor2 and   " +
                        "        (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1)  " + filter +
                        "    )   " +
                        "    UNION   " +
                        "    (   " +
                        "    select his.Fecha, his.Consecutivo, his.CobranzaGestionID,ges.Descriptivo as GestionDesc,  crd.Libranza,'Codeudor' as Destino, " +
                        "        ciu.Descriptivo as Ciudad, dep.Descriptivo as Depto,  " +
                        "        ter.Descriptivo as Nombre, crd.Codeudor3 as Documento, ter.Direccion, ter.Tel1,ter.Celular1,ter.CECorporativo as Mail, " +
                        "        (case when ges.CartaCodeudorInd = 1 then 'X' else '' end) as Carta,    " +
                        "        (case when ges.CorreoInd  = 1 then 'X' else '' end) as Correo,   " +
                        "        (case when ges.MensajeTextoInd = 1 then 'X' else '' end) as MsgTexto,   " +
                        "        (case when ges.MensajeVozInd = 1 then 'X' else '' end) as MsgVoz,   " +
                        "        '' as Reporte,   " +
                        "        ''as Llamada, his.CodConfirmacion,his.FechaConfirmacion    " +
                        "    from ccHistoricoGestionCobranza his   " +
                        "        left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc   " +
                        "        left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "        left join ccCliente cli on crd.ClienteID = cli.ClienteID   " +
                        "        left join coTercero ter on crd.Codeudor3 = ter.TerceroID   " +
                        "        left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID   " +
                        "        left join glLugarGeografico ciu on ter.LugarGeograficoID = ciu.LugarGeograficoID and ter.eg_glLugarGeografico = ciu.EmpresaGrupoID " +
                        "        left join glLugarGeografico dep on left(ter.LugarGeograficoID,2) = dep.LugarGeograficoID and ter.eg_glLugarGeografico = dep.EmpresaGrupoID " +
                        "    where  ges.CoDeudorInd = 1 and crd.Codeudor3 is not null and crd.Codeudor3 <> 0 and cli.CedEsposa <> crd.Codeudor3 and  " +
                        "        (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1)   " + filter +
                        "     ) " +
                        " ) det " + where +
                        " ORDER BY Fecha desc, CobranzaGestionID, Nombre, destino ";

                #endregion

                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable();

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                if (!string.IsNullOrEmpty(mySqlCommandSel.CommandText))
                    sda.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_CarteraToExcel");
                return null;

            }
        }


        /// <summary>
        /// Obtiene una lista con la info de las cobranzas
        /// </summary>
        /// <param name="fechaCorte">FEcha o dia de consulta</param>
        /// <param name="gestionCobrID">ID de la cobranza</param>
        /// <param name="estado">Estado a consultar</param>
        /// <param name="tipoGestion">Tipo de gestion</param>
        /// <returns>Lista</returns>
        public List<DTO_ccHistoricoGestionCobranza> DAL_ccHistoricoGestionCobranza_GetGestion(DateTime fechaCorte, string gestionCobrID, byte? estado, byte? tipoGestion,string centroCosto)
        {
            try
            {
                List<DTO_ccHistoricoGestionCobranza> results = new List<DTO_ccHistoricoGestionCobranza>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@fecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters["@fecha"].Value = fechaCorte.Date;
                string where = string.Empty;
                string centroCtofilter = string.Empty;

                if (!string.IsNullOrEmpty(gestionCobrID))
                {
                    where += " and his.CobranzaGestionID = @CobranzaGestionID ";
                    mySqlCommandSel.Parameters.Add("@CobranzaGestionID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters["@CobranzaGestionID"].Value = gestionCobrID;
                }
                if (!string.IsNullOrEmpty(centroCosto))
                {
                    centroCtofilter = " and ctrl.CentroCostoID = @CentroCostoID ";
                    mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommandSel.Parameters["@CentroCostoID"].Value = centroCosto;
                }
                if (estado == 1)
                    where += " and his.CodConfirmacion is null ";
                else if (estado == 2)
                    where += " and his.CodConfirmacion is not null ";

                if (tipoGestion == 1)
                    where += " and ges.CorreoInd = 1 ";
                else if (tipoGestion == 2)
                    where += " and ges.CartaInd = 1 ";
                else if (tipoGestion == 3)
                    where += " and ges.MensajeVozInd = 1 ";
                else if (tipoGestion == 4)
                    where += " and ges.MensajeTextoInd = 1 ";
                else if (tipoGestion == 5)
                    where += " and ges.LlamadaInd = 1 ";
                else if (tipoGestion == 6)
                    where += " and ges.ReporteInd = 1 ";

                #region CommanText

                mySqlCommandSel.CommandText =
                        " SELECT * FROM  " +
                        " ( " +
                        "     ( " +
                        "     select his.CobranzaGestionID,ges.Descriptivo as GestionDesc, crd.ClienteID, 'Deudor' as Destino, cli.Descriptivo as Nombre, crd.Libranza, " +
                        "       isNull(ges.CartaInd,0) as Carta,   isNull(ges.CorreoInd,0) as Correo, " +
                        "       isNull(ges.MensajeTextoInd,0) as MsgTexto, isNull(ges.MensajeVozInd,0) as MsgVoz, " +
                        "       isNull(ges.ReporteInd,0) as Reporte, isNull(ges.LlamadaInd,0) as Llamada, " +
                        "       his.NumeroDoc,his.Consecutivo,his.CodConfirmacion,his.FechaConfirmacion,his.FechaControl,his.VlrCuota,his.Dato1, " +
                        "       ges.PlantillaCarta,ges.PlantillaEMail,ges.Referencia,cli.ResidenciaDir as Direccion,lg.Descriptivo as Ciudad,ges.Mensaje,cli.Celular1 as Telefono  " +
                        "     from ccHistoricoGestionCobranza his " +
                        "      left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc " +
                        "      left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "      left join ccCliente cli on crd.ClienteID = cli.ClienteID and cli.EmpresaGrupoID = crd.eg_ccCliente" +
                        "      left join glLugarGeografico lg on lg.LugarGeograficoID = cli.ResidenciaCiudad and lg.EmpresaGrupoID = cli.eg_glLugarGeografico " +
                        "      left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID " +
                        "     where CAST(his.fecha as DATE) = @fecha " + where + centroCtofilter +
                        "     ) " +
                        "     UNION " +
                        "     ( " +
                        "     select his.CobranzaGestionID,ges.Descriptivo as GestionDesc, cli.cedEsposa as ClienteID, 'Conyugue' as Destino, cli.NomEsposa as Nombre, crd.Libranza, " +
                        "       0 as Carta,   isNull(ges.CorreoInd,0) as Correo, " +
                         "      isNull(ges.MensajeTextoInd,0) as MsgTexto, isNull(ges.MensajeVozInd,0) as MsgVoz, " +
                        "       0 as Reporte, 0 as Llamada, " +
                        "       his.NumeroDoc,his.Consecutivo,his.CodConfirmacion,his.FechaConfirmacion,his.FechaControl,his.VlrCuota,his.Dato1, " +
                        "       ges.PlantillaCarta,ges.PlantillaEMail,ges.Referencia,cli.ResidenciaDir as Direccion,lg.Descriptivo as Ciudad,ges.Mensaje,cli.Celular1 as Telefono  " + 
                        "     from ccHistoricoGestionCobranza his " +
                        "      left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc " +
                        "      left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "      left join ccCliente cli on crd.ClienteID = cli.ClienteID and cli.EmpresaGrupoID = crd.eg_ccCliente " +
                        "      left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID " +
                        "      left join glLugarGeografico lg on lg.LugarGeograficoID = cli.ResidenciaCiudad and lg.EmpresaGrupoID = cli.eg_glLugarGeografico " +
                        "     where CAST(his.fecha as DATE) = @fecha and ges.ConyugueInd = 1 and cli.CedEsposa is not null and (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1) " + where + centroCtofilter +
                        "     ) " +
                        "     UNION " +
                        "     ( " +
                        "     select his.CobranzaGestionID,ges.Descriptivo as GestionDesc, crd.Codeudor1 as ClienteID, 'Codeudor' as Destino, ter.Descriptivo as Nombre, crd.Libranza, " +
                        "       0 as Carta,   isNull(ges.CorreoInd,0) as Correo, " +
                        "       isNull(ges.MensajeTextoInd,0) as MsgTexto, isNull(ges.MensajeVozInd,0) as MsgVoz, " +
                        "       0 as Reporte,  0 as Llamada, " +
                        "       his.NumeroDoc,his.Consecutivo,his.CodConfirmacion,his.FechaConfirmacion,his.FechaControl,his.VlrCuota,his.Dato1, " +
                        "       ges.PlantillaCarta,ges.PlantillaEMail,ges.Referencia,ter.Direccion,lg.Descriptivo as Ciudad,ges.Mensaje,ter.Celular1 as Telefono  " +
                        "     from ccHistoricoGestionCobranza his " +
                        "      left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc " +
                        "      left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "      left join coTercero ter on crd.Codeudor1 = ter.TerceroID  " +
                        "      left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID " +
                        "      left join glLugarGeografico lg on lg.LugarGeograficoID = ter.LugarGeograficoID and lg.EmpresaGrupoID = ter.eg_glLugarGeografico " +
                        "     where CAST(his.fecha as DATE) = @fecha and ges.CoDeudorInd = 1 and crd.Codeudor1 is not null and crd.Codeudor1 <> 0 and  " +
                        "       (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1) " + where + centroCtofilter +
                        "     ) " +
                        "     UNION " +
                        "     ( " +
                        "     select his.CobranzaGestionID,ges.Descriptivo as GestionDesc, crd.Codeudor2 as ClienteID, 'Codeudor' as Destino, ter.Descriptivo as Nombre, crd.Libranza, " +
                        "       0 as Carta,  isNull(ges.CorreoInd,0) as Correo,  isNull(ges.MensajeTextoInd,0) as MsgTexto, " +
                        "       isNull(ges.MensajeVozInd,0) as MsgVoz, 0 as Reporte, 0 as Llamada, " +
                        "       his.NumeroDoc,his.Consecutivo,his.CodConfirmacion,his.FechaConfirmacion,his.FechaControl,his.VlrCuota,his.Dato1, " +
                        "       ges.PlantillaCarta,ges.PlantillaEMail,ges.Referencia,ter.Direccion,lg.Descriptivo as Ciudad,ges.Mensaje,ter.Celular1 as Telefono   " +
                        "     from ccHistoricoGestionCobranza his " +
                        "      left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc " +
                        "      left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "      left join coTercero ter on crd.Codeudor2 = ter.TerceroID " +
                        "      left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID " +
                        "      left join glLugarGeografico lg on lg.LugarGeograficoID = ter.LugarGeograficoID and lg.EmpresaGrupoID = ter.eg_glLugarGeografico " +
                        "     where  CAST(his.fecha as DATE) = @fecha and ges.CoDeudorInd = 1 and crd.Codeudor2 is not null and crd.Codeudor2 <> 0 and  " +
                        "       (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1)  " + where + centroCtofilter +
                        "     ) " +
                        "     UNION " +
                        "     ( " +
                        "    Select his.CobranzaGestionID,ges.Descriptivo as GestionDesc, crd.Codeudor3 as ClienteID, 'Codeudor' as Destino, ter.Descriptivo as Nombre, crd.Libranza, " +
                        "       0 as Carta,  isNull(ges.CorreoInd,0) as Correo, " +
                         "      isNull(ges.MensajeTextoInd,0) as MsgTexto, isNull(ges.MensajeVozInd,0) as MsgVoz, " +
                        "       0 as Reporte, 0 as Llamada, " +
                        "       his.NumeroDoc,his.Consecutivo,his.CodConfirmacion,his.FechaConfirmacion,his.FechaControl,his.VlrCuota,his.Dato1, " +
                        "       ges.PlantillaCarta,ges.PlantillaEMail,ges.Referencia,ter.Direccion,lg.Descriptivo as Ciudad,ges.Mensaje,ter.Celular1 as Telefono   " +
                        "     from ccHistoricoGestionCobranza his " +
                        "      left join ccCreditoDocu crd on his.numerodoc = crd.NumeroDoc " +
                        "      left join glDocumentoControl ctrl on ctrl.NumeroDoc = crd.NumeroDoc   " +
                        "      left join coTercero ter on crd.Codeudor3 = ter.TerceroID " +
                        "      left join ccCobranzaGestion ges on his.CobranzaGestionID = ges.CobranzaGestionID " +
                        "      left join glLugarGeografico lg on lg.LugarGeograficoID = ter.LugarGeograficoID and lg.EmpresaGrupoID = ter.eg_glLugarGeografico " +
                        "     where CAST(his.fecha as DATE) = @fecha and ges.CoDeudorInd = 1 and crd.Codeudor3 is not null and crd.Codeudor2 <> 0 and  " +
                        "       (ges.MensajeTextoInd=1 or ges.MensajeVozInd=1 or ges.CorreoInd=1)  " + where + centroCtofilter +
                        "     ) " +
                        " ) det " +
                        " ORDER BY CobranzaGestionID, Libranza, destino ";

                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccHistoricoGestionCobranza r = new DTO_ccHistoricoGestionCobranza();
                    r.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                    r.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);                   
                    r.CobranzaGestionID.Value = Convert.ToString(dr["CobranzaGestionID"]);
                    r.GestionDesc.Value = Convert.ToString(dr["GestionDesc"]);
                    r.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                    r.Nombre.Value = Convert.ToString(dr["Nombre"]);
                    r.Destino.Value = Convert.ToString(dr["Destino"]);
                    r.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    r.CartaInd.Value = Convert.ToBoolean(dr["Carta"]);
                    r.CorreoInd.Value = Convert.ToBoolean(dr["Correo"]);
                    r.MensajeTextoInd.Value = Convert.ToBoolean(dr["MsgTexto"]);
                    r.MensajeVozInd.Value = Convert.ToBoolean(dr["MsgVoz"]);
                    r.ReporteInd.Value = Convert.ToBoolean(dr["Reporte"]);
                    r.LlamadaInd.Value = Convert.ToBoolean(dr["Llamada"]);
                    r.CodConfirmacion.Value = Convert.ToString(dr["CodConfirmacion"]);
                    if (!string.IsNullOrWhiteSpace(dr["FechaConfirmacion"].ToString()))
                        r.FechaConfirmacion.Value = Convert.ToDateTime(dr["FechaConfirmacion"]);
                    if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                        r.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                    else r.VlrCuota.Value = 0;
                    if (!string.IsNullOrWhiteSpace(dr["FechaControl"].ToString()))
                        r.FechaControl.Value = Convert.ToDateTime(dr["FechaControl"]);
                    r.Dato1.Value = Convert.ToString(dr["Dato1"]);
                    r.PlantillaCarta.Value = Convert.ToString(dr["PlantillaCarta"]);
                    r.PlantillaEMail.Value = Convert.ToString(dr["PlantillaEMail"]);
                    r.Referencia.Value = Convert.ToString(dr["Referencia"]);
                    r.Direccion.Value = Convert.ToString(dr["Direccion"]);
                    r.Ciudad.Value = Convert.ToString(dr["Ciudad"]);
                    r.Telefono.Value = Convert.ToString(dr["Telefono"]);
                    r.Mensaje.Value = Convert.ToString(dr["Mensaje"]);
                    results.Add(r);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_CarteraToExcel");
                return null;

            }
        }

    }

}
