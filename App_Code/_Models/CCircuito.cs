using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class CCircuito
{
	
	private int idcircuito = 0;
	private string circuito = "";
    private string descripcion = "";
    private int idtablero = 0;
    private int idlinea = 0;
    private int idcategoria = 0;
    private string imagen = "";
	private bool baja = false;

	public int IdCircuito
	{
		get
		{
			return idcircuito;
		}
		set
		{
			idcircuito = value;
		}
	}

	public string Circuito
	{
		get
		{
			return circuito;
		}
		set
		{
			circuito = value;
		}
	}

    public string Descripcion
    {
        get
        {
            return descripcion;
        }
        set
        {
            descripcion = value;
        }
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

    public int IdLinea
    {
        get
        {
            return idlinea;
        }
        set
        {
            idlinea = value;
        }
    }

    public int IdCategoria
    {
        get
        {
            return idcategoria;
        }
        set
        {
            idcategoria = value;
        }
    }

    public string Imagen
    {
        get
        {
            return imagen;
        }
        set
        {
            imagen = value;
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
        string Query = "UPDATE Circuito SET Baja = @Baja WHERE IdCircuito=@IdCircuito ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdCircuito", idcircuito);
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
                idcircuito = !(Datos["IdCircuito"] is DBNull) ? Convert.ToInt32(Datos["IdCircuito"]) : idcircuito;
                circuito = !(Datos["Circuito"] is DBNull) ? Convert.ToString(Datos["Circuito"]) : circuito;
                descripcion = !(Datos["Descripcion"] is DBNull) ? Convert.ToString(Datos["Descripcion"]) : descripcion;              
                idtablero = !(Datos["IdTablero"] is DBNull) ? Convert.ToInt32(Datos["IdTablero"]) : idtablero;
                idlinea = !(Datos["IdLinea"] is DBNull) ? Convert.ToInt32(Datos["IdLinea"]) : idlinea;
                idcategoria = !(Datos["IdCategoria"] is DBNull) ? Convert.ToInt32(Datos["IdCategoria"]) : idcategoria;
                imagen = !(Datos["Imagen"] is DBNull) ? Convert.ToString(Datos["Imagen"]) : imagen;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    public void Agregar(CDB Conn)
    {
        string Query = "EXEC SP_Circuito_AgregarCircuito @IdTablero, @Circuito, @Descripcion ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdTablero", idtablero);
        Conn.AgregarParametros("@Circuito", circuito);
        Conn.AgregarParametros("@Descripcion", descripcion);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }
    
    public static int ValidaExiste(int IdMedidor, string NumeroCircuito, CDB Conn)
    {
        int IdCircuito = 0;
        string Query = "SELECT IdCircuito FROM Circuito C LEFT JOIN Tablero T ON C.IdTablero=T.IdTablero WHERE IdMedidor=@IdMedidor AND C.Circuito=@NumeroCircuito ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdMedidor", IdMedidor);
        Conn.AgregarParametros("@NumeroCircuito", NumeroCircuito);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdCircuito"))
        {
            IdCircuito = (int)Registro.Get("IdCircuito");
        }
        return IdCircuito;
    }

    public static int ValidaExisteEditar(int IdCircuito, string NumeroCircuito, int IdTablero, int IdMedidor, CDB Conn)
    {
        int Id = 0;
        string Query = "SELECT IdCircuito FROM Circuito C LEFT JOIN Tablero T ON C.IdTablero=T.IdTablero WHERE IdMedidor=@IdMedidor AND C.Circuito=@NumeroCircuito AND IdCircuito<>@IdCircuito";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@NumeroCircuito", NumeroCircuito);
        Conn.AgregarParametros("@IdTablero", IdTablero);
        Conn.AgregarParametros("@IdCircuito", IdCircuito);
        Conn.AgregarParametros("@IdMedidor", IdMedidor);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdCircuito"))
        {
            Id = (int)Registro.Get("IdCircuito");
        }
        return Id;
    }

    public void Editar(CDB Conn)
    {
        string Query = "UPDATE Circuito SET IdTablero=@IdTablero, Circuito=@Circuito, Descripcion=@Descripcion WHERE IdCircuito= @IdCircuito " +
            "SELECT * FROM Circuito WHERE IdCircuito = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdCircuito", IdCircuito);
        Conn.AgregarParametros("@IdTablero", idtablero);
        Conn.AgregarParametros("@Circuito", circuito);
        Conn.AgregarParametros("@Descripcion", descripcion);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    // Cargar Cliente
    public void Obtener(CDB Conn)
    {
        if (idcircuito != 0)
        {
            string Query = "SELECT * FROM Circuito WHERE IdCircuito = @IdCircuito";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdCircuito", idcircuito);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    public void EditarImagen(CDB Conn)
    {
        string Query = "UPDATE Circuito SET Imagen=@Imagen WHERE IdCircuito= @IdCircuito " +
            "SELECT * FROM Circuito WHERE IdCircuito = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdCircuito", idcircuito);
        Conn.AgregarParametros("@Imagen", imagen);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    private void LimpiarPropiedades()
    {
        idcircuito = 0;
        idtablero = 0;
        idlinea = 0;
        idcategoria = 0;
        circuito ="";
        descripcion = "";
        imagen = "";
        baja = false;
    }

	public CCircuito()
	{

	}
}