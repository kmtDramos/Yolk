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

public partial class _Controls_Sucursal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarSucursales(int Pagina, string Columna, string Orden, int IdCliente)
    {
        CObjeto Respuesta = new CObjeto();
        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 10;
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
                CDB ConexionBaseDatos = new CDB();
                SqlConnection con = ConexionBaseDatos.conStr();
                SqlCommand Stored = new SqlCommand("spg_grdSucursal", con);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = IdCliente;
                Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
                Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);

                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);

                DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTableSucursales = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Sucursales", Conn.ObtenerRegistrosDataTable(DataTableSucursales));
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

        CUnit.Firmado(delegate(CDB conn)
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

        CUnit.Firmado(delegate(CDB conn)
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
    public static string ObtenerMunicipios(int IdEstado)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdMunicipio AS Valor, Municipio AS Etiqueta FROM Municipio WHERE IdEstado = @IdEstado AND Baja = 0";
                conn.DefinirQuery(query);
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
    public static string AgregarSucursal(string Sucursal, int IdCliente, int IdMunicipio, int IdRegion)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		    CSecurity permiso = new CSecurity();
		    if (permiso.tienePermiso("puedeAgregarSucursal"))
		    {

			    if (Conn.Conectado)
                {
                    int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);

                    CObjeto Datos = new CObjeto();

                    CSucursal cSucursal = new CSucursal();
                    cSucursal.Sucursal = Sucursal;
                    cSucursal.IdCliente = IdCliente;
                    cSucursal.IdMunicipio = IdMunicipio;
                    cSucursal.IdRegion = IdRegion;
                    cSucursal.Baja = false;
                    Error = ValidarSucursal(cSucursal);
                    if (Error == "")
                    {
                        int contador = CSucursal.ValidaExiste(IdCliente, IdMunicipio, IdRegion, Sucursal, Conn);
                        if (contador == 0)
                        {
                            //Agregar Sucursal
                            cSucursal.Agregar(Conn);

                            //Agregar UsuarioSucursal
                            CUsuarioSucursal cUsuarioSucursal = new CUsuarioSucursal();
                            cUsuarioSucursal.IdUsuario = IdUsuario;
                            cUsuarioSucursal.IdSucursal = cSucursal.IdSucursal;
                            cUsuarioSucursal.Baja = false;
                            cUsuarioSucursal.Agregar(Conn);
                        }
                        else
                        {
                            Error = Error + "<li>Ya existe la sucursal.</li>";
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
    public static string DesactivarSucursal(int IdSucursal, int Baja)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                bool desactivar = false;
                if (Baja == 0)
                {
                    desactivar = true;
                }
                else
                {
                    desactivar = false;
                }
                CObjeto Datos = new CObjeto();

                CSucursal cSucursal = new CSucursal();
                cSucursal.IdSucursal = IdSucursal;
                cSucursal.Baja = desactivar;
                cSucursal.Desactivar(Conn);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }


    [WebMethod]
    public static string EditarSucursal(int IdSucursal, int IdMunicipio, int IdRegion, string Nombre)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CSucursal cSucursal = new CSucursal();
                cSucursal.IdSucursal = IdSucursal;
                cSucursal.Obtener(Conn);
                cSucursal.Sucursal = Nombre;
                cSucursal.IdMunicipio = IdMunicipio;
                cSucursal.IdRegion = IdRegion;
                Error = ValidarSucursal(cSucursal);
                if (Error == "")
                {

                    int contador = CSucursal.ValidaExisteEditar(IdSucursal, cSucursal.IdCliente, IdMunicipio, IdRegion, Nombre, Conn);
                    if (contador == 0)
                    {
                        cSucursal.Editar(Conn);
                    }
                    else
                    {
                        Error = Error + "<li>Ya existe esta sucursal.</li>";
                    }
                }

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    private static string ValidarSucursal(CSucursal Sucursal)
    {
        string Mensaje = "";

        Mensaje += (Sucursal.Sucursal == "") ? "<li>Favor de completar el campo Sucursal.</li>" : Mensaje;
        Mensaje += (Sucursal.IdCliente == 0) ? "<li>Favor de completar el campo Cliente.</li>" : Mensaje;
        Mensaje += (Sucursal.IdMunicipio == 0) ? "<li>Favor de completar el campo Municipio.</li>" : Mensaje;
        Mensaje += (Sucursal.IdRegion == 0) ? "<li>Favor de completar el campo Región.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }

    
}