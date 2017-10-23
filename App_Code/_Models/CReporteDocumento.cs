using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class CReporteDocumento
{
    int idreportedocumento = 0;
    string documento = "";
    string tipodocumento = "";
    string descripcion = "";
    int idreporte = 0;
    int idusuarioalta = 0;

    public int IdReporteDocumento {
        get { return idreportedocumento; }
        set { idreportedocumento = value; }
    }

    public string Documento
    {
        get { return documento; }
        set { documento = value; }
    }

    public string TipoDocumento
    {
        get { return tipodocumento; }
        set { tipodocumento = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    public int IdReporte
    {
        get { return idreporte; }
        set { idreporte = value; }
    }

    public int IdUsuarioAlta
    {
        get { return idusuarioalta; }
        set { idusuarioalta = value; }
    }


    public void Obtener(CDB Conn)
    {
        if (idreportedocumento != 0)
        {
            string Query = "SELECT * FROM ReporteDocumento WHERE IdReporteDocumento = @IdReporteDocumento";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdReporteDocumento", idreportedocumento);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }
 
    public static JObject ObtenerJsonReporteDocumentos(JObject esteObjeto)
    {
        CDB conn = new CDB();
        string spDocumento = "EXEC sp_ReporteDocumento_Consultar @Opcion, @IdReporte";
        conn.DefinirQuery(spDocumento);
        conn.AgregarParametros("@Opcion", 1);
        conn.AgregarParametros("@IdReporte", Convert.ToInt32(esteObjeto.Property("IdReporte").Value.ToString()));
        SqlDataReader dr = conn.Ejecutar();
        JArray arrayDocumento = new JArray();
        while (dr.Read())
        {
            JObject Documento = new JObject();
            Documento.Add(new JProperty("IdReporteDocumento", dr["IdReporteDocumento"].ToString()));
            Documento.Add(new JProperty("Documento", dr["Documento"].ToString()));
            Documento.Add(new JProperty("Descripcion", dr["Descripcion"].ToString()));
            arrayDocumento.Add(Documento);
        }
        dr.Close();
        esteObjeto.Add(new JProperty("Documentos", arrayDocumento));
        return esteObjeto;
    }

    public int AgregarDocumento(string pNombreDocumento, string pTipoDocumento, int pIdReporte, int pIdUsuario, CDB Conn)
    {
        int pIdReporteDocumento = 0;

        string Query = "EXEC SP_ReporteDocumento_Agregar @Documento, @TipoDocumento, @Descripcion, @IdReporte, @IdUsuarioAlta";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@Documento", pNombreDocumento);
        Conn.AgregarParametros("@TipoDocumento", pTipoDocumento);
        Conn.AgregarParametros("@Descripcion", "");
        Conn.AgregarParametros("@IdReporte", pIdReporte);
        Conn.AgregarParametros("@IdUsuarioAlta", pIdUsuario);
        SqlDataReader Datos = Conn.Ejecutar();

        while (Datos.Read())
        {
            pIdReporteDocumento = !(Datos["IdReporteDocumento"] is DBNull) ? Convert.ToInt32(Datos["IdReporteDocumento"]) : idreportedocumento;
        }

        Datos.Close();

        return pIdReporteDocumento;
    }

    // Definir valores de instancia
    private void DefinirPropiedades(SqlDataReader Datos)
    {
        if (Datos.HasRows)
        {
            while (Datos.Read())
            {
                idreportedocumento = !(Datos["IdReporteDocumento"] is DBNull) ? Convert.ToInt32(Datos["IdReporteDocumento"]) : idreportedocumento;
                documento = !(Datos["Documento"] is DBNull) ? Convert.ToString(Datos["Documento"]) : documento;
                tipodocumento = !(Datos["TipoDocumento"] is DBNull) ? Convert.ToString(Datos["TipoDocumento"]) : tipodocumento;
                descripcion = !(Datos["Descripcion"] is DBNull) ? Convert.ToString(Datos["Descripcion"]) : documento;
                idreporte = !(Datos["IdReporte"] is DBNull) ? Convert.ToInt32(Datos["IdReporte"]) : idreporte;
                idusuarioalta = !(Datos["IdUsuarioAlta"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioAlta"]) : idreporte;
            }
        }
    }

    public void Editar(CDB Conn)
    {
        string Query = "EXEC sp_ReporteDocumento_Editar @IdReporteDocumento, @Documento, @TipoDocumento, @Descripcion, @IdReporte, @IdUsuarioAlta";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdReporteDocumento", idreportedocumento);
        Conn.AgregarParametros("@Documento", documento);
        Conn.AgregarParametros("@TipoDocumento", tipodocumento);
        Conn.AgregarParametros("@Descripcion", descripcion);
        Conn.AgregarParametros("@IdReporte", idreporte);
        Conn.AgregarParametros("@IdUsuarioAlta", idusuarioalta);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public void Eliminar(CDB Conn)
    {
        string Query = "EXEC sp_ReporteDocumento_Eliminar @IdReporteDocumento";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdReporteDocumento", idreportedocumento);
        SqlDataReader Datos = Conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public JObject DescargarDocumento(string pDocumento)
    {
        JObject miObject = new JObject();
        CDB conn = new CDB();
        string spDocumento = "EXEC sp_ReporteDocumento_Consultar @Opcion, 0, @IdReporteDocumento";
        conn.DefinirQuery(spDocumento);
        conn.AgregarParametros("@Opcion", 2);
        conn.AgregarParametros("@IdReporteDocumento", Convert.ToInt32(pDocumento));
        SqlDataReader dr = conn.Ejecutar();

        while (dr.Read())
        {

            miObject.Add(new JProperty("IdReporteDocumento", dr["IdReporteDocumento"].ToString()));
            miObject.Add(new JProperty("Documento", dr["Documento"].ToString()));
            miObject.Add(new JProperty("TipoDocumento", dr["TipoDocumento"].ToString()));
        }
        dr.Close();

        return miObject;
    }

    private void LimpiarPropiedades()
    {
        idreportedocumento = 0;
        documento = "";
        tipodocumento = "";
        descripcion = "";
        idreporte = 0;
        idusuarioalta = 0;
    }
}