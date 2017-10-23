$(function () {
    $("#btnObtenerFormaAgregarCliente").click(ObtenerFormaPrueba);
});

function ObtenerFormaPrueba() {

    TMPL("tmplEditarCliente.html", function (Template) {
        $("body").append(Template);
        $("#modalEditarCliente").on("hidden.bs.modal", function () { $("#modalEditarCliente").remove();});
        $("#modalEditarCliente").click(AgregarSucursal);
        $("#modalEditarCliente").modal();

        var Cliente = new Object();
        Cliente.IdCliente = 1;
        var pRequest = JSON.stringify(Cliente)
        ASP(pRequest, "_Controls/Cliente.aspx/ObtenerPrueba", processPrueba);
    });

    $("#pepito").obtenerVista({
        nombreTemplate: "tmplConsultarPrueba.html",
        url: "_Controls/Cliente.aspx/ObtenerPrueba",
        parametros: 1,
        despuesDeCompilar: function (pRespuesta) {
          $("#dialogConsultarBanco").dialog("open");
        }
    });
}

//-----Plugin Obtener Vistas
jQuery.fn.obtenerVista = function (pOpciones) {
    MostrarBloqueo();
    var contenedor = $(this);
    var vista = {
        nombreTemplate: "",
        url: "",
        parametros: {},
        esDialog: false,
        remplazarVista: true,
        modelo: {},
        antesDeCompilar: function (pVista) { },
        despuesDeCompilar: function (pVista) { },
    }
    $.extend(vista, pOpciones);
    if (vista.nombreTemplate == "") {
        MostrarMensajeError("<p>Favor de completar los siguientes requisitos:</p><span>*</span> No se definió el nombre del template.");
    }
    else if (vista.url != "" && vista.nombreTemplate != "") {
        $.ajax({
            type: "POST",
            url: vista.url,
            data: vista.parametros,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (pRespuesta) {
                var respuesta = jQuery.parseJSON(pRespuesta.d);
                if (respuesta.Error)
                { MostrarMensajeError(respuesta.Descripcion); OcultarBloqueo(); }
                else {
                    vista.modelo = respuesta.Modelo;
                    var existeTemplate = false;
                    $.each(jqTemplates, function (pIndice, pTemplate) {
                        if (pTemplate.nombre == vista.nombreTemplate) {
                            vista.template = pTemplate.template;
                            existeTemplate = true;
                        }
                        return (pTemplate.nombre != vista.nombre);
                    });

                    if (!existeTemplate) {
                        $.ajax({
                            type: 'Get',
                            url: "../Templates/" + vista.nombreTemplate,
                            cache: false,
                            success: function (data) {
                                var oTemplate = new Object();
                                oTemplate.nombre = vista.nombreTemplate;
                                oTemplate.template = data;
                                //jqTemplates.push(oTemplate);
                                vista.template = oTemplate.template;
                                vista.antesDeCompilar(vista);
                                if (vista.esDialog) {
                                    contenedor.dialog('option', "title", respuesta.Titulo);
                                    contenedor.dialog('option', "width", respuesta.Ancho);
                                    contenedor.dialog('option', "height", respuesta.Alto);
                                }
                                ProcesarTemplate(vista, contenedor);
                                vista.despuesDeCompilar(vista);
                            }
                        });
                    }
                    else {
                        vista.antesDeCompilar(vista);
                        if (vista.esDialog) {
                            contenedor.dialog('option', "title", respuesta.Titulo);
                            contenedor.dialog('option', "width", respuesta.Ancho);
                            contenedor.dialog('option', "height", respuesta.Alto);
                        }
                        ProcesarTemplate(vista, contenedor);
                        vista.despuesDeCompilar(vista);
                    }
                }
            },
            error: function () {
                //OcultarBloqueo();
            },
            complete: function () {
            }
        });
    }
    else if (vista.url == "" && vista.nombreTemplate != "") {
        var existeTemplate = false;
        $.each(jqTemplates, function (pIndice, pTemplate) {
            if (pTemplate.nombre == vista.nombreTemplate) {
                vista.template = pTemplate.template;
                existeTemplate = true;
            }
            return (pTemplate.nombre == vista.nombre);
        });

        if (!existeTemplate) {
            $.ajax({
                type: 'Get',
                url: "../Templates/" + vista.nombreTemplate,
                cache: false,
                success: function (data) {
                    var oTemplate = new Object();
                    oTemplate.nombre = vista.nombreTemplate;
                    oTemplate.template = data;
                    //jqTemplates.push(oTemplate);
                    vista.template = oTemplate.template;
                    vista.antesDeCompilar(vista);
                    ProcesarTemplate(vista, contenedor)
                    vista.despuesDeCompilar(vista);
                },
                error: function () {
                    //OcultarBloqueo();
                }
            });
        }
        else {
            vista.antesDeCompilar(vista);
            ProcesarTemplate(vista, contenedor);
            vista.despuesDeCompilar(vista);
        }
    }
}

function ProcesarTemplate(pVista, pContenedor) {
    if (pVista.remplazarVista == true) {
        switch (pVista.efecto) {
            case "slide":
                if (pContenedor.html().length) {
                    pContenedor.children().slideUp('slow', function () {
                        pContenedor.empty();
                        $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                        pContenedor.children().slideDown('slow');
                    });
                }
                else {
                    pContenedor.empty();
                    $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                    pContenedor.children().slideDown('slow');
                }
                break;
            case "fade":
                if (pContenedor.html().length) {
                    pContenedor.children().fadeOut('slow', function () {
                        pContenedor.empty();
                        $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                        pContenedor.children().fadeIn('slow');
                    });
                }
                else {
                    pContenedor.empty();
                    $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                    pContenedor.children().fadeIn('slow');
                }
                break;
            default:
                pContenedor.empty();
                $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                break;
        }
    }
    else {
        switch (pVista.efecto) {
            case "slide":
                $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                pContenedor.children().slideDown('slow');
                break;
            case "fade":
                $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                pContenedor.children().fadeIn('slow');
                break;
            default:
                $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                break;
        }
    }

}