using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CViews
{

	//PROD
	private static string CarpetaViews = @"C:\inetpub\wwwrootsapps\ProductionApplications\Medidor17082017\_Views";
	private static string Domain = "http://www.kmt.tsk.com.mx/Medidor17082017/";
	
	//private static string CarpetaViews = @"C:\inetpub\wwwroot\Medidor\_Views";
	//private static string Domain = "http://localhost:8080/Medidor/";
	
	public static string CargarView(string NombreView)
	{
		string Contenido = "";

		if (System.IO.File.Exists(CarpetaViews + @"\" + NombreView))
			Contenido = System.IO.File.ReadAllText(CarpetaViews + @"\" + NombreView);

		Contenido = Contenido.Replace("[DOMAIN]", Domain);
		Contenido = Contenido.Replace("[TICKS]", DateTime.Now.Ticks.ToString());

		return Contenido;
	}

}