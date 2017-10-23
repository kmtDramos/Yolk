using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class Prueba : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Pagina = "";

        CUnit.Firmado(delegate(CDB Conn)
        {
            Pagina = CViews.CargarView("tmplBootstrapPage.html");
            string Contenido = CViews.CargarView("tmplContenido.html");
            string Pantalla = CViews.CargarView("tmplPruebas.html");

            Contenido = Contenido.Replace("[Contenido]", Pantalla);

            Pagina = Pagina.Replace("[Title]", "Tarifas");
            Pagina = Pagina.Replace("[Contenido]", Contenido);
            Pagina = Pagina.Replace("[JS]", "<script src=\"js/Prueba.js?_=" + DateTime.Now.Ticks + "\"></script>");
        });

        Response.Write(Pagina);
        Response.End();
    }
}