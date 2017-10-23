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
        ObtenerDetalle();
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

	$("#cmbCircuito").change(function () {
		ObtenerDetalle();
	});

	// Actualizacion de la información
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

function ObtenerDetalle() {
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
	    Medicion.Pagina = 1//parseInt($("#PaginadorMediciones #txtPagina").val());
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
		WM("_Controls/ReporteMetaVsReal.aspx/ObtenerDetalle", Request, function (Respuesta) {
		    var json = JSON.parse(Respuesta.d);
		    if (json.Error == "") {
		        if (json.Datos.length > 0) {

		             $("#divGrafica").empty();
		            $.each(json.Datos, function (i, value) {
		               
						Anio=value.Anio;

		                var Categorias = new Array();
		                var Real = new Array();
		                var Meta = new Array();
		                $.each(value.Registros, function (index, valor) {

		                    Categorias.push(valor.Mes);
		                    Real.push(valor.DatoReal);
		                    Meta.push(valor.Meta);
		                });

		                for (a in Meta) {
		                    Meta[a] = parseFloat(Meta[a], 10);
		                }

		                for (a in Real) {
		                    Real[a] = parseFloat(Real[a], 10);
		                }

		                GraficaPorMes(i,Anio,Categorias, Real, Meta);

		            });

		            
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

function GraficaPorMes(i, Anio, Categorias, Real, Meta) {
    //Crea Div
    var divGrafica = '';
    divGrafica = "divGrafica-" + i

    //si ya existe el div de la grafica se limpia
    if ($("#" + divGrafica).length > 0) {
        $("#" + divGrafica).empty();
    } else {
        $("#divGrafica").append('<div id="' + divGrafica + '" style="min-width: 310px; height: 400px; margin: 0 auto" ></td>');
    }

    Highcharts.chart(divGrafica, {
        title: {
            text: Anio
        },
        xAxis: {
            categories: Categorias
        },
        labels: {
            items: [{
                html: 'KWH',
                style: {
                    left: '50px',
                    top: '18px',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                }
            }]
        },
        series:
        [
            {
                type: 'column',
                name: 'Real',
                data: Real
            },
            {
                type: 'spline',
                name: 'Meta',
                data: Meta,
                marker: {
                    lineWidth: 2,
                    lineColor: Highcharts.getOptions().colors[3],
                    fillColor: 'white'
                }
            }
        ]
    });
}