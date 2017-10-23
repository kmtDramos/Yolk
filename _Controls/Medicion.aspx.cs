using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;

public partial class _Controls_Medicion : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string ObtenerClientes()
	{
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "EXEC sp_Cliente_Obtener @Opcion, @pIdUsuario";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@Opcion", 2);
                conn.AgregarParametros("@pIdUsuario", IdUsuario);
                CArreglo Clientes = conn.ObtenerRegistros();

                Datos.Add("Clientes", Clientes);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
	}
    [WebMethod]
    public static string ObtenerPaises(int IdCliente)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
                string query = "EXEC SP_Pais_ObtenerPaisesPorCliente @IdCliente, @IdUsuario";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCliente", IdCliente);
                conn.AgregarParametros("@IdUsuario", IdUsuario);
                CArreglo Paises = conn.ObtenerRegistros();

                Datos.Add("Paises", Paises);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerEstados(int IdCliente, int IdPais)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);

                string query = "EXEC SP_Estado_ObtenerEstadosPorCliente @IdCliente, @IdPais, @IdUsuario";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCliente", IdCliente);
                conn.AgregarParametros("@IdPais", IdPais);
                conn.AgregarParametros("@IdUsuario", IdUsuario);
                CArreglo Estados = conn.ObtenerRegistros();

                Datos.Add("Estados", Estados);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerMunicipios(int IdCliente, int IdPais, int IdEstado)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);

                string query = "EXEC SP_Municipio_ObtenerMunicipiosPorCliente @IdCliente, @IdPais, @IdEstado, @IdUsuario";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCliente", IdCliente);
                conn.AgregarParametros("@IdPais", IdPais);
                conn.AgregarParametros("@IdEstado", IdEstado);
                conn.AgregarParametros("@IdUsuario", IdUsuario);
                CArreglo Municipios = conn.ObtenerRegistros();

                Datos.Add("Municipios", Municipios);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

	[WebMethod]
	public static string ObtenerSucursales(int IdCliente, int IdMunicipio)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn) {
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				CObjeto Datos = new CObjeto();
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);

				string query = "EXEC SP_Sucursal_ObtenerSucursalesPorCliente @IdCliente, @IdMunicipio, @IdUsuario";
				conn.DefinirQuery(query);
				conn.AgregarParametros("@IdCliente", IdCliente);
                conn.AgregarParametros("@IdMunicipio", IdMunicipio);
                conn.AgregarParametros("@IdUsuario", IdUsuario);
				CArreglo Sucursales = conn.ObtenerRegistros();

				Datos.Add("Sucursales", Sucursales);

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerTableros(int IdSucursal)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn) {
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				CObjeto Datos = new CObjeto();
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
                string query = "EXEC SP_Tablero_ObtenerFiltro @Opcion, @IdSucursal, @IdUsuario";
				conn.DefinirQuery(query);
                conn.AgregarParametros("@Opcion", 1);
                conn.AgregarParametros("@IdSucursal", IdSucursal);
                conn.AgregarParametros("@IdUsuario", IdUsuario);
				CArreglo Tableros = conn.ObtenerRegistros();

				Datos.Add("Tableros", Tableros);

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerCircuitos(int IdTablero)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn) {
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				CObjeto Datos = new CObjeto();
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);

                string query = "EXEC SP_Circuito_ObtenerFiltro @Opcion, @IdTablero, @IdUsuario";
				conn.DefinirQuery(query);
                conn.AgregarParametros("@Opcion", 1);
				conn.AgregarParametros("@IdTablero", IdTablero);
                conn.AgregarParametros("@IdUsuario", IdUsuario);
				CArreglo Circuitos = conn.ObtenerRegistros();

				Datos.Add("Circuitos", Circuitos);

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
    public static string ObtenerDetalle(int Pagina, string Columna, string Orden,int IdCliente, int IdPais, int IdEstado, int IdMunicipio, int IdSucursal, int IdTablero, int IdCircuito, string FechaInicio, string FechaFin)
	{
        //var serializer = new JavaScriptSerializer();
        //serializer.MaxJsonLength = 500000000;

        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 10;

                CDB ConexionBaseDatos = new CDB();
                SqlConnection conexion = ConexionBaseDatos.conStr();
                SqlCommand Stored = new SqlCommand("spr_Reporte_Medicion4", conexion);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("IdCliente", SqlDbType.Int).Value = IdCliente;
                Stored.Parameters.Add("IdPais", SqlDbType.Int).Value = IdPais;
                Stored.Parameters.Add("IdEstado", SqlDbType.Int).Value = IdEstado;
                Stored.Parameters.Add("IdMunicipio", SqlDbType.Int).Value = IdMunicipio;
                Stored.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
                Stored.Parameters.Add("IdTablero", SqlDbType.Int).Value = IdTablero;
                Stored.Parameters.Add("IdCircuito", SqlDbType.Int).Value = IdCircuito;
                Stored.Parameters.Add("FechaInicio", SqlDbType.Text).Value = FechaInicio;
                Stored.Parameters.Add("FechaFin", SqlDbType.Text).Value = FechaFin;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);
                dataAdapterRegistros.SelectCommand.CommandTimeout = 1800;
                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);
                DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTableConsumos = ds.Tables[1];
                DataTable DataTableMetas = ds.Tables[2];
                DataTable DataTableReal = ds.Tables[3];


                Datos.Add("Paginador", conn.ObtenerRegistrosDataTable(DataTablePaginador)); 
                Datos.Add("Detalle", conn.ObtenerRegistrosDataTable(DataTableConsumos));
                Datos.Add("Consumo", conn.ObtenerRegistrosDataTable(DataTableMetas));                
                Datos.Add("Real", conn.ObtenerRegistrosDataTable(DataTableReal));

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });



        return Respuesta.ToString();
	}    
}