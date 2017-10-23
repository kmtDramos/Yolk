using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


public class CEstado
{

	private int idestado = 0;
    private int idpais = 0;
	private string estado = "";
	private string pais = "";
    private string comando = "";
    private string nombrepermiso = "";
    private int baja = 0;

    public int IdEstado
	{
		get
		{
			return idestado;
		}
		set
		{
			idestado = value;
		}
	}

    public int IdPais
    {
        get
        {
            return idpais;
        }
        set
        {
            idpais = value;
        }
    }

    public string Estado
	{
		get
		{
			return estado;
		}
		set
		{
			estado = value;
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

    public string Pais
	{
		get
		{
			return pais;
		}
		set
		{
			pais = value;
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
		string query = "SELECT * FROM Estado WHERE IdEstado = @IdEstado";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdEstado", idestado);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}
	
	public void Agregar(CDB conn)
	{
		string query = "INSERT INTO Estado (Estado,IdPais,Baja) VALUES (@Estado,@IdPais,@Baja) " +
            "SELECT * FROM Estado WHERE IdEstado = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@Estado", estado);
        conn.AgregarParametros("@IdPais", IdPais);
        conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

    public static int ValidaExisteEditaEstado(int IdEstado, string Estado, int IdPais, CDB Conn)
    {
        int Id = 0;
        string Query = "SELECT IdEstado FROM Estado WHERE Estado=@Estado AND IdPais=@IdPais";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdEstado", IdEstado);
        Conn.AgregarParametros("@Estado", Estado);
        Conn.AgregarParametros("@IdPais", IdPais);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdEstado"))
        {
            Id = (int)Registro.Get("IdEstado");
        }
        return Id;
    }

    public static int ValidaExiste(int IdPais, string Estado, CDB Conn)
    {
        int IdEstado = 0;
        string Query = "SELECT IdEstado FROM Estado WHERE IdPais=@IdPais AND Estado LIKE '%' + @Estado + '%' ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPais", IdPais);
        Conn.AgregarParametros("@Estado", Estado);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdEstado"))
        {
            IdEstado = (int)Registro.Get("IdEstado");
        }
        return IdEstado;
    }

    public void Editar(CDB conn)
    {
        string query = "UPDATE Estado SET Estado = @Estado,IdPais = @IdPais WHERE IdEstado = @IdEstado " +
               "SELECT * FROM Estado WHERE IdEstado = SCOPE_IDENTITY()";
        conn.DefinirQuery(query);
        conn.AgregarParametros("@IdEstado", IdEstado);
        conn.AgregarParametros("@Estado", Estado);
        conn.AgregarParametros("@IdPais", IdPais);
        SqlDataReader Datos = conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Desactivar(CDB conn)
    {
        string query = "UPDATE Estado SET Baja = @Baja WHERE IdEstado = @IdEstado " +
            "SELECT * FROM Estado WHERE Estado = SCOPE_IDENTITY()";
        conn.DefinirQuery(query);
        conn.AgregarParametros("@Baja", baja);
        conn.AgregarParametros("@IdEstado", IdEstado);
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
				idpais = !(Datos["IdPais"] is DBNull) ? Convert.ToInt32(Datos["IdPais"]) : 0;
				estado = !(Datos["Estado"] is DBNull) ? Convert.ToString(Datos["Estado"]) : "";
			}
		}
	}

}