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

public partial class _Controls_Catalogo_Tablero : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarTableros(int Pagina, string Columna, string Orden, int IdMedidor)
    {
        CObjeto Respuesta = new CObjeto();
        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		CSecurity permiso = new CSecurity();
		if (permiso.tienePermiso("accesoTablero"))
		{
			if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 10;
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
                CDB ConexionBaseDatos = new CDB();
                SqlConnection con = ConexionBaseDatos.conStr();
                SqlCommand Stored = new SqlCommand("spg_grdTablero", con);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("pIdMedidor", SqlDbType.Int).Value = IdMedidor;
                Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
                Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);

                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);

                DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTableTableros = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Tableros", Conn.ObtenerRegistrosDataTable(DataTableTableros));
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
    public static string AgregarTablero(int IdMedidor, string Tablero)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		CSecurity permiso = new CSecurity();
		if (permiso.tienePermiso("puedeAgregarTablero"))
		{
			if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CTablero cTablero = new CTablero();
                cTablero.IdMedidor = IdMedidor;
                cTablero.Tablero = Tablero;
                cTablero.Baja = false;
                Error = ValidarTablero(cTablero);
                if (Error == "")
                {
                    cTablero.Agregar(Conn);
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
    public static string EditarTablero(int IdTablero, string Tablero)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		CSecurity permiso = new CSecurity();
		if (permiso.tienePermiso("puedeEditarTablero"))
		{
			if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CTablero cTablero = new CTablero();
                cTablero.IdTablero = IdTablero;
                cTablero.Tablero = Tablero;                
                cTablero.Baja = false;
                Error = ValidarTablero(cTablero);
                if (Error == "")
                {
                    cTablero.Editar(Conn);
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
    public static string DesactivarTablero(int IdTablero, int Baja)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;

			CSecurity permiso = new CSecurity();
			if(permiso.tienePermiso("puedeManipularBajaTablero")) 
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

                    CTablero cTablero = new CTablero();
                    cTablero.IdTablero = IdTablero;
                    cTablero.Baja = desactivar;
                    cTablero.Desactivar(Conn);

                    Respuesta.Add("Datos", Datos);
                }
                else { Error = Error + "<li>"+Conn.Mensaje+"</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

        Respuesta.Add("Error", Error);

        });

        return Respuesta.ToString();
    }

    private static string ValidarTablero(CTablero Tablero)
    {
        string Mensaje = "";

        Mensaje += (Tablero.Tablero == "") ? "<li>Favor de completar el campo de descripción del tablero.</li>" : Mensaje;
        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
}