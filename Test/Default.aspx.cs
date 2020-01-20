using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacturaAfipPdf;

namespace Test
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FacturaAfipPdf.Print.BusinessInfo business = new Print.BusinessInfo();
            business.Address = "Presidente Ibañez 129";
            business.IVACondition = "IVA Responsable Inscripto";
            business.GrossIncome = "207-123456-20";
            business.CUIT = 30714460354;
            business.InitialActivities = DateTime.Now.Date;
            business.BusinessName = "adver infinity group";
            business.PathImage = @"E:\Encabezado_FC.png";

            FacturaAfipPdf.Print.ClientInfo client = new Print.ClientInfo()
            {
                Address = "Presidente Ibañez 129",
                BusinessName = "asdadasda",
                CUIT = 12345678910,
                IVACondition = "Responsable inscripto",
                SaleCondition = "Otra",
                FirstName = "Oscar",
                LastName = "Martinez",
            };

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/pdf";
            response.AppendHeader("Content-Disposition", "inline; filename=" + "Mi_pdf" + ".pdf"); //attachment
            response.ContentType = "application/pdf";
            response.BinaryWrite(FacturaAfipPdf.Print.ReceipToPdf(business,client));
            response.End();
        }
    }
}