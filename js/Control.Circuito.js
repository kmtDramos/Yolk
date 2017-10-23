
/**/
$(function () {

    ObtenerFiltroCliente();

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

    $("#cmbFiltroSucursal").change(function () {
        ObtenerFiltroMedidores(parseInt($(this).val()));
    });

    $("#cmbFiltroMedidor").change(function () {
        ObtenerFiltroTableros(parseInt($(this).val()));
    });

    $("#PaginadorCircuitos #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorCircuitos #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorCircuitos #txtPagina").val(Pagina).change();
    });

    $("#PaginadorCircuitos #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorCircuitos #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorCircuitos #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorCircuitos #txtPagina").val(Pagina).change();
    });

    $("#PaginadorCircuitos #txtPagina").change(function () {
        ListarCircuitos();
    }).change();

    $("#btnObtenerFormaAgregarCircuito").click(ObtenerFormaAgregarCircuito);

    $('body').on('click', "#btn-Meta", function (e) {
        e.preventDefault();
        var row = $(this).parents('tr');
        var id = row.data('id');
        
        ObtenerFormaMeta(id);
    });

    $('body').on('click', "#btn-EditarMeta", function (e) {
        e.preventDefault();
        var row = $(this).parents('tr');
        var id = row.data('id');

        ObtenerFormaEditarMeta(id);
    });

    $('body').on('click', "#btn-Logo", function (e) {
        e.preventDefault();
        var row = $(this).parents('tr');
        var id = row.data('id');
        
        ObtenerFormaLogo(id);
    });

    $('body').on('click', "#btn-EditarCircuito", function (e) {
        e.preventDefault();
        var row = $(this).parents('tr');
        var id = row.data('id');

        ObtenerFormaEditarCircuito(id);
    });

    $('body').on('click', "#btn-DesactivarCircuito", function (e) {
        e.preventDefault();
        baja = $(this).attr('estatus');
        var row = $(this).parents('tr');
        var id = row.data('id');

        DesactivarCircuito(id, baja);
    });

    $('body').on('click', "#btnBuscarCircuito", function (e) {
        e.preventDefault();
        ListarCircuitos();
        $('#FiltroCircuito.panel-collapse.in')
        .collapse('hide');
    });

    $('body').on('click', "#btnLimpiarFiltroCircuito", function (e) {
        e.preventDefault();
        LimpiarFiltro();
    });

    $('body').on('click', "#btnBuscarMeta", function (e) {
        e.preventDefault();
        ListarMetas();

        $('#FiltroMeta.panel-collapse.in')
        .collapse('hide');
    });

    $('body').on('click', "#btnFiltroMetaLimpiar", function (e) {
        e.preventDefault();
        LimpiarFiltroMeta();
    });

    //$('#tblListaCircuitos').DataTable({
    //    "language": DataTableLanguageJO(1),
    //    "bSort": false
    //});
});

function ListarCircuitos() {
    var Circuitos = new Object();
    Circuitos.Pagina = parseInt($("#PaginadorCircuitos #txtPagina").val());
    Circuitos.Columna = "T.Tablero";
    Circuitos.Orden = "ASC";
    Circuitos.IdCliente = $("#cmbFiltroCliente").val();
    Circuitos.IdPais = $("#cmbFiltroPais").val();
    Circuitos.IdEstado = $("#cmbFiltroEstado").val();
    Circuitos.IdMunicipio = $("#cmbFiltroMunicipio").val();
    Circuitos.IdSucursal = $("#cmbFiltroSucursal").val();
    Circuitos.IdMedidor = $("#cmbFiltroMedidor").val();
    Circuitos.IdTablero = $("#cmbFiltroTablero").val();
    Circuitos.Circuito = $("#txtFiltroCircuito").val();
    Circuitos.Descripcion = $("#txtFiltroDescripcion").val();
    var Request = JSON.stringify(Circuitos);
    WM("_Controls/Circuito.aspx/ListarCircuitos", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorCircuitos #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorCircuitos #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Circuitos = json.Datos.Circuitos;
            $("tbody", "#tblListaCircuitos").html('');
            for (x in Circuitos) {
                var cto = Circuitos[x];
                var tr = $("<tr data-id="+ cto.IdCircuito +"></tr>");
                $(tr)
                    .append($("<td class=\"hidden-xs hidden-sm\">" + cto.Cliente + "</td>"))
                    .append($("<td>" + cto.Sucursal + "</td>"))
                    .append($("<td class=\"hidden-xs hidden-sm\">" + cto.Medidor + "</td>"))
                    .append($("<td>" + cto.Tablero + "</td>"))
					.append($("<td class='text-center'>" + cto.Circuito + "</td>"))
                    .append($("<td>" + cto.Descripcion + "</td>"))
                    .append($("<td class=\"ku-clickable text-center\" id=\"btn-Logo\"><span class='glyphicon glyphicon-picture'></span></td>"))
                    .append($("<td class=\"ku-clickable text-center\" id=\"btn-Meta\"><span class='glyphicon glyphicon-flag'></span></td>"))					
					.append($("<td class=\"text-center ku-clickable\" id=\"btn-EditarCircuito\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td class=\"ku-clickable text-center\" id=\"btn-DesactivarCircuito\"  estatus=\"" + cto.Baja + "\">" + cto.Estatus + "</td>"));
                $("tbody", "#tblListaCircuitos").append(tr);
				
				
            }
        }
        else {
            Error("Listar Circuitos", json.Error);
        }
    });	
}

function DesactivarCircuito(IdCircuito, Baja) {

    Ejecutar = true;
    errores = [];

    if (IdCircuito === 0) {
        Ejecutar = false;
        errores.push("Circuito es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Circuito = new Object();
        Circuito.IdCircuito = IdCircuito;
        Circuito.Baja = Baja;

        var Request = JSON.stringify(Circuito);
        WM("_Controls/Circuito.aspx/DesactivarCircuito", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarCircuitos();
            }
            else {
                Error("Desactivar circuito", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaAgregarCircuito() {
    var Cliente = new Object();
    var pRequest = JSON.stringify(Cliente);
    ASPT(pRequest, "_Controls/Circuito.aspx/ObtenerFormaAgregarCircuito", "_Views/tmplAgregarCircuito.html", "#modalAgregarCircuitoTmpl", procesarAgregarCircuito);
}

function procesarAgregarCircuito(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarCircuito';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarCircuitoTmpl").processTemplate(data);
        $("#" + modalName).modal();

        //Al terminar de cargar el modal
        $("#" + modalName).on('shown.bs.modal', function () {
            $("#btnAgregarCircuito").click(function () {
                AgregarCircuito();
            });

            $("#cmbCliente").change(function () {
                ObtenerPaises(parseInt($(this).val()));
            });

            $("#cmbPais").change(function () {
                IdCliente = parseInt($("#cmbCliente").val());
                ObtenerEstados(IdCliente, parseInt($(this).val()));
            });

            $("#cmbEstado").change(function () {
                IdCliente = parseInt($("#cmbCliente").val());
                IdPais = parseInt($("#cmbPais").val());
                ObtenerMunicipios(IdCliente, IdPais, parseInt($(this).val()));
            });

            $("#cmbMunicipio").change(function () {
                IdCliente = parseInt($("#cmbCliente").val());
                ObtenerSucursales(IdCliente, parseInt($(this).val()));
            });

            $("#cmbSucursal").change(function () {
                ObtenerMedidores(parseInt($(this).val()));
            });

            $("#cmbMedidor").change(function () {
                ObtenerTableros(parseInt($(this).val()));
            });
        });
    }
    else {
        Error("Forma alta circuito ", json.Error);
    }
}

function ObtenerPaises(IdCliente) {
    var Cliente = new Object();
    Cliente.IdCliente = IdCliente;
    var Request = JSON.stringify(Cliente);
    WM("_Controls/Circuito.aspx/ObtenerPaises", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbPais", json.Datos.Paises);
    });
}

function ObtenerEstados(IdCliente, IdPais) {
    var Estado = new Object();
    Estado.IdCliente = IdCliente;
    Estado.IdPais = IdPais;
    var Request = JSON.stringify(Estado);
    WM("_Controls/Circuito.aspx/ObtenerEstados", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbEstado", json.Datos.Estados);
        LimpiarCombo("#cmbSucursal");
        LimpiarCombo("#cmbMedidor");
        LimpiarCombo("#cmbTablero");
    });
}

function ObtenerMunicipios(IdCliente, IdPais, IdEstado) {
    var Municipio = new Object();
    Municipio.IdCliente = IdCliente;
    Municipio.IdPais = IdPais;
    Municipio.IdEstado = IdEstado;
    var Request = JSON.stringify(Municipio);
    WM("_Controls/Circuito.aspx/ObtenerMunicipios", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbMunicipio", json.Datos.Municipios);
        LimpiarCombo("#cmbSucursal");
        LimpiarCombo("#cmbMedidor");
        LimpiarCombo("#cmbTablero");
    });

}

function ObtenerSucursales(IdCliente, IdMunicipio) {
    var Medicion = new Object();
    Medicion.IdCliente = IdCliente;
    Medicion.IdMunicipio = IdMunicipio;
    var Request = JSON.stringify(Medicion);
    WM("_Controls/Circuito.aspx/ObtenerSucursales", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbSucursal", json.Datos.Sucursales);

        LimpiarCombo("#cmbMedidor");
        LimpiarCombo("#cmbTablero");
    });
}

function ObtenerMedidores(IdSucursal) {
    var Medidor = new Object();
    Medidor.IdSucursal = IdSucursal;
    var Request = JSON.stringify(Medidor);
    WM("_Controls/Circuito.aspx/ObtenerMedidores", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbMedidor", json.Datos.Medidores);
    });
}

function ObtenerTableros(IdMedidor) {
    var Tablero = new Object();
    Tablero.IdMedidor = IdMedidor;
    var Request = JSON.stringify(Tablero);
    WM("_Controls/Circuito.aspx/ObtenerTableros", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbTablero", json.Datos.Tableros);
    });
}

function AgregarCircuito() {
    IdCliente = $("#cmbCliente", "#modalAgregarCircuito").val();
    IdPais = $("#cmbPais", "#modalAgregarCircuito").val();
    IdEstado = $("#cmbEstado", "#modalAgregarCircuito").val();
    IdMunicipio = $("#cmbMunicipio", "#modalAgregarCircuito").val();
    IdSucursal = $("#cmbSucursal", "#modalAgregarCircuito").val();
    IdMedidor = $("#cmbMedidor", "#modalAgregarCircuito").val();
    IdTablero = $("#cmbTablero", "#modalAgregarCircuito").val();
    NumeroCircuito = $("#txtNumeroCircuito", "#modalAgregarCircuito").val();
    Descripcion    = $("#txtDescripcion", "#modalAgregarCircuito").val();

    Ejecutar = true;
    errores = [];

    if (IdCliente === "0") {
        Ejecutar = false;
        errores.push("Cliente es requerido.");
    }

    if (IdPais === "0") {
        Ejecutar = false;
        errores.push("Pais es requerido.");
    }

    if (IdEstado === "0") {
        Ejecutar = false;
        errores.push("Estado es requerido.");
    }

    if (IdMunicipio === "0") {
        Ejecutar = false;
        errores.push("Municipio es requerido.");
    }

    if (IdSucursal === "0") {
        Ejecutar = false;
        errores.push("Sucursal es requerido.");
    }

    if (IdMedidor === "0") {
        Ejecutar = false;
        errores.push("Medidor es requerido.");
    }

    if (IdTablero === "0") {
        Ejecutar = false;
        errores.push("Tablero es requerido.");
    }

    if (NumeroCircuito === "") {
        Ejecutar = false;
        errores.push("No. circuito es requerido.");
    }

    if (Descripcion === "") {
        Ejecutar = false;
        errores.push("Descripción es requerido.");
    }   

    if (Ejecutar) {
        var Circuito = new Object();
        Circuito.IdMedidor = IdMedidor;
        Circuito.IdTablero = IdTablero;
        Circuito.NumeroCircuito = NumeroCircuito;
        Circuito.Descripcion = Descripcion;

        var Request = JSON.stringify(Circuito);
        WM("_Controls/Circuito.aspx/AgregarCircuito", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarCircuito").modal("hide");
                ListarCircuitos();
            }
            else {
                Error("Agregar circuito", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarCircuito(IdCircuito) {
    var Circuito = new Object();
    Circuito.IdCircuito= IdCircuito;
    var pRequest = JSON.stringify(Circuito);
    ASPT(pRequest, "_Controls/Circuito.aspx/ObtenerFormaEditarCircuito", "_Views/tmplEditarCircuito.html", "#modalEditarCircuitoTmpl", procesarEditarCircuito);
}

function procesarEditarCircuito(data) {
    if (data.Error == "") {
        modalName = 'modalEditarCircuito';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarCircuitoTmpl").processTemplate(data);
        $("#" + modalName).modal();

        //Al terminar de cargar el modal
        $("#" + modalName).on('shown.bs.modal', function () {

            $("#btnEditarCircuito").click(EditarCircuito);

            $("#cmbCliente").change(function () {
                ObtenerPaises(parseInt($(this).val()));
            });

            $("#cmbPais").change(function () {
                IdCliente = parseInt($("#cmbCliente").val());
                ObtenerEstados(IdCliente, parseInt($(this).val()));
            });

            $("#cmbEstado").change(function () {
                IdCliente = parseInt($("#cmbCliente").val());
                IdPais = parseInt($("#cmbPais").val());
                ObtenerMunicipios(IdCliente, IdPais, parseInt($(this).val()));
            });

            $("#cmbMunicipio").change(function () {
                IdCliente = parseInt($("#cmbCliente").val());
                ObtenerSucursales(IdCliente, parseInt($(this).val()));
            });

            $("#cmbSucursal").change(function () {
                ObtenerMedidores(parseInt($(this).val()));
            });

            $("#cmbMedidor").change(function () {
                ObtenerTableros(parseInt($(this).val()));
            });
        });
    }
}

function EditarCircuito() {
    IdCircuito = $("#modalEditarCircuito .modal-dialog").attr('idCircuito');
    IdCliente = $("#cmbCliente", "#modalEditarCircuito").val();
    IdPais = $("#cmbPais", "#modalEditarCircuito").val();
    IdEstado = $("#cmbEstado", "#modalEditarCircuito").val();
    IdMunicipio = $("#cmbMunicipio", "#modalEditarCircuito").val();
    IdSucursal = $("#cmbSucursal", "#modalEditarCircuito").val();
    IdMedidor = $("#cmbMedidor", "#modalEditarCircuito").val();
    IdTablero = $("#cmbTablero", "#modalEditarCircuito").val();
    NumeroCircuito = $("#txtNumeroCircuito", "#modalEditarCircuito").val();
    Descripcion = $("#txtDescripcion", "#modalEditarCircuito").val();

    Ejecutar = true;
    errores = [];

    if (IdCliente === "0") {
        Ejecutar = false;
        errores.push("Cliente es requerido.");
    }

    if (IdPais === "0") {
        Ejecutar = false;
        errores.push("Pais es requerido.");
    }

    if (IdEstado === "0") {
        Ejecutar = false;
        errores.push("Estado es requerido.");
    }

    if (IdMunicipio === "0") {
        Ejecutar = false;
        errores.push("Municipio es requerido.");
    }

    if (IdSucursal === "0") {
        Ejecutar = false;
        errores.push("Sucursal es requerido.");
    }

    if (IdMedidor === "0") {
        Ejecutar = false;
        errores.push("Medidor es requerido.");
    }

    if (IdTablero === "0") {
        Ejecutar = false;
        errores.push("Tablero es requerido.");
    }

    if (NumeroCircuito === "") {
        Ejecutar = false;
        errores.push("No. circuito es requerido.");
    }

    if (Descripcion === "") {
        Ejecutar = false;
        errores.push("Descripción es requerido.");
    }

    if (Ejecutar) {
        var Circuito = new Object();
        Circuito.IdCircuito = IdCircuito;
        Circuito.IdTablero = IdTablero;
        Circuito.Circuito = NumeroCircuito;
        Circuito.Descripcion = Descripcion;
        Circuito.IdMedidor = IdMedidor;

        var Request = JSON.stringify(Circuito);
        WM("_Controls/Circuito.aspx/EditarCircuito", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarCircuito").modal("hide");
				ListarCircuitos()
            }
            else {
                Error("Editar circuito", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaLogo(IdCircuito) {
    var Circuito = new Object();
    Circuito.IdCircuito = IdCircuito;
    var pRequest = JSON.stringify(Circuito);
    ASPT(pRequest, "_Controls/Circuito.aspx/ObtenerFormaLogo", "_Views/tmplAgregarCircuitoImagen.html", "#modalAgregarCircuitoImagenTmpl", procesarAgregarCircuitoImagen);
}

function procesarAgregarCircuitoImagen(data) {
    if (data.Error == "") {
        IdCircuito =data.Datos.Circuito.IdCircuito;
        modalName = 'modalAgregarCircuitoImagen';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarCircuitoImagenTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#" + modalName + " .modal-dialog").attr('idCircuito', IdCircuito);
        $("#" + modalName + " #txtCircuito").text(data.Datos.Circuito.Circuito);
        $("#" + modalName + " #txtLogo").attr("src", data.Datos.Circuito.URL);

        var uploader = new qq.FileUploader({
            element: document.getElementById('file-uploader-imageLogo'),
            multiple: false,
            action: 'Handler/SubirCircuitoImagen.ashx',
            debug: true,
            sizeLimit: 4194304,
            messages: {
                sizeError: "{file} el archivo es muy pesado. El peso máximo permitido es {sizeLimit}."
            },
            showMessage: function (message) {
                MostrarMensajeError("<span>*</span> " + message);
            },
            params: {
                pIdCircuito: IdCircuito,
                pName: $(".qqfile").val()
            },
            onSubmit: function (id, fileName) {
                $(".qq-upload-list").empty();
            },
            onComplete: function (id, filename, responseJSON) {
                if (responseJSON.success) {
                    $(".qq-upload-file").text(responseJSON.newFileName);
                    AgregarCircuitoImagen();
                }
                else {
                    ManejarArregloDeErrores(responseJSON.message);
                    $("#modalAgregarCircuitoImagen").modal("hide");
                }
            }
        });
    }
}

function AgregarCircuitoImagen() {
    IdCircuito = $("#modalAgregarCircuitoImagen .modal-dialog").attr('idCircuito');
    Imagen       = $("#modalAgregarCircuitoImagen .qq-upload-file").text();

    Ejecutar = true;
    errores = [];

    if (IdCircuito === 0) {
        Ejecutar = false;
        errores.push("Circuito es requerido.");
    }

    if (Imagen === "") {
        Ejecutar = false;
        errores.push("Logo es requerido.");
    }

    if (Ejecutar) {
        var Circuito = new Object();
        Circuito.IdCircuito = IdCircuito;
        Circuito.Imagen = Imagen;

        var Request = JSON.stringify(Circuito);
        WM("_Controls/Circuito.aspx/EditarCircuitoImagen", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarCircuitoImagen ").modal("hide");
            }
            else {
                Error("Agregar imagen", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaMeta(idCircuito) {
    var Circuito = new Object();
    Circuito.IdCircuito = idCircuito
    var pRequest = JSON.stringify(Circuito);
    ASPT(pRequest, "_Controls/Circuito.aspx/ObtenerFormaListarMetas", "_Views/tmplMetas.html", "#modalMetasTmpl", procesarListarMetas);
}

function procesarListarMetas(data) {
    respuesta = data.Datos.Circuito;
    if (data.Error == "") {
        modalName = 'modalMetas';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalMetasTmpl").processTemplate(data);
        $("#" + modalName).modal();
    }
    
    $("#" + modalName).on('shown.bs.modal', function () {
        $("#PaginadorMetas #btnAnteriorPagina").click(function () {
            var Pagina = parseInt($("#PaginadorMetas #txtPagina").val()) - 1;
            Pagina = (Pagina < 1) ? 1 : Pagina;
            $("#PaginadorMetas #txtPagina").val(Pagina).change();
        });

        $("#PaginadorMetas #btnSiguientePagina").click(function () {
            var Paginas = parseInt($("#PaginadorMetas #lblPaginas").text());
            var Pagina = parseInt($("#PaginadorMetas #txtPagina").val()) + 1;
            Pagina = (Pagina > Paginas) ? Paginas : Pagina;
            $("#PaginadorMetas #txtPagina").val(Pagina).change();
        });

        $("#PaginadorMetas #txtPagina").change(function () {
            ListarMetas(respuesta.IdCircuito);
        }).change();

        $("#btnObtenerFormaAgregarMeta").click(ObtenerFormaAgregarMeta);

        $('#txtFiltroMetaAnio').datetimepicker({
            format: "YYYY",
            maxDate: new Date(),
            stepping: 1,
            date: new Date(),
            locale: 'es'
        });
    });
        
}

function ListarMetas(IdCircuito) {
    var Metas = new Object();
    Metas.Pagina = parseInt($("#PaginadorMetas #txtPagina").val());
    Metas.Columna = "M.IdCircuito";
    Metas.Orden = "ASC";
    Metas.Mes = $("#cmbFiltroMetaMes").val();
    Metas.Anio = $("#txtFiltroMetaAnio").find('input', anio).val();
    Metas.IdCircuito = $("#modalMetas .modal-dialog").attr('idCircuito'); //IdCircuito;

    var Request = JSON.stringify(Metas);
    WM("_Controls/Circuito.aspx/ListarMetas", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorMetas #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorMetas #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Metas = json.Datos.Metas;
            $("tbody", "#tblListaMetas").html('');
            for (x in Metas) {
                var mta = Metas[x];
                var tr = $("<tr data-id=" + mta.IdMeta + "></tr>");
                $(tr)
                    .append($("<td class='text-center'>" + mta.Mes + "</td>"))
                    .append($("<td class='text-center'>" + mta.Anio + "</td>"))
                    .append($("<td class='text-right'>" + comaFormato(monedaFormato(mta.MetaKwH)) + "</td>"))                    
                    .append($("<td class='text-right'>$" + comaFormato(monedaFormato(mta.MetaConsumo)) + "</td>"))
                    .append($("<td class='text-center'>" + mta.MetaHorasUso + "</td>"))
                    .append($("<td class='text-center ku-clickable' id=\"btn-EditarMeta\"><span class='glyphicon glyphicon-edit'></span></td>"));
                $("tbody", "#tblListaMetas").append(tr);
            }
        }
        else {
            Error("Listar metas", json.Error);
        }
    });
}

function ObtenerFormaEditarMeta(IdMeta){	
	var Meta = new Object();
    Meta.IdMeta= IdMeta;
    var pRequest = JSON.stringify(Meta);
    ASPT(pRequest, "_Controls/Circuito.aspx/ObtenerFormaEditarMeta", "_Views/tmplEditarMeta.html", "#modalEditarMetaTmpl", procesarEditarMeta);
}

function procesarEditarMeta(data) {
    if (data.Error == "") {
        modalName = 'modalEditarMeta';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarMetaTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#btnEditarMeta").click(EditarMeta);
    }
}

function EditarMeta() {
    IdMeta = $("#modalEditarMeta .modal-dialog").attr('idMeta');
	IdCircuito = $("#modalEditarMeta .modal-dialog").attr('idCircuito');
    MetaKwH        = $("#txtMetaKwH", "#modalEditarMeta").val();
    MetaHorasUso   = $("#txtMetaHorasUso", "#modalEditarMeta").val();
    MetaConsumo    = $("#txtMetaConsumo", "#modalEditarMeta").val();

    Ejecutar = true;
    errores = [];    

    if (IdMeta === "0") {
        Ejecutar = false;
        errores.push("Meta es requerido.");
    }

    if (MetaKwH === "") {
        Ejecutar = false;
        errores.push("Meta horas uso es requerido.");
    }

    if (MetaHorasUso === "") {
        Ejecutar = false;
        errores.push("Meta horas uso es requerido.");
    }

    if (MetaConsumo === "") {
        Ejecutar = false;
        errores.push("Meta consumo es requerido");
    }    

    if (Ejecutar) {
        var Meta = new Object();
        Meta.IdMeta = IdMeta;
        Meta.MetaKwH = MetaKwH;
        Meta.MetaHorasUso = MetaHorasUso;
        Meta.MetaConsumo = MetaConsumo;

        var Request = JSON.stringify(Meta);
        WM("_Controls/Circuito.aspx/EditarMeta", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarMeta").modal("hide");
				ListarMetas(IdCircuito)
            }
            else {
                Error("Editar circuito", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaAgregarMeta() {
    IdCircuito = $("#modalMetas .modal-dialog").attr('idCircuito');
    var Meta = new Object();
    Meta.IdCircuito = IdCircuito
    var pRequest = JSON.stringify(Meta);
    ASPT(pRequest, "_Controls/Circuito.aspx/ObtenerFormaAgregarMeta", "_Views/tmplAgregarMeta.html", "#modalAgregarMetaTmpl", procesarAgregarMeta);
}

function procesarAgregarMeta(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarMeta';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarMetaTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $('#txtAnio', '#modalAgregarMeta').datetimepicker({
            format: "YYYY",
            stepping: 1,
            date: new Date(),
            locale: 'es'
        });

        $("#btnAgregarMeta").click(function () {
            AgregarMeta();
        });
    }
    else {
        Error("Formulario Meta ", json.Error);
    }
}

function AgregarMeta() {
    IdCircuito = $("#modalAgregarMeta .modal-dialog").attr('idCircuito');
    MetaKwH = $("#txtMetaKwH", "#modalAgregarMeta").val();
    MetaHorasUso = $("#txtMetaHorasUso", "#modalAgregarMeta").val();
    MetaConsumo = $("#txtMetaConsumo", "#modalAgregarMeta").val();
    Mes = $("#cmbMes", "#modalAgregarMeta").val();
    Anio = $("#txtAnio", "#modalAgregarMeta").val();

    Ejecutar = true;
    errores = [];

    if (Mes === "0") {
        Ejecutar = false;
        errores.push("Mes es requerido.");
    }

    if (Anio === "") {
        Ejecutar = false;
        errores.push("Año es requerido.");
    }

    if (MetaKwH === "") {
        Ejecutar = false;
        errores.push("Meta horas uso es requerido.");
    }

    if (MetaHorasUso === "") {
        Ejecutar = false;
        errores.push("Meta horas uso es requerido.");
    }

    if (MetaConsumo === "") {
        Ejecutar = false;
        errores.push("Meta consumo es requerido");
    }

    if (Ejecutar) {
        var Meta = new Object();
        Meta.IdCircuito = IdCircuito;
        Meta.MetaConsumo = MetaConsumo;
        Meta.MetaHorasUso = MetaHorasUso;
        Meta.MetaKwH = MetaKwH;
        Meta.Mes = Mes;
        Meta.Anio = Anio;

        var Request = JSON.stringify(Meta);
        WM("_Controls/Circuito.aspx/AgregarMeta", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarMeta").modal("hide");
                ListarMetas(IdCircuito);
            }
            else {
                Error("Agregar Meta", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
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
    WM("_Controls/Circuito.aspx/ObtenerFiltroCliente", Request, function (Respuesta) {
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

function ObtenerFiltroMedidores(IdSucursal) {
    var Medidor = new Object();
    Medidor.IdSucursal = IdSucursal;
    var Request = JSON.stringify(Medidor);
    WM("_Controls/Circuito.aspx/ObtenerMedidores", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbFiltroMedidor", json.Datos.Medidores);
    });
}

function ObtenerFiltroTableros(IdMedidor) {
    var Tablero = new Object();
    Tablero.IdMedidor = IdMedidor;
    var Request = JSON.stringify(Tablero);
    WM("_Controls/Circuito.aspx/ObtenerTableros", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbFiltroTablero", json.Datos.Tableros);
    });
}

function LimpiarFiltro() {
    $("#cmbFiltroCliente").val("0");
    LimpiarCombo("#cmbFiltroPais");
    LimpiarCombo("#cmbFiltroEstado");
    LimpiarCombo("#cmbFiltroMunicipio");
    LimpiarCombo("#cmbFiltroSucursal");
    LimpiarCombo("#cmbFiltroMedidor");
    LimpiarCombo("#cmbFiltroTablero");
    $("#txtFiltroCircuito").val("");
    $("#txtFiltroDescripcion").val("");
}

function LimpiarFiltroMeta() {
    $("#cmbFiltroMetaMes").val("0");
    $("#txtFiltroMetaAnio").find('input', anio).val('');
}
