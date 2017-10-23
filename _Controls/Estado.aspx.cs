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

public partial class _Controls_Estado : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

    [WebMethod]
    public static string ListarEstado(int Pagina, string Columna, string Orden)
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
                SqlCommand Stored = new SqlCommand("spg_grdEstado", con);
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
    public static string ObtenerPaises()
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB conn)
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
    public static string DesactivarEstado(int IdEstado, int Baja)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB Conn)
        {
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeManipularBajaEstado"))
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
                    CEstado cEstado = new CEstado();
                    cEstado.IdEstado = IdEstado;
                    cEstado.Baja = desactivar;
                    cEstado.Desactivar(Conn);
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
    public static string ObtenerFormaAgregarEstado()
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";
         
        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarEstado"))
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
    public static string AgregarEstado(int IdPais, string Estado)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarEstado"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CEstado cEstado = new CEstado();
                    cEstado.IdPais = IdPais;
                    cEstado.Estado = Estado;
                    Error = ValidarEstado(cEstado);
                    if (Error == "")
                    {
                        CObjeto Valida = new CObjeto();
                        int IdEstado = CEstado.ValidaExiste(IdPais, Estado, Conn);
                        if (IdEstado != 0)
                        {
                            Error = Error + "<li>Ya existe este Estado.</li>";
                        }
                        else
                        {
                            cEstado.Agregar(Conn);
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
    public static string ObtenerFormaEditarEstado(int IdEstado)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarEstado"))
            {

                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    string spEstado = "SELECT * FROM Estado WHERE IdEstado=@IdEstado ";
                    conn.DefinirQuery(spEstado);
                    conn.AgregarParametros("@IdEstado", IdEstado);
                    CObjeto oEStado = conn.ObtenerRegistro();
                    Datos.Add("IdEstado", oEStado.Get("IdEstado").ToString());
                    Datos.Add("Estado", oEStado.Get("Estado").ToString());
                    Datos.Add("IdPais", oEStado.Get("IdPais").ToString());
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
    public static string EditarEstado(int IdEstado, string Estado, int IdPais)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB Conn)
        {

            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarEstado"))
            {
                if (Conn.Conectado)
                {

                    CObjeto Datos = new CObjeto();
                    CEstado cEstado = new CEstado();
                    cEstado.IdEstado = IdEstado;
                    cEstado.Estado = Estado;
                    cEstado.IdPais = IdPais;
                    Error = ValidarEstado(cEstado);
                    if (Error == "")
                    {
                        int ExisteNom = CEstado.ValidaExisteEditaEstado(IdEstado, Estado, IdPais, Conn);
                        if (ExisteNom != 0)
                        {
                            Error = Error + "<li>Ya existe un estado con el mismo Nombre.</li>";
                        }
                        else
                        {
                            cEstado.Editar(Conn);
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

    private static string ValidarEstado(CEstado Estado)
    {
        string Mensaje = "";

        Mensaje += (Estado.IdPais == 0) ? "<li>Favor de completar el campo Pais.</li>" : Mensaje;
        Mensaje += (Estado.Estado == "") ? "<li>Favor de completar el campo Estado.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
}