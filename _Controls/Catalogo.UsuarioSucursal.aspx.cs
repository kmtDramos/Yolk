using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Controls_Catalogo_UsuarioSucursal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ObtenerFormaListarMetas(int IdCircuito)
    {

        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdCircuito AS Valor, Circuito as Numero, Descripcion AS Etiqueta FROM Circuito WHERE IdCircuito=@IdCircuito";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCircuito", IdCircuito);
                CObjeto Circuito = conn.ObtenerRegistro();
                Circuito.Add("IdCircuito", Convert.ToInt32(Circuito.Get("Valor")));
                Circuito.Add("Circuito", Convert.ToInt32(Circuito.Get("Numero")));
                Circuito.Add("Descripcion", Convert.ToString(Circuito.Get("Etiqueta")));
                Datos.Add("Circuito", Circuito);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }
}