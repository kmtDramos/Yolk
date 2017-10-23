using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Controls_Medidor : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string Inicio()
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate(CDB Conn)
		{
			string Error = Conn.Mensaje;

			if (Conn.Conectado)
			{
				CObjeto Datos = new CObjeto();

				string Query = "SELECT TOP 1 CONVERT(VARCHAR,FECHA,120) AS FECHA,GENL1,GENL2,GENL3,GENC1,GENC2,GENC3,LUZSALITA,CONTCOMPRAS,LUZALMACEN,CONTLOG,LUZBODEGA,LUZLAB FROM DATOS ORDER BY FECHA DESC";
				Conn.DefinirQuery(Query);
				CObjeto Registro = Conn.ObtenerRegistro();

				CObjeto Medidor = new CObjeto();
				Medidor.Add("GENL1", Registro.Get("GENL1"));
				Medidor.Add("GENL2", Registro.Get("GENL2"));
				Medidor.Add("GENL3", Registro.Get("GENL3"));
				Medidor.Add("GENC1", Registro.Get("GENC1"));
				Medidor.Add("GENC2", Registro.Get("GENC2"));
				Medidor.Add("GENC3", Registro.Get("GENC3"));
				Medidor.Add("LUZSALITA", Registro.Get("LUZSALITA"));
				Medidor.Add("CONTCOMPRAS", Registro.Get("CONTCOMPRAS"));
				Medidor.Add("LUZALMACEN", Registro.Get("LUZALMACEN"));
				Medidor.Add("CONTLOG", Registro.Get("CONTLOG"));
				Medidor.Add("LUZBODEGA", Registro.Get("LUZBODEGA"));
				Medidor.Add("LUZLAB", Registro.Get("LUZLAB"));

				Datos.Add("Medidor", Medidor);

				Respuesta.Add("Datos", Datos);
			}

			Respuesta.Add("Error", Error);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string Reporte(DateTime Inicio, DateTime Fin)
	{
		CObjeto Respuesta = new CObjeto();

		CUnit.Firmado(delegate(CDB Conn)
		{
			string Error = Conn.Mensaje;
			if (Conn.Conectado)
			{
				CObjeto Datos = new CObjeto();

				string Query = "SELECT Circuito, 1000 AS [Meta KwH],KwH AS [Real KwH], 54 AS [Meta Horas uso], Horas AS [Real Horas uso]  FROM " +
				"(SELECT P.Circuito, SUM(P.Consumo) AS KwH, SUM(CASE WHEN P.Consumo > 0 THEN P.Minutos ELSE 0 END) / 60 AS Horas " +
				"FROM (SELECT (0.5) AS Minutos, LUZSALITA, CONTCOMPRAS, LUZALMACEN, CONTLOG, LUZBODEGA, LUZLAB FROM DATOS WHERE Fecha BETWEEN @Inicio AND @Fin) AS T " +
				"UNPIVOT(Consumo FOR Circuito IN (LUZSALITA,CONTCOMPRAS,LUZALMACEN,CONTLOG,LUZBODEGA,LUZLAB)) P " +
				"GROUP BY P.Circuito) R";
				Conn.DefinirQuery(Query);
				Conn.AgregarParametros("@Inicio", Inicio.ToString("yyyy-MM-dd HH:mm:ss"));
				Conn.AgregarParametros("@Fin", Fin.ToString("yyyy-MM-dd HH:mm:ss"));

				CArreglo Registros = Conn.ObtenerRegistros();

				Datos.Add("Reporte", Registros);
				Datos.Add("Inicio", Inicio.ToString("yyyy-MM-dd HH:mm:ss"));
				Datos.Add("Fin", Fin.ToString("yyyy-MM-dd HH:mm:ss"));

				Respuesta.Add("Datos", Datos);
			}
			Respuesta.Add("Error", Error);
		});

		return Respuesta.ToString();
	}

}