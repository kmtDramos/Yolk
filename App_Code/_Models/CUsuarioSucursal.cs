using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de CUsuarioSucursal
/// </summary>
public class CUsuarioSucursal
{
    private int idusuariosucursal = 0;
    private int idusuario = 0;
    private int idsucursal = 0;
    private bool baja = false;

	public CUsuarioSucursal()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int IdUsuarioSucursal
    {
        get
        {
            return idusuariosucursal;
        }
        set
        {
            idusuariosucursal = value;
        }

    }
    public int IdUsuario
    {
        get
        {
            return idusuario;
        }
        set
        {
            idusuario = value;
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

    // Cargar Cliente
    public void Obtener(CDB Conn)
    {
        if (idusuariosucursal != 0)
        {
            string Query = "SELECT * FROM UsuarioSucursal WHERE IdUsuarioSucursal = @IdUsuarioSucursal";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdUsuarioSucursal", idusuariosucursal);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO UsuarioSucursal (IdUsuario, IdSucursal, Baja) VALUES (@IdUsuario, @IdSucursal,@Baja)" +
            "SELECT * FROM UsuarioSucursal WHERE IdUsuarioSucursal = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuario", idusuario);
        Conn.AgregarParametros("@IdSucursal", idsucursal);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE UsuarioSucursal SET Baja = @Baja WHERE IdUsuarioSucursal=@IdUsuarioSucursal ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuarioSucursal", idusuariosucursal);
        Conn.AgregarParametros("@Baja", baja);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void DesactivarEspecial(CDB Conn)
    {
        string Query = "UPDATE UsuarioSucursal SET Baja = @Baja WHERE IdUsuarioSucursal=@IdUsuarioSucursal AND IdUsuario=@IdUsuario ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuarioSucursal", idusuariosucursal);
        Conn.AgregarParametros("@IdUsuario", idusuario);
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
                idusuariosucursal = !(Datos["IdUsuarioSucursal"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioSucursal"]) : idusuariosucursal;
                idusuario = !(Datos["IdUsuario"] is DBNull) ? Convert.ToInt32(Datos["IdUsuario"]) : idusuario;
                idsucursal = !(Datos["IdSucursal"] is DBNull) ? Convert.ToInt32(Datos["IdSucursal"]) : idsucursal;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    public void Editar(CDB Conn)
    {
        string Query = "UPDATE UsuarioSucursal SET IdUsuario=@IdUsuario, IdSucursal=@IdSucursal WHERE IdUsuarioSucursal= @IdUsuarioSucursal " +
            "SELECT * FROM UsuarioSucursal WHERE IdUsuarioSucursal = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuarioSucursal", idusuariosucursal);
        Conn.AgregarParametros("@IdUsuario", idusuariosucursal);
        Conn.AgregarParametros("@IdSucursal", idsucursal);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void EditarEspecial(CDB Conn)
    {
        string Query = "UPDATE UsuarioSucursal SET IdUsuario=@IdUsuario, IdSucursal=@IdSucursal WHERE IdUsuarioSucursal= @IdUsuarioSucursal " +
            "SELECT * FROM UsuarioSucursal WHERE IdUsuarioSucursal = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuarioSucursal", idusuariosucursal);
        Conn.AgregarParametros("@IdUsuario", idusuario);
        Conn.AgregarParametros("@IdSucursal", idsucursal);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    // Limpiar valores de instancia
    private void LimpiarPropiedades()
    {
        idusuariosucursal = 0;
        idusuario = 0;
        idsucursal = 0;
        baja = false;
    }

    public static int ValidaExiste(int IdUsuario, int IdSucursal, CDB Conn)
    {
        int IdUsuarioSucursal = 0;
        string Query = "SELECT IdUsuarioSucursal FROM UsuarioSucursal WHERE IdUsuario=@IdUsuario AND IdSucursal=@IdSucursal ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuario", IdUsuario);
        Conn.AgregarParametros("@IdSucursal", IdSucursal);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdUsuarioSucursal"))
        {
            IdUsuarioSucursal = (int)Registro.Get("IdUsuarioSucursal");
        }
        return IdUsuarioSucursal;
    }
}