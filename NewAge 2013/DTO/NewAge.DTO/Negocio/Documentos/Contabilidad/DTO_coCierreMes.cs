using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_coCierreMes
    {
        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coCierreMes(IDataReader dr, bool isReport)
        {
            this.InitCols();

            try
            {

                this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();

                if (!isReport)
                {
                    this.Ano.Value = Convert.ToInt16(dr["Año"]);
                    this.LocalDB01.Value = Convert.ToDecimal(dr["LocalDB"]);
                    this.LocalCR01.Value = Convert.ToDecimal(dr["LocalCR"]);
                    this.ExtraDB01.Value = Convert.ToDecimal(dr["ExtraDB"]);
                    this.ExtraCR01.Value = Convert.ToDecimal(dr["ExtraCR"]);
                }
                else
                {
                    this.Ano.Value = Convert.ToInt16(dr["Ano"]);
                    if (!string.IsNullOrEmpty(dr["LocalINI"].ToString()))
                        this.LocalINI.Value = Convert.ToDecimal(dr["LocalINI"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB01"].ToString()))
                        this.LocalDB01.Value = Convert.ToDecimal(dr["LocalDB01"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR01"].ToString()))
                        this.LocalCR01.Value = Convert.ToDecimal(dr["LocalCR01"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB01"].ToString()))
                        this.ExtraDB01.Value = Convert.ToDecimal(dr["ExtraDB01"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR01"].ToString()))
                        this.ExtraCR01.Value = Convert.ToDecimal(dr["ExtraCR01"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB02"].ToString()))
                        this.LocalDB02.Value = Convert.ToDecimal(dr["LocalDB02"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR02"].ToString()))
                        this.LocalCR02.Value = Convert.ToDecimal(dr["LocalCR02"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB02"].ToString()))
                        this.ExtraDB02.Value = Convert.ToDecimal(dr["ExtraDB02"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR02"].ToString()))
                        this.ExtraCR02.Value = Convert.ToDecimal(dr["ExtraCR02"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB03"].ToString()))
                        this.LocalDB03.Value = Convert.ToDecimal(dr["LocalDB03"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR03"].ToString()))
                        this.LocalCR03.Value = Convert.ToDecimal(dr["LocalCR03"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB03"].ToString()))
                        this.ExtraDB03.Value = Convert.ToDecimal(dr["ExtraDB03"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR03"].ToString()))
                        this.ExtraCR03.Value = Convert.ToDecimal(dr["ExtraCR03"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB04"].ToString()))
                        this.LocalDB04.Value = Convert.ToDecimal(dr["LocalDB04"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR04"].ToString()))
                        this.LocalCR04.Value = Convert.ToDecimal(dr["LocalCR04"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB04"].ToString()))
                        this.ExtraDB04.Value = Convert.ToDecimal(dr["ExtraDB04"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR04"].ToString()))
                        this.ExtraCR04.Value = Convert.ToDecimal(dr["ExtraCR04"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB05"].ToString()))
                        this.LocalDB05.Value = Convert.ToDecimal(dr["LocalDB05"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR05"].ToString()))
                        this.LocalCR05.Value = Convert.ToDecimal(dr["LocalCR05"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB05"].ToString()))
                        this.ExtraDB05.Value = Convert.ToDecimal(dr["ExtraDB05"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR05"].ToString()))
                        this.ExtraCR05.Value = Convert.ToDecimal(dr["ExtraCR05"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB06"].ToString()))
                        this.LocalDB06.Value = Convert.ToDecimal(dr["LocalDB06"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR06"].ToString()))
                        this.LocalCR06.Value = Convert.ToDecimal(dr["LocalCR06"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB06"].ToString()))
                        this.ExtraDB06.Value = Convert.ToDecimal(dr["ExtraDB06"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR06"].ToString()))
                        this.ExtraCR06.Value = Convert.ToDecimal(dr["ExtraCR06"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB07"].ToString()))
                        this.LocalDB07.Value = Convert.ToDecimal(dr["LocalDB07"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR07"].ToString()))
                        this.LocalCR07.Value = Convert.ToDecimal(dr["LocalCR07"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB07"].ToString()))
                        this.ExtraDB07.Value = Convert.ToDecimal(dr["ExtraDB07"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR07"].ToString()))
                        this.ExtraCR07.Value = Convert.ToDecimal(dr["ExtraCR07"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB08"].ToString()))
                        this.LocalDB08.Value = Convert.ToDecimal(dr["LocalDB08"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR08"].ToString()))
                        this.LocalCR08.Value = Convert.ToDecimal(dr["LocalCR08"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB08"].ToString()))
                        this.ExtraDB08.Value = Convert.ToDecimal(dr["ExtraDB08"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR08"].ToString()))
                        this.ExtraCR08.Value = Convert.ToDecimal(dr["ExtraCR08"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB09"].ToString()))
                        this.LocalDB09.Value = Convert.ToDecimal(dr["LocalDB09"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR09"].ToString()))
                        this.LocalCR09.Value = Convert.ToDecimal(dr["LocalCR09"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB09"].ToString()))
                        this.ExtraDB09.Value = Convert.ToDecimal(dr["ExtraDB09"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR09"].ToString()))
                        this.ExtraCR09.Value = Convert.ToDecimal(dr["ExtraCR09"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB10"].ToString()))
                        this.LocalDB10.Value = Convert.ToDecimal(dr["LocalDB10"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR10"].ToString()))
                        this.LocalCR10.Value = Convert.ToDecimal(dr["LocalCR10"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB10"].ToString()))
                        this.ExtraDB10.Value = Convert.ToDecimal(dr["ExtraDB10"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR10"].ToString()))
                        this.ExtraCR10.Value = Convert.ToDecimal(dr["ExtraCR10"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB11"].ToString()))
                        this.LocalDB11.Value = Convert.ToDecimal(dr["LocalDB11"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR11"].ToString()))
                        this.LocalCR11.Value = Convert.ToDecimal(dr["LocalCR11"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB11"].ToString()))
                        this.ExtraDB11.Value = Convert.ToDecimal(dr["ExtraDB11"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR11"].ToString()))
                        this.ExtraCR11.Value = Convert.ToDecimal(dr["ExtraCR11"]);
                    if (!string.IsNullOrEmpty(dr["LocalDB12"].ToString()))
                        this.LocalDB12.Value = Convert.ToDecimal(dr["LocalDB12"]);
                    if (!string.IsNullOrEmpty(dr["LocalCR12"].ToString()))
                        this.LocalCR12.Value = Convert.ToDecimal(dr["LocalCR12"]);
                    if (!string.IsNullOrEmpty(dr["ExtraDB12"].ToString()))
                        this.ExtraDB12.Value = Convert.ToDecimal(dr["ExtraDB12"]);
                    if (!string.IsNullOrEmpty(dr["ExtraCR12"].ToString()))
                        this.ExtraCR12.Value = Convert.ToDecimal(dr["ExtraCR12"]);
                    if (!string.IsNullOrEmpty(dr["ExtraINI"].ToString()))
                        this.ExtraINI.Value = Convert.ToDecimal(dr["ExtraINI"]);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coCierreMes(bool resetValue = false)
        {
            this.InitCols();

            if (resetValue)
            {
                this.LocalINI.Value = 0;
                this.ExtraINI.Value = 0;
                #region Local DB
                this.LocalDB01.Value = 0;
                this.LocalDB02.Value = 0;
                this.LocalDB03.Value = 0;
                this.LocalDB04.Value = 0;
                this.LocalDB05.Value = 0;
                this.LocalDB06.Value = 0;
                this.LocalDB07.Value = 0;
                this.LocalDB08.Value = 0;
                this.LocalDB09.Value = 0;
                this.LocalDB10.Value = 0;
                this.LocalDB11.Value = 0;
                this.LocalDB12.Value = 0;
                #endregion
                #region Local CR
                this.LocalCR01.Value = 0;
                this.LocalCR02.Value = 0;
                this.LocalCR03.Value = 0;
                this.LocalCR04.Value = 0;
                this.LocalCR05.Value = 0;
                this.LocalCR06.Value = 0;
                this.LocalCR07.Value = 0;
                this.LocalCR08.Value = 0;
                this.LocalCR09.Value = 0;
                this.LocalCR10.Value = 0;
                this.LocalCR11.Value = 0;
                this.LocalCR12.Value = 0;
                #endregion
                #region Extra DB
                this.ExtraDB01.Value = 0;
                this.ExtraDB02.Value = 0;
                this.ExtraDB03.Value = 0;
                this.ExtraDB04.Value = 0;
                this.ExtraDB05.Value = 0;
                this.ExtraDB06.Value = 0;
                this.ExtraDB07.Value = 0;
                this.ExtraDB08.Value = 0;
                this.ExtraDB09.Value = 0;
                this.ExtraDB10.Value = 0;
                this.ExtraDB11.Value = 0;
                this.ExtraDB12.Value = 0;
                #endregion
                #region Extra CR
                this.ExtraCR01.Value = 0;
                this.ExtraCR02.Value = 0;
                this.ExtraCR03.Value = 0;
                this.ExtraCR04.Value = 0;
                this.ExtraCR05.Value = 0;
                this.ExtraCR06.Value = 0;
                this.ExtraCR07.Value = 0;
                this.ExtraCR08.Value = 0;
                this.ExtraCR09.Value = 0;
                this.ExtraCR10.Value = 0;
                this.ExtraCR11.Value = 0;
                this.ExtraCR12.Value = 0;
                #endregion
                #region Local Inicial
                this.LocalINI01.Value = 0;
                this.LocalINI02.Value = 0;
                this.LocalINI03.Value = 0;
                this.LocalINI04.Value = 0;
                this.LocalINI05.Value = 0;
                this.LocalINI06.Value = 0;
                this.LocalINI07.Value = 0;
                this.LocalINI08.Value = 0;
                this.LocalINI09.Value = 0;
                this.LocalINI10.Value = 0;
                this.LocalINI11.Value = 0;
                this.LocalINI12.Value = 0;
                #endregion
                #region Extra Inicial
                this.ExtraINI01.Value = 0;
                this.ExtraINI02.Value = 0;
                this.ExtraINI03.Value = 0;
                this.ExtraINI04.Value = 0;
                this.ExtraINI05.Value = 0;
                this.ExtraINI06.Value = 0;
                this.ExtraINI07.Value = 0;
                this.ExtraINI08.Value = 0;
                this.ExtraINI09.Value = 0;
                this.ExtraINI10.Value = 0;
                this.ExtraINI11.Value = 0;
                this.ExtraINI12.Value = 0;
                #endregion 
            }          
        }

  
        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.Ano = new UDTSQL_smallint();
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.CuentaID = new UDT_CuentaID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.LocalINI = new UDT_Valor();
            this.ExtraINI = new UDT_Valor();
            #region Local DB
            this.LocalDB01 = new UDT_Valor();
            this.LocalDB02 = new UDT_Valor();
            this.LocalDB03 = new UDT_Valor();
            this.LocalDB04 = new UDT_Valor();
            this.LocalDB05 = new UDT_Valor();
            this.LocalDB06 = new UDT_Valor();
            this.LocalDB07 = new UDT_Valor();
            this.LocalDB08 = new UDT_Valor();
            this.LocalDB09 = new UDT_Valor();
            this.LocalDB10 = new UDT_Valor();
            this.LocalDB11 = new UDT_Valor();
            this.LocalDB12 = new UDT_Valor(); 
            #endregion
            #region Local CR
            this.LocalCR01 = new UDT_Valor();
            this.LocalCR02 = new UDT_Valor();
            this.LocalCR03 = new UDT_Valor();
            this.LocalCR04 = new UDT_Valor();
            this.LocalCR05 = new UDT_Valor();
            this.LocalCR06 = new UDT_Valor();
            this.LocalCR07 = new UDT_Valor();
            this.LocalCR08 = new UDT_Valor();
            this.LocalCR09 = new UDT_Valor();
            this.LocalCR10 = new UDT_Valor();
            this.LocalCR11 = new UDT_Valor();
            this.LocalCR12 = new UDT_Valor(); 
            #endregion
            #region Extra DB
            this.ExtraDB01 = new UDT_Valor();
            this.ExtraDB02 = new UDT_Valor();
            this.ExtraDB03 = new UDT_Valor();
            this.ExtraDB04 = new UDT_Valor();
            this.ExtraDB05 = new UDT_Valor();
            this.ExtraDB06 = new UDT_Valor();
            this.ExtraDB07 = new UDT_Valor();
            this.ExtraDB08 = new UDT_Valor();
            this.ExtraDB09 = new UDT_Valor();
            this.ExtraDB10 = new UDT_Valor();
            this.ExtraDB11 = new UDT_Valor();
            this.ExtraDB12 = new UDT_Valor(); 
            #endregion
            #region Extra CR
            this.ExtraCR01 = new UDT_Valor();
            this.ExtraCR02 = new UDT_Valor();
            this.ExtraCR03 = new UDT_Valor();
            this.ExtraCR04 = new UDT_Valor();
            this.ExtraCR05 = new UDT_Valor();
            this.ExtraCR06 = new UDT_Valor();
            this.ExtraCR07 = new UDT_Valor();
            this.ExtraCR08 = new UDT_Valor();
            this.ExtraCR09 = new UDT_Valor();
            this.ExtraCR10 = new UDT_Valor();
            this.ExtraCR11 = new UDT_Valor();
            this.ExtraCR12 = new UDT_Valor(); 
            #endregion
           
            //Campos Adicionales
            this.Rompimiento1 = new UDTSQL_bool();
            this.Rompimiento2 = new UDTSQL_bool();
            this.Descriptivo = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.SaldoLocal = new UDT_Valor();
            this.SaldoExtra = new UDT_Valor();
            this.Detalle = new List<DTO_coCierreMes>();
            this.PeriodoID = new UDT_PeriodoID();
            #region Local Inicial
            this.LocalINI01 = new UDT_Valor();
            this.LocalINI02 = new UDT_Valor();
            this.LocalINI03 = new UDT_Valor();
            this.LocalINI04 = new UDT_Valor();
            this.LocalINI05 = new UDT_Valor();
            this.LocalINI06 = new UDT_Valor();
            this.LocalINI07 = new UDT_Valor();
            this.LocalINI08 = new UDT_Valor();
            this.LocalINI09 = new UDT_Valor();
            this.LocalINI10 = new UDT_Valor();
            this.LocalINI11 = new UDT_Valor();
            this.LocalINI12 = new UDT_Valor();
            #endregion
            #region Extra Inicial
            this.ExtraINI01 = new UDT_Valor();
            this.ExtraINI02 = new UDT_Valor();
            this.ExtraINI03 = new UDT_Valor();
            this.ExtraINI04 = new UDT_Valor();
            this.ExtraINI05 = new UDT_Valor();
            this.ExtraINI06 = new UDT_Valor();
            this.ExtraINI07 = new UDT_Valor();
            this.ExtraINI08 = new UDT_Valor();
            this.ExtraINI09 = new UDT_Valor();
            this.ExtraINI10 = new UDT_Valor();
            this.ExtraINI11 = new UDT_Valor();
            this.ExtraINI12 = new UDT_Valor();
            #endregion
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDTSQL_smallint Ano { get; set; }

        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Valor LocalINI { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI { get; set; }
        
        #region Local BD
        [DataMember]
        public UDT_Valor LocalDB01 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB02 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB03 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB04 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB05 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB06 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB07 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB08 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB09 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB10 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB11 { get; set; }

        [DataMember]
        public UDT_Valor LocalDB12 { get; set; }
        
        #endregion

        #region Local CR
        [DataMember]
        public UDT_Valor LocalCR01 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR02 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR03 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR04 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR05 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR06 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR07 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR08 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR09 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR10 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR11 { get; set; }

        [DataMember]
        public UDT_Valor LocalCR12 { get; set; } 
        #endregion

        #region Extra DB

        [DataMember]
        public UDT_Valor ExtraDB01 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB02 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB03 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB04 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB05 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB06 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB07 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB08 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB09 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB10 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB11 { get; set; }

        [DataMember]
        public UDT_Valor ExtraDB12 { get; set; }
        
        #endregion

        #region Extra CR
        [DataMember]
        public UDT_Valor ExtraCR01 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR02 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR03 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR04 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR05 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR06 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR07 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR08 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR09 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR10 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR11 { get; set; }

        [DataMember]
        public UDT_Valor ExtraCR12 { get; set; } 
        #endregion

        #region Campos Adicionales

        [DataMember]
        [AllowNull]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor SaldoLocal { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor SaldoExtra { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        [AllowNull]
        public List<DTO_coCierreMes> Detalle { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_bool Rompimiento1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_bool Rompimiento2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PeriodoID PeriodoID { get; set; }

        #region Local INICIAL
        [DataMember]
        public UDT_Valor LocalINI01 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI02 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI03 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI04 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI05 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI06 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI07 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI08 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI09 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI10 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI11 { get; set; }

        [DataMember]
        public UDT_Valor LocalINI12 { get; set; }

        #endregion

        #region Extra INICIAL
        [DataMember]
        public UDT_Valor ExtraINI01 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI02 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI03 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI04 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI05 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI06 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI07 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI08 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI09 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI10 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI11 { get; set; }

        [DataMember]
        public UDT_Valor ExtraINI12 { get; set; }

        #endregion

        #endregion

        #endregion
    }
}
