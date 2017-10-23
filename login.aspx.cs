using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Cookies[CMD5.Encriptar("KeepUnitUserCookie")].Expires = DateTime.Today.AddDays(-1);
        string Contenido = "";
        CUnit.Accion(delegate(CDB Conn)
        {
            Contenido = CViews.CargarView("tmplBootstrapPage.html");
            string Login = CViews.CargarView("tmplLogin.html");

            Contenido = Contenido.Replace("[Title]", "Inicio de sesión");
            Contenido = Contenido.Replace("[JS]", "<script src=\"js/Control.Login.js?_=" + DateTime.Now.Ticks + "\"></script>");
            Contenido = Contenido.Replace("[Contenido]", Login);
            Conn.Cerrar();
        });

        Response.Write(Contenido);

        Response.End();
        
    }
}