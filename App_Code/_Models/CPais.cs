using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Summary description for CPais
/// </summary>

public class CPais

{

	private int idpais = 0;
	private string pais = "";
	private bool baja = false;

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

	public void Desactivar(CDB Conn)
	{
		string Query = "UPDATE Pais SET Baja = @Baja WHERE IdPais=@IdPais ";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@IdPais", idpais);
		Conn.AgregarParametros("@Baja", baja);
		SqlDataReader Datos = Conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	// Definir valores de instancia
	private void DefinirPropiedades(SqlDataReader Datos)
	{
		if (Datos.HasRows)
		{
			while (Datos.Read())
			{
				idpais = !(Datos["IdPais"] is DBNull) ? Convert.ToInt32(Datos["IdPais"]) : idpais;
				pais = !(Datos["Pais"] is DBNull) ? Convert.ToString(Datos["Pais"]) : pais;
				baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
			}
		}
	}

	public void Agregar(CDB Conn)
	{
		string Query = "EXEC SP_Pais_AgregarPais @Pais";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@Pais", pais);
		SqlDataReader Datos = Conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	public static int ValidaExiste(string Pais, CDB Conn)
	{

		int Contador = 0;
		string Query = "SELECT COUNT(Pais) AS Contador FROM Pais WHERE Pais COLLATE Latin1_general_CI_AI LIKE '%'+ @Pais + '%'";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@Pais", Pais);
		CObjeto Registro = Conn.ObtenerRegistro();
		if (Registro.Exist("Contador"))
		{
			Contador = (int)Registro.Get("Contador");
		}
		return Contador;
	}

	public static int ValidaExisteEditar(int IdPais, string Pais, CDB Conn)
	{
		int Id = 0;
		string Query = "SELECT IdPais FROM Pais WHERE Pais COLLATE Latin1_general_CI_AI like '%'+@Pais + '%' AND IdPais<>@IdPais";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@IdPais", IdPais);
		Conn.AgregarParametros("@Pais", Pais);
		CObjeto Registro = Conn.ObtenerRegistro();
		if (Registro.Exist("IdPais"))
		{
			Id = (int)Registro.Get("IdPais");
		}
		return Id;
	}

	public void Editar(CDB conn)
	{
		string query = "UPDATE Pais SET Pais = @Pais WHERE IdPais = @IdPais " +
			   "SELECT * FROM Pais WHERE IdPais = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdPais", idpais);
		conn.AgregarParametros("@Pais", pais);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}


	public void Obtener(CDB Conn)
	{
		if (idpais != 0)
		{
			string Query = "SELECT * FROM Pais WHERE IdPais = @IdPais";
			Conn.DefinirQuery(Query);
			Conn.AgregarParametros("@IdPais", idpais);
			SqlDataReader Datos = Conn.Ejecutar();
			DefinirPropiedades(Datos);
			Datos.Close();
		}
	}

	private void LimpiarPropiedades()
	{
		idpais = 0;
		pais = "";
		baja = false;
	}
}
