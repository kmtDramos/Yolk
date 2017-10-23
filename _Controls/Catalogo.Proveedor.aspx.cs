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
	public static string ListarProveedor(int Pagina, string Columna, string Orden)
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
				SqlCommand Stored = new SqlCommand("spg_grdPRoveedor", con);
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
				DataTable DataTableProveedor = ds.Tables[1];

				Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
				Datos.Add("Proveedores", Conn.ObtenerRegistrosDataTable(DataTableProveedor));
				Respuesta.Add("Datos", Datos);
			}

			Respuesta.Add("Error", Error);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string DesactivarProveedor(int IdProveedor, int Baja)
	{
		CObjeto Respuesta = new CObjeto();
		string Error = "";

		CUnit.Firmado(delegate (CDB Conn)
		{
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeManipularBajaProveedor"))
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
					CProveedor cProv = new CProveedor();
					cProv.IdProveedor = IdProveedor;
					cProv.Baja = desactivar;
					cProv.Desactivar(Conn);
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
	public static string ObtenerFormaAgregarProveedor()
	{
		CObjeto Respuesta = new CObjeto();
		string Error = "";

		CUnit.Firmado(delegate (CDB conn)
		{
			int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeAgregarProveedor"))
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
	public static string AgregarProveedor(string Proveedor)
	{
		CObjeto Respuesta = new CObjeto();
		CUnit.Firmado(delegate (CDB Conn)
		{
			string Error = "";
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeAgregarProveedor"))
			{
				if (Conn.Conectado)
				{
					CObjeto Datos = new CObjeto();
					CProveedor cProv= new CProveedor();
					cProv.Proveedor = Proveedor;
					cProv.Baja = false;
					Error = ValidaProveedor(cProv);
					if (Error == ""){
						
						int IdProveedor = CProveedor.ValidaExiste(Proveedor, Conn);
						if(IdProveedor != 0)
						{
							Error = Error + "<li>El proveedor ya existe.</li>";
						}
						else
						{
							cProv.Agregar(Conn);
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

	private static string ValidaProveedor(CProveedor IdProveedor)
	{
		string Mensaje = "";

		Mensaje += (IdProveedor.Proveedor == "") ? "<li>Favor de completar el campo proveedor.</li>" : Mensaje;

		Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

		return Mensaje;
	}

	[WebMethod]
	public static string ObtenerFormaEditarProveedor(int IdProveedor)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			string Error = "";
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeEditarProveedor"))
			{

				if (conn.Conectado)
				{
					CObjeto Datos = new CObjeto();

					string spProveedor = "SELECT * FROM Proveedor WHERE IdProveedor=@IdProveedor ";
					conn.DefinirQuery(spProveedor);
					conn.AgregarParametros("@IdProveedor", IdProveedor);
					CObjeto oProv = conn.ObtenerRegistro();
					Datos.Add("IdProveedor", oProv.Get("IdProveedor").ToString());
					Datos.Add("Proveedor", oProv.Get("Proveedor").ToString());
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
	public static string EditarProveedor(int IdProveedor, string Proveedor)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB Conn)
		{

			string Error = "";
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeEditarProveedor"))
			{
				if (Conn.Conectado)
				{
					CObjeto Datos = new CObjeto();
					CProveedor cProv = new CProveedor();
					cProv.IdProveedor = IdProveedor;
					cProv.Obtener(Conn);
					cProv.Proveedor = Proveedor;
					Error = ValidaProveedor(cProv);
					if (Error == "")
					{
						int ExisteNom = CProveedor.ValidaExisteEditar(IdProveedor, Proveedor, Conn);
						if (ExisteNom != 0)
						{
							Error = Error + "<li>Ya existe un proveedor con el mismo Nombre.</li>";
						}
						else
						{
							cProv.Editar(Conn);
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