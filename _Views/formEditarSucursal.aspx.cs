using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Views_formEditarSucursal : System.Web.UI.Page
{
    public static int IdSucursal = 0;
    public static string Sucursal = "";
    public static string IdMunicipio = "0";
    public static string IdEstado = "0";
    public static string IdPais = "0";
    public static string IdTipoTarifa = "0";
    public static string IdTipoTension = "0";
    public static string IdTipoCuota = "0";
    public static string IdRegion = "0";
    public static CArreglo Municipios = new CArreglo();
    public static CArreglo Estados = new CArreglo();
    public static CArreglo Paises = new CArreglo();
    public static CArreglo TipoTarifas = new CArreglo();
    public static CArreglo TipoTensiones = new CArreglo();
    public static CArreglo TipoCuotas = new CArreglo();
    public static CArreglo Regiones = new CArreglo();
    public static string Checked = "Checked";

    protected void Page_Load(object sender, EventArgs e)
    {
        CUnit.Firmado(delegate(CDB conn)
        {
            if (conn.Conectado)
            {
                IdSucursal = Convert.ToInt32(Request["IdSucursal"]);
                CSucursal cSucursal = new CSucursal();
                cSucursal.IdSucursal = IdSucursal;
                cSucursal.Obtener(conn);
                Sucursal = cSucursal.Sucursal;
                IdMunicipio = cSucursal.IdMunicipio.ToString();
                IdRegion = cSucursal.IdRegion.ToString();
                Checked = (cSucursal.Baja == false) ? "checked" : "";

                string query = "SELECT * FROM Municipio WHERE IdMunicipio = @IdMunicipio";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdMunicipio", IdMunicipio);
                CObjeto Validar = conn.ObtenerRegistro();
                IdEstado = Validar.Get("IdEstado").ToString();

                query = "SELECT * FROM Estado WHERE IdEstado = @IdEstado";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdEstado", IdEstado);
                Validar = conn.ObtenerRegistro();
                IdPais = Validar.Get("IdPais").ToString();

                query = "SELECT R.IdRegion, TC.IdTipoCuota, TC.IdTipoTension, TT.IdTipoTarifa FROM Region R INNER JOIN TipoCuota TC ON TC.IdTipoCuota=R.IdTipoCuota	INNER JOIN TipoTension TT ON TT.IdTipoTension=TC.IdTipoTension WHERE R.IdRegion= @IdRegion";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdRegion", IdRegion);
                Validar = conn.ObtenerRegistro();
                IdTipoCuota = Validar.Get("IdTipoCuota").ToString();
                IdTipoTension = Validar.Get("IdTipoTension").ToString();
                IdTipoTarifa = Validar.Get("IdTipoTarifa").ToString();

                //query = "SELECT * FROM TipoCuota WHERE IdRegion = @IdRegion";
                //conn.DefinirQuery(query);
                //conn.AgregarParametros("@IdRegion", IdRegion);
                //Validar = conn.ObtenerRegistro();
                //IdTipoCuota = Validar.Get("IdTipoCuota").ToString();

                /*----------ARREGLOS----------*/
                query = "SELECT * FROM Municipio WHERE IdEstado=@IdEstado";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdEstado", IdEstado);
                Municipios = conn.ObtenerRegistros();

                query = "SELECT * FROM Estado WHERE IdPais=@IdPais";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@IdPais", IdPais);
                Estados = conn.ObtenerRegistros();

                query = "SELECT * FROM Pais";
                conn.DefinirQuery(query);
                Paises = conn.ObtenerRegistros();

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
        });

    }
}