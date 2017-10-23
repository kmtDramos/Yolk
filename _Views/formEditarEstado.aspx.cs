using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Views_formEditarEstado : System.Web.UI.Page
{

    public static string Id = "0";
	public static string Estado = "";
    public static string NombreEstado = "";
    public static string IdPais = "0";
	public static CArreglo Estados = new CArreglo();
	public static CArreglo Paises = new CArreglo();

	protected void Page_Load(object sender, EventArgs e)
	{
		CUnit.Accion(delegate (CDB conn) {
			int IdEstado = Convert.ToInt32(Request["IdEstado"]);
			if (IdEstado > 0)
			{
				string query = "SELECT * FROM Estado WHERE IdEstado = @IdEstado";
				conn.DefinirQuery(query);
				conn.AgregarParametros("@IdEstado", IdEstado);
				CObjeto oEstado = conn.ObtenerRegistro();
				
                Estado = IdEstado.ToString();
				if (oEstado.Exist("Estado"))
				{
                    Id = oEstado.Get("IdEstado").ToString();
                    query = "SELECT * FROM Estado WHERE IdEstado = @IdEstado";
					conn.DefinirQuery(query);
					conn.AgregarParametros("@IdEstado", IdEstado);
                    CObjeto Validar = conn.ObtenerRegistro();
                    NombreEstado = Validar.Get("Estado").ToString();
                    IdPais = Validar.Get("IdPais").ToString();

                    query = "SELECT * FROM Pais";
					conn.DefinirQuery(query);
					Paises = conn.ObtenerRegistros();
				}
			}
		});
	}

}