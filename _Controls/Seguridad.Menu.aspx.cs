using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public partial class _Controls_Seguridad_Menu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarMenus(int Pagina, string Columna, string Orden, int Baja)
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
                SqlCommand Stored = new SqlCommand("spg_grdMenu", con);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = Baja;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);

                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);

                DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTableMenus = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Menus", Conn.ObtenerRegistrosDataTable(DataTableMenus));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string DesactivarMenu(int IdMenu, int Baja)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB Conn)
        {
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeManipularBajaMenu"))
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
                    CMenu cMenu = new CMenu();
                    cMenu.IdMenu = IdMenu;
                    cMenu.Baja = desactivar;
                    cMenu.Desactivar(Conn);
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
    public static string ObtenerFormaAgregarMenu()
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarMenu"))
            {

                if (conn.Conectado)
                {
                    
                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AgregarMenu(string Menu)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarMenu"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CMenu cMenu = new CMenu();
                    cMenu.Menu = Menu;
                    Error = ValidarMenu(cMenu);
                    if (Error == "")
                    {
                        CObjeto Valida = new CObjeto();
                        int IdMenu = CMenu.ValidaExiste(Menu, Conn);
                        if (IdMenu != 0)
                        {
                            Error = Error + "<li>Ya existe este menu</li>";
                        }
                        else
                        {
                            cMenu.Agregar(Conn);
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
    public static string ObtenerFormaEditarMenu(int IdMenu)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarMenu"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    string spMenu = "SELECT * FROM Menu WHERE IdMenu=@IdMenu ";
                    conn.DefinirQuery(spMenu);
                    conn.AgregarParametros("@IdMenu", IdMenu);
                    CObjeto oMenu = conn.ObtenerRegistro();
                    Datos.Add("IdMenu", oMenu.Get("IdMenu").ToString());
                    Datos.Add("Menu", oMenu.Get("Menu").ToString());
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
    public static string EditarMenu(int IdMenu, string Menu)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {

            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarMenu"))
            {
                if (Conn.Conectado)
                {

                    CObjeto Datos = new CObjeto();
                    CMenu cMenu = new CMenu();
                    cMenu.IdMenu = IdMenu;
                    cMenu.Obtener(Conn);
                    cMenu.Menu = Menu;
                    Error = ValidarMenu(cMenu);
                    if (Error == "")
                    {
                        int ExisteNom = CMenu.ValidaExisteEditarMenu(IdMenu, Menu, Conn);
                        if (ExisteNom != 0)
                        {
                            Error = Error + "<li>Ya existe un  tipo de problema con el mismo Nombre.</li>";
                        }
                        else
                        {
                            cMenu.Editar(Conn);
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
    public static string Reordenar(Dictionary<string, object> pRequest)
    {

        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB conn)
        {
            //int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarMenu"))
            {
            //    CObjeto Datos = new CObjeto();
                if (conn.Conectado)
                {

                    foreach (Dictionary<string, object> pMenu in (Array)pRequest["pMenu"])
                    {
                        int pIdMenu = Convert.ToInt32(pMenu["IdMenu"]);
                        int pOrden = Convert.ToInt32(pMenu["Orden"]);

                        CMenu cMenu = new CMenu();
                        cMenu.IdMenu = pIdMenu;
                        cMenu.Obtener(conn);
                        cMenu.Orden = pOrden;
                        cMenu.Editar(conn);
                    }
            
                }
                else
                {
                    Error = Error + "<li>" + conn.Mensaje + "</li>";
                }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();

    }

    private static string ValidarMenu(CMenu Menu)
    {
        string Mensaje = "";
        Mensaje += (Menu.Menu == "") ? "<li>Favor de completar el campo Menu.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
}