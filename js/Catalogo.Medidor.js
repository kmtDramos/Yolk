/**/
$(function () {

    ObtenerFiltroCliente();

    $("#PaginadorMedidores #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorMedidores #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorMedidores #txtPagina").val(Pagina).change();
    });

    $("#PaginadorMedidores #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorMedidores #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorMedidores #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorMedidores #txtPagina").val(Pagina).change();
    });

    $("#PaginadorMedidores #txtPagina").change(function () {
        ListarMedidores();
    }).change();


    $("#btnObtenerFormaAgregarMedidor").click(ObtenerFormaAgregarMedidor);


    $("#cmbFiltroCliente").change(function () {
        ObtenerFiltroPaises(parseInt($(this).val()));
    });

    $("#cmbFiltroPais").change(function () {
        IdCliente = parseInt($("#cmbFiltroCliente").val());
        ObtenerFiltroEstados(IdCliente, parseInt($(this).val()));
    });

    $("#cmbFiltroEstado").change(function () {
        IdCliente = parseInt($("#cmbFiltroCliente").val());
        IdPais = parseInt($("#cmbFiltroPais").val());
        ObtenerFiltroMunicipios(IdCliente, IdPais, parseInt($(this).val()));
    });

    $("#cmbFiltroMunicipio").change(function () {
        IdCliente = parseInt($("#cmbFiltroCliente").val());
        ObtenerFiltroSucursales(IdCliente, parseInt($(this).val()));
    });

    $('body').on('click', "#btnBuscarMedidor", function (e) {
        e.preventDefault();
        ListarMedidores();

        $('#FiltroMedidor.panel-collapse.in')
        .collapse('hide');
    });

});

function ListarMedidores() {
    var Medidores = new Object();
    Medidores.Pagina = parseInt($("#PaginadorMedidores #txtPagina").val());
    Medidores.Columna = "Medidor";
    Medidores.Orden = "ASC";
    Medidores.IdCliente = $("#cmbFiltroCliente").val();
    Medidores.IdPais = $("#cmbFiltroPais").val();
    Medidores.IdEstado = $("#cmbFiltroEstado").val();
    Medidores.IdMunicipio = $("#cmbFiltroMunicipio").val();
    Medidores.IdSucursal = $("#cmbFiltroSucursal").val();
    Medidores.Medidor = $("#txtFiltroDescripcionMedidor").val();
    Medidores.Estatus = $("#cmbEstatus").val();

    var Request = JSON.stringify(Medidores);
    WM("_Controls/Catalogo.Medidores.aspx/ListarMedidores", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorMedidores #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorMedidores #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Medidores = json.Datos.Medidores;
            $("tbody", "#tblListaMedidores").html('');
            for (x in Medidores) {
                var usr = Medidores[x];
                var tr = $("<tr></tr>");
                $(tr)
                    .append($("<td>" + usr.Cliente + "</td>"))
                    .append($("<td>" + usr.Sucursal + "</td>"))
                    .append($("<td>" + usr.Medidor + "</td>"))
                    .append($("<td class=\"text-center ku-clickable\" onclick=\"ObtenerFormaTableros(" + usr.IdMedidor + ");\"><span class='glyphicon glyphicon-list-alt'></span></td>"))
                    .append($("<td class=\"text-center ku-clickable\"  onclick=\"ObtenerFormaEditarMedidor(" + usr.IdMedidor + ");\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td id='desactivarMedidor' onclick=\"DesactivarMedidor(" + usr.IdMedidor + "," + usr.Baja + " );\" estatus=\"" + usr.Baja + "\" class=\"ku-clickable hidden-xs hidden-sm\">" + usr.Estatus + "</td>"));
                $("tbody", "#tblListaMedidores").append(tr);
            }
        }
        else {
            Error("Listar Medidores", json.Error);
        }
    });
}

function ObtenerFormaAgregarMedidor() {
    TMPL("tmplAgregarMedidor.html", function (Template) {
        $("body").append(Template);
        $("#modalAgregarMedidor").modal();
        $("#modalAgregarMedidor").on("hidden.bs.modal", function () { $("#modalAgregarMedidor").remove(); ListarMedidores(); });
        $("#btnAgregarMedidor").click(AgregarMedidor);
        obtenerClientes();
    });
}

function AgregarMedidor() {

    Nombre     = $("#txtMedidor", "#modalAgregarMedidor").val();
    IdSucursal = $("#cmbSucursal", "#modalAgregarMedidor").val();
    IdCliente  = $("#cmbCliente", "#modalAgregarMedidor").val();

    Ejecutar = true;
    errores = [];


    if (IdCliente === 0) {
        Ejecutar = false;
        errores.push("Cliente es requerido.");
    }

    if (IdSucursal === 0) {
        Ejecutar = false;
        errores.push("Sucursal es requerido.");
    }

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Medidor es requerido.");
    }

    if (Ejecutar) {
        var Medidor = new Object();
        Medidor.Medidor = Nombre;
        Medidor.IdSucursal = IdSucursal;
        Medidor.IdCliente = IdCliente;

        var Request = JSON.stringify(Medidor);
        WM("_Controls/Catalogo.Medidores.aspx/AgregarMedidor", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarMedidor").modal("hide");
            }
            else {
                Error("Agregar medidor", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarMedidor(IdMedidor) {
    var Medidor = new Object();
    Medidor.IdMedidor = IdMedidor;
    FORM("formEditarMedidor.aspx", Medidor, function (Forma) {
        $("body").append(Forma);
        $("#modalEditarMedidor").modal();
        $("#modalEditarMedidor").on("hidden.bs.modal", function () { $("#modalEditarMedidor").remove(); ListarMedidores(); });
        $("#btnEditarMedidor").click(EditarMedidor);
    });
}

function EditarMedidor() {
    IdMedidor = $("#modalEditarMedidor").attr('idMedidor');
    Nombre = $("#txtMedidor", "#modalEditarMedidor").val();

    Ejecutar = true;
    errores = [];

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Medidor es requerido.");
    }

    if (Ejecutar) {
        var Medidor = new Object();
        Medidor.IdMedidor = IdMedidor;
        Medidor.Medidor = Nombre;

        var Request = JSON.stringify(Medidor);
        WM("_Controls/Catalogo.Medidores.aspx/EditarMedidor", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarMedidor").modal("hide");
            }
            else {
                Error("Editar Medidor", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function DesactivarMedidor(IdMedidor, Baja) {
    IdSucursal = $("#modalMedidores").attr('idSucursal');
    IdCliente = $("#modalSucursales").attr('idCliente');
    Ejecutar = true;
    errores = [];

    if (IdMedidor === 0) {
        Ejecutar = false;
        errores.push("Medidor es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Medidor = new Object();
        Medidor.IdMedidor = IdMedidor;
        Medidor.Baja = Baja;

        var Request = JSON.stringify(Medidor);
        WM("_Controls/Catalogo.Medidores.aspx/DesactivarMedidor", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarMedidores(IdSucursal, IdCliente);
            }
            else {
                Error("Desactivar medidor", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function obtenerClientes() {
    var Cliente = new Object();
    var Request = JSON.stringify(Cliente);
    WM("_Controls/Catalogo.Medidores.aspx/ObtenerFiltroCliente", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbCliente", json.Datos.Clientes);
    });

    $("#cmbCliente").change(function () {
        ObtenerSucursales(parseInt($(this).val()));
    });
}

function ObtenerSucursales(IdCliente) {
    var Sucursal = new Object();
    Sucursal.IdCliente = IdCliente;
    var Request = JSON.stringify(Sucursal);
    WM("_Controls/Catalogo.Medidores.aspx/ObtenerSucursales", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbSucursal", json.Datos.Sucursales);
    });
}

/*
TABLEROS
---------------------------------------------------------------------------------------------------------------------------------
*/
function ObtenerFormaTableros(IdMedidor) {
    TMPL("tmplTableros.html", function (Template) {
        $("body").append(Template);
        $("#modalTableros").modal();
        $("#modalTableros").attr("idMedidor", IdMedidor)
        $("#modalTableros").on("hidden.bs.modal", function () { $("#modalTableros").remove(); });

        $("#PaginadorTableros #btnAnteriorPagina").click(function () {
            var Pagina = parseInt($("#PaginadorTableros #txtPagina").val()) - 1;
            Pagina = (Pagina < 1) ? 1 : Pagina;
            $("#PaginadorTableros #txtPagina").val(Pagina).change();
        });

        $("#PaginadorTableros #btnSiguientePagina").click(function () {
            var Paginas = parseInt($("#PaginadorTableros #lblPaginas").text());
            var Pagina = parseInt($("#PaginadorTableros #txtPagina").val()) + 1;
            Pagina = (Pagina > Paginas) ? Paginas : Pagina;
            $("#PaginadorTableros #txtPagina").val(Pagina).change();
        });

        $("#PaginadorTableros #txtPagina").change(function () {
            ListarTableros(IdMedidor);
        }).change();

        $("#btnObtenerFormaAgregarTablero").click(ObtenerFormaAgregarTablero);

        ListarTableros(IdMedidor)
    });
}

function AgregarTablero() {

    Nombre = $("#txtTablero", "#modalAgregarTablero").val();
    IdMedidor = $("#modalTableros").attr('idMedidor');

    Ejecutar = true;
    errores = [];

    if (IdMedidor === "0") {
        Ejecutar = false;
        errores.push("Medidor es requerido.");
    }

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Medidor es requerido.");
    }

    if (Ejecutar) {
        var Tablero = new Object();
        Tablero.IdMedidor = IdMedidor;
        Tablero.Tablero = Nombre;

        var Request = JSON.stringify(Tablero);
        WM("_Controls/Catalogo.Tablero.aspx/AgregarTablero", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarTablero").modal("hide");
            }
            else {
                Error("Agregar tablero", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ListarTableros(IdMedidor) {
    var Tableros = new Object();
    Tableros.Pagina = parseInt($("#PaginadorTableros #txtPagina").val());
    Tableros.Columna = "Tablero";
    Tableros.Orden = "ASC";
    Tableros.IdMedidor = IdMedidor;
    var Request = JSON.stringify(Tableros);
    WM("_Controls/Catalogo.Tablero.aspx/ListarTableros", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorTableros #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorTableros #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Tableros = json.Datos.Tableros;
            $("tbody", "#tblListaTableros").html('');
            for (x in Tableros) {
                var usr = Tableros[x];
                var tr = $("<tr class=\"ku-clickable\"></tr>");
                $(tr)
					.append($("<td onclick=\"ObtenerFormaEditarTablero(" + usr.IdTablero + ");\">" + usr.Tablero + "</td>"))
                    .append($("<td onclick=\"DesactivarTablero(" + usr.IdTablero + "," + usr.Baja + ");\" class=\"hidden-xs hidden-sm\">" + usr.Estatus + "</td>"));
                $("tbody", "#tblListaTableros").append(tr);
            }
        }
        else {
            Error("Listar Tableros", json.Error);
        }
    });
}

function ObtenerFormaAgregarTablero() {
    IdMedidor = $("#modalTableros").attr('idMedidor');
    TMPL("tmplAgregarTablero.html", function (Template) {
        $("body").append(Template);
        $("#modalAgregarTablero").modal();
        $("#modalAgregarTablero").attr("idMedidor", IdMedidor)
        $("#modalAgregarTablero").on("hidden.bs.modal", function () { $("#modalAgregarTablero").remove(); ListarTableros(IdMedidor); });
        $("#btnAgregarTablero").click(AgregarTablero);
    });
}

function DesactivarTablero(IdTablero, Baja) {
    IdMedidor= $("#modalTableros").attr('idMedidor');
    Ejecutar = true;
    errores = [];

    if (IdTablero === 0) {
        Ejecutar = false;
        errores.push("Tablero es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Tablero = new Object();
        Tablero.IdTablero = IdTablero;
        Tablero.Baja = Baja;

        var Request = JSON.stringify(Tablero);
        WM("_Controls/Catalogo.Tablero.aspx/DesactivarTablero", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarTableros(IdMedidor);
            }
            else {
                Error("Desactivar tablero", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarTablero(IdTablero) {
    IdMedidor = $("#modalTableros").attr('idMedidor');

    var Tablero = new Object();
    Tablero.IdTablero = IdTablero;
    FORM("formEditarTablero.aspx", Tablero, function (Forma) {
        $("body").append(Forma);
        $("#modalEditarTablero").modal();
        $("#modalEditarTablero").on("hidden.bs.modal", function () { $("#modalEditarTablero").remove(); ListarTableros(IdMedidor); });
        $("#btnEditarTablero").click(EditarTablero);
    });
}

function EditarTablero() {
    IdTablero = $("#modalEditarTablero").attr('idTablero');
    Nombre = $("#txtTablero", "#modalEditarTablero").val();

    Ejecutar = true;
    errores = [];

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Tablero es requerido.");
    }

    if (Ejecutar) {
        var Tablero = new Object();
        Tablero.IdTablero = IdTablero;
        Tablero.Tablero = Nombre;

        var Request = JSON.stringify(Tablero);
        WM("_Controls/Catalogo.Tablero.aspx/EditarTablero", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarTablero").modal("hide");
            }
            else {
                Error("Editar tablero", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

//Filtros
function ObtenerFiltroCliente() {
    var Cliente = new Object();
    var Request = JSON.stringify(Cliente);
    WM("_Controls/Catalogo.Medidores.aspx/ObtenerFiltroCliente", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbFiltroCliente", json.Datos.Clientes);
    });
}

function ObtenerFiltroPaises(IdCliente) {
    var Cliente = new Object();
    Cliente.IdCliente = IdCliente;
    var Request = JSON.stringify(Cliente);
    WM("_Controls/Circuito.aspx/ObtenerPaises", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbFiltroPais", json.Datos.Paises);
    });
}

function ObtenerFiltroEstados(IdCliente, IdPais) {
    var Estado = new Object();
    Estado.IdCliente = IdCliente;
    Estado.IdPais = IdPais;
    var Request = JSON.stringify(Estado);
    WM("_Controls/Circuito.aspx/ObtenerEstados", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbFiltroEstado", json.Datos.Estados);
        LimpiarCombo("#cmbFiltroSucursal");
        LimpiarCombo("#cmbFiltroMedidor");
        LimpiarCombo("#cmbFiltroTablero");
    });
}

function ObtenerFiltroMunicipios(IdCliente, IdPais, IdEstado) {
    var Municipio = new Object();
    Municipio.IdCliente = IdCliente;
    Municipio.IdPais = IdPais;
    Municipio.IdEstado = IdEstado;
    var Request = JSON.stringify(Municipio);
    WM("_Controls/Circuito.aspx/ObtenerMunicipios", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbFiltroMunicipio", json.Datos.Municipios);
        LimpiarCombo("#cmbFiltroSucursal");
        LimpiarCombo("#cmbFiltroMedidor");
        LimpiarCombo("#cmbFiltroTablero");
    });

}

function ObtenerFiltroSucursales(IdCliente, IdMunicipio) {
    var Medicion = new Object();
    Medicion.IdCliente = IdCliente;
    Medicion.IdMunicipio = IdMunicipio;
    var Request = JSON.stringify(Medicion);
    WM("_Controls/Circuito.aspx/ObtenerSucursales", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbFiltroSucursal", json.Datos.Sucursales);

        LimpiarCombo("#cmbFiltroMedidor");
        LimpiarCombo("#cmbFiltroTablero");
    });
}