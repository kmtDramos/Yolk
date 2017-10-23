using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class CProveedor
{
    private int idproveedor = 0;
    private string proveedor = "";
    private DateTime fechaalta = new DateTime(1900,1,1);
    private int idusuarioalta = 0;
	private bool baja = false;

	public int IdProveedor
	{
		get
		{
			return idproveedor;
		}
		set
		{
			idproveedor = value;
		}
	}

	public string Proveedor
	{
		get
		{
			return proveedor;
		}
		set
		{
			proveedor = value;
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
		string Query = "UPDATE Proveedor SET Baja = @Baja WHERE IdProveedor=@IdProveedor ";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@IdProveedor", idproveedor);
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
				idproveedor = !(Datos["IdProveedor"] is DBNull) ? Convert.ToInt32(Datos["IdProveedor"]) : idproveedor;
				proveedor = !(Datos["Proveedor"] is DBNull) ? Convert.ToString(Datos["Proveedor"]) : proveedor;
				baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
			}
		}
	}

	public void Agregar(CDB Conn)
	{
		string Query = "EXEC SP_Proveedor_AgregarProveedor @Proveedor";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@Proveedor", proveedor);
		Conn.AgregarParametros("@Baja", baja);
		SqlDataReader Datos = Conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	public static int ValidaExiste(string Proveedor, CDB Conn)
	{

		int Contador = 0;
		string Query = "SELECT COUNT(Proveedor) AS Contador FROM Proveedor WHERE Proveedor COLLATE Latin1_general_CI_AI LIKE '%'+ @Proveedor + '%' ";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@Proveedor", Proveedor);
		CObjeto Registro = Conn.ObtenerRegistro();
		if (Registro.Exist("Contador"))
		{
			Contador = (int)Registro.Get("Contador");
		}
		return Contador;
	}

	public void Editar(CDB conn)
	{
		string query = "UPDATE Proveedor SET Proveedor = @Proveedor WHERE IdProveedor = @IdProveedor " +
			   "SELECT * FROM Proveedor WHERE IdProveedor = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdProveedor", idproveedor);
		conn.AgregarParametros("@Proveedor", proveedor);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	public static int ValidaExisteEditar(int IdProveedor, string Proveedor, CDB Conn)
	{
		int Id = 0;
		string Query = "SELECT IdProveedor FROM Proveedor WHERE Proveedor COLLATE Latin1_general_CI_AI like '%'+@Proveedor + '%' AND IdProveedor<>@IdProveedor";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@IdProveedor", IdProveedor);
		Conn.AgregarParametros("@Proveedor", Proveedor);
		CObjeto Registro = Conn.ObtenerRegistro();
		if (Registro.Exist("IdProveedor"))
		{
			Id = (int)Registro.Get("IdProveedor");
		}
		return Id;
	}

	public void Obtener(CDB Conn)
	{
		if (idproveedor != 0)
		{
			string Query = "SELECT * FROM Proveedor WHERE IdProveedor = @IdProveedor";
			Conn.DefinirQuery(Query);
			Conn.AgregarParametros("@IdProveedor", idproveedor);
			SqlDataReader Datos = Conn.Ejecutar();
			DefinirPropiedades(Datos);
			Datos.Close();
		}
	}

	private void LimpiarPropiedades()
	{
		idproveedor = 0;
		proveedor = "";
		baja = false;
	}

	public static JObject ObtenerJsonProveedores(JObject esteObjeto)
	{
		CDB conn = new CDB();
		string spProveedor = "EXEC sp_Proveedor_Consultar @Opcion, @IdProveedor";
		conn.DefinirQuery(spProveedor);
		conn.AgregarParametros("@Opcion", 1);
		conn.AgregarParametros("@IdProveedor", Convert.ToInt32(esteObjeto.Property("IdProveedor").Value.ToString()));
		SqlDataReader dr = conn.Ejecutar();
		JArray arrayProveedor = new JArray();

		while (dr.Read())
		{
			JObject Proveedor = new JObject();
			Proveedor.Add(new JProperty("Valor", Convert.ToInt32(dr["Valor"].ToString())));
			Proveedor.Add(new JProperty("Etiqueta", dr["Etiqueta"].ToString()));
			arrayProveedor.Add(Proveedor);
		}

		dr.Close();
		esteObjeto.Add(new JProperty("Proveedores", arrayProveedor));
		return esteObjeto;
	}

    public static JObject ObtenerJsonProveedoresTodos(JObject esteObjeto)
    {
        CDB conn = new CDB();
        string spProveedor = "EXEC sp_Proveedor_Consultar @Opcion, 0";
        conn.DefinirQuery(spProveedor);
        conn.AgregarParametros("@Opcion", 1);
        SqlDataReader dr = conn.Ejecutar();
        JArray arrayProveedor = new JArray();

        while (dr.Read())
        {
            JObject Proveedor = new JObject();
            Proveedor.Add(new JProperty("Valor", Convert.ToInt32(dr["Valor"].ToString())));
            Proveedor.Add(new JProperty("Etiqueta", dr["Etiqueta"].ToString()));
            arrayProveedor.Add(Proveedor);
        }

        dr.Close();
        esteObjeto.Add(new JProperty("Proveedores", arrayProveedor));
        return esteObjeto;
    }

}