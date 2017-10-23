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

public partial class _Controls_Perfil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarPerfiles(int Pagina, string Columna, string Orden, int pBaja)
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
                SqlCommand Stored = new SqlCommand("spg_grdPerfil", con);
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
                DataTable DataTablePerfiles = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Perfiles", Conn.ObtenerRegistrosDataTable(DataTablePerfiles));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string DesactivarPerfil(int IdPerfil, int Baja)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB Conn)
        {
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeManipularBajaPerfil"))
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
                    CPerfil cPerfil = new CPerfil();
                    cPerfil.IdPerfil = IdPerfil;
                    cPerfil.Baja = desactivar;
                    cPerfil.Desactivar(Conn);
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
    public static string ObtenerFormaAgregarPerfil()
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarPerfil"))
            {
                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    string spPermiso = "EXEC sp_Pagina_Obtener @Opcion";
                    conn.DefinirQuery(spPermiso);
                    conn.AgregarParametros("@Opcion", 1);
                    CArreglo Paginas = conn.ObtenerRegistros();
                    Respuesta.Add("Paginas", Paginas);
                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }


    [WebMethod]
    public static string AgregarPerfil(string Perfil, int IdPagina)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarPerfil"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CPerfil cPerfil = new CPerfil();
                    cPerfil.Perfil = Perfil;
                    cPerfil.IdPagina = IdPagina;
                    cPerfil.Baja = false;
                    Error = ValidarPerfil(cPerfil);
                    if (Error == "")
                    {
                     cPerfil.Agregar(Conn);                        
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
    public static string ObtenerFormaEditarPerfil(int IdPerfil)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarPerfil"))
            {
                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    string spPerfil = "SELECT * FROM Perfil WHERE IdPerfil=@IdPerfil ";
                    conn.DefinirQuery(spPerfil);
                    conn.AgregarParametros("@IdPerfil", IdPerfil);
                    CObjeto oPerfil = conn.ObtenerRegistro();
                    Datos.Add("IdPerfil", oPerfil.Get("IdPerfil").ToString());
                    Datos.Add("Perfil", oPerfil.Get("Perfil").ToString());
                    Datos.Add("IdPagina", Convert.ToInt32(oPerfil.Get("IdPagina").ToString()));
                    Respuesta.Add("Dato", Datos);
                  

                    string spPagina = "SELECT IdPagina AS Valor, Pagina AS Etiqueta FROM Pagina WHERE Baja = 0";
                    conn.DefinirQuery(spPagina);
                    CArreglo Paginas = conn.ObtenerRegistros();
                    Respuesta.Add("Paginas", Paginas);
                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }


    [WebMethod]
    public static string EditarPerfil(int IdPerfil, string Perfil, int IdPagina)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {

            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarPerfil"))
            {
                if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    CPerfil cPerfil = new CPerfil();
                    cPerfil.IdPerfil = IdPerfil;
                    cPerfil.Perfil = Perfil;
                    cPerfil.IdPagina = IdPagina;
                    cPerfil.Baja = false;
                    Error = ValidarPerfil(cPerfil);
                    if (Error == "")
                    {
                        CObjeto Valida = new CObjeto();
                        int Existe = CPerfil.ValidaExisteEditar(IdPerfil,Perfil, IdPagina, Conn);
                        if (Existe != 0)
                        {
                            Error = Error + "<li>Ya existe un perfil con el mismo nombre.</li>";
                        }
                        else
                        {
                            cPerfil.Editar(Conn);
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
    public static string ObtenerFormaListarPerfilPermisos(int IdPerfil)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdPerfil FROM Perfil WHERE IdPerfil=@IdPerfil";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdPerfil", IdPerfil);
                CObjeto Perfil = conn.ObtenerRegistro();
                Datos.Add("IdPerfil", (int)Perfil.Get("IdPerfil"));
                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ListarPermisosDisponibles(int Pagina, string Columna, string Orden, int IdPerfil)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB Conn)
        {
            string Error = Conn.Mensaje;

            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 10;
                int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(Conn);
                CDB ConexionBaseDatos = new CDB();
                SqlConnection con = ConexionBaseDatos.conStr();
                SqlCommand spPermisosDisponibles = new SqlCommand("spg_grdPermisosDisponibles", con);
                spPermisosDisponibles.CommandType = CommandType.StoredProcedure;
                spPermisosDisponibles.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                spPermisosDisponibles.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                spPermisosDisponibles.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 50).Value = Columna;
                spPermisosDisponibles.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                spPermisosDisponibles.Parameters.Add("pIdPerfil", SqlDbType.Int).Value = IdPerfil;
                spPermisosDisponibles.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;
                SqlDataAdapter daPermisos = new SqlDataAdapter(spPermisosDisponibles);

                DataSet dsPermiso = new DataSet();
                daPermisos.Fill(dsPermiso);

                DataTable DataTablePaginador = dsPermiso.Tables[0];
                DataTable DataTablePermisosDisponibles = dsPermiso.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("PermisosDisponibles", Conn.ObtenerRegistrosDataTable(DataTablePermisosDisponibles));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ListarPermisosAsignadas(int Pagina, string Columna, string Orden, int IdPerfil)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate (CDB Conn)
        {
            string Error = Conn.Mensaje;

            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 10;
                int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(Conn);
                CDB ConexionBaseDatos = new CDB();
                SqlConnection con = ConexionBaseDatos.conStr();
                SqlCommand spPermisosAsignadas = new SqlCommand("spg_grdPermisosAsignadas", con);
                spPermisosAsignadas.CommandType = CommandType.StoredProcedure;
                spPermisosAsignadas.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                spPermisosAsignadas.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                spPermisosAsignadas.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 50).Value = Columna;
                spPermisosAsignadas.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                spPermisosAsignadas.Parameters.Add("pIdPerfil", SqlDbType.Int).Value = IdPerfil;
                spPermisosAsignadas.Parameters.Add("pBaja", SqlDbType.Int).Value = 0;
                SqlDataAdapter daPermisos = new SqlDataAdapter(spPermisosAsignadas);

                DataSet dsPermiso = new DataSet();
                daPermisos.Fill(dsPermiso);

                DataTable DataTablePaginador = dsPermiso.Tables[0];
                DataTable DataTablePermisosAsignadas = dsPermiso.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("PermisosAsignadas", Conn.ObtenerRegistrosDataTable(DataTablePermisosAsignadas));

                //string Query = "SELECT ISNULL(IdPermisoPredeterminada,0) AS IdPermisoPredeterminada FROM Perfil WHERE IdPerfil = @IdPerfil";
                //Conn.DefinirQuery(Query);
                //Conn.AgregarParametros("@IdPerfil", IdPerfil);
                //CObjeto Registro = Conn.ObtenerRegistro();
                //int IdPermisoPredeterminada = (int)Registro.Get("IdPermisoPredeterminada");

                //Respuesta.Add("IdPermisoPredeterminada", IdPermisoPredeterminada);
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AsignarPermisos(List<int> IdPermisoDisponible, int IdPerfil)
    {

        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate (CDB conn)
        {
            int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarCircuito"))
            {
                CObjeto Datos = new CObjeto();
                if (conn.Conectado)
                {
                    foreach (int IdPermiso in IdPermisoDisponible)
                    {

                        if (Error == "")
                        {
                            CPerfilPermiso PerfilPermiso = new CPerfilPermiso();
                            PerfilPermiso.IdPerfil = IdPerfil;
                            PerfilPermiso.Baja = false;

                            int IdPerfilPermiso = CPerfilPermiso.ValidaExiste(IdPerfil, IdPermiso, conn);
                            if (IdPerfilPermiso != 0)
                            {
                                PerfilPermiso.IdPerfilPermiso = IdPerfilPermiso;
                                PerfilPermiso.DesactivarEspecial(conn);
                            }
                            else
                            {
                                PerfilPermiso.IdPermiso = IdPermiso;
                                PerfilPermiso.Agregar(conn);
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
        });
        return Respuesta.ToString();

    }

    
    [WebMethod]
    public static string DesasignarPermisos(List<int> pIdPerfilPermiso, int IdPerfil)
    {
        CObjeto Respuesta = new CObjeto();
        string Error = "";

        CUnit.Firmado(delegate (CDB conn)
        {
            int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(conn);
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarCircuito"))
            {
                CObjeto Datos = new CObjeto();
                if (conn.Conectado)
                {
                    foreach (int IdPerfilPermiso in pIdPerfilPermiso)
                    {
                        CPerfilPermiso PerfilPermiso = new CPerfilPermiso();
                        PerfilPermiso.IdPerfil = IdPerfil;
                        PerfilPermiso.IdPerfilPermiso = IdPerfilPermiso;
                        PerfilPermiso.Baja = true;

                        if (Error == "")
                        {
                            PerfilPermiso.DesactivarEspecial(conn);
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
        });
        return Respuesta.ToString();

    }

    private static string ValidarPerfil(CPerfil Perfil)
    {
        string Mensaje = "";

        Mensaje += (Perfil.Perfil == "") ? "<li>Favor de completar el campo perfil.</li>" : Mensaje;
        Mensaje += (Perfil.IdPagina == 0) ? "<li>Favor de completar el campo pagina.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
}