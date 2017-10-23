using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


public class CUsuarioPermiso
{

	private int idusuariopermiso = 0;
	private int idusuario = 0;
	private int idpermiso = 0;
	private bool baja = false;

	public int IdUsuarioPermiso
	{
		get
		{
			return idusuariopermiso;
		}
		set
		{
			idusuariopermiso = value;
		}
	}

	public int IdUsuario
	{
		get
		{
			return idusuario;
		}
		set
		{
			idusuario = value;
		}
	}

	public int IdPermiso
	{
		get
		{
			return idpermiso;
		}
		set
		{
			idpermiso = value;
		}
	}

	public bool Estatus
	{
		get
		{
			return baja;
		}
		set
		{
			baja = value;
		}
	}

	public void Obtener(CDB conn)
	{
		string query = "SELECT * FROM UsuarioPermiso WHERE IdUsuarioPermiso = @IdUsuarioPermiso";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdUsuarioPermiso", idusuariopermiso);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	public void Agregar(CDB conn)
	{
		string query = "INSERT INTO UsuarioPermiso (IdUsuario,IdPermiso,Estatus) VALUES (@IdUsuario,@IdPermiso,@Estatus) " +
			"SELECT * FROM UsuarioPermiso WHERE IdUsuarioPermiso = SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdUsuario", idusuario);
		conn.AgregarParametros("@IdPermiso", idpermiso);
		conn.AgregarParametros("@Estatus", baja);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	public void Editar(CDB conn)
	{
		string query = "UPDATE UsuarioPermiso SET IdUsuario=@IdUsuario,IdPermiso=@IdPermiso,Estatus=@Estatus WHERE IdUsuarioPermiso=@IdPermiso "+
			"SELECT * FROM UsuarioPermiso WHERE IdPermiso=SCOPE_IDENTITY()";
		conn.DefinirQuery(query);
		conn.AgregarParametros("@IdUsuario", idusuario);
		conn.AgregarParametros("@IdPermiso", idpermiso);
		conn.AgregarParametros("@Estatus", baja);
		conn.AgregarParametros("@IdUsuarioPermiso", idusuariopermiso);
		SqlDataReader Datos = conn.Ejecutar();
		DefinirPropiedades(Datos);
		Datos.Close();
	}

	private void DefinirPropiedades(SqlDataReader Datos)
	{
		if (Datos.HasRows)
		{
			while(Datos.Read())
			{
				idusuariopermiso = !(Datos["IdUsuarioPermiso"] is DBNull) ? Convert.ToInt32(Datos["IdUsuarioPermiso"]) : 0;
				idusuario = !(Datos["IdUsuario"] is DBNull) ? Convert.ToInt32(Datos["IdUsuario"]) : 0;
				idpermiso = !(Datos["IdPermiso"] is DBNull) ? Convert.ToInt32(Datos["IdPermiso"]) : 0;
				baja = !(Datos["Estatus"] is DBNull) ? Convert.ToBoolean(Datos["Estatus"]) : false;
			}
		}
	}

}