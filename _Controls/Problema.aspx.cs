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

public partial class _Controls_Problema : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

    [WebMethod]
    public static string ListarProblemas(int Pagina, string Columna, string Orden)
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
                SqlCommand Stored = new SqlCommand("spg_grdProblema", con);
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
                DataTable DataTablePermisos = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Permisos", Conn.ObtenerRegistrosDataTable(DataTablePermisos));           
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string DesactivarProblema(int IdProblema, int Baja)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB Conn)
        {
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeManipularBajaProblema"))
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
                    CProblema cProblema = new CProblema();
                    cProblema.IdProblema = IdProblema;
                    cProblema.Baja = desactivar;
                    cProblema.Desactivar(Conn);
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
    public static string ObtenerFormaAgregarProblema()
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";
         
        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarProblema"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    string query = "EXEC sp_TipoProblema_Obtener @Opcion";
                    conn.DefinirQuery(query);
                    conn.AgregarParametros("@Opcion", 1);
                    CArreglo TipoProblemas = conn.ObtenerRegistros();
                    Respuesta.Add("TipoProblemas", TipoProblemas);

                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AgregarProblema(int IdTipoProblema, string Problema)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarProblema"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CProblema cProblema = new CProblema();
                    cProblema.IdTipoProblema = IdTipoProblema;
                    cProblema.Problema = Problema;
                    Error = ValidarProblema(cProblema);
                    if (Error == "")
                    {
                        CObjeto Valida = new CObjeto();
                        int IdPermiso = CProblema.ValidaExiste(IdTipoProblema,Problema, Conn);
                        if (IdPermiso != 0)
                        {
                            Error = Error + "<li>Ya existe este problema.</li>";
                        }
                        else
                        {
                            cProblema.Agregar(Conn);
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
    public static string ObtenerFormaEditarProblema(int IdProblema)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarProblema"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    string spProblema = "SELECT * FROM Problema WHERE IdProblema=@IdProblema ";
                    conn.DefinirQuery(spProblema);
                    conn.AgregarParametros("@IdProblema", IdProblema);
                    CObjeto oPermiso = conn.ObtenerRegistro();
                    Datos.Add("IdProblema", oPermiso.Get("IdProblema").ToString());
                    Datos.Add("IdTipoProblema", oPermiso.Get("IdTipoProblema").ToString());
                    Datos.Add("Problema", oPermiso.Get("Problema").ToString());
                    Respuesta.Add("Dato", Datos);

                    string query = "EXEC sp_TipoProblema_Obtener @Opcion";
                    conn.DefinirQuery(query);
                    conn.AgregarParametros("@Opcion", 1);
                    CArreglo TipoProblemas = conn.ObtenerRegistros();
                    Respuesta.Add("TipoProblemas", TipoProblemas);

                    
                }
                else { Error = Error + "<li>"+conn.Mensaje+"</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarProblema(int IdProblema, int IdTipoProblema, string Problema)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB Conn)
        {

            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarProblema"))
            {
                if (Conn.Conectado)
                {

                    CObjeto Datos = new CObjeto();
                    CProblema cProblema = new CProblema();
                    cProblema.IdProblema = IdProblema;
                    cProblema.Obtener(Conn);
                    cProblema.IdTipoProblema = IdTipoProblema;
                    cProblema.Problema = Problema;
                    Error = ValidarProblema(cProblema);
                    if (Error == "")
                    {
                        int ExisteNom = CProblema.ValidaExisteEditarProblema(IdProblema,IdTipoProblema, Problema, Conn);
                        if (ExisteNom != 0)
                        {
                            Error = Error + "<li>Ya existe un problema con el mismo Nombre.</li>";
                        }
                        else
                        {
                            cProblema.Editar(Conn);
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

    private static string ValidarProblema(CProblema Problema)
    {
        string Mensaje = "";
        Mensaje += (Problema.IdTipoProblema == 0) ? "<li>Favor de completar el campo tipo problema.</li>" : Mensaje;
        Mensaje += (Problema.Problema == "") ? "<li>Favor de completar el campo Nombre del problema.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
}