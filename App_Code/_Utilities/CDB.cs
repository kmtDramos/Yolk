using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;
using System.Data;



public class CDB
{
    private const int COMMANDTIMEOUT = 0;
	public bool Conectado = false;
	public string Mensaje = "";

	private SqlConnection conn;
	private string connStr = "";
	private SqlCommand cmd;

	public CDB()
	{

		//connStr = ConfigurationManager.ConnectionStrings["ServidorSQL"].ConnectionString;
		connStr = ConfigurationManager.ConnectionStrings["LocalhostSQL"].ConnectionString;
		conn = new SqlConnection(connStr);

		try
		{
			conn.Open();
			Conectado = true;
			Mensaje = "";
		}
		catch (Exception e)
		{
			Conectado = false;
			Mensaje = e.Message + " - " + e.StackTrace;
		}

	}

    public SqlConnection conStr()
    {
        connStr = ConfigurationManager.ConnectionStrings["LocalhostSQL"].ConnectionString;
        conn = new SqlConnection(connStr);
        return conn;
    }

	public void DefinirStoreProcedure(string Procedimiento)
	{
		cmd.CommandText = Procedimiento;
	}

	public void AgregarParametros(string Parametro, object Valor)
	{
		cmd.Parameters.AddWithValue(Parametro, Valor);
	}

	public void DefinirQuery(string Query)
	{

		cmd = new SqlCommand(Query, conn);
        cmd.CommandTimeout = COMMANDTIMEOUT;
	}

	public SqlDataReader EjecutarStoreProcedure()
	{
        SqlDataReader result;
        cmd.CommandTimeout = COMMANDTIMEOUT;
        result = cmd.ExecuteReader();
        return result;
	}
	
	public SqlDataReader Ejecutar()
	{
        SqlDataReader result;
        cmd.CommandTimeout = COMMANDTIMEOUT;
        result = cmd.ExecuteReader();
        return result;
	}

	public CArreglo ObtenerRegistros()
	{
		CArreglo Registros = new CArreglo();
        SqlDataReader Datos;
        cmd.CommandTimeout = COMMANDTIMEOUT;
		Datos = cmd.ExecuteReader();

		if (Datos.HasRows)
		{
            //Por cada registro entra
			while (Datos.Read())
			{
				CObjeto Registro = new CObjeto();                
                //Por cada columna mete todos al objeto
				for (int i = 0; i < Datos.FieldCount; i++)
				{
					Registro.Add(Datos.GetName(i), Datos[i]);
				}
				Registros.Add(Registro);
			}
		}
		Datos.Close();

		return Registros;
	}

	public CObjeto ObtenerRegistro()
	{
		CObjeto Registro = new CObjeto();
        SqlDataReader Datos;
        cmd.CommandTimeout = COMMANDTIMEOUT;
        Datos = cmd.ExecuteReader();

		if (Datos.HasRows)
		{
			while (Datos.Read())
			{
				for (int i = 0; i < Datos.FieldCount; i++)
				{
					Registro.Add(Datos.GetName(i), Datos[i]);
				}
			}
		}

		Datos.Close();

		return Registro;
	}

    public CArreglo ObtenerRegistrosDataTable(DataTable datatable)
    {
        CArreglo Registros = new CArreglo();
        int totalFilas = datatable.Rows.Count;
        int totalColumnas = datatable.Columns.Count;

        if (totalFilas>0)
        {
            int a = 0;
            while (a < totalFilas)
            {
                CObjeto Registro = new CObjeto();
                for (int i = 0; i < totalColumnas; i++)
                {
                    Registro.Add(datatable.Columns[i].ToString(), datatable.Rows[a][i].ToString());
                }
                Registros.Add(Registro);
                a++; ;
            }
        }
        return Registros;
    }


	public void Cerrar()
	{
		conn.Close();
	}
}