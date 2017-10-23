using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class CUsuario
{

	private int idusuario = 0;
	private string usuario = "";
	private string password = "";
	private string nombre = "";
	private string apellidopaterno = "";
	private string apellidomaterno = "";
	private string correo = "";
	private string cookie = "";
	private string ip = "";
	private DateTime fechaultimoacceso = new DateTime(1900,1,1);
    private int idperfil = 0;
    private int idsucursalpredeterminada = 0;
	private int idusuariojefe = 0;
	private bool esresponsablesucursal = false;
	private bool baja = false;

	public int IdUsuario {
		get
		{
			return idusuario;
		}
		set
		{
			idusuario = value;
		}
	}

	public int IdUsuarioJefe
	{
		get
		{
			return idusuariojefe;
		}
		set
		{
			idusuariojefe = value;
		}
	}

	public string Usuario
	{
		get
		{
			return usuario;
		}
		set
		{
			usuario = value;
		}
	}

	public string Password
	{
		get
		{
			return password;
		}
		set
		{
			password = value;
		}
	}

	public string Nombre
	{
		get
		{
			return nombre;
		}
		set
		{
			nombre = value;
		}
	}

	public string ApellidoPaterno
	{
		get
		{
			return apellidopaterno;
		}
		set
		{
			apellidopaterno = value;
		}
	}

	public string ApellidoMaterno
	{
		get
		{
			return apellidomaterno;
		}
		set
		{
			apellidomaterno = value;
		}
	}

	public string Correo
	{
		get
		{
			return correo;
		}
		set
		{
			correo = value;
		}
	}

	public string Cookie
	{
		get
		{
			return cookie;
		}
		set
		{
			cookie = value;
		}
	}

	public string IP
	{
		get
		{
			return ip;
		}
		set
		{
			ip = value;
		}
	}

	public DateTime FechaUltimoAcceso
	{
		get
		{
			return fechaultimoacceso;
		}
		set
		{
			fechaultimoacceso = value;
		}
	}

    public int IdPerfil
    {
        get
        {
            return idperfil;
        }
        set
        {
            idperfil = value;
        }
    }

	public int IdSucursalPredeterminada
    {
        get
        {
            return idsucursalpredeterminada;
        }
        set
        {
            idsucursalpredeterminada = value;
        }
    }

	public bool Baja
	{
		get
		{
			return baja;
		}
		set
		{
			baja = value;
		}
	}

	public bool EsResponsableSucursal
	{
		get
		{
			return esresponsablesucursal;
		}
		set
		{
			esresponsablesucursal = value;
		}
	}

	// Constructor
	public CUsuario()
	{

	}

	// Cargar Usuario
	public void Obtener(CDB Conn)
	{
		if (idusuario != 0)
		{
			string Query = "SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario";
			Conn.DefinirQuery(Query);
			Conn.AgregarParametros("@IdUsuario", idusuario);
			SqlDataReader Datos = Conn.Ejecutar();
			DefinirPropiedades(Datos);
			Datos.Close();
		}
	}

	// Agregar registro
	public void Agregar(CDB Conn)
	{
		string Query = "INSERT INTO Usuario (Usuario,Password,Nombre,ApellidoPaterno,ApellidoMaterno,Correo,Cookie,IP,FechaUltimoAcceso,idPerfil,IdSucursalPredeterminada,Baja,IdUsuarioJefe,EsResponsableSucursal) VALUES (@Usuario,@Password,@Nombre,@ApellidoPaterno,@ApellidoMaterno,@Correo,@Cookie,@IP,@FechaUltimoAcceso, @IdPerfil,@IdSucursalPredeterminada,@Baja,@IdUsuarioJefe,@EsResponsableSucursal)" +
			"SELECT * FROM Usuario WHERE IdUsuario = SCOPE_IDENTITY()";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@Usuario", usuario);
		Conn.AgregarParametros("@Password",password);
		Conn.AgregarParametros("@Nombre",nombre);
		Conn.AgregarParametros("@ApellidoPaterno",apellidopaterno);
		Conn.AgregarParametros("@ApellidoMaterno",apellidomaterno);
		Conn.AgregarParametros("@Correo",correo);
		Conn.AgregarParametros("@Cookie",cookie);
		Conn.AgregarParametros("@IP",ip);
		Conn.AgregarParametros("@FechaUltimoAcceso",fechaultimoacceso);
        Conn.AgregarParametros("@IdPerfil", idperfil);
        Conn.AgregarParametros("@Idsucursalpredeterminada", IdSucursalPredeterminada);
		Conn.AgregarParametros("@IdUsuarioJefe", idusuariojefe);
		Conn.AgregarParametros("@EsResponsableSucursal", esresponsablesucursal);
        Conn.AgregarParametros("@Baja",baja);
		SqlDataReader Datos = Conn.Ejecutar();

		DefinirPropiedades(Datos);
		Datos.Close();
	}

	// Editar registro
	public void Editar(CDB Conn)
	{
		if (idusuario != 0)
		{
			string Query = "UPDATE Usuario SET Usuario=@Usuario,Password=@Password,Nombre=@Nombre,ApellidoPaterno=@ApellidoPaterno,ApellidoMaterno=@ApellidoMaterno,Correo=@Correo,Cookie=@Cookie,IP=@IP,FechaUltimoAcceso=@FechaUltimoAcceso, IdPerfil=@IdPerfil, IdUsuarioJefe=@IdUsuarioJefe, EsResponsableSucursal=@EsResponsableSucursal, Baja=@Baja WHERE IdUsuario=@IdUsuario;" +
			"SELECT * FROM Usuario WHERE IdUsuario = SCOPE_IDENTITY()";
			Conn.DefinirQuery(Query);
			Conn.AgregarParametros("@IdUsuario", idusuario);
			Conn.AgregarParametros("@Usuario", usuario);
			Conn.AgregarParametros("@Password", password);
			Conn.AgregarParametros("@Nombre", nombre);
			Conn.AgregarParametros("@ApellidoPaterno", apellidopaterno);
			Conn.AgregarParametros("@ApellidoMaterno", apellidomaterno);
			Conn.AgregarParametros("@Correo", correo);
			Conn.AgregarParametros("@Cookie", cookie);
			Conn.AgregarParametros("@IP", ip);
			Conn.AgregarParametros("@FechaUltimoAcceso", fechaultimoacceso);
            Conn.AgregarParametros("@IdPerfil", idperfil);
            Conn.AgregarParametros("@IdUsuarioJefe", idusuariojefe);
            Conn.AgregarParametros("@EsResponsableSucursal", esresponsablesucursal);
			Conn.AgregarParametros("@Baja", baja);
			SqlDataReader Datos = Conn.Ejecutar();
			DefinirPropiedades(Datos);
			Datos.Close();
		}
	}

    public void EditarContrasena(CDB Conn)
    {
        if (idusuario != 0)
        {
            string Query = "UPDATE Usuario SET Password=@Password WHERE IdUsuario=@IdUsuario;" +
            "SELECT * FROM Usuario WHERE IdUsuario = SCOPE_IDENTITY()";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdUsuario", idusuario);
            Conn.AgregarParametros("@Password", password);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Editar registro
    public void EditarEspecial(CDB Conn)
    {
        if (idusuario != 0)
        {
            string Query = "UPDATE Usuario SET Nombre=@Nombre,ApellidoPaterno=@ApellidoPaterno,ApellidoMaterno=@ApellidoMaterno,Correo=@Correo,Cookie=@Cookie,IP=@IP,FechaUltimoAcceso=@FechaUltimoAcceso, IdPerfil=@IdPerfil WHERE IdUsuario=@IdUsuario;" +
            "SELECT * FROM Usuario WHERE IdUsuario = SCOPE_IDENTITY()";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdUsuario", idusuario);
            Conn.AgregarParametros("@Nombre", nombre);
            Conn.AgregarParametros("@ApellidoPaterno", apellidopaterno);
            Conn.AgregarParametros("@ApellidoMaterno", apellidomaterno);
            Conn.AgregarParametros("@Correo", correo);
            Conn.AgregarParametros("@Cookie", cookie);
            Conn.AgregarParametros("@IP", ip);
            Conn.AgregarParametros("@FechaUltimoAcceso", fechaultimoacceso);
            Conn.AgregarParametros("@IdPerfil", idperfil);            
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

	// Definir valores de instancia
	private void DefinirPropiedades(SqlDataReader Datos)
	{
		if (Datos.HasRows)
		{
			while (Datos.Read())
			{
				idusuario = !(Datos["IdUsuario"] is DBNull) ? Convert.ToInt32(Datos["IdUsuario"]) : idusuario;
				usuario = !(Datos["Usuario"] is DBNull) ? Convert.ToString(Datos["Usuario"]) : usuario;
				password = !(Datos["Password"] is DBNull) ? Convert.ToString(Datos["Password"]) : password;
				nombre = !(Datos["Nombre"] is DBNull) ? Convert.ToString(Datos["Nombre"]) : nombre;
				apellidopaterno = !(Datos["ApellidoPaterno"] is DBNull) ? Convert.ToString(Datos["ApellidoPaterno"]) : apellidopaterno;
				apellidomaterno = !(Datos["ApellidoMaterno"] is DBNull) ? Convert.ToString(Datos["ApellidoMaterno"]) : apellidomaterno;
				correo = !(Datos["Correo"] is DBNull) ? Convert.ToString(Datos["Correo"]) : correo;
				cookie = !(Datos["Cookie"] is DBNull) ? Convert.ToString(Datos["Cookie"]) : cookie;
				ip = !(Datos["IP"] is DBNull) ? Convert.ToString(Datos["IP"]) : ip;
				fechaultimoacceso = !(Datos["FechaUltimoAcceso"] is DBNull) ? Convert.ToDateTime(Datos["FechaUltimoAcceso"]) : fechaultimoacceso;
                idperfil = !(Datos["IdPerfil"] is DBNull) ? Convert.ToInt32(Datos["IdPerfil"]) : idperfil;
                idsucursalpredeterminada = !(Datos["IdSucursalPredeterminada"] is DBNull) ? Convert.ToInt32(Datos["IdSucursalPredeterminada"]) : idsucursalpredeterminada;
                idusuariojefe = !(Datos["IdUsuarioJefe"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioJefe"]) : idusuariojefe;
                esresponsablesucursal = !(Datos["EsResponsableSucursal"] is DBNull) ? Convert.ToBoolean(Datos["EsResponsableSucursal"]) : esresponsablesucursal;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
			}
		}
	}

	// Limpiar valores de instancia
	private void LimpiarPropiedades()
	{
		idusuario = 0;
		usuario = "";
		password = "";
		nombre = "";
		apellidopaterno = "";
		apellidomaterno = "";
		correo = "";
		cookie = "";
		ip = "";
		fechaultimoacceso = new DateTime();
        idperfil = 0;
        idsucursalpredeterminada = 0;
		baja = false;
	}

    public static int ObtieneUsuarioSesion(CDB Conn)
    {
        int IdUsuario = 0;
        HttpCookie Dato = HttpContext.Current.Request.Cookies[CMD5.Encriptar("KeepUnitUserCookie")];
        if (Dato != null)
        {
            string Cookie = Dato.Value;
            string Query = "SELECT IdUsuario FROM Usuario WHERE Cookie = @Cookie";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@Cookie", Cookie);
            CObjeto Registro = Conn.ObtenerRegistro();
            CUsuario Usuario = new CUsuario();
            Usuario.IdUsuario = (Registro.Exist("IdUsuario")) ? (int)Registro.Get("IdUsuario") : 0;
            Usuario.Obtener(Conn);
            IdUsuario = Usuario.IdUsuario;
        }
        return IdUsuario;
    }

    public static JObject ObtenerJsonClienteObtieneDatosUsuarioSesion(JObject esteObjeto)
    {
        CDB conn = new CDB();
        string sp = "EXEC SP_Cliente_ObtenerDatosUsuarioSesion @Opcion, @pIdUsuario";
        conn.DefinirQuery(sp);
        conn.AgregarParametros("@Opcion", 1);
        conn.AgregarParametros("@pIdUsuario", Convert.ToInt32(esteObjeto.Property("IdUsuario").Value.ToString()));
        CObjeto Registro = conn.ObtenerRegistro();

        string Logo = "";
        int IdPerfil = 0;

        if (Registro.Exist("IdCliente"))
        {
            Logo = (string)Registro.Get("Logo");
            IdPerfil = (Int32)Registro.Get("IdPerfil");
        }

        if (Logo == "")
        {
            Logo = "NoImage.png";
        }

        esteObjeto.Add(new JProperty("Logo", Logo));
        esteObjeto.Add(new JProperty("IdPerfil", IdPerfil));
        return esteObjeto;
    }

    public void EditarSucursalPredeterminada(CDB Conn)
    {
        if (idusuario != 0)
        {
            string Query = "UPDATE Usuario SET IdSucursalPredeterminada=@IdSucursalPredeterminada WHERE IdUsuario=@IdUsuario;" +
            "SELECT * FROM Usuario WHERE IdUsuario = SCOPE_IDENTITY()";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdUsuario", idusuario);
            Conn.AgregarParametros("@IdSucursalPredeterminada", idsucursalpredeterminada);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    public static int ValidaSucursalPredeterminada(int IdSucursalPredeterminada, int IdUsuario, CDB Conn)
    {
        int puede = 0;
        string Query = "EXEC sp_Usuario_ValidaSucursalesAsignados @Opcion, @IdUsuario, @IdSucursal";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Opcion", 1);
        Conn.AgregarParametros("@IdUsuario", IdUsuario);
        Conn.AgregarParametros("@IdSucursal", IdSucursalPredeterminada);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdSucursal"))
        {
            puede = 1;
        }
        return puede;
    }

    public bool TieneSucursalAsignada(int IdUsuario, CDB Conn)
    {
        bool tieneSucursalAsignada = false;
        string Query = "EXEC sp_Usuario_Consultar_TieneSucursalAsignada @IdUsuario";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuario", IdUsuario);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("NoSucursalesAsignadas"))
        {
            if ((int)Registro.Get("NoSucursalesAsignadas") > 0)
            {
                tieneSucursalAsignada = true;
            }
        }
        return tieneSucursalAsignada;

    }


    public void DesactivarUsuario(CDB Conn)
    {
        string Query = "UPDATE Usuario SET Baja = @Baja WHERE IdUsuario=@IdUsuario ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuario", idusuario);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public static int ValidaExiste(string Usuario, CDB Conn)
    {
        int Contador = 0;
        string Query = "SELECT COUNT(IdUsuario) AS Contador FROM Usuario WHERE Usuario =@Usuario";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Usuario", Usuario);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("Contador"))
        {
            Contador = (int)Registro.Get("Contador");
        }
        return Contador;
    }
}