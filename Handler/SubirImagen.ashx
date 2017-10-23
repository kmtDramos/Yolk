<%@ WebHandler Language="C#" Class="SubirImagen" %>

using System.Linq;
using System.Web.UI;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.IO.IsolatedStorage;
using System.Web.SessionState;
using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

public class SubirImagen : IHttpHandler {
    
    public void ProcessRequest(HttpContext context)
    {
        string IdCliente = HttpContext.Current.Request.Params["pIdCliente"];
        string filename = string.Empty;
        string extension = string.Empty;
        context.Response.ContentType = "text/plain";
        context.Response.Expires = -1;

        filename = HttpContext.Current.Request.Headers["X-File-Name"];
        extension = filename.Substring(filename.LastIndexOf("."));

        if (extension == ".png" | extension == ".bmp" | extension == ".bmp" | extension == ".gif" | extension == ".jpg" | extension == ".jpeg")
        {
            filename = string.Empty;
            filename = "Logo" + "_" + IdCliente.ToString().Trim() + ".png";

            string ruta = HttpContext.Current.Server.MapPath("~") + "\\Archivos\\Logo";

            if (System.IO.File.Exists(ruta + "\\" + filename))
            {
                System.IO.File.Delete(ruta + "\\" + filename);
            }
            Stream inputStream = HttpContext.Current.Request.InputStream;
            FileStream fileStream = new FileStream(ruta + "\\" + filename, FileMode.OpenOrCreate, FileAccess.Write);

            byte[] bytesInStream = new byte[inputStream.Length];
            inputStream.Read(bytesInStream, 0, (int)bytesInStream.Length);
            fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            fileStream.Close();

            HttpContext.Current.Response.Write("{success:true, newFileName:'" + filename + "', name:'undisclosed', path:'undisclosed/undisclosed', message:'<ol>El archivo se guardo con exito.</ol>'}");
        }
        else
        {
            HttpContext.Current.Response.Write("{success:false, name:'undisclosed', path:'undisclosed/undisclosed', message:'<ol>El archivo no tiene la extencion correcta.</ol>'}");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}