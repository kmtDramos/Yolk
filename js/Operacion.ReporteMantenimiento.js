/**/

$(function ()
{
    $("#PaginadorReporteMantenimientos #btnAnteriorPagina").click(function () {
        var Pagina = parseInt($("#PaginadorReporteMantenimientos #txtPagina").val()) - 1;
        Pagina = (Pagina < 1) ? 1 : Pagina;
        $("#PaginadorReporteMantenimientos #txtPagina").val(Pagina).change();
    });

    $("#PaginadorReporteMantenimientos #btnSiguientePagina").click(function () {
        var Paginas = parseInt($("#PaginadorReporteMantenimientos #lblPaginas").text());
        var Pagina = parseInt($("#PaginadorReporteMantenimientos #txtPagina").val()) + 1;
        Pagina = (Pagina > Paginas) ? Paginas : Pagina;
        $("#PaginadorReporteMantenimientos #txtPagina").val(Pagina).change();
    });

    $("#PaginadorReporteMantenimientos #txtPagina").change(function () {
        ListarReporteMantenimientos();
    }).change();
    
    $('body').on('click', "#btnConsultarReporte", function (e) {
        e.preventDefault();
        var row = $(this).parents('tr');
        var id = row.data('id');

        ObtenerFormaConsultarReporte(id);
    });

    $('body').on('click', "#btnAgregarBitacora", function (e) {
        e.preventDefault();
        var row = $(this).parents('tr');
        var id = row.data('id');

        AgregarBitacora(id);
    });

    $('body').on('click', "#editarSeguimiento", function (e) {
        e.preventDefault(); 
        var Reporte = new Object();
        Reporte.IdReporte = $("#modalConsultarReporteMantenimiento .modal-dialog").attr('idReporte');
        var pRequest = JSON.stringify(Reporte);
        ObtenerFormaEditarSeguimiento(pRequest);
    });

    $('body').on('click', "#imgConsultarDocumento", function (e) {
        e.preventDefault();
        var Reporte = new Object();
        Reporte.IdReporte = $("#modalConsultarReporteMantenimiento .modal-dialog").attr('idReporte');
        var pRequest = JSON.stringify(Reporte);
        ObtenerFormaConsultarDocumento(pRequest);
    });

    $('body').on('click', "#eliminarDocumento", function (e) {
        e.preventDefault();
        var Reporte = new Object();
        Reporte.pIdReporteDocumento = $(this).attr('Documento');
        var pRequest = JSON.stringify(Reporte);
        EliminarDocumento(pRequest, Reporte.pIdReporteDocumento);
    });

    $('body').on('click', "#descargarDocumento", function (e) {
        e.preventDefault();
        Documento = $(this).attr('Documento');
        window.open("Handler/DescargarDocumento.ashx?Doc=" + Documento, "_blank");
    });

    $('body').on('click', "#btnConsultarEntrega", function (e) {
        e.preventDefault();
        var Reporte = new Object();
        Reporte.IdReporte = $("#modalConsultarReporteMantenimiento .modal-dialog").attr('idReporte');
        var pRequest = JSON.stringify(Reporte);
        EntregarReporte(pRequest);
    });
});

function ListarReporteMantenimientos() {
    var Reporte = new Object();
    Reporte.Pagina = parseInt($("#PaginadorReporteMantenimientos #txtPagina").val());
    Reporte.Columna = "R.FechaLevantamiento";
    Reporte.Orden = "DESC";
    Reporte.IdEstatus = "-1";
    var Request = JSON.stringify(Reporte);
    WM("_Controls/Operacion.ReporteMantenimiento.aspx/ListarReporteMantenimientos", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorReporteMantenimientos #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorReporteMantenimientos #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Reporte = json.Datos.Reportes;
            $("tbody", "#tblListaReporteMantenimientos").html('');
            for (x in Reporte) {
                var rep = Reporte[x];
                var tr = $("<tr data-id=" + rep.IdReporte + "></tr>");
                $(tr)
                    .append($("<td>" + rep.Estatus + "</td>"))
                    .append($("<td>" + rep.Folio + "</td>"))
                    .append($("<td>" + rep.FechaLevantamiento + "</td>"))
                    .append($("<td>" + rep.Pais + "</td>"))
                    .append($("<td>" + rep.Estado + "</td>"))
                    .append($("<td>" + rep.Municipio + "</td>"))
                    .append($("<td>" + rep.Sucursal + "</td>"))
                    .append($("<td>" + rep.Medidor + "</td>"))
                    .append($("<td>" + rep.Tablero + "</td>"))
                    .append($("<td>" + rep.Circuito + "</td>"))
                    .append($("<td>" + rep.DescripcionCircuito + "</td>"))
                    .append($("<td>" + rep.TipoConsumo + "</td>"))
                    .append($("<td>" + rep.Responsable + "</td>"))
                .append($("<td class=\"ku-clickable text-center\" id=\"btnConsultarReporte\"><span class='glyphicon glyphicon-edit'></span></td>"));
					
                $("tbody", "#tblListaReporteMantenimientos").append(tr);
            }
        }
        else {
            Error("Listar Reporte", json.Error);
        }
    });
}

function ObtenerFormaConsultarReporte(id) {
    var Reporte = new Object();
    Reporte.IdReporte = id;
    var pRequest = JSON.stringify(Reporte);
    ASPT(pRequest, "_Controls/Operacion.ReporteMantenimiento.aspx/ObtenerFormaConsultarReporte", "_Views/tmplConsultarReporteMantenimiento.html", "#modalConsultarReporteMantenimientoTmpl", procesarConsultarReporte);
}

function procesarConsultarReporte(data) {
    if (data.Error == "") {
        modalName = 'modalConsultarReporteMantenimiento';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalConsultarReporteMantenimientoTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#" + modalName).on('shown.bs.modal', function () {

            $("#PaginadorBitacora #btnAnteriorPagina").click(function () {
                var Pagina = parseInt($("#PaginadorBitacora #txtPagina").val()) - 1;
                Pagina = (Pagina < 1) ? 1 : Pagina;
                $("#PaginadorBitacora #txtPagina").val(Pagina).change();
            });

            $("#PaginadorBitacora #btnSiguientePagina").click(function () {
                var Paginas = parseInt($("#PaginadorBitacora #lblPaginas").text());
                var Pagina = parseInt($("#PaginadorBitacora #txtPagina").val()) + 1;
                Pagina = (Pagina > Paginas) ? Paginas : Pagina;
                $("#PaginadorBitacora #txtPagina").val(Pagina).change();
            });

            $("#PaginadorBitacora #txtPagina").change(function () {
                ListarBitacora();
            }).change();

            ListarBitacora();

        });
    }
}


function ListarBitacora() {
    var Bitacora = new Object();
    Bitacora.Pagina = parseInt($("#PaginadorBitacora #txtPagina").val());
    Bitacora.Columna = "B.Fecha";
    Bitacora.Orden = "DESC";
    Bitacora.IdReporte = $("#modalConsultarReporteMantenimiento .modal-dialog").attr('idReporte');
    var Request = JSON.stringify(Bitacora);
    WM("_Controls/Operacion.ReporteMantenimiento.aspx/ListarBitacoras", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorBitacora #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorBitacora #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var Bitacoras = json.Datos.Bitacora;
            $("tbody", "#tblListaBitacora").html('');
            for (x in Bitacoras) {
                var bta = Bitacoras[x];
                var tr = $("<tr data-id=" + bta.IdBitacora + "></tr>");
                $(tr)
                    .append($("<td>" + bta.Fecha + "</td>"))
                    .append($("<td>" + bta.Hora + "</td>"))
                    .append($("<td>" + bta.Emisor + "</td>"))
                    .append($("<td>" + bta.Bitacora + "</td>"))                    
                $("tbody", "#tblListaBitacora").append(tr);
            }
        }
        else {
            Error("Listar Circuitos", json.Error);
        }
    });
}

function ObtenerProblemas(IdTipoProblema) {
    var Problema = new Object();
    Problema.IdTipoProblema = IdTipoProblema;
    var Request = JSON.stringify(Problema);
    WM("_Controls/Operacion.ReporteMantenimiento.aspx/ObtenerProblemas", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbProblema", json.Datos.Problemas);
    });
}

function ObtenerUsuarioProveedores(IdProveedor) {
    var UsuarioProveedor = new Object();
    UsuarioProveedor.IdProveedor = IdProveedor;
    var Request = JSON.stringify(UsuarioProveedor);
    WM("_Controls/Operacion.ReporteMantenimiento.aspx/ObtenerUsuarioProveedores", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {
            COMBO("#cmbUsuarioProveedor", json.Datos.UsuarioProveedores);
        }
        else {
            Error("Listar Circuitos", json.Error);
        }
    });
}

function AgregarBitacora() {
    IdReporte = $("#modalConsultarReporteMantenimiento .modal-dialog").attr('idReporte');
    BitacoraDescripcion = $("#taBitacoraDescripcion", "#modalConsultarReporteMantenimiento").val();
    ChkIntegrantes = $("#chkIntegrantes", "#modalConsultarReporteMantenimiento").is(':checked');
    ChkProveedor = $("#chkProveedor", "#modalConsultarReporteMantenimiento").is(':checked');

    var Bitacora = new Object();
    Bitacora.IdReporte = IdReporte;
    Bitacora.BitacoraDescripcion = BitacoraDescripcion;
    Bitacora.EnviaCorreoIntegrante = ChkIntegrantes;
    Bitacora.EnviaCorreoProveedor = ChkProveedor;
    var Request = JSON.stringify(Bitacora);
    WM("_Controls/Operacion.ReporteMantenimiento.aspx/AgregarBitacora", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {
            ListarBitacora();
        }
        else {
            Error("Agregar bitacora", json.Error);
        }
    });
}

function ObtenerFormaEditarSeguimiento(pRequest) {
    ASPT(pRequest, "_Controls/Operacion.ReporteMantenimiento.aspx/ObtenerFormaEditarSeguimiento", "_Views/tmplEditarSeguimientoReporteMantenimiento.html", "#modalEditarSeguimientoReporteMantenimientoTmpl", procesarEditarSeguimientoReporte);
}

function procesarEditarSeguimientoReporte(data) {
    if (data.Error == "") {
        modalName = 'modalEditarSeguimientoReporteMantenimiento';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarSeguimientoReporteMantenimientoTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#" + modalName).on('shown.bs.modal', function () {
            $("#btnEditarSeguimiento").click(EditarSeguimiento);

            $("#cmbTipoProblema").change(function () {
                ObtenerProblemas(parseInt($(this).val()));
            });

            $("#cmbProveedor").change(function () {
                ObtenerUsuarioProveedores(parseInt($(this).val()));
            });
        });
    }
}

function EditarSeguimiento() {
    IdReporte = $("#modalEditarSeguimientoReporteMantenimiento .modal-dialog").attr('idReporte');
    IdTipoProblema = $("#cmbTipoProblema", "#modalEditarSeguimientoReporteMantenimiento").val();
    IdProblema = $("#cmbProblema", "#modalEditarSeguimientoReporteMantenimiento").val();
    IdProveedor = $("#cmbProveedor", "#modalEditarSeguimientoReporteMantenimiento").val();
    IdUsuarioProveedor = $("#cmbUsuarioProveedor", "#modalEditarSeguimientoReporteMantenimiento").val();

    var Reporte = new Object();
    Reporte.IdReporte = IdReporte;
    Reporte.IdTipoProblema = IdTipoProblema;
    Reporte.IdProblema = IdProblema;
    Reporte.IdProveedor = IdProveedor;
    Reporte.IdUsuarioProveedor = IdUsuarioProveedor;

    var Request = JSON.stringify(Reporte);
    WM("_Controls/Operacion.ReporteMantenimiento.aspx/EditarSeguimiento", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#lblTipoProblema", "#modalConsultarReporteMantenimiento").text(json.Datos.TipoProblema);
            $("#lblProblema", "#modalConsultarReporteMantenimiento").text(json.Datos.Problema);
            $("#lblProveedor", "#modalConsultarReporteMantenimiento").text(json.Datos.Proveedor);
            $("#lblUsuarioProveedor", "#modalConsultarReporteMantenimiento").text(json.Datos.UsuarioProveedor);

            $("#modalEditarSeguimientoReporteMantenimiento").modal("hide");
        }
        else {
            Error("Agregar bitacora", json.Error);
        }
    });
}

function ObtenerFormaConsultarDocumento(pRequest) {
    ASPT(pRequest, "_Controls/Operacion.ReporteMantenimiento.aspx/ObtenerFormaConsultarDocumento", "_Views/tmplConsultarDocumento.html", "#modalConsultarDocumentoTmpl", procesarConsultarDocumento);
}

function procesarConsultarDocumento(data) {
    if (data.Error == "") {
        modalName = 'modalConsultarDocumento';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalConsultarDocumentoTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#" + modalName).on('shown.bs.modal', function () {
            upload();
            //subir();



        });
    }
}

function upload() {
    pIdReporte = $("#modalConsultarReporteMantenimiento .modal-dialog").attr('idReporte');

    'use strict';
    var url = 'Handler/FileUpload.ashx?pReporte=' + pIdReporte,
	uploadButton = $('<button/>')
	.addClass('btn btn-success btn-small')
	.prop('disabled', true)
	.text('Cargando...')
	.on('click', function () {
	    var $this = $(this),
		data = $this.data();
	    $this
            .off('click')
            .text('Cancelar')
            .on('click', function () {
                $this.remove();
                data.abort();
            });
	    data.submit().always(function () {
	        $this.remove();
	    });
	});


    $('#fileupload').fileupload({
        url: url,
        dataType: 'json',
        autoUpload: false,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png|pdf|doc|docx|xls|xlsx)$/i,
        maxFileSize: 5000000, // 5 MB
        disableImageResize: /Android(?!.*Chrome)|Opera/
            .test(window.navigator.userAgent),
        previewMaxWidth: 100,
        previewMaxHeight: 100,
        previewCrop: true
    }).on('fileuploadadd', function (e, data) {
        data.context = $('<div/>').appendTo('#files');
        $.each(data.files, function (index, file) {
            var node = $('<p/>')            
            .append($('<span style="padding: 0 10px 0 5px; width:400px;white-space: nowrap;overflow: hidden;text-overflow: ellipsis;" class="fileName" /> <input class="form-control" placeholder="Descripción del documento" id="txtDescripcionDocumento" name="txtDescripcionDocumento" style="width:300px; display:inline;" />').text(file.name));

            if (!index) {
                node.prepend(uploadButton.clone(true).data(data));
            }
            node.appendTo(data.context);
            file.url = 1;
        });
    }).on('fileuploadprocessalways', function (e, data) {
        var index = data.index,
            file = data.files[index],
            node = $(data.context.children()[index]);

        if (file.error) {
            node
                .append('<br>')
                .append($('<span class="text-danger"/>').text(file.error));
        }
        if (index + 1 === data.files.length) {
            data.context.find('button')
                .text('Cargar')
                .prop('disabled', !!data.files.error);
        }
    }).on('fileuploadprogressall', function (e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progress .progress-bar').css(
            'width',
            progress + '%'
        );
    }).on('fileuploaddone', function (e, data) {
        $.each(data.files, function (index, file) {
            if (data.result.url) {                

                var Documento = new Object();
                Documento.pIdReporteDocumento = data.result.IdReporteDocumento;
                Documento.pDescripcion = $(data.context.children().children('input')).val()
                var oRequest = new Object();
                oRequest.Documento = Documento;
                var pRequest = JSON.stringify(oRequest);
                actualizaDescripcionDocumento(pRequest);

                $(data.context.children().children('input')).prop('disabled', true);

                var link = $('<a>')
                    .attr('target', '_blank')
                    .prop('href', data.result.url);

                //$(data.context.children().children('span')).wrap(link)

                
            } else if (file.error) {
                var error = $('<span class="text-danger"/>').text(file.error);
                $(data.context.children()[index])
                    .append('<br>')
                    .append(error);
            }
        });
    }).on('fileuploadfail', function (e, data) {
        $.each(data.files, function (index) {
            var error = $('<span class="text-danger"/>').text('Falló la carga de documento');
            $(data.context.children()[index])
                .append('<br>')
                .append(error);
        });
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');

    return false;
}

function actualizaDescripcionDocumento(pRequest) {
    WM("_Controls/Operacion.ReporteMantenimiento.aspx/actualizaDescripcionDocumento", pRequest, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {
            $('#tblListaDocumentos tr:last').after('<tr id="renglon-' + json.IdReporteDocumento + '"><td style="width:20px;padding:auto;"><span id="descargarDocumento" class="ku-clickable glyphicon glyphicon-download" Documento="' + json.IdReporteDocumento + '" /></span></td><td>' + json.Documento + '</td><td>' + json.Descripcion + '</td><td style="width:20px;padding:auto;"><span id="eliminarDocumento" class="ku-clickable glyphicon glyphicon-trash" Documento="' + json.IdReporteDocumento + '" /></span></td></tr>');
        }
        else {
            Error("Documento", json.Error);
        }
     });    
}

function EliminarDocumento(pRequest, pIdReporteDocumento) {
    WM("_Controls/Operacion.ReporteMantenimiento.aspx/EliminarDocumento", pRequest, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {
            $("#tblListaDocumentos tr#renglon-" + pIdReporteDocumento).remove();

            //Error("Documentos", "Documento eliminado");
        }
        else {
            Error("Agregar bitacora", json.Error);
        }
    });
}

function ObtenerFormaEntregarReporte(pRequest) {
    ASPT(pRequest, "_Controls/Operacion.ReporteMantenimiento.aspx/ObtenerFormaEntregarReporte", "_Views/tmplEntregarReporteMantenimiento.html", "#modalEntregarReporteMantenimientoTmpl", procesarEntregarReporte);
}

function procesarEntregarReporte(data) {
    if (data.Error == "") {
        modalName = 'modalEntregarReporte';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEntregarReporteTmpl").processTemplate(data);
        $("#" + modalName).modal();

        $("#" + modalName).on('shown.bs.modal', function () {
            $("#btnEntregarReporte").click(EntregarReporte);
        });
    }
}

function EntregarReporte(pRequest) {
    WM("_Controls/Operacion.ReporteMantenimiento.aspx/EntregarReporte", pRequest, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {
            ListarBitacora();
            $("#modalConsultarReporteMantenimiento").modal("hide");
        }
        else {
            Error("Entregar reporte", json.Error);
        }
    });
}