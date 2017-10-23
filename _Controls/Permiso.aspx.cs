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

public partial class _Controls_Permiso : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

    [WebMethod]
    public static string ListarPermisos(int Pagina, string Columna, string Orden)
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
                SqlCommand Stored = new SqlCommand("spg_grdPermiso", con);
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
    public static string DesactivarPermiso(int IdPermiso, int Baja)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB Conn)
        {
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeManipularBajaPermiso"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    int desactivar = 0;
                    if (Baja == 0)
                    {
                        desactivar = 1;
                    }
                    else
                    {
                        desactivar = 0;
                    }                    
                    CPermiso cPermiso = new CPermiso();
                    cPermiso.IdPermiso = IdPermiso;
                    cPermiso.Baja = desactivar;
                    cPermiso.Desactivar(Conn);
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
    public static string ObtenerFormaAgregarPermiso()
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";
         
        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarPermiso"))
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
    public static string AgregarPermiso(string NombrePermiso, string Comando, string Pantalla)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarPermiso"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CPermiso cPermiso = new CPermiso();
                    cPermiso.Permiso = NombrePermiso;
                    cPermiso.Comando = Comando;
                    cPermiso.Pantalla = Pantalla;
                    Error = ValidarPermiso(cPermiso);
                    if (Error == "")
                    {
                        CObjeto Valida = new CObjeto();
                        int IdPermiso = CPermiso.ValidaExiste(NombrePermiso, Comando,Pantalla, Conn);
                        if (IdPermiso != 0)
                        {
                            Error = Error + "<li>Ya existe este permiso ó este comando.</li>";
                        }
                        else
                        {
                            cPermiso.Agregar(Conn);
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
    public static string ObtenerFormaEditarPermiso(int IdPermiso)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarPermiso"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    string spPermiso = "SELECT * FROM Permiso WHERE IdPermiso=@IdPermiso ";
                    conn.DefinirQuery(spPermiso);
                    conn.AgregarParametros("@IdPermiso", IdPermiso);
                    CObjeto oPermiso = conn.ObtenerRegistro();
                    Datos.Add("IdPermiso", oPermiso.Get("IdPermiso").ToString());
                    Datos.Add("Permiso", oPermiso.Get("Permiso").ToString());
                    Datos.Add("Comando", oPermiso.Get("Comando").ToString());
                    Datos.Add("Pantalla", oPermiso.Get("Pantalla").ToString());
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
    public static string EditarPermiso(int IdPermiso, string NombrePermiso, string Comando, string Pantalla)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {

            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarPermiso"))
            {
                if (Conn.Conectado)
                {

                    CObjeto Datos = new CObjeto();
                    CPermiso cPermiso = new CPermiso();
                    cPermiso.IdPermiso = IdPermiso;
                    cPermiso.Permiso = NombrePermiso;
                    cPermiso.Comando = Comando;
                    cPermiso.Pantalla = Pantalla;
                    Error = ValidarPermiso(cPermiso);
                    if (Error == "")
                    {
                        CObjeto Valida = new CObjeto();
                        int Existe = CPermiso.ValidaExisteEditarComando(IdPermiso, NombrePermiso, Comando, Pantalla, Conn);
                        if (Existe != 0)
                        {
                            Error = Error + "<li>Ya existe un permiso con el mismo comando.</li>";
                        }
                        else
                        {
                            Error = ValidarPermiso(cPermiso);
                            if (Error == "")
                            {
                                int ExisteNom = CPermiso.ValidaExisteEditarNombrePermiso(IdPermiso, NombrePermiso, Comando, Pantalla, Conn);
                                if (ExisteNom != 0)
                                {
                                    Error = Error + "<li>Ya existe un permiso con el mismo Nombre.</li>";
                                }
                                else
                                {
                                    cPermiso.Editar(Conn);
                                }
                            }
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

    private static string ValidarPermiso(CPermiso Permiso)
    {
        string Mensaje = "";

        Mensaje += (Permiso.Permiso == "") ? "<li>Favor de completar el campo Permiso.</li>" : Mensaje;
        Mensaje += (Permiso.Comando == "") ? "<li>Favor de completar el campo Comando.</li>" : Mensaje;
        Mensaje += (Permiso.Pantalla == "") ? "<li>Favor de completar el campo Pantalla.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
}