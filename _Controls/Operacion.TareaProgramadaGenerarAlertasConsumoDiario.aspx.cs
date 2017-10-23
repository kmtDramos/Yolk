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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Configuration;

public partial class _Controls_Operacion_TareaProgramadaGenerarAlertasConsumoDiario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GenerarReporteMantenimientos();
    }

    [WebMethod]
    public static string GenerarReporteMantenimientos()
    {
        string Redaccion = "";
        CObjeto Respuesta = new CObjeto();
        CUnit.Anonimo(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                CDB ConexionBaseDatos = new CDB();
                SqlConnection con = ConexionBaseDatos.conStr();
                //SqlCommand Stored = new SqlCommand("spr_Reporte_MedicionXDía_Mike", con);
                //Stored.CommandType = CommandType.StoredProcedure;
                //SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);
                //DataSet ds = new DataSet();
                //dataAdapterRegistros.Fill(ds);

                string spCorreos = "EXEC spr_Reporte_MedicionXDía_Mike";
                Conn.DefinirQuery(spCorreos);
                SqlDataReader Obten = Conn.Ejecutar();

                if (Obten.HasRows)
                {
                    while (Obten.Read())
                    {
                        Redaccion ="El circuito "+ Obten["Circuito"].ToString() +" del medidor "+Obten["Medidor"].ToString()+" del tablero "+ Obten["Tablero"].ToString()+" en la sucursal "+Obten["Sucursal"].ToString()+" ha excedido la meta estimada. Meta KwH: "+Obten["MetaKwH"].ToString()+". Consumo real KwH: "+Obten["RealKwH"].ToString();
                        string thisEnter = "\r\n";
                        string emailQuoteBody = "";

                        emailQuoteBody = emailQuoteBody + thisEnter + "<html>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "<head>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "<style>";

                        emailQuoteBody = emailQuoteBody + thisEnter + "</style>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "</head>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "<body>";

                        emailQuoteBody = emailQuoteBody + thisEnter + "<table align='center'>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "<tr>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "<td align='center'>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "<table>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "<tr>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "<td style='text-align:left;width:450px;'>";
                        emailQuoteBody = emailQuoteBody + thisEnter + Redaccion;
                        emailQuoteBody = emailQuoteBody + thisEnter + "</td>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "</tr>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "</table>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "</body>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "</head>";
                        emailQuoteBody = emailQuoteBody + thisEnter + "</html>";

                        MailMessage msg = new MailMessage();
                        msg.To.Add("mcuevas@keepmoving.com.mx");
                        msg.CC.Add(new MailAddress("mcuevas@keepmoving.com.mx"));
                        msg.From = new MailAddress("mcuevas@keepmoving.com.mx");
                        //msg.Bcc.Add(new MailAddress("tempo@keepmoving.com.mx"));
                        msg.Subject = "Generación de alertas, sistema medición Yolk";
                        msg.Body = "Se generaron nuevas alertas del día anterior, éstas mismas las podrá consultar en el sistema en la sección de reportes de mantenimiento.";
                        msg.IsBodyHtml = true;
                        msg.Priority = MailPriority.Normal;

                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailQuoteBody, null, "text/html");

                        msg.AlternateViews.Add(htmlView);

                        SmtpClient clienteSmtp = new SmtpClient();

                        try
                        {
                            clienteSmtp.Send(msg);
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                            Console.ReadLine();
                        }
                    }
                }
                Obten.Close();

                
                

            }
            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarReporte(int IdReporte)
    {
        JObject Respuesta = new JObject();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeConsultarReporteMantenimiento"))
            {
                if (conn.Conectado)
                {
                    string IdUsuario = Convert.ToString(CUsuario.ObtieneUsuarioSesion(conn));
                    int IdEstatus=0;
                    int bandera = 0;

                    JObject Reporte = new JObject();
                    string spReporte = "EXEC sp_Reporte_Consultar @Opcion";
                    conn.DefinirQuery(spReporte);
                    conn.AgregarParametros("@Opcion", 1);
                    CObjeto oMeta = conn.ObtenerRegistro();
                    Reporte.Add("IdReporte", oMeta.Get("IdReporte").ToString());
                    
                    Reporte.Add("Folio", oMeta.Get("Folio").ToString());
                    Reporte.Add("FechaLevantamiento", oMeta.Get("FechaLevantamiento").ToString());
                    Reporte.Add("FechaAtencion", oMeta.Get("FechaAtencion").ToString());
                    Reporte.Add("Estatus", oMeta.Get("Estatus").ToString());
                    Reporte.Add("Pais", oMeta.Get("Pais").ToString());
                    Reporte.Add("Estado", oMeta.Get("Estado").ToString());
                    Reporte.Add("Municipio", oMeta.Get("Municipio").ToString());
                    Reporte.Add("Sucursal", oMeta.Get("Sucursal").ToString());
                    Reporte.Add("Medidor", oMeta.Get("Medidor").ToString());
                    Reporte.Add("Tablero", oMeta.Get("Tablero").ToString());
                    Reporte.Add("Circuito", oMeta.Get("Circuito").ToString());
                    Reporte.Add("DescripcionCircuito", oMeta.Get("DescripcionCircuito").ToString());
                    Reporte.Add("TipoConsumo", oMeta.Get("TipoConsumo").ToString());
                    Reporte.Add("Responsable", oMeta.Get("Responsable").ToString());
                    Reporte.Add("IdTipoProblema", oMeta.Get("IdTipoProblema").ToString());
                    Reporte.Add("IdProblema", oMeta.Get("IdProblema").ToString());
                    Reporte.Add("IdProveedor", oMeta.Get("IdProveedor").ToString());
                    Reporte.Add("IdUsuarioProveedor", oMeta.Get("IdUsuarioProveedor").ToString());
                    Reporte.Add("TipoProblema", oMeta.Get("TipoProblema").ToString());
                    Reporte.Add("Problema", oMeta.Get("Problema").ToString());
                    Reporte.Add("Proveedor", oMeta.Get("Proveedor").ToString());
                    Reporte.Add("UsuarioProveedor", oMeta.Get("UsuarioProveedor").ToString());
                   
                    Reporte = CProblema.ObtenerJsonProblemas(Reporte);
                    Reporte = CTipoProblema.ObtenerJsonTipoProblemas(Reporte);
                    Reporte = CProveedor.ObtenerJsonProveedores(Reporte);
                    Reporte = CUsuarioProveedor.ObtenerJsonUsuarioProveedores(Reporte);


                    if(IdUsuario == oMeta.Get("IdUsuarioResponsable").ToString())
                    {
                        bandera = 1;
                        Reporte.Add("Bandera",bandera);
                    }

                    IdEstatus= Convert.ToInt32(oMeta.Get("IdEstatus").ToString());

                    //Cambiar Estatus a Leido si entra el responsable y si el estatus es el inicial                    
                    if (IdEstatus == 1 && bandera==1)
                    {
                        CReporte cReporte = new CReporte();
                        cReporte.IdReporte = IdReporte;
                        cReporte.Obtener(conn);
                        cReporte.IdEstatus = 2;
                        cReporte.Editar(conn);

                        IdEstatus = 2;
                    }

                    Reporte.Add("IdEstatus", IdEstatus);

                                     

                    Respuesta.Add(new JProperty("Reporte", Reporte));
                    Respuesta.Add("TA", "textarea");

                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ListarBitacoras(int Pagina, string Columna, string Orden, int IdReporte)
    {
        CObjeto Respuesta = new CObjeto();
        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = Conn.Mensaje;
            if (Conn.Conectado)
            {
                CObjeto Datos = new CObjeto();
                int Paginado = 3;
                int IdUsuario = CUsuario.ObtieneUsuarioSesion(Conn);
                CDB ConexionBaseDatos = new CDB();
                SqlConnection con = ConexionBaseDatos.conStr();
                SqlCommand Stored = new SqlCommand("spg_grdBitacora", con);
                Stored.CommandType = CommandType.StoredProcedure;
                Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = Paginado;
                Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = Pagina;
                Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = Columna;
                Stored.Parameters.Add("TipoOrden", SqlDbType.Text).Value = Orden;
                Stored.Parameters.Add("pIdReporte", SqlDbType.Int).Value = IdReporte;
                Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
                SqlDataAdapter dataAdapterRegistros = new SqlDataAdapter(Stored);

                DataSet ds = new DataSet();
                dataAdapterRegistros.Fill(ds);

                DataTable DataTablePaginador = ds.Tables[0];
                DataTable DataTableBitacora = ds.Tables[1];

                Datos.Add("Paginador", Conn.ObtenerRegistrosDataTable(DataTablePaginador));
                Datos.Add("Bitacora", Conn.ObtenerRegistrosDataTable(DataTableBitacora));
                Respuesta.Add("Datos", Datos);
            }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerProblemas(int IdTipoProblema)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {

            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "EXEC sp_Problema_Consultar @Opcion, @IdTipoProblema";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@Opcion", 1);
                conn.AgregarParametros("@IdTipoProblema", IdTipoProblema);
                CArreglo Problemas = conn.ObtenerRegistros();

                Datos.Add("Problemas", Problemas);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerUsuarioProveedores(int IdProveedor)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB conn)
        {
            
            string Error = conn.Mensaje;
            if (conn.Conectado)
            {
                CObjeto Datos = new CObjeto();

                string query = "EXEC sp_UsuarioProveedor_Consultar @Opcion, @IdProveedor";
                conn.DefinirQuery(query);
                conn.AgregarParametros("@Opcion", 1);
                conn.AgregarParametros("@IdProveedor", IdProveedor);
                CArreglo UsuarioProveedores = conn.ObtenerRegistros();

                Datos.Add("UsuarioProveedores", UsuarioProveedores);

                Respuesta.Add("Datos", Datos);
            }
            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AgregarBitacora(int IdReporte, string BitacoraDescripcion, bool EnviaCorreoIntegrante, bool EnviaCorreoProveedor)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarBitacoraReporteMantenimiento"))
            {
                if (Conn.Conectado)
                {
                    int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(Conn);

                    CObjeto Datos = new CObjeto();
                    CBitacora cBitacora = new CBitacora();
                    cBitacora.IdReporte = IdReporte;
                    cBitacora.Bitacora = BitacoraDescripcion;
                    cBitacora.IdUsuarioAlta = IdUsuarioSesion;
                    cBitacora.Fecha = DateTime.Now;
                    Error = ValidarAgregarBitacora(cBitacora);
                    if (Error == "")
                    {
                       //cBitacora.Agregar(Conn);

                        //EnviarCorreo
                        if (EnviaCorreoProveedor == true || EnviaCorreoIntegrante == true)
                        {
                            string To = "";
                            string Cc = "";
                            string Bcc = "";
                            string id = "";
                            string folio = "";
                            string fechalevantamiento = "";
                            string pais = "";
                            string estado = "";
                            string municipio = "";
                            string sucursal = "";
                            string medidor = "";
                            string tablero = "";
                            string circuito = "";
                            string descripcionCircuito = "";
                            string tipoConsumo = "";
                            string responsable = "";
                            string lugar = "";
                            string correoproveedor = "";
                            string correoresponsable = "";
                            string correos = ""; 
                           
                            string spReporte = "EXEC sp_Reporte_Consultar @Opcion";
                            Conn.DefinirQuery(spReporte);
                            Conn.AgregarParametros("@Opcion", 1);
                            CObjeto oMeta = Conn.ObtenerRegistro();
                            id = oMeta.Get("IdReporte").ToString();
                            folio = oMeta.Get("Folio").ToString();
                            fechalevantamiento = oMeta.Get("FechaLevantamiento").ToString();                            
                            pais = oMeta.Get("Pais").ToString();
                            estado =  oMeta.Get("Estado").ToString();
                            municipio = oMeta.Get("Municipio").ToString();
                            sucursal = oMeta.Get("Sucursal").ToString();
                            medidor = oMeta.Get("Medidor").ToString();
                            tablero =  oMeta.Get("Tablero").ToString();
                            circuito = oMeta.Get("Circuito").ToString();
                            descripcionCircuito =  oMeta.Get("DescripcionCircuito").ToString();
                            tipoConsumo =  oMeta.Get("TipoConsumo").ToString();
                            responsable = oMeta.Get("Responsable").ToString();
                            correoproveedor = oMeta.Get("CorreoProveedor").ToString();
                            correoresponsable = oMeta.Get("CorreoResponsable").ToString();


                            string spCorreos = "EXEC sp_Usuario_ConsultarCorreos @Opcion, @IdReporte";
                            Conn.DefinirQuery(spCorreos);
                            Conn.AgregarParametros("@Opcion", 1);
                            Conn.AgregarParametros("@IdReporte", IdReporte);
                            SqlDataReader Obten = Conn.Ejecutar();

                            if (Obten.HasRows)
                            {
                                while (Obten.Read())
                                {
                                    correos = correos + Obten["Correo"].ToString() + ";";
                                }
                            }
                            Obten.Close();

                            if (correos != "")
                            {
                                correos = correos.Substring(0, correos.Length - 1);
                            }

                            

                            if (EnviaCorreoProveedor == true && EnviaCorreoIntegrante == true)
                            {
                                To = correoproveedor;
                                Cc = correos;
                            }
                            else
                            {
                                if (EnviaCorreoProveedor == true && EnviaCorreoIntegrante == false)
                                {
                                    To = correoproveedor;
                                    Cc = correoresponsable;
                                }
                                else
                                {
                                    if (EnviaCorreoProveedor == false && EnviaCorreoIntegrante == true)
                                    {
                                        To = correos;
                                    }
                                }
                            }
                            Bcc = "";


                            lugar = municipio + ' ' + estado + ' ' + pais;

                            string html = "";
                            string thisEnter = "\r\n";

                            string separador = HttpContext.Current.Request.UrlReferrer.ToString();
                            string pagina = separador.Substring(0, separador.LastIndexOf("/") + 1);
                            string URLCorreo = pagina;

                            html = html + thisEnter + "<html>";
                            html = html + thisEnter + "<head></head>";
                            html = html + thisEnter + "<body>";
                            html = html + thisEnter + "<table>";
	                            html = html + thisEnter + "<tr>";
		                            html = html + thisEnter + "<td style='text-align: center; background-color: #f5f5f5;' colspan='4'><strong>Detalle</strong></td>";
	                            html = html + thisEnter + "</tr>";
	                            html = html + thisEnter + "<tr>";
		                            html = html + thisEnter + "<td><strong>Fecha levantamiento</strong></td>";
		                            html = html + thisEnter + "<td>"+fechalevantamiento+"</td>";
                                    html = html + thisEnter + "<td><strong>Responsable</strong></td>";
		                            html = html + thisEnter + "<td>"+responsable+"</td>";
                            html = html + thisEnter + " </tr>";
	                            html = html + thisEnter + "<tr>";
                                    html = html + thisEnter + "<td><strong>Lugar</strong></td>";
		                            html = html + thisEnter + "<td>"+lugar+"</td>";
                                    html = html + thisEnter + "<td><strong>Sucursal</strong></td>";
		                            html = html + thisEnter + "<td>"+sucursal+"</td>";
	                            html = html + thisEnter + "</tr>";
	                            html = html + thisEnter + "<tr>";
                                    html = html + thisEnter + "<td><strong>Medidor</strong></td>";
		                            html = html + thisEnter + "<td>"+medidor+"</td>";
                                    html = html + thisEnter + "<td><strong>Tablero</strong></td>";
		                            html = html + thisEnter + "<td>"+tablero+"</td>";
	                            html = html + thisEnter + "</tr>";
	                            html = html + thisEnter + "<tr>";
                                    html = html + thisEnter + "<td><strong>Circuito</strong></td>";
		                            html = html + thisEnter + "<td>"+circuito+"</td>";
                                    html = html + thisEnter + "<td><strong>Descripcion</strong></td>";
		                            html = html + thisEnter + "<td>"+descripcionCircuito+"</td>";
	                            html = html + thisEnter + "</tr>";
	                            html = html + thisEnter + "<tr>";
                                    html = html + thisEnter + "<td><strong>Tipo Consumo</strong></td>";
		                            html = html + thisEnter + "<td>"+tipoConsumo+"</td>";
                                    html = html + thisEnter + "<td><strong>Consumo por día</strong></td>";
		                            html = html + thisEnter + "<td></td>";
	                            html = html + thisEnter + "</tr>";
	                            html = html + thisEnter + "<tr>";
		                            html = html + thisEnter + "<td style='text-align: center; background-color: #f5f5f5;' colspan='4'><strong>Comentario</strong></td>";
	                            html = html + thisEnter + "</tr>";
		                            html = html + thisEnter + "<tr>";
                                    html = html + thisEnter + "<td colspan='4'>"+BitacoraDescripcion+"</td>";
	                            html = html + thisEnter + "</tr>";
                            html = html + thisEnter + "</table>";

                            html = html + thisEnter + "<br/><br/>Favor de iniciar sesión para dar seguimiento <a name='aceptar' href='" + URLCorreo + "'  class='enlaceboton' style='text-decoration: none'  title=' Ir a sitio web '>Visitar sitio</a> ";

                            html = html + thisEnter + "</body>";
                            html = html + thisEnter + "</html>";

                           CMail.EnviarCorreo("keepunit@keepmoving.com.mx", To,Cc,Bcc, "Asunto", html);
                        }


                    }
                    Respuesta.Add("Datos", Datos);
                }
                else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    private static string ValidarAgregarBitacora(CBitacora Bitacora)
    {
        string Mensaje = "";
        Mensaje += (Bitacora.Bitacora == "") ? "<li>Favor de completar el campo de descripción.</li>" : Mensaje;

        return Mensaje;
    }

    [WebMethod]
    public static string ObtenerFormaEditarSeguimiento(int IdReporte)
    {
        JObject Respuesta = new JObject();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarSeguimientoReporteMantenimiento"))
            {
                if (conn.Conectado)
                {

                    JObject Reporte = new JObject();
                    string spReporte = "EXEC sp_Reporte_Consultar @Opcion";
                    conn.DefinirQuery(spReporte);
                    conn.AgregarParametros("@Opcion", 1);
                    CObjeto oMeta = conn.ObtenerRegistro();
                    Reporte.Add("IdReporte", oMeta.Get("IdReporte").ToString());
                    
                    Reporte.Add("IdTipoProblema", oMeta.Get("IdTipoProblema").ToString());
                    Reporte.Add("TipoProblema", oMeta.Get("TipoProblema").ToString());
                    Reporte.Add("IdProblema", oMeta.Get("IdProblema").ToString());
                    Reporte.Add("Problema", oMeta.Get("Problema").ToString());
                    Reporte.Add("IdProveedor", oMeta.Get("IdProveedor").ToString());
                    Reporte.Add("Proveedor", oMeta.Get("Proveedor").ToString());
                    Reporte.Add("IdUsuarioProveedor", oMeta.Get("IdUsuarioProveedor").ToString());
                    Reporte.Add("UsuarioProveedor", oMeta.Get("UsuarioProveedor").ToString());

                    Reporte = CProblema.ObtenerJsonProblemas(Reporte);
                    Reporte = CTipoProblema.ObtenerJsonTipoProblemas(Reporte);
                    Reporte = CProveedor.ObtenerJsonProveedoresTodos(Reporte);
                    Reporte = CUsuarioProveedor.ObtenerJsonUsuarioProveedores(Reporte);


                    Respuesta.Add(new JProperty("Reporte", Reporte));

                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarSeguimiento(int IdReporte, int IdTipoProblema, int IdProblema, int IdProveedor, int IdUsuarioProveedor)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEditarSeguimientoReporteMantenimiento"))
            {
                if (Conn.Conectado)
                {
                    int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(Conn);

                    CObjeto Datos = new CObjeto();
                    CReporte Reporte = new CReporte();
                    Reporte.IdReporte = IdReporte;
                    Reporte.Obtener(Conn); 
                    Reporte.IdTipoProblema = IdTipoProblema;
                    Reporte.IdProveedor = IdProveedor;
                    Reporte.IdUsuarioProveedor = IdUsuarioProveedor;
                    //Error = ValidarAgregarReporte(Reporte);
                    //if (Error == "")
                    //{
                        Reporte.Editar(Conn);
                    //}

                        string spReporte = "EXEC sp_Reporte_Consultar @Opcion";
                        Conn.DefinirQuery(spReporte);
                        Conn.AgregarParametros("@Opcion", 1);
                        CObjeto oMeta = Conn.ObtenerRegistro();
                        Datos.Add("IdReporte", oMeta.Get("IdReporte").ToString());
                        Datos.Add("TipoProblema", oMeta.Get("TipoProblema").ToString());
                        Datos.Add("Problema", oMeta.Get("Problema").ToString());
                        Datos.Add("Proveedor", oMeta.Get("Proveedor").ToString());
                        Datos.Add("UsuarioProveedor", oMeta.Get("UsuarioProveedor").ToString());

                    Respuesta.Add("Datos", Datos);
                }
                else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarDocumento(int IdReporte)
    {
        JObject Respuesta = new JObject();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeConsultarReporteMantenimiento"))
            {
                if (conn.Conectado)
                {

                    string IdUsuario = Convert.ToString(CUsuario.ObtieneUsuarioSesion(conn));
                    int IdEstatus = 0;
                    int bandera = 0;

                    JObject Reporte = new JObject();
                    CReporte cReporte = new CReporte();
                    cReporte.IdReporte = IdReporte;
                    cReporte.Obtener(conn);
                    Reporte.Add("IdEstatus", Convert.ToInt32(cReporte.IdEstatus));
                    Reporte.Add("IdReporte", Convert.ToInt32(IdReporte));
                    Reporte = CReporteDocumento.ObtenerJsonReporteDocumentos(Reporte);

                    if (Convert.ToInt32(IdUsuario) == cReporte.IdUsuarioResponsable)
                    {
                        bandera = 1;
                        Reporte.Add("Bandera", bandera);
                    }



                    Respuesta.Add(new JProperty("Reporte", Reporte));

                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string actualizaDescripcionDocumento(Dictionary<string, object> Documento)
    {
        JObject Respuesta = new JObject();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeAgregarDocumentoReporteMantenimiento"))
            {
                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CReporteDocumento cReporteDocumento = new CReporteDocumento();
                    cReporteDocumento.IdReporteDocumento = Convert.ToInt32(Documento["pIdReporteDocumento"]);
                    cReporteDocumento.Obtener(conn);
                    cReporteDocumento.Descripcion = Convert.ToString(Documento["pDescripcion"]);
                    cReporteDocumento.Editar(conn);

                    Respuesta.Add("IdReporteDocumento", cReporteDocumento.IdReporteDocumento);
                    Respuesta.Add("Documento", cReporteDocumento.Documento);
                    Respuesta.Add("Descripcion", cReporteDocumento.Descripcion);
                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();    
    }

    [WebMethod]
    public static string EliminarDocumento(int pIdReporteDocumento)
    {
        JObject Respuesta = new JObject();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEliminarDocumentoReporteMantenimiento"))
            {
                if (conn.Conectado)
                {
                    CObjeto Datos = new CObjeto();
                    CReporteDocumento cReporteDocumento = new CReporteDocumento();
                    cReporteDocumento.IdReporteDocumento = pIdReporteDocumento;
                    cReporteDocumento.Eliminar(conn);


                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEntregarReporte(int IdReporte)
    {
        JObject Respuesta = new JObject();

        CUnit.Firmado(delegate(CDB conn)
        {
            string Error = conn.Mensaje;
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEntregarReporteMantenimiento"))
            {
                if (conn.Conectado)
                {

                    JObject Reporte = new JObject();
                    string spReporte = "EXEC sp_Reporte_Consultar @Opcion";
                    conn.DefinirQuery(spReporte);
                    conn.AgregarParametros("@Opcion", 1);
                    CObjeto oMeta = conn.ObtenerRegistro();
                    Reporte.Add("IdReporte", oMeta.Get("IdReporte").ToString());
                    Respuesta.Add("TAEntrega", "textarea");
                    Respuesta.Add(new JProperty("Reporte", Reporte));

                }
                else { Error = Error + "<li>" + conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EntregarReporte(int IdReporte)
    {
        CObjeto Respuesta = new CObjeto();

        CUnit.Firmado(delegate(CDB Conn)
        {
            string Error = "";
            CSecurity permiso = new CSecurity();
            if (permiso.tienePermiso("puedeEntregarReporteMantenimiento"))
            {
                if (Conn.Conectado)
                {
                    int IdUsuarioSesion = CUsuario.ObtieneUsuarioSesion(Conn);

                    CObjeto Datos = new CObjeto();
                    CReporte cReporte = new CReporte();
                    cReporte.IdReporte = IdReporte;
                    cReporte.Obtener(Conn); 
                    cReporte.FechaCierre = DateTime.Now;
                    cReporte.IdEstatus = 4;
                    
                    int Documento = CReporte.ValidaExisteDocumento(IdReporte, Conn);
                    if (Documento == 0)
                    {
                        Error = Error + "Favor de agregar documentos.";
                    }
                    else
                    {
                        cReporte.Editar(Conn);
                    }
                        
                    
                    Respuesta.Add("Datos", Datos);
                }
                else { Error = Error + "<li>" + Conn.Mensaje + "</li>"; }
            }
            else { Error = Error + "<li>No tienes los permisos necesarios</li>"; }

            Respuesta.Add("Error", Error);
        });

        return Respuesta.ToString();
    }
}