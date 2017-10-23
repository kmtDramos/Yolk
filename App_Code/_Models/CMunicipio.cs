using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


public class CMunicipio
{

	private int idmunicipio = 0;
	private int idestado = 0;
	private int idpais = 0;
	private string municipio = "";
	private string estado = "";
	private string pais = "";
	private string comando = "";
	private string nombrepermiso = "";
	private int baja = 0;

	public int IdMunicipio
	{
		get
		{
			return idmunicipio;
		}
		set
		{
			idmunicipio = value;
		}
	}
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

	public string Municipio
	{
		get
		{
			return municipio;
		}
		set
		{
			municipio = value;
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
		string query = "SELECT * FROM Municipio WHERE IdMunicipio = @IdMunicipio";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdMunicipio", idmunicipio);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	public void Agregar(CDB conn)
	{
		string query = "INSERT INTO Municipio (Municipio,IdEstado,IdPais,Baja) VALUES (@Municipio,@IdEstado,@IdPais,@Baja) " +
			"SELECT * FROM Municipio WHERE IdMunicipio = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@Municipio", municipio);
		conn.AgregarParametros("@IdEstado", IdEstado);
		conn.AgregarParametros("@IdPais", IdPais);
		conn.AgregarParametros("@Baja", baja);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	public static int ValidaExisteEditaMunicipio(int IdMunicipio, string Municipio, int IdEstado, int IdPais, CDB Conn)
	{
		int Id = 0;
		string Query = "SELECT IdMunicipio FROM Municipio WHERE Municipio=@Municipio AND IdEstado=@IdEstado AND IdPais=@IdPais AND IdMunicipio<>@IdMunicipio ";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@IdMunicipio", IdMunicipio);
		Conn.AgregarParametros("@Municipio", Municipio);
		Conn.AgregarParametros("@IdEstado", IdEstado);
		Conn.AgregarParametros("@IdPais", IdPais);
		CObjeto Registro = Conn.ObtenerRegistro();
		if (Registro.Exist("IdMunicipio"))
		{
			Id = (int)Registro.Get("IdMunicipio");
		}
		return Id;
	}

	public static int ValidaExiste(int IdPais, int IdEstado, string Municipio, CDB Conn)
	{
		int IdMunicipio = 0;
		string Query = "SELECT IdMunicipio FROM Municipio WHERE IdEstado=@IdEstado AND IdPais=@IdPais AND Municipio LIKE '%' + @Municipio + '%' ";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@IdMunicipio", IdMunicipio);
		Conn.AgregarParametros("@Municipio", Municipio);
		Conn.AgregarParametros("@IdEstado", IdEstado);
		Conn.AgregarParametros("@IdPais", IdPais);
		CObjeto Registro = Conn.ObtenerRegistro();
		if (Registro.Exist("IdMunicipio"))
		{
			IdMunicipio = (int)Registro.Get("IdMunicipio");
		}
		return IdMunicipio;
	}

	public void Editar(CDB conn)
	{
		string query = "UPDATE Municipio SET Municipio = @Municipio, IdEstado = @IdEstado, IdPais = @IdPais WHERE IdMunicipio = @IdMunicipio " +
			   "SELECT * FROM Municipio WHERE IdMunicipio = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdMunicipio", IdMunicipio);
		conn.AgregarParametros("@Municipio", Municipio);
		conn.AgregarParametros("@IdEstado", IdEstado);
		conn.AgregarParametros("@IdPais", IdPais);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	public void Desactivar(CDB conn)
	{
		string query = "UPDATE Municipio SET Baja = @Baja WHERE IdMunicipio = @IdMunicipio " +
			"SELECT * FROM Municipio WHERE Municipio = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@Baja", baja);
		conn.AgregarParametros("@IdMunicipio", IdMunicipio);
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
				idestado = !(Datos["IdEstado"] is DBNull) ? Convert.ToInt32(Datos["IdEstado"]) : 0;
				municipio = !(Datos["Municipio"] is DBNull) ? Convert.ToString(Datos["Municipio"]) : "";
			}
		}
	}

}