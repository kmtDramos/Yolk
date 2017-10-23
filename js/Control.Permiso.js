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
        ListarPermisos();
    }).change();

    $("#btnObtenerFormaAgregarPermiso").click(ObtenerFormaAgregarPermiso);

    
});

function ListarPermisos() {
    var Permisos = new Object();
    Permisos.Pagina = parseInt($("#PaginadorPermisos #txtPagina").val());
    Permisos.Columna = "Permiso";
    Permisos.Orden = "ASC";
    var Request = JSON.stringify(Permisos);
    WM("_Controls/Permiso.aspx/ListarPermisos", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorPermisos #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorPermisos #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Permisos = json.Datos.Permisos;
            $("tbody", "#tblListaPermisos").html('');
            for (x in Permisos) {
                var pms = Permisos[x];
                var tr = $("<tr data-id=" + pms.IdPermiso + "></tr>");
                $(tr)
                    .append($("<td class=\"hidden-xs hidden-sm\">" + pms.Permiso + "</td>"))
                    .append($("<td>" + pms.Comando + "</td>"))
                    .append($("<td class=\"hidden-xs hidden-sm\">" + pms.Pantalla + "</td>"))
                    .append($("<td class=\"text-center ku-clickable\" onclick=\"ObtenerFormaEditarPermiso(" + pms.IdPermiso + ");\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td  class=\"ku-clickable text-center\" id='desactivarPermiso' onclick=\"DesactivarPermiso(" + pms.IdPermiso + "," + pms.Baja + " );\" estatus=\"" + pms.Baja + "\">" + pms.Estatus + "</td>"));
                $("tbody", "#tblListaPermisos").append(tr);
				
				
            }
        }
        else {
            Error("Listar Permisos", json.Error);
        }
    });
	
}

function DesactivarPermiso(IdPermiso, Baja) {

    Ejecutar = true;
    errores = [];

    if (IdPermiso === 0) {
        Ejecutar = false;
        errores.push("Permiso es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Permiso = new Object();
        Permiso.IdPermiso = IdPermiso;
        Permiso.Baja = Baja;

        var Request = JSON.stringify(Permiso);
        WM("_Controls/Permiso.aspx/DesactivarPermiso", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarPermisos();
            }
            else {
                Error("Desactivar permiso", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaAgregarPermiso() {
    var Permiso = new Object();
    var pRequest = JSON.stringify(Permiso);
    ASPT(pRequest, "_Controls/Permiso.aspx/ObtenerFormaAgregarPermiso", "_Views/tmplAgregarPermiso.html", "#modalAgregarPermisoTmpl", procesarAgregarPermiso);
}

function procesarAgregarPermiso(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarPermiso';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarPermisoTmpl").processTemplate(data);
        $("#" + modalName).modal();

        
        $("#btnAgregarPermiso").click(function () {
            AgregarPermiso();
        });
    }
    else {
        Error("Forma alta permiso ", json.Error);
    }
}


function AgregarPermiso() {
    NombrePermiso = $("#txtNombrePermiso", "#modalAgregarPermiso").val();
    Comando = $("#txtComando", "#modalAgregarPermiso").val();
    Pantalla = $("#txtPantalla", "#modalAgregarPermiso").val();

    Ejecutar = true;
    errores = [];

    if (NombrePermiso === "") {
        Ejecutar = false;
        errores.push("El nombre del permiso es requerido.");
    }

    if (Comando === "") {
        Ejecutar = false;
        errores.push("El nombre del comando es requerido.");
    }

    if (Pantalla === "") {
        Ejecutar = false;
        errores.push("El nombre de la pantalla.");
    }

    if (Ejecutar) {
        var Permiso = new Object();
        Permiso.NombrePermiso = NombrePermiso;
        Permiso.Comando = Comando;
        Permiso.Pantalla = Pantalla;

        var Request = JSON.stringify(Permiso);
        WM("_Controls/Permiso.aspx/AgregarPermiso", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarPermiso").modal("hide");
                ListarPermisos();
            }
            else {
                Error("Agregar permiso", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarPermiso(IdPermiso) {
    var Permiso = new Object();
    Permiso.IdPermiso= IdPermiso;
    var pRequest = JSON.stringify(Permiso);
    ASPT(pRequest, "_Controls/Permiso.aspx/ObtenerFormaEditarPermiso", "_Views/tmplEditarPermiso.html", "#modalEditarPermisoTmpl", procesarEditarPermiso);
}

function procesarEditarPermiso(data) {
    if (data.Error == "") {
        modalName = 'modalEditarPermiso';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarPermisoTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#btnEditarPermiso").click(EditarPermiso);
    }
}

function EditarPermiso() {
    IdPermiso = $("#modalEditarPermiso .modal-dialog").attr('IdPermiso');
    NombrePermiso = $("#txtNombrePermiso", "#modalEditarPermiso").val();
    Comando = $("#txtComando", "#modalEditarPermiso").val();
    Pantalla = $("#txtPantalla", "#modalEditarPermiso").val();

    Ejecutar = true;
    errores = [];

    if (NombrePermiso === "") {
        Ejecutar = false;
        errores.push("Nombre del permiso es requerido.");
    }

    if (Comando === "") {
        Ejecutar = false;
        errores.push("Nombre del comando es requerido.");
    }

    if (Pantalla === "") {
        Ejecutar = false;
        errores.push("Nombre de la pantalla es requerida.");
    }

    if (Ejecutar) {
        var Permiso = new Object();
        Permiso.IdPermiso = IdPermiso;
        Permiso.NombrePermiso = NombrePermiso;
        Permiso.Comando = Comando;
        Permiso.Pantalla = Pantalla;


        var Request = JSON.stringify(Permiso);
        WM("_Controls/Permiso.aspx/EditarPermiso", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarPermiso").modal("hide");
				ListarPermisos()
            }
            else {
                Error("Editar permiso", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}


