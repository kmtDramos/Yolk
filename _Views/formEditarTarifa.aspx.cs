using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Views_formEditarTarifa : System.Web.UI.Page
{
    public static string Id = "0";
    public static string Tarifa = "";
	public static string IdFuente = "0";
    public static string IdTipoTarifa = "0";
    public static string IdTipoTension = "0";
    public static string IdTipoCuota = "0";
    public static string IdRegion = "0";
    //public static string Fecha = "";
	public static string Mes = "";
	public static string Anio = "";
    public static string ConsumoBaja = "0";
    public static string ConsumoMedia = "0";
    public static string ConsumoAlta = "0";
    public static string Demanda = "0";
	public static CArreglo TipoFuentes = new CArreglo();
	public static CArreglo TipoTarifas = new CArreglo();
    public static CArreglo TipoTensiones = new CArreglo();
    public static CArreglo TipoCuotas = new CArreglo();
    public static CArreglo Regiones = new CArreglo();

    protected void Page_Load(object sender, EventArgs e)
    {
        CUnit.Accion(delegate(CDB conn)
        {
            int IdTarifa = Convert.ToInt32(Request["IdTarifa"]);
            if (IdTarifa > 0)
            {
                string query = "SELECT * FROM Tarifa WHERE IdTarifa = @IdTarifa";
				conn.DefinirQuery(query);
				conn.AgregarParametros("@IdTarifa", IdTarifa);
				CObjeto oTarifa = conn.ObtenerRegistro();
				Tarifa = IdTarifa.ToString();
                if (oTarifa.Exist("IdTarifa"))
                {
                    Id = oTarifa.Get("IdTarifa").ToString();
					IdFuente = oTarifa.Get("IdFuente").ToString();
                    IdRegion = oTarifa.Get("IdRegion").ToString();
                    ConsumoBaja = oTarifa.Get("ConsumoBaja").ToString();
                    ConsumoMedia = oTarifa.Get("ConsumoMedia").ToString();
                    ConsumoAlta = oTarifa.Get("ConsumoAlta").ToString();
                    Demanda = oTarifa.Get("Demanda").ToString();
					//Fecha = Convert.ToDateTime(oTarifa.Get("Fecha").ToString()).ToShortDateString();
					Mes = oTarifa.Get("Mes").ToString().ToString();
					Anio = oTarifa.Get("Anio").ToString().ToString();



					query = "SELECT T.IdTarifa,R.IdRegion, TC.IdTipoCuota, TT.IdTipoTension, TT.IdTipoTarifa, F.IdFuente,T.Mes,T.Anio FROM Tarifa T INNER JOIN Region R ON T.IdRegion=R.IdRegion INNER JOIN TipoCuota TC ON TC.IdTipoCuota=R.IdTipoCuota INNER JOIN TipoTension TT ON TT.IdTipoTension=TC.IdTipoTension INNER JOIN Fuente F ON T.IdFuente=F.IdFuente WHERE T.IdTarifa= @IdTarifa ";
                    conn.DefinirQuery(query);
                    conn.AgregarParametros("@IdTarifa", IdTarifa);
                    CObjeto Validar = conn.ObtenerRegistro();
					IdFuente = Validar.Get("IdFuente").ToString();
                    IdRegion = Validar.Get("IdRegion").ToString();
                    IdTipoCuota = Validar.Get("IdTipoCuota").ToString();
                    IdTipoTension = Validar.Get("IdTipoTension").ToString();
                    IdTipoTarifa = Validar.Get("IdTipoTarifa").ToString();
					Mes = Validar.Get("Mes").ToString();
					Anio = Validar.Get("Anio").ToString();

					/*-------------Arreglos*/
					query = "SELECT * FROM Fuente WHERE IdFuente=@IdFuente";
					conn.DefinirQuery(query);
					conn.AgregarParametros("@IdFuente", IdFuente);
					TipoFuentes = conn.ObtenerRegistros();

					query = "SELECT * FROM Region WHERE IdTipoCuota=@IdTipoCuota";
                    conn.DefinirQuery(query);
                    conn.AgregarParametros("@IdTipoCuota", IdTipoCuota);
                    Regiones = conn.ObtenerRegistros();

                    query = "SELECT * FROM TipoCuota WHERE IdTipoTension=@IdTipoTension";
                    conn.DefinirQuery(query);
                    conn.AgregarParametros("@IdTipoTension", IdTipoTension);
                    TipoCuotas = conn.ObtenerRegistros();

                    query = "SELECT * FROM TipoTension WHERE IdTipoTarifa=@IdTipoTarifa";
                    conn.DefinirQuery(query);
                    conn.AgregarParametros("@IdTipoTarifa", IdTipoTarifa);
                    TipoTensiones = conn.ObtenerRegistros();

                    query = "SELECT * FROM TipoTarifa WHERE Baja=0";
                    conn.DefinirQuery(query);
                    TipoTarifas = conn.ObtenerRegistros();
                }
            }
        });
    }
}