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


public partial class _Controls_ReporteFueraDeRango : System.Web.UI.Page
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
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdCliente AS Valor, Cliente AS Etiqueta FROM Cliente WHERE Baja = 0";
                conn.DefinirQuery(query);
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

                string query = "EXEC SP_Pais_ObtenerPaisesPorCliente @IdCliente";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCliente", IdCliente);
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

                string query = "EXEC SP_Estado_ObtenerEstadosPorCliente @IdCliente, @IdPais";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCliente", IdCliente);
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
    public static string ObtenerMunicipios(int IdCliente, int IdPais, int IdEstado)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "EXEC SP_Municipio_ObtenerMunicipiosPorCliente @IdCliente, @IdPais, @IdEstado";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCliente", IdCliente);
                conn.AgregarParametros("@IdPais", IdPais);
                conn.AgregarParametros("@IdEstado", IdEstado);
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

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "EXEC SP_Sucursal_ObtenerSucursalesPorCliente @IdCliente, @IdMunicipio";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCliente", IdCliente);
                conn.AgregarParametros("@IdMunicipio", IdMunicipio);
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

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdTablero AS Valor, Tablero AS Etiqueta FROM Tablero WHERE IdSucursal = @IdSucursal AND Baja = 0";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdSucursal", IdSucursal);
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

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdCircuito AS Valor, (Circuito +' ' + Descripcion) AS Etiqueta FROM Circuito WHERE IdTablero = @IdTablero AND Baja = 0 ";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdTablero", IdTablero);
                CArreglo Circuitos = conn.ObtenerRegistros();

                Datos.Add("Circuitos", Circuitos);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    
    [WebMethod]
    public static string ObtenerDetalleUno(int IdCliente, int IdPais, int IdEstado, int IdMunicipio, int IdSucursal, int IdTablero, int IdCircuito, int IdRango, string FechaInicio, string FechaFin)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CDB ConexionBaseDatos = new CDB();
                SqlConnection conexion = ConexionBaseDatos.conStr();
                SqlCommand Stored = new SqlCommand("spr_Reporte_EstatusPorCircuito", conexion);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("IdCliente", SqlDbType.Int).Value = IdCliente;
                Stored.Parameters.Add("IdPais", SqlDbType.Int).Value = IdPais;
                Stored.Parameters.Add("IdEstado", SqlDbType.Int).Value = IdEstado;
                Stored.Parameters.Add("IdMunicipio", SqlDbType.Int).Value = IdMunicipio;
                Stored.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
                Stored.Parameters.Add("IdTablero", SqlDbType.Int).Value = IdTablero;
                Stored.Parameters.Add("IdCircuito", SqlDbType.Int).Value = IdCircuito;
                Stored.Parameters.Add("IdRango", SqlDbType.Text).Value = Convert.ToString(IdRango);
                Stored.Parameters.Add("FechaInicio", SqlDbType.Text).Value = FechaInicio;
                Stored.Parameters.Add("FechaFin", SqlDbType.Text).Value = FechaFin;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);
                dataAdapterRegistros.SelectCommand.CommandTimeout = 1800;
                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);
                DataTable DataTableRegistros = ds.Tables[0];
                DataTable DataTableContadores = ds.Tables[1];

                Datos.Add("Detalle", conn.ObtenerRegistrosDataTable(DataTableRegistros));
                Datos.Add("Contadores", conn.ObtenerRegistrosDataTable(DataTableContadores));

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });



        return Respuesta.ToString();
    }  

    [WebMethod]
    public static string ObtenerDetalleConsumo(int IdCliente, int IdPais, int IdEstado, int IdMunicipio, int IdSucursal, int IdTablero, int IdCircuito, int IdRango, int IdTipoConsumo, string FechaInicio, string FechaFin)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                //string query = "SELECT * FROM dbo.fn_Reporte_DetalleRangos(@IdCliente, @IdPais, @IdEstado, @IdMunicipio, @IdSucursal, @IdTablero, @IdCircuito,@IdRango, @IdTipoConsumo, @FechaInicio, @FechaFin)";
                string query = "EXEC spr_Reporte_EstatusPorCircuito_Detalle @IdCliente, @IdPais, @IdEstado, @IdMunicipio, @IdSucursal, @IdTablero, @IdCircuito,@IdRango, @IdTipoConsumo, @FechaInicio, @FechaFin";
                conn.DefinirQuery(query);
                conn.AgregarParametros("IdCliente", IdCliente);
                conn.AgregarParametros("IdPais", IdPais);
                conn.AgregarParametros("IdEstado", IdEstado);
                conn.AgregarParametros("IdMunicipio", IdMunicipio);
                conn.AgregarParametros("IdSucursal", IdSucursal);
                conn.AgregarParametros("IdTablero", IdTablero);
                conn.AgregarParametros("IdCircuito", IdCircuito);
                conn.AgregarParametros("IdRango", Convert.ToString(IdRango));
                conn.AgregarParametros("IdTipoConsumo", Convert.ToString(IdTipoConsumo));
                conn.AgregarParametros("FechaInicio", FechaInicio);
                conn.AgregarParametros("FechaFin", FechaFin);
                CArreglo Detalle = conn.ObtenerRegistros();

                Datos.Add("Detalle", Detalle);
                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTotales(int IdCliente, int IdSucursal, int IdTablero, int IdCircuito, int IdRango, string FechaInicio, string FechaFin)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT * FROM dbo.fn_Reporte_ObtenerTotales(@IdCliente, @IdSucursal, @IdTablero, @IdCircuito, @IdRango, @FechaInicio, @FechaFin)";
                conn.DefinirQuery(query);
                conn.AgregarParametros("IdCliente", IdCliente);
                conn.AgregarParametros("IdSucursal", IdSucursal);
                conn.AgregarParametros("IdTablero", IdTablero);
                conn.AgregarParametros("IdCircuito", IdCircuito);
                conn.AgregarParametros("IdRango", IdRango);
                conn.AgregarParametros("FechaInicio", FechaInicio);
                conn.AgregarParametros("FechaFin", FechaFin);

                CArreglo Detalle = conn.ObtenerRegistros();
                Datos.Add("Detalle", Detalle);
                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerModalMostrarCircuitoImagen(int IdCircuito)
    {
        string[] separador = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('/');

        string pagina = separador[separador.Length - 5] + '/' + separador[separador.Length - 4] + '/' + separador[separador.Length - 3] + '/' + separador[separador.Length - 2];

       CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdCircuito, Circuito, Descripcion, Imagen FROM Circuito WHERE IdCircuito=@IdCircuito";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCircuito", IdCircuito);
                CObjeto Circuito = conn.ObtenerRegistro();
                string Imagen = Convert.ToString(Circuito.Get("Imagen"));
                if (Imagen == "")
                {
                    Imagen = "NoImage.png";
                }

                Random rnd = new Random();
                var valor = rnd.Next(5000);
                Imagen = Imagen + "?r=" + valor;

                Circuito.Add("URL", (pagina + "/Archivos/CircuitoImagen/" + Imagen));
                Datos.Add("Circuito", Circuito);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }
}