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

public partial class _Controls_Operacion_Mantenimiento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarMantenimientos(int Pagina, string Columna, string Orden)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;

            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                //int Paginado = 10;
                //int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
                //CDB ConexionBaseDatos = new CDB();
                //SqlConnection con = ConexionBaseDatos.conStr();
                //SqlCommand Stored = new SqlCommand("spg_grdCircuito", con);
                //Stored.CommandType = CommandType.StoredProcedure;
                //Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                //Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                //Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                //Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                //Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = IdCliente;
                //Stored.Parameters.Add("pIdPais", SqlDbType.Int).Value = IdPais;
                //Stored.Parameters.Add("pIdEstado", SqlDbType.Int).Value = IdEstado;
                //Stored.Parameters.Add("pIdMunicipio", SqlDbType.Int).Value = IdMunicipio;
                //Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = IdSucursal;
                //Stored.Parameters.Add("pIdMedidor", SqlDbType.Int).Value = IdMedidor;
                //Stored.Parameters.Add("pIdTablero", SqlDbType.Int).Value = IdTablero;
                //Stored.Parameters.Add("pCircuito", SqlDbType.Text).Value = Circuito;
                //Stored.Parameters.Add("pDescripcionCircuito", SqlDbType.Text).Value = Descripcion;
                //Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
                //Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;
                //SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);

                //DataSet ds = new DataSet();
                //dataAdapterRegistros.Fill(ds);

                //DataTable DataTablePaginador = ds.Tables[0];
                //DataTable DataTableCircuitos = ds.Tables[1];

                //Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                //Datos.Add("Circuitos", Conn.ObtenerRegistrosDataTable(DataTableCircuitos));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }
}