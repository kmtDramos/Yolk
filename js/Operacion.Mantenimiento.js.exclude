/**/

$(function () {

    $("#PaginadorMantenimientos #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorMantenimientos #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorMantenimientos #txtPagina").val(Pagina).change();
    });

    $("#PaginadorMantenimientos #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorMantenimientos #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorMantenimientos #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorMantenimientos #txtPagina").val(Pagina).change();
    });

    $("#PaginadorMantenimientos #txtPagina").change(function () {
        ListarMantenimientos();
    }).change();


    $("#btnObtenerFormaAgregarMantenimiento").click(ObtenerFormaAgregarMantenimiento);
});

function ListarMantenimientos() {
    var Mantenimientos = new Object();
    Mantenimientos.Pagina = parseInt($("#PaginadorMantenimientos #txtPagina").val());
    Mantenimientos.Columna = "M.IdMantenimiento";
    Mantenimientos.Orden = "ASC";
    var Request = JSON.stringify(Mantenimientos);
    WM("_Controls/Operacion.Mantenimiento.aspx/ListarMantenimientos", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorMantenimientos #lblPaginas").text(json.Datos.Paginas);
            $("#PaginadorMantenimientos #lblRegistros").text(json.Datos.Registros);

            var Mantenimientos = json.Datos.Mantenimientos;
            $("tbody", "#tblListaMantenimientos").html('');
            for (x in Mantenimientos) {
                var usr = Mantenimientos[x];
                var tr = $("<tr class=\"ku-clickable\"></tr>");
                $(tr)
                    .append($("<td>" + usr.FechaAlta + "</td>"))
                    .append($("<td>" + usr.Nombre + "</td>"))
                    .append($("<td onclick=\"ObtenerFormaEditarMantenimiento(" + usr.IdMantenimiento + ");\">" + usr.Observaciones + "</td>"))
                .append($("<td><span class='glyphicon glyphicon-edit'></span></td>"));
					
                $("tbody", "#tblListaMantenimientos").append(tr);
            }
        }
        else {
            Error("Listar Mantenimientos", json.Error);
        }
    });
}

function ObtenerFormaAgregarMantenimiento() {
    var Mantenimiento = new Object();
    var pRequest = JSON.stringify(Mantenimiento);
    ASPT(pRequest, "_Controls/Operacion.Mantenimiento.aspx/ObtenerFormaAgregarMantenimiento", "_Views/tmplAgregarMantenimiento.html", "#modalAgregarMantenimientoTmpl", procesarAgregarMantenimiento);
}

function procesarAgregarMantenimiento() {
    if (data.Error == "") {
        modalName = 'modalAgregarMantenimiento';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarMantenimientoTmpl").processTemplate(data);
        $("#" + modalName).modal();


        $("#btnAgregarMantenimiento").click(function () {
            AgregarMantenimiento();
        });        
    }
    else {
        Error("Forma alta mantenimiento ", json.Error);
    }
}

function AgregarMantenimiento() {

}