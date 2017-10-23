using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de Permiso
/// </summary>
public class Permiso
{
    private int idperfil= 0;
    private string perfil= "";
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
                idpagina = !(Datos["IdPagina"] is DBNull) ? Convert.ToInt32(Datos["IdPagina"]) : idpagina;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    public void Agregar(CDB Conn)
    {
        string Query = "EXEC SP_Perfil_AgregarPerfil @IdPerfil, @Perfil, @IdPagina, @Baja ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfil", idperfil);
        Conn.AgregarParametros("@Perfil", perfil);
        Conn.AgregarParametros("@IdPagina", idpagina);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Editar(CDB Conn)
    {
        string Query = "UPDATE Perfil SET IdPerfil=@IdPerfil, Perfil=@Perfil, Descripcion=@Descripcion WHERE IdPerfil= @IdPerfil " +
            "SELECT * FROM Perfil WHERE IdPerfil = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfil", idperfil);
        Conn.AgregarParametros("@Perfil", perfil);
        Conn.AgregarParametros("@IdPagina", idpagina);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    // Cargar Cliente
    public void Obtener(CDB Conn)
    {
        if (idperfil != 0)
        {
            string Query = "SELECT * FROM Perfil WHERE IdPerfil = @IdPerfil";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdPerfil", idperfil);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

}