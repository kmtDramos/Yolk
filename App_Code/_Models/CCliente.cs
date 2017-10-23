using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class CCliente
{
    private int idcliente = 0;
    private string cliente = "";
    private int idmunicipio= 0;
    private string logo = "";
    private bool baja = false;

    public int IdCliente {
        get {
            return idcliente;        
        }
        set {
            idcliente = value;
        }
    }

    public string Cliente
    {
        get
        {
            return cliente;
        }
        set
        {
            cliente = value;
        }
    }

    public string Logo
    {
        get
        {
            return logo;
        }
        set
        {
            logo = value;
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

    // Constructor
    public CCliente()
	{

	}

    // Cargar Cliente
    public void Obtener(CDB Conn)
    {
        if (idcliente != 0)
        {
            string Query = "SELECT * FROM Cliente WHERE IdCliente = @IdCliente";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdCliente", idcliente);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO Cliente (Cliente,IdMunicipio,Baja) VALUES (@Cliente,@IdMunicipio,@Baja)" +
            "SELECT * FROM Cliente WHERE IdCliente = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Cliente", cliente);
        Conn.AgregarParametros("@IdMunicipio", idmunicipio);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE Cliente SET Baja = @Baja WHERE IdCliente=@IdCliente ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdCliente", idcliente);
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
                idcliente = !(Datos["IdCliente"] is DBNull) ? Convert.ToInt32(Datos["IdCliente"]) : idcliente;
                cliente = !(Datos["Cliente"] is DBNull) ? Convert.ToString(Datos["Cliente"]) : cliente;
                idmunicipio = !(Datos["IdMunicipio"] is DBNull) ? Convert.ToInt32(Datos["IdMunicipio"]) : idmunicipio;                
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    public void Editar(CDB Conn)
    {
        string Query = "UPDATE Cliente SET Cliente=@Cliente, IdMunicipio=@IdMunicipio WHERE IdCliente= @IdCliente " +
            "SELECT * FROM Cliente WHERE IdCliente = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdCliente", idcliente);
        Conn.AgregarParametros("@Cliente", cliente);
        Conn.AgregarParametros("@IdMunicipio", idmunicipio);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void EditarLogo(CDB Conn)
    {
        string Query = "UPDATE Cliente SET Logo=@Logo WHERE IdCliente= @IdCliente " +
            "SELECT * FROM Cliente WHERE IdCliente = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdCliente", idcliente);
        Conn.AgregarParametros("@Logo", logo);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    // Limpiar valores de instancia
    private void LimpiarPropiedades()
    {
        idcliente = 0;
        cliente = "";
        logo = "";
        idmunicipio = 0;        
        baja = false;
    }

    public static int ValidaExiste(int IdMunicipio, string Cliente, CDB Conn)
    {
        int Contador = 0;
        string Query = "SELECT COUNT(IdCliente) AS Contador FROM Cliente WHERE IdMunicipio=@IdMunicipio AND Cliente COLLATE Latin1_general_CI_AI LIKE '%' + @Cliente + '%'";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdMunicipio", IdMunicipio);
        Conn.AgregarParametros("@Cliente", Cliente);
        CObjeto ValidaExiste = Conn.ObtenerRegistro();
        if (ValidaExiste.Exist("Contador"))
        {
            Contador = (int)ValidaExiste.Get("Contador");
        }
        return Contador;
    }

    public static int ValidaExisteEditar(int IdCliente, int IdMunicipio, string Cliente, CDB Conn)
    {
        int Contador = 0;
        string Query = "SELECT COUNT(IdCliente) AS Contador FROM Cliente WHERE IdMunicipio=@IdMunicipio AND Cliente COLLATE Latin1_general_CI_AI LIKE '%' + @Cliente + '%' AND IdCliente<>@IdCliente";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdCliente", IdCliente); 
        Conn.AgregarParametros("@IdMunicipio", IdMunicipio);
        Conn.AgregarParametros("@Cliente", Cliente);
        CObjeto ValidaExiste = Conn.ObtenerRegistro();
        if (ValidaExiste.Exist("Contador"))
        {
            Contador = (int)ValidaExiste.Get("Contador");
        }
        return Contador;
    }
}