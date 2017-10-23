using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Views_formEditarMedidor : System.Web.UI.Page
{
    public static int IdMedidor = 0;
    public static string Medidor = "";
    public static string Checked = "Checked";

    protected void Page_Load(object sender, EventArgs e)
    {
        CUnit.Firmado(delegate(CDB Conn)
        {
            if (Conn.Conectado)
            {
                IdMedidor = Convert.ToInt32(Request["IdMedidor"]);
                CMedidores cMedidor = new CMedidores();
                cMedidor.IdMedidor = IdMedidor;
                cMedidor.Obtener(Conn);
                Medidor = cMedidor.Medidor;
                Checked = (cMedidor.Baja == false) ? "checked" : "";
            }
        });

    }
}