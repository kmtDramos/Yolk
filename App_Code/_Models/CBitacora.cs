using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class CBitacora
{
    int idbitacora = 0; 
    int idreporte = 0;
    DateTime fecha = new DateTime(1900, 1, 1);
    int idusuarioalta = 0;
    string bitacora ="";

    public int IdBitacora
    {
        get
        {
            return idbitacora;
        }
        set
        {
            idbitacora = value;
        }
    }

    public int IdReporte
    {
        get
        {
            return idreporte;
        }
        set
        {
            idreporte = value;
        }
    }

    public int IdUsuarioAlta
    {
        get
        {
            return idusuarioalta;
        }
        set
        {
            idusuarioalta = value;
        }
    }

    public string Bitacora
    {
        get
        {
            return bitacora;
        }
        set
        {
            bitacora = value;
        }
    }

    public DateTime Fecha
    {
        get
        {
            return fecha;
        }
        set
        {
            fecha = value;
        }
    }

    // Cargar Usuario
    public void Obtener(CDB Conn)
    {
        if (idbitacora != 0)
        {
            string Query = "SELECT * FROM Bitacora WHERE IdBitacora = @IdBitacora";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdBitacora", idbitacora);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO Bitacora (IdReporte, Bitacora, Fecha, IdUsuarioAlta) VALUES (@IdReporte, @Bitacora, @Fecha, @IdUsuarioAlta)" +
            "SELECT * FROM Bitacora WHERE IdBitacora = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdReporte", idreporte);
        Conn.AgregarParametros("@Bitacora", bitacora);
        Conn.AgregarParametros("@Fecha", fecha);
        Conn.AgregarParametros("@IdUsuarioAlta", idusuarioalta);        
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
                idbitacora = !(Datos["IdBitacora"] is DBNull) ? Convert.ToInt32(Datos["IdBitacora"]) : idbitacora;
                bitacora = !(Datos["Bitacora"] is DBNull) ? Convert.ToString(Datos["Bitacora"]) : bitacora;
                fecha = !(Datos["Fecha"] is DBNull) ? Convert.ToDateTime(Datos["Fecha"]) : fecha;
                idusuarioalta = !(Datos["IdUsuarioAlta"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioAlta"]) : idusuarioalta;

            }
        }
    }

    // Limpiar valores de instancia
    private void LimpiarPropiedades()
    {
        idbitacora = 0;
        bitacora = "";
        fecha = new DateTime();
        idusuarioalta = 0;
    }

}