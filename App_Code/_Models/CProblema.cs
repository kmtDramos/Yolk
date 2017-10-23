using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public class CProblema
{

    private int idproblema = 0;
    private int idtipoproblema = 0;
    private string problema = "";
    private bool baja = false;

    public int IdProblema
    {
        get
        {
            return idproblema;
        }
        set
        {
            idproblema = value;
        }
    }

    public int IdTipoProblema
    {
        get
        {
            return idtipoproblema;
        }
        set
        {
            idtipoproblema = value;
        }
    }

    public string Problema
    {
        get
        {
            return problema;
        }
        set
        {
            problema = value;
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

    //Metodos

    public void Obtener(CDB conn)
	{
		string query = "SELECT * FROM Problema WHERE IdProblema = @IdProblema";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdProblema", idproblema);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}
	
	public void Agregar(CDB conn)
	{
		string query = "INSERT INTO Problema (IdTipoProblema, Problema, Baja) VALUES (@IdTipoProblema, @Problema,@Baja) " +
            "SELECT * FROM Problema WHERE IdProblema = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
        conn.AgregarParametros("@IdTipoProblema", idtipoproblema); 
        conn.AgregarParametros("@Problema", problema);
        conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

    public static int ValidaExisteEditarProblema(int IdProblema, int IdTipoProblema, string Problema, CDB Conn)
    {
        int Contador = 0;
        string Query = "SELECT COUNT(IdProblema) AS Contador FROM Problema WHERE IdTipoProblema = @IdTipoProblema AND Problema COLLATE Latin1_general_CI_AI like '%'+@Problema + '%' AND IdProblema<>@IdProblema";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdTipoProblema", IdTipoProblema);
        Conn.AgregarParametros("@IdProblema", IdProblema);        
        Conn.AgregarParametros("@Problema", Problema);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("Contador"))
        {
            Contador = (int)Registro.Get("Contador");
        }
        return Contador;
    }

    public static int ValidaExiste(int IdTipoProblema, string Problema, CDB Conn)
    {
        int Contador = 0;
        string Query = "SELECT COUNT(IdProblema) AS Contador FROM Problema WHERE IdTipoProblema=@IdTipoProblema AND Problema COLLATE Latin1_general_CI_AI LIKE '%' + @Problema + '%'";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdTipoProblema", IdTipoProblema);
        Conn.AgregarParametros("@Problema", Problema);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("Contador"))
        {
            Contador = (int)Registro.Get("Contador");
        }
        return Contador;
    }

    public void Editar(CDB conn)
	{
		string query = "UPDATE Problema SET IdTipoProblema=@IdTipoProblema, Problema = @Problema WHERE IdProblema = @IdProblema " +
            "SELECT * FROM Problema WHERE IdProblema = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
        conn.AgregarParametros("@IdTipoProblema", idtipoproblema);
		conn.AgregarParametros("@Problema", problema);
        conn.AgregarParametros("@IdProblema", IdProblema);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

    public void Desactivar(CDB conn)
    {
        string query = "UPDATE Problema SET Baja = @Baja WHERE IdProblema = @IdProblema " +
            "SELECT * FROM Problema WHERE IdProblema = SCOPE_IDENTITY()";
        conn.DefinirQuery(query);
        conn.AgregarParametros("@Baja", baja);
        conn.AgregarParametros("@IdProblema", idproblema);
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
                idproblema = !(Datos["IdProblema"] is DBNull) ? Convert.ToInt32(Datos["IdProblema"]) : idproblema;
                idtipoproblema = !(Datos["IdTipoProblema"] is DBNull) ? Convert.ToInt32(Datos["IdTipoProblema"]) : idtipoproblema;
                problema = !(Datos["Problema"] is DBNull) ? Convert.ToString(Datos["Problema"]) : problema;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
		}
	}

    public static JObject ObtenerJsonProblemas(JObject esteObjeto)
    {
        CDB conn = new CDB();
        string spProblema = "EXEC sp_Problema_Consultar @Opcion, @IdTipoProblema";
        conn.DefinirQuery(spProblema);
        conn.AgregarParametros("@Opcion", 1);
        conn.AgregarParametros("@IdTipoProblema", Convert.ToInt32(esteObjeto.Property("IdTipoProblema").Value.ToString()));
        SqlDataReader dr = conn.Ejecutar();
        JArray arrayProblema = new JArray();
        while (dr.Read())
        {
            JObject Problema = new JObject();
            Problema.Add(new JProperty("Valor", Convert.ToInt32(dr["Valor"].ToString())));
            Problema.Add(new JProperty("Etiqueta", dr["Etiqueta"].ToString()));
            arrayProblema.Add(Problema);
        }
        dr.Close();
        esteObjeto.Add(new JProperty("Problemas", arrayProblema));
        return esteObjeto;
    }

}