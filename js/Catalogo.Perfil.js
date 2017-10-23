$(function () {
    $("#PaginadorPerfiles #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorPerfiles #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorPerfiles #txtPagina").val(Pagina).change();
    });

    $("#PaginadorPerfiles #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorPerfiles #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorPerfiles #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorPerfiles #txtPagina").val(Pagina).change();
    });

    $("#PaginadorPerfiles #txtPagina").change(function () {
        ListarPerfiles();
    }).change();


    $("#btnObtenerFormaAgregarPerfil").click(ObtenerFormaAgregarPerfil);

    $('body').on('click', "#btnObtenerFormaAgregarPerfil", function (e) {
        e.preventDefault();
        var row = $(this).parents('tr');
        var id = row.data('id');
        ObtenerFormaEditarPerfil(id);
    });

    $('body').on('click', "#btnAsignarPermisos", function (e) {
        AsignarPermisos();
    });
    $('body').on('click', "#btnDesasignarPermisos", function (e) {
        DesasignarPermisos();
    });

    $('body').on('click', "#TodosAsignar", function (e) {
        if ($(this).is(':checked')) {
            $('input[class=ChkAsignar]').prop('checked', true);
        } else {
            $('input[class=ChkAsignar]').prop('checked', false);
        }
    });

    $('body').on('click', "#TodosDesAsignar", function (e) {
        if ($(this).is(':checked')) {
            $('input[class=ChkDesasignar]').prop('checked', true);
        } else {
            $('input[class=ChkDesasignar]').prop('checked', false);
        }
    });

});

function ListarPermisosDisponibles() {
    var PerfilPermiso = new Object();
    PerfilPermiso.Pagina = parseInt($("#PaginadorPermisosDisponibles #txtPagina").val());
    PerfilPermiso.Columna = "S.Permiso";
    PerfilPermiso.Orden = "ASC";
    PerfilPermiso.IdPerfil = $("#modalPerfilPermisos .modal-dialog").attr('idPerfil');
    
    var Request = JSON.stringify(PerfilPermiso);
    WM("_Controls/Perfil.aspx/ListarPermisosDisponibles", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorPermisosDisponibles #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorPermisosDisponibles #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var PermisosDisponibles = json.Datos.PermisosDisponibles;
            $("tbody", "#tblListaPermisosDisponibles").html('');
            for (x in PermisosDisponibles) {
                var suc = PermisosDisponibles[x];
                var tr = $("<tr data-id=" + suc.IdSucursal + "></tr>");
                $(tr)

                    .append($("<td class='text-left'>" + suc.Permiso + "</td>"))
                    .append($("<td class='text-center'><input type='checkbox' class='ChkAsignar' value=" + suc.IdPermiso + "></td>"));
                $("tbody", "#tblListaPermisosDisponibles").append(tr);
            }
        }
        else {
            Error("Listar Permisos disponibles", json.Error);
        }
    });
}

function ListarPermisosAsignadas() {
    var PerfilPermiso = new Object();
    PerfilPermiso.Pagina = parseInt($("#PaginadorPermisosAsignadas #txtPagina").val());
    PerfilPermiso.Columna = "P.Permiso";
    PerfilPermiso.Orden = "ASC";
    PerfilPermiso.IdPerfil = $("#modalPerfilPermisos .modal-dialog").attr('idPerfil');

    var Request = JSON.stringify(PerfilPermiso);
    WM("_Controls/Perfil.aspx/ListarPermisosAsignadas", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        //IdSucursalPredeterminada = json.IdSucursalPredeterminada;

        if (json.Error == "") {

            $("#PaginadorPermisosAsignadas #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorPermisosAsignadas #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var PermisosAsignadas = json.Datos.PermisosAsignadas;
            $("tbody", "#tblListaPermisosAsignadas").html('');
            for (x in PermisosAsignadas) {
                predeterminado = "<img src=\"imgs/off.png\">";

                var suc = PermisosAsignadas[x];

                //if (IdSucursalPredeterminada == suc.IdSucursal) {
                //    predeterminado = "<img src=\"imgs/on.png\">";
                //}
                var tr = $("<tr data-id=" + suc.IdPerfilPermiso + "></tr>");
                $(tr)
                    //.append($("<td class='text-center'><div id=\"divPredeterminada\">" + predeterminado + "</div></td>"))
                    //.append($("<td class='text-left'>" + suc.Perfil + "</td>"))
                    .append($("<td class='text-left'>" + suc.Permiso + "</td>"))
                    .append($("<td class='text-center'><input type='checkbox' class='ChkDesasignar' value=" + suc.IdPerfilPermiso + "></td>"));
                $("tbody", "#tblListaPermisosAsignadas").append(tr);
            }
        }
        else {
            Error("Listar Permisos asignados", json.Error);
        }
    });
}


function AsignarPermisos() {
    var FilasDisponibles = [];

    $(".ChkAsignar").each(function (index) {
        if ($(this).is(':checked')) {
            FilasDisponibles.push($(this).val());
        }
    });

    Ejecutar = true;
    errores = [];
    if (FilasDisponibles.length == 0) {
        Ejecutar = false;
        errores.push("Debe selecionar los permisos a asignar");
    }

    if (Ejecutar) {
        var Permiso = new Object();
        Permiso.IdPermisoDisponible = FilasDisponibles;
        Permiso.IdPerfil = $("#modalPerfilPermisos .modal-dialog").attr('idPerfil');

        var Request = JSON.stringify(Permiso);
        WM("_Controls/Perfil.aspx/AsignarPermisos", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarPermisosDisponibles()
                ListarPermisosAsignadas()
            }
            else {
                Error("Asignar Permisos", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function DesasignarPermisos() {
    var FilasAsignadas = [];

    $(".ChkDesasignar").each(function (index) {
        if ($(this).is(':checked')) {
            FilasAsignadas.push($(this).val());
        }
    });

    Ejecutar = true;
    errores = [];
    if (FilasAsignadas.length == 0) {
        Ejecutar = false;
        errores.push("Debe selecionar los permisos para desasignar");
    }

    if (Ejecutar) {
        var Permiso = new Object();
        Permiso.pIdPerfilPermiso = FilasAsignadas;
        Permiso.IdPerfil = $("#modalPerfilPermisos .modal-dialog").attr('idPerfil');;

        var Request = JSON.stringify(Permiso);
        WM("_Controls/Perfil.aspx/DesasignarPermisos", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarPermisosDisponibles()
                ListarPermisosAsignadas()
            }
            else {
                Error("Desasignar permisos", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}


function ListarPerfiles() {
    var Perfiles = new Object();
    Perfiles.Pagina = parseInt($("#PaginadorPerfiles #txtPagina").val());
    Perfiles.Columna = "P.Perfil";
    Perfiles.Orden = "ASC";
    Perfiles.pBaja = -1;

    var Request = JSON.stringify(Perfiles);
    WM("_Controls/Perfil.aspx/ListarPerfiles", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorPerfiles #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorPerfiles #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Perfiles = json.Datos.Perfiles;
            $("tbody", "#tblListaPerfiles").html('');
            for (x in Perfiles) {
                var p = Perfiles[x];
                var tr = $("<tr data-id=" + p.IdPerfil + "></tr>");
                $(tr)
                    .append($("<td>" + p.Perfil + "</td>"))
                    .append($("<td>" + p.Pagina + "</td>"))
                    .append($("<td class=\"ku-clickable text-center\" onclick=\"btnPerfilPermiso(" + p.IdPerfil + ");\"><span class='glyphicon glyphicon-home'></span></td>"))
                    //.append($("<td id=\"btnPerfilPermiso(" + p.Perfil + ")\" class=\"ku-clickable text-center\"><span class='glyphicon glyphicon-link'></span></td>"))
                    //.append($("<td class=\"ku-clickable text-center\" onclick=\"ObtenerFormaAsignarSucursal(" + usr.IdUsuario + ");\"><span class='glyphicon glyphicon-home'></span></td>"))
					.append($("<td id=\"btnObtenerFormaAgregarPerfil\" class=\"ku-clickable text-center\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td  class=\"ku-clickable text-center\" id='desactivarPerfil' onclick=\"DesactivarPerfil(" + p.IdPerfil + "," + p.Baja + " );\" estatus=\"" + p.Baja + "\">" + p.Estatus + "</td>"));
                $("tbody", "#tblListaPerfiles").append(tr);
            }
        }
        else {
            Error("Listar Perfiles", json.Error);
        }
    });
}

function DesactivarPerfil(IdPerfil, Baja) {
    Ejecutar = true;
    errores = [];

    if (IdPerfil === 0) {
        Ejecutar = false;
        errores.push("Perfil es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Perfil = new Object();
        Perfil.IdPerfil = IdPerfil;
        Perfil.Baja = Baja;

        var Request = JSON.stringify(Perfil);
        WM("_Controls/Perfil.aspx/DesactivarPerfil", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarPerfiles();
            }
            else {
                Error("Desactivar Perfil", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaAgregarPerfil() {
    var Cliente = new Object();
    var pRequest = JSON.stringify(Cliente);
    ASPT(pRequest, "_Controls/Perfil.aspx/ObtenerFormaAgregarPerfil", "_Views/tmplAgregarPerfil.html", "#modalAgregarPerfilTmpl", procesarFormaAgregarPerfil);
}

function procesarFormaAgregarPerfil(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarPerfil';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarPerfilTmpl").processTemplate(data);
        $("#" + modalName).modal();


        $("#btnAgregarPerfil").click(function () {
            AgregarPerfil();
        });

        $("#cmbPermiso").change(function () {
            ObtenerPermisos(parseInt($(this).val()));
        });
    }
    else {
        Error("Forma alta página ", json.Error);
    }
}


function AgregarPerfil() {
    IdPagina = $("#cmbPagina", "#modalAgregarPerfil").val();    
    DescripcionPerfil = $("#txtPerfil", "#modalAgregarPerfil").val();

    Ejecutar = true;
    errores = [];

    if (IdPagina === "0") {
        Ejecutar = false;
        errores.push("Pagina es requerido.");
    }

    if (DescripcionPerfil === "") {
        Ejecutar = false;
        errores.push("Perfil es requerido.");
    }

    if (Ejecutar) {
        var Perfil = new Object();
        Perfil.IdPagina = IdPagina;
        Perfil.Perfil = DescripcionPerfil;

        var Request = JSON.stringify(Perfil);
        WM("_Controls/Perfil.aspx/AgregarPerfil", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarPerfil").modal("hide");
                ListarPerfiles();
            }
            else {
                Error("Agregar perfil", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}


function ObtenerFormaEditarPerfil(IdPerfil) {
    var Perfil = new Object();
    Perfil.IdPerfil = IdPerfil;
    var pRequest = JSON.stringify(Perfil);
    ASPT(pRequest, "_Controls/Perfil.aspx/ObtenerFormaEditarPerfil", "_Views/tmplEditarPerfil.html", "#modalEditarPerfilTmpl", procesarEditarPerfil);
}

function procesarEditarPerfil(data) {
    if (data.Error == "") {
        modalName = 'modalEditarPerfil';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarPerfilTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#btnEditarPerfil").click(EditarPerfil);

        $('#modalEditarPerfil').on('click', "#btnEditarPerfil", function (e) {
            e.preventDefault();
            e.stopPropagation();
            EditarPerfil();
        });
    }
}

function EditarPerfil() {
    IdPerfil = $("#modalEditarPerfil .modal-dialog").attr('idPerfil');
    DescripcionPerfil = $("#txtPerfil", "#modalEditarPerfil").val();
    IdPagina = $("#cmbPagina", "#modalEditarPerfil").val();

    Ejecutar = true;
    errores = [];

    if (DescripcionPerfil === "") {
        Ejecutar = false;
        errores.push("Perfil es requerido.");
    }

    if (IdPagina === "0") {
        Ejecutar = false;
        errores.push("Página es requerido.");
    }
   
    if (Ejecutar) {
        var Perfil = new Object();
        Perfil.IdPerfil = IdPerfil;
        Perfil.Perfil = DescripcionPerfil;
        Perfil.IdPagina = IdPagina;        

        var Request = JSON.stringify(Perfil);
        WM("_Controls/Perfil.aspx/EditarPerfil", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarPerfil").modal("hide");
                ListarPerfiles()
            }
            else {
                Error("Editar perfil", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function btnPerfilPermiso(IdPerfil) {
    var Usuario = new Object();
    Usuario.IdPerfil = IdPerfil;
    var pRequest = JSON.stringify(Usuario);
    ASPT(pRequest, "_Controls/Perfil.aspx/ObtenerFormaListarPerfilPermisos", "_Views/tmplPerfilPermisos.html", "#modalPerfilPermisosTmpl", procesarListarPerfilPermisos);
}

function procesarListarPerfilPermisos(data) {
    respuesta = data.Datos.PermisosDisponibles;
    if (data.Error == "") {
        modalName = 'modalPerfilPermisos';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalPerfilPermisosTmpl").processTemplate(data);
        $("#" + modalName).modal();
    }

    $("#" + modalName).on('shown.bs.modal', function () {
        ///////////////////////////////////////////
        //Permisos DISPONIBLES
        $("#PaginadorPermisosDisponibles #btnAnteriorPagina").click(function () {
            var Pagina = parseInt($("#PaginadorPermisosDisponibles #txtPagina").val()) - 1;
            Pagina = (Pagina < 1) ? 1 : Pagina;
            $("#PaginadorPermisosDisponibles #txtPagina").val(Pagina).change();
        });

        $("#PaginadorPermisosDisponibles #btnSiguientePagina").click(function () {
            var Paginas = parseInt($("#PaginadorPermisosDisponibles #lblPaginas").text());
            var Pagina = parseInt($("#PaginadorPermisosDisponibles #txtPagina").val()) + 1;
            Pagina = (Pagina > Paginas) ? Paginas : Pagina;
            $("#PaginadorPermisosDisponibles #txtPagina").val(Pagina).change();
        });

        $("#PaginadorPermisosDisponibles #txtPagina").change(function () {
            ListarPermisosDisponibles();
        }).change();

        ///////////////////////////////////////////
        //Permisos ASIGNADAS
        $("#PaginadorPermisosAsignadas #btnAnteriorPagina").click(function () {
            var Pagina = parseInt($("#PaginadorPermisosAsignadas #txtPagina").val()) - 1;
            Pagina = (Pagina < 1) ? 1 : Pagina;
            $("#PaginadorPermisosAsignadas #txtPagina").val(Pagina).change();
        });

        $("#PaginadorPermisosAsignadas #btnSiguientePagina").click(function () {
            var Paginas = parseInt($("#PaginadorPermisosAsignadas #lblPaginas").text());
            var Pagina = parseInt($("#PaginadorPermisosAsignadas #txtPagina").val()) + 1;
            Pagina = (Pagina > Paginas) ? Paginas : Pagina;
            $("#PaginadorPermisosAsignadas #txtPagina").val(Pagina).change();
        });

        $("#PaginadorPermisosAsignadas #txtPagina").change(function () {
            ListarPermisosAsignadas();
        }).change();
    });

    
}
