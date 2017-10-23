using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class CSecurity
{

	public CSecurity()
	{

	}

	public static int Login(string Usuario, string Password, CDB Conn)
	{
		int IdUsuario = 0;
		string Query = "SELECT IdUsuario FROM Usuario WHERE Usuario = @Usuario AND Password = @Password";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@Usuario", Usuario);
		Conn.AgregarParametros("@Password", Password);
		CObjeto Registro = Conn.ObtenerRegistro();
		if (Registro.Exist("IdUsuario"))
		{
			IdUsuario = (int)Registro.Get("IdUsuario");
			Acceder(IdUsuario, Conn);
		}
		return IdUsuario;
	}

	private static void Acceder(int IdUsuario, CDB Conn)
	{
		CUsuario Usuario = new CUsuario();
		Usuario.IdUsuario = IdUsuario;
		Usuario.Obtener(Conn);
		if (Usuario.IdUsuario != 0)
		{
			Usuario.FechaUltimoAcceso = DateTime.Now;
			Usuario.IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			Usuario.Cookie = CMD5.Encriptar(Usuario.IP + Usuario.FechaUltimoAcceso.ToShortDateString() + Usuario.IdUsuario.ToString());
			Usuario.Editar(Conn);

			HttpContext.Current.Response.Cookies[CMD5.Encriptar("KeepUnitUserCookie")].Value = Usuario.Cookie;
			//CMail.EnviarCorreo("keepunit@keepmoving.com.mx", Usuario.Correo, "Inicio de sesión", "Se ha iniciado sesión desde: "+ Usuario.IP);
		}

	}

	public static bool HaySesion(CDB Conn)
	{
		bool sesion = false;
		HttpCookie Dato = HttpContext.Current.Request.Cookies[CMD5.Encriptar("KeepUnitUserCookie")];
		if (Dato != null)
		{
			string Cookie = Dato.Value;
			string Query = "SELECT IdUsuario FROM Usuario WHERE Cookie = @Cookie";
			Conn.DefinirQuery(Query);
			Conn.AgregarParametros("@Cookie", Cookie);
			CObjeto Registro = Conn.ObtenerRegistro();
			CUsuario Usuario = new CUsuario();
			Usuario.IdUsuario = (Registro.Exist("IdUsuario")) ? (int)Registro.Get("IdUsuario") : 0;
			Usuario.Obtener(Conn);
			sesion = (Usuario.IdUsuario != 0);
		}
		return sesion;
	}

    public bool tienePermiso(string accion)
    {
        bool permiso = false;

        CUnit.Firmado(delegate(CDB Conn)
        {
            CDB conn = new CDB();

            int IdPerfil = 0;
            HttpCookie Dato = HttpContext.Current.Request.Cookies[CMD5.Encriptar("KeepUnitUserCookie")];
            if (Dato != null)
            {
                string Cookie = Dato.Value;

                string Query = "SELECT IdUsuario, IdPerfil FROM Usuario WHERE Cookie = @Cookie";
                Conn.DefinirQuery(Query);
                Conn.AgregarParametros("@Cookie", Cookie);
                CObjeto Registro = Conn.ObtenerRegistro();

                CUsuario Usuario = new CUsuario();
                Usuario.IdUsuario = (Registro.Exist("IdUsuario")) ? (int)Registro.Get("IdUsuario") : 0;
                Usuario.Obtener(Conn);
                IdPerfil = Usuario.IdPerfil;
            }


            //Obtengo los permisos del usuario dependiendo su perfil
            string query = "EXEC sp_PerfilPermiso_Consulta @Opcion, @IdPerfil, @IdPagina";
            conn.DefinirQuery(query);
            conn.AgregarParametros("@Opcion", 1);
            conn.AgregarParametros("@IdPerfil", IdPerfil);
            conn.AgregarParametros("@IdPagina", 0);
            SqlDataReader Datos = conn.Ejecutar();

            if (Datos.HasRows)
            {
                while (Datos.Read())
                {
                    //Valido si existe el permiso seleccionado en su lista de permisos
                    if (accion == Datos["Comando"].ToString())
                    {
                        permiso = true;
                        break;
                    }
                }
            }
            Datos.Close();
        });
        

        return permiso;
    }

    public string CrearMenu(int IdPerfil)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";
        string menuCompleto = "";
        string esteMenu = "";

        bool visible = false;

        CUnit.Firmado(delegate(CDB conn)
        {
            if (conn.Conectado)
            {
                //OBTIENE LISTA DE MENU
                string spMenu = "EXEC SP_Menu_Consultar @Opcion";
                conn.DefinirQuery(spMenu);
                conn.AgregarParametros("@Opcion", 1);
                SqlDataReader ListaMenus = conn.Ejecutar();
                if (ListaMenus.HasRows)
                {
                    while (ListaMenus.Read())
                    {

                        int IdMenu=Convert.ToInt32(ListaMenus["IdMenu"].ToString());
                        string DescripcionMenu = ListaMenus["Menu"].ToString();
                        esteMenu = "<a class='dropdown-toggle' data-toggle='dropdown' href='#'>" + DescripcionMenu + "<span class='caret'></span></a>";

                        //OBTIENE LISTA PAGINAS POR MENU
                        string esteSubMenu = "";
                        int tieneSubMenu = 0;

                        string spPagina = "EXEC SP_Pagina_Consultar @Opcion, @IdMenu";
                        conn.DefinirQuery(spPagina);
                        conn.AgregarParametros("@Opcion", 1);
                        conn.AgregarParametros("@IdMenu", IdMenu);
                        SqlDataReader ListaPaginas = conn.Ejecutar();
                        if (ListaPaginas.HasRows)
                        {       
                            while (ListaPaginas.Read())
                            {                                
                                int IdPagina = Convert.ToInt32(ListaPaginas["IdPagina"].ToString());
                                string Pagina = ListaPaginas["Pagina"].ToString(); 
                                string DescripcionPagina = ListaPaginas["Descripcion"].ToString();
                                int IdPermiso = Convert.ToInt32(ListaPaginas["IdPermiso"].ToString());

                                if (tienePermisoPorID(IdPermiso, IdPerfil))
                                {
                                    tieneSubMenu = tieneSubMenu + 1;
                                    visible = true;                                    
                                    esteSubMenu = esteSubMenu + "<li><a href='" + Pagina + "'>" + DescripcionPagina + "</a></li>";
                                }
                            }
                            if (visible)
                            {
                                esteSubMenu = "<ul class='dropdown-menu'>" + esteSubMenu + " </ul>";
                            }

                        }
                        ListaPaginas.Close();
                        if (tieneSubMenu >= 1)
                        {
                            menuCompleto = menuCompleto + "<li class='dropdown'>" + esteMenu + esteSubMenu + "</li>";
                        }
                    }
                    
                }
                ListaMenus.Close();
            }

        });

        menuCompleto = "<ul class='nav navbar-nav'>" + menuCompleto + "</ul>";
        //menuCompleto = menuCompleto;
        
        return menuCompleto;
    }

    public bool tienePermisoPorID(int IdPermiso, int IdPerfil)
    {
        bool permiso = false;
        CUnit.Firmado(delegate(CDB conn)
        {
            if (conn.Conectado)
            {                
                //Verifico si el perfil tiene el permiso de acceso a la página
                string spPermiso = "EXEC sp_PerfilPermiso_Consultar @Opcion, @IdPermiso, @IdPerfil";
                conn.DefinirQuery(spPermiso);
                conn.AgregarParametros("@Opcion", 1);
                conn.AgregarParametros("@IdPermiso", IdPermiso);
                conn.AgregarParametros("@IdPerfil", IdPerfil);
                CObjeto Permiso = conn.ObtenerRegistro();

                if (Permiso.Exist("IdPermiso")) {
                    permiso = true;
                }
            }
        });

        return permiso;
    }
}