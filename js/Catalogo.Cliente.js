/**/

$(function () {

    //ObtenerFiltroCliente();

    $("#PaginadorClientes #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorClientes #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorClientes #txtPagina").val(Pagina).change();
    });

    $("#PaginadorClientes #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorClientes #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorClientes #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorClientes #txtPagina").val(Pagina).change();
    });

    $("#PaginadorClientes #txtPagina").change(function () {
        ListarClientes();
    }).change();


    $("#btnObtenerFormaAgregarCliente").click(ObtenerFormaAgregarCliente);
});

/*CLIENTES*/

function ListarClientes() {
    var Clientes = new Object();
    Clientes.Pagina = parseInt($("#PaginadorClientes #txtPagina").val());
    Clientes.Columna = "Cliente";
    Clientes.Orden = "ASC";
    var Request = JSON.stringify(Clientes);
    WM("_Controls/Cliente.aspx/ListarClientes", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorClientes #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorClientes #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Clientes = json.Datos.Clientes;
            $("tbody", "#tblListaClientes").html('');
            for (x in Clientes) {
                var cte = Clientes[x];
                var tr = $("<tr></tr>");
                $(tr)
					.append($("<td>" + cte.Cliente + "</td>"))
                    .append($("<td class=\"text-center ku-clickable\" onclick=\"ObtenerFormaLogo(" + cte.IdCliente + ");\"><span class='glyphicon glyphicon-picture'></span></td>"))
                    .append($("<td class=\"text-center ku-clickable\" onclick=\"ObtenerFormaSucursales(" + cte.IdCliente + ");\"><span class='glyphicon glyphicon-home'></span></td>"))
                    .append($("<td class=\"text-center ku-clickable\"  onclick=\"ObtenerFormaEditarCliente(" + cte.IdCliente + ");\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td id='desactivarCliente' onclick=\"DesactivarCliente(" + cte.IdCliente + "," + cte.Baja + " );\" estatus=\"" + cte.Baja + "\" class=\"text-center ku-clickable hidden-xs hidden-sm\">" + cte.Estatus + "</td>"));
                $("tbody", "#tblListaClientes").append(tr);
            }
        }
        else {
            Error("Listar Clientes", json.Error);
        }
    });
}

function ObtenerFormaAgregarCliente() {
    TMPL("tmplAgregarCliente.html", function (Template) {
        $("body").append(Template);
        $("#modalAgregarCliente").modal();
        $("#modalAgregarCliente").on("hidden.bs.modal", function () { $("#modalAgregarCliente").remove(); ListarClientes(); });
        $("#btnAgregarCliente").click(AgregarCliente);
        ObtenerPaises();
    });
}

function ObtenerPaises() {
    var Pais = new Object();
    var Request = JSON.stringify(Pais);
    WM("_Controls/Cliente.aspx/ObtenerPaises", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbPais", json.Datos.Paises);
    });
    
    $("#cmbPais").change(function () {
        ObtenerEstados(parseInt($(this).val()));
    });
}

function ObtenerEstados(IdPais) {
    var Estado = new Object();
    Estado.IdPais = IdPais;
    var Request = JSON.stringify(Estado);
    WM("_Controls/Cliente.aspx/ObtenerEstados", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbEstado", json.Datos.Estados);
        LimpiarCombo("#cmbMunicipio");
    });

    $("#cmbEstado").change(function () {
        ObtenerMunicipios(parseInt($(this).val()));
    });
}

function ObtenerMunicipios(IdEstado) {
    var Municipio = new Object();
    Municipio.IdEstado = IdEstado;
    var Request = JSON.stringify(Municipio);
    WM("_Controls/Cliente.aspx/ObtenerMunicipios", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbMunicipio", json.Datos.Municipios);
    });

}
function AgregarCliente() {
    
    Nombre = $("#txtCliente", "#modalAgregarCliente").val();
    Municipio = $("#cmbMunicipio", "#modalAgregarCliente").val();
   
    Ejecutar = true;
    errores = [];

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Cliente es requerido.");
    }

    if (Municipio === 0) {
        Ejecutar = false;
        errores.push("Municipio es requerido.");
    }

    if (Ejecutar) {
        var Cliente = new Object();
        Cliente.Cliente = Nombre;
        Cliente.IdMunicipio = Municipio;

        var Request = JSON.stringify(Cliente);
        WM("_Controls/Cliente.aspx/AgregarCliente", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarCliente").modal("hide");
            }
            else {
                Error("Agregar cliente", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarCliente(IdCliente) {
    var Cliente = new Object();
    Cliente.IdCliente = IdCliente;
    FORM("formEditarCliente.aspx", Cliente, function (Forma) {
        $("body").append(Forma);
        $("#modalEditarCliente").modal();
        $("#modalEditarCliente").on("hidden.bs.modal", function () { $("#modalEditarCliente").remove(); ListarClientes(); });
        $("#btnEditarCliente").click(EditarCliente);

        $("#cmbPais").change(function () {
            ObtenerEstados(parseInt($(this).val()));
        });

        $("#cmbEstado").change(function () {
            ObtenerMunicipios(parseInt($(this).val()));
        });
    });
}

function EditarCliente() {
    IdCliente = $("#modalEditarCliente").attr('idCliente');
    Nombre = $("#txtCliente", "#modalEditarCliente").val();
    Municipio = $("#cmbMunicipio", "#modalEditarCliente").val();

    Ejecutar = true;
    errores = [];

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Cliente es requerido.");
    }

    if (Municipio === 0) {
        Ejecutar = false;
        errores.push("Municipio es requerido.");
    }

    if (Ejecutar) {
        var Cliente = new Object();
        Cliente.IdCliente = IdCliente;
        Cliente.Cliente = Nombre;
        Cliente.IdMunicipio = Municipio;

        var Request = JSON.stringify(Cliente);
        WM("_Controls/Cliente.aspx/EditarCliente", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarCliente").modal("hide");
            }
            else {
                Error("Editar cliente", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
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

/*END CLIENTES*/

/*
SUCURSALES
---------------------------------------------------------------------------------------------------------------------------------
*/
function ObtenerFormaSucursales(IdCliente) {
    TMPL("tmplSucursales.html", function (Template) {
        $("body").append(Template);
        $("#modalSucursales").modal();
        $("#modalSucursales").attr("idCliente", IdCliente)
        $("#modalSucursales").on("hidden.bs.modal", function () { $("#modalSucursales").remove(); /*ListarSucursales();*/ });

        $("#PaginadorSucursales #btnAnteriorPagina").click(function () {
            var Pagina = parseInt($("#PaginadorSucursales #txtPagina").val()) - 1;
            Pagina = (Pagina < 1) ? 1 : Pagina;
            $("#PaginadorSucursales #txtPagina").val(Pagina).change();
        });

        $("#PaginadorSucursales #btnSiguientePagina").click(function () {
            var Paginas = parseInt($("#PaginadorSucursales #lblPaginas").text());
            var Pagina = parseInt($("#PaginadorSucursales #txtPagina").val()) + 1;
            Pagina = (Pagina > Paginas) ? Paginas : Pagina;
            $("#PaginadorSucursales #txtPagina").val(Pagina).change();
        });

        $("#PaginadorSucursales #txtPagina").change(function () {
            ListarSucursales(IdCliente);
        }).change();

        $("#btnObtenerFormaAgregarSucursal").click(ObtenerFormaAgregarSucursal);

        ListarSucursales(IdCliente)
    });
}

function ListarSucursales(IdCliente) {
    var Sucursales = new Object();
    Sucursales.Pagina = parseInt($("#PaginadorSucursales #txtPagina").val());
    Sucursales.Columna = "Sucursal";
    Sucursales.Orden = "ASC";
    Sucursales.IdCliente = IdCliente;
    var Request = JSON.stringify(Sucursales);
    WM("_Controls/Sucursal.aspx/ListarSucursales", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorSucursales #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorSucursales #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Sucursales = json.Datos.Sucursales;
            $("tbody", "#tblListaSucursales").html('');
            for (x in Sucursales) {
                var suc = Sucursales[x];
                var tr = $("<tr class=\"ku-clickable\"></tr>");
                $(tr)
					.append($("<td onclick=\"ObtenerFormaEditarSucursal(" + suc.IdSucursal + ");\">" + suc.Sucursal + "</td>"))
                    .append($("<td onclick=\"ObtenerFormaEditarSucursal(" + suc.IdSucursal + ", " + suc.IdCliente + ");\" >" + suc.Municipio + "</td>"))
                    .append($("<td onclick=\"ObtenerFormaEditarSucursal(" + suc.IdSucursal + ", " + suc.IdCliente + ");\" >" + suc.Estado + "</td>"))
                    .append($("<td onclick=\"ObtenerFormaEditarSucursal(" + suc.IdSucursal + ", " + suc.IdCliente + ");\" class=\"hidden-xs hidden-sm\">" + suc.Pais + "</td>"))
                    .append($("<td onclick=\"ObtenerFormaEditarSucursal(" + suc.IdSucursal + ", " + suc.IdCliente + ");\" class=\"hidden-xs hidden-sm\">" + suc.TipoTarifa + "</td>"))
                    .append($("<td onclick=\"ObtenerFormaEditarSucursal(" + suc.IdSucursal + ", " + suc.IdCliente + ");\" class=\"hidden-xs hidden-sm\">" + suc.TipoTension + "</td>"))
                    .append($("<td onclick=\"ObtenerFormaEditarSucursal(" + suc.IdSucursal + ", " + suc.IdCliente + ");\" class=\"hidden-xs hidden-sm\">" + suc.TipoCuota + "</td>"))
                    .append($("<td onclick=\"ObtenerFormaEditarSucursal(" + suc.IdSucursal + ", " + suc.IdCliente + ");\" >" + suc.Region + "</td>"))
                    .append($("<td onclick=\"DesactivarSucursal(" + suc.IdSucursal + "," + suc.Baja + ");\" class=\"hidden-xs hidden-sm\">" + suc.Estatus + "</td>"));
                $("tbody", "#tblListaSucursales").append(tr);
            }
        }
        else {
            Error("Listar Sucursales", json.Error);
        }
    });
}

function ObtenerFormaAgregarSucursal() {
    IdCliente = $("#modalSucursales").attr('idCliente');
    TMPL("tmplAgregarSucursal.html", function (Template) {
        $("body").append(Template);
        $("#modalAgregarSucursal").modal();
        $("#modalAgregarSucursal").on("hidden.bs.modal", function () { $("#modalAgregarSucursal").remove(); ListarSucursales(IdCliente); });
        $("#btnAgregarSucursal").click(AgregarSucursal);
        ObtenerPaises();
        ObtenerTipoTarifa();
    });
}

function AgregarSucursal() {

    Nombre = $("#txtSucursal", "#modalAgregarSucursal").val();
    IdCliente = $("#modalSucursales").attr('idCliente');
    IdMunicipio = $("#cmbMunicipio").val();
    IdTipoTarifa = $("#cmbTipoTarifa").val();
    IdTipoTension = $("#cmbTipoTension").val();
    IdTipoCuota = $("#cmbTipoCuota").val();
    IdRegion = $("#cmbRegion").val();

    Ejecutar = true;
    errores = [];

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Sucursal es requerido.");
    }

    if (IdCliente === 0) {
        Ejecutar = false;
        errores.push("Cliente es requerido.");
    }

    if (IdMunicipio === 0) {
        Ejecutar = false;
        errores.push("Municipio es requerido.");
    }

    if (IdTipoTarifa === 0) {
        Ejecutar = false;
        errores.push("Tipo tarifa es requerido.");
    }

    if (IdTipoTension === 0) {
        Ejecutar = false;
        errores.push("Tipo tensión es requerido.");
    }

    if (IdTipoCuota === 0) {
        Ejecutar = false;
        errores.push("Tipo cuota es requerido.");
    }

    if (IdRegion === 0) {
        Ejecutar = false;
        errores.push("Región es requerido.");
    }

    if (Ejecutar) {
        var Sucursal = new Object();
        Sucursal.Sucursal = Nombre;
        Sucursal.IdCliente = IdCliente;
        Sucursal.IdMunicipio = IdMunicipio;
        Sucursal.IdRegion = IdRegion;

        var Request = JSON.stringify(Sucursal);
        WM("_Controls/Sucursal.aspx/AgregarSucursal", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarSucursal").modal("hide");
            }
            else {
                Error("Agregar Sucursal", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function DesactivarCliente(IdCliente, Baja) {
    

    Ejecutar = true;
    errores = [];

    if (IdCliente === 0) {
        Ejecutar = false;
        errores.push("Cliente es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Cliente = new Object();
        Cliente.IdCliente = IdCliente;
        Cliente.Baja = Baja;

        var Request = JSON.stringify(Cliente);
        WM("_Controls/Cliente.aspx/DesactivarCliente", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarClientes();
            }
            else {
                Error("Desactivar cliente", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function DesactivarSucursal(IdSucursal, Baja) {


    Ejecutar = true;
    errores = [];

    if (IdSucursal === 0) {
        Ejecutar = false;
        errores.push("Sucursal es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Sucursal = new Object();
        Sucursal.IdSucursal = IdSucursal;
        Sucursal.Baja = Baja;

        var Request = JSON.stringify(Sucursal);
        WM("_Controls/Sucursal.aspx/DesactivarSucursal", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                IdCliente = $("#modalSucursales").attr('idCliente');
                ListarSucursales(IdCliente);
            }
            else {
                Error("Desactivar sucursal", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }

}

function obtenerFormaEditarCliente2() {
    TMPL("tmplEditarCliente.html", function (Template) {
        $("body").append(Template);       
        $("#modalEditarCliente").on("hidden.bs.modal", function () { $("#modalEditarCliente").remove();  });
        $("#modalEditarCliente").click(AgregarSucursal);


        var Cliente = new Object();
        Cliente.IdCliente = 1;
        var pRequest = JSON.stringify(Cliente);
        ASPT(pRequest, "_Controls/Cliente.aspx/ObtenerPrueba", "_Views/tmplPruebas.html", "#clienteFormEditTmpl", processPrueba)
    });    
}

function processPrueba(thisContent) {
	$("#modalEditarCliente").setTemplateElement("clienteFormEditTmpl").processTemplate(thisContent);
	$("#modalEditarCliente").modal();
}

function ObtenerFormaEditarSucursal(IdSucursal) {
    IdCliente = $("#modalSucursales").attr('idCliente');
    var Sucursal = new Object();
    Sucursal.IdSucursal = IdSucursal;
    FORM("formEditarSucursal.aspx", Sucursal, function (Forma) {
        $("body").append(Forma);
        $("#modalEditarSucursal").modal();
        $("#modalEditarSucursal").on("hidden.bs.modal", function () { $("#modalEditarSucursal").remove(); ListarSucursales(IdCliente); });
        $("#btnEditarSucursal").click(EditarSucursal);

        $("#cmbPais").change(function () {
            ObtenerEstados(parseInt($(this).val()));
        });

        $("#cmbEstado").change(function () {
            ObtenerMunicipios(parseInt($(this).val()));
        });

        $("#cmbTipoTarifa").change(function () {
            ObtenerTipoTension(parseInt($(this).val()));
        });

        $("#cmbTipoTension").change(function () {
            ObtenerTipoCuota(parseInt($(this).val()));
        });

        $("#cmbTipoCuota").change(function () {
            ObtenerRegion(parseInt($(this).val()));
        });

        
    });
}

function EditarSucursal() {
    IdCliente = $("#modalSucursales").attr('idCliente');
    IdSucursal = parseInt($("#modalEditarSucursal").attr("IdSucursal"));
    Nombre = $("#txtSucursal", "#modalEditarSucursal").val();
    IdMunicipio = $("#cmbMunicipio", "#modalEditarSucursal").val();
    IdRegion = $("#cmbRegion", "#modalEditarSucursal").val();
    

    Ejecutar = true;
    errores = [];

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Nombre es requerido.");
    }

    if (IdMunicipio === "0") {
        Ejecutar = false;
        errores.push("Sucursal es requerido.");
    }

    if (IdRegion === "0") {
        Ejecutar = false;
        errores.push("Region es requerido.");
    }

    if (Ejecutar) {
        var Sucursal = new Object();
        Sucursal.IdSucursal = IdSucursal;
        Sucursal.Nombre = Nombre;
        Sucursal.IdMunicipio = IdMunicipio;
        Sucursal.IdRegion = IdRegion;

        var Request = JSON.stringify(Sucursal);
        WM("_Controls/Sucursal.aspx/EditarSucursal", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarSucursal").modal("hide");
                ListarSucursales(IdCliente);
            }
            else {
                Error("Editar sucursal", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerTipoTarifa() {
    var TipoTarifa = new Object();
    var Request = JSON.stringify(TipoTarifa);
    WM("_Controls/Cliente.aspx/ObtenerTipoTarifa", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbTipoTarifa", json.Datos.TipoTarifas);
    });

    $("#cmbTipoTarifa").change(function () {
        ObtenerTipoTension(parseInt($(this).val()));
    });
}

function ObtenerTipoTension(IdTipoTarifa) {
    var TipoTension = new Object();
    TipoTension.IdTipoTarifa = IdTipoTarifa;
    var Request = JSON.stringify(TipoTension);
    WM("_Controls/Cliente.aspx/ObtenerTipoTension", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbTipoTension", json.Datos.TipoTensiones);

        LimpiarCombo("#cmbTipoCuota");
        LimpiarCombo("#cmbRegion");
    });

    $("#cmbTipoTension").change(function (e) {       
        ObtenerTipoCuota(parseInt($(this).val()));
    });
}

function ObtenerTipoCuota(IdTipoTension) {
    var TipoCuota = new Object();
    TipoCuota.IdTipoTension = IdTipoTension;
    var Request = JSON.stringify(TipoCuota);
    WM("_Controls/Cliente.aspx/ObtenerTipoCuota", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbTipoCuota", json.Datos.TipoCuotas);
        LimpiarCombo("#cmbRegion");
    });

    $("#cmbTipoCuota").change(function (e) {
        ObtenerRegion(parseInt($(this).val()));
    });
}

function ObtenerRegion(IdTipoCuota) {
    var Region = new Object();
    Region.IdTipoCuota = IdTipoCuota;
    var Request = JSON.stringify(Region);
    WM("_Controls/Cliente.aspx/ObtenerRegion", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbRegion", json.Datos.Regiones);
    });
}

function ObtenerFormaLogo(IdCliente) {
    var Cliente =  new Object();
    Cliente.IdCliente = IdCliente;
    var Request = JSON.stringify(Cliente);
    WM("_Controls/Cliente.aspx/ObtenerFormaLogo", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            TMPL("tmplAgregarLogo.html", function (Template) {
                $("body").append(Template);
                $("#modalAgregarLogo").modal();
                $("#modalAgregarLogo").on("hidden.bs.modal", function () { $("#modalAgregarLogo").remove(); });
                $("#modalAgregarLogo").attr('idCliente',json.Datos.Cliente.IdCliente);
                $("#modalAgregarLogo #txtCliente").text(json.Datos.Cliente.Cliente);
                $("#modalAgregarLogo #txtLogo").attr("src", json.Datos.Cliente.URL);

                

                var uploader = new qq.FileUploader({
                    element: document.getElementById('file-uploader-imageLogo'),
                    multiple: false,
                    action: 'Handler/SubirImagen.ashx',
                    debug: true,
                    sizeLimit: 4194304,
                    messages: {
                        sizeError: "{file} el archivo es muy pesado. El peso máximo permitido es {sizeLimit}."
                    },
                    showMessage: function (message) {
                        MostrarMensajeError("<span>*</span> " + message);
                    },
                    params: {
                        pIdCliente: IdCliente,
                        pName: $(".qqfile").val()
                    },
                    onSubmit: function (id, fileName) {
                        $(".qq-upload-list").empty();
                    },
                    onComplete: function (id, filename, responseJSON) {
                        if (responseJSON.success) {
                            $(".qq-upload-file").text(responseJSON.newFileName);
                            AgregarLogo();
                        }
                        else {
                            ManejarArregloDeErrores(responseJSON.message);
                            $("#modalAgregarLogo").modal("hide");
                        }
                    }
                });
            });
            
        }
        else {
            Error("Subir Logo", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
        }
    });

    
}

function AgregarLogo() {
    IdCliente = $("#modalAgregarLogo").attr('idCliente');
    Logo      = $("#modalAgregarLogo .qq-upload-file").text();

    Ejecutar = true;
    errores = [];

    if (IdCliente === 0) {
        Ejecutar = false;
        errores.push("Cliente es requerido.");
    }

    if (Logo === "") {
        Ejecutar = false;
        errores.push("Logo es requerido.");
    }

    if (Ejecutar) {
        var Cliente = new Object();
        Cliente.IdCliente = IdCliente;
        Cliente.Logo = Logo;

        var Request = JSON.stringify(Cliente);
        WM("_Controls/Cliente.aspx/EditarLogo", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarLogo").modal("hide");
                location.reload();
            }
            else {
                Error("Agregar logo", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}
/*END SUCURSALES*/