/**/
$(function () {

    $("#PaginadorMenu #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorMenu #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorMenu #txtPagina").val(Pagina).change();
    });

    $("#PaginadorMenu #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorMenu #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorMenu #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorMenu #txtPagina").val(Pagina).change();
    });

    $("#PaginadorMenu #txtPagina").change(function () {
        ListarMenus();
    }).change();

    $("#btnObtenerFormaAgregarMenu").click(ObtenerFormaAgregarMenu);

    $("#tblListaMenus #thActivo").change(function () {
        ListarMenus();
    }).change();

});

function ListarMenus() {
    var cMenu = new Object();
    cMenu.Pagina = parseInt($("#PaginadorMenu #txtPagina").val());
    cMenu.Columna = "Orden";
    cMenu.Orden = "ASC";
    cMenu.Baja = $("#tblListaMenus #thActivo").val();
    var Request = JSON.stringify(cMenu);
    WM("_Controls/Seguridad.Menu.aspx/ListarMenus", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorMenu #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorMenu #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var datosMenu = json.Datos.Menus;
            $("tbody", "#tblListaMenus").html('');
            for (x in datosMenu) {
                var m = datosMenu[x];
                var tr = $("<tr data-id=" + m.IdMenu + "></tr>");
                $(tr)
                    .append($("<td>" + m.Menu + "</td>"))
                    .append($("<td id=\"editaOrden\">" + m.Orden + "</td>"))
                    .append($("<td class=\"text-center ku-clickable\" onclick=\"ObtenerFormaEditarMenu(" + m.IdMenu + ");\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td  class=\"ku-clickable text-center\" id='desactivarMenu' onclick=\"DesactivarMenu(" + m.IdMenu + "," + m.Baja + " );\" estatus=\"" + m.Baja + "\">" + m.Estatus + "</td>"));
                $("tbody", "#tblListaMenus").append(tr);
            }
            html = "<tr><td></td><td><button id='btnGuardarOrden'>Reordenar</button></td><td></td><td></td></tr>";
            $("tbody", "#tblListaMenus").append(html);
            
            $("#tblListaMenus.table tr td#editaOrden").bind("click", dataClick);
            $("#btnGuardarOrden").bind("click", saveButton);
        }
        else {
            Error("Listar Menus", json.Error);
        }
    });

}

function DesactivarMenu(IdMenu, Baja) {

    Ejecutar = true;
    errores = [];

    if (IdMenu === 0) {
        Ejecutar = false;
        errores.push("El menu requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var cMenu = new Object();
        cMenu.IdMenu = IdMenu;
        cMenu.Baja = Baja;

        var Request = JSON.stringify(cMenu);
        WM("_Controls/Seguridad.Menu.aspx/DesactivarMenu", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarMenus();
            }
            else {
                Error("Desactivar Menu", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaAgregarMenu() {
    var Menu = new Object();
    var pRequest = JSON.stringify(Menu);
    ASPT(pRequest, "_Controls/Seguridad.Menu.aspx/ObtenerFormaAgregarMenu", "_Views/tmplAgregarMenu.html", "#modalAgregarMenuTmpl", procesarAgregarMenu);
}

function procesarAgregarMenu(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarMenu';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarMenuTmpl").processTemplate(data);
        $("#" + modalName).modal();


        $("#btnAgregarMenu").click(function () {
            AgregarMenu();
        });
    }
    else {
        Error("Forma alta menu ", json.Error);
    }
}

function AgregarMenu() {
    Menu = $("#txtMenu", "#modalAgregarMenu").val();

    Ejecutar = true;
    errores = [];

    if (Menu === "") {
        Ejecutar = false;
        errores.push("El nombre del menu es requerido.");
    }

    if (Ejecutar) {
        var cMenu = new Object();
        cMenu.Menu = Menu;

        var Request = JSON.stringify(cMenu);
        WM("_Controls/Seguridad.Menu.aspx/AgregarMenu", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarMenu").modal("hide");
                ListarMenus();
            }
            else {
                Error("Agregar menu", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarMenu(IdMenu) {
    var cMenu = new Object();
    cMenu.IdMenu = IdMenu;
    var pRequest = JSON.stringify(cMenu);
    ASPT(pRequest, "_Controls/Seguridad.Menu.aspx/ObtenerFormaEditarMenu", "_Views/tmplEditarMenu.html", "#modalEditarMenuTmpl", procesarEditarMenu);
}

function procesarEditarMenu(data) {
    if (data.Error == "") {
        modalName = 'modalEditarMenu';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarMenuTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#btnEditarMenu").click(EditarMenu);
    }
}

function EditarMenu() {
    IdMenu = $("#modalEditarMenu .modal-dialog").attr('idMenu');
    Menu = $("#txtMenu", "#modalEditarMenu").val();

    Ejecutar = true;
    errores = [];

    if (Menu === "") {
        Ejecutar = false;
        errores.push("Nombre del menu es requerido.");
    }


    if (Ejecutar) {
        var cMenu = new Object();
        cMenu.IdMenu = IdMenu;
        cMenu.Menu = Menu;

        var Request = JSON.stringify(cMenu);
        WM("_Controls/Seguridad.Menu.aspx/EditarMenu", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarMenu").modal("hide");
                ListarMenus()
            }
            else {
                Error("Editar menu", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function dataClick(e) {
    console.log(e);
    //if (e.currentTarget.innerHTML != "") return;

    if (e.currentTarget.contentEditable != "") {
        $(e.currentTarget).attr("contentEditable", true);
    }
    else {
        $(e.currentTarget).append("<input type='text'>");
    }
}

function saveButton() {
    var renglones = [];

    $("#tblListaMenus.table tr td#editaOrden").each(function (td, index) {

        var Menus = new Object();
        var row = $(this).parents('tr');
        var pid = row.data('id');
        var porden = $(index).text();

        Menus.IdMenu = pid;
        Menus.Orden = validaNumero(porden) ? porden : 0;
        renglones.push(Menus);

    });

    var pRequestO = new Object();
    pRequestO.pMenu = renglones;

    Ejecutar = true;
    errores = [];
    if (renglones.length == 0) {
        Ejecutar = false;
        errores.push("Debe haber un registro a ordenar");
    }

    if (Ejecutar) {
        var Menu = new Object();
        Menu.pRequest = pRequestO;
        var Request = JSON.stringify(Menu);
        WM("_Controls/Seguridad.Menu.aspx/Reordenar", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarMenus();
            }
            else {
                Error("Reordenar menu", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}