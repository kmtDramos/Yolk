using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de CPerfilPermiso
/// </summary>
public class CPerfilPermiso
{
    private int idperfilpermiso = 0;
    private int idperfil = 0;
    private int idpermiso = 0;
    private bool baja = false;

	public CPerfilPermiso()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int IdPerfilPermiso
    {
        get
        {
            return idperfilpermiso;
        }
        set
        {
            idperfilpermiso = value;
        }

    }
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

    public int IdPermiso
    {
        get
        {
            return idpermiso;
        }
        set
        {
            idpermiso = value;
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

    // Cargar
    public void Obtener(CDB Conn)
    {
        if (idperfilpermiso != 0)
        {
            string Query = "SELECT * FROM PerfilPermiso WHERE IdPerfilPermiso = @IdPerfilPermiso";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdPerfilPermiso", idperfilpermiso);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO PerfilPermiso (IdPerfil, IdPermiso, Baja) VALUES (@IdPerfil, @IdPermiso,@Baja)" +
            "SELECT * FROM PerfilPermiso WHERE IdPerfilPermiso = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfil", idperfil);
        Conn.AgregarParametros("@IdPermiso", idpermiso);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE PerfilPermiso SET Baja = @Baja WHERE IdPerfilPermiso=@IdPerfilPermiso ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfilPermiso", idperfilpermiso);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void DesactivarEspecial(CDB Conn)
    {
        string Query = "UPDATE PerfilPermiso SET Baja = @Baja WHERE IdPerfilPermiso=@IdPerfilPermiso AND IdPerfil=@IdPerfil ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfilPermiso", idperfilpermiso);
        Conn.AgregarParametros("@IdPerfil", idperfil);
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
                idperfilpermiso = !(Datos["IdPerfilPermiso"] is DBNull) ? Convert.ToInt32(Datos["IdPerfilPermiso"]) : idperfilpermiso;
                idperfil = !(Datos["IdPerfil"] is DBNull) ? Convert.ToInt32(Datos["IdPerfil"]) : idperfil;
                idpermiso = !(Datos["IdPermiso"] is DBNull) ? Convert.ToInt32(Datos["IdPermiso"]) : idpermiso;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    public void Editar(CDB Conn)
    {
        string Query = "UPDATE PerfilPermiso SET IdPerfil=@IdPerfil, IdPermiso=@IdPermiso WHERE IdPerfilPermiso= @IdPerfilPermiso " +
            "SELECT * FROM PerfilPermiso WHERE IdPerfilPermiso = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfilPermiso", idperfilpermiso);
        Conn.AgregarParametros("@IdPerfil", idperfilpermiso);
        Conn.AgregarParametros("@IdPermiso", idpermiso);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void EditarEspecial(CDB Conn)
    {
        string Query = "UPDATE PerfilPermiso SET IdPerfil=@IdPerfil, IdPermiso=@IdPermiso WHERE IdPerfilPermiso= @IdPerfilPermiso " +
            "SELECT * FROM PerfilPermiso WHERE IdPerfilPermiso = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfilPermiso", idperfilpermiso);
        Conn.AgregarParametros("@IdPerfil", idperfil);
        Conn.AgregarParametros("@IdPermiso", idpermiso);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    // Limpiar valores de instancia
    private void LimpiarPropiedades()
    {
        idperfilpermiso = 0;
        idperfil = 0;
        idpermiso = 0;
        baja = false;
    }

    public static int ValidaExiste(int IdPerfil, int IdPermiso, CDB Conn)
    {
        int IdPerfilPermiso = 0;
        string Query = "SELECT IdPerfilPermiso FROM PerfilPermiso WHERE IdPerfil=@IdPerfil AND IdPermiso=@IdPermiso ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPerfil", IdPerfil);
        Conn.AgregarParametros("@IdPermiso", IdPermiso);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdPerfilPermiso"))
        {
            IdPerfilPermiso = (int)Registro.Get("IdPerfilPermiso");
        }
        return IdPerfilPermiso;
    }
}