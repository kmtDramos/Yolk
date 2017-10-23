using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class Circuito : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        CSecurity permiso = new CSecurity();
        if (!permiso.tienePermiso("accesoCircuito"))
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
            string Logo ="";
            JObject response = new JObject();
            response.Add(new JProperty("IdUsuario", IdUsuario));
            response = CUsuario.ObtenerJsonClienteObtieneDatosUsuarioSesion(response); 
            Logo= Convert.ToString(response.Property("Logo").Value.ToString());
            Logo = "Archivos/Logo/" + Logo;
            CSecurity Menu = new CSecurity();
            int idPerfil = Convert.ToInt32(response.Property("IdPerfil").Value.ToString());
            string MostrarMenu = Menu.CrearMenu(idPerfil);

            Pagina = CViews.CargarView("tmplBootstrapPage.html");
            string Contenido = CViews.CargarView("tmplContenido.html");
            string Pantalla = CViews.CargarView("tmplCircuitos.html");            

            Contenido = Contenido.Replace("[Contenido]", Pantalla);
            Contenido = Contenido.Replace("[Logo]", Logo);
            Contenido = Contenido.Replace("[Menu]", MostrarMenu);
            Pagina = Pagina.Replace("[Title]", "Circuitos");
            Pagina = Pagina.Replace("[Contenido]", Contenido);
            Pagina = Pagina.Replace("[JS]", "<script src=\"js/Control.Circuito.js?_=" + DateTime.Now.Ticks + "\"></script>");
        });

        Response.Write(Pagina);
        Response.End();    
    }
}