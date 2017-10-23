using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class ReporteMetaVsReal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CSecurity permiso = new CSecurity();
        if (!permiso.tienePermiso("accesoReporteMedicion"))
        {
            Response.Redirect("login.aspx");
        }
        else
        {
            MostrarContenido();
        }

    }

    public void MostrarContenido()
    {
        string Pagina = "";
        CUnit.Firmado(delegate(CDB Conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
            string Logo = "";
            JObject response = new JObject();
            response.Add(new JProperty("IdUsuario", IdUsuario));
            response = CUsuario.ObtenerJsonClienteObtieneDatosUsuarioSesion(response);
            Logo = Convert.ToString(response.Property("Logo").Value.ToString());
            Logo = "Archivos/Logo/" + Logo;
            CSecurity Menu = new CSecurity();
            int idPerfil = Convert.ToInt32(response.Property("IdPerfil").Value.ToString());
            string MostrarMenu = Menu.CrearMenu(idPerfil);

            Pagina = CViews.CargarView("tmplBootstrapPage.html");
            string Contenido = CViews.CargarView("tmplContenido.html");
            string Pantalla = CViews.CargarView("tmplReporteMetaVsReal.html");
            Contenido = Contenido.Replace("[Logo]", Logo);
            Contenido = Contenido.Replace("[Menu]", MostrarMenu);

            Contenido = Contenido.Replace("[Contenido]", Pantalla);
            Pagina = Pagina.Replace("[Title]", "Reporte Meta vs Real");
            Pagina = Pagina.Replace("[Contenido]", Contenido);
            Pagina = Pagina.Replace("[JS]", "<script src=\"js/Control.ReporteMetavsReal.js?_=" + DateTime.Now.Ticks + "\"></script>");
        });
        Response.Write(Pagina);
        Response.End();
    }
}