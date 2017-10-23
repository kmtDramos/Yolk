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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class _Controls_Pagina : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarPaginas(int Pagina, string Columna, string Orden, int pBaja)
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
                SqlCommand Stored = new SqlCommand("spg_grdPagina", con);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pBaja;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);

                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);

                DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTablePaginas = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Paginas", Conn.ObtenerRegistrosDataTable(DataTablePaginas));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string DesactivarPagina(int IdPagina, int Baja)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB Conn)
        {
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeManipularBajaPagina"))
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
                    CPagina cPagina = new CPagina();
                    cPagina.IdPagina = IdPagina;
                    cPagina.Baja = desactivar;
                    cPagina.Desactivar(Conn);
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
    public static string ObtenerFormaAgregarPagina()
    {
        JObject Respuesta = new JObject();
        string Error = "";

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarPagina"))
            {

                if (conn.Conectado)
                {
                    JObject Datos = new JObject();

                    Datos = CMenu.ObtenerJsonMenus(Datos);
                    Datos = CPermiso.ObtenerJsonPermisos(Datos);

                    Respuesta.Add(new JProperty("Dato", Datos));

                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarPagina(int IdPagina)
    {
        JObject Respuesta = new JObject();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarPagina"))
            {

                if (conn.Conectado)
                {
                    JObject Datos = new JObject();

                    string spPagina = "SELECT * FROM Pagina WHERE IdPagina=@IdPagina ";
                    conn.DefinirQuery(spPagina);
                    conn.AgregarParametros("@IdPagina", IdPagina);
                    CObjeto oPagina = conn.ObtenerRegistro();
                    Datos.Add("IdPagina", oPagina.Get("IdPagina").ToString());
                    Datos.Add("Pagina", oPagina.Get("Pagina").ToString());
                    Datos.Add("Descripcion", oPagina.Get("Descripcion").ToString());
                    Datos.Add("IdMenu", oPagina.Get("IdMenu").ToString());
                    Datos.Add("IdPermiso", oPagina.Get("IdPermiso").ToString());


                    Datos = CMenu.ObtenerJsonMenus(Datos);
                    Datos = CPermiso.ObtenerJsonPermisos(Datos);

                    Respuesta.Add(new JProperty("Dato", Datos));


                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarPagina(int IdPagina, string Descripcion, int IdMenu, int IdPermiso, string Pagina)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {

            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarPagina"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CPagina cPagina = new CPagina();
                    cPagina.IdPagina = IdPagina;
                    cPagina.Obtener(Conn);
                    cPagina.Pagina = Pagina;
                    cPagina.Descripcion = Descripcion;
                    cPagina.IdMenu = IdMenu;
                    cPagina.IdPermiso = IdPermiso;
                    Error = ValidaPagina(cPagina);
                    if (Error == "")
                    {
                        int ExisteNom = CPagina.ValidaExisteEditar(IdPagina, Pagina, Conn);
                        if (ExisteNom != 0)
                        {
                            Error = Error + "<li>Ya existe una página con el mismo Nombre.</li>";
                        }
                        else
                        {
                            cPagina.Editar(Conn);
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
    public static string AgregarPagina(string Pagina, string Descripcion, int IdMenu, int IdPermiso)
    {
        CObjeto Respuesta = new CObjeto();
        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarPagina"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CPagina cPagina = new CPagina();
                    cPagina.Pagina = Pagina;
                    cPagina.Descripcion = Descripcion;
                    cPagina.IdMenu = IdMenu;
                    cPagina.IdPermiso = IdPermiso;
                    cPagina.Baja = false;
                    Error = ValidaPagina(cPagina);
                    if (Error == "")
                    {
                        CObjeto Valida = new CObjeto();
                        int IdPagina = CPagina.ValidaExiste(Pagina, Conn);
                        if (IdPagina != 0)
                        {
                            Error = Error + "<li>La página ya existe.</li>";
                        }
                        else
                        {
                            cPagina.Agregar(Conn);
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

    [WebMethod]
    public static string Reordenar(Dictionary<string, object> pRequest)
    {

        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB conn)
        {
            //int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarPagina"))
            {
                //    CObjeto Datos = new CObjeto();
                if (conn.Conectado)
                {

                    foreach (Dictionary<string, object> pPagina in (Array)pRequest["pPagina"])
                    {
                        int pIdPagina = Convert.ToInt32(pPagina["IdPagina"]);
                        int pOrden = Convert.ToInt32(pPagina["Orden"]);

                        CPagina cPagina = new CPagina();
                        cPagina.IdPagina = pIdPagina;
                        cPagina.Obtener(conn);
                        cPagina.Orden = pOrden;
                        cPagina.Editar(conn);
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

    private static string ValidaPagina(CPagina IdPagina)
    {
        string Mensaje = "";

        Mensaje += (IdPagina.Pagina == "") ? "<li>Favor de completar el campo página.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
}