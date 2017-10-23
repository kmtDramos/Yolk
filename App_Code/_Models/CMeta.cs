using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de CMeta
/// </summary>
public class CMeta
{
	public CMeta()
	{
	}

    private int idmeta = 0;   
    private decimal metakwh = 0;
    private decimal metahorasuso = 0;
    private decimal metaconsumo = 0;
    private int idcircuito = 0;
    private int mes = 0;
    private int anio = 0;

    public int IdMeta
    {
        get
        {
            return idmeta;
        }
        set
        {
            idmeta = value;
        }
    }

    public decimal MetaKwH
    {
        get
        {
            return metakwh;
        }
        set
        {
            metakwh = value;
        }
    }

    public decimal MetaHorasUso
    {
        get
        {
            return metahorasuso;
        }
        set
        {
            metahorasuso = value;
        }
    }

    public decimal MetaConsumo
    {
        get
        {
            return metaconsumo;
        }
        set
        {
            metaconsumo = value;
        }
    }

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

    public int Mes
    {
        get
        {
            return mes;
        }
        set
        {
            mes = value;
        }
    }

    public int Anio
    {
        get
        {
            return anio;
        }
        set
        {
            anio = value;
        }
    }

    // Definir valores de instancia
    private void DefinirPropiedades(SqlDataReader Datos)
    {
        if (Datos.HasRows)
        {
            while (Datos.Read())
            {
                idmeta = !(Datos["IdMeta"] is DBNull) ? Convert.ToInt32(Datos["IdMeta"]) : idmeta;                
                metakwh = !(Datos["MetaKwH"] is DBNull) ? Convert.ToDecimal(Datos["MetaKwH"]) : metakwh;
                metahorasuso = !(Datos["MetaHorasUso"] is DBNull) ? Convert.ToDecimal(Datos["MetaHorasUso"]) : metahorasuso;
                metaconsumo = !(Datos["MetaConsumo"] is DBNull) ? Convert.ToDecimal(Datos["MetaConsumo"]) : metaconsumo;
                idcircuito = !(Datos["IdCircuito"] is DBNull) ? Convert.ToInt32(Datos["IdCircuito"]) : idcircuito;
                mes = !(Datos["Mes"] is DBNull) ? Convert.ToInt32(Datos["Mes"]) : mes;
                anio = !(Datos["Anio"] is DBNull) ? Convert.ToInt32(Datos["Anio"]) : anio;
            }
        }
    }

    public void Agregar(CDB Conn)
    {
        string Query = "EXEC SP_Meta_Agregar @MetaKwH, @MetaHorasUso, @MetaConsumo, @IdCircuito, @Mes, @Anio ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@MetaKwH", metakwh);
        Conn.AgregarParametros("@MetaHorasUso", metahorasuso);
        Conn.AgregarParametros("@MetaConsumo", metaconsumo);
        Conn.AgregarParametros("@IdCircuito", idcircuito);
        Conn.AgregarParametros("@Mes", mes);
        Conn.AgregarParametros("@Anio", anio);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    //public static int ValidaExiste(string NumeroCircuito, int IdTablero, CDB Conn)
    //{
    //    int IdCircuito = 0;
    //    string Query = "SELECT IdCircuito FROM Circuito WHERE Circuito = @NumeroCircuito AND IdTablero = @IdTablero";
    //    Conn.DefinirQuery(Query);
    //    Conn.AgregarParametros("@NumeroCircuito", NumeroCircuito);
    //    Conn.AgregarParametros("@IdTablero", IdTablero);
    //    CObjeto Registro = Conn.ObtenerRegistro();
    //    if (Registro.Exist("IdCircuito"))
    //    {
    //        IdCircuito = (int)Registro.Get("IdCircuito");
    //    }
    //    return IdCircuito;
    //}

    //public static int ValidaExisteEditar(int IdCircuito, string NumeroCircuito, int IdTablero, CDB Conn)
    //{
    //    int Id = 0;
    //    string Query = "SELECT IdCircuito FROM Circuito WHERE Circuito = @NumeroCircuito AND IdTablero = @IdTablero AND IdCircuito<>@IdCircuito";
    //    Conn.DefinirQuery(Query);
    //    Conn.AgregarParametros("@NumeroCircuito", NumeroCircuito);
    //    Conn.AgregarParametros("@IdTablero", IdTablero);
    //    Conn.AgregarParametros("@IdCircuito", IdCircuito);
    //    CObjeto Registro = Conn.ObtenerRegistro();
    //    if (Registro.Exist("IdCircuito"))
    //    {
    //        Id = (int)Registro.Get("IdCircuito");
    //    }
    //    return Id;
    //}

    public void Editar(CDB Conn)
    {
        string Query = "UPDATE Meta SET MetaKwH=@MetaKwH, MetaHorasUso=@MetaHorasUso, MetaConsumo=@MetaConsumo WHERE IdMeta= @IdMeta " +
            "SELECT * FROM Meta WHERE IdMeta = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdMeta", idmeta);
		Conn.AgregarParametros("@MetaKwH", metakwh);
        Conn.AgregarParametros("@MetaHorasUso", metahorasuso);
        Conn.AgregarParametros("@MetaConsumo", metaconsumo);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    // Cargar Meta
    public void Obtener(CDB Conn)
    {
        if (idmeta != 0)
        {
            string Query = "SELECT * FROM Meta WHERE IdMeta = @IdMeta";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdMeta", idmeta);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    public static int ValidaExiste(int IdCircuito, int Mes, int Anio, CDB Conn)
    {
        int IdMeta = 0;
        string Query = "SELECT IdMeta FROM Meta WHERE IdCircuito=@IdCircuito AND Mes = @Mes AND Anio = @Anio";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdCircuito", IdCircuito);
        Conn.AgregarParametros("@Mes", Mes);
        Conn.AgregarParametros("@Anio", Anio);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdMeta"))
        {
            IdMeta = (int)Registro.Get("IdMeta");
        }
        return IdMeta;
    }

    private void LimpiarPropiedades()
    {
        metakwh = 0;
        metahorasuso = 0;
        metaconsumo = 0;
        idcircuito = 0;
        mes = 0;
        anio = 0;
    }

    public static int ValidaExisteReporte(int IdCircuito, string FechaInicio, string FechaFin, CDB Conn)
    {
        int Contador = 0;
        string Query = "EXEC sp_Meta_Consultar @Opcion, @IdCircuito, @FechaInicio, @FechaFin  ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Opcion", 1);
        Conn.AgregarParametros("@IdCircuito", IdCircuito);
        Conn.AgregarParametros("@FechaInicio", FechaInicio);
        Conn.AgregarParametros("@FechaFin", FechaFin);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("ContadorMeta"))
        {
            Contador = (int)Registro.Get("ContadorMeta");
        }
        
        return Contador;
    }
}