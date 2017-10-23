using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

public class CJson
{
	
	// Convertir objetos a json
	public static string Stringify(object Obj)
	{
        var serializer = new JavaScriptSerializer();
        serializer.MaxJsonLength = 2147483644;
                                 

		return serializer.Serialize(Obj);
	}

}