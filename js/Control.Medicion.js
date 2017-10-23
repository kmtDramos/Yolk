/**/

$(function () {
    setInterval(MantenerSesion, 150000); //2.5 minutos


    $("#PaginadorMediciones #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorMediciones #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorMediciones #txtPagina").val(Pagina).change();
    });

    $("#PaginadorMediciones #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorMediciones #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorMediciones #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorMediciones #txtPagina").val(Pagina).change();
    });

    $("#PaginadorMediciones #txtPagina").change(function () {
        ObtenerDetalle(0);
    }).change();


	// Inicializacion de combo de cliente
	ObtenerClientes();
    
    // Inicializacion de componente de fecha y hora
	$('#txtFechaInicio').datetimepicker({
		format: "DD/MM/YYYY HH:mm",
		maxDate: new Date(),
		stepping: 1,
		locale: 'es'
	});

	$('#txtFechaFin').datetimepicker({
		format: "DD/MM/YYYY HH:mm",
		maxDate: new Date(),
		stepping: 1,
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
	    ObtenerMunicipios(IdCliente,IdPais, parseInt($(this).val()));
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

	// Actualizacion de la información
	$("#btnActualizar").click(function () {
		ObtenerDetalle(1);
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
    WM("_Controls/Cliente.aspx/ObtenerMunicipios", Request, function (Respuesta) {
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

function ObtenerDetalle(actualizaGrafica) {
	IdCliente = parseInt($("#cmbCliente").val());
	IdPais = parseInt($("#cmbPais").val());
	IdEstado = parseInt($("#cmbEstado").val());
	IdMunicipio = parseInt($("#cmbMunicipio").val());
	IdSucursal = parseInt($("#cmbSucursal").val());
	IdTablero = parseInt($("#cmbTablero").val());
	IdCircuito = parseInt($("#cmbCircuito").val());
	FechaInicio = $("#txtFechaInicio").find('input', inicio).val();
	FechaFin = $("#txtFechaFin").find('input', fin).val();
		
	Ejecutar = true;
    errores = [];
	
	if (IdCliente === 0) {
        Ejecutar = false;
        errores.push("Favor de seleccionar un cliente.");
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
	    Medicion.Pagina = parseInt($("#PaginadorMediciones #txtPagina").val());
	    Medicion.Columna = "Fecha";
	    Medicion.Orden = "ASC";
		Medicion.IdCliente = IdCliente;
		Medicion.IdPais = IdPais;
		Medicion.IdEstado = IdEstado;
		Medicion.IdMunicipio = IdMunicipio;
		Medicion.IdSucursal = IdSucursal;
		Medicion.IdTablero = IdTablero;
		Medicion.IdCircuito = IdCircuito;
		Medicion.FechaInicio = FechaInicio;
		Medicion.FechaFin = FechaFin;		

		var Request = JSON.stringify(Medicion);
		WM("_Controls/Medicion.aspx/ObtenerDetalle", Request, function (Respuesta) {
		    var json = JSON.parse(Respuesta.d);
		    if (json.Error == "") {
		        if (json.Datos.Detalle.length > 0) {

		            $("#PaginadorMediciones #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
		            $("#PaginadorMediciones #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

		            FILLTABLE($("tbody", "#tblDetalle"), json.Datos.Detalle);

		            //$("tr", $("tbody", "#tblDetalle")).each(function (index, element) {
		            //    $("td:eq(5)", element).css("text-align", "center");
		            //    $("td:eq(6)", element).css("text-align", "center");
		            //    $("td:eq(7)", element).css("text-align", "center");
		            //    $("td:eq(8)", element).css("text-align", "center");
		            //    $("td:eq(9)", element).css({ "text-align": "center" });
		            //    $("td:eq(10)", element).css({ "text-align": "center" });
		            //    $("td:eq(11)", element).css({ "text-align": "center" });
		            //    $("td:eq(12)", element).css({ "text-align": "center" });
		            //    $("td:eq(13)", element).css({ "text-align": "center" });
		            //    $("td:eq(14)", element).css({ "text-align": "center" });
		            //    $("td:eq(15)", element).css({ "text-align": "center" });
		            //    $("td:eq(16)", element).css({ "text-align": "center" });
		            //    $("td:eq(17)", element).css({ "text-align": "center" });
		            //    $("td:eq(18)", element).css({ "text-align": "center" });

		            //    $("td:eq(16)", element).html('$' + $("td:eq(16)", element).html());
		            //    $("td:eq(17)", element).html('$' + $("td:eq(17)", element).html());
		            //    $("td:eq(18)", element).html('$' + $("td:eq(18)", element).html());
		            //});

		            if (actualizaGrafica == 1) {
		                GeneraGraficas(json.Datos.Consumo, json.Datos.Real);
		            }
		        }
		        else {
		            Error("Reporte Medición", "Sin datos para mostrar");
		        }

		    }		
		    else {
                Error("Reporte Medición", json.Error);
	        }

		});
	}
	else {
        ManejarArregloDeErrores(errores);
    }
}

function GeneraGraficas(TotalMeta, Real) {
    IdCircuito = parseInt($("#cmbCircuito").val());
	var KwH = [];
	var Consumo = [];
	var Horas = [];

	var objKwH = { Etiqueta: "KwH", Meta: 0, Real: 0, Diferencia: 0 };
	var objConsumo = { Etiqueta: "Importe", Meta: 0, Real: 0, Diferencia: 0 };
	var objHoras = { Etiqueta: "Horas", Meta: 0, Real: 0, Diferencia: 0 };

    var KwHMeta = 0;
	var KwHReal = 0;
	var KwHDiferencia = 0;

	var importeMeta = 0;
	var importeReal = 0;
	var importeDiferencia = 0;

	var horaMeta = 0;
	var horaReal = 0;
	var horaDiferencia = 0;

	//for (x in Detalle) {
	//    var Registro = Detalle[x];

	//    KwHReal = parseFloat(parseFloat(KwHReal) + parseFloat(Registro.RealKwH)).toFixed(6);

	//    importeReal = parseFloat(parseFloat(importeReal) + parseFloat(Registro.RealConsumo)).toFixed(6);

	//    horaReal = parseFloat(parseFloat(horaReal) + parseFloat(Registro.RealHorasUso)).toFixed(6);
		
	//}
	if (Real.length == 0) {
	    KwHReal = 0;
	    importeReal = 0;
	    horaReal = 0;
	}
	else {
	    KwHReal = Real[0].RealKWH;
	    importeReal = Real[0].RealConsumo;
	    horaReal = Real[0].RealHorasUso;
	}

	if (TotalMeta.length == 0) {
	    totalKwH = 0;
	    totalImporte = 0;
	    totalHorasUso = 0;
	}
	else {
	    totalKwH = TotalMeta[0].MetaKwH;
	    totalImporte = TotalMeta[0].MetaImporte;
	    totalHorasUso = TotalMeta[0].MetaHorasUso;
	}
    
	KwHMeta = totalKwH;
	importeMeta = totalImporte;
	horaMeta = totalHorasUso;

	objKwH.Meta = KwHMeta;
	objKwH.Real = parseFloat(KwHReal).toFixed(6);

	objConsumo.Meta = importeMeta;
	objConsumo.Real = parseFloat(importeReal).toFixed(6);

	objHoras.Meta = horaMeta;
	objHoras.Real = parseFloat(horaReal).toFixed(6);

	KwH.push(objKwH);
	Horas.push(objHoras);
	Consumo.push(objConsumo);

	GraficaKwH(KwH);
	GraficaConsumo(Consumo);
	GraficaHoras(Horas);
}

function GraficaKwH(data) {
	$("#gfcKwH").html('');
	$("#legend").html('');
	
	var chart=  Morris.Bar({
		element: 'gfcKwH',
		data : data,
		xkey : "Etiqueta",
		ykeys : ["Meta", "Real"],
		labels : ["Meta", "Real"],
		hideHover: 'auto',
		resize: true,      
		smooth:true,
		barSize: 30,
		lineColors: ['blue', 'grey']    
	});
	
	chart.options.labels.forEach(function(label, i){
		var legendItem = $('<span></span>').text(label['label']).prepend('<i><p style="padding-left: 18px;">'+label+'</p></i>');
		legendItem.find('i').css('backgroundColor', chart.options.lineColors[i]);
		$('#legend').append(legendItem)
	})
}

function GraficaConsumo(data) {
	$("#gfcConsumo").html('');
	var config = new Object();
	config.element = "gfcConsumo";
	config.data = data;
	config.xkey = "Etiqueta";
	config.ykeys = ["Meta", "Real"];
	config.labels = ["Meta", "Real"];
	config.hideHover = "auto";
	config.resize = true;
	config.barSize = 30;
	config.gridUnderlineColor = '#fff000';
	config.gridUnderlineDecoration = 'underline';
	Morris.Bar(config);
}

function GraficaHoras(data) {
	$("#gfcHoras").html('');
	var config = new Object();
	config.element = "gfcHoras";
	config.data = data;
	config.xkey = "Etiqueta";
	config.ykeys = ["Meta", "Real"];
	config.labels = ["Meta", "Real"];
	config.hideHover = "auto";
	config.resize = true;
	config.barSize = 30;
	Morris.Bar(config);
}