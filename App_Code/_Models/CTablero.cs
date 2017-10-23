using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class CTablero
{

    private int idtablero = 0;
    private int idmedidor = 0;
    private string tablero = "";
    private bool baja = false;

	public CTablero()
	{

	}

    public int IdTablero
    {
        get
        {
            return idtablero;
        }
        set
        {
            idtablero = value;
        }
    }

    public int IdMedidor
    {
        get
        {
            return idmedidor;
        }
        set
        {
            idmedidor = value;
        }
    }

    public string Tablero
    {
        get
        {
            return tablero;
        }
        set
        {
            tablero = value;
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

    // Cargar Tablero
    public void Obtener(CDB Conn)
    {
        if (idtablero != 0)
        {
            string Query = "SELECT * FROM Tablero WHERE IdTablero = @IdTablero";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdTablero", idtablero);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO Tablero (IdMedidor, Tablero,Baja) VALUES (@IdMedidor,@Tablero,@Baja)" +
            "SELECT * FROM Tablero WHERE IdTablero = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdMedidor", idmedidor);
        Conn.AgregarParametros("@Tablero", tablero);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE Tablero SET Baja = @Baja WHERE IdTablero=@IdTablero ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdTablero", idtablero);
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
                idtablero = !(Datos["IdTablero"] is DBNull) ? Convert.ToInt32(Datos["IdTablero"]) : idtablero;
                idmedidor = !(Datos["IdMedidor"] is DBNull) ? Convert.ToInt32(Datos["IdMedidor"]) : idmedidor;
                tablero = !(Datos["Tablero"] is DBNull) ? Convert.ToString(Datos["Tablero"]) : tablero;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    // Editar registro
    public void Editar(CDB Conn)
    {
        if (idtablero != 0)
        {
            string Query = "UPDATE Tablero SET Tablero=@Tablero,Baja=@Baja WHERE IdTablero=@IdTablero " +
            "SELECT * FROM Tablero WHERE IdTablero = SCOPE_IDENTITY()";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdTablero", idtablero);
            Conn.AgregarParametros("@Tablero", tablero);
            Conn.AgregarParametros("@Baja", baja);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }


    // Limpiar valores de instancia
    private void LimpiarPropiedades()
    {
        idtablero = 0;
        idmedidor = 0;
        tablero = "";
        baja = false;
    }

}