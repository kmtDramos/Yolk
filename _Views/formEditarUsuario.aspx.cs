using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Views_formEditarUsuario : System.Web.UI.Page
{

	public static int IdUsuario = 0;
	public static string Nombre = "";
	public static string ApellidoPaterno = "";
	public static string ApellidoMaterno = "";
	public static string Usuario = "";
	public static string Correo = "";
	public static string Checked = "Checked";
    public static string IdPerfil = "0";
    public static CArreglo Perfiles = new CArreglo();
    public static CArreglo Jefes = new CArreglo();
	public static string IdProveedor = "0";
	public static CArreglo Proveedores = new CArreglo();
    public static string Id = "0";
	public static string Cliente = "";
    public static string IdCliente = "";
    public static string IdMunicpio = "0";
	public static string IdEstado = "0";
	public static string IdPais = "0";
    public static string IdSucursal = "0";
    public static CArreglo Municipios = new CArreglo();
	public static CArreglo Estados = new CArreglo();
    public static CArreglo Paises = new CArreglo();
    public static CArreglo Clientes = new CArreglo();
    public static CArreglo Sucursales = new CArreglo();


    protected void Page_Load(object sender, EventArgs e)
	{
		CUnit.Firmado(delegate(CDB Conn)
		{
			if (Conn.Conectado)
			{
				IdUsuario = Convert.ToInt32(Request["IdUsuario"]);
				CUsuario cUsuario = new CUsuario();
				cUsuario.IdUsuario = IdUsuario;
				cUsuario.Obtener(Conn);
				Nombre = cUsuario.Nombre;
				ApellidoPaterno = cUsuario.ApellidoPaterno;
				ApellidoMaterno = cUsuario.ApellidoMaterno;
				Usuario = cUsuario.Usuario;
				Correo = cUsuario.Correo;
                IdPerfil = Convert.ToString(cUsuario.IdPerfil);
				//Checked = (cUsuario.Baja == false) ? "checked" : "";

                var query = "EXEC sp_Perfil_Consultar @Opcion";
                Conn.DefinirQuery(query);
                Conn.AgregarParametros("@Opcion", 2);
                Perfiles = Conn.ObtenerRegistros();

                var spUsuario = "EXEC sp_Usuario_Consultar @Opcion";
                Conn.DefinirQuery(spUsuario);
                Conn.AgregarParametros("@Opcion", 2);
                Jefes = Conn.ObtenerRegistros();

                //IdCliente = Convert.ToString(cUsuario.IdClientePredeterminado);
                //if (IdCliente != "0")
               // {
                    //IdSucursal = Convert.ToString(cUsuario.IdSucursalPredeterminada);

                    //query = "SELECT * FROM Cliente WHERE IdCliente = @IdCliente";
                    //Conn.DefinirQuery(query);
                    //Conn.AgregarParametros("@IdCliente", IdCliente);
                    //Clientes = Conn.ObtenerRegistros();
                    //CObjeto oCliente = Conn.ObtenerRegistro();

                    //Cliente = IdCliente.ToString();
                    //if (oCliente.Exist("Cliente"))
                    //{
                    //    Id = oCliente.Get("IdCliente").ToString();
                    //    Cliente = oCliente.Get("Cliente").ToString();
                    //    IdMunicpio = oCliente.Get("IdMunicipio").ToString();

                    //    query = "SELECT * FROM Municipio WHERE IdMunicipio = @IdMunicipio";
                    //    Conn.DefinirQuery(query);
                    //    Conn.AgregarParametros("@IdMunicipio", IdMunicpio);
                    //    CObjeto Validar = Conn.ObtenerRegistro();
                    //    IdEstado = Validar.Get("IdEstado").ToString();

                    //    query = "SELECT * FROM Estado WHERE IdEstado = @IdEstado";
                    //    Conn.DefinirQuery(query);
                    //    Conn.AgregarParametros("@IdEstado", IdEstado);
                    //    Validar = Conn.ObtenerRegistro();
                    //    IdPais = Validar.Get("IdPais").ToString();
                    //    /**/
                    //    query = "SELECT * FROM Municipio WHERE IdEstado=@IdEstado";
                    //    Conn.DefinirQuery(query);
                    //    Conn.AgregarParametros("@IdEstado", IdEstado);
                    //    Municipios = Conn.ObtenerRegistros();

                    //    query = "SELECT * FROM Estado WHERE IdPais=@IdPais";
                    //    Conn.DefinirQuery(query);
                    //    Conn.AgregarParametros("@IdPais", IdPais);
                    //    Estados = Conn.ObtenerRegistros();

                    //    query = "SELECT * FROM Pais";
                    //    Conn.DefinirQuery(query);
                    //    Paises = Conn.ObtenerRegistros();

                    //    query = "SELECT * FROM Sucursal WHERE IdSucursal = @IdSucursal";
                    //    Conn.DefinirQuery(query);
                    //    Conn.AgregarParametros("@Idsucursal", IdSucursal);
                    //    Sucursales = Conn.ObtenerRegistros();
                    //}
                }
		});
    }
}