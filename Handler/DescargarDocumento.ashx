<%@ WebHandler Language="C#" Class="DescargarDocumento" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class DescargarDocumento : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string pDocumento = context.Request.QueryString["Doc"].ToString();

        CReporteDocumento miDocumento = new CReporteDocumento();
        JObject miJDocumento = miDocumento.DescargarDocumento(pDocumento);

        string Filename = HttpContext.Current.Server.MapPath("~/Archivos/ReporteMantenimiento/" + miJDocumento.Property("Documento").Value.ToString());

        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        context.Response.ContentType = miJDocumento.Property("TipoDocumento").Value.ToString();
        response.AddHeader("Content-Disposition", "attachment; filename=" + miJDocumento.Property("Documento").Value.ToString() + ";");
        response.TransmitFile(Filename);
        response.Flush();

        if (File.Exists(Filename))
        {
            File.Delete(Filename);
        }

        response.End();
    }
 
    public bool IsReusable 
    {
        get {
            return false;
        }
    }

}