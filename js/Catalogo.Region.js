/**/

$(function () {

    $("#PaginadorRegiones #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorRegiones #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorRegiones #txtPagina").val(Pagina).change();
    });

    $("#PaginadorRegiones #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorRegiones #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorRegiones #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorRegiones #txtPagina").val(Pagina).change();
    });

    $("#PaginadorRegiones #txtPagina").change(function () {
        ListarRegiones();
    }).change();


    $("#btnObtenerFormaAgregarRegion").click(ObtenerFormaAgregarRegion);
});

function ListarRegiones() {
    var Regiones = new Object();
    Regiones.Pagina = parseInt($("#PaginadorRegiones #txtPagina").val());
    Regiones.Columna = "Region";
    Regiones.Orden = "ASC";
    var Request = JSON.stringify(Regiones);
    WM("_Controls/Region.aspx/ListarRegiones", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorRegiones #lblPaginas").text(json.Datos.Paginas);
            $("#PaginadorRegiones #lblRegistros").text(json.Datos.Registros);

            var Regiones = json.Datos.Regiones;
            $("tbody", "#tblListaRegiones").html('');
            for (x in Regiones) {
                var usr = Regiones[x];
                var tr = $("<tr class=\"ku-clickable\"></tr>");
                $(tr)
					.append($("<td onclick=\"ObtenerFormaEditarRegion(" + usr.IdRegion + ");\">" + usr.Region + "</td>"))                 
					.append($("<td id='desactivarRegion' onclick=\"DesactivarRegion(" + usr.IdRegion + "," + usr.Baja + " );\" estatus=\"" + usr.Baja + "\" class=\"hidden-xs hidden-sm\">" + usr.Estatus + "</td>"));
                $("tbody", "#tblListaRegiones").append(tr);
            }
        }
        else {
            Error("Listar Regiones", json.Error);
        }
    });
}

function ObtenerFormaAgregarRegion() {
    TMPL("tmplAgregarRegion.html", function (Template) {
        $("body").append(Template);
        $("#modalAgregarRegion").modal();
        $("#modalAgregarRegion").on("hidden.bs.modal", function () { $("#modalAgregarRegion").remove(); ListarRegiones(); });
        $("#btnAgregarRegion").click(AgregarRegion);
    });
}

function AgregarRegion() {

    Nombre = $("#txtRegion", "#modalAgregarRegion").val();

    Ejecutar = true;
    errores = [];

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Region es requerido.");
    }

    if (Ejecutar) {
        var Region = new Object();
        Region.Region = Nombre;

        var Request = JSON.stringify(Region);
        WM("_Controls/Region.aspx/AgregarRegion", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarRegion").modal("hide");
            }
            else {
                Error("Agregar Region", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarRegion(IdRegion) {
    var Region = new Object();
    Region.IdRegion = IdRegion;
    FORM("formEditarRegion.aspx", Region, function (Forma) {
        $("body").append(Forma);
        $("#modalEditarRegion").modal();
        $("#modalEditarRegion").on("hidden.bs.modal", function () { $("#modalEditarRegion").remove(); ListarRegiones(); });
        $("#btnEditarRegion").click(EditarRegion);
    });
}

function EditarRegion() {
    IdRegion = $("#modalEditarRegion").attr('idRegion');
    Nombre = $("#txtRegion", "#modalEditarRegion").val();

    Ejecutar = true;
    errores = [];

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Region es requerido.");
    }

    if (Ejecutar) {
        var Region = new Object();
        Region.IdRegion = IdRegion;
        Region.Region = Nombre;

        var Request = JSON.stringify(Region);
        WM("_Controls/Region.aspx/EditarRegion", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarRegion").modal("hide");
            }
            else {
                Error("Editar Region", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function DesactivarRegion(IdRegion, Baja) {


    Ejecutar = true;
    errores = [];

    if (IdRegion === 0) {
        Ejecutar = false;
        errores.push("Region es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Region = new Object();
        Region.IdRegion = IdRegion;
        Region.Baja = Baja;

        var Request = JSON.stringify(Region);
        WM("_Controls/Region.aspx/DesactivarRegion", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarRegiones();
            }
            else {
                Error("Desactivar región", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}
