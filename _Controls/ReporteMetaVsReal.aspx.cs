using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class _Controls_ReporteMetaVsReal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ObtenerDetallex(int Pagina, string Columna, string Orden, int IdCliente, int IdPais, int IdEstado, int IdMunicipio, int IdSucursal, int IdTablero, int IdCircuito, string FechaInicio, string FechaFin)
    {
        //var serializer = new JavaScriptSerializer();
        //serializer.MaxJsonLength = 500000000;

        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 10;

                CDB ConexionBaseDatos = new CDB();
                SqlConnection conexion = ConexionBaseDatos.conStr();
                SqlCommand Stored = new SqlCommand("spr_Reporte_Medicion3", conexion);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("IdCliente", SqlDbType.Int).Value = IdCliente;
                Stored.Parameters.Add("IdPais", SqlDbType.Int).Value = IdPais;
                Stored.Parameters.Add("IdEstado", SqlDbType.Int).Value = IdEstado;
                Stored.Parameters.Add("IdMunicipio", SqlDbType.Int).Value = IdMunicipio;
                Stored.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
                Stored.Parameters.Add("IdTablero", SqlDbType.Int).Value = IdTablero;
                Stored.Parameters.Add("IdCircuito", SqlDbType.Int).Value = IdCircuito;
                Stored.Parameters.Add("FechaInicio", SqlDbType.Text).Value = FechaInicio;
                Stored.Parameters.Add("FechaFin", SqlDbType.Text).Value = FechaFin;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);
                dataAdapterRegistros.SelectCommand.CommandTimeout = 1800;
                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);
                // DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTableConsumos = ds.Tables[0];
                DataTable DataTableMetas = ds.Tables[1];

                Datos.Add("Detalle", conn.ObtenerRegistrosDataTable(DataTableConsumos));
                Datos.Add("Consumo", conn.ObtenerRegistrosDataTable(DataTableMetas));
                //Datos.Add("Paginador", conn.ObtenerRegistrosDataTable(DataTablePaginador));

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });



        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerDetalle(int Pagina, string Columna, string Orden, int IdCliente, int IdPais, int IdEstado, int IdMunicipio, int IdSucursal, int IdTablero, int IdCircuito, string FechaInicio, string FechaFin)
    {
        //var serializer = new JavaScriptSerializer();
        //serializer.MaxJsonLength = 500000000;

        JObject Respuesta = new JObject();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 10;

                CDB ConexionBaseDatos = new CDB();
                SqlConnection conexion = ConexionBaseDatos.conStr();
                SqlCommand Stored = new SqlCommand("spr_reportemetavsreal_prueba1", conexion);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);
                dataAdapterRegistros.SelectCommand.CommandTimeout = 1800;
                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);
                DataTable DataTableAnos = ds.Tables[0];
                DataTable DataTableRegistros = ds.Tables[1];

                int totalFilasAnio = (DataTableAnos.Rows.Count);
                int totalFilas = (DataTableRegistros.Rows.Count);
                int a = 0;
                


                JArray JAAnios = new JArray();

                while (a < totalFilasAnio)
                {
                    int r = 0;

                    JObject JOAnio = new JObject();
                    JOAnio.Add(new JProperty("Anio", DataTableAnos.Rows[a]["Anio"].ToString()));

                    JArray JRegistros = new JArray();
                    while (r < totalFilas)
                    {
                        if ( DataTableAnos.Rows[a]["Anio"].ToString() == DataTableRegistros.Rows[r]["Anio"].ToString())
                        {
                            JObject Registro = new JObject();
                            Registro.Add(new JProperty("Mes", DataTableRegistros.Rows[r]["Mes"].ToString()));
                            Registro.Add(new JProperty("DatoReal", DataTableRegistros.Rows[r]["DatoReal"].ToString()));
                            Registro.Add(new JProperty("Meta", DataTableRegistros.Rows[r]["Meta"].ToString()));
                            JRegistros.Add(Registro);
                        }
                        r++;
                    }
                    JOAnio.Add("Registros", JRegistros);
                    a++;

                    JAAnios.Add(JOAnio);
                }

                Respuesta.Add("Datos", JAAnios);
            }
            Respuesta.Add("Error", Error);
        });



        return Respuesta.ToString();
    }    
}