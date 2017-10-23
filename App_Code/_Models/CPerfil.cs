using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de CPerfil
/// </summary>
public class CPerfil
{
    private int idperfil = 0;
    private string perfil = "";
    private int idpagina = 0;
    private bool baja = false;

    public int IdPerfil
    {
        get
        {
            return idperfil;
        }
        set
        {
            idperfil = value;
        }
    }

    public string Perfil
    {
        get
        {
            return perfil;
        }
        set
        {
            perfil = value;
        }
    }

    public int IdPagina
    {
        get
        {
            return idpagina;
        }
        set
        {
            idpagina = value;
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
        string Query = "UPDATE Perfil SET Baja = @Baja WHERE IdPerfil=@IdPerfil ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfil", idperfil);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    private void DefinirPropiedades(SqlDataReader Datos)
    {
        if (Datos.HasRows)
        {
            while (Datos.Read())
            {
                idperfil = !(Datos["IdPerfil"] is DBNull) ? Convert.ToInt32(Datos["IdPerfil"]) : idperfil;
                perfil = !(Datos["Perfil"] is DBNull) ? Convert.ToString(Datos["Perfil"]) : perfil;
                idpagina = !(Datos["IdPerfil"] is DBNull) ? Convert.ToInt32(Datos["IdPerfil"]) : idpagina;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    public void Agregar(CDB Conn)
    {
        string Query = "EXEC SP_Perfil_Agregar @Perfil, @IdPagina, @Baja ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Perfil", perfil);
        Conn.AgregarParametros("@IdPagina", idpagina);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Editar(CDB Conn)
    {
        string Query = "UPDATE Perfil SET Perfil=@Perfil, IdPagina=@IdPagina WHERE IdPerfil= @IdPerfil " +
            "SELECT * FROM Perfil WHERE IdPerfil = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfil", idperfil);
        Conn.AgregarParametros("@Perfil", perfil);
        Conn.AgregarParametros("@IdPagina", idpagina);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Obtener(CDB Conn)
    {
        if (idpagina != 0)
        {
            string Query = "SELECT * FROM Perfil WHERE IdPerfil = @IdPerfil";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdPerfil", idperfil);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    public static int ValidaExisteEditar(int IdPerfil, string Perfil, int IdPagina, CDB Conn)
    {
        int Id = 0;
        string Query = "SELECT IdPerfil FROM Perfil WHERE Perfil=@Perfil AND IdPerfil<>@IdPerfil";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Perfil", Perfil);
        Conn.AgregarParametros("@IdPerfil", IdPerfil);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdPerfil"))
        {
            Id = (int)Registro.Get("IdPerfil");
        }
        return Id;
    }
}