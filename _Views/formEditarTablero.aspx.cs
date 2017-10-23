using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Views_formEditarTablero : System.Web.UI.Page
{
    public static int IdTablero = 0;
    public static string Tablero = "";
    public static string Checked = "Checked";

    protected void Page_Load(object sender, EventArgs e)
    {
        CUnit.Firmado(delegate(CDB Conn)
        {
            if (Conn.Conectado)
            {
                IdTablero = Convert.ToInt32(Request["IdTablero"]);
                CTablero cTablero = new CTablero();
                cTablero.IdTablero = IdTablero;
                cTablero.Obtener(Conn);
                Tablero = cTablero.Tablero;
                Checked = (cTablero.Baja == false) ? "checked" : "";
            }
        });
    }
}