using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public class CTipoProblema
{
    private int idpermiso = 0;
    private int idTipoproblema = 0;
    private string nombretipoproblema = "";
    private bool baja = false;

    public int IdTipoProblema
    {
        get
        {
            return idTipoproblema;
        }
        set
        {
            idTipoproblema = value;
        }
    }

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

    public string NombreTipoProblema
    {
        get
        {
            return nombretipoproblema;
        }
        set
        {
            nombretipoproblema = value;
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
		string query = "SELECT * FROM Permiso WHERE IdPermiso = @IdPermiso";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdPermiso", idpermiso);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}
	
	public void Agregar(CDB conn)
	{
		string query = "INSERT INTO TipoProblema (TipoProblema,Baja) VALUES (@NombreTipoProblema,@Baja) " +
            "SELECT * FROM TipoProblema WHERE IdTipoProblema = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@NombreTipoProblema", nombretipoproblema);
        conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

    public static int ValidaExisteEditarTipoProblema(int IdTipoProblema, string NombreTipoProblema, CDB Conn)
    {
        int Id = 0;
        string Query = "SELECT IdTipoProblema FROM TipoProblema WHERE TipoProblema COLLATE Latin1_general_CI_AI like '%'+@NombreTipoProblema + '%' AND IdTipoProblema<>@IdTipoProblema";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdTipoProblema", IdTipoProblema);
        Conn.AgregarParametros("@NombreTipoProblema", NombreTipoProblema);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdTipoProblema"))
        {
            Id = (int)Registro.Get("IdTipoProblema");
        }
        return Id;
    }

    public static int ValidaExiste(string NombreTipoProblema, CDB Conn)
    {
        int IdTipoProblema = 0;
        string Query = "SELECT IdTipoProblema FROM TipoProblema WHERE TipoProblema COLLATE Latin1_general_CI_AI LIKE '%' + @NombreTipoProblema + '%'";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@NombreTipoProblema", NombreTipoProblema);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdTipoProblema"))
        {
            IdTipoProblema = (int)Registro.Get("IdTipoProblema");
        }
        return IdTipoProblema;
    }

    public void Editar(CDB conn)
	{
		string query = "UPDATE TipoProblema SET TipoProblema = @NombreTipoProblema WHERE IdTipoProblema = @IdTipoProblema " +
            "SELECT * FROM TipoProblema WHERE IdTipoProblema = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@NombreTipoProblema", nombretipoproblema);
        conn.AgregarParametros("@IdTipoProblema", IdTipoProblema);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

    public void Desactivar(CDB conn)
    {
        string query = "UPDATE TipoProblema SET Baja = @Baja WHERE IdTipoProblema = @IdTipoProblema " +
            "SELECT * FROM TipoProblema WHERE IdTipoProblema = SCOPE_IDENTITY()";
        conn.DefinirQuery(query);
        conn.AgregarParametros("@Baja", baja);
        conn.AgregarParametros("@IdTipoProblema", IdTipoProblema);
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
                IdTipoProblema = !(Datos["IdTipoProblema"] is DBNull) ? Convert.ToInt32(Datos["IdTipoProblema"]) : 0;
                nombretipoproblema = !(Datos["TipoProblema"] is DBNull) ? Convert.ToString(Datos["TipoProblema"]) : "";
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
		}
	}

    public static JObject ObtenerJsonTipoProblemas(JObject esteObjeto)
    {
        CDB conn = new CDB();
        string spTipoProblema = "EXEC sp_TipoProblema_Consultar @Opcion";
        conn.DefinirQuery(spTipoProblema);
        conn.AgregarParametros("@Opcion", 1);
        SqlDataReader dr = conn.Ejecutar();
        JArray arrayTipoProblema = new JArray();
        while (dr.Read())
        {
            JObject TipoProblema = new JObject();
            TipoProblema.Add(new JProperty("Valor", Convert.ToInt32(dr["Valor"].ToString())));
            TipoProblema.Add(new JProperty("Etiqueta", dr["Etiqueta"].ToString()));
            arrayTipoProblema.Add(TipoProblema);
        }
        dr.Close();



        esteObjeto.Add(new JProperty("TipoProblemas", arrayTipoProblema));
        return esteObjeto;
    }
}