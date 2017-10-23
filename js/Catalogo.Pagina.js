$(function () {
    $("#PaginadorPaginas #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorPaginas #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorPaginas #txtPagina").val(Pagina).change();
    });

    $("#PaginadorPaginas #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorPaginas #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorPaginas #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorPaginas #txtPagina").val(Pagina).change();
    });

    $("#PaginadorPaginas #txtPagina").change(function () {
        ListarPaginas();
    }).change();


    $("#btnObtenerFormaAgregarPagina").click(ObtenerFormaAgregarPagina);

    $('body').on('click', "#btnConsultarPagina", function (e) {
        e.preventDefault();
        var row = $(this).parents('tr');
        var id = row.data('id');
        ObtenerFormaEditarPagina(id);
    });

    $('body').on('click', "#btnDesactivarPagina", function (e) {
        e.preventDefault();
        var row = $(this).parents('tr');
        var id = row.data('id');
        var estatus = $(this).attr('estatus');
        DesactivarPagina(id, estatus);
    });

    $("#tblListaPaginas #thActivo").change(function () {
        ListarPaginas();
    }).change();

});

function ListarPaginas() {
    var Paginas = new Object();
    Paginas.Pagina = parseInt($("#PaginadorPaginas #txtPagina").val());
    Paginas.Columna = "M.Orden, P.Orden";
    Paginas.Orden = "ASC";
    Paginas.pBaja = $("#thActivo").val();

    var Request = JSON.stringify(Paginas);
    WM("_Controls/Pagina.aspx/ListarPaginas", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorPaginas #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorPaginas #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Paginas = json.Datos.Paginas;
            $("tbody", "#tblListaPaginas").html('');
            for (x in Paginas) {
                var pag = Paginas[x];
                var tr = $("<tr data-id=" + pag.IdPagina + "></tr>");
                $(tr)
                    
                    .append($("<td>" + pag.Menu + "</td>"))
                    .append($("<td>" + pag.Pagina + "</td>"))
                    .append($("<td id=\"editaOrden\">" + pag.Orden + "</td>"))
                    .append($("<td>" + pag.Permiso + "</td>"))
					.append($("<td id=\"btnConsultarPagina\" class='ku-clickable'><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td id=\"btnDesactivarPagina\" estatus=\"" + pag.Baja + "\" class=\"ku-clickable text-center\">" + pag.Estatus + "</td>"));
                $("tbody", "#tblListaPaginas").append(tr);
            }

            html = "<tr><td></td><td></td><td><button id='btnGuardarOrden'>Reordenar</button></td><td></td><td></td><td></td></tr>";
            $("tbody", "#tblListaPaginas").append(html);

            $("#tblListaPaginas.table tr td#editaOrden").bind("click", dataClick);
            $("#btnGuardarOrden").bind("click", saveButton);

        }
        else {
            Error("Listar Paginas", json.Error);
        }
    });
}

function DesactivarPagina(IdPagina, Baja) {
    Ejecutar = true;
    errores = [];

    if (IdPagina === 0) {
        Ejecutar = false;
        errores.push("Pagina es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Pagina = new Object();
        Pagina.IdPagina = IdPagina;
        Pagina.Baja = Baja;

        var Request = JSON.stringify(Pagina);
        WM("_Controls/Pagina.aspx/DesactivarPagina", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarPaginas();
            }
            else {
                Error("Desactivar Pagina", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaAgregarPagina() {
    var Cliente = new Object();
    var pRequest = JSON.stringify(Cliente);
    ASPT(pRequest, "_Controls/Pagina.aspx/ObtenerFormaAgregarPagina", "_Views/tmplAgregarPagina.html", "#modalAgregarPaginaTmpl", procesarFormaAgregarPagina);
}

function procesarFormaAgregarPagina(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarPagina';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarPaginaTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#btnAgregarPagina").click(function () {
            AgregarPagina();
        });

    }
    else {
        Error("Forma alta página ", json.Error);
    }
}

function ObtenerFormaEditarPagina(IdPagina) {
    var Pagina = new Object();
    Pagina.IdPagina = IdPagina;
    var pRequest = JSON.stringify(Pagina);
    ASPT(pRequest, "_Controls/Pagina.aspx/ObtenerFormaEditarPagina", "_Views/tmplEditarPagina.html", "#modalEditarPaginaTmpl", procesarEditarPagina);
}

function procesarEditarPagina(data) {
    if (data.Error == "") {
        modalName = 'modalEditarPagina';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarPaginaTmpl").processTemplate(data);
        $("#" + modalName).modal();

        //Al terminar de cargar el modal
        $("#" + modalName).on('shown.bs.modal', function () {

            $("#btnEditarPagina").click(EditarPagina);
        });
    }
}

function EditarPagina() {
    IdPagina = $("#modalEditarPagina .modal-dialog").attr('idPagina');
    Pagina = $("#txtPagina", "#modalEditarPagina").val();
    Descripcion = $("#txtDescripcion", "#modalEditarPagina").val();
    IdMenu = $("#cmbMenu", "#modalEditarPagina").val();
    IdPermiso = $("#cmbPermiso", "#modalEditarPagina").val();

    Ejecutar = true;
    errores = [];

    if (IdPagina === 0) {
        Ejecutar = false;
        errores.push("Nombre del página es requerido.");
    }



    if (Ejecutar) {
        var cPagina = new Object();
        cPagina.Pagina = Pagina;
        cPagina.IdPagina = IdPagina;
        cPagina.Descripcion = Descripcion;
        cPagina.IdMenu = IdMenu;
        cPagina.IdPermiso = IdPermiso;


        var Request = JSON.stringify(cPagina);
        WM("_Controls/Pagina.aspx/EditarPagina", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarPagina").modal("hide");
                ListarPaginas()
            }
            else {
                Error("Editar pagina", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function AgregarPagina() {
    Pagina = $("#txtPagina", "#modalAgregarPagina").val();
    Descripcion = $("#txtDescripcion", "#modalAgregarPagina").val();
    IdMenu = $("#cmbMenu", "#modalAgregarPagina").val();
    IdPermiso = $("#cmbPermiso", "#modalAgregarPagina").val();

    Ejecutar = true;
    errores = [];

    if (Pagina === "") {
        Ejecutar = false;
        errores.push("País es requerido.");
    }

    if (Ejecutar) {
        var cPagina = new Object();
        cPagina.Pagina = Pagina;
        cPagina.Descripcion = Descripcion;
        cPagina.IdMenu = IdMenu;
        cPagina.IdPermiso = IdPermiso;

        var request = JSON.stringify(cPagina);
        WM("_Controls/Pagina.aspx/AgregarPagina", request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarPagina").modal("hide");
                ListarPaginas();
            }
            else {
                Error("Agregar página", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }

}



function dataClick(e) {
    if (e.currentTarget.contentEditable != "") {
        $(e.currentTarget).attr("contentEditable", true);
    }
    else {
        $(e.currentTarget).append("<input type='text'>");
    }
}

function saveButton() {
    var renglones = [];

    $("#tblListaPaginas.table tr td#editaOrden").each(function (td, index) {

        var Paginas = new Object();
        var row = $(this).parents('tr');
        var pid = row.data('id');
        var porden = $(index).text();

        Paginas.IdPagina = pid;
        Paginas.Orden = validaNumero(porden) ? porden : 0;
        renglones.push(Paginas);

    });

    var pRequestO = new Object();
    pRequestO.pPagina = renglones;

    Ejecutar = true;
    errores = [];
    if (renglones.length == 0) {
        Ejecutar = false;
        errores.push("Debe haber un registro a ordenar");
    }

    if (Ejecutar) {
        var Pagina = new Object();
        Pagina.pRequest = pRequestO;
        var Request = JSON.stringify(Pagina);
        WM("_Controls/Pagina.aspx/Reordenar", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarPaginas();
            }
            else {
                Error("Reordenar paginas", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}