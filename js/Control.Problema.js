/**/
$(function () {

      $("#PaginadorPermisos #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorPermisos #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorPermisos #txtPagina").val(Pagina).change();
    });

    $("#PaginadorPermisos #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorPermisos #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorPermisos #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorPermisos #txtPagina").val(Pagina).change();
    });

    $("#PaginadorPermisos #txtPagina").change(function () {
        ListarProblemas();
    }).change();

    $("#btnObtenerFormaAgregarProblema").click(ObtenerFormaAgregarProblema);

    
});

function ListarProblemas() {
    var Problema = new Object();
    Problema.Pagina = parseInt($("#PaginadorPermisos #txtPagina").val());
    Problema.Columna = "Problema";
    Problema.Orden = "ASC";
    var Request = JSON.stringify(Problema);
    WM("_Controls/Problema.aspx/ListarProblemas", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorPermisos #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorPermisos #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Problema = json.Datos.Permisos;
            $("tbody", "#tblListaProblemas").html('');
            for (x in Problema) {
                var pms = Problema[x];
                var tr = $("<tr data-id=" + pms.IdProblema + "></tr>");
                $(tr)
                    .append($("<td class=\"hidden-xs hidden-sm\">" + pms.TipoProblema + "</td>"))
                    .append($("<td class=\"hidden-xs hidden-sm\">" + pms.Problema + "</td>"))
                    .append($("<td class=\" text-center ku-clickable\" onclick=\"ObtenerFormaEditarProblema(" + pms.IdProblema + ");\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td  class=\"ku-clickable text-center\" id='desactivarProblema' onclick=\"DesactivarProblema(" + pms.IdProblema + "," + pms.Baja + " );\" estatus=\"" + pms.Baja + "\">" + pms.Estatus + "</td>"));
                $("tbody", "#tblListaProblemas").append(tr);
				
				
            }
        }
        else {
            Error("Listar Problemas", json.Error);
        }
    });
	
}

function DesactivarProblema(IdProblema, Baja) {

    Ejecutar = true;
    errores = [];

    if (IdProblema === 0) {
        Ejecutar = false;
        errores.push("Problema es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var DsTProblema = new Object();
        DsTProblema.IdProblema = IdProblema;
        DsTProblema.Baja = Baja;

        var Request = JSON.stringify(DsTProblema);
        WM("_Controls/Problema.aspx/DesactivarProblema", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarProblemas();
            }
            else {
                Error("Desactivar Problema", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaAgregarProblema() {
    var AgPermiso = new Object();
    var pRequest = JSON.stringify(AgPermiso);
    ASPT(pRequest, "_Controls/Problema.aspx/ObtenerFormaAgregarProblema", "_Views/tmplAgregarProblema.html", "#modalAgregarProblemaTmpl", procesarAgregarProblema);
}

function procesarAgregarProblema(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarProblema';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarProblemaTmpl").processTemplate(data);
        $("#" + modalName).modal();

       // ObtenerFiltroTipoProblema();
        $("#btnAgregarProblema").click(function () {
            AgregarProblema();
        });
    }
    else {
        Error("Forma alta problema ", json.Error);
    }
}

function ObtenerFiltroTipoProblema() {
    var TipoProblema = new Object();
    var Request = JSON.stringify(TipoProblema);
    WM("_Controls/Problema.aspx/ObtenerFiltroTipoProblema", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbTipoProblema", json.Datos.TipoProblema);
    });
}

function AgregarProblema() {
    IdTipoProblema = $("#cmbTipoProblema", "#modalAgregarProblema").val();
    Problema = $("#txtNombreProblema", "#modalAgregarProblema").val();

    Ejecutar = true;
    errores = [];

    if (IdTipoProblema === "0") {
        Ejecutar = false;
        errores.push("El tipo problema es requerido.");
    }

    if (Problema === "") {
        Ejecutar = false;
        errores.push("El nombre del problema es requerido.");
    }

    if (Ejecutar) {
        var TipoProblema = new Object();
        TipoProblema.IdTipoProblema = IdTipoProblema;
        TipoProblema.Problema = Problema;

        var Request = JSON.stringify(TipoProblema);
        WM("_Controls/Problema.aspx/AgregarProblema", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarProblema").modal("hide");
                ListarProblemas();
            }
            else {
                Error("Agregar problema", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarProblema(IdProblema) {
    var Problema = new Object();
    Problema.IdProblema = IdProblema;
    var pRequest = JSON.stringify(Problema);
    ASPT(pRequest, "_Controls/Problema.aspx/ObtenerFormaEditarProblema", "_Views/tmplEditarProblema.html", "#modalEditarProblemaTmpl", procesarEditarProblema);
}

function procesarEditarProblema(data) {
    if (data.Error == "") {
        modalName = 'modalEditarProblema';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarProblemaTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#btnEditarProblema").click(EditarProblema);
    }
}

function EditarProblema() {
    IdProblema = $("#modalEditarProblema .modal-dialog").attr('idProblema');
    IdTipoProblema = $("#cmbTipoProblema", "#modalEditarProblema").val();
    DescripcionProblema = $("#txtNombreProblema", "#modalEditarProblema").val();

    Ejecutar = true;
    errores = [];

    if (IdTipoProblema === 0) {
        Ejecutar = false;
        errores.push("Tipo problema es requerido.");
    }

    if (DescripcionProblema === "") {
        Ejecutar = false;
        errores.push("Nombre del problema es requerido.");
    }

    if (Ejecutar) {
        var Problema = new Object();
        Problema.IdProblema = IdProblema;
        Problema.IdTipoProblema = IdTipoProblema;
        Problema.Problema = DescripcionProblema;

        var Request = JSON.stringify(Problema);
        WM("_Controls/Problema.aspx/EditarProblema", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarProblema").modal("hide");
				ListarProblemas()
            }
            else {
                Error("Editar problema", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}