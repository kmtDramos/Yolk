using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CFecha
/// </summary>
public class CFecha
{

	private DateTime fecha;

	public CFecha()
	{
		fecha = new DateTime(1900, 1, 1);
	}

	public void Today()
	{
		fecha = DateTime.Today;
	}

	public void Now()
	{
		fecha = DateTime.Now;
	}

	public void AddMilliseconds(int Milliseconds)
	{
		fecha.AddMilliseconds(Milliseconds);
	}

	public void AddSeconds(int Seconds)
	{
		fecha.AddSeconds(Seconds);
	}

	public void AddMinutes(int Minutes)
	{
		fecha.AddMinutes(Minutes);
	}

	public void AddHours(int Hours)
	{
		fecha.AddHours(Hours);
	}

	public void AddDays(int Days)
	{
		fecha.AddDays(Days);
	}

	public void AddMonths(int Months)
	{
		fecha.AddMonths(Months);
	}

	public void AddYears(int Years)
	{
		fecha.AddYears(Years);
	}

	override public string ToString()
	{
		string sFecha = "";
		sFecha = fecha.Day.ToString("00") + "/" + fecha.Month.ToString("00") + "/" + fecha.Year + " " + fecha.Hour.ToString("00") + ":" + fecha.Minute.ToString("00");
		return sFecha;
	}

	public string ToShortDateString()
	{
		string sFecha = "";
		sFecha = fecha.Day.ToString("00") + "/" + fecha.Month.ToString("00") + "/" + fecha.Year;
		return sFecha;
	}

	public string ToStringDate()
	{
		string sFecha = "";
		sFecha = fecha.Year + "-" + fecha.Month.ToString("00") + "-" + fecha.Day.ToString("00");
		return sFecha;
	}

	public string ToShortTimeString()
	{
		string sFecha = "";
		sFecha = fecha.Hour.ToString("00") + ":" + fecha.Minute.ToString("00");
		return sFecha;
	}

}