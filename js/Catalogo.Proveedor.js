/* JS de Catalogo Proveedor */
$(function () {

	$("#PaginadorProveedor #btnAnteriorPagina").click(function () {
		var Pagina = parseInt($("#PaginadorProveedor #txtPagina").val()) - 1;
		Pagina = (Pagina < 1) ? 1 : Pagina;
		$("#PaginadorProveedor #txtPagina").val(Pagina).change();
	});

	$("#PaginadorProveedor #btnSiguientePagina").click(function () {
		var Paginas = parseInt($("#PaginadorProveedor #lblPaginas").text());
		var Pagina = parseInt($("#PaginadorProveedor #txtPagina").val()) + 1;
		Pagina = (Pagina > Paginas) ? Paginas : Pagina;
		$("#PaginadorProveedor #txtPagina").val(Pagina).change();
	});

	$("#PaginadorProveedor #txtPagina").change(function () {
		ListarProveedor();
	}).change();

	$("#btnObtenerFormaAgregarProveedor").click(ObtenerFormaAgregarProveedor);


	$('body').on('click', "#btnConsultarProveedor", function (e) {
	    e.preventDefault();
	    var row = $(this).parents('tr');
	    var id = row.data('id');
	    ObtenerFormaEditarProveedor(id);
	});

	$('body').on('click', "#btnDesactivarProveedor", function (e) {
	    e.preventDefault();
	    var row = $(this).parents('tr');
	    var id = row.data('id');
	    var estatus = $(this).attr('estatus');
	    DesactivarProveedor(id, estatus);
	});
});

/*Proveedor*/

function ListarProveedor() {
	var Proveedor = new Object();
	Proveedor.Pagina = parseInt($("#PaginadorProveedor #txtPagina").val());
	Proveedor.Columna = "Proveedor";
	Proveedor.Orden = "ASC";
	var Request = JSON.stringify(Proveedor);
	WM("_Controls/Catalogo.Proveedor.aspx/ListarProveedor", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		if (json.Error == "") {

			$("#PaginadorProveedor #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
			$("#PaginadorProveedor #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

			var Proveedores = json.Datos.Proveedores;
			$("tbody", "#tblListaProveedor").html('');
			for (x in Proveedores) {
				var prov = Proveedores[x];
				var tr = $("<tr data-id=" + prov.IdProveedor + "></tr>");
				$(tr)
					.append($("<td>" + prov.Proveedor + "</td>"))
					.append($("<td id=\"btnConsultarProveedor\" class=\"text-center ku-clickable\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td id=\"btnDesactivarProveedor\" estatus=\"" + prov.Baja + "\" class=\"text-center ku-clickable hidden-xs hidden-sm\">" + prov.Estatus + "</td>"));
				$("tbody", "#tblListaProveedor").append(tr);
			}
		}
		else {
			Error("Listar Proveedores", json.Error);
		}
	});
}

function ObtenerFormaAgregarProveedor() {
	var prov = new Object();
	var pRequest = JSON.stringify(prov);
	ASPT(pRequest, "_Controls/Catalogo.Proveedor.aspx/ObtenerFormaAgregarProveedor", "_Views/tmplAgregarProveedor.html", "#modalAgregarProveedorTmpl", procesarAgregarProveedor);
}

function procesarAgregarProveedor(data) {

	if (data.Error == "") {
		modalName = 'modalAgregarProveedor';
		CREARMODAL(modalName);
		$("#" + modalName).setTemplateElement("modalAgregarProveedorTmpl").processTemplate(data);
		$("#" + modalName).modal();

		$("#btnAgregarProveedor").click(function () {
			AgregarProveedor();
		});
	}
	else {
		Error("Formulario Proveedor ", json.Error);
	}
}

function AgregarProveedor() {
	Proveedor = $("#txtProveedor", "#modalAgregarProveedor").val();
	Ejecutar = true;
	errores = [];

	if (Proveedor === "") {
		Ejecutar = false;
		errores.push("Proveedor es requerido.");
	}

	if (Ejecutar) {
		var proveedor = new Object();
		proveedor.Proveedor = Proveedor;

		var request = JSON.stringify(proveedor);
		WM("_Controls/Catalogo.Proveedor.aspx/AgregarProveedor", request, function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "") {
				$("#modalAgregarProveedor").modal("hide");
				ListarProveedor();
			}
			else {
				Error("Agregar proveedor", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
			}
		});
	}
	else {
		ManejarArregloDeErrores(errores);
	}
	
}

function ObtenerFormaEditarProveedor(IdProveedor) {
	var Proveedor = new Object();
	Proveedor.IdProveedor = IdProveedor;
	var pRequest = JSON.stringify(Proveedor);
	ASPT(pRequest, "_Controls/Catalogo.Proveedor.aspx/ObtenerFormaEditarProveedor", "_Views/tmplEditarProveedor.html", "#modalEditarProveedorTmpl", procesarEditarProveedor);
}

function procesarEditarProveedor(data) {
	if (data.Error == "") {
	    modalName = 'modalEditarProveedor';
		CREARMODAL(modalName);
		$("#" + modalName).setTemplateElement("modalEditarProveedorTmpl").processTemplate(data);
		$("#" + modalName).modal();

		//Al terminar de cargar el modal
		$("#" + modalName).on('shown.bs.modal', function () {

			$("#btnEditarProveedor").click(EditarProveedor);
		});
	}
}

function EditarProveedor() {
    
	IdProveedor = $("#modalEditarProveedor .modal-dialog").attr('idproveedor');
	Proveedor = $("#txtProveedor", "#modalEditarProveedor").val();
	
	Ejecutar = true;
	errores = [];

	if (IdProveedor === 0) {
		Ejecutar = false;
		errores.push("Nombre del proveedor es requerido.");
	}

	if (Ejecutar) {
		var EdProveedor = new Object();
		EdProveedor.Proveedor = Proveedor;
		EdProveedor.IdProveedor = IdProveedor;


		var Request = JSON.stringify(EdProveedor);
		WM("_Controls/Catalogo.Proveedor.aspx/EditarProveedor", Request, function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "") {
				$("#modalEditarProveedor").modal("hide");
				ListarProveedor()
			}
			else {
				Error("Editar proveedor", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
			}
		});
	}
	else {
		ManejarArregloDeErrores(errores);
	}
}

function DesactivarProveedor(IdProveedor, Baja) {

	Ejecutar = true;
	errores = [];

	if (IdProveedor === 0) {
		Ejecutar = false;
		errores.push("Proveedor es requerido.");
	}

	if (Baja === "") {
		Ejecutar = false;
		errores.push("Estatus es requerido.");
	}

	if (Ejecutar) {
		var Proveedor = new Object();
		Proveedor.IdProveedor = IdProveedor;
		Proveedor.Baja = Baja;

		var Request = JSON.stringify(Proveedor);
		WM("_Controls/Catalogo.Proveedor.aspx/DesactivarProveedor", Request, function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "") {
				ListarProveedor();
			}
			else {
				Error("Desactivar proveedor", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
			}
		});
	}
	else {
		ManejarArregloDeErrores(errores);
	}
}