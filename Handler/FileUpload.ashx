<%@ WebHandler Language="C#" Class="FileUpload" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;

public class FileUpload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        if (context.Request.Files.Count == 0)
        {

            LogRequest("No se envio ningun archivo");
            context.Response.ContentType = "text/plain";
            context.Response.Write("No se recibieron archivos.");

        }
        else
        {
            CUnit.Firmado(delegate(CDB Conn)
            {
                if (Conn.Conectado)
                {
                    int pIdReporte = Convert.ToInt32(context.Request.QueryString["pReporte"].ToString());
                    int pIdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);

                    
                    
                    
                    HttpPostedFile uploadedfile = context.Request.Files[0];
                    string FileName = uploadedfile.FileName;
                    string FileType = uploadedfile.ContentType;
                    int FileSize = uploadedfile.ContentLength;

                    DateTime Fecha = DateTime.Now;
                    string documento = "";
                    documento = uploadedfile.FileName;
                    string extension = documento.Substring(documento.LastIndexOf("."));
                    documento = "Documento" + "_" + Fecha.ToString("yyyyMMddHHmmss") + "_" + pIdReporte + extension;
                              
            
                    LogRequest(documento + ", " + FileType + ", " + FileSize);
                    string pRuta = HttpContext.Current.Server.MapPath("~/Archivos/ReporteMantenimiento") + "\\" + documento;
                    uploadedfile.SaveAs(pRuta);

                    int IdReporteArchivo = 0;            
            
                    CReporteDocumento ReporteDocumento = new CReporteDocumento();
                    IdReporteArchivo = ReporteDocumento.AgregarDocumento(documento, FileType, pIdReporte, pIdUsuario, Conn);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write("{\"name\":\"" + documento + "\",\"type\":\"" + FileType + "\",\"IdReporteDocumento\":\"" + IdReporteArchivo + "\",\"size\":\"" + FileSize + "\",\"url\":\"/Handler/FileUpload.ashx?IdReporteArchivo=" + IdReporteArchivo + "\"}");  
                }
            });
            
            
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private void LogRequest(string Log)
    {
        StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/Log") + "\\LogReporteDocumento.txt", true);
        sw.WriteLine(DateTime.Now.ToString() + " - " + Log);
        sw.Flush();
        sw.Close();
    }

}