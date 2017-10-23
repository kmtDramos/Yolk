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

public partial class _Controls_Cliente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarClientes(int Pagina, string Columna, string Orden)
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
                //CDB ConexionBaseDatos = new CDB();
                SqlConnection con = Conn.conStr();
                SqlCommand Stored = new SqlCommand("spg_grdCliente", con);
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
                DataTable DataTableClientes = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Clientes", Conn.ObtenerRegistrosDataTable(DataTableClientes));
                Respuesta.Add("Datos", Datos);
                con.Close();
            }

            Respuesta.Add("Error", Error);
            Conn.Cerrar();
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
            conn.Cerrar();
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
            conn.Cerrar();
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
            conn.Cerrar();
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AgregarCliente(string Cliente, int IdMunicipio)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
        string Error = Conn.Mensaje;
		CSecurity permiso = new CSecurity();
		if (permiso.tienePermiso("puedeAgregarCliente"))
		{
			if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                CCliente cCliente = new CCliente();                
                cCliente.IdMunicipio = IdMunicipio;
                cCliente.Cliente = Cliente;
                Error = ValidarCliente(cCliente);
                if (Error == "")
                {

                    int contador = CCliente.ValidaExiste(IdMunicipio, Cliente, Conn);
                    if (contador == 0)
                    {
                        cCliente.Agregar(Conn);                        
                    }
                    else
                    {
                        Error = Error + "<li>Ya existe este cliente en este municipio.</li>"; 
                    }                   
                }

                Respuesta.Add("Datos", Datos);
				}
				else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }
			Respuesta.Add("Error", Error);

            Conn.Cerrar();
        });

        return Respuesta.ToString();
    }

    private static string ValidarCliente(CCliente Cliente)
    {
        string Mensaje = "";

        Mensaje += (Cliente.Cliente == "") ? "<li>Favor de completar el campo cliente.</li>" : Mensaje;
        Mensaje += (Cliente.IdMunicipio == 0) ? "<li>Favor de completar el campo municipio.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }

    private static string ValidarLogo(CCliente Cliente)
    {
        string Mensaje = "";

        Mensaje += (Cliente.IdCliente == 0) ? "<li>Favor de completar el campo del cliente.</li>" : Mensaje;
        Mensaje += (Cliente.Logo == "") ? "<li>Favor de subir archivo.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }

    [WebMethod]
    public static string DesactivarCliente(int IdCliente, int Baja)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		    CSecurity permiso = new CSecurity();
		    if (permiso.tienePermiso("puedeManipularBajaCliente"))
		    {
			    if (Conn.Conectado)
                {
                    bool desactivar = false;
                    if (Baja == 0)
                    {
                        desactivar = true;
                    }
                    else {
                        desactivar = false;
                    }
                    CObjeto Datos = new CObjeto();

                    CCliente cCliente = new CCliente();
                    cCliente.IdCliente = IdCliente;
                    cCliente.Baja = desactivar;
                    cCliente.Desactivar(Conn);                

                    Respuesta.Add("Datos", Datos);
                }
				else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }
			Respuesta.Add("Error", Error);
            Conn.Cerrar();
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerPrueba()
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

                Respuesta.Add("Paises", Paises);
            }
            Respuesta.Add("Error", Error);
            conn.Cerrar();
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarCliente(int IdCliente, string Cliente, int IdMunicipio)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		    CSecurity permiso = new CSecurity();
		    if (permiso.tienePermiso("puedeEditarCliente"))
		    {
			    if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    CCliente cCliente = new CCliente();
                    cCliente.IdCliente = IdCliente;
                    cCliente.Cliente = Cliente;
                    cCliente.IdMunicipio = IdMunicipio;
                    Error = ValidarCliente(cCliente);
                    if (Error == "")
                    {
                        int contador = CCliente.ValidaExisteEditar(IdCliente, IdMunicipio, Cliente, Conn);
                        if (contador == 0)
                        {
                            cCliente.Editar(Conn);
                        }
                        else
                        {
                            Error = Error + "<li>Ya existe este cliente en este municipio.</li>";
                        } 

                    
                    }

                    Respuesta.Add("Datos", Datos);
                }
				else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			}
			else { 
                Error = Error + "<li>No tienes los permisos necesarios</li>"; 
            }
			Respuesta.Add("Error", Error);

            Conn.Cerrar();
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTipoTarifa()
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdTipoTarifa AS Valor, TipoTarifa AS Etiqueta FROM TipoTarifa WHERE Baja = 0";
                conn.DefinirQuery(query);
                CArreglo TipoTarifas = conn.ObtenerRegistros();

                Datos.Add("TipoTarifas", TipoTarifas);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTipoTension(int IdTipoTarifa)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdTipoTension AS Valor, TipoTension AS Etiqueta FROM TipoTension WHERE IdTipoTarifa = @IdTipoTarifa AND Baja = 0";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdTipoTarifa", IdTipoTarifa);
                CArreglo TipoTensiones = conn.ObtenerRegistros();

                Datos.Add("TipoTensiones", TipoTensiones);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTipoCuota(int IdTipoTension)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdTipoCuota AS Valor, TipoCuota AS Etiqueta FROM TipoCuota WHERE IdTipoTension=@IdTipoTension AND Baja = 0";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdTipoTension", IdTipoTension);
                CArreglo TipoCuotas = conn.ObtenerRegistros();

                Datos.Add("TipoCuotas", TipoCuotas);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerRegion(int IdTipoCuota)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdRegion AS Valor, Region AS Etiqueta FROM Region WHERE IdTipoCuota=@IdTipoCuota AND Baja = 0";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdTipoCuota", IdTipoCuota);
                CArreglo Regiones = conn.ObtenerRegistros();

                Datos.Add("Regiones", Regiones);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarLogo(int IdCliente, string Logo)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		    CSecurity permiso = new CSecurity();
		    if (permiso.tienePermiso("puedeAgregarLogoCliente"))
		    {
			    if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    CCliente cCliente = new CCliente();
                    cCliente.IdCliente = IdCliente;
                    cCliente.Obtener(Conn);
                    cCliente.Logo = Logo;
                    Error = ValidarLogo(cCliente);
                    if (Error == "")
                    {
                        cCliente.EditarLogo(Conn);
                    }

                    Respuesta.Add("Datos", Datos);
                }
				else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			}
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }
			Respuesta.Add("Error", Error);

            Conn.Cerrar();
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaLogo(int IdCliente)
    {
        string[] separador = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('/');

        string pagina = separador[separador.Length - 5] + '/'+separador[separador.Length - 4] + '/'+ separador[separador.Length - 3] + '/' +separador[separador.Length - 2];

        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
		    CSecurity permiso = new CSecurity();
		    if (permiso.tienePermiso("puedeAgregarLogoCliente"))
		    {
			    if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    string query = "SELECT IdCliente, Cliente, Logo FROM Cliente WHERE IdCliente=@IdCliente";
                    conn.DefinirQuery(query);
                    conn.AgregarParametros("@IdCliente", IdCliente);
                    CObjeto Cliente = conn.ObtenerRegistro();
                    string Logo = Convert.ToString(Cliente.Get("Logo"));
                    if (Logo == "") {
                        Logo = "NoImage.png";
                    }

                    Random rnd = new Random();
                    var valor = rnd.Next(5000);
                    //var num = new Random(DateTime.Now.Millisecond);
                    Logo = Logo + "?r=" + valor;

                    Cliente.Add("URL", (pagina+"/Archivos/Logo/"+ Logo));
                    Datos.Add("Cliente", Cliente);

                    Respuesta.Add("Datos", Datos);
				}
				else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }

			}

			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }
			Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();
    }
}