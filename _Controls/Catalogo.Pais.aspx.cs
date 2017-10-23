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

public partial class _Controls_Catalogo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	[WebMethod]
	public static string ListarPais(int Pagina, string Columna, string Orden)
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
				SqlCommand Stored = new SqlCommand("spg_grdPais", con);
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
				DataTable DataTablePaises = ds.Tables[1];

				Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
				Datos.Add("Paises", Conn.ObtenerRegistrosDataTable(DataTablePaises));
				Respuesta.Add("Datos", Datos);
			}

			Respuesta.Add("Error", Error);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string DesactivarPais(int IdPais, int Baja)
	{
		CObjeto Respuesta = new CObjeto();
		string Error = "";

		CUnit.Firmado(delegate (CDB Conn)
		{
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeManipularBajaPais"))
			{
				if (Conn.Conectado)
				{
					CObjeto Datos = new CObjeto();
					bool desactivar = false;
					if (Baja == 0)
					{
						desactivar = true;
					}
					else
					{
						desactivar = false;
					}
					CPais cPais = new CPais();
					cPais.IdPais = IdPais;
					cPais.Baja = desactivar;
					cPais.Desactivar(Conn);
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
	public static string ObtenerFormaAgregarPais()
	{
		CObjeto Respuesta = new CObjeto();
		string Error = "";

		CUnit.Firmado(delegate (CDB conn)
		{
			int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeAgregarPais"))
			{
				if (conn.Conectado)
				{
					CObjeto Datos = new CObjeto();
					Respuesta.Add("Datos", Datos);
				}
				else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

			Respuesta.Add("Error", Error);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string AgregarPais(string Pais)
	{
		CObjeto Respuesta = new CObjeto();
		CUnit.Firmado(delegate (CDB Conn)
		{
			string Error = "";
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeAgregarPais"))
			{
				if (Conn.Conectado)
				{
					CObjeto Datos = new CObjeto();
					CPais cPais = new CPais();
					cPais.Pais = Pais;
					cPais.Baja = false;
					Error = ValidaPais(cPais);
					if (Error == ""){
						CObjeto Valida = new CObjeto();
						int IdPais = CPais.ValidaExiste(Pais, Conn);
						if(IdPais != 0)
						{
							Error = Error + "<li>El país ya existe.</li>";
						}
						else
						{
							cPais.Agregar(Conn);
						}
					}
					Respuesta.Add("Datos", Datos);
				}
				else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tiene los permisos necesarios</li>"; }

			Respuesta.Add("Error", Error);
		});
		return Respuesta.ToString();
	}

	private static string ValidaPais(CPais IdPais)
	{
		string Mensaje = "";

		Mensaje += (IdPais.Pais == "") ? "<li>Favor de completar el campo país.</li>" : Mensaje;

		Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

		return Mensaje;
	}

	[WebMethod]
	public static string ObtenerFormaEditarPais(int IdPais)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			string Error = "";
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeEditarPais"))
			{

				if (conn.Conectado)
				{
					CObjeto Datos = new CObjeto();

					string spPais = "SELECT * FROM Pais WHERE IdPais=@IdPais ";
					conn.DefinirQuery(spPais);
					conn.AgregarParametros("@IdPais", IdPais);
					CObjeto oPais = conn.ObtenerRegistro();
					Datos.Add("IdPais", oPais.Get("IdPais").ToString());
					Datos.Add("Pais", oPais.Get("Pais").ToString());
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
	public static string EditarPais(int IdPais, string Pais)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB Conn)
		{

			string Error = "";
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeEditarPais"))
			{
				if (Conn.Conectado)
				{
					CObjeto Datos = new CObjeto();
					CPais cPais = new CPais();
					cPais.IdPais = IdPais;
					cPais.Obtener(Conn);
					cPais.Pais = Pais;
					Error = ValidaPais(cPais);
					if (Error == "")
					{
						int ExisteNom = CPais.ValidaExisteEditar(IdPais, Pais, Conn);
						if (ExisteNom != 0)
						{
							Error = Error + "<li>Ya existe un país con el mismo Nombre.</li>";
						}
						else
						{
							cPais.Editar(Conn);
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


}