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

public partial class _Controls_Tarifa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarTarifas(int Pagina, string Columna, string Orden)
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
                SqlCommand Stored = new SqlCommand("spg_grdTarifa", con);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 100).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
                Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);

                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);

                DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTableTarifas = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Tarifas", Conn.ObtenerRegistrosDataTable(DataTableTarifas));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }
 
    [WebMethod]
    public static string AgregarTarifa(int IdFuente, int IdRegion, decimal ConsumoBaja, decimal ConsumoMedia, decimal ConsumoAlta, decimal Demanda, int Mes, int Anio)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
		CSecurity permiso = new CSecurity();
		if (permiso.tienePermiso("puedeAgregarTarifa"))
		{
			if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CTarifa cTarifa = new CTarifa();
                //cTarifa.Fecha = Fecha;
                cTarifa.ConsumoBaja = ConsumoBaja;
                cTarifa.ConsumoMedia = ConsumoMedia;
                cTarifa.ConsumoAlta = ConsumoAlta;
                cTarifa.Demanda = Demanda;
                cTarifa.IdRegion = IdRegion;
				cTarifa.IdFuente = IdFuente;
				cTarifa.Mes = Mes;
				cTarifa.Anio = Anio;
				cTarifa.Baja = false;
                Error = ValidarTarifa(cTarifa);
                if (Error == "")
                {
						int contador = CTarifa.ValidaExiste(IdRegion, Mes, Anio, Conn);
						if (contador == 0)
						{
							cTarifa.Agregar(Conn);
						}
						else
						{
							Error = Error + "<li>Ya existe tarifa para este mes en esta region.</li>";
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
    public static string EditarTarifa(int IdTarifa, int IdFuente, int IdRegion,decimal ConsumoBaja, decimal ConsumoMedia, decimal ConsumoAlta, decimal Demanda, int Mes, int Anio)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
			CSecurity permiso = new CSecurity();
			if (permiso.tienePermiso("puedeEditarTarifa"))
			{ 
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CTarifa cTarifa = new CTarifa();
                cTarifa.IdTarifa = IdTarifa;
				cTarifa.IdFuente = IdFuente;
                cTarifa.IdRegion = IdRegion;
                //cTarifa.Fecha = Fecha;
				cTarifa.Mes=Mes;
				cTarifa.Anio = Anio;
                cTarifa.ConsumoBaja = ConsumoBaja;
                cTarifa.ConsumoMedia = ConsumoMedia;
                cTarifa.ConsumoAlta = ConsumoAlta;
                cTarifa.Demanda = Demanda;
                cTarifa.Baja = false;
                Error = ValidarTarifa(cTarifa);
                if (Error == "")
                {
						int contador = CTarifa.ValidaExisteEditar(IdTarifa, IdRegion, Mes, Anio, Conn);
						if (contador == 0)
						{
							cTarifa.Editar(Conn);
						}
						else
						{
							Error = Error + "<li>Ya existe tarifa para este mes en esta region.</li>";
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
    public static string DesactivarTarifa(int IdTarifa, int Baja)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
			CSecurity permiso = new CSecurity();
			if(permiso.tienePermiso("puedeManipularBajaTarifa"))
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

                CTarifa cTarifa = new CTarifa();
                cTarifa.IdTarifa = IdTarifa;
                cTarifa.Baja = desactivar;
                cTarifa.Desactivar(Conn);

                Respuesta.Add("Datos", Datos);
            }
				else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
			}
			else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }
			Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    private static string ValidarTarifa(CTarifa Tarifa)
    {
        string Mensaje = "";

        Mensaje += (Tarifa.IdRegion == 0) ? "<li>Favor de completar el campo región.</li>" : Mensaje;
        //Mensaje += (Tarifa.Fecha == "") ? "<li>Favor de completar el campo fecha.</li>" : Mensaje;
        Mensaje += (Tarifa.ConsumoBaja == 0) ? "<li>Favor de completar el campo consumo baja.</li>" : Mensaje;
        Mensaje += (Tarifa.ConsumoMedia == 0) ? "<li>Favor de completar el campo consumo media.</li>" : Mensaje;
        Mensaje += (Tarifa.ConsumoAlta == 0) ? "<li>Favor de completar el campo consumo alta.</li>" : Mensaje;
        Mensaje += (Tarifa.Demanda == 0) ? "<li>Favor de completar el campo demanda.</li>" : Mensaje;
        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }

    [WebMethod]
    public static string ObtenerTipoTarifa()
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdTipoTarifa AS Valor, TipoTarifa AS Etiqueta FROM TipoTarifa WHERE Baja = 0";
                conn.DefinirQuery(query);
                CArreglo TipoTarifas = conn.ObtenerRegistros();

                Datos.Add("TipoTarifas", TipoTarifas);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTipoTension(int IdTipoTarifa)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdTipoTension AS Valor, TipoTension AS Etiqueta FROM TipoTension WHERE IdTipoTarifa = @IdTipoTarifa AND Baja = 0";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdTipoTarifa", IdTipoTarifa);
                CArreglo TipoTensiones = conn.ObtenerRegistros();

                Datos.Add("TipoTensiones", TipoTensiones);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTipoCuota(int IdTipoTension)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdTipoCuota AS Valor, TipoCuota AS Etiqueta FROM TipoCuota WHERE IdTipoTension=@IdTipoTension AND Baja = 0";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdTipoTension", IdTipoTension);
                CArreglo TipoCuotas = conn.ObtenerRegistros();

                Datos.Add("TipoCuotas", TipoCuotas);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerRegion(int IdTipoCuota)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "SELECT IdRegion AS Valor, Region AS Etiqueta FROM Region WHERE IdTipoCuota=@IdTipoCuota AND Baja = 0";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdTipoCuota", IdTipoCuota);
                CArreglo Regiones = conn.ObtenerRegistros();

                Datos.Add("Regiones", Regiones);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

	[WebMethod]
	public static string ObtenerFuente()
	
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate (CDB conn)
		{
			string Error = conn.Mensaje;
			if (conn.Conectado)
			{
				CObjeto Datos = new CObjeto();

				string query = "SELECT IdFuente AS Valor, Fuente AS Etiqueta FROM Fuente WHERE Baja = 0";
				conn.DefinirQuery(query);
				CArreglo TipoFuentes = conn.ObtenerRegistros();

				Datos.Add("TipoFuentes", TipoFuentes);

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);
		});
		return Respuesta.ToString();
	}

}