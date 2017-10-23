/* JS de Catalogo Pais */
$(function () {

	$("#PaginadorPaises #btnAnteriorPagina").click(function () {
		var Pagina = parseInt($("#PaginadorPaises #txtPagina").val()) - 1;
		Pagina = (Pagina < 1) ? 1 : Pagina;
		$("#PaginadorPaises #txtPagina").val(Pagina).change();
	});

	$("#PaginadorPaises #btnSiguientePagina").click(function () {
		var Paginas = parseInt($("#PaginadorPaises #lblPaginas").text());
		var Pagina = parseInt($("#PaginadorPaises #txtPagina").val()) + 1;
		Pagina = (Pagina > Paginas) ? Paginas : Pagina;
		$("#PaginadorPaises #txtPagina").val(Pagina).change();
	});

	$("#PaginadorPaises #txtPagina").change(function () {
		ListarPaises();
	}).change();

	$("#btnObtenerFormaAgregarPais").click(ObtenerFormaAgregarPais);


	$('body').on('click', "#btnConsultarPais", function (e) {
	    e.preventDefault();
	    var row = $(this).parents('tr');
	    var id = row.data('id');
	    ObtenerFormaEditarPais(id);
	});

	$('body').on('click', "#btnDesactivarPais", function (e) {
	    e.preventDefault();
	    var row = $(this).parents('tr');
	    var id = row.data('id');
	    var estatus = $(this).attr('estatus');
	    DesactivarPais(id, estatus);
	});
});

/*PAISES*/

function ListarPaises() {
	var Paises = new Object();
	Paises.Pagina = parseInt($("#PaginadorPaises #txtPagina").val());
	Paises.Columna = "Pais";
	Paises.Orden = "ASC";
	var Request = JSON.stringify(Paises);
	WM("_Controls/Catalogo.Pais.aspx/ListarPais", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		if (json.Error == "") {

			$("#PaginadorPaises #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
			$("#PaginadorPaises #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

			var Paises = json.Datos.Paises;
			$("tbody", "#tblListaPaises").html('');
			for (x in Paises) {
				var pais = Paises[x];
				var tr = $("<tr data-id=" + pais.IdPais + "></tr>");
				$(tr)
					.append($("<td>" + pais.Pais + "</td>"))
                    .append($("<td id=\"btnConsultarPais\" class=\"text-center ku-clickable\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td id=\"btnDesactivarPais\" estatus=\"" + pais.Baja + "\" class=\"text-center ku-clickable hidden-xs hidden-sm\">" + pais.Estatus + "</td>"));
				$("tbody", "#tblListaPaises").append(tr);
			}
		}
		else {
			Error("Listar Paises", json.Error);
		}
	});
}

function DesactivarPais(IdPais, Baja) {

	Ejecutar = true;
	errores = [];

	if (IdPais === 0) {
		Ejecutar = false;
		errores.push("País es requerido.");
	}

	if (Baja === "") {
		Ejecutar = false;
		errores.push("Estatus es requerido.");
	}

	if (Ejecutar) {
		var Pais = new Object();
		Pais.IdPais = IdPais;
		Pais.Baja = Baja;

		var Request = JSON.stringify(Pais);
		WM("_Controls/Catalogo.Pais.aspx/DesactivarPais", Request, function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "") {
				ListarPaises();
			}
			else {
				Error("Desactivar país", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
			}
		});
	}
	else {
		ManejarArregloDeErrores(errores);
	}
}

function ObtenerFormaAgregarPais() {
	var pais = new Object();
	var pRequest = JSON.stringify(pais);
	ASPT(pRequest, "_Controls/Catalogo.Pais.aspx/ObtenerFormaAgregarPais", "_Views/tmplAgregarPais.html", "#modalAgregarPaisTmpl", procesarAgregarPais);
}

function procesarAgregarPais(data) {

	if (data.Error == "") {
		modalName = 'modalAgregarPais';
		CREARMODAL(modalName);
		$("#" + modalName).setTemplateElement("modalAgregarPaisTmpl").processTemplate(data);
		$("#" + modalName).modal();

		$("#btnAgregarPais").click(function () {
			AgregarPais();
		});
	}
	else {
		Error("Formulario Meta ", json.Error);
	}
}

function AgregarPais() {
	Pais = $("#txtPais", "#modalAgregarPais").val();
	Ejecutar = true;
	errores = [];

	if (Pais === "") {
		Ejecutar = false;
		errores.push("País es requerido.");
	}

	if (Ejecutar) {
		var pais = new Object();
		pais.Pais = Pais;

		var request = JSON.stringify(pais);
		WM("_Controls/Catalogo.Pais.aspx/AgregarPais", request, function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "") {
				$("#modalAgregarPais").modal("hide");
				ListarPaises();
			}
			else {
				Error("Agregar país", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
			}
		});
	}
	else {
		ManejarArregloDeErrores(errores);
	}
	
}

function ObtenerFormaEditarPais(IdPais) {
	var Pais = new Object();
	Pais.IdPais = IdPais;
	var pRequest = JSON.stringify(Pais);
	ASPT(pRequest, "_Controls/Catalogo.Pais.aspx/ObtenerFormaEditarPais", "_Views/tmplEditarPais.html", "#modalEditarPaisTmpl", procesarEditarPais);
}

function procesarEditarPais(data) {
	if (data.Error == "") {
		modalName = 'modalEditarPais';
		CREARMODAL(modalName);
		$("#" + modalName).setTemplateElement("modalEditarPaisTmpl").processTemplate(data);
		$("#" + modalName).modal();

		//Al terminar de cargar el modal
		$("#" + modalName).on('shown.bs.modal', function () {

			$("#btnEditarPais").click(EditarPais);
		});
	}
}

function EditarPais() {
	IdPais = $("#modalEditarPais .modal-dialog").attr('idpais');
	//IdPais = $("#modalEditarPaisTmpl").attr('IdPais');
	Pais = $("#txtPais", "#modalEditarPais").val();
	
	Ejecutar = true;
	errores = [];

	if (IdPais === 0) {
		Ejecutar = false;
		errores.push("Nombre del pais es requerido.");
	}

	if (Ejecutar) {
		var EdPais = new Object();
		EdPais.Pais = Pais;
		EdPais.IdPais = IdPais;


		var Request = JSON.stringify(EdPais);
		WM("_Controls/Catalogo.Pais.aspx/EditarPais", Request, function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "") {
				$("#modalEditarPais").modal("hide");
				ListarPaises()
			}
			else {
				Error("Editar pais", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
			}
		});
	}
	else {
		ManejarArregloDeErrores(errores);
	}
}

