using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class CMedidor
{

	private int id = 0;
	private DateTime fecha = new DateTime();
	private decimal genl1 = 0;
	private decimal genl2 = 0;
	private decimal genl3 = 0;
	private decimal genc1 = 0;
	private decimal genc2 = 0;
	private decimal genc3 = 0;
	private bool i1 = false;
	private bool i2 = false;
	private bool i3 = false;
	private bool i4 = false;
	private bool i5 = false;
	private bool i6 = false;
	private bool i7 = false;
	private bool i8 = false;
	private bool i9 = false;
	private bool i10 = false;
	private bool i11 = false;
	private bool i12 = false;
	private bool i13 = false;
	private bool i14 = false;
	private decimal luzsalita = 0;
	private decimal contcompras = 0;
	private decimal luzalmacen = 0;
	private decimal contlog = 0;
	private decimal luzbodega = 0;
	private decimal luzlab = 0;

	public int ID
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public DateTime FECHA
	{
		get
		{
			return fecha;
		}
		set
		{
			fecha = value;
		}
	}

	public decimal GENL1
	{
		get
		{
			return genl1;
		}
		set
		{
			genl1 = value;
		}
	}

	public decimal GENL2
	{
		get
		{
			return genl2;
		}
		set
		{
			genl2 = value;
		}
	}

	public decimal GENL3
	{
		get
		{
			return genl3;
		}
		set
		{
			genl3 = value;
		}
	}

	public decimal GENC1
	{
		get
		{
			return genc1;
		}
		set
		{
			genc1 = value;
		}
	}

	public bool I1
	{
		get
		{
			return i1;
		}
		set
		{
			i1 = value;
		}
	}

	public bool I2
	{
		get
		{
			return i2;
		}
		set
		{
			i2 = value;
		}
	}

	public bool I3
	{
		get
		{
			return i3;
		}
		set
		{
			i3 = value;
		}
	}

	public bool I4
	{
		get
		{
			return i4;
		}
		set
		{
			i4 = value;
		}
	}

	public bool I5
	{
		get
		{
			return i5;
		}
		set
		{
			i5 = value;
		}
	}

	public bool I6
	{
		get
		{
			return i6;
		}
		set
		{
			i6 = value;
		}
	}

	public bool I7
	{
		get
		{
			return i7;
		}
		set
		{
			i7 = value;
		}
	}

	public bool I8
	{
		get
		{
			return i8;
		}
		set
		{
			i8 = value;
		}
	}

	public bool I9
	{
		get
		{
			return i9;
		}
		set
		{
			i9 = value;
		}
	}

	public bool I10
	{
		get
		{
			return i10;
		}
		set
		{
			i10 = value;
		}
	}

	public bool I11
	{
		get
		{
			return i6;
		}
		set
		{
			i6 = value;
		}
	}

	public bool I12
	{
		get
		{
			return i12;
		}
		set
		{
			i12 = value;
		}
	}

	public bool I13
	{
		get
		{
			return i13;
		}
		set
		{
			i13 = value;
		}
	}

	public bool I14
	{
		get
		{
			return i14;
		}
		set
		{
			i14 = value;
		}
	}
	
	public decimal LUZSALITA
	{
		get
		{
			return luzsalita;
		}
		set
		{
			luzsalita = value;
		}
	}

	public decimal CONTCOMPRAS
	{
		get
		{
			return contcompras;
		}
		set
		{
			contcompras = value;
		}
	}

	public decimal LUZALMACEN
	{
		get
		{
			return luzalmacen;
		}
		set
		{
			luzalmacen = value;
		}
	}

	public decimal CONTLOG
	{
		get
		{
			return contlog;
		}
		set
		{
			contlog = value;
		}
	}

	public decimal LUZBODEGA
	{
		get
		{
			return luzbodega;
		}
		set
		{
			luzbodega = value;
		}
	}

	public decimal LUZLAB
	{
		get
		{
			return luzlab;
		}
		set
		{
			luzlab = value;
		}
	}

	// Cargar Usuario
	public void Obtener(CDB Conn)
	{
		if (id != 0)
		{
			string Query = "SELECT * FROM DATOS WHERE ID = @ID";
			Conn.DefinirQuery(Query);
			Conn.AgregarParametros("@ID", id);
			SqlDataReader Datos = Conn.Ejecutar();
			DefinirPropiedades(Datos);
			Datos.Close();
		}
	}

	// Definir valores de instancia
	private void DefinirPropiedades(SqlDataReader Datos)
	{
		if (Datos.HasRows)
		{
			while (Datos.Read())
			{
				id = !(Datos["ID"] is DBNull) ? Convert.ToInt32(Datos["ID"]) : id;
				fecha = !(Datos["FECHA"] is DBNull) ? Convert.ToDateTime(Datos["FECHA"]) : fecha;
				genl1 = !(Datos["GENL1"] is DBNull) ? Convert.ToInt32(Datos["GENL1"]) : genl1;
				genl2 = !(Datos["GENL2"] is DBNull) ? Convert.ToInt32(Datos["GENL2"]) : genl2;
				genl3 = !(Datos["GENL3"] is DBNull) ? Convert.ToInt32(Datos["GENL3"]) : genl3;
				genc1 = !(Datos["GENC1"] is DBNull) ? Convert.ToInt32(Datos["GENC1"]) : genc1;
				genc2 = !(Datos["GENC2"] is DBNull) ? Convert.ToInt32(Datos["GENC2"]) : genc2;
				genc3 = !(Datos["GENC3"] is DBNull) ? Convert.ToInt32(Datos["GENC3"]) : genc3;
				i1 = !(Datos["I1"] is DBNull) ? Convert.ToBoolean(Datos["I1"]) : i1;
				i2 = !(Datos["I2"] is DBNull) ? Convert.ToBoolean(Datos["I2"]) : i2;
				i3 = !(Datos["I3"] is DBNull) ? Convert.ToBoolean(Datos["I3"]) : i3;
				i4 = !(Datos["I4"] is DBNull) ? Convert.ToBoolean(Datos["I4"]) : i4;
				i5 = !(Datos["I5"] is DBNull) ? Convert.ToBoolean(Datos["I5"]) : i5;
				i6 = !(Datos["I6"] is DBNull) ? Convert.ToBoolean(Datos["I6"]) : i6;
				i7 = !(Datos["I7"] is DBNull) ? Convert.ToBoolean(Datos["I7"]) : i7;
				i8 = !(Datos["I8"] is DBNull) ? Convert.ToBoolean(Datos["I8"]) : i8;
				i9 = !(Datos["I9"] is DBNull) ? Convert.ToBoolean(Datos["I9"]) : i9;
				i10 = !(Datos["I10"] is DBNull) ? Convert.ToBoolean(Datos["I10"]) : i10;
				i11 = !(Datos["I11"] is DBNull) ? Convert.ToBoolean(Datos["I11"]) : i11;
				i12 = !(Datos["I12"] is DBNull) ? Convert.ToBoolean(Datos["I12"]) : i12;
				i13 = !(Datos["I13"] is DBNull) ? Convert.ToBoolean(Datos["I13"]) : i13;
				i14 = !(Datos["I14"] is DBNull) ? Convert.ToBoolean(Datos["I14"]) : i14;
				luzsalita = !(Datos["ID"] is DBNull) ? Convert.ToInt32(Datos["ID"]) : luzsalita;
				contcompras = !(Datos["ID"] is DBNull) ? Convert.ToInt32(Datos["ID"]) : contcompras;
				luzalmacen = !(Datos["ID"] is DBNull) ? Convert.ToInt32(Datos["ID"]) : luzalmacen;
				contlog = !(Datos["ID"] is DBNull) ? Convert.ToInt32(Datos["ID"]) : contlog;
				luzbodega = !(Datos["ID"] is DBNull) ? Convert.ToInt32(Datos["ID"]) : luzbodega;
				luzlab = !(Datos["ID"] is DBNull) ? Convert.ToInt32(Datos["ID"]) : luzlab;
			}
		}
	}

}