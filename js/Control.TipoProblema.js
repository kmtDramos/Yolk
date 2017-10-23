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
        ListarTipoProblemas();
    }).change();

    $("#btnObtenerFormaAgregarTipoProblema").click(ObtenerFormaAgregarTipoProblema);

    
});

function ListarTipoProblemas() {
    var TpProblema = new Object();
    TpProblema.Pagina = parseInt($("#PaginadorPermisos #txtPagina").val());
    TpProblema.Columna = "TipoProblema";
    TpProblema.Orden = "ASC";
    var Request = JSON.stringify(TpProblema);
    WM("_Controls/TipoProblema.aspx/ListarTipoProblemas", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorPermisos #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorPermisos #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var TpProblema = json.Datos.Permisos;
            $("tbody", "#tblListaTipoProblemas").html('');
            for (x in TpProblema) {
                var pms = TpProblema[x];
                var tr = $("<tr data-id=" + pms.IdProblema + "></tr>");
                $(tr)
                    .append($("<td class=\"hidden-xs hidden-sm\">" + pms.TipoProblema + "</td>"))
                    .append($("<td class=\"text-center ku-clickable\" onclick=\"ObtenerFormaEditarTipoProblema(" + pms.IdTipoProblema + ");\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td  class=\"ku-clickable text-center\" id='desactivarTipoProblema' onclick=\"DesactivarTipoProblema(" + pms.IdTipoProblema + "," + pms.Baja + " );\" estatus=\"" + pms.Baja + "\">" + pms.Estatus + "</td>"));
                $("tbody", "#tblListaTipoProblemas").append(tr);
				
				
            }
        }
        else {
            Error("Listar TipoProblemas", json.Error);
        }
    });
	
}

function DesactivarTipoProblema(IdTipoProblema, Baja) {

    Ejecutar = true;
    errores = [];

    if (IdTipoProblema === 0) {
        Ejecutar = false;
        errores.push("El tipo de Problema es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var DsTProblema = new Object();
        DsTProblema.IdTipoProblema = IdTipoProblema;
        DsTProblema.Baja = Baja;

        var Request = JSON.stringify(DsTProblema);
        WM("_Controls/TipoProblema.aspx/DesactivarTipoProblema", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarTipoProblemas();
            }
            else {
                Error("Desactivar Tipo de Problema", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaAgregarTipoProblema() {
    var AgPermiso = new Object();
    var pRequest = JSON.stringify(AgPermiso);
    ASPT(pRequest, "_Controls/TipoProblema.aspx/ObtenerFormaAgregarTipoProblema", "_Views/tmplAgregarTipoProblema.html", "#modalAgregarTipoProblemaTmpl", procesarAgregarTipoProblema);
}

function procesarAgregarTipoProblema(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarTipoProblema';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarTipoProblemaTmpl").processTemplate(data);
        $("#" + modalName).modal();


        $("#btnAgregarTipoProblema").click(function () {
            AgregarTipoProblema();
        });
    }
    else {
        Error("Forma alta tipo de problema ", json.Error);
    }
}

function AgregarTipoProblema() {
    NombreTipoProblema = $("#txtNombreTipoProblema", "#modalAgregarTipoProblema").val();

    Ejecutar = true;
    errores = [];

    if (NombreTipoProblema === "") {
        Ejecutar = false;
        errores.push("El nombre del tipo de problema es requerido.");
    }

    if (Ejecutar) {
        var AgProblema = new Object();
        AgProblema.NombreTipoProblema = NombreTipoProblema;

        var Request = JSON.stringify(AgProblema);
        WM("_Controls/TipoProblema.aspx/AgregarTipoProblema", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarTipoProblema").modal("hide");
                ListarTipoProblemas();
            }
            else {
                Error("Agregar tipo de problema", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarTipoProblema(IdTipoProblema) {
    var EdtProblema = new Object();
    EdtProblema.IdTipoProblema = IdTipoProblema;
    var pRequest = JSON.stringify(EdtProblema);
    ASPT(pRequest, "_Controls/TipoProblema.aspx/ObtenerFormaEditarTipoProblema", "_Views/tmplEditarTipoProblema.html", "#modalEditarTipoProblemaTmpl", procesarEditarTipoProblema);
}

function procesarEditarTipoProblema(data) {
    if (data.Error == "") {
        modalName = 'modalEditarTipoProblema';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarTipoProblemaTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#btnEditarTipoProblema").click(EditarTipoProblema);
    }
}

function EditarTipoProblema() {
    IdTipoProblema = $("#modalEditarTipoProblema .modal-dialog").attr('idTipoProblema');
    NombreTipoProblema = $("#txtNombreTipoProblema", "#modalEditarTipoProblema").val();

    Ejecutar = true;
    errores = [];

    if (NombreTipoProblema === "") {
        Ejecutar = false;
        errores.push("Nombre del tipo de problema es requerido.");
    }


    if (Ejecutar) {
        var EdProblema = new Object();
        EdProblema.IdTipoProblema = IdTipoProblema;
        EdProblema.NombreTipoProblema = NombreTipoProblema;

        var Request = JSON.stringify(EdProblema);
        WM("_Controls/TipoProblema.aspx/EditarTipoProblema", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarTipoProblema").modal("hide");
				ListarTipoProblemas()
            }
            else {
                Error("Editar tipo de problema", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}


