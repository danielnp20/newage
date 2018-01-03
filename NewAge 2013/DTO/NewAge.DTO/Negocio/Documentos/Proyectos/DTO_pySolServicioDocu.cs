﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_pySolServicioDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pySolServicioDocu
    {
        #region pySolServicioDocu

        public DTO_pySolServicioDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                this.EmpresaNombre.Value = Convert.ToString(dr["EmpresaNombre"]);
                this.SolicitanteEMP.Value = Convert.ToString(dr["SolicitanteEMP"]);
                this.ResponsableEMP.Value = Convert.ToString(dr["ResponsableEMP"]);
                this.ResponsableCLI.Value = Convert.ToString(dr["ResponsableCLI"]);
                this.ResponsableCorreo.Value = Convert.ToString(dr["ResponsableCorreo"]);
                this.ResponsableTelefono.Value = Convert.ToString(dr["ResponsableTelefono"]);
                this.ContratoID.Value = Convert.ToString(dr["ContratoID"]);
               this.ActividadID.Value = Convert.ToString(dr["ActividadID"]);
               this.ClaseServicioID.Value = Convert.ToString(dr["ClaseServicioID"]);
               if (!string.IsNullOrEmpty(dr["TipoSolicitud"].ToString()))
                    this.TipoSolicitud.Value = Convert.ToByte(dr["TipoSolicitud"]);              
               if (!string.IsNullOrEmpty(dr["RecursosXTrabajoInd"].ToString()))
                    this.RecursosXTrabajoInd.Value = Convert.ToBoolean(dr["RecursosXTrabajoInd"]);
               this.DescripcionSOL.Value = Convert.ToString(dr["DescripcionSOL"]);
               this.Licitacion.Value = Convert.ToString(dr["Licitacion"]);
               if (!string.IsNullOrEmpty(dr["APUIncluyeAIUInd"].ToString()))
                   this.APUIncluyeAIUInd.Value = Convert.ToBoolean(dr["APUIncluyeAIUInd"]);
               if (!string.IsNullOrEmpty(dr["Jerarquia"].ToString()))
                    this.Jerarquia.Value = Convert.ToByte(dr["Jerarquia"]);
               if (!string.IsNullOrEmpty(dr["PorClienteADM"].ToString()))
                   this.PorClienteADM.Value = Convert.ToDecimal(dr["PorClienteADM"]);
               if (!string.IsNullOrEmpty(dr["PorClienteIMP"].ToString()))
                   this.PorClienteIMP.Value = Convert.ToDecimal(dr["PorClienteIMP"]);
               if (!string.IsNullOrEmpty(dr["PorClienteUTI"].ToString()))
                   this.PorClienteUTI.Value = Convert.ToDecimal(dr["PorClienteUTI"]);
               if (!string.IsNullOrEmpty(dr["PorEmpresaADM"].ToString()))
                   this.PorEmpresaADM.Value = Convert.ToDecimal(dr["PorEmpresaADM"]);
               if (!string.IsNullOrEmpty(dr["PorEmpresaIMP"].ToString()))
                   this.PorEmpresaIMP.Value = Convert.ToDecimal(dr["PorEmpresaIMP"]);
               if (!string.IsNullOrEmpty(dr["PorEmpresaUTI"].ToString()))
                   this.PorEmpresaUTI.Value = Convert.ToDecimal(dr["PorEmpresaUTI"]);
               if (!string.IsNullOrEmpty(dr["PorMultiplicadorPresup"].ToString()))
                   this.PorMultiplicadorPresup.Value = Convert.ToDecimal(dr["PorMultiplicadorPresup"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pySolServicioDocu()
        {
            InitCols();
            this.PorEmpresaADM.Value = 0;
            this.PorEmpresaIMP.Value = 0;
            this.PorEmpresaUTI.Value = 0;
            this.PorClienteADM.Value = 0;
            this.PorClienteIMP.Value = 0;
            this.PorClienteUTI.Value = 0;
            this.Valor.Value = 0;
            this.ValorCliente.Value = 0;
            this.ValorIVA.Value = 0;
            this.ValorOtros.Value = 0;
        }

        public void InitCols() {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.ClienteID = new UDT_ClienteID();
            this.EmpresaNombre = new UDT_DescripTExt();
            this.SolicitanteEMP = new UDT_UsuarioID();
            this.ResponsableEMP = new UDT_UsuarioID();
            this.ResponsableCLI = new UDT_DescripTBase();
            this.ResponsableCorreo = new UDT_DescripTBase();
            this.ResponsableTelefono = new UDT_DescripTBase();
            this.ContratoID = new UDT_ContratoID();
            this.ActividadID = new UDT_ActividadID();
            this.ClaseServicioID = new UDT_CodigoGrl();
            this.TipoSolicitud = new UDTSQL_tinyint();
            this.RecursosXTrabajoInd = new UDT_SiNo();
            this.DescripcionSOL = new UDT_DescripTExt();
            this.Licitacion = new UDT_DescripTBase();
            this.APUIncluyeAIUInd = new UDT_SiNo();
            this.Jerarquia = new UDTSQL_tinyint();
            this.PorClienteADM = new UDT_PorcentajeID();
            this.PorClienteIMP = new UDT_PorcentajeID();
            this.PorClienteUTI = new UDT_PorcentajeID();
            this.PorEmpresaADM = new UDT_PorcentajeID();
            this.PorEmpresaIMP = new UDT_PorcentajeID();
            this.PorEmpresaUTI = new UDT_PorcentajeID();
            this.PorMultiplicadorPresup = new UDT_PorcentajeID();
            //Adicionales
            this.Valor = new UDT_Valor();
            this.ValorCliente = new UDT_Valor();
            this.ValorIVA = new UDT_Valor();
            this.ValorOtros = new UDT_Valor();
            this.ClienteDesc = new UDT_Descriptivo();
            this.PrefDoc = new UDT_DescripTBase();
            this.DetalleTareas = new List<DTO_pySolServicioTarea>();
        }
           #endregion


        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_DescripTExt EmpresaNombre { get; set; }

        [DataMember]
        public UDT_UsuarioID SolicitanteEMP { get; set; }

        [DataMember]
        public UDT_UsuarioID ResponsableEMP { get; set; }

        [DataMember]
        public UDT_DescripTBase ResponsableCLI { get; set; }

        [DataMember]
        public UDT_DescripTBase ResponsableCorreo { get; set; }

        [DataMember]
        public UDT_DescripTBase ResponsableTelefono { get; set; }

        [DataMember]
        public UDT_ContratoID ContratoID { get; set; }

        [DataMember]
        public UDT_ActividadID ActividadID { get; set; }

        [DataMember]
        public UDT_CodigoGrl ClaseServicioID { get; set; }        

        [DataMember]
        public UDTSQL_tinyint TipoSolicitud { get; set; }

        [DataMember]
        public UDT_SiNo RecursosXTrabajoInd { get; set; }

        [DataMember]
        public UDT_DescripTExt DescripcionSOL { get; set; }

        [DataMember]
        public UDT_DescripTBase Licitacion { get; set; }
        	
        [DataMember]
        public UDT_SiNo APUIncluyeAIUInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint Jerarquia { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorClienteADM { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorClienteIMP { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorClienteUTI { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorEmpresaADM { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorEmpresaIMP { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorEmpresaUTI { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorMultiplicadorPresup { get; set; }

        //Adicionales
        [DataMember]
        public UDT_Valor Valor { get; set; } //Total Presupuesto

        [DataMember]
        public UDT_Valor ValorCliente { get; set; } //Total Presupuesto Cliente

        [DataMember]
        public UDT_Valor ValorIVA { get; set; } //Total IVA

        [DataMember]
        public UDT_Valor ValorOtros { get; set; } //Total Otros

        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public List<DTO_pySolServicioTarea> DetalleTareas { get; set; }
    }
}
