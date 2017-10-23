using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class CMenu
{
    private int idmenu = 0;
    private string menu = "";
    private int orden = 0;
    private bool baja = false;

    public int IdMenu
    {
        get { return idmenu; }
        set { idmenu = value; }
    }

    public string Menu
    {
        get { return menu; }
        set { menu = value; }
    }

    public int Orden
    {
        get { return orden; }
        set { orden = value; }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    public void Desactivar(CDB Conn)
    {
        string Query = "UPDATE Menu SET Baja = @Baja WHERE IdMenu=@IdMenu ";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdMenu", idmenu);
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
                idmenu = !(Datos["IdMenu"] is DBNull) ? Convert.ToInt32(Datos["IdMenu"]) : idmenu;
                menu = !(Datos["Menu"] is DBNull) ? Convert.ToString(Datos["Menu"]) : menu;
                orden = !(Datos["Orden"] is DBNull) ? Convert.ToInt32(Datos["Orden"]) : orden;
                baja = !(Datos["Baja"] is DBNull) ? Convert.ToBoolean(Datos["Baja"]) : baja;
            }
        }
    }

    public void Agregar(CDB Conn)
    {
        string Query = "EXEC sp_Menu_Agregar @Menu, @Orden";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Menu", menu);
        Conn.AgregarParametros("@Orden", orden);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Editar(CDB conn)
    {
        string query = "UPDATE Menu SET Menu = @Menu, Orden = @Orden WHERE IdMenu = @IdMenu " +
               "SELECT * FROM Menu WHERE IdMenu = SCOPE_IDENTITY()";
        conn.DefinirQuery(query);
        conn.AgregarParametros("@IdMenu", idmenu);
        conn.AgregarParametros("@Menu", menu);
        conn.AgregarParametros("@Orden", orden);
        SqlDataReader Datos = conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Obtener(CDB Conn)
    {
        if (idmenu != 0)
        {
            string Query = "SELECT * FROM Menu WHERE IdMenu = @IdMenu ";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdMenu", idmenu);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    private void LimpiarPropiedades()
    {
        idmenu = 0;
        menu = "";
        orden = 0;
        baja = false;
    }

    public static int ValidaExiste(string Menu, CDB Conn)
    {

        int Contador = 0;
        string Query = "SELECT COUNT(Menu) AS Contador FROM Menu WHERE  Menu COLLATE Latin1_general_CI_AI LIKE '%'+ @Menu + '%'";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Menu", Menu);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("Contador"))
        {
            Contador = (int)Registro.Get("Contador");
        }
        return Contador;
    }

    public static int ValidaExisteEditarMenu(int IdMenu, string Menu, CDB Conn)
    {
        int Id = 0;
        string Query = "SELECT IdMenu FROM Menu WHERE Menu COLLATE Latin1_general_CI_AI like '%'+@Menu + '%' AND IdMenu<>@IdMenu";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdMenu", IdMenu);
        Conn.AgregarParametros("@Menu", Menu);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("IdMenu"))
        {
            Id = (int)Registro.Get("IdMenu");
        }
        return Id;
    }

    public static JObject ObtenerJsonMenus(JObject esteObjeto)
    {
        CDB conn = new CDB();
        string spMenu = "EXEC SP_Menu_Consultar @Opcion";
        conn.DefinirQuery(spMenu);
        conn.AgregarParametros("@Opcion", 2);
        SqlDataReader dr = conn.Ejecutar();
        JArray arrayMenu = new JArray();
        while (dr.Read())
        {
            JObject Menu = new JObject();
            Menu.Add(new JProperty("Valor", Convert.ToInt32(dr["Valor"].ToString())));
            Menu.Add(new JProperty("Etiqueta", dr["Etiqueta"].ToString()));
            arrayMenu.Add(Menu);
        }
        dr.Close();
        esteObjeto.Add(new JProperty("Menus", arrayMenu));
        return esteObjeto;
    }
}