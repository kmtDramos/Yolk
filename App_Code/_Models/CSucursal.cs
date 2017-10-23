using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class CSucursal
{

	private int idsucursal = 0;
    private string sucursal = "";
    private int idcliente= 0;
    private int idmunicipio = 0;
    private int idregion = 0;
    private bool baja = false;

    public int IdSucursal {
        get {
            return idsucursal;        
        }
        set {
            idsucursal = value;
        }
    }

    public string Sucursal
    {
        get
        {
            return sucursal;
        }
        set
        {
            sucursal = value;
        }
    }

    public int IdCliente {
        get {
            return idcliente;        
        }
        set {
            idcliente = value;
        }
    }

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

    // Constructor
    public CSucursal()
	{

	}

    // Cargar Cliente
    public void Obtener(CDB Conn)
    {
        if (idsucursal != 0)
        {
            string Query = "SELECT * FROM Sucursal WHERE IdSucursal = @IdSucursal";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdSucursal", idsucursal);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO Sucursal (Sucursal,IdCliente,IdMunicipio,IdRegion,Baja) VALUES (@Sucursal,@IdCliente,@IdMunicipio, @IdRegion, @Baja)" +
            "SELECT * FROM Sucursal WHERE IdSucursal = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Sucursal", sucursal);
        Conn.AgregarParametros("@IdCliente", idcliente);
        Conn.AgregarParametros("@IdMunicipio", idmunicipio);
        Conn.AgregarParametros("@IdRegion", idregion);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE Sucursal SET Baja = @Baja WHERE IdSucursal=@IdSucursal ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdSucursal", idsucursal);
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
                idsucursal = !(Datos["IdSucursal"] is DBNull) ? Convert.ToInt32(Datos["IdSucursal"]) : idsucursal;
                sucursal = !(Datos["Sucursal"] is DBNull) ? Convert.ToString(Datos["Sucursal"]) : sucursal;
                idcliente = !(Datos["IdCliente"] is DBNull) ? Convert.ToInt32(Datos["IdCliente"]) : idcliente;
                idmunicipio = !(Datos["IdMunicipio"] is DBNull) ? Convert.ToInt32(Datos["IdMunicipio"]) : idmunicipio;
                idregion = !(Datos["IdRegion"] is DBNull) ? Convert.ToInt32(Datos["IdRegion"]) : idregion;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;

            }
        }
    }

    // Editar registro
    public void Editar(CDB Conn)
    {
        if (idsucursal != 0)
        {
            string Query = "UPDATE Sucursal SET Sucursal=@Sucursal,IdMunicipio=@IdMunicipio, IdRegion=@IdRegion WHERE IdSucursal=@IdSucursal " +
            "SELECT * FROM Sucursal WHERE IdSucursal = SCOPE_IDENTITY()";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdSucursal", idsucursal);
            Conn.AgregarParametros("@Sucursal", sucursal);
            Conn.AgregarParametros("@IdMunicipio", idmunicipio);
            Conn.AgregarParametros("@IdRegion", idregion);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }


    // Limpiar valores de instancia
    private void LimpiarPropiedades()
    {
        idsucursal = 0;
        sucursal = "";
        idcliente = 0;
        idmunicipio = 0;
        idregion = 0; 
        baja = false;
    }

    public static int ValidaExiste(int IdCliente, int IdMunicipio, int IdRegion, string Sucursal, CDB Conn)
    {
        int Contador = 0;
        string Query = "SELECT COUNT(IdSucursal) AS Contador FROM Sucursal WHERE IdMunicipio=@IdMunicipio AND IdRegion=@IdRegion AND IdCliente=@IdCliente AND  Sucursal COLLATE Latin1_general_CI_AI LIKE '%' + @Sucursal + '%'";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdCliente", IdCliente);
        Conn.AgregarParametros("@IdMunicipio", IdMunicipio);
        Conn.AgregarParametros("@IdRegion", IdRegion);
        Conn.AgregarParametros("@Sucursal", Sucursal);
        CObjeto ValidaExiste = Conn.ObtenerRegistro();
        if (ValidaExiste.Exist("Contador"))
        {
            Contador = (int)ValidaExiste.Get("Contador");
        }
        return Contador;
    }

    public static int ValidaExisteEditar(int IdSucursal, int IdCliente, int IdMunicipio, int IdRegion, string Sucursal, CDB Conn)
    {
        int Contador = 0;
        string Query = "SELECT COUNT(IdSucursal) AS Contador FROM Sucursal WHERE IdMunicipio=@IdMunicipio AND IdRegion=@IdRegion AND IdCliente=@IdCliente AND  Sucursal COLLATE Latin1_general_CI_AI LIKE '%' + @Sucursal + '%' AND IdSucursal<>@IdSucursal";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdSucursal", IdSucursal);
        Conn.AgregarParametros("@IdCliente", IdCliente);
        Conn.AgregarParametros("@IdMunicipio", IdMunicipio);
        Conn.AgregarParametros("@IdRegion", IdRegion);
        Conn.AgregarParametros("@Sucursal", Sucursal);
        CObjeto ValidaExiste = Conn.ObtenerRegistro();
        if (ValidaExiste.Exist("Contador"))
        {
            Contador = (int)ValidaExiste.Get("Contador");
        }
        return Contador;
    }

}