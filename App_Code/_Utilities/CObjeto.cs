using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CObjeto
{ 
	private Dictionary<string, object> objeto;

	public CObjeto()
	{
		objeto = new Dictionary<string, object>();
	}

	public void Add(string Propiedad, CObjeto Valor)
	{
		objeto.Add(Propiedad, Valor.ToDictionary());
	}

	public void Add(string Propiedad, CArreglo Valor)
	{
		objeto.Add(Propiedad, Valor.ToList());
	}

	public void Add(string Propiedad, object Valor)
	{
		objeto.Add(Propiedad, Valor);
	}

	public void Remove(string Propiedad)
	{
		objeto.Remove(Propiedad);
	}
	
	public object Get(string Propiedad)
	{
		return objeto[Propiedad];
	}

	public bool Exist(string Propiedad)
	{
		return objeto.ContainsKey(Propiedad);
	}
	
	public Dictionary<string, object> ToDictionary()
	{
		return objeto;
	}

	override public string ToString()
	{
		string json = CJson.Stringify(objeto);
		json = json.Replace(@"\","");
		return json;
	}

}