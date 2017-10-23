using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public class CPermiso
{

	private int idpermiso = 0;
	private string permiso = "";
	private string pantalla = "";
    private string comando = "";
    private string nombrepermiso = "";
    private int baja = 0;

    public int IdPermiso
	{
		get
		{
			return idpermiso;
		}
		set
		{
			idpermiso = value;
		}
	}

	public string Permiso
	{
		get
		{
			return permiso;
		}
		set
		{
			permiso = value;
		}
	}

    public string Comando
    {
        get
        {
            return comando;
        }
        set
        {
            comando = value;
        }
    }

    public string NombrePermiso
    {
        get
        {
            return nombrepermiso;
        }
        set
        {
            nombrepermiso = value;
        }
    }

    public string Pantalla
	{
		get
		{
			return pantalla;
		}
		set
		{
			pantalla = value;
		}
	}

    public int Baja
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

    //Metodos

    public void Obtener(CDB conn)
	{
		string query = "SELECT * FROM Permiso WHERE IdPermiso = @IdPermiso";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdPermiso", idpermiso);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}
	
	public void Agregar(CDB conn)
	{
		string query = "INSERT INTO Permiso (Permiso,Comando,Pantalla,Baja) VALUES (@Permiso,@Comando ,@Pantalla,@Baja) " +
            "SELECT * FROM Permiso WHERE IdPermiso = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@Permiso", permiso);
        conn.AgregarParametros("@Comando", comando);
        conn.AgregarParametros("@Pantalla", pantalla);
        conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

    public static int ValidaExisteEditarComando(int IdPermiso, string NombrePermiso, string Comando, string Pantalla, CDB Conn)
    {
        int Id = 0;
        string Query = "SELECT IdPermiso FROM Permiso WHERE Comando=@Comando AND IdPermiso<>@IdPermiso";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPermiso", IdPermiso);
        Conn.AgregarParametros("@NombrePermiso", NombrePermiso);
        Conn.AgregarParametros("@Comando", Comando);
        Conn.AgregarParametros("@Pantalla", Pantalla);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdPermiso"))
        {
            Id = (int)Registro.Get("IdPermiso");
        }
        return Id;
    }

    public static int ValidaExisteEditarNombrePermiso(int IdPermiso, string NombrePermiso, string Comando, string Pantalla, CDB Conn)
    {
        int Id = 0;
        string Query = "SELECT IdPermiso FROM Permiso WHERE Permiso=@NombrePermiso AND IdPermiso<>@IdPermiso";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPermiso", IdPermiso);
        Conn.AgregarParametros("@NombrePermiso", NombrePermiso);
        Conn.AgregarParametros("@Comando", Comando);
        Conn.AgregarParametros("@Pantalla", Pantalla);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdPermiso"))
        {
            Id = (int)Registro.Get("IdPermiso");
        }
        return Id;
    }

    public static int ValidaExiste(string NombrePermiso, string Comando, string Pantalla, CDB Conn)
    {
        int IdPermiso = 0;
        string Query = "SELECT IdPermiso FROM Permiso WHERE Permiso=@NombrePermiso or Comando=@Comando ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@NombrePermiso", NombrePermiso);
        Conn.AgregarParametros("@Comando", Comando);
        Conn.AgregarParametros("@Pantalla", Pantalla);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdPermiso"))
        {
            IdPermiso = (int)Registro.Get("IdPermiso");
        }
        return IdPermiso;
    }

    public void Editar(CDB conn)
	{
		string query = "UPDATE Permiso SET Permiso = @Permiso,Comando = @Comando ,Pantalla = @Pantalla WHERE IdPermiso = @IdPermiso " +
            "SELECT * FROM Permiso WHERE IdPermiso = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@Permiso", permiso);
        conn.AgregarParametros("@Comando", comando);
        conn.AgregarParametros("@Pantalla", pantalla);
		conn.AgregarParametros("@IdPermiso", idpermiso);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

    public void Desactivar(CDB conn)
    {
        string query = "UPDATE Permiso SET Baja = @Baja WHERE IdPermiso = @IdPermiso " +
            "SELECT * FROM Permiso WHERE IdPermiso = SCOPE_IDENTITY()";
        conn.DefinirQuery(query);
        conn.AgregarParametros("@Baja", baja);
        conn.AgregarParametros("@IdPermiso", idpermiso);
        SqlDataReader Datos = conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    private void DefinirPropiedades(SqlDataReader Datos)
	{
		if (Datos.HasRows)
		{
			while (Datos.Read())
			{
				idpermiso = !(Datos["IdPermiso"] is DBNull) ? Convert.ToInt32(Datos["IdPermiso"]) : 0;
				permiso = !(Datos["Permiso"] is DBNull) ? Convert.ToString(Datos["Permiso"]) : "";
				pantalla = !(Datos["Pantalla"] is DBNull) ? Convert.ToString(Datos["Pantalla"]) : "";
			}
		}
	}

    public static JObject ObtenerJsonPermisos(JObject esteObjeto)
    {
        CDB conn = new CDB();
        string spPermiso = "EXEC SP_Permiso_Consultar @Opcion";
        conn.DefinirQuery(spPermiso);
        conn.AgregarParametros("@Opcion", 1);
        SqlDataReader dr = conn.Ejecutar();
        JArray arrayPermiso = new JArray();
        while (dr.Read())
        {
            JObject Permiso = new JObject();
            Permiso.Add(new JProperty("Valor", Convert.ToInt32(dr["Valor"].ToString())));
            Permiso.Add(new JProperty("Etiqueta", dr["Etiqueta"].ToString()));
            arrayPermiso.Add(Permiso);
        }
        dr.Close();
        esteObjeto.Add(new JProperty("Permisos", arrayPermiso));
        return esteObjeto;
    }

}