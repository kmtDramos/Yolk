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

public partial class _Controls_Circuito : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

    [WebMethod]
    public static string ListarCircuitos(int Pagina, string Columna, string Orden, int IdCliente, int IdPais, int IdEstado, int
        IdMunicipio, int IdSucursal, int IdMedidor, int IdTablero, string Circuito, string Descripcion)
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
                SqlCommand Stored = new SqlCommand("spg_grdCircuito", con);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = IdCliente;
                Stored.Parameters.Add("pIdPais", SqlDbType.Int).Value = IdPais;
                Stored.Parameters.Add("pIdEstado", SqlDbType.Int).Value = IdEstado;
                Stored.Parameters.Add("pIdMunicipio", SqlDbType.Int).Value = IdMunicipio;
                Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = IdSucursal;
                Stored.Parameters.Add("pIdMedidor", SqlDbType.Int).Value = IdMedidor;
                Stored.Parameters.Add("pIdTablero", SqlDbType.Int).Value = IdTablero;
                Stored.Parameters.Add("pCircuito", SqlDbType.Text).Value = Circuito;
                Stored.Parameters.Add("pDescripcionCircuito", SqlDbType.Text).Value = Descripcion;
                Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
                Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;                 
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);  
              
                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);

                DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTableCircuitos = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Circuitos", Conn.ObtenerRegistrosDataTable(DataTableCircuitos));           
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string DesactivarCircuito(int IdCircuito, int Baja)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB Conn)
        {
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeManipularBajaCircuito"))
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
                    CCircuito cCircuito = new CCircuito();
                    cCircuito.IdCircuito = IdCircuito;
                    cCircuito.Baja = desactivar;
                    cCircuito.Desactivar(Conn);
                    Respuesta.Add("Datos", Datos);
                }
                else { Error = Error + "<li>"+Conn.Mensaje+"</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }


            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }
    
    [WebMethod]
    public static string ObtenerFormaAgregarCircuito()
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";
         
        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarCircuito"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    string spCliente = "EXEC sp_Cliente_Obtener @Opcion, @pIdUsuario";
                    conn.DefinirQuery(spCliente);
                    conn.AgregarParametros("@Opcion", 1);
                    conn.AgregarParametros("@pIdUsuario", IdUsuario);
                    CArreglo Clientes = conn.ObtenerRegistros();
                    Respuesta.Add("Clientes", Clientes);
                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

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
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

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
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

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
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

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

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

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
    public static string ObtenerMedidores(int IdSucursal)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "EXEC SP_Medidor_ObtenerMedidores @Opcion, @IdSucursal, @IdUsuario";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@Opcion", 1);
                conn.AgregarParametros("@IdSucursal", IdSucursal);
                conn.AgregarParametros("@IdUsuario", IdUsuario);
                CArreglo Medidores = conn.ObtenerRegistros();

                Datos.Add("Medidores", Medidores);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTableros(int IdMedidor)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "EXEC SP_Medidor_ObtenerTableros @Opcion, @IdMedidor, @IdUsuario";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@Opcion", 1);
                conn.AgregarParametros("@IdMedidor", IdMedidor);
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
    public static string AgregarCircuito(int IdMedidor, int IdTablero, string NumeroCircuito, string Descripcion)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarCircuito"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CCircuito cCircuito = new CCircuito();
                    cCircuito.Circuito = NumeroCircuito;
                    cCircuito.IdTablero = IdTablero;
                    cCircuito.Descripcion = Descripcion;
                    cCircuito.Baja = false;
                    Error = ValidarCircuito(cCircuito);
                    if (Error == "") {
                        CObjeto Valida = new CObjeto();
                        int IdCircuito = CCircuito.ValidaExiste(IdMedidor, NumeroCircuito, Conn);
                        if (IdCircuito != 0) {
                            Error = Error + "<li>Ya existe este número de circuito en este medidor.</li>";
                        }
                        else {
                            cCircuito.Agregar(Conn);
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

    private static string ValidarCircuito(CCircuito Circuito)
    {
        string Mensaje = "";

        Mensaje += (Circuito.IdTablero == 0) ? "<li>Favor de completar el campo tablero.</li>" : Mensaje;
        Mensaje += (Circuito.Descripcion == "") ? "<li>Favor de completar el campo descripcion.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
	
	private static string ValidarMeta(CMeta Meta)
    {
        string Mensaje = "";

        //Mensaje += (Meta.IdMeta == 0) ? "<li>Favor de completar el campo meta.</li>" : Mensaje;
        Mensaje += (Meta.MetaKwH == 0) ? "<li>Favor de completar el campo meta kwh.</li>" : Mensaje;
        Mensaje += (Meta.MetaHorasUso == 0) ? "<li>Favor de completar el campo meta horas Uso.</li>" : Mensaje;
        Mensaje += (Meta.MetaConsumo == 0) ? "<li>Favor de completar el campo meta Consumo.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }

    private static string ValidarMetaAgregar(CMeta Meta)
    {
        string Mensaje = "";
        Mensaje += (Meta.MetaKwH == 0) ? "<li>Favor de completar el campo meta kwh.</li>" : Mensaje;
        Mensaje += (Meta.MetaHorasUso == 0) ? "<li>Favor de completar el campo meta horas Uso.</li>" : Mensaje;
        Mensaje += (Meta.MetaConsumo == 0) ? "<li>Favor de completar el campo meta Consumo.</li>" : Mensaje;
        Mensaje += (Meta.Mes == 0) ? "<li>Favor de completar el campo mes.</li>" : Mensaje;
        Mensaje += (Meta.Anio == 0) ? "<li>Favor de completar el campo año.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;
        return Mensaje;
    }

    [WebMethod]
    public static string ObtenerFormaEditarCircuito(int IdCircuito)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarCircuito"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    int IdMedidor = 0;
                    int IdSucursal = 0;
                    int IdCliente = 0;
                    int IdEstado = 0;
                    int IdMunicipio = 0;
                    int IdPais = 0;

                    string spCicuito = "SELECT * FROM Circuito WHERE IdCircuito=@IdCircuito ";
                    conn.DefinirQuery(spCicuito);
                    conn.AgregarParametros("@IdCircuito", IdCircuito);
                    CObjeto oCircuito = conn.ObtenerRegistro();
                    Datos.Add("IdCircuito", oCircuito.Get("IdCircuito").ToString());
                    Datos.Add("Circuito", oCircuito.Get("Circuito").ToString());
                    Datos.Add("Descripcion", oCircuito.Get("Descripcion").ToString());

                    Datos.Add("IdTablero", Convert.ToInt32(oCircuito.Get("IdTablero").ToString()));
                    //////////////////
                    string query = "SELECT C.IdCliente, S.IdSucursal, M.IdMedidor, S.IdMunicipio, E.IdEstado, E.IdPais FROM Cliente C LEFT JOIN Sucursal S ON C.IdCliente=S.IdCliente LEFT JOIN Medidor M ON M.IdSucursal=S.IdSucursal LEFT JOIN Tablero T ON T.IdMedidor=M.IdMedidor LEFT JOIN Municipio MP ON MP.IdMunicipio=S.IdCliente LEFT JOIN Estado E ON E.IdEstado=MP.IdEstado WHERE T.IdTablero=@IdTablero";
                    conn.DefinirQuery(query);
                    conn.AgregarParametros("@IdTablero", Convert.ToInt32(oCircuito.Get("IdTablero").ToString()));
                    CObjeto Validar = conn.ObtenerRegistro();
                    Datos.Add("IdCliente", Validar.Get("IdCliente").ToString());
                    Datos.Add("IdSucursal", Validar.Get("IdSucursal").ToString());
                    Datos.Add("IdMedidor", Validar.Get("IdMedidor").ToString());
                    Datos.Add("IdMunicipio", Validar.Get("IdMunicipio").ToString());
                    Datos.Add("IdEstado", Validar.Get("IdEstado").ToString());
                    Datos.Add("IdPais", Validar.Get("IdPais").ToString());
                    Respuesta.Add("Dato", Datos);

                    IdMedidor = Convert.ToInt32(Validar.Get("IdMedidor").ToString());
                    IdSucursal = Convert.ToInt32(Validar.Get("IdSucursal").ToString());
                    IdCliente = Convert.ToInt32(Validar.Get("IdCliente").ToString());
                    IdEstado = Convert.ToInt32(Validar.Get("IdEstado").ToString());
                    IdMunicipio = Convert.ToInt32(Validar.Get("IdMunicipio").ToString());
                    IdPais = Convert.ToInt32(Validar.Get("IdPais").ToString());

                    /////////////////////////////////
                    string spCliente = "SELECT IdCliente AS Valor, Cliente AS Etiqueta FROM Cliente WHERE Baja = 0";
                    conn.DefinirQuery(spCliente);
                    CArreglo Clientes = conn.ObtenerRegistros();
                    Respuesta.Add("Clientes", Clientes);

                    /////////////////////////
                    string spPais = "SELECT P.IdPais AS Valor, P.Pais AS Etiqueta FROM Pais P INNER JOIN (SELECT M.IdPais FROM Sucursal S LEFT JOIN Municipio M ON S.IdMunicipio=M.IdMunicipio WHERE IdCliente=" + IdCliente + " GROUP BY IdPais) S ON P.IdPais=S.IdPais WHERE P.Baja=0";
                    conn.DefinirQuery(spPais);
                    CArreglo Paises = conn.ObtenerRegistros();
                    Respuesta.Add("Paises", Paises);

                    /////////////////////////
                    string spEstado = "SELECT IdEstado AS Valor, Estado AS Etiqueta FROM Estado WHERE IdPais= " + IdPais + " AND Baja = 0";
                    conn.DefinirQuery(spEstado);
                    CArreglo Estados = conn.ObtenerRegistros();
                    Respuesta.Add("Estados", Estados);

                    /////////////////////////
                    string spMunicipio = "SELECT IdMunicipio AS Valor, Municipio AS Etiqueta FROM Municipio WHERE IdEstado= " + IdEstado + " AND Baja = 0";
                    conn.DefinirQuery(spMunicipio);
                    CArreglo Municipios = conn.ObtenerRegistros();
                    Respuesta.Add("Municipios", Municipios);

                    /////////////////////////
                    string spSucursal = "SELECT IdSucursal AS Valor, Sucursal AS Etiqueta FROM Sucursal WHERE IdCliente= " + IdCliente + " AND Baja = 0";
                    conn.DefinirQuery(spSucursal);
                    CArreglo Sucursales = conn.ObtenerRegistros();
                    Respuesta.Add("Sucursales", Sucursales);

                    ////////////////////////////////////////
                    string spMedidor = "SELECT IdMedidor AS Valor, Medidor AS Etiqueta FROM Medidor WHERE IdSucursal= " + IdSucursal + " AND Baja = 0";
                    conn.DefinirQuery(spMedidor);
                    CArreglo Medidores = conn.ObtenerRegistros();
                    Respuesta.Add("Medidores", Medidores);

                    ////////////////////////////////////////
                    string spTablero = "SELECT IdTablero AS Valor, Tablero AS Etiqueta FROM Tablero WHERE IdMedidor= " + IdMedidor + " AND Baja = 0";
                    conn.DefinirQuery(spTablero);
                    CArreglo Tableros = conn.ObtenerRegistros();
                    Respuesta.Add("Tableros", Tableros);
                }
                else { Error = Error + "<li>"+conn.Mensaje+"</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarCircuito(int IdCircuito, int IdTablero, string Circuito, string Descripcion, int IdMedidor)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {

            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarCircuito"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    CCircuito cCircuito = new CCircuito();
                    cCircuito.IdCircuito = IdCircuito;
                    cCircuito.Circuito = Circuito;
                    cCircuito.Descripcion = Descripcion;
                    cCircuito.IdTablero = IdTablero;
                    cCircuito.Baja = false;
                    Error = ValidarCircuito(cCircuito);
                    if (Error == "")
                    {
                        CObjeto Valida = new CObjeto();
                        int Existe = CCircuito.ValidaExisteEditar(IdCircuito, Circuito, IdTablero, IdMedidor, Conn);
                        if (Existe != 0)
                        {
                            Error = Error + "<li>Ya existe un circuito con este número en este medidor.</li>";
                        }
                        else
                        {
                            cCircuito.Editar(Conn);
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
    public static string ObtenerFormaLogo(int IdCircuito)
    {
        string[] separador = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('/');

        string URL = separador[separador.Length - 5] + '/' + separador[separador.Length - 4] + '/' + separador[separador.Length - 3] + '/' + separador[separador.Length - 2];

        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeAgregarLogo"))
			{
				if (conn.Conectado)
				{
					CObjeto Datos = new CObjeto();
					string query = "SELECT IdCircuito, Circuito, Descripcion,Imagen FROM Circuito WHERE IdCircuito=@IdCircuito";
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
					Circuito.Add("URL", (URL + "/Archivos/CircuitoImagen/" + Imagen));
					Datos.Add("Circuito", Circuito);
					
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
	public static string EditarCircuitoImagen(int IdCircuito, string Imagen)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB Conn)
		{
			string Error = Conn.Mensaje;
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeAgregarLogo"))
			{
				if (Conn.Conectado)
				{
					CObjeto Datos = new CObjeto();
					CCircuito cCircuito = new CCircuito();
					cCircuito.IdCircuito = IdCircuito;
					cCircuito.Obtener(Conn);
					cCircuito.Imagen = Imagen;
					Error = ValidarCircuitoImagen(cCircuito);
					if (Error == "")
					{
						cCircuito.EditarImagen(Conn);
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

    private static string ValidarCircuitoImagen(CCircuito Circuito)
    {
        string Mensaje = "";

        Mensaje += (Circuito.IdCircuito == 0) ? "<li>Favor de completar el campo del Circuito.</li>" : Mensaje;
        Mensaje += (Circuito.Imagen == "") ? "<li>Favor de subir imagen.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }

    [WebMethod]
    public static string ObtenerFormaListarMetas(int IdCircuito)
    {

        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeListarMetas"))
			{
				if (conn.Conectado)
				{
					CObjeto Datos = new CObjeto();

					string query = "SELECT IdCircuito AS Valor, Circuito as Numero, Descripcion AS Etiqueta FROM Circuito WHERE IdCircuito=@IdCircuito";
					conn.DefinirQuery(query);
					conn.AgregarParametros("@IdCircuito", IdCircuito);
					CObjeto Circuito = conn.ObtenerRegistro();
					Circuito.Add("IdCircuito", Convert.ToInt32(Circuito.Get("Valor")));
					Circuito.Add("Circuito", Convert.ToInt32(Circuito.Get("Numero")));
					Circuito.Add("Descripcion", Convert.ToString(Circuito.Get("Etiqueta")));
					Datos.Add("Circuito", Circuito);

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
    public static string ListarMetas(int Pagina, string Columna, string Orden, int Mes, string Anio, int IdCircuito)
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
                SqlCommand spMeta = new SqlCommand("spg_grdMeta", con);
                spMeta.CommandType = CommandType.StoredProcedure;
                spMeta.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                spMeta.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                spMeta.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 50).Value = Columna;
                spMeta.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                spMeta.Parameters.Add("Mes", SqlDbType.Int).Value = Mes;
                spMeta.Parameters.Add("Anio", SqlDbType.VarChar, 4).Value = Anio;
                spMeta.Parameters.Add("IdCircuito", SqlDbType.Int).Value = IdCircuito;
                spMeta.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;
                SqlDataAdapter daMeta = new SqlDataAdapter(spMeta);

                DataSet dsMeta = new DataSet();
                daMeta.Fill(dsMeta);

                DataTable DataTablePaginador = dsMeta.Tables[0];
                DataTable DataTableMetas = dsMeta.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Metas", Conn.ObtenerRegistrosDataTable(DataTableMetas));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }
	
	[WebMethod]
    public static string ObtenerFormaEditarMeta(int IdMeta)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
		CSecurity permiso = new CSecurity();
		if (permiso.tienePermiso("puedeAgregarMeta"))
		{
			if (conn.Conectado)
            {
				
                CObjeto Datos = new CObjeto();
                string spMeta = "SELECT * FROM Meta WHERE IdMeta=@IdMeta";
                conn.DefinirQuery(spMeta);
                conn.AgregarParametros("@IdMeta", IdMeta);
                CObjeto oMeta = conn.ObtenerRegistro();
                Datos.Add("IdCircuito", oMeta.Get("IdCircuito").ToString());
				Datos.Add("IdMeta", oMeta.Get("IdMeta").ToString());                
                Datos.Add("MetaKwH", oMeta.Get("MetaKwH").ToString());
                Datos.Add("MetaHorasUso", oMeta.Get("MetaHorasUso").ToString());
                Datos.Add("MetaConsumo", oMeta.Get("MetaConsumo").ToString());
				
				string spCircuito = "SELECT * FROM Circuito WHERE IdCircuito=@IdCircuito";
                conn.DefinirQuery(spCircuito);
                conn.AgregarParametros("@IdCircuito", Convert.ToInt32(oMeta.Get("IdCircuito").ToString()));
                CObjeto oCircuito = conn.ObtenerRegistro();
                Datos.Add("Circuito", oCircuito.Get("Circuito").ToString());
				Datos.Add("Descripcion", oCircuito.Get("Descripcion").ToString());
				
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
    public static string EditarMeta(int IdMeta, decimal MetaKwH, decimal MetaHorasUso, decimal MetaConsumo)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CMeta cMeta = new CMeta();
                cMeta.IdMeta = IdMeta;                
                cMeta.MetaKwH = MetaKwH;
                cMeta.MetaHorasUso = MetaHorasUso;
                cMeta.MetaConsumo = MetaConsumo;
				Error = ValidarMeta(cMeta);
                if (Error == "")
                {
                    if (IdMeta > 0)
                    {
                        cMeta.Editar(Conn);
                    }
                    else {
                        Error = "Hubo un problema al intentar leer el registro";
                    }
                }

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarMeta(int IdCircuito)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
		CSecurity permiso = new CSecurity();
		if (permiso.tienePermiso("puedeAgregarMeta"))
		{
			if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string spCliente = "SELECT IdCircuito, Circuito, Descripcion FROM Circuito WHERE IdCircuito = @IdCircuito ";
                conn.DefinirQuery(spCliente);
                conn.AgregarParametros("@IdCircuito", IdCircuito);
                CObjeto Circuito = conn.ObtenerRegistro();

                Circuito.Add("Valor", Convert.ToString(Circuito.Get("IdCircuito")));
                Circuito.Add("Numero", Convert.ToString(Circuito.Get("Circuito")));
                Circuito.Add("Etiqueta", Convert.ToString(Circuito.Get("Descripcion")));              
                Respuesta.Add("Dato", Circuito);
            }
				else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

			Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AgregarMeta(int IdCircuito, decimal MetaKwH, decimal MetaHorasUso, decimal MetaConsumo, int Mes, int Anio)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CMeta cMeta = new CMeta();
                cMeta.IdCircuito = IdCircuito;
                cMeta.MetaKwH = MetaKwH;
                cMeta.MetaHorasUso = MetaHorasUso;
                cMeta.MetaConsumo = MetaConsumo;
                cMeta.Mes = Mes;
                cMeta.Anio = Anio;
                Error = ValidarMetaAgregar(cMeta);
                if (Error == "")
                {
                    CObjeto Valida = new CObjeto();
                    int IdMeta = CMeta.ValidaExiste(IdCircuito, Mes, Anio, Conn);
                    if (IdMeta != 0)
                    {
                        Error = "Ya existe una meta para el circuito en el mes y año seleccionado";
                    }
                    else
                    {
                        cMeta.Agregar(Conn);
                    }
                }

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }


    [WebMethod]
    public static string ObtenerFiltroCliente()
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
                conn.AgregarParametros("@Opcion", 1);
                conn.AgregarParametros("@pIdUsuario", IdUsuario);
                CArreglo Clientes = conn.ObtenerRegistros();

                Datos.Add("Clientes", Clientes);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }
}