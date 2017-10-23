using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de CRegion
/// </summary>
public class CRegion
{
    private int idregion = 0;
    private string region = "";
    private bool baja = false;

    public int IdRegion
    {
        get
        {
            return idregion;
        }
        set
        {
            idregion = value;
        }
    }

    public string Region
    {
        get
        {
            return region;
        }
        set
        {
            region = value;
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

	public CRegion()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    // Cargar Region
    public void Obtener(CDB Conn)
    {
        if (idregion != 0)
        {
            string Query = "SELECT * FROM Region WHERE IdRegion = @IdRegion";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdRegion", idregion);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO Region (Region,Baja) VALUES (@Region,@Baja)" +
            "SELECT * FROM Region WHERE IdRegion = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Region", region);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE Region SET Baja = @Baja WHERE IdRegion=@IdRegion ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdRegion", idregion);
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
                idregion = !(Datos["IdRegion"] is DBNull) ? Convert.ToInt32(Datos["IdRegion"]) : idregion;
                region = !(Datos["Region"] is DBNull) ? Convert.ToString(Datos["Region"]) : region;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    // Editar registro
    public void Editar(CDB Conn)
    {
        if (idregion != 0)
        {
            string Query = "UPDATE Region SET Region=@Region,Baja=@Baja WHERE IdRegion=@IdRegion " +
            "SELECT * FROM Region WHERE IdRegion = SCOPE_IDENTITY()";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdRegion", idregion);
            Conn.AgregarParametros("@Region", region);
            Conn.AgregarParametros("@Baja", baja);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }


    // Limpiar valores de instancia
    private void LimpiarPropiedades()
    {
        idregion = 0;
        region = "";
        baja = false;
    }
}