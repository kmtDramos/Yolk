using System.Web.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Script.Services;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mime;

public partial class _Controls_Municipio : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string ListarMunicipio(int Pagina, string Columna, string Orden)
	{
		CObjeto Respuesta = new CObjeto();
		CUnit.Firmado(delegate (CDB Conn)
		{
			string Error = Conn.Mensaje;
			if (Conn.Conectado)
			{
				CObjeto Datos = new CObjeto();
				int Paginado = 10;
				int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
				CDB ConexionBaseDatos = new CDB();
				SqlConnection con = ConexionBaseDatos.conStr();
				SqlCommand Stored = new SqlCommand("spg_grdMunicipio", con);
				Stored.CommandType = CommandType.StoredProcedure;
				Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
				Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
				Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
				Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
				Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
				Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;
				SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);

				DataSet ds = new DataSet();
				dataAdapterRegistros.Fill(ds);

				DataTable DataTablePaginador = ds.Tables[0];
				DataTable DataTablePermisos = ds.Tables[1];

				Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
				Datos.Add("Permisos", Conn.ObtenerRegistrosDataTable(DataTablePermisos));
				Respuesta.Add("Datos", Datos);
			}

			Respuesta.Add("Error", Error);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerPaises()
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				CObjeto Datos = new CObjeto();

				string query = "SELECT IdPais AS Valor, Pais AS Etiqueta FROM Pais WHERE Baja = 0";
				conn.DefinirQuery(query);
				CArreglo Paises = conn.ObtenerRegistros();

				Datos.Add("Paises", Paises);

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);
		});
		return Respuesta.ToString();
	}


	[WebMethod]
	public static string ObtenerEstados(int IdPais)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				CObjeto Datos = new CObjeto();

				string query = "SELECT IdEstado AS Valor, Estado AS Etiqueta FROM Estado WHERE IdPais = @IdPais AND Baja = 0";
				conn.DefinirQuery(query);
				conn.AgregarParametros("@IdPais", IdPais);
				CArreglo Estados = conn.ObtenerRegistros();

				Datos.Add("Estados", Estados);

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);
		});

		return Respuesta.ToString();
	}


	[WebMethod]
	public static string DesactivarMunicipio(int IdMunicipio, int Baja)
	{
		CObjeto Respuesta = new CObjeto();
		string Error = "";

		CUnit.Firmado(delegate (CDB Conn)
		{
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeManipularBajaMunicipio"))
			{
				if (Conn.Conectado)
				{
					CObjeto Datos = new CObjeto();

					int desactivar = 0;
					if (Baja == 0)
					{
						desactivar = 1;
					}
					else
					{
						desactivar = 0;
					}

					CMunicipio cMpio = new CMunicipio();
					cMpio.IdMunicipio = IdMunicipio;
					cMpio.Obtener(Conn);
					cMpio.Baja = desactivar;
					cMpio.Desactivar(Conn);
					Respuesta.Add("Datos", cMpio);
				}
				else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }


			Respuesta.Add("Error", Error);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
    public static string ObtenerFormaAgregarMunicipio()
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";
         
        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarMunicipio"))
            {

                if (conn.Conectado)
                {
					//Aqui en lugar de hacer arreglo de permisos es de Paises
                    CObjeto Datos = new CObjeto();
					string spPermiso = "EXEC sp_Pais_Obtener @Opcion";
					conn.DefinirQuery(spPermiso);
					conn.AgregarParametros("@Opcion", 1);
					CArreglo Paises = conn.ObtenerRegistros();
					Respuesta.Add("Paises", Paises);
				}
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

	[WebMethod]
	public static string AgregarMunicipio(int IdPais, int IdEstado, string Municipio)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB Conn)
		{
			string Error = "";
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeAgregarMunicipio"))
			{
				if (Conn.Conectado)
				{
					CObjeto Datos = new CObjeto();
					CMunicipio cMunicipio = new CMunicipio();
					cMunicipio.IdPais = IdPais;
					cMunicipio.IdEstado = IdEstado;
					cMunicipio.Municipio = Municipio;
					cMunicipio.Baja = 0;
					Error = ValidarMunicipio(cMunicipio);
					if (Error == "")
					{
						CObjeto Valida = new CObjeto();
						int IdMunicipio = CMunicipio.ValidaExiste(IdPais, IdEstado, Municipio, Conn);
						if (IdMunicipio != 0)
						{
							Error = Error + "<li>Ya existe este Municipio.</li>";
						}
						else
						{
							cMunicipio.Agregar(Conn);
						}
					}
					Respuesta.Add("Datos", Datos);
				}
				else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

			Respuesta.Add("Error", Error);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerFormaEditarMunicipio(int IdMunicipio)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			string Error = "";
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeEditarMunicipio"))
			{

				if (conn.Conectado)
				{
					CObjeto Datos = new CObjeto();

					string spMunicipio = "SELECT * FROM Municipio WHERE IdMunicipio=@IdMunicipio ";
					conn.DefinirQuery(spMunicipio);
					conn.AgregarParametros("@IdMunicipio", IdMunicipio);
					CObjeto oMpio = conn.ObtenerRegistro();
					Datos.Add("IdMunicipio", oMpio.Get("IdMunicipio").ToString());
					Datos.Add("Municipio", oMpio.Get("Municipio").ToString());
					Datos.Add("IdEstado", oMpio.Get("IdEstado").ToString());
					Datos.Add("IdPais", oMpio.Get("IdPais").ToString());
					Respuesta.Add("Dato", Datos);


				}
				else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

			Respuesta.Add("Error", Error);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string EditarMunicipio(int IdMunicipio, string Municipio, int IdEstado, int IdPais)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB Conn)
		{

			string Error = "";
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeEditarMunicipio"))
			{
				if (Conn.Conectado)
				{

					CObjeto Datos = new CObjeto();
					CMunicipio cMpio = new CMunicipio();
					cMpio.IdMunicipio = IdMunicipio;
					cMpio.Municipio = Municipio;
					cMpio.IdEstado = IdEstado;
					cMpio.IdPais = IdPais;
					Error = ValidarMunicipio(cMpio);
					if (Error == "")
					{
						int ExisteNom = CMunicipio.ValidaExisteEditaMunicipio(IdMunicipio, Municipio, IdEstado, IdPais, Conn);
						if (ExisteNom != 0)
						{
							Error = Error + "<li>Ya existe un municipio con el mismo Nombre.</li>";
						}
						else
						{
							cMpio.Editar(Conn);
						}
					}
					Respuesta.Add("Datos", Datos);
				}
				else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

			Respuesta.Add("Error", Error);
		});

		return Respuesta.ToString();
	}

	private static string ValidarMunicipio(CMunicipio Municipio)
	{
		string Mensaje = "";
		
		Mensaje += (Municipio.IdPais == 0) ? "<li>Favor de completar el campo Pais.</li>" : Mensaje;
		Mensaje += (Municipio.IdEstado == 0) ? "<li>Favor de completar el campo Estado.</li>" : Mensaje;
		Mensaje += (Municipio.Municipio == "") ? "<li>Favor de completar el campo Municipio.</li>" : Mensaje;

		Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

		return Mensaje;
	}
}