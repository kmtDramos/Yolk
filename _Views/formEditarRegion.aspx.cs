using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Views_formEditarRegion : System.Web.UI.Page
{
    public static int IdRegion = 0;
    public static string Region = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        CUnit.Firmado(delegate(CDB Conn)
        {
            if (Conn.Conectado)
            {
                IdRegion = Convert.ToInt32(Request["IdRegion"]);
                CRegion cRegion = new CRegion();
                cRegion.IdRegion = IdRegion;
                cRegion.Obtener(Conn);
                Region = cRegion.Region;
            }
        });

    }
}