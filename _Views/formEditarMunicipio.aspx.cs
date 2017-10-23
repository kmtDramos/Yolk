using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Views_formEditarMunicipio : System.Web.UI.Page
{

    public static string Id = "0";
	public static string Municipio = "";
    public static string NombreMunicipio = "";
	public static string IdEstado = "0";
	public static string IdPais = "0";
	public static CArreglo Estados = new CArreglo();
	public static CArreglo Paises = new CArreglo();

	protected void Page_Load(object sender, EventArgs e)
	{
		CUnit.Accion(delegate (CDB conn) {
			int IdMunicipio = Convert.ToInt32(Request["IdMunicipio"]);
			if (IdMunicipio > 0)
			{
				string query = "SELECT * FROM Municipio WHERE IdMunicipio = @IdMunicipio";
				conn.DefinirQuery(query);
				conn.AgregarParametros("@IdMunicipio", IdMunicipio);
				CObjeto oMpio = conn.ObtenerRegistro();

				Municipio = IdMunicipio.ToString();
				if (oMpio.Exist("Municipio"))
				{
                    Id = oMpio.Get("IdMunicipio").ToString();
					NombreMunicipio = oMpio.Get("Municipio").ToString();
					IdEstado = oMpio.Get("IdEstado").ToString();
                    IdPais = oMpio.Get("IdPais").ToString();

                    query = "SELECT * FROM Estado WHERE Baja = 0";
					conn.DefinirQuery(query);
					Estados = conn.ObtenerRegistros();

					query = "SELECT * FROM Pais WHERE Baja = 0";
					conn.DefinirQuery(query);
					Paises = conn.ObtenerRegistros();
				}
			}
		});
	}

}