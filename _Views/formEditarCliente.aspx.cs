using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Views_formEditarCliente : System.Web.UI.Page
{

    public static string Id = "0";
	public static string Cliente = "";
	public static string IdMunicpio = "0";
	public static string IdEstado = "0";
	public static string IdPais = "0";
	public static CArreglo Municipios = new CArreglo();
	public static CArreglo Estados = new CArreglo();
	public static CArreglo Paises = new CArreglo();

	protected void Page_Load(object sender, EventArgs e)
	{
		CUnit.Accion(delegate (CDB conn) {
			int IdCliente = Convert.ToInt32(Request["IdCliente"]);
			if (IdCliente > 0)
			{
				string query = "SELECT * FROM Cliente WHERE IdCliente = @IdCliente";
				conn.DefinirQuery(query);
				conn.AgregarParametros("@IdCliente", IdCliente);
				CObjeto oCliente = conn.ObtenerRegistro();
				
                Cliente = IdCliente.ToString();
				if (oCliente.Exist("Cliente"))
				{
                    Id = oCliente.Get("IdCliente").ToString();
					Cliente = oCliente.Get("Cliente").ToString();
					IdMunicpio = oCliente.Get("IdMunicipio").ToString();

					query = "SELECT * FROM Municipio WHERE IdMunicipio = @IdMunicipio";
					conn.DefinirQuery(query);
					conn.AgregarParametros("@IdMunicipio", IdMunicpio);
					CObjeto Validar = conn.ObtenerRegistro();
					IdEstado = Validar.Get("IdEstado").ToString();

					query = "SELECT * FROM Estado WHERE IdEstado = @IdEstado";
					conn.DefinirQuery(query);
					conn.AgregarParametros("@IdEstado", IdEstado);
					Validar = conn.ObtenerRegistro();
					IdPais = Validar.Get("IdPais").ToString();
                    /**/
                    query = "SELECT * FROM Municipio WHERE IdEstado=@IdEstado";
					conn.DefinirQuery(query);
                    conn.AgregarParametros("@IdEstado", IdEstado);
					Municipios = conn.ObtenerRegistros();

                    query = "SELECT * FROM Estado WHERE IdPais=@IdPais";
					conn.DefinirQuery(query);
                    conn.AgregarParametros("@IdPais", IdPais);
					Estados = conn.ObtenerRegistros();

					query = "SELECT * FROM Pais";
					conn.DefinirQuery(query);
					Paises = conn.ObtenerRegistros();
				}
			}
		});
	}

}