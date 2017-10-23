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

public partial class _Controls_TipoProblema : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

    [WebMethod]
    public static string ListarTipoProblemas(int Pagina, string Columna, string Orden)
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
                SqlCommand Stored = new SqlCommand("spg_grdTipoProblema", con);
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
    public static string DesactivarTipoProblema(int IdTipoProblema, int Baja)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB Conn)
        {
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeManipularBajaTipoProblema"))
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
                    CTipoProblema cTipoProblema = new CTipoProblema();
                    cTipoProblema.IdTipoProblema = IdTipoProblema;
                    cTipoProblema.Baja = desactivar;
                    cTipoProblema.Desactivar(Conn);
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
    public static string ObtenerFormaAgregarTipoProblema()
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";
         
        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarTipoProblema"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    string spPermiso = "EXEC sp_Permiso_Obtener @Opcion";
                    conn.DefinirQuery(spPermiso);
                    conn.AgregarParametros("@Opcion", 1);
                    CArreglo Permisos = conn.ObtenerRegistros();
                    Respuesta.Add("Permisos", Permisos);
                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AgregarTipoProblema(string NombreTipoProblema)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarTipoProblema"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CTipoProblema cTipoProblema = new CTipoProblema();
                    cTipoProblema.NombreTipoProblema = NombreTipoProblema;
                    Error = ValidarTipoProblema(cTipoProblema);
                    if (Error == "")
                    {
                        CObjeto Valida = new CObjeto();
                        int IdTipoProblema = CTipoProblema.ValidaExiste(NombreTipoProblema, Conn);
                        if (IdTipoProblema != 0)
                        {
                            Error = Error + "<li>Ya existe este tipo de tipo de problema.</li>";
                        }
                        else
                        {
                            cTipoProblema.Agregar(Conn);
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
    public static string ObtenerFormaEditarTipoProblema(int IdTipoProblema)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarTipoProblema"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    string spPermiso = "SELECT * FROM TipoProblema WHERE IdTipoProblema=@IdTipoProblema ";
                    conn.DefinirQuery(spPermiso);
                    conn.AgregarParametros("@IdTipoProblema", IdTipoProblema);
                    CObjeto oPermiso = conn.ObtenerRegistro();
                    Datos.Add("IdTipoProblema", oPermiso.Get("IdTipoProblema").ToString());
                    Datos.Add("TipoProblema", oPermiso.Get("TipoProblema").ToString());
                    Respuesta.Add("Dato", Datos);

                    
                }
                else { Error = Error + "<li>"+conn.Mensaje+"</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarTipoProblema(int IdTipoProblema, string NombreTipoProblema)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB Conn)
        {

            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarTipoProblema"))
            {
                if (Conn.Conectado)
                {

                    CObjeto Datos = new CObjeto();
                    CTipoProblema cTipoProblema = new CTipoProblema();
                    cTipoProblema.IdTipoProblema = IdTipoProblema;
                    cTipoProblema.NombreTipoProblema = NombreTipoProblema;
                    Error = ValidarTipoProblema(cTipoProblema);
                    if (Error == "")
                    {
                        int ExisteNom = CTipoProblema.ValidaExisteEditarTipoProblema(IdTipoProblema, NombreTipoProblema, Conn);
                        if (ExisteNom != 0)
                        {
                            Error = Error + "<li>Ya existe un  tipo de problema con el mismo Nombre.</li>";
                        }
                        else
                        {
                            cTipoProblema.Editar(Conn);
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

    private static string ValidarTipoProblema(CTipoProblema TipoProblema)
    {
        string Mensaje = "";
        Mensaje += (TipoProblema.NombreTipoProblema == "") ? "<li>Favor de completar el campo Nombre del tipo de problema.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
}