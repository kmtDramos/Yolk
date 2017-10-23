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

public partial class _Controls_Usuario : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string Login(string Usuario, string Password)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Anonimo(delegate (CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                string estaPagina = "login.aspx";

                CObjeto Datos = new CObjeto();
                int IdUsuario = CSecurity.Login(Usuario, CMD5.Encriptar(Password), Conn);

                if (IdUsuario != 0)
                {

                    CUsuario UsuarioValida = new CUsuario();
                    if (UsuarioValida.TieneSucursalAsignada(IdUsuario, Conn))
                    {
                        string query = "EXEC SP_Perfil_ConsultarPorIdUsuario @Opcion, @IdUsuario";
                        Conn.DefinirQuery(query);
                        Conn.AgregarParametros("@Opcion", 1);
                        Conn.AgregarParametros("@IdUsuario", IdUsuario);
                        SqlDataReader Obten = Conn.Ejecutar();

                        if (Obten.HasRows)
                        {
                            if (Obten.Read())
                            {
                                estaPagina = Obten["Pagina"].ToString();
                            }
                            Datos.Add("Pagina", estaPagina);
                        }
                        else
                        {
                            Error = "Su perfil no tiene ninguna página de inicio configurada, favor de avisar al administrador.";
                        }
                        Obten.Close();
                    }
                    else
                    {
                        Error = "No tiene ninguna sucursal asignada, favor de avisar al administrador.";
                    }
                }
                else
                {
                    Error = "Usuario o contraseña incorrecto";
                }

                Respuesta.Add("Datos", Datos);
            }
            else
            {
                Error = Conn.Mensaje;
            }

            Respuesta.Add("Error", Error);

            Conn.Cerrar();
        });

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string CerrarSesion()
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB Conn)
		{
			string Error = Conn.Mensaje;
			if (Conn.Conectado)
			{
				string Cookie = "";
				Cookie = HttpContext.Current.Request.Cookies[CMD5.Encriptar("KeepUnitUserCookie")].Value;

				string Query = "SELECT * FROM Usuario WHERE Cookie = @Cookie";
				Conn.DefinirQuery(Query);
				Conn.AgregarParametros("@Cookie", Cookie);

				CObjeto Registro = Conn.ObtenerRegistro();

				int IdUsuario = (int)Registro.Get("IdUsuario");

				if (IdUsuario != 0)
				{
					HttpContext.Current.Response.Cookies[CMD5.Encriptar("KeepUnitUserCookie")].Expires = DateTime.Today.AddDays(-1);
				}

			}
			Respuesta.Add("Error", Error);

            Conn.Cerrar();
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ListarUsuarios(int Pagina, string Columna, string Orden)
	{
        CObjeto Respuesta = new CObjeto();
        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 30;
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
                CDB ConexionBaseDatos = new CDB();
                SqlConnection con = ConexionBaseDatos.conStr();
                SqlCommand Stored = new SqlCommand("spg_grdUsuario", con);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);

                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);

                DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTableUsuarios = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Usuarios", Conn.ObtenerRegistrosDataTable(DataTableUsuarios));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);

            Conn.Cerrar();
        });
        return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerProveedores(int IdPerfil)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				CObjeto Datos = new CObjeto();

				string query = "SP_Proveedor_ObtenerProveedor @IdPerfil";
				conn.DefinirQuery(query);
				conn.AgregarParametros("@IdPerfil", IdPerfil);
				//conn.AgregarParametros("@IdProveedor", IdProveedor);
				CArreglo Proveedores = conn.ObtenerRegistros();

				Datos.Add("Proveedores", Proveedores);

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);

			conn.Cerrar();
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string AgregarUsuario(string Nombre, string ApellidoPaterno, string ApellidoMaterno, string Usuario, string Password, string Correo, int IdPerfil, int IdSucursal, int JefeInmediato, bool esRespSuc)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB Conn)
		{
			string Error = Conn.Mensaje;
			CSecurity permiso = new CSecurity();
			if(permiso.tienePermiso("puedeAgregarUsuario"))
			{ 
			    if (Conn.Conectado)
			    {
				    CObjeto Datos = new CObjeto();

				    CUsuario cUsuario = new CUsuario();
				    cUsuario.Nombre = Nombre;
				    cUsuario.ApellidoPaterno = ApellidoPaterno;
				    cUsuario.ApellidoMaterno = ApellidoMaterno;
				    cUsuario.Usuario = Usuario;
				    cUsuario.Password = CMD5.Encriptar(Password);
				    cUsuario.Correo = Correo;
                    cUsuario.IdPerfil = IdPerfil;
                    cUsuario.IdSucursalPredeterminada = IdSucursal;
				    cUsuario.IdUsuarioJefe = JefeInmediato;
				    cUsuario.EsResponsableSucursal = esRespSuc;
				    cUsuario.Baja = false;
               
                    Error = ValidarUsuarioAgregar(cUsuario);
				    if (Error == "")
				    {
                        int existe = CUsuario.ValidaExiste(Usuario, Conn);
                        if (existe != 0)
                        {
                            Error = Error + "<li>El usuario ya existe dado de alta</li>";
                        }
                        else
                        {
                            cUsuario.Agregar(Conn);
                            int IdUsuario = cUsuario.IdUsuario;
                            CUsuarioSucursal CUsuarioSucursal = new CUsuarioSucursal();
                            CUsuarioSucursal.IdSucursal = IdSucursal;
                            CUsuarioSucursal.IdUsuario = IdUsuario;
                            CUsuarioSucursal.Baja = false;
                            CUsuarioSucursal.Agregar(Conn);
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
    public static string EditarUsuario(int IdUsuario, string Nombre, string ApellidoPaterno, string ApellidoMaterno, string Usuario, string Correo, int IdPerfil, bool EsReponsableSucursal, int IdProveedor, int IdUsuarioJefe)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB Conn)
		{
			string Error = Conn.Mensaje;
			CSecurity permiso = new CSecurity();
			if(permiso.tienePermiso("puedeEditarUsuario"))
			{ 
			if (Conn.Conectado)
			{
				CObjeto Datos = new CObjeto();

				CUsuario cUsuario = new CUsuario();
                cUsuario.IdUsuario = IdUsuario;
				cUsuario.Obtener(Conn);
				cUsuario.Nombre = Nombre;
				cUsuario.ApellidoPaterno = ApellidoPaterno;
				cUsuario.ApellidoMaterno = ApellidoMaterno;
				//cUsuario.Password = (Password == "") ? CMD5.Encriptar(Password) : cUsuario.Password;
				cUsuario.Correo = Correo;
                cUsuario.IdPerfil = IdPerfil;
                cUsuario.IdUsuarioJefe = IdUsuarioJefe;
                cUsuario.EsResponsableSucursal = EsReponsableSucursal;
				Error = ValidarUsuarioEditar(cUsuario);
				if (Error == "")
				{
					cUsuario.Editar(Conn);

                    if (IdPerfil == 4) { 

                        //Si existe edita, de lo contrario da de alta en UsuarioProveedor
                        CUsuarioProveedor UsuarioProveedor = new CUsuarioProveedor();
                        int IdUsuarioProveedor = CUsuarioProveedor.ValidaExiste(IdUsuario, Conn);
                        if (IdUsuarioProveedor > 0)
                        {
                            UsuarioProveedor.IdUsuarioProveedor = IdUsuarioProveedor;
                            UsuarioProveedor.Obtener(Conn);
                            UsuarioProveedor.IdUsuario = IdUsuario;
                            UsuarioProveedor.IdProveedor = IdProveedor;
                            UsuarioProveedor.Editar(Conn);
                        }
                        else
                        {
                            UsuarioProveedor.IdUsuario = IdUsuario;
                            UsuarioProveedor.IdProveedor = IdProveedor;
                            UsuarioProveedor.Agregar(Conn);
                        }
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

	private static string ValidarUsuarioAgregar(CUsuario Usuario)
	{
		string Mensaje = "";      
        Mensaje += (Usuario.Nombre == "") ? "<li>Favor de completar el campo de nombre.</li>" : Mensaje;
        Mensaje += (Usuario.Usuario == "") ? "<li>Favor de completar el campo de usuario.</li>" : Mensaje;
		Mensaje += (Usuario.ApellidoPaterno == "") ? "<li>Favor de completar el campo de apellido paterno.</li>" : Mensaje;
		Mensaje += (Usuario.Correo == "") ? "<li>Favor de completar el campo de correo.</li>" : Mensaje;
        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

		return Mensaje;
	}

    private static string ValidarUsuarioEditar(CUsuario Usuario)
    {
        string Mensaje = "";
        Mensaje += (Usuario.Nombre == "") ? "<li>Favor de completar el campo de nombre.</li>" : Mensaje;
        Mensaje += (Usuario.ApellidoPaterno == "") ? "<li>Favor de completar el campo de apellido paterno.</li>" : Mensaje;
        Mensaje += (Usuario.Correo == "") ? "<li>Favor de completar el campo de correo.</li>" : Mensaje;
        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }

	[WebMethod]
	public static string ObtenerUsuarioPermisos(int IdUsuario)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				CObjeto Datos = new CObjeto();

				string query = "SELECT P.*,ISNULL(UP.IdUsuarioPermiso,0) AS IdUsuarioPermiso,ISNULL(UP.IdUsuario,0) AS IdUsuario,ISNULL(UP.Estatus,0) AS Estatus " +
								"FROM Permiso P LEFT JOIN UsuarioPermiso UP ON P.IdPermiso = UP.IdPermiso AND UP.IdUsuario = @IdUsuario";
				conn.DefinirQuery(query);
				conn.AgregarParametros("@IdUsuario", IdUsuario);
				CArreglo Permisos = conn.ObtenerRegistros();

				Datos.Add("Permisos", Permisos);

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);

            conn.Cerrar();
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string GuardarUsuarioPermisos(Dictionary<string, object> Parametros)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				int IdUsuario = 0;
				Object[] IdPermisos;
				try
				{
					IdUsuario = Convert.ToInt32(Parametros["IdUsuario"]);
					IdPermisos = (Object[])Parametros["IdPermisos"];
				}
				catch (Exception e)
				{
					Error = e.Message + " - " + e.StackTrace;
				}
			}
			Respuesta.Add("Error", Error);

            conn.Cerrar();
		});

		return Respuesta.ToString();
	}

    [WebMethod]
    public static string MantenerSesion()
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaClienteAsignadoUsuario()
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarClientePredeterminado"))
            {
                if (conn.Conectado)
                {
                    int IdSucursalPredeterminada = 0;
                 
                    CObjeto Datos = new CObjeto();
                    string spCliente = "EXEC sp_Cliente_Obtener @Opcion, @pIdUsuario";
                    conn.DefinirQuery(spCliente);
                    conn.AgregarParametros("@Opcion", 2);
                    conn.AgregarParametros("@pIdUsuario", IdUsuario);
                    CArreglo Clientes = conn.ObtenerRegistros();

                    string spUsuario = "SELECT IdUsuario, IdSucursalPredeterminada FROM Usuario WHERE IdUsuario=" + IdUsuario;
                    conn.DefinirQuery(spUsuario);
                    CObjeto Usuario = conn.ObtenerRegistro();
                    IdSucursalPredeterminada = Convert.ToInt32(Usuario.Get("IdSucursalPredeterminada"));

                    string spSucursal = "SELECT IdSucursal, IdCliente,IdMunicipio FROM Sucursal WHERE IdSucursal=" + IdSucursalPredeterminada;
                    conn.DefinirQuery(spSucursal);
                    CObjeto Sucursal = conn.ObtenerRegistro();
                    int IdCliente = Convert.ToInt32(Sucursal.Get("IdCliente"));
                    int IdMunicipio = Convert.ToInt32(Sucursal.Get("IdMunicipio"));

                    string query = "SELECT IdMunicipio, IdEstado, IdPais FROM Municipio WHERE IdMunicipio=" + IdMunicipio + " AND Baja=0";
                    conn.DefinirQuery(query);
                    conn.AgregarParametros("@IdSucursal", IdSucursalPredeterminada);
                    CObjeto Municipio = conn.ObtenerRegistro();
                    int IdEstado = (int)Municipio.Get("IdEstado");
                    int IdPais = (int)Municipio.Get("IdPais");


                    string spPais = "EXEC SP_Pais_ObtenerPaisesPorCliente @IdCliente, @IdUsuario";
                    conn.DefinirQuery(spPais);
                    conn.AgregarParametros("@IdCliente", IdCliente);
                    conn.AgregarParametros("@IdUsuario", IdUsuario);
                    CArreglo Paises = conn.ObtenerRegistros();


                    string spEstado = "EXEC SP_Estado_ObtenerEstadosPorCliente @IdCliente, @IdPais, @IdUsuario";
                    conn.DefinirQuery(spEstado);
                    conn.AgregarParametros("@IdCliente", IdCliente);
                    conn.AgregarParametros("@IdPais", IdPais);
                    conn.AgregarParametros("@IdUsuario", IdUsuario);
                    CArreglo Estados = conn.ObtenerRegistros();

                    string spMunicipio = "EXEC SP_Municipio_ObtenerMunicipiosPorCliente @IdCliente, @IdPais, @IdEstado, @IdUsuario";
                    conn.DefinirQuery(spMunicipio);
                    conn.AgregarParametros("@IdCliente", IdCliente);
                    conn.AgregarParametros("@IdPais", IdPais);
                    conn.AgregarParametros("@IdEstado", IdEstado);
                    conn.AgregarParametros("@IdUsuario", IdUsuario);
                    CArreglo Municipios = conn.ObtenerRegistros();

                    string spSucursales = "EXEC SP_Sucursal_ObtenerSucursalesPorCliente @IdCliente, @IdMunicipio, @IdUsuario";
                    conn.DefinirQuery(spSucursales);
                    conn.AgregarParametros("@IdCliente", IdCliente);
                    conn.AgregarParametros("@IdMunicipio", IdMunicipio);
                    conn.AgregarParametros("@IdUsuario", IdUsuario);
                    CArreglo Sucursales = conn.ObtenerRegistros();


                    Respuesta.Add("IdClientePredeterminado", IdCliente);
                    Respuesta.Add("Clientes", Clientes);
                    Respuesta.Add("Sucursales", Sucursales);
                    Respuesta.Add("Municipios", Municipios);
                    Respuesta.Add("Estados", Estados);
                    Respuesta.Add("Paises", Paises);
                    Respuesta.Add("IdPaisPredeterminado", IdPais);
                    Respuesta.Add("IdEstadoPredeterminado", IdEstado);
                    Respuesta.Add("IdMunicipioPredeterminado", IdMunicipio);
                    Respuesta.Add("IdSucursalPredeterminada", IdSucursalPredeterminada);

                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarClientePredeterminado(int IdSucursalPredeterminada)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
                
                CObjeto Datos = new CObjeto();

                CUsuario cUsuario = new CUsuario();
                cUsuario.IdUsuario = IdUsuario;
                cUsuario.Obtener(Conn);
                cUsuario.IdSucursalPredeterminada = IdSucursalPredeterminada;

                if (IdSucursalPredeterminada != 0)
                {
                    CObjeto ValidaSucursalPredeterminada = new CObjeto();
                    int puede = CUsuario.ValidaSucursalPredeterminada(IdSucursalPredeterminada, IdUsuario, Conn);
                    if (puede == 0)
                    {
                        Error = Error + "<li>No tiene asignada esta sucursal</li>";
                    }
                    else
                    {
                        cUsuario.EditarSucursalPredeterminada(Conn);
                    }                    
                }

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
            Conn.Cerrar();
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarUsuario()
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            string Error = conn.Mensaje;
		CSecurity permiso = new CSecurity();
		if (permiso.tienePermiso("puedeAgregarUsuario"))
		{
			if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                string spPerfil = "EXEC sp_Perfil_Consultar @Opcion";
                conn.DefinirQuery(spPerfil);
                conn.AgregarParametros("@Opcion", 1);
                CArreglo Perfiles = conn.ObtenerRegistros();
                Respuesta.Add("Perfiles", Perfiles);
                Respuesta.Add("Dato", Perfiles);

                Respuesta.Add("IdPerfilPredeterminado", 3);

                string spCliente = "EXEC sp_Cliente_Obtener @Opcion, @pIdUsuario";
                conn.DefinirQuery(spCliente);
                conn.AgregarParametros("@Opcion", 3);
                conn.AgregarParametros("@pIdUsuario", IdUsuario);
                CArreglo Clientes = conn.ObtenerRegistros();
                Respuesta.Add("Clientes", Clientes);

				string spJefeInmediato = "EXEC sp_Obtener_JefeInmediato @pIdUsuario";
				conn.DefinirQuery(spJefeInmediato);
				conn.AgregarParametros("@pIdUsuario", IdUsuario);
				CArreglo JefeInmediato = conn.ObtenerRegistros();
				Respuesta.Add("JefeInmediato", JefeInmediato);


			}
				else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }
			Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFiltroCliente()
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB conn)
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

            conn.Cerrar();
        });

        return Respuesta.ToString();
    }

	[WebMethod]
	public static string ObtenerFiltroJefeInmediato()
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				CObjeto Datos = new CObjeto();

				string query = "EXEC sp_Obtener_JefeInmediato @pIdUsuario";
				conn.DefinirQuery(query);
				conn.AgregarParametros("@pIdUsuario", IdUsuario);
				CArreglo JefeInmediato = conn.ObtenerRegistros();

				Datos.Add("JefeInmediato", JefeInmediato);

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);

			conn.Cerrar();
		});

		return Respuesta.ToString();
	}

	[WebMethod]
    public static string ObtenerPaises(int IdCliente)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB conn)
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

            conn.Cerrar();
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerEstados(int IdCliente, int IdPais)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB conn)
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

                conn.Cerrar();
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerMunicipios(int IdCliente, int IdPais, int IdEstado)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB conn)
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

                conn.Cerrar();
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerSucursales(int IdCliente, int IdMunicipio)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB conn)
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

            conn.Cerrar();
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaListarUsuarioSucursales(int IdUsuario)
    {

        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
		//CSecurity permiso = new CSecurity();
		//if (permiso.tienePermiso("ListarUsuarioSucursales"))
		//{
			if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdUsuario FROM Usuario WHERE IdUsuario=@IdUsuario";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdUsuario", IdUsuario);
                CObjeto Usuario = conn.ObtenerRegistro();
                Datos.Add("IdUsuario", (int)Usuario.Get("IdUsuario"));
                Respuesta.Add("Datos", Datos);
            }
			//	else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			//}
			//else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }
			Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();
    }


    [WebMethod]
    public static string ListarSucursalesDisponibles(int Pagina, string Columna, string Orden, int IdUsuario)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;

            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 10;
                int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(Conn);
                CDB ConexionBaseDatos = new CDB();
                SqlConnection con = ConexionBaseDatos.conStr();
                SqlCommand spSucursalesDisponibles = new SqlCommand("spg_grdSucursalesDisponibles", con);
                spSucursalesDisponibles.CommandType = CommandType.StoredProcedure;
                spSucursalesDisponibles.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                spSucursalesDisponibles.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                spSucursalesDisponibles.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 50).Value = Columna;
                spSucursalesDisponibles.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                spSucursalesDisponibles.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
                spSucursalesDisponibles.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;
                SqlDataAdapter daSucursales = new SqlDataAdapter(spSucursalesDisponibles);

                DataSet dsSucursal = new DataSet();
                daSucursales.Fill(dsSucursal);

                DataTable DataTablePaginador = dsSucursal.Tables[0];
                DataTable DataTableSucursalesDisponibles = dsSucursal.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("SucursalesDisponibles", Conn.ObtenerRegistrosDataTable(DataTableSucursalesDisponibles));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);

            Conn.Cerrar();
        });
        return Respuesta.ToString();
    }
    [WebMethod]
    public static string ListarSucursalesAsignadas(int Pagina, string Columna, string Orden, int IdUsuario)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;

            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 10;
                int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(Conn);
                CDB ConexionBaseDatos = new CDB();
                SqlConnection con = ConexionBaseDatos.conStr();
                SqlCommand spSucursalesAsignadas = new SqlCommand("spg_grdSucursalesAsignadas", con);
                spSucursalesAsignadas.CommandType = CommandType.StoredProcedure;
                spSucursalesAsignadas.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                spSucursalesAsignadas.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                spSucursalesAsignadas.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 50).Value = Columna;
                spSucursalesAsignadas.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                spSucursalesAsignadas.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
                spSucursalesAsignadas.Parameters.Add("pBaja", SqlDbType.Int).Value = 0;
                SqlDataAdapter daSucursales = new SqlDataAdapter(spSucursalesAsignadas);

                DataSet dsSucursal = new DataSet();
                daSucursales.Fill(dsSucursal);

                DataTable DataTablePaginador = dsSucursal.Tables[0];
                DataTable DataTableSucursalesAsignadas = dsSucursal.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("SucursalesAsignadas", Conn.ObtenerRegistrosDataTable(DataTableSucursalesAsignadas));

                string Query = "SELECT ISNULL(IdSucursalPredeterminada,0) AS IdSucursalPredeterminada FROM Usuario WHERE IdUsuario = @IdUsuario";
                Conn.DefinirQuery(Query);
                Conn.AgregarParametros("@IdUsuario", IdUsuario);
                CObjeto Registro = Conn.ObtenerRegistro();
                int IdSucursalPredeterminada = (int)Registro.Get("IdSucursalPredeterminada");

                Respuesta.Add("IdSucursalPredeterminada", IdSucursalPredeterminada);
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);

            Conn.Cerrar();
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AsignarSucursales(List<int> IdSucursalDisponible, int IdUsuario)
    {

        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarSucursal"))
            {
                CObjeto Datos = new CObjeto();
                if (conn.Conectado)
                {
                    foreach (int IdSucursal in IdSucursalDisponible)
                    {

                        if (Error == "")
                        {
                            CUsuarioSucursal UsuarioSucursal = new CUsuarioSucursal();
                            UsuarioSucursal.IdUsuario = IdUsuario;
                            UsuarioSucursal.Baja = false;

                            int IdUsuarioSucursal = CUsuarioSucursal.ValidaExiste(IdUsuario, IdSucursal, conn);
                            if (IdUsuarioSucursal != 0)
                            {
                                UsuarioSucursal.IdUsuarioSucursal = IdUsuarioSucursal;
                                UsuarioSucursal.DesactivarEspecial(conn);
                            }
                            else
                            {
                                UsuarioSucursal.IdSucursal = IdSucursal;
                                UsuarioSucursal.Agregar(conn);
                            }
                        }
                    }
                    Respuesta.Add("Datos", Datos);
                }
                else
                {
                    Error = Error + "<li>" + conn.Mensaje + "</li>";
                }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();

    }


    [WebMethod]
    public static string DesasignarSucursales(List<int> pIdUsuarioSucursal, int IdUsuario)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarCircuito"))
            {
                CObjeto Datos = new CObjeto();
                if (conn.Conectado)
                {
                    foreach (int IdUsuarioSucursal in pIdUsuarioSucursal)
                    {
                        CUsuarioSucursal UsuarioSucursal = new CUsuarioSucursal();
                        UsuarioSucursal.IdUsuario = IdUsuario;
                        UsuarioSucursal.IdUsuarioSucursal = IdUsuarioSucursal;
                        UsuarioSucursal.Baja = true;

                        if (Error == "")
                        {
                            UsuarioSucursal.DesactivarEspecial(conn);
                        }
                    }
                    Respuesta.Add("Datos", Datos);
                }
                else
                {
                    Error = Error + "<li>" + conn.Mensaje + "</li>";
                }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();

    }

    [WebMethod]
    public static string SucursalPredeterminada(int IdUsuario)
    {
        CObjeto Respuesta = new CObjeto();
        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(Conn);

                //Entra si tiene sucursales asigandas
                CUsuario UsuarioValida = new CUsuario();
                if (UsuarioValida.TieneSucursalAsignada(IdUsuario, Conn))
                {
                    //Valida que dentro de las asignadas esté la predeterminada
                    CDB conn = new CDB();
                    string sp = "EXEC sp_UsuarioSucursal_Consultar @Opcion, @IdUsuario";
                    conn.DefinirQuery(sp);
                    conn.AgregarParametros("@Opcion", 1);
                    conn.AgregarParametros("@IdUsuario", IdUsuario);
                    CObjeto Registro = conn.ObtenerRegistro();

                    if ((int)Registro.Get("Coincide") == 0)
                    {
                        Error = "Favor de seleccionar una sucursal predeterminada.";
                    }
                }
                Respuesta.Add("Error", Error);
            }

            Conn.Cerrar();
        });

        return Respuesta.ToString();
    }


    [WebMethod]
    public static string EstablecerSucursalPredeterminada(int IdUsuario, int IdUsuarioSucursal)
    {
        CObjeto Respuesta = new CObjeto();
        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(Conn);

                CUsuarioSucursal UsuarioSucursal = new CUsuarioSucursal();
                UsuarioSucursal.IdUsuarioSucursal = IdUsuarioSucursal;
                UsuarioSucursal.Obtener(Conn);
                int IdUsuarioRegistro = UsuarioSucursal.IdUsuario;
                int IdSucursalRegistro = UsuarioSucursal.IdSucursal;

                //Si el IdUsuario del registro corresponde con el usuario que estamos editando entra
                if (IdUsuarioRegistro == IdUsuario)
                {
                    CUsuario cUsuario = new CUsuario();
                    cUsuario.IdUsuario = IdUsuario;
                    cUsuario.IdSucursalPredeterminada = IdSucursalRegistro;
                    cUsuario.EditarSucursalPredeterminada(Conn);
                }
                else
                {
                    Error = "Hubo un error.";
                }

                Respuesta.Add("Error", Error);
            }

            Conn.Cerrar();
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarContrasena(int IdUsuario)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarUsuario"))
            {
                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    string spUsuario = "SELECT Usuario FROM Usuario WHERE IdUsuario=@IdUsuario ";
                    conn.DefinirQuery(spUsuario);
                    conn.AgregarParametros("@IdUsuario", IdUsuario);
                    CObjeto oUsuario = conn.ObtenerRegistro();
                    Datos.Add("IdUsuario", IdUsuario);
                    Datos.Add("Usuario", oUsuario.Get("Usuario").ToString());
                    Respuesta.Add("Dato", Datos);
                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);

            conn.Cerrar();
        });
        return Respuesta.ToString();
    }


    [WebMethod]
    public static string EditarContrasena(int IdUsuario, string Password)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		    CSecurity permiso = new CSecurity();
		    if (permiso.tienePermiso("puedeEditarContrasena"))
		    {
			    if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    CUsuario cUsuario = new CUsuario();
                    cUsuario.IdUsuario = IdUsuario;
                    cUsuario.Obtener(Conn);
                    cUsuario.Password = CMD5.Encriptar(Password);
                    cUsuario.EditarContrasena(Conn);
                
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
    public static string DesactivarUsuario(int IdUsuario, int Baja)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeManipularBajaUsuario"))
            {
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

                    CUsuario cUsuario = new CUsuario();
                    cUsuario.IdUsuario = IdUsuario;
                    cUsuario.Baja = desactivar;
                    cUsuario.DesactivarUsuario(Conn);

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
    public static string ObtenerFormaEditarUsuario( int IdUsuario)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarUsuario"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    CUsuario cUsuario = new CUsuario();
                    cUsuario.IdUsuario = IdUsuario;
                    cUsuario.Obtener(conn);
                    Datos.Add("IdUsuario", cUsuario.IdUsuario);
                    Datos.Add("Nombre", cUsuario.Nombre);
                    Datos.Add("ApellidoPaterno", cUsuario.ApellidoPaterno);
                    Datos.Add("ApellidoMaterno", cUsuario.ApellidoMaterno);
                    Datos.Add("Usuario", cUsuario.Usuario);
                    Datos.Add("Correo", cUsuario.Correo);
                    Datos.Add("IdPerfil", Convert.ToString(cUsuario.IdPerfil));
                    Datos.Add("IdUsuarioJefe", Convert.ToString(cUsuario.IdUsuarioJefe));
                    Datos.Add("EsResponsableSucursal", Convert.ToString(cUsuario.EsResponsableSucursal));
                    Respuesta.Add("Dato", Datos);

                    int  IdProveedor=0;
                    string spIdProveedor = "EXEC sp_UsuarioProveedor_Consultar @Opcion, 0, @IdUsuario ";
                    conn.DefinirQuery(spIdProveedor);
                    conn.AgregarParametros("@Opcion", 2);
                    conn.AgregarParametros("@IdUsuario", IdUsuario);
                    CObjeto UsuarioProveedor = conn.ObtenerRegistro();
                    if (UsuarioProveedor.Exist("IdUsuarioProveedor"))
                    {
                        IdProveedor = Convert.ToInt32(UsuarioProveedor.Get("IdProveedor").ToString());
                    }
                    Datos.Add("IdProveedor", IdProveedor);

                    string spPerfil = "EXEC sp_Perfil_Consultar @Opcion ";
                    conn.DefinirQuery(spPerfil);
                    conn.AgregarParametros("@Opcion", 2);
                    CArreglo Perfiles = conn.ObtenerRegistros();
                    Respuesta.Add("Perfiles", Perfiles);

                    string spProveedor = "EXEC sp_Proveedor_Consultar @Opcion ";
                    conn.DefinirQuery(spProveedor);
                    conn.AgregarParametros("@Opcion", 1);
                    CArreglo Proveedores = conn.ObtenerRegistros();
                    Respuesta.Add("Proveedores", Proveedores);

                    string spJefe = "EXEC sp_Usuario_Consultar @Opcion ";
                    conn.DefinirQuery(spJefe);
                    conn.AgregarParametros("@Opcion", 2);
                    CArreglo Jefes = conn.ObtenerRegistros();
                    Respuesta.Add("Jefes", Jefes);
                    
                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }
}