using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;

public class CMD5
{
	
	public static string Encriptar(string Cadena)
	{
		string hash = "";

		MD5 convertidor = new MD5CryptoServiceProvider();

		convertidor.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Cadena));

		byte[] datos = convertidor.Hash;

		StringBuilder constructor = new StringBuilder();

		for (int i = 0; i < datos.Length; i++)
			constructor.Append(datos[i].ToString("x2"));

		hash = constructor.ToString();

		return hash;
	}

}