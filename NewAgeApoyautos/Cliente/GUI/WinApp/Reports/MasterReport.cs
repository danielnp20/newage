using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.Proxy.Model;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraReports.UI;
using NewAge.Librerias.Project;
using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    public class MasterReport : BaseReport
    {
        DTO_aplMaestraPropiedades FrmProperties;

        /// <summary>
        /// Constructor de reporte de maestras
        /// </summary>
        /// <param name="docId">Número de documento de la maestra</param>
        /// <param name="Consulta">Consulta que constiene la selección de columnas</param>
        public MasterReport(int docId, DTO_glConsulta consulta=null){

            this.Consulta = consulta;

            this.FrmProperties = _bc.AdministrationModel.MasterProperties[docId];
            List<DTO_aplMaestraCampo> fields = new List<DTO_aplMaestraCampo>();
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, docId.ToString());
            if (this.FrmProperties.ColumnaID != "" && !this.FrmProperties.ColumnaID.Contains(",")){
                DTO_aplMaestraCampo Col_ID = new DTO_aplMaestraCampo("UDT_BasicID");
                Col_ID.NombreColumna = this.FrmProperties.ColumnaID;
                fields.Add(Col_ID);

                DTO_aplMaestraCampo descripcion = new DTO_aplMaestraCampo("UDT_Descriptivo");
                descripcion.NombreColumna = "Descriptivo";

                fields.Add(descripcion);

            }
            DTO_aplMaestraCampo activoInd = new DTO_aplMaestraCampo("UDT_SiNo");
            activoInd.NombreColumna = "ActivoInd";

            fields.AddRange(this.FrmProperties.Campos.Where(x=>x.GrillaInd==true).ToList());

            fields.Add(activoInd);

            //Trae recursos para parametros del reporte
            this.lblFecha.Text = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DateReport);
            this.lblPage.Text = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PageReport);
            this.lblUser.Text = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.UserReport);

            //Valida el numero de columnas y ajusta el tamaño de pagina, de fuentes y ubicación de controles
            if (fields.Count() > 10)
            {
                this.Landscape = true;
                this.lblFecha.LocationFloat = new DevExpress.Utils.PointFloat(840.4166F, 0F);
                this.lblParamFecha.LocationFloat = new DevExpress.Utils.PointFloat(887.2916F, 0F);
                this.lblReportName.LocationFloat = new DevExpress.Utils.PointFloat(315.8334F, 112.5416F);
                this.lblPage.LocationFloat = new DevExpress.Utils.PointFloat(945.5416F, 100.999982F);
                this.xrPage.LocationFloat = new DevExpress.Utils.PointFloat(1000.7083F, 100.999982F);
                this.lblUser.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 100.999982F);
                this.lblUserName.LocationFloat = new DevExpress.Utils.PointFloat(60.16669F, 100.999982F);
                this.PageHeader.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Bold);
                this.Detail.Font = new System.Drawing.Font("Arial Narrow", 7F);
                if (fields.Count() > 20)
                {
                    this.PageHeader.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Bold);
                    this.Detail.Font = new System.Drawing.Font("Arial Narrow", 6F);
                }
            }
            // Create a table to represent headers
            XRTable tableHeader = new XRTable();
            tableHeader.Height = 10;
            tableHeader.Width = (this.PageWidth - (this.Margins.Left + this.Margins.Right));
            XRTableRow headerRow = new XRTableRow();
            headerRow.Width = tableHeader.Width;
            //tableHeader.Rows.Add(headerRow);

            // Create a table to display data
            XRTable tableDetail = new XRTable();
            tableDetail.Height = 10;
            tableDetail.Width = (this.PageWidth - (this.Margins.Left + this.Margins.Right));
            XRTableRow detailRow = new XRTableRow();
            detailRow.Width = tableDetail.Width;
            //tableDetail.Rows.Add(detailRow);
            tableDetail.EvenStyleName = "EvenStyle";
            tableDetail.OddStyleName = "OddStyle";

            int tamPredemGrilla=120;
                
            int[] anchosGrilla=new int[fields.Count()];
            int anchoTotal = 0;
            int i = 0;
            foreach (DTO_aplMaestraCampo campo in fields){
                if (campo.Tipo.Equals("UDT_Descriptivo") && campo.ColumnaPosicion == 0)
                {
                    anchoTotal += 300;
                    anchosGrilla[i] = 300; ;
                    i++;
                }
                else
                {
                    anchoTotal += campo.ColumnaTamano == 0 ? tamPredemGrilla : campo.ColumnaTamano;
                    anchosGrilla[i] = campo.ColumnaTamano == 0 ? tamPredemGrilla : campo.ColumnaTamano;
                    i++;
                } 
            }
            decimal rel = ((decimal)tableDetail.Width) / ((decimal)anchoTotal);
                
            for (i = 0; i < anchosGrilla.Count(); i++)
            {
                anchosGrilla[i] = (int)(anchosGrilla[i] * rel);
            }

            this.Detail.HeightF = 10;
            this.Detail.WidthF = 100;

            // Create table cells, fill the header cells with text, bind the cells to data
            i = 0;
            foreach (DTO_aplMaestraCampo campo in fields)
            {
                if (this.Consulta != null && this.Consulta.Selecciones!=null)
                {
                    //Verificar si está dentro de la selecciones de la consulta, si no está, no se hace nada
                    string columnaConsulta = campo.NombreColumna;

                    if (this.FrmProperties.ColumnaID.Equals(campo.NombreColumna))
                        columnaConsulta = "ID";
                    if (this.Consulta.Selecciones.Where(x => x.CampoFisico.Equals(columnaConsulta)).Count() == 0)
                        continue;
                }
                XRTableCell headerCell = new XRTableCell();

                headerCell.BackColor = Color.DimGray;
                headerCell.ForeColor = Color.White;
                headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                headerCell.Font = new Font(headerCell.Font, FontStyle.Bold);
            
                headerCell.Width = anchosGrilla[i];
                string resourceKey = docId + "_" + campo.NombreColumna;

                headerCell.Text = this._bc.GetResource(LanguageTypes.Forms, resourceKey);
                
                headerCell.Text = fields.Count() > 20 && headerCell.Text.Count() > 15 ? headerCell.Text.Substring(0, 11) : headerCell.Text;

                XRTableCell detailCell = new XRTableCell();
                detailCell.Width = headerCell.Width;
                
                if (this.FrmProperties.ColumnaID.Equals(campo.NombreColumna)){
                    detailCell.DataBindings.Add("Text", null, "ID");
                }
                else{
                    detailCell.DataBindings.Add("Text", null, campo.NombreColumna);
                }
                headerCell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
              
                // Place the cells into the corresponding tables
                headerRow.Cells.Add(headerCell);
                detailRow.Cells.Add(detailCell);
                i++;
            }
            
            tableHeader.Rows.Add(headerRow);
            tableDetail.Rows.Add(detailRow);
            this.Bands[BandKind.PageHeader].Controls.Add(tableHeader);
            this.Bands[BandKind.Detail].Controls.Add(tableDetail);
        }

    }
}
