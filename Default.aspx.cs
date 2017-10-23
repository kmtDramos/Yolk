using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        string Contenido = "";
        CUnit.Accion(delegate(CDB Conn)
        {
            if (CSecurity.HaySesion(Conn))
            {
                Response.Redirect("Inicio.aspx");
            }

            Contenido = CViews.CargarView("tmplBootstrapPage.html");
            string Login = CViews.CargarView("tmplLogin.html");

            Contenido = Contenido.Replace("[Title]", "Inicio de sesión");
            Contenido = Contenido.Replace("[JS]", "<script src=\"js/Control.Login.js?_=" + DateTime.Now.Ticks + "\"></script>");
            Contenido = Contenido.Replace("[Contenido]", Login);
        });

        Response.Write(Contenido);
        Response.End();
	}
}