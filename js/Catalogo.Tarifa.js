/**/

$(function () {

    $("#PaginadorTarifas #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorTarifas #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorTarifas #txtPagina").val(Pagina).change();
    });

    $("#PaginadorTarifas #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorTarifas #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorTarifas #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorTarifas #txtPagina").val(Pagina).change();
    });

    $("#PaginadorTarifas #txtPagina").change(function () {
        ListarTarifas();
    }).change();


    $("#btnObtenerFormaAgregarTarifa").click(ObtenerFormaAgregarTarifa);

    $('#txtAnio').datetimepicker({
    	format: "YYYY",
    	maxDate: new Date(),
    	stepping: 1,
    	date: new Date(),
    	locale: 'es'
    });

});

function ListarTarifas() {
    var Tarifas = new Object();
    Tarifas.Pagina = parseInt($("#PaginadorTarifas #txtPagina").val());
    Tarifas.Columna = "TC.TipoCuota,R.Region,T.Mes, T.Anio";
    Tarifas.Orden = "ASC";
    var Request = JSON.stringify(Tarifas);
    WM("_Controls/Tarifa.aspx/ListarTarifas", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorTarifas #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorTarifas #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Tarifas = json.Datos.Tarifas;
            $("tbody", "#tblListaTarifas").html('');
            for (x in Tarifas) {
                var tfa = Tarifas[x];
                var tr = $("<tr></tr>");
                $(tr)
                    
					.append($("<td>" + tfa.Fuente + "</td>"))
                    .append($("<td>" + tfa.TipoTarifa + "</td>"))
                    .append($("<td>" + tfa.TipoTension + "</td>"))
                    .append($("<td>" + tfa.TipoCuota + "</td>"))
					.append($("<td>" + tfa.Region + "</td>"))
					.append($("<td>" + tfa.Mes + "</td>"))
					.append($("<td>" + tfa.Anio + "</td>"))
                    .append($("<td style='text-align:right;'>$" + tfa.ConsumoBaja + "</td>"))
                    .append($("<td style='text-align:right;'>$" + tfa.ConsumoMedia + "</td>"))
                    .append($("<td style='text-align:right;'>$" + tfa.ConsumoAlta + "</td>"))
                    .append($("<td style='text-align:right;'>$" + tfa.Demanda + "</td>"))
                    .append($("<td class=\"ku-clickable\" onclick=\"ObtenerFormaEditarTarifa(" + tfa.IdTarifa + ");\"><span class='glyphicon glyphicon-edit'></span></td>"))
					.append($("<td id='desactivarTarifa' onclick=\"DesactivarTarifa(" + tfa.IdTarifa + "," + tfa.Baja + " );\" estatus=\"" + tfa.Baja + "\" class=\"hidden-xs hidden-sm\">" + tfa.Estatus + "</td>"));
                $("tbody", "#tblListaTarifas").append(tr);
            }
            
        }
        else {
            Error("Listar Tarifas", json.Error);
        }
    });
}

function ObtenerFormaAgregarTarifa() {
    TMPL("tmplAgregarTarifa.html", function (Template) {
        $("body").append(Template);
        $("#modalAgregarTarifa").modal();
        $("#modalAgregarTarifa").on("hidden.bs.modal", function () { $("#modalAgregarTarifa").remove(); ListarTarifas(); });
        $("#btnAgregarTarifa").click(AgregarTarifa);
        ObtenerFuente();
        ObtenerTipoTarifa();

        $('#txtAnio').datetimepicker({
        	format: "YYYY",
        	maxDate: new Date(),
        	stepping: 1,
        	date: new Date(),
        	locale: 'es'
        });

    });
}

function AgregarTarifa() {
	IdFuente = $("#cmbFuente", "#modalAgregarTarifa").val();
    IdRegion     = $("#cmbRegion", "#modalAgregarTarifa").val();
    Mes = parseInt($("#cmbMes", "#modalAgregarTarifa").val());
    Anio = parseInt($("#txtAnio", "#modalAgregarTarifa").val());
    ConsumoBaja  = $("#txtConsumoBaja", "#modalAgregarTarifa").val();
    ConsumoAlta  = $("#txtConsumoAlta", "#modalAgregarTarifa").val();
    ConsumoMedia = $("#txtConsumoMedia", "#modalAgregarTarifa").val();
    Demanda      = $("#txtDemanda", "#modalAgregarTarifa").val();

    Ejecutar = true;
    errores = [];

    var fechaActual = new Date();
    mesActual = (fechaActual.getMonth() + 1);
    anioActual = fechaActual.getFullYear();

    if (IdFuente == 0) {
    	Ejecutar = false;
    	errores.push("Fuente es requerida.");
    }

    if (IdRegion == 0) {
        Ejecutar = false;
        errores.push("Region es requerida.");
    }

    if (Mes == 0) {
    	Ejecutar = false;
    	errores.push("Mes es requerido.");
    }

    if (Anio == 0) {
    	Ejecutar = false;
    	errores.push("Año es requerido.");
    }

    if (ConsumoBaja === 0) {
        Ejecutar = false;
        errores.push("Consumo baja es requerida.");
    }
    if (ConsumoMedia === 0) {
        Ejecutar = false;
        errores.push("Consumo media es requerida.");
    }
    if (ConsumoAlta === 0) {
        Ejecutar = false;
        errores.push("Consumo alta es requerida.");
    }
    if (Demanda === 0) {
        Ejecutar = false;
        errores.push("Demanda es requerida.");
    }

    if (Anio > anioActual) {
        Ejecutar = false;
        errores.push("El año no puede ser mayor al actual.");
    }
    else
    {
        if (Anio == anioActual) {
            if (Mes > 0) {
                if (Mes > mesActual) {
                    Ejecutar = false;
                    errores.push("El mes y año no puede ser mayor al actual");
                }
            }
        }
    }
    

    if (Ejecutar) {
    	var Tarifa = new Object();
    	Tarifa.IdFuente = IdFuente;
        Tarifa.IdRegion = IdRegion;
        Tarifa.Mes = Mes;
        Tarifa.Anio = Anio;
        Tarifa.ConsumoBaja = validaNumero(ConsumoBaja) ? ConsumoBaja : 0;
        Tarifa.ConsumoMedia = validaNumero(ConsumoMedia) ? ConsumoMedia : 0;
        Tarifa.ConsumoAlta = validaNumero(ConsumoAlta) ? ConsumoAlta : 0;
        Tarifa.Demanda = validaNumero(Demanda) ? Demanda : 0;

        var Request = JSON.stringify(Tarifa);
        WM("_Controls/Tarifa.aspx/AgregarTarifa", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalAgregarTarifa").modal("hide");
            }
            else {
                Error("Agregar Tarifa", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerFormaEditarTarifa(IdTarifa) {
    var Tarifa = new Object();
    Tarifa.IdTarifa = IdTarifa;
    FORM("formEditarTarifa.aspx", Tarifa, function (Forma) {
        $("body").append(Forma);
        $("#modalEditarTarifa").modal();
        $("#modalEditarTarifa").on("hidden.bs.modal", function () { $("#modalEditarTarifa").remove(); ListarTarifas(); });
        $("#btnEditarTarifa").click(EditarTarifa);

        $('#txtAnio').datetimepicker({
        	format: "YYYY",
        	maxDate: new Date(),
        	stepping: 1,
        	date: new Date(),
        	locale: 'es'
        });
    });
}

function EditarTarifa() {
	IdTarifa = $("#modalEditarTarifa").attr('idTarifa');
	IdFuente = $("#cmbFuente", "#modalEditarTarifa").val();
	IdRegion = $("#cmbRegion", "#modalEditarTarifa").val();
	Nombre = $("#txtTarifa", "#modalEditarTarifa").val();
	ConsumoBaja = $("#txtConsumoBaja", "#modalEditarTarifa").val();
	ConsumoMedia = $("#txtConsumoMedia", "#modalEditarTarifa").val();
	ConsumoAlta = $("#txtConsumoAlta", "#modalEditarTarifa").val();
	Demanda = $("#txtDemanda", "#modalEditarTarifa").val();
    Mes = $("#cmbMes", "#modalEditarTarifa").val();
    Anio = $("#txtAnio", "#modalEditarTarifa").val();

    Ejecutar = true;
    errores = [];

    var fechaActual = new Date();
    mesActual = (fechaActual.getMonth() + 1);
    anioActual = fechaActual.getFullYear(); 

    if (Nombre === "") {
        Ejecutar = false;
        errores.push("Tarifa es requerido.");
    }

    if (IdFuente === 0) {
    	Ejecutar = false;
    	errores.push("Fuente es requerido.");
    }

    if (IdRegion === 0) {
        Ejecutar = false;
        errores.push("Region es requerido.");
    }

    if (Anio > anioActual) {
    	Ejecutar = false;
    	errores.push("El año no puede ser mayor al actual.");
    }
    else {
    	if (Anio == anioActual) {
    		if (Mes > 0) {
    			if (Mes > mesActual) {
    				Ejecutar = false;
    				errores.push("El mes y año no puede ser mayor al actual");
    			}
    		}
    	}
    }

    if (Ejecutar) {
        var Tarifa = new Object();
        Tarifa.IdTarifa = IdTarifa;
        Tarifa.Tarifa = Nombre;
        Tarifa.IdFuente = IdFuente;
        Tarifa.IdRegion = IdRegion;
        Tarifa.ConsumoBaja = ConsumoBaja;
        Tarifa.ConsumoMedia = ConsumoMedia;
        Tarifa.ConsumoAlta = ConsumoAlta;
        Tarifa.Demanda = Demanda;
        Tarifa.Mes = Mes;
        Tarifa.Anio = Anio;

        var Request = JSON.stringify(Tarifa);
        WM("_Controls/Tarifa.aspx/EditarTarifa", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarTarifa").modal("hide");
            }
            else {
                Error("Editar Tarifa", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function DesactivarTarifa(IdTarifa, Baja) {


    Ejecutar = true;
    errores = [];

    if (IdTarifa === 0) {
        Ejecutar = false;
        errores.push("Tarifa es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Tarifa = new Object();
        Tarifa.IdTarifa = IdTarifa;
        Tarifa.Baja = Baja;

        var Request = JSON.stringify(Tarifa);
        WM("_Controls/Tarifa.aspx/DesactivarTarifa", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarTarifas();
            }
            else {
                Error("Desactivar tarifa", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
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
    WM("_Controls/Tarifa.aspx/ObtenerTipoTarifa", Request, function (Respuesta) {
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
    WM("_Controls/Tarifa.aspx/ObtenerTipoTension", Request, function (Respuesta) {
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
    WM("_Controls/Tarifa.aspx/ObtenerTipoCuota", Request, function (Respuesta) {
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
    WM("_Controls/Tarifa.aspx/ObtenerRegion", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbRegion", json.Datos.Regiones);
    });
}

function ObtenerFuente() {
	var Fuente = new Object();
	var Request = JSON.stringify(Fuente);
	WM("_Controls/Tarifa.aspx/ObtenerFuente", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		COMBO("#cmbFuente", json.Datos.TipoFuentes);
	});
}