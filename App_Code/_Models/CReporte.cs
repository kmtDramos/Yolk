using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class CReporte
{

    int idreporte = 0;
    string folio = "";
    int idestatus = 0;
    int idcircuito = 0;
    int idtipoconsumo = 0;
    DateTime fechalevantamiento = new DateTime(1900, 1, 1);
    DateTime fechaatencion = new DateTime(1900, 1, 1);
    DateTime fechaenvioproveedor = new DateTime(1900, 1, 1);
    DateTime fechacierre = new DateTime(1900, 1, 1);
    int idtipoproblema = 0;
    string reporte = "";
    int idusuarioalta = 0;
    int idusuariorequiere = 0;
    int idusuarioresponsable = 0;
    string comentarioscierre = "";
    int idproveedor = 0;
    int idusuarioproveedor = 0;

    public int IdReporte 
    {
        get { return idreporte; }
        set { idreporte = value; }
    }

    public string Folio
    {
        get { return folio; }
        set { folio = value; }
    }

    public int IdEstatus
    {
        get { return idestatus ; }
        set { idestatus  = value; }
    }

    public int IdCircuito
    {
        get { return idcircuito; }
        set { idcircuito = value; }
    }

    public int IdTipoConsumo
    {
        get { return idtipoconsumo; }
        set { idtipoconsumo = value; }
    }

    public DateTime FechaLevantamiento
    {
        get { return fechalevantamiento; }
        set { fechalevantamiento = value; }
    }

    public DateTime FechaAtencion
    {
        get { return fechaatencion; }
        set { fechaatencion = value; }
    }

    public DateTime FechaEnvioProveedor
    {
        get { return fechaenvioproveedor; }
        set { fechaenvioproveedor = value; }
    }

    public DateTime FechaCierre {
        get { return fechacierre; }
        set { fechacierre = value; }
    }

    public int IdTipoProblema
    {
        get { return idtipoproblema; }
        set { idtipoproblema = value; }
    }

    public string Reporte
    {
        get { return reporte; }
        set { reporte = value; }
    }

    public int IdUsuarioAlta
    {
        get { return idusuarioalta; }
        set { idusuarioalta = value; }
    }

    public int IdUsuarioRequiere
    {
        get { return idusuariorequiere; }
        set { idusuariorequiere = value; }
    }

    public int IdUsuarioResponsable
    {
        get { return idusuarioresponsable; }
        set { idusuarioresponsable = value; }
    }

    public int IdProveedor
    {
        get { return idproveedor; }
        set { idproveedor = value; }
    }

    public int IdUsuarioProveedor
    {
        get { return idusuarioproveedor; }
        set { idusuarioproveedor = value; }
    }

    public void Obtener(CDB Conn)
    {
        if (idreporte != 0)
        {
            string Query = "SELECT * FROM Reporte WHERE IdReporte = @IdReporte";
            Conn.DefinirQuery(Query);
            Conn.AgregarParametros("@IdReporte", idreporte);
            SqlDataReader Datos = Conn.Ejecutar();
            DefinirPropiedades(Datos);
            Datos.Close();
        }
    }

    private void DefinirPropiedades(SqlDataReader Datos)
    {
        if (Datos.HasRows)
        {
            while (Datos.Read())
            {
                idreporte = !(Datos["IdReporte"] is DBNull) ? Convert.ToInt32(Datos["IdReporte"]) : idreporte;
                folio = !(Datos["Folio"] is DBNull) ? Convert.ToString(Datos["Folio"]) : folio;
                idestatus = !(Datos["IdEstatus"] is DBNull) ? Convert.ToInt32(Datos["IdEstatus"]) : idestatus;
                idcircuito = !(Datos["IdCircuito"] is DBNull) ? Convert.ToInt32(Datos["IdCircuito"]) : idcircuito;
                idtipoconsumo = !(Datos["IdTipoConsumo"] is DBNull) ? Convert.ToInt32(Datos["IdTipoConsumo"]) : idtipoconsumo;
                fechalevantamiento = !(Datos["FechaLevantamiento"] is DBNull) ? Convert.ToDateTime(Datos["FechaLevantamiento"]) : fechalevantamiento;
                fechaatencion = !(Datos["FechaAtencion"] is DBNull) ? Convert.ToDateTime(Datos["FechaAtencion"]) : fechaatencion;
                fechaenvioproveedor = !(Datos["FechaEnvioProveedor"] is DBNull) ? Convert.ToDateTime(Datos["FechaEnvioProveedor"]) : fechaenvioproveedor;
                fechacierre = !(Datos["FechaCierre"] is DBNull) ? Convert.ToDateTime(Datos["FechaCierre"]) : fechacierre;
                idtipoproblema = !(Datos["IdTipoProblema"] is DBNull) ? Convert.ToInt32(Datos["IdTipoProblema"]) : idtipoproblema;
                reporte = !(Datos["Reporte"] is DBNull) ? Convert.ToString(Datos["Reporte"]) : reporte;
                idusuarioalta = !(Datos["IdUsuarioAlta"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioAlta"]) : idusuarioalta;
                idusuariorequiere = !(Datos["IdUsuarioRequiere"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioRequiere"]) : idusuariorequiere;
                idusuarioresponsable = !(Datos["IdUsuarioResponsable"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioResponsable"]) : idusuarioresponsable;
                comentarioscierre = !(Datos["ComentariosCierre"] is DBNull) ? Convert.ToString(Datos["ComentariosCierre"]) : comentarioscierre;
                idproveedor = !(Datos["IdProveedor"] is DBNull) ? Convert.ToInt32(Datos["IdProveedor"]) : idproveedor;
                idusuarioproveedor = !(Datos["IdUsuarioProveedor"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioProveedor"]) : idusuarioproveedor;
            }
        }
    }


    public void Editar(CDB conn)
    {
        string query = "EXEC sp_Reporte_Editar @IdReporte, @Folio, @IdEstatus, @IdCircuito, @IdTipoConsumo, @FechaLevantamiento, " +
               " @FechaAtencion, @FechaEnvioProveedor, @FechaCierre,@IdTipoProblema, @Reporte, @IdUsuarioAlta, @IdUsuarioRequiere, " +
               " @IdUsuarioResponsable, @ComentariosCierre, @IdProveedor, @IdUsuarioProveedor ";
        conn.DefinirQuery(query);
        conn.AgregarParametros("@IdReporte", idreporte);
        conn.AgregarParametros("@Folio", folio);
        conn.AgregarParametros("@IdEstatus", idestatus);
        conn.AgregarParametros("@IdCircuito", idcircuito);
        conn.AgregarParametros("@IdTipoConsumo", idtipoconsumo);
        conn.AgregarParametros("@FechaLevantamiento", fechalevantamiento);
        conn.AgregarParametros("@FechaAtencion", fechaatencion);
        conn.AgregarParametros("@FechaEnvioProveedor", fechaenvioproveedor);
        conn.AgregarParametros("@FechaCierre", fechacierre);
        conn.AgregarParametros("@IdTipoProblema", idtipoproblema);
        conn.AgregarParametros("@Reporte", reporte);
        conn.AgregarParametros("@IdUsuarioAlta", idusuarioalta);
        conn.AgregarParametros("@IdUsuarioRequiere", idusuariorequiere);
        conn.AgregarParametros("@IdUsuarioResponsable", idusuarioresponsable);
        conn.AgregarParametros("@ComentariosCierre", comentarioscierre);
        conn.AgregarParametros("@IdProveedor", idproveedor);
        conn.AgregarParametros("@IdUsuarioProveedor", idusuarioproveedor);
        SqlDataReader Datos = conn.Ejecutar();
        DefinirPropiedades(Datos);
        Datos.Close();
    }

    public static int ValidaExisteDocumento(int IdReporte, CDB Conn)
    {

        int Contador = 0;
        string Query = "SELECT COUNT(IdReporteDocumento) AS Contador FROM ReporteDocumento WHERE IdReporte=@IdReporte";
        Conn.DefinirQuery(Query);
        Conn.AgregarParametros("@IdReporte", IdReporte);
        CObjeto Registro = Conn.ObtenerRegistro();
        if (Registro.Exist("Contador"))
        {
            Contador = (int)Registro.Get("Contador");
        }
        return Contador;
    }

}