using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CArreglo
/// </summary>
public class CArreglo
{

	private List<object> arreglo;

	public CArreglo()
	{
		arreglo = new List<object>();
	}

	public void Add(CObjeto Objeto)
	{
		arreglo.Add(Objeto.ToDictionary());
	}

	public void Add(CArreglo Arreglo)
	{
		arreglo.Add(Arreglo.ToList());
	}

	public void Add(object Valor)
	{
		arreglo.Add(Valor);
	}

	public void Remove(int Index)
	{
		arreglo.Remove(arreglo[Index]);
	}

	public int Count()
	{
		return arreglo.Count;
	}

	public object Get(int Index)
	{
		return arreglo[Index];
	}

	public List<object> ToList()
	{
		return arreglo;
	}

	override public string ToString()
	{
		string json = CJson.Stringify(arreglo);
		json = json.Replace(@"\", "");
		return json;
	}

}