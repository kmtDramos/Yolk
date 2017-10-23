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

public partial class _Controls_Catalogo_Medidores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarMedidores(int Pagina, string Columna, string Orden,int IdCliente, int IdPais, int IdEstado, 
        int IdMunicipio, int IdSucursal, string Medidor, int Estatus)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		    CSecurity permiso = new CSecurity();
		    if (permiso.tienePermiso("accesoMedidor"))
		    {
			    if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    int Paginado = 10;
                    int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
                    CDB ConexionBaseDatos = new CDB();
                    SqlConnection con = ConexionBaseDatos.conStr();

                    SqlCommand spMedidor = new SqlCommand("spg_grdMedidor", con);
                    spMedidor.CommandType = CommandType.StoredProcedure;
                    spMedidor.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                    spMedidor.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                    spMedidor.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                    spMedidor.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                    spMedidor.Parameters.Add("pIdCliente", SqlDbType.Int).Value = IdCliente;
                    spMedidor.Parameters.Add("pIdPais", SqlDbType.Int).Value = IdPais;
                    spMedidor.Parameters.Add("pIdEstado", SqlDbType.Int).Value = IdEstado;
                    spMedidor.Parameters.Add("pIdMunicipio", SqlDbType.Int).Value = IdMunicipio;
                    spMedidor.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = IdSucursal;
                    spMedidor.Parameters.Add("pMedidor", SqlDbType.Text).Value = Medidor;                
                    spMedidor.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
                    spMedidor.Parameters.Add("pBaja", SqlDbType.Int).Value = Estatus;                 
                    SqlDataAdapter daMedidor = new SqlDataAdapter(spMedidor);  
              
                    DataSet dsMedidor = new DataSet();
                    daMedidor.Fill(dsMedidor);

                    DataTable dtPaginador = dsMedidor.Tables[0];
                    DataTable dtMedidores = dsMedidor.Tables[1];

                    Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(dtPaginador));
                    Datos.Add("Medidores", Conn.ObtenerRegistrosDataTable(dtMedidores));

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
    public static string DesactivarMedidor(int IdMedidor, int Baja)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		CSecurity permiso = new CSecurity();
		if (permiso.tienePermiso("puedeManipularBajaMedidor"))
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

                CMedidores cMedidores = new CMedidores();
                cMedidores.IdMedidor = IdMedidor;
                cMedidores.Baja = desactivar;
                cMedidores.Desactivar(Conn);

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
    public static string ObtenerClientes()
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdCliente AS Valor, Cliente AS Etiqueta FROM Cliente WHERE Baja = 0";
                conn.DefinirQuery(query);
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
    public static string ObtenerSucursales(int IdCliente)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            int IdUsuario = CUsuario.ObtieneUsuarioSesion(conn);
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdSucursal AS Valor, Sucursal AS Etiqueta FROM Sucursal WHERE IdCliente= @IdCliente AND Baja=0 AND IdSucursal IN (SELECT IdSucursal FROM UsuarioSucursal WHERE IdUsuario=@IdUsuario AND Baja=0)";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdCliente", IdCliente);
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
    public static string AgregarMedidor(string Medidor, int IdSucursal, int IdCliente)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
			
			string Error = Conn.Mensaje;
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeAgregarMedidor"))
			{
			if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CMedidores cMedidor = new CMedidores();
                cMedidor.Medidor = Medidor;
                cMedidor.IdSucursal = IdSucursal;
                cMedidor.IdCliente = IdCliente;
                cMedidor.Baja = false;
                Error = ValidarMedidor(cMedidor);
                if (Error == "")
                {
                    cMedidor.Agregar(Conn);
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

    [WebMethod]
    public static string EditarMedidor(int IdMedidor, string Medidor)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeEditarMedidor"))
			{
				if (Conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();

                    CMedidores cMedidor = new CMedidores();
                    cMedidor.IdMedidor = IdMedidor;
                    cMedidor.Obtener(Conn);
                    cMedidor.Medidor = Medidor;
                    Error = ValidarMedidor(cMedidor);
                    if (Error == "")
                    {
                        cMedidor.Editar(Conn);
                    }

                    Respuesta.Add("Datos", Datos);
			    }
			    else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
		    }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; 
            }

		    Respuesta.Add("Error", Error);
            Conn.Cerrar();
        });

        return Respuesta.ToString();
    }


    private static string ValidarMedidor(CMedidores Medidor)
    {
        string Mensaje = "";

        Mensaje += (Medidor.Medidor == "") ? "<li>Favor de completar el campo Medidor.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }
	
	[WebMethod]
    public static string ObtenerFiltroCliente()
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
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

}