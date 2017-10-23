using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class Reporte : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string Pagina = "";

		CUnit.Firmado(delegate (CDB Conn)
		{
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
            string Logo = "";
            JObject response = new JObject();
            response.Add(new JProperty("IdUsuario", IdUsuario));
            response = CUsuario.ObtenerJsonClienteObtieneDatosUsuarioSesion(response);
            Logo = Convert.ToString(response.Property("Logo").Value.ToString());
            Logo = "Archivos/Logo/" + Logo;

			Pagina = CViews.CargarView("tmplBootstrapPage.html");
			string Contenido = CViews.CargarView("tmplContenido.html");
			string Pantalla = CViews.CargarView("tmplReporte.html");
			CFecha Inicio = new CFecha();
			Inicio.Today();
			CFecha Fin = new CFecha();
			Fin.Now();
			Pantalla = Pantalla.Replace("[FechaInicio]", Inicio.ToStringDate());
			Pantalla = Pantalla.Replace("[HoraInicio]", "08:30");
			Pantalla = Pantalla.Replace("[FechaFin]", Fin.ToStringDate());
			Pantalla = Pantalla.Replace("[HoraFin]", Fin.ToShortTimeString());
			Contenido = Contenido.Replace("[Contenido]", Pantalla);
            Contenido = Contenido.Replace("[Logo]", Logo);

			Pagina = Pagina.Replace("[Title]", "Reporte");
			Pagina = Pagina.Replace("[Contenido]", Contenido);
			Pagina = Pagina.Replace("[JS]", "<script src=\"js/Control.Reporte.js?_=" + DateTime.Now.Ticks + "\"></script>");
			
		});

		Response.Write(Pagina);
		Response.End();
	}
}