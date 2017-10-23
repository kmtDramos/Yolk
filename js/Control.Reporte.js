/**/

$(function () {

	$("#btnActualizarReporte").click(ObtenerReporte);

	//ObtenerReporte();

});

function ObtenerReporte() {
	var fechaInicio = $("#txtFechaInicio").val().split("-");
	var horaInicio = $("#txtHoraInicio").val().split(":");
	var fechaFin = $("#txtFechaFin").val().split("-");
	var horaFin = $("#txtHoraFin").val().split(":");
	var Reporte = new Object();
	Reporte.Inicio = new Date(fechaInicio[0], fechaInicio[1] - 1, fechaInicio[2], horaInicio[0], horaInicio[1]);
	Reporte.Fin = new Date(fechaFin[0], fechaFin[1] - 1, fechaFin[2], horaFin[0], horaFin[1]);

	var Request = JSON.stringify(Reporte);

	WM("_Controls/Medidor.aspx/Reporte", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		if (json.Error == "")
		{
			$("#divReporte").html('');

			var Reporte = json.Datos.Reporte;

			var graficas = $('<table><tr><td><div id="Consumos"></div></td><td><div id="Horas"></div></td><td><div id="Recibo"></div></td><tr></table>');

			$("#divReporte").append(graficas);

			var Consumos = DatosGraficaConsumo(Reporte);

			console.log(Consumos);

			Morris.Bar(Consumos);

			var Horas = DatosGraficaHoras(Reporte);

			Morris.Bar(Horas);

			$("#divReporte").append($("<hr/>"));

			var Table = $("<table class=\"table\"></table>");
			var Thead = $("<thead></thead>");
			var Tbody = $("<tbody></tbody>");
			var TrHead = $("<tr></tr>");

			for (x in Reporte[0])
				$(TrHead).append($("<th>" + x + "</th>"));

			$(Thead).append(TrHead);

			for (x in Reporte) {
				var Registro = Reporte[x];
				var tr = $("<tr></tr>");
				for(y in Registro)
					$(tr).append($("<td>" + Registro[y] + "</td>"));
				$(Tbody).append(tr);
			}

			$(Table)
				.append(Thead)
				.append(Tbody);

			$("#divReporte").append(Table);
		}
		else
		{

		}
	});
}

function DatosGraficaConsumo(Datos) {
	var config = new Object();
	var consumo = [];

	for (x in Datos) {
		var Circuito = Datos[x];
		var obj = new Object();
		obj.Circuito = Circuito["Circuito"];
		obj.Meta = Circuito["Meta KwH"];
		obj.Real = Circuito["Real KwH"];
		consumo.push(obj);
	}

	config.element = "Consumos";
	config.data = consumo;
	config.xkey = "Circuito";
	config.ykeys = ["Meta", "Real"];
	config.labels = ["Meta", "Real"];
	config.hideHover = "auto";
	config.resize = true;

	return config;
}

function DatosGraficaHoras(Datos) {
	var config = new Object();
	var consumo = [];

	for (x in Datos) {
		var Circuito = Datos[x];
		var obj = new Object();
		obj.Circuito = Circuito["Circuito"];
		obj.Meta = Circuito["Meta Horas uso"];
		obj.Real = Circuito["Real Horas uso"];
		consumo.push(obj);
	}

	config.element = "Horas";
	config.data = consumo;
	config.xkey = "Circuito";
	config.ykeys = ["Meta", "Real"];
	config.labels = ["Meta", "Real"];
	config.hideHover = "auto";
	config.resize = true;

	return config;
}
