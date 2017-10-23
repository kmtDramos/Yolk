/**/

$(function () {
	ActualizarMedidor();
});

function ActualizarMedidor() {
	WM("_Controls/Medidor.aspx/Inicio", "", function (Respuesta) {
		var d = Respuesta.d;
		MostrarRegistros(JSON.parse(d));
		setTimeout(ActualizarMedidor, 1000 * 10);
	});
}

function MostrarRegistros(Registro) {
	if (Registro.Error == "")
	{
		$("#divMedidor").html('');
		var Medidor = Registro.Datos.Medidor;
		var Tabla = $("<table class='table table-striped table-bordered'></table>");
		var Tbody = $("<tbody></tbody>");
		for (x in Medidor) {
			$(Tbody).append($("<tr><td>"+ x +"</td><td>"+ Medidor[x] +"</td></tr>"));
		}
		$(Tabla).append(Tbody);
		$("#divMedidor").append(Tabla);
	}
	else
	{
		Error("Inicio", Registro.Error);
	}
}