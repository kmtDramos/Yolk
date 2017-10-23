
/**/
$(function () {

	$("#PaginadorMunicipio #btnAnteriorPagina").click(function () {
		var Pagina = parseInt($("#PaginadorMunicipio #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorMunicipio #txtPagina").val(Pagina).change();
    });

	$("#PaginadorMunicipio #btnSiguientePagina").click(function () {
		var Paginas = parseInt($("#PaginadorMunicipio #lblPaginas").text());
		var Pagina = parseInt($("#PaginadorMunicipio #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorMunicipio #txtPagina").val(Pagina).change();
    });

	$("#PaginadorMunicipio #txtPagina").change(function () {
       ListarMunicipio();
    }).change();

    $("#btnObtenerFormaAgregarMunicipio").click(ObtenerFormaAgregarMunicipio);

   
});

function ListarMunicipio() {

	var ListMpio = new Object();
	ListMpio.Pagina = parseInt($("#PaginadorMunicipio #txtPagina").val());
	ListMpio.Columna = "Municipio";
	ListMpio.Orden = "ASC";
	var Request = JSON.stringify(ListMpio);
	WM("_Controls/Municipio.aspx/ListarMunicipio", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		if (json.Error == "") {

			$("#PaginadorMunicipio #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
			$("#PaginadorMunicipio #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

			var Municipio = json.Datos.Permisos;
			$("tbody", "#tblListaMunicipio").html('');
			for (x in Municipio) {
				var mpio = Municipio[x];
				var tr = $("<tr data-id=" + mpio.IdMunicipio + "></tr>");
				$(tr)
                    .append($("<td class=\"hidden-xs hidden-sm\">" + mpio.Pais + "</td>"))
                    .append($("<td class=\"hidden-xs hidden-sm\">" + mpio.Estado + "</td>"))
                    .append($("<td>" + mpio.Municipio + "</td>"))
                    .append($("<td class=\"text-center ku-clickable\" onclick=\"ObtenerFormaEditarMunicipio(" + mpio.IdMunicipio + ");\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td  class=\"ku-clickable text-center\" id='desactivarMunicipio' onclick=\"DesactivarMunicipio(" + mpio.IdMunicipio + "," + mpio.Baja + " );\" estatus=\"" + mpio.Baja + "\">" + mpio.Estatus + "</td>"));
				$("tbody", "#tblListaMunicipio").append(tr);
			}
		}
		else {
			Error("Listar Municipio", json.Error);
		}
	});
}

function DesactivarMunicipio(IdMunicipio, Baja) {

	Ejecutar = true;
	errores = [];

	if (IdMunicipio === 0) {
		Ejecutar = false;
		errores.push("Municipio es requerido.");
	}

	if (Baja === "") {
		Ejecutar = false;
		errores.push("Estatus es requerido.");
	}

	if (Ejecutar) {
		var Municipio = new Object();
		Municipio.IdMunicipio = IdMunicipio;
		Municipio.Baja = Baja;

		var Request = JSON.stringify(Municipio);
		WM("_Controls/Municipio.aspx/DesactivarMunicipio", Request, function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "") {
				ListarMunicipio();
			}
			else {
				Error("Desactivar municipio", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
			}
		});
	}
	else {
		ManejarArregloDeErrores(errores);
	}
}

function ObtenerFormaAgregarMunicipio() {
	var Municipio = new Object();
	var pRequest = JSON.stringify(Municipio);
    ASPT(pRequest, "_Controls/Municipio.aspx/ObtenerFormaAgregarMunicipio", "_Views/tmplAgregarMunicipio.html", "#modalAgregarMunicipioTmpl", procesarAgregarMunicipio);
}

function ObtenerPaises() {
	var Pais = new Object();
	var Request = JSON.stringify(Pais);
	WM("_Controls/Municipio.aspx/ObtenerPaises", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		COMBO("#cmbPais", json.Datos.Paises);
	});

	$("#cmbPais").change(function () {
		ObtenerEstados(parseInt($(this).val()));
	});


}

function ObtenerEstados(IdPais) {
	var Estado = new Object();
	Estado.IdPais = IdPais;
	var Request = JSON.stringify(Estado);
	WM("_Controls/Municipio.aspx/ObtenerEstados", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		COMBO("#cmbEstado", json.Datos.Estados);
	});
}

function procesarAgregarMunicipio(data) {
	//Aqui los paises que hiciste en el webmetod regresan en data y se planchan en el template, es jquery template
	if (data.Error == "") {
		modalName = 'modalAgregarMunicipio';
		CREARMODAL(modalName);
		$("#" + modalName).setTemplateElement("modalAgregarMunicipioTmpl").processTemplate(data);
		$("#" + modalName).modal();


		$("#cmbPais").change(function () {
			ObtenerEstados(parseInt($(this).val()));
		});

		$("#btnAgregarMunicipio").click(function () {
			AgregarMunicipio();
		});
	}
	else {
		Error("Forma alta municipio ", json.Error);
	}
}

function AgregarMunicipio() {
	IdPais = $("#cmbPais", "#modalAgregarMunicipio").val();
	IdEstado = $("#cmbEstado", "#modalAgregarMunicipio").val();
	DescripcionMunicipio = $("#txtMunicipio", "#modalAgregarMunicipio").val();

	Ejecutar = true;
	errores = [];

	if (IdPais === 0) {
		Ejecutar = false;
		errores.push("Pais es requerido.");
	}

	if (IdEstado === 0) {
		Ejecutar = false;
		errores.push("Estado es requerido.");
	}

	if (DescripcionMunicipio === "") {
		Ejecutar = false;
		errores.push("El nombre del Municipio es requerido.");
	}
	//no se puede llamar igual dice q algo de circular
	if (Ejecutar) {
		var Municipio = new Object();
		Municipio.IdPais = IdPais;
		Municipio.IdEstado = IdEstado;
		Municipio.Municipio = DescripcionMunicipio;

		var Request = JSON.stringify(Municipio);
		WM("_Controls/Municipio.aspx/AgregarMunicipio", Request, function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "") {
				$("#modalAgregarMunicipio").modal("hide");
				ListarMunicipio();
			}
			else {
				Error("Agregar Municipio", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
			}
		});
	}
	else {
		ManejarArregloDeErrores(errores);
	}
	
}

function ObtenerFormaEditarMunicipio(IdMunicipio) {
	var Municipio = new Object();
	Municipio.IdMunicipio = IdMunicipio;
	FORM("formEditarMunicipio.aspx", Municipio, function (Forma) {
		$("body").append(Forma);
		$("#modalEditarMunicipio").modal();
		$("#modalEditarMunicipio").on("hidden.bs.modal", function () { $("#modalEditarMunicipio").remove(); ListarMunicipio(); });
		$("#btnEditarMunicipio").click(EditarMunicipio);
	});
}

function EditarMunicipio() {
	IdMunicipio = $("#modalEditarMunicipio").attr('IdMunicipio');
	Municipio = $("#txtNombreMunicipio", "#modalEditarMunicipio").val();
	IdEstado = $("#cmbEstado", "#modalEditarMunicipio").val();
	IdPais = $("#cmbPais", "#modalEditarMunicipio").val();
	

	Ejecutar = true;
	errores = [];

	if (IdPais === 0) {
		Ejecutar = false;
		errores.push("Nombre del pais es requerido.");
	}

	if (IdEstado === 0) {
		Ejecutar = false;
		errores.push("Nombre del estado es requerido.");
	}

	if (Municipio === "") {
		Ejecutar = false;
		errores.push("Nombre del municipio es requerido.");
	}

	if (Ejecutar) {
		var EditarMpio = new Object();
		EditarMpio.IdMunicipio = IdMunicipio;
		EditarMpio.Municipio = Municipio;
		EditarMpio.IdEstado = IdEstado;
		EditarMpio.IdPais = IdPais;


		var Request = JSON.stringify(EditarMpio);
		WM("_Controls/Municipio.aspx/EditarMunicipio", Request, function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "") {
				$("#modalEditarMunicipio").modal("hide");
				ListarMunicipio()
			}
			else {
				Error("Editar Municipio", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
			}
		});
	}
	else {
		ManejarArregloDeErrores(errores);
	}
}


