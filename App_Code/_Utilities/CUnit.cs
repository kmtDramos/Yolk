using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
public class CUnit
{

	public static void Accion(Action<CDB> fn)
	{
		CDB conn = new CDB();
		fn(conn);
		conn.Cerrar();
	}

	public static void Anonimo(Action<CDB> fn)
	{
		CDB conn = new CDB();
		fn(conn);
		conn.Cerrar();
	}

	public static void Firmado(Action<CDB> fn)
	{
        //En cada new CDB manda a abrir la bd
		CDB conn = new CDB();
		if (conn.Conectado)
		{
			if (CSecurity.HaySesion(conn))
			{
				fn(conn);
			}
			else
			{
				HttpContext.Current.Response.Redirect("../Medidor17082017/");
			}
			conn.Cerrar();
		}
	}

}