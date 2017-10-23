using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de CTarifa
/// </summary>
public class CTarifa
{
    private int idtarifa = 0;
    private int idregion = 0;
	private int idfuente = 0;
    //private string fecha = "";
	private int mes = 0;
	private int anio = 0;
    private decimal consumoBaja = 0;
    private decimal consumoMedia = 0;
    private decimal consumoAlta = 0;
    private decimal demanda = 0;
    private bool baja = false;

	public CTarifa()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int IdTarifa
    {
        get
        {
            return idtarifa;
        }
        set
        {
            idtarifa = value;
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

	public int IdFuente
	{
		get
		{
			return idfuente;
		}
		set
		{
			idfuente = value;
		}
	}

	//public string Fecha
 //   {
 //       get
 //       {
 //           return fecha;
 //       }
 //       set
 //       {
 //           fecha = value;
 //       }
 //   }

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

	public decimal ConsumoBaja
    {
        get
        {
            return consumoBaja;
        }
        set
        {
            consumoBaja = value;
        }
    }

    public decimal ConsumoMedia
    {
        get
        {
            return consumoMedia;
        }
        set
        {
            consumoMedia = value;
        }
    }

    public decimal ConsumoAlta
    {
        get
        {
            return consumoAlta;
        }
        set
        {
            consumoAlta = value;
        }
    }

    public decimal Demanda
    {
        get
        {
            return demanda;
        }
        set
        {
            demanda = value;
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

    // Cargar Tarifa
    public void Obtener(CDB Conn)
    {
        if (idtarifa != 0)
        {
            string Query = "SELECT * FROM Tarifa WHERE IdTarifa = @IdTarifa";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdTarifa", idtarifa);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO Tarifa (IdFuente,IdRegion,Mes,Anio, ConsumoBaja, ConsumoMedia, ConsumoAlta, Demanda, Baja) VALUES (@IdFuente,@IdRegion,@Mes,@Anio, @ConsumoBaja, @ConsumoMedia, @ConsumoAlta, @Demanda, @Baja)" +
            "SELECT * FROM Tarifa WHERE IdTarifa = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@IdFuente", idfuente);
        Conn.AgregarParametros("@IdRegion", idregion);
		//Conn.AgregarParametros("@Fecha", fecha);
		Conn.AgregarParametros("@Mes", mes);
		Conn.AgregarParametros("@Anio", anio);
		Conn.AgregarParametros("@ConsumoBaja", consumoBaja);
        Conn.AgregarParametros("@ConsumoMedia", consumoMedia);
        Conn.AgregarParametros("@ConsumoAlta", consumoAlta);
        Conn.AgregarParametros("@Demanda", demanda);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE Tarifa SET Baja = @Baja WHERE IdTarifa=@IdTarifa ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdTarifa", idtarifa);
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
				idfuente = !(Datos["IdFuente"] is DBNull) ? Convert.ToInt32(Datos["IdFuente"]) : idfuente;
                idtarifa = !(Datos["IdTarifa"] is DBNull) ? Convert.ToInt32(Datos["IdTarifa"]) : idtarifa;
                idregion = !(Datos["IdRegion"] is DBNull) ? Convert.ToInt32(Datos["IdRegion"]) : idregion;
				//fecha = !(Datos["Fecha"] is DBNull) ? Convert.ToString(Datos["Fecha"]) : fecha;
				mes = !(Datos["Mes"] is DBNull) ? Convert.ToInt32(Datos["Mes"]) : mes;
				anio = !(Datos["ANIO"] is DBNull) ? Convert.ToInt32(Datos["Anio"]) : anio;
				consumoBaja = !(Datos["Fecha"] is DBNull) ? Convert.ToDecimal(Datos["ConsumoBaja"]) : consumoBaja;
                consumoMedia = !(Datos["Fecha"] is DBNull) ? Convert.ToDecimal(Datos["ConsumoMedia"]) : consumoMedia;
                consumoAlta = !(Datos["Fecha"] is DBNull) ? Convert.ToDecimal(Datos["ConsumoAlta"]) : consumoAlta;
                demanda = !(Datos["Fecha"] is DBNull) ? Convert.ToDecimal(Datos["Demanda"]) : demanda;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    // Editar registro
    public void Editar(CDB Conn)
    {
        if (idtarifa != 0)
        {
            string Query = "UPDATE Tarifa SET Mes=@Mes, Anio=@Anio, ConsumoBaja=@ConsumoBaja, ConsumoMedia=@ConsumoMedia, ConsumoAlta=@ConsumoAlta, Demanda=@Demanda, IdRegion=@IdRegion, IdFuente=@IdFuente, Baja=@Baja WHERE IdTarifa=@IdTarifa " +
            "SELECT * FROM Tarifa WHERE IdTarifa = SCOPE_IDENTITY()";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdTarifa", idtarifa);
			//Conn.AgregarParametros("@Fecha", fecha);
			Conn.AgregarParametros("@Mes", mes);
			Conn.AgregarParametros("@Anio", anio);
			Conn.AgregarParametros("@ConsumoBaja", consumoBaja);
            Conn.AgregarParametros("@ConsumoMedia", consumoMedia);
            Conn.AgregarParametros("@ConsumoAlta", consumoAlta);
            Conn.AgregarParametros("@Demanda", demanda);
            Conn.AgregarParametros("@IdRegion", idregion);
			Conn.AgregarParametros("@IdFuente", idfuente);
            Conn.AgregarParametros("@Baja", baja);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Limpiar valores de instancia
    private void LimpiarPropiedades()
    {
		idfuente = 0;
        idtarifa = 0;
		idfuente = 0;
        idregion = 0;
		//fecha = "";
		mes = 0;
		anio = 0;
		consumoBaja = 0;
        consumoMedia = 0;
        consumoAlta = 0;
        demanda = 0;
        baja = false;
    }

	public static int ValidaExiste(int IdRegion, int Mes, int Anio, CDB Conn)
	{
		int Contador = 0;
		string Query = "SELECT COUNT(IdRegion) AS Contador FROM Tarifa WHERE IdRegion=@IdRegion AND Mes=@Mes AND Anio=@Anio";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@IdRegion", IdRegion);
		Conn.AgregarParametros("@Mes", Mes);
		Conn.AgregarParametros("@Anio", Anio);
		CObjeto ValidaExiste = Conn.ObtenerRegistro();
		if (ValidaExiste.Exist("Contador"))
		{
			Contador = (int)ValidaExiste.Get("Contador");
		}
		return Contador;
	}

	public static int ValidaExisteEditar(int IdTarifa, int IdRegion, int Mes, int Anio, CDB Conn)
	{
		int Contador = 0;
		string Query = "SELECT COUNT(IdRegion) AS Contador FROM Tarifa WHERE IdTarifa<>@IdTarifa AND IdRegion=@IdRegion AND Mes=@Mes AND Anio=@Anio";
		Conn.DefinirQuery(Query);
		Conn.AgregarParametros("@IdTarifa", IdTarifa);
		Conn.AgregarParametros("@IdRegion", IdRegion);
		Conn.AgregarParametros("@Mes", Mes);
		Conn.AgregarParametros("@Anio", Anio);
		CObjeto ValidaExisteEditar = Conn.ObtenerRegistro();
		if (ValidaExisteEditar.Exist("Contador"))
		{
			Contador = (int)ValidaExisteEditar.Get("Contador");
		}
		return Contador;
	}

}