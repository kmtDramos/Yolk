$(document).on('hidden.bs.modal', function () {
    $('body').addClass('modal-open');
});
/**/
function WM(WebMethod, Request, Callback) {
	$.ajax({
		url: WebMethod,
		type: "POST",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: Callback
	});
}

function FORM(FormView, Request, Callback) {
	$.ajax({
		url: "_Views/" + FormView,
		type: "get",
		data: Request,
		contentType: "application/json; charset=utf-8",
		success: Callback
	});
}

function CREARMODAL(modalName) {
    html = '<div id=' + modalName + ' class="modal fade" role="dialog">';
    $("body").append(html);
    $("#" + modalName).on("hidden.bs.modal", function () { $("#" + modalName).remove(); });
}

function TMPL(Template, Callback) {
	$.ajax({
		url: "_Views/" + Template,
		success: Callback
	});
}

function Error(Title,Message){
	$.ajax({
		url: "_Views/tmplModal.html",
		success: function (Respuesta) {
			Respuesta = Respuesta.replace("[TITLE]", Title);
			Respuesta = Respuesta.replace("[MESSAGE]", Message);
			var mdl = $(Respuesta);
			$("body").append(mdl);
			$(mdl).modal();
			$(mdl).on("hidden.bs.modal", function () { $(mdl).remove() });
		}
	});
}

function CerrarSesion() {
	$.ajax({
		url: "_Controls/Usuario.aspx/CerrarSesion",
		type: "POST",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error != "") {
				Error("Cerrar sesión", json.Error);
			}
			else {
				window.location.reload();
			}
		}
	});
}

function LIST(WebMethod, Request, Selector, Callback) {
	$.ajax({
		url: "_Cantrols/" + WebMethod,
		type: "POST",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == "")
			{

			}
			else
			{
				Error("Error", json.Error);
			}
		}
	});
}


/**/
function COMBO(Selector, Datos) {
	$(Selector).html('');
	$(Selector).append($("<option value=\"0\">-Seleccionar-</option>"));
	for (x in Datos) {
		$(Selector).append("<option value=\"" + Datos[x].Valor + "\">" + Datos[x].Etiqueta + "</option>");
	}
}

function FILLTABLE(Selector, Datos) {
	$(Selector).html('');
	for (x in Datos) {
	    var Fila = $("<tr></tr>");
		for (y in Datos[x]) {
			$(Fila).append($("<td>"+Datos[x][y]+"</td>"));
		}
		$(Selector).append(Fila);
	}
}

function ManejarArregloDeErrores(estosErrores) {
    var listaErrores = "<ol>"
    $.each(estosErrores, function(index, value) {
        listaErrores = listaErrores + "<li>" + value + "</li>";
    });
    listaErrores = listaErrores + "</ol>"
    Error("",listaErrores);
}

function ASPT(thisData, thisLocation, thisTemplate, thisTemplateId,thisProcess){
    $.ajax({
      type: "POST",
      url: thisLocation,
      data: thisData,
      dataType: "json",
      contentType: "application/json; charset=utf-8",
      beforeSend: function(){
        increaseProcessBuffer();
      },
      success: function (pRespuesta) {
          //var json = JSON.parse(Respuesta.d);
          propiedades = jQuery.parseJSON(pRespuesta.d);
          if (propiedades.Error === "") {
              if (!($(thisTemplateId).length)) {
                  $.ajax({
                      type: 'Get',
                      url: thisTemplate,
                      cache: false,
                      beforeSend: function(){
                        increaseProcessBuffer();
                      },
                      success: function(data) {
                          $("#onTheFlyTemplates").append(data);
                          thisProcess(propiedades);                                                            
                      },
                      complete: function() {
                          decreaseProcessBuffer();
                      }
                  });
              }
              else {
                 thisProcess(propiedades);                        
              }
          } else {
              Error("Alerta", propiedades.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
          }
      },
      error: function() {
      },
      complete: function() {
          decreaseProcessBuffer();
      }
  });
}


function ASP(thisData, thisLocation, thisSuccess) {
    $.ajax({
        type: "POST",
        url: thisLocation,
        data: thisData,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            increaseProcessBuffer();
        },
        success: thisSuccess,
        error: function () {
        },
        complete: function () {
            decreaseProcessBuffer();
        }
    });
}

var inProcess = 0;

function revaluateBlock() {
    if (inProcess >= 1) {
        $(".hiddenOverlay").removeClass('noDisplay');
    } else {
        $(".hiddenOverlay").addClass('noDisplay');
    }
}

function decreaseProcessBuffer() {
    inProcess = inProcess - 1;
    revaluateBlock();
}

function increaseProcessBuffer() {
    inProcess = inProcess + 1;
    revaluateBlock();
}

function MantenerSesion(pRaiz) {
    if (pRaiz == "" || pRaiz == null)
    { var url = "_Controls/Usuario.aspx/MantenerSesion"; }
    else
    { var url = "_Controls/Usuario.aspx/MantenerSesion"; }
    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            //MostrarMensajeError(pRespuesta.d);
        }
    });
}

function validaNumero(number) {
    return $.isNumeric(number);
}

function validate(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var selector = "input[name='" + theEvent.currentTarget.name + "']";

    if (key === 46 && $(selector).val().split('.').length === 4) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.srcElement;
        return false;
    }

    var selectorMsj = "#_" + theEvent.currentTarget.name;
    $(selectorMsj).css("display", "none");
    $(selector).parent().removeClass("has-error");
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.srcElement;
        $(selectorMsj).css("display", "inline");
        //$(this).addClass("has-error")
        $(selector).parent().addClass("has-error");
    }
}

function clean(evt) {
    var theEvent = evt || window.event;
    var selector = "input[name='" + theEvent.currentTarget.name + "']";
    if ($(selector).val() == "0") {
        $(selector).val("0.0000");
        $(selector).attr("value", "0.0000");
    }
    var selectorMsj = "#_" + theEvent.currentTarget.name;
    $(selectorMsj).css("display", "none");
    $(selector).parent().removeClass("has-error");


}

function LimpiarCombo (thisNameId) {
    $(thisNameId).children('option:not(:first)').remove();
}

function monedaFormato(dato) {
    var i = parseFloat(dato), minus = '';
    if (isNaN(i)) {
        i = 0.00;
    }
    if (i < 0) {
        minus = '-';
    }
    i = Math.abs(i);
    i = parseInt((i + 0.005) * 100, 10);
    i = i / 100;
    s = new String(i);
    if (s.indexOf('.') < 0) {
        s += '.00';
    }
    if (s.indexOf('.') === (s.length - 2)) {
        s += '0';
    }
    s = minus + s;
    return s;
}


function comaFormato(dato) {
    var delimiter = ",", a = dato.split('.', 2), d = a[1], i = parseInt(a[0], 10), n = new String(i), c = [], minus = '', nn;
    if (isNaN(i)) {
        return '';
    }
    if (i < 0) {
        minus = '-';
    }
    i = Math.abs(i);
    while (n.length > 3) {
        nn = n.substr(n.length - 3);
        c.unshift(nn);
        n = n.substr(0, n.length - 3);
    }
    if (n.length > 0) {
        c.unshift(n);
    }
    n = c.join(delimiter);
    if (d.length < 1) {
        dato = n;
    } else {
        dato = n + '.' + d;
    }
    dato = minus + dato;
    return dato;
}

function DataTableLanguageJO (idioma)
{
  if(idioma == 1)
  {
    return  { "sProcessing": "Procesando...", "sLengthMenu": "Mostrar _MENU_ registros", "sZeroRecords": "No se encontraron resultados", "sEmptyTable": "Ning&uacute;n dato disponible en esta tabla", "sInfo": "Registros del _START_ al _END_ de _TOTAL_ registros", "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros", "sInfoFiltered": "(filtrado de un total de _MAX_ registros)", "sInfoPostFix": "", "sSearch": "Buscar:", "sUrl": "", "sInfoThousands": ",", "sLoadingRecords": "Cargando...", "oPaginate": { "sFirst": "Primero", "sLast": "Último", "sNext": "Siguiente", "sPrevious": "Anterior" }, "oAria": { "sSortAscending": ": Activar para ordenar la columna de manera ascendente", "sSortDescending": ": Activar para ordenar la columna de manera descendente" } };
  }
  else
  {
    return    {
    "sEmptyTable": "Nenhum registro encontrado",
    "sInfo": "Mostrando de _START_ at&eacute; _END_ de _TOTAL_ registros",
    "sInfoEmpty": "Mostrando 0 at&eacute; 0 de 0 registros",
    "sInfoFiltered": "(Filtrados de _MAX_ registros)",
    "sInfoPostFix": "",
    "sInfoThousands": ".",
    "sLengthMenu": "Mostra _MENU_ resultados por p&aacute;gina",
    "sLoadingRecords": "Carregando...",
    "sProcessing": "Processando...",
    "sZeroRecords": "Nenhum registro encontrado",
    "sSearch": "Pesquisar",
    "oPaginate": {
        "sNext": "Pr&oacute;ximo",
        "sPrevious": "Anterior",
        "sFirst": "Primeiro",
        "sLast": "Último"
    },
    "oAria": {
        "sSortAscending": ": Ordenar colunas de forma ascendente",
        "sSortDescending": ": Ordenar colunas de forma descendente"
    }
    }
  }
}

function cambiarCliente() {
    var Cliente = new Object();
    var pRequest = JSON.stringify(Cliente);
    ASPT(pRequest, "_Controls/Usuario.aspx/ObtenerFormaClienteAsignadoUsuario", "_Views/tmplClienteAsignadoUsuario.html", "#modalClienteAsignadoUsuarioTmpl", procesarClienteAsignadoUsuario);
}

function procesarClienteAsignadoUsuario(data) {
    if (data.Error == "") {
        modalName = 'modalClienteAsignadoUsuario';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalClienteAsignadoUsuarioTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#" + modalName).on('shown.bs.modal', function () {

            $("#cmbClientePredeterminado").change(function () {
                ObtenerPaisesPredeterminado(parseInt($(this).val()));
            });

            $("#modalClienteAsignadoUsuario #cmbPaisPredeterminado").change(function () {
                IdCliente = parseInt($("#modalClienteAsignadoUsuario #cmbClientePredeterminado").val());
                ObtenerEstadosPredeterminado(IdCliente, parseInt($(this).val()));
            });

            $("#modalClienteAsignadoUsuario  #cmbEstadoPredeterminado").change(function () {
                IdCliente = parseInt($("#modalClienteAsignadoUsuario #cmbClientePredeterminado").val());
                IdPais = parseInt($("#modalClienteAsignadoUsuario #cmbPaisPredeterminado").val());
                ObtenerMunicipiosPredeterminado(IdCliente, IdPais, parseInt($(this).val()));
            });

            $("#modalClienteAsignadoUsuario  #cmbMunicipioPredeterminado").change(function () {
                IdCliente = parseInt($("#modalClienteAsignadoUsuario #cmbClientePredeterminado").val());
                ObtenerSucursalesPredeterminado(IdCliente, parseInt($(this).val()));
            });

            $("#btnEditarClientePredeterminado").click(function () {
                var Usuario = new Object();
                Usuario.pIdClientePredeterminado = parseInt($(this).val());
                EditarClientePredeterminado(JSON.stringify(Usuario));
            });
        });        
    }
    else {
        Error("Forma alta circuito ", json.Error);
    }
}

function EditarClientePredeterminado(pIdCliente) {
    IdSucursalPredeterminadaActual = $("#modalClienteAsignadoUsuario .modal-dialog").attr('idSucursalPredeterminada');
    IdSucursalPredeterminada = $("#modalClienteAsignadoUsuario #cmbSucursalPredeterminado").val();


    Ejecutar = true;
    errores = [];

    if (IdSucursalPredeterminadaActual == IdSucursalPredeterminada) {
        Ejecutar = false;
        errores.push("La sucursal seleccionada es la misma que la predeterminada");
    }

    if (Ejecutar) {
        var Usuario = new Object();
        Usuario.IdSucursalPredeterminada = IdSucursalPredeterminada;

        var Request = JSON.stringify(Usuario);
        WM("_Controls/Usuario.aspx/EditarClientePredeterminado", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalClienteAsignadoUsuario").modal("hide");
                location.reload();
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

function ObtenerPaisesPredeterminado(IdCliente) {
    var Cliente = new Object();
    Cliente.IdCliente = IdCliente;
    var Request = JSON.stringify(Cliente);
    WM("_Controls/Circuito.aspx/ObtenerPaises", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#modalClienteAsignadoUsuario #cmbPaisPredeterminado", json.Datos.Paises);
        LimpiarCombo("#modalClienteAsignadoUsuario #cmbEstadoPredeterminado");
        LimpiarCombo("#modalClienteAsignadoUsuario #cmbMunicipioPredeterminado");
        LimpiarCombo("#modalClienteAsignadoUsuario #cmbSucursalPredeterminado");
    });
}

function ObtenerEstadosPredeterminado(IdCliente, IdPais) {
    var Estado = new Object();
    Estado.IdCliente = IdCliente;
    Estado.IdPais = IdPais;
    var Request = JSON.stringify(Estado);
    WM("_Controls/Circuito.aspx/ObtenerEstados", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#modalClienteAsignadoUsuario #cmbEstadoPredeterminado", json.Datos.Estados);
        LimpiarCombo("#modalClienteAsignadoUsuario #cmbSucursalPredeterminado");
    });
}

function ObtenerMunicipiosPredeterminado(IdCliente, IdPais, IdEstado) {
    var Municipio = new Object();
    Municipio.IdCliente = IdCliente;
    Municipio.IdPais = IdPais;
    Municipio.IdEstado = IdEstado;
    var Request = JSON.stringify(Municipio);
    WM("_Controls/Circuito.aspx/ObtenerMunicipios", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#modalClienteAsignadoUsuario #cmbMunicipioPredeterminado", json.Datos.Municipios);
        //LimpiarCombo("#modalClienteAsignadoUsuario #cmbSucursalPredeterminado");
    });

}

function ObtenerSucursalesPredeterminado(IdCliente, IdMunicipio) {
    var Medicion = new Object();
    Medicion.IdCliente = IdCliente;
    Medicion.IdMunicipio = IdMunicipio;
    var Request = JSON.stringify(Medicion);
    WM("_Controls/Circuito.aspx/ObtenerSucursales", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#modalClienteAsignadoUsuario #cmbSucursalPredeterminado", json.Datos.Sucursales);
    });
}


/**/
$.ajaxSetup({ 
	beforeSend: function (req) {
		this.loader = $('<div class="shade"><div class="loader"></div></div>');
		$("body").append(this.loader);
	},
	error: function(){
		$(this.loader).remove();
	},
	complete: function () {
		$(this.loader).remove();
	}
});


