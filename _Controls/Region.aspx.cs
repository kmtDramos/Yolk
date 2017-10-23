using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Controls_Region : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ListarRegiones(int Pagina, string Columna, string Orden)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;

            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string Query = "SELECT COUNT(IdRegion) AS Regiones FROM Region";
                Conn.DefinirQuery(Query);
                CObjeto Registro = Conn.ObtenerRegistro();

                double Regiones = (int)Registro.Get("Regiones");
                int Paginado = 10;

                Query = "SELECT * FROM (" +
                        "SELECT IdRegion, Region, " +
                        "CASE WHEN Baja=0 THEN '0' ELSE '1' END AS Baja, " +
                        "CASE WHEN Baja=0 THEN 'Activo' ELSE 'Inactivo' END AS Estatus,ROW_NUMBER() OVER(ORDER BY " + Columna + " " + Orden + ") AS Num " +
                        "FROM Region) AS R WHERE Num BETWEEN @Inicio AND @Fin";
                Conn.DefinirQuery(Query);
                Conn.AgregarParametros("@Inicio", Paginado * Pagina - (Paginado - 1));
                Conn.AgregarParametros("@Fin", Paginado * Pagina);

                Datos.Add("Regiones", Conn.ObtenerRegistros());
                Datos.Add("Paginas", Math.Ceiling((double)(Regiones / Paginado)));
                Datos.Add("Registros", Regiones);

                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AgregarRegion(string Region)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CRegion cRegion = new CRegion();
                cRegion.Region = Region;
                cRegion.Baja = false;
                Error = ValidarRegion(cRegion);
                if (Error == "")
                {
                    cRegion.Agregar(Conn);
                }

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    private static string ValidarRegion(CRegion Region)
    {
        string Mensaje = "";

        Mensaje += (Region.Region == "") ? "<li>Favor de completar el campo región.</li>" : Mensaje;

        Mensaje = (Mensaje != "") ? "<p>Favor de completar los siguientes campos:<ul>" + Mensaje + "</ul></p>" : Mensaje;

        return Mensaje;
    }

    [WebMethod]
    public static string DesactivarRegion(int IdRegion, int Baja)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                bool desactivar = false;
                if (Baja == 0)
                {
                    desactivar = true;
                }
                else
                {
                    desactivar = false;
                }
                CObjeto Datos = new CObjeto();

                CRegion cRegion = new CRegion();
                cRegion.IdRegion = IdRegion;
                cRegion.Baja = desactivar;
                cRegion.Desactivar(Conn);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarRegion(int IdRegion, string Region)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                CRegion cRegion = new CRegion();
                cRegion.IdRegion = IdRegion;
                cRegion.Region = Region;
                cRegion.Baja = false;
                Error = ValidarRegion(cRegion);
                if (Error == "")
                {
                    cRegion.Editar(Conn);
                }

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }
}