/**/

$(function () {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    // Inicializacion de combo de cliente
    ObtenerClientes();

    // Inicializacion de componente de fecha y hora
    $('#txtFechaInicio').datetimepicker({
        format: "DD/MM/YYYY HH:mm",
        maxDate: new Date(),
        stepping: 1,
        date: new Date(),
        locale: 'es'
    });

    $('#txtFechaFin').datetimepicker({
        format: "DD/MM/YYYY HH:mm",
        maxDate: new Date(),
        stepping: 1,
        date: new Date(),
        locale: 'es'
    });

    var fecha = new Date();
    var primerDiaMes = new Date(fecha.getFullYear(), fecha.getMonth(), 1);
    primerDiaMesPasado = new Date(primerDiaMes.setMonth(primerDiaMes.getMonth() - 1));
    ultimoDiaMesPasado = new Date(primerDiaMesPasado.getFullYear(), primerDiaMesPasado.getMonth() + 1, 0);


    var Inicio = primerDiaMesPasado;
    diaInicio = Inicio.getDate();
    if (diaInicio < 10) {
        diaInicio = '0' + diaInicio;
    }

    mes = (Inicio.getMonth() + 1);
    if (mes < 10) {
        mes = '0' + mes;
    }

    //Fin
    var Fin = ultimoDiaMesPasado;
    diaFin = Fin.getDate();
    if (diaFin < 10) {
        diaFin = '0' + diaFin;
    }

    finicio = diaInicio + '/' + mes + '/' + Inicio.getFullYear() + ' 00:00';
    ffin = diaFin + '/' + mes + '/' + Fin.getFullYear() + ' 23:59';

    $("#txtFechaInicio").find('input', inicio).val(finicio);
    $("#txtFechaFin").find('input', fin).val(ffin);
    
    // Incializacion de combos para prellenado filtrado
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
        ObtenerTableros(parseInt($(this).val()));
    });

    $("#cmbTablero").change(function () {
        ObtenerCircuitos(parseInt($(this).val()));
    });

    $("#tipoConsumoKWH").click(function () {
        tipoConsumo = $(this).attr('tipoConsumo');
        ObtenerDetalleConsumo(tipoConsumo);
    });

    $("#tipoConsumoImporte").click(function () {
        tipoConsumo = $(this).attr('tipoConsumo');
        ObtenerDetalleConsumo(tipoConsumo);
    });

    $("#tipoConsumoHoras").click(function () {
        tipoConsumo = $(this).attr('tipoConsumo');
        ObtenerDetalleConsumo(tipoConsumo);
    });

    $("#btnActualizar").click(function () {
        ObtenerDetalle();
    });

});

function ObtenerClientes() {
    var Medicion = new Object();
    var Request = JSON.stringify(Medicion);
    WM("_Controls/Medicion.aspx/ObtenerClientes", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbCliente", json.Datos.Clientes);
    });
}

function ObtenerPaises(IdCliente) {
    var Cliente = new Object();
    Cliente.IdCliente = IdCliente;
    var Request = JSON.stringify(Cliente);
    WM("_Controls/Medicion.aspx/ObtenerPaises", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbPais", json.Datos.Paises);
    });
}

function ObtenerEstados(IdCliente, IdPais) {
    var Estado = new Object();
    Estado.IdCliente = IdCliente;
    Estado.IdPais = IdPais;
    var Request = JSON.stringify(Estado);
    WM("_Controls/Medicion.aspx/ObtenerEstados", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbEstado", json.Datos.Estados);
    });
}

function ObtenerMunicipios(IdCliente, IdPais, IdEstado) {
    var Municipio = new Object();
    Municipio.IdCliente = IdCliente;
    Municipio.IdPais = IdPais;
    Municipio.IdEstado = IdEstado;
    var Request = JSON.stringify(Municipio);
    WM("_Controls/Medicion.aspx/ObtenerMunicipios", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbMunicipio", json.Datos.Municipios);
    });

}

function ObtenerSucursales(IdCliente, IdMunicipio) {
    var Medicion = new Object();
    Medicion.IdCliente = IdCliente;
    Medicion.IdMunicipio = IdMunicipio;
    var Request = JSON.stringify(Medicion);
    WM("_Controls/Medicion.aspx/ObtenerSucursales", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbSucursal", json.Datos.Sucursales);
    });
}

function ObtenerTableros(IdSucursal) {
    var Medicion = new Object();
    Medicion.IdSucursal = IdSucursal;
    var Request = JSON.stringify(Medicion);
    WM("_Controls/Medicion.aspx/ObtenerTableros", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbTablero", json.Datos.Tableros);
    });
}

function ObtenerCircuitos(IdTablero) {
    var Medicion = new Object();
    Medicion.IdTablero = IdTablero;
    var Request = JSON.stringify(Medicion);
    WM("_Controls/Medicion.aspx/ObtenerCircuitos", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbCircuito", json.Datos.Circuitos);
    });
}
function ObtenerDetalle() {
    IdCliente = parseInt($("#cmbCliente").val());
    IdPais = parseInt($("#cmbPais").val());
    IdEstado = parseInt($("#cmbEstado").val());
    IdMunicipio = parseInt($("#cmbMunicipio").val());
    IdSucursal = parseInt($("#cmbSucursal").val());
    IdTablero = parseInt($("#cmbTablero").val());
    IdCircuito = parseInt($("#cmbCircuito").val());    
    IdRango = parseInt($("#cmbRango").val());
    FechaInicio = $("#txtFechaInicio").find('input', inicio).val();
    FechaFin = $("#txtFechaFin").find('input', fin).val();

    Ejecutar = true;
    errores = [];

    if (IdCliente === 0) {
        Ejecutar = false;
        errores.push("Favor de seleccionar un cliente.");
    }

    if (IdRango === 0) {
        Ejecutar = false;
        errores.push("Favor de seleccionar el estatus.");
    }

    if (FechaInicio === "") {
        Ejecutar = false;
        errores.push("Favor de seleccionar una fecha inicio.");
    }

    if (FechaFin === "") {
        Ejecutar = false;
        errores.push("Favor de seleccionar una fecha fin.");
    }

    if (Ejecutar) {
        var Medicion = new Object();
        Medicion.IdCliente = IdCliente;
        Medicion.IdPais = IdPais;
        Medicion.IdEstado = IdEstado;
        Medicion.IdMunicipio = IdMunicipio;
        Medicion.IdSucursal = IdSucursal;
        Medicion.IdTablero = IdTablero;
        Medicion.IdCircuito = IdCircuito;
        Medicion.IdRango = IdRango;
        Medicion.FechaInicio = FechaInicio;
        Medicion.FechaFin = FechaFin;

        var Request = JSON.stringify(Medicion);
        WM("_Controls/ReporteFueraDeRango.aspx/ObtenerDetalleUno", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            FILLTABLE($("tbody", "#tblDetalle"), json.Datos.Detalle);
            $("tr", $("tbody", "#tblDetalle")).each(function (index, element) {
                $("td:eq(5)", element).css("text-align", "center");
                $("td:eq(6)", element).css("text-align", "center");
                $("td:eq(7)", element).css("text-align", "center");
                $("td:eq(8)", element).css("text-align", "center");
                $("td:eq(9)", element).css("text-align", "center");
                $("td:eq(10)", element).css("text-align", "center");
                $("td:eq(11)", element).css("text-align", "center");

                Tablero =  $("td:eq(6)", element).html();
                Circuito = $("td:eq(7)", element).html();

                $("td:eq(10)", element).html("<span onclick='verCircuitoImagen(" + Circuito + ");' class='glyphicon glyphicon-search'></span>");
                $("td:eq(11)", element).html("<span onclick='verMantenimiento(" + Circuito + ");' class='glyphicon glyphicon-search'></span>");


            });

            for (x in json.Datos.Contadores) {
                var Registro = json.Datos.Contadores[x];
                $("#divConsumoKWH").html(Registro.DiferenciaKwH);
                $("#divConsumoHoras").html(Registro.DiferenciaHorasUso);
                $("#divConsumoImporte").html(Registro.DiferenciaConsumo);
               
            }

            //ObtenerTotales(Request);

        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}


function ObtenerDetalleConsumo(tipoConsumo) {
    IdCliente = parseInt($("#cmbCliente").val());
    IdPais = parseInt($("#cmbPais").val());
    IdEstado = parseInt($("#cmbEstado").val());
    IdMunicipio = parseInt($("#cmbMunicipio").val());
    IdSucursal  = parseInt($("#cmbSucursal").val());
    IdTablero   = parseInt($("#cmbTablero").val());
    IdCircuito  = parseInt($("#cmbCircuito").val());    
    IdRango       = parseInt($("#cmbRango").val());
    IdTipoConsumo = tipoConsumo;
    FechaInicio = $("#txtFechaInicio").find('input', inicio).val()
    FechaFin = $("#txtFechaFin").find('input', fin).val()

    Ejecutar = true;
    errores = [];

    if (IdCliente === 0) {
        Ejecutar = false;
        errores.push("Favor de seleccionar un cliente.");
    }

    if (IdRango === 0) {
        Ejecutar = false;
        errores.push("Favor de seleccionar el estatus.");
    }

    if (FechaInicio === "") {
        Ejecutar = false;
        errores.push("Favor de seleccionar una fecha inicio.");
    }

    if (FechaFin === "") {
        Ejecutar = false;
        errores.push("Favor de seleccionar una fecha fin.");
    }

    if (Ejecutar) {
        var Medicion = new Object();
        Medicion.IdCliente = IdCliente;
        Medicion.IdPais = IdPais;
        Medicion.IdEstado = IdEstado;
        Medicion.IdMunicipio = IdMunicipio;
        Medicion.IdSucursal = IdSucursal;
        Medicion.IdTablero = IdTablero;
        Medicion.IdCircuito = IdCircuito;
        Medicion.IdRango = IdRango;
        Medicion.IdTipoConsumo = IdTipoConsumo;
        Medicion.FechaInicio = FechaInicio;
        Medicion.FechaFin = FechaFin;

        var Request = JSON.stringify(Medicion);
        WM("_Controls/ReporteFueraDeRango.aspx/ObtenerDetalleConsumo", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            FILLTABLEREPORTE($("tbody", "#tblDetalle"), json.Datos.Detalle);
            $("tr", $("tbody", "#tblDetalle")).each(function (index, element) {
                $("td:eq(5)", element).css("text-align", "center");
                $("td:eq(6)", element).css("text-align", "center");
                $("td:eq(7)", element).css("text-align", "center");
                $("td:eq(8)", element).css("text-align", "center");
                $("td:eq(9)", element).css("text-align", "center");
                $("td:eq(10)", element).css("text-align", "center");
                $("td:eq(11)", element).css("text-align", "center");

                Tablero = $("td:eq(6)", element).html();
                Circuito = $("td:eq(7)", element).html();

                $("td:eq(10)", element).html("<span onclick='verCircuitoImagen(" + Circuito + ");' class='glyphicon glyphicon-search'></span>");
                $("td:eq(11)", element).html("<span onclick='verMantenimiento(" + Circuito + ");' class='glyphicon glyphicon-search'></span>");
            });
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerTotales(Respuesta) {
    WM("_Controls/ReporteFueraDeRango.aspx/ObtenerTotales", Respuesta, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        for (x in json.Datos.Detalle) {
            var Registro = json.Datos.Detalle[x];
            $("#divConsumoKWH").html(Registro.DiferenciaKwH);
            $("#divConsumoImporte").html(Registro.DiferenciaConsumo);
            $("#divConsumoHoras").html(Registro.DiferenciaHorasUso);
        }
        
    });
}

function FILLTABLEREPORTE(Selector, Datos) {
    $(Selector).html('');
    for (Renglon in Datos) {
        var Fila = $("<tr></tr>");
        for (y in Datos[x]) {
            $(Fila).append($("<td>" + Datos[Renglon][y] + "</td>"));
        }
        $(Selector).append(Fila);
    }
}

function verCircuitoImagen(param) {
    var Circuito = new Object();
    Circuito.IdCircuito=param
    var pRequest = JSON.stringify(Circuito);
    ASPT(pRequest, "_Controls/ReporteFueraDeRango.aspx/ObtenerModalMostrarCircuitoImagen", "_Views/tmplMostrarCircuitoImagen.html", "#modalMostrarCircuitoImagenTmpl", procesarMostrarCircuitoImagen);
}
function procesarMostrarCircuitoImagen(data) {
    if (data.Error == "") {
        modalName = 'modalMostrarCircuitoImagen';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalMostrarCircuitoImagenTmpl").processTemplate(data);
       // $('.imagepreview').attr('src', data.Datos.Circuito.URL);
        $("#" + modalName).modal();
    }
}

function verTablero2(param) {
    TMPL("tmplVerImagen.html", function (Template) {
        $("body").append(Template);

        $("#modalVerImagen").on("hidden.bs.modal", function () { $("#modalVerImagen").remove(); });
        $('.imagepreview').attr('src', "http://localhost:8080/Medidor/Archivos/circuito1.jpg");
        $('#imagemodal').modal('show');
        $("#modalVerImagen").modal();
    });
}


function verMantenimiento(param) {
    TMPL("tmplVerImagen.html", function (Template) {
        $("body").append(Template);

        $("#modalVerImagen").on("hidden.bs.modal", function () { $("#modalVerImagen").remove(); });


        $('.imagepreview').attr('src', "http://localhost:8080/Medidor/Archivos/mantenimientoCircuito1.jpg");
        $('#imagemodal').modal('show');
        $("#modalVerImagen").modal();


    });
}