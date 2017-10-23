using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de CPagina
/// </summary>
public class CPagina
{
    private int idpagina = 0;
    private int orden = 0;
    private string pagina = "";
    private string descripcion = "";
    private int idmenu = 0;
    private int idpermiso = 0;
    private bool baja = false;

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

    public int Orden
    {
        get
        {
            return orden;
        }
        set
        {
            orden = value;
        }
    }

    public string Pagina
    {
        get
        {
            return pagina;
        }
        set
        {
            pagina = value;
        }
    }

    public string Descripcion
    {
        get
        {
            return descripcion;
        }
        set
        {
            descripcion = value;
        }
    }

    public int IdMenu
    {
        get
        {
            return idmenu;
        }
        set
        {
            idmenu = value;
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

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE Pagina SET Baja = @Baja WHERE IdPagina=@IdPagina ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPagina", idpagina);
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
                idpagina = !(Datos["IdPagina"] is DBNull) ? Convert.ToInt32(Datos["IdPagina"]) : idpagina;
                orden = !(Datos["Orden"] is DBNull) ? Convert.ToInt32(Datos["Orden"]) : orden;
                pagina = !(Datos["Pagina"] is DBNull) ? Convert.ToString(Datos["Pagina"]) : pagina;
                descripcion = !(Datos["Descripcion"] is DBNull) ? Convert.ToString(Datos["Descripcion"]) : descripcion;
                idmenu = !(Datos["IdMenu"] is DBNull) ? Convert.ToInt32(Datos["IdMenu"]) : idmenu;
                idpermiso = !(Datos["IdPermiso"] is DBNull) ? Convert.ToInt32(Datos["IdPermiso"]) : idpermiso;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    public void Agregar(CDB Conn)
    {
        string Query = "EXEC sp_Pagina_Agregar @Pagina, @Descripcion, @IdMenu, @IdPermiso ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Pagina", pagina);
        Conn.AgregarParametros("@Descripcion", descripcion);
        Conn.AgregarParametros("@IdMenu", idmenu);
        Conn.AgregarParametros("@IdPermiso", idpermiso);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Editar(CDB Conn)
    {
        string Query = "UPDATE Pagina SET Pagina=@Pagina, Orden=@Orden, Descripcion=@Descripcion, IdMenu=@IdMenu, IdPermiso=@IdPermiso WHERE IdPagina= @IdPagina " +
            "SELECT * FROM Pagina WHERE IdPagina = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPagina", idpagina);
        Conn.AgregarParametros("@Pagina", pagina);
        Conn.AgregarParametros("@Orden", orden);
        Conn.AgregarParametros("@Descripcion", descripcion);
        Conn.AgregarParametros("@IdMenu", idmenu);
        Conn.AgregarParametros("@IdPermiso", idpermiso);

        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    // Cargar Cliente
    public void Obtener(CDB Conn)
    {
        if (idpagina != 0)
        {
            string Query = "SELECT * FROM Pagina WHERE IdPagina = @IdPagina";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdPagina", idpagina);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    public static int ValidaExisteEditar(int IdPagina, string Pagina, CDB Conn)
    {
        int Id = 0;
        string Query = "SELECT IdPagina FROM Pagina WHERE Pagina COLLATE Latin1_general_CI_AI like '%'+@Pagina + '%' AND IdPagina<>@IdPagina";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdPagina", IdPagina);
        Conn.AgregarParametros("@Pagina", Pagina);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdPagina"))
        {
            Id = (int)Registro.Get("IdPagina");
        }
        return Id;
    }

    public static int ValidaExiste(string Pagina, CDB Conn)
    {

        int Contador = 0;
        string Query = "SELECT COUNT(IdPagina) AS Contador FROM Pagina WHERE Pagina COLLATE Latin1_general_CI_AI LIKE '%'+ @Pagina + '%'";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Pagina", Pagina);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("Contador"))
        {
            Contador = (int)Registro.Get("Contador");
        }
        return Contador;
    }

    private void LimpiarPropiedades()
    {
        idpagina = 0;
        pagina = "";
        orden = 0;
        descripcion = "";
        idmenu = 0;
        idpermiso= 0;
        baja = false;
    }
	
}