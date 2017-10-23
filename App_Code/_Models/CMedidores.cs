using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class CMedidores
{
    private int idmedidor = 0;
    private int idsucursal = 0;
    private int idcliente = 0;
    private string medidor = "";
    private bool baja = false;

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

    public int IdSucursal
    {
        get
        {
            return idsucursal;
        }
        set
        {
            idsucursal = value;
        }
    }

    public int IdCliente
    {
        get
        {
            return idcliente;
        }
        set
        {
            idcliente = value;
        }
    }

    public string Medidor
    {
        get
        {
            return medidor;
        }
        set
        {
            medidor = value;
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

    public CMedidores()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    // Cargar Medidor
    public void Obtener(CDB Conn)
    {
        if (idmedidor != 0)
        {
            string Query = "SELECT * FROM Medidor WHERE IdMedidor = @IdMedidor";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdMedidor", idmedidor);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Definir valores de instancia
    private void DefinirPropiedades(SqlDataReader Datos)
    {
        if (Datos.HasRows)
        {
            while (Datos.Read())
            {
                idmedidor = !(Datos["IdMedidor"] is DBNull) ? Convert.ToInt32(Datos["IdMedidor"]) : idmedidor;
                idsucursal = !(Datos["IdSucursal"] is DBNull) ? Convert.ToInt32(Datos["IdSucursal"]) : idsucursal;
                idcliente = !(Datos["IdCliente"] is DBNull) ? Convert.ToInt32(Datos["IdCliente"]) : idcliente;
                medidor = !(Datos["Medidor"] is DBNull) ? Convert.ToString(Datos["Medidor"]) : medidor;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO Medidor (Medidor,IdSucursal, IdCliente,Baja) VALUES (@Medidor,@IdSucursal, @IdCliente, @Baja)" +
            "SELECT * FROM Medidor WHERE IdMedidor = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Medidor", medidor);
        Conn.AgregarParametros("@IdSucursal", idsucursal);
        Conn.AgregarParametros("@IdCliente", idcliente);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE Medidor SET Baja = @Baja WHERE IdMedidor=@IdMedidor ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdMedidor", idmedidor);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Editar(CDB Conn)
    {
        string Query = "UPDATE Medidor SET Medidor=@Medidor WHERE IdMedidor= @IdMedidor " +
            "SELECT * FROM Medidor WHERE IdMedidor = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdMedidor", idmedidor);
        Conn.AgregarParametros("@Medidor", medidor);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    // Limpiar valores de instancia
    private void LimpiarPropiedades()
    {
        idmedidor = 0;
        idsucursal = 0;
        idcliente = 0;
        medidor = "";
        baja = false;
    }
}