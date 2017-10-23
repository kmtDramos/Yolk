/**/
$(function () {

      $("#PaginadorEstado #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorEstado #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorEstado #txtPagina").val(Pagina).change();
    });

    $("#PaginadorEstado #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorEstado #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorEstado #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorEstado #txtPagina").val(Pagina).change();
    });

    $("#PaginadorEstado #txtPagina").change(function () {
        ListarEstado();
    }).change();

    $("#btnObtenerFormaAgregarEstado").click(ObtenerFormaAgregarEstado);

});

function ListarEstado() {
    var LSTEstado = new Object();
    LSTEstado.Pagina = parseInt($("#PaginadorEstado #txtPagina").val());
    LSTEstado.Columna = "Estado";
    LSTEstado.Orden = "ASC";
    var Request = JSON.stringify(LSTEstado);
    WM("_Controls/Estado.aspx/ListarEstado", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorEstado #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorEstado #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Estado = json.Datos.Permisos;
            $("tbody", "#tblListaEstado").html('');
            for (x in Estado) {
                var edo = Estado[x];
                var tr = $("<tr data-id=" + edo.IdEstado + "></tr>");
                $(tr)
                    .append($("<td class=\"hidden-xs hidden-sm\">" + edo.Pais + "</td>"))
                    .append($("<td>" + edo.Estado + "</td>"))
                    .append($("<td class='text-center ku-clickable' onclick=\"ObtenerFormaEditarEstado(" + edo.IdEstado + ");\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td  class=\"text-centerku-clickable\" id='desactivarEstado' onclick=\"DesactivarEstado(" + edo.IdEstado + "," + edo.Baja + " );\" estatus=\"" + edo.Baja + "\">" + edo.Estatus + "</td>"));
                $("tbody", "#tblListaEstado").append(tr);
				
				
            }
        }
        else {
            Error("Listar Estado", json.Error);
        }
    });
	
}

function DesactivarEstado(IdEstado, Baja) {

    Ejecutar = true;
    errores = [];

    if (IdEstado === 0) {
        Ejecutar = false;
        errores.push("Estado es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var BJEstado = new Object();
        BJEstado.IdEstado = IdEstado;
        BJEstado.Baja = Baja;

        var Request = JSON.stringify(BJEstado);
        WM("_Controls/Estado.aspx/DesactivarEStado", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarEstado();
            }
            else {
                Error("Desactivar estado", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaAgregarEstado() {
    var AgEstado = new Object();
    var pRequest = JSON.stringify(AgEstado);
    ASPT(pRequest, "_Controls/Estado.aspx/ObtenerFormaAgregarEstado", "_Views/tmplAgregarEstado.html", "#modalAgregarEstadoTmpl", procesarAgregarEstado);
    
}

function ObtenerPaises() {
    var Pais = new Object();
    var Request = JSON.stringify(Pais);
    WM("_Controls/Estado.aspx/ObtenerPaises", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbPais", json.Datos.Paises);
    });
}

function procesarAgregarEstado(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarEstado';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarEstadoTmpl").processTemplate(data);
        $("#" + modalName).modal();
        ObtenerPaises();
        
        $("#btnAgregarEstado").click(function () {
            AgregarEstado();
        });
    }
    else {
        Error("Forma alta estado ", json.Error);
    }
}

function AgregarEstado() {
    IdPais = $("#cmbPais", "#modalAgregarEstado").val();
    Estado = $("#txtEstado", "#modalAgregarEstado").val();

    Ejecutar = true;
    errores = [];

    if (IdPais === 0) {
        Ejecutar = false;
        errores.push("Pais es requerido.");
    }

    if (Estado === "") {
        Ejecutar = false;
        errores.push("El nombre del Estado es requerido.");
    }

    if (Ejecutar) {
        var AgEstado = new Object();
        AgEstado.IdPais = IdPais;
        AgEstado.Estado = Estado;

        var Request = JSON.stringify(AgEstado);
        WM("_Controls/Estado.aspx/AgregarEstado", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarEstado").modal("hide");
                ListarEstado();
            }
            else {
                Error("Agregar estado", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarEstado(IdEstado) {
    var Estado = new Object();
    Estado.IdEstado = IdEstado;
    FORM("formEditarEstado.aspx", Estado, function (Forma) {
        $("body").append(Forma);
        $("#modalEditarEstado").modal();
        $("#modalEditarEstado").on("hidden.bs.modal", function () { $("#modalEditarEstado").remove(); ListarEstado(); });
        $("#btnEditarEstado").click(EditarEstado);
    });
}

function EditarEstado() {
    IdEstado = $("#modalEditarEstado").attr('IdEstado');
    Estado = $("#txtNombreEstado", "#modalEditarEstado").val();
    IdPais = $("#cmbPais", "#modalEditarEstado").val();
    
    Ejecutar = true;
    errores = [];

    if (IdPais === 0) {
        Ejecutar = false;
        errores.push("Pais es requerido.");
    }

    if (Estado === "") {
        Ejecutar = false;
        errores.push("Nombre del Estado es requerido.");
    }

    if (Ejecutar) {
        var EditarEstado = new Object();
        EditarEstado.IdEstado = IdEstado;
        EditarEstado.Estado = Estado;
        EditarEstado.IdPais = IdPais;

        var Request = JSON.stringify(EditarEstado);
        WM("_Controls/Estado.aspx/EditarEstado", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarEstado").modal("hide");
                ListarEstado();
            }
            else {
                Error("Editar estado", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}


