using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class CUsuarioProveedor
{
    private int idusuarioproveedor = 0; 
    private int idusuario = 0;
    private int idproveedor = 0;


    public int IdUsuarioProveedor
    {
        get
        {
            return idusuarioproveedor;
        }
        set
        {
            idusuarioproveedor = value;
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

    public int IdProveedor
    {
        get
        {
            return idproveedor;
        }
        set
        {
            idproveedor = value;
        }
    }


    // Agregar registro
    public void Agregar(CDB Conn)
    {
        string Query = "INSERT INTO UsuarioProveedor (IdUsuario, IdProveedor) VALUES (@IdUsuario, @IdProveedor)" +
            "SELECT * FROM UsuarioProveedor WHERE IdUsuarioProveedor = SCOPE_IDENTITY()";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuario", idusuario);
        Conn.AgregarParametros("@IdProveedor", idproveedor);       
        SqlDataReader Datos = Conn.Ejecutar();

        DefinirPropiedades(Datos);
        Datos.Close();
    }

    // Editar registro
    public void Editar(CDB Conn)
    {
        if (idusuario != 0)
        {
            string Query = "UPDATE UsuarioProveedor SET IdUsuario=@IdUsuario,IdProveedor=@IdProveedor WHERE IdUsuarioProveedor=@IdUsuarioProveedor;" +
            "SELECT * FROM UsuarioProveedor WHERE IdUsuarioProveedor = SCOPE_IDENTITY()";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdUsuarioProveedor", idusuarioproveedor);
            Conn.AgregarParametros("@IdUsuario", idusuario);
            Conn.AgregarParametros("@IdProveedor", idproveedor);
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
                idusuarioproveedor = !(Datos["IdUsuarioProveedor"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioProveedor"]) : idusuarioproveedor;
                idusuario = !(Datos["IdUsuario"] is DBNull) ? Convert.ToInt32(Datos["IdUsuario"]) : idusuario;
                idproveedor = !(Datos["IdProveedor"] is DBNull) ? Convert.ToInt32(Datos["IdProveedor"]) : idproveedor;
            }
        }
    }


    // Cargar UsuarioP
    public void Obtener(CDB Conn)
    {
        if (idusuarioproveedor != 0)
        {
            string Query = "SELECT * FROM UsuarioProveedor WHERE IdUsuarioProveedor = @IdUsuarioProveedor";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdUsuarioProveedor", idusuarioproveedor);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    public static JObject ObtenerJsonUsuarioProveedores(JObject esteObjeto)
    {
        CDB conn = new CDB();
        string spUsuarioProveedor = "EXEC sp_UsuarioProveedor_Consultar @Opcion, @IdProveedor";
        conn.DefinirQuery(spUsuarioProveedor);
        conn.AgregarParametros("@Opcion", 1);
        conn.AgregarParametros("@IdProveedor", Convert.ToInt32(esteObjeto.Property("IdProveedor").Value.ToString()));
        SqlDataReader dr = conn.Ejecutar();
        JArray arrayUsuarioProveedor = new JArray();

        while (dr.Read())
        {
            JObject UsuarioProveedor = new JObject();
            UsuarioProveedor.Add(new JProperty("Valor", Convert.ToInt32(dr["Valor"].ToString())));
            UsuarioProveedor.Add(new JProperty("Etiqueta", dr["Etiqueta"].ToString()));
            arrayUsuarioProveedor.Add(UsuarioProveedor);
        }

        dr.Close();
        esteObjeto.Add(new JProperty("UsuarioProveedores", arrayUsuarioProveedor));
        return esteObjeto;
    }

    public static int ValidaExiste(int IdUsuario, CDB Conn)
    {
        int IdUsuarioProveedor = 0;
        string Query = "SELECT IdUsuarioProveedor FROM UsuarioProveedor WHERE IdUsuario =@IdUsuario";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdUsuario", IdUsuario);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdUsuarioProveedor"))
        {
            IdUsuarioProveedor = (int)Registro.Get("IdUsuarioProveedor");
        }
        return IdUsuarioProveedor;
    }


}