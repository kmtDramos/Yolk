/**/

$(function () {

	$("#btnAnteriorPagina").click(function () {
		var Pagina = parseInt($("#txtPagina").val()) - 1;
		Pagina = (Pagina < 1) ? 1 : Pagina;
		$("#txtPagina").val(Pagina).change();
	});

	$("#btnSiguientePagina").click(function () {
		var Paginas = parseInt($("#lblPaginas").text());
		var Pagina = parseInt($("#txtPagina").val()) + 1;
		Pagina = (Pagina > Paginas) ? Paginas : Pagina;
		$("#txtPagina").val(Pagina).change();
	});

	$("#txtPagina").change(function () {
		ListarUsuarios();
	}).change();

	$("#btnObtenerFormaAgregarUsuario").click(ObtenerFormaAgregarUsuario);

	$('body').on('click', "#TodosAsignar", function (e) {
	    if ($(this).is(':checked')) {
	        $('input[class=ChkAsignar]').prop('checked', true);
	    } else {
	        $('input[class=ChkAsignar]').prop('checked', false);
	    }
	});

	$('body').on('click', "#TodosDesAsignar", function (e) {
	    if ($(this).is(':checked')) {
	        $('input[class=ChkDesasignar]').prop('checked', true);
	    } else {
	        $('input[class=ChkDesasignar]').prop('checked', false);
	    }
	});

	$('body').on('click', "#btnAsignarSucursales", function (e) {
	    AsignarSucursales();
	});

	$('body').on('click', "#btnDesasignarSucursales", function (e) {
	    DesasignarSucursales();
	});

	$('body').on('click', "#divPredeterminada", function (e) {
	    var row = $(this).parents('tr');
	    var id = row.data('id');
        Predeterminada(id)
	});

	$('body').on('click', "#btnCerrarUsuarioSucursal", function (e) {
	    SucursalPredeterminada();
	});

	$('body').on('click', "#btnEditarContrasena", function (e) {
	    EditarContrasena();
	});

	

});

function ListarUsuarios() {
	var Usuarios = new Object();
	Usuarios.Pagina = parseInt($("#txtPagina").val())
	Usuarios.Columna = "Nombre";
	Usuarios.Orden = "ASC";
	var Request = JSON.stringify(Usuarios);
	WM("_Controls/Usuario.aspx/ListarUsuarios", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		if (json.Error == "")
		{
		    $("#lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
		    $("#lblRegistros").text(json.Datos.Paginador[0].NoRegistros);
			var Usuarios = json.Datos.Usuarios;
			$("tbody", "#tblListaUsuarios").html('');
			for (x in Usuarios) {
				var usr = Usuarios[x];
				var tr = $("<tr></tr>");
				$(tr)
					.append($("<td>" + usr.Nombre + "</td>"))
					.append($("<td>" + usr.ApellidoPaterno + "</td>"))
					.append($("<td>" + usr.ApellidoMaterno + "</td>"))
					.append($("<td>" + usr.Perfil + "</td>"))
					.append($("<td>" + usr.Usuario + "</td>"))
					.append($("<td class=\"hidden-xs hidden-sm\">" + usr.Correo + "</td>"))
					.append($("<td class=\"ku-clickable text-center\" onclick=\"ObtenerFormaEditarContrasena(" + usr.IdUsuario + ")\"><i class=\"fa fa-key\"/></td>"))
                    .append($("<td class=\"ku-clickable text-center\" onclick=\"ObtenerFormaAsignarSucursal(" + usr.IdUsuario + ");\"><span class='glyphicon glyphicon-home'></span></td>"))
                    .append($("<td class=\"ku-clickable text-center\" onclick=\"ObtenerFormaEditarUsuario(" + usr.IdUsuario + ")\"><i class=\"glyphicon glyphicon-edit\"/></td>"))
                    .append($("<td id='desactivarUsuario' onclick=\"DesactivarUsuario(" + usr.IdUsuario + "," + usr.Baja + " );\" estatus=\"" + usr.Baja + "\" class=\"ku-clickable text-center hidden-xs hidden-sm\">" + usr.Estatus + "</td>"));
				$("tbody", "#tblListaUsuarios").append(tr);
			}
		}
		else
		{
			Error("Listar usuarios", json.Error);
		}
	});
}

function ObtenerFormaAgregarUsuario() {
    var Usuario = new Object();
    var pRequest = JSON.stringify(Usuario);
    ASPT(pRequest, "_Controls/Usuario.aspx/ObtenerFormaAgregarUsuario", "_Views/tmplAgregarUsuario.html", "#modalAgregarUsuarioTmpl", procesarAgregarUsuario);
}

function procesarAgregarUsuario(data) {
    if (data.Error == "") {
        modalName = 'modalAgregarUsuario';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalAgregarUsuarioTmpl").processTemplate(data);
        $("#" + modalName).modal();

        //Al terminar de cargar el modal
        $("#" + modalName).on('shown.bs.modal', function () {

            $("#btnAgregarUsuario").click(function () {
                AgregarUsuario();
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
		
            $("#cmbPerfil").change(function () {
        	    ObtenerProveedores(parseInt($(this).val()));
            });

            $("#cmbProveedor").change(function () {
        	    IdPerfil = parseInt($("#cmbPerfil").val());
    		    IdPoveedor = $("#cmbProveedor").val();
    		    console.log(IdPerfil);
    		    ObtenerProveedores(IdPerfil, IdPoveedor, parseInt($(this).val()));
            });
        });
    }
    else {
        Error("Formulario Usuario ", json.Error);
    }
}

function AgregarUsuario() {
	var Usuario = new Object();
	Usuario.Nombre = $("#txtNombre", "#modalAgregarUsuario").val();
	Usuario.ApellidoPaterno = $("#txtApellidoPaterno", "#modalAgregarUsuario").val();
	Usuario.ApellidoMaterno = $("#txtApellidoMaterno", "#modalAgregarUsuario").val();
	Usuario.Usuario = $("#txtUsuario", "#modalAgregarUsuario").val();
	Usuario.Password = $("#txtPassword", "#modalAgregarUsuario").val();
	Usuario.Correo = $("#txtCorreo", "#modalAgregarUsuario").val();
	Usuario.IdPerfil = $("#cmbPerfil", "#modalAgregarUsuario").val();
	Usuario.JefeInmediato = parseInt($("#cmbJefeInmediato", "#modalAgregarUsuario").val());
	Usuario.IdSucursal = $("#cmbSucursal", "#modalAgregarUsuario").val();
	Usuario.esRespSuc = ($("#checkEsRespSuc").is(':checked')) ? "true" : "false";
    Usuario.IdProveedor = ($("#cmbProveedor", "#modalAgregarUsuario").val());

	Ejecutar = true;
	errores = [];

	if (Usuario.Nombre === "") {
	    Ejecutar = false;
	    errores.push("Nombre es requerido.");
	}

	if (Usuario.ApellidoPaterno === "") {
	    Ejecutar = false;
	    errores.push("Apellido paterno es requerido.");
	}

	if (Usuario.Correo === "") {
	    Ejecutar = false;
	    errores.push("Correo es requerido.");
	}

	if (Usuario.IdPerfil === "0") {
	    Ejecutar = false;
	    errores.push("Perfil es requerido.");
	}
	else {
	    if (Usuario.IdPerfil === "4" && Usuario.IdProveedor === "0") {
	        Ejecutar = false;
	        errores.push("El proveedor es requerido");
	    }
	}

	if (Usuario.IdSucursal === "0") {
	    Ejecutar = false;
	    errores.push("La sucursal es requerida.");
	}

	if (Usuario.Usuario === "") {
	    Ejecutar = false;
	    errores.push("Usuario es requerido.");
	}

	if (Usuario.Password === "") {
	    Ejecutar = false;
	    errores.push("Contraseña es requerido.");
	}

	if (Ejecutar) {
	    var Request = JSON.stringify(Usuario);
	    WM("_Controls/Usuario.aspx/AgregarUsuario", Request, function (Respuesta) {
	        var json = JSON.parse(Respuesta.d);
	        if (json.Error == "") {
	            ListarUsuarios();
	            $("#modalAgregarUsuario").modal("hide");
	        }
	        else {
	            Error("Agregar usuario", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
	        }
	    });
    }
    else {
	        ManejarArregloDeErrores(errores);
	}

}

function ObtenerFormaEditarUsuario(IdUsuario) {

    var Usuario = new Object();
    Usuario.IdUsuario = IdUsuario;
    var pRequest = JSON.stringify(Usuario);
    ASPT(pRequest, "_Controls/Usuario.aspx/ObtenerFormaEditarUsuario", "_Views/tmplEditarUsuario.html", "#modalEditarUsuarioTmpl", procesarEditarUsuario);
}

function procesarEditarUsuario(data) {
    if (data.Error == "") {
        modalName = 'modalEditarUsuario';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalEditarUsuarioTmpl").processTemplate(data);
        $("#" + modalName).modal();

        //Al terminar de cargar el modal
        $("#" + modalName).on('shown.bs.modal', function () {
            $("#btnEditarUsuario").click(EditarUsuario);

            if ( $("#cmbPerfil").val() == 4) {
                $("#divProveedor").removeClass("hidden");
            }
            else{
                $("#divProveedor").addClass("hidden");
            }

            $("#cmbPerfil").change(function () {
                ObtenerProveedores(parseInt($(this).val()));
            });
        });
    }
    else {
        Error("Editar usuario", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
    }
}
function EditarUsuario() {
	var Usuario = new Object();
	Usuario.IdUsuario = parseInt($("#modalEditarUsuario .modal-dialog").attr('idUsuario')); 
	Usuario.Nombre = $("#txtNombre", "#modalEditarUsuario").val();
	Usuario.ApellidoPaterno = $("#txtApellidoPaterno", "#modalEditarUsuario").val();
	Usuario.ApellidoMaterno = $("#txtApellidoMaterno", "#modalEditarUsuario").val();
	Usuario.Usuario = $("#txtUsuario", "#modalEditarUsuario").val();
	Usuario.Correo = $("#txtCorreo", "#modalEditarUsuario").val();
	Usuario.IdPerfil = $("#cmbPerfil", "#modalEditarUsuario").val();
	Usuario.EsReponsableSucursal = ($("#EsResponsableSucursal").is(':checked')) ? "true" : "false";
	Usuario.IdUsuarioJefe = $("#cmbJefeInmediato", "#modalEditarUsuario").val();
	Usuario.IdProveedor = $("#cmbProveedor", "#modalEditarUsuario").val();	

	Ejecutar = true;
	errores = [];

	if (Usuario.IdPerfil === "0") {
	    Ejecutar = false;
	    errores.push("Perfil es requerido.");
	}
	else {
	    if (Usuario.IdPerfil === "4" && Usuario.IdProveedor === "0") {
	        Ejecutar = false;
	        errores.push("El proveedor es requerido");
	    }
	}

	if (Usuario.Nombre === "") {
	    Ejecutar = false;
	    errores.push("El nombre del usuario es requerido.");
	}

	if (Usuario.Correo === "") {
	    Ejecutar = false;
	    errores.push("El correo es requerido.");
	}

	if (Ejecutar) {
	    var Request = JSON.stringify(Usuario);
	    WM("_Controls/Usuario.aspx/EditarUsuario", Request, function (Respuesta) {
		    var json = JSON.parse(Respuesta.d);
		    if (json.Error == "") {
		        $("#modalEditarUsuario").modal("hide");
		        ListarUsuarios();
		    }
		    else {
			    Error("Editar usuario", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
		    }
	    });
	}
	else {
	    ManejarArregloDeErrores(errores);
	}
}

function ObtenerPermisos(IdUsuario) {
	var Usuario = new Object();
	Usuario.IdUsuario = IdUsuario;
	var Request = JSON.stringify(Usuario);
	WM("_Controls/Usuario.aspx/ObtenerUsuarioPermisos", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		var Permisos = json.Datos.Permisos;
		var Lista = $("<ul></ul>");
		for (x in Permisos) {
			var IdPermiso = Permisos[x].IdPermiso;
			var Permiso = Permisos[x].Permiso;
			var IdUsuarioPermiso = Permisos[x].IdUsuarioPermiso;
			var Pantalla = Permisos[x].Pantalla;
			var Estatus = Permisos[x].Estatus;
			$(Lista).append($("<li><label><input type=\"checkbox\" value=\"" + IdPermiso + "\" " + ((Estatus == 0) ? "" : "Checked") + "/>" + Permiso + " (" + Pantalla + ")</labe></li>"));
		}
		TMPL("tmplUsuarioPermisos.html", function (Template) {
			var tmpl = $(Template);
			$("body").append(tmpl);
			$("#modalPermisosUsuario").modal();
			$("#divPermisos", "#modalPermisosUsuario").append(Lista);
			$("#modalPermisosUsuario").on("hidden.bs.modal", function () { $("#modalPermisosUsuario").remove(); });
			$("#btnGuardarPermisos").click(GuardarPermisos);
		});
	});
}





function GuardarPermisos(IdUsuario) {
	var Permisos = new Object();
	Permisos.IdUsuario = IdUsuario;
}

function ObtenerComboPerfil() {
    var Perfil = new Object();
    var Request = JSON.stringify(Perfil);
    WM("_Controls/Usuario.aspx/ObtenerComboPerfil", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbPerfil", json.Datos.Perfiles);
    });
}

function ObtenerFormaAsignarSucursal(IdUsuario) {
    var Usuario = new Object();
    Usuario.IdUsuario = IdUsuario;
    var pRequest = JSON.stringify(Usuario);
    ASPT(pRequest, "_Controls/Usuario.aspx/ObtenerFormaListarUsuarioSucursales", "_Views/tmplUsuarioSucursales.html", "#modalUsuarioSucursalesTmpl", procesarListarUsuarioSucursales);
}

function procesarListarUsuarioSucursales(data) {
    respuesta = data.Datos.SucursalesDisponibles;
    if (data.Error == "") {
        modalName = 'modalUsuarioSucursales';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement("modalUsuarioSucursalesTmpl").processTemplate(data);
        $("#" + modalName).modal();

        //Al terminar de cargar el modal
        $("#" + modalName).on('shown.bs.modal', function () {
            ///////////////////////////////////////////
            //SUCURSALES DISPONIBLES
            $("#PaginadorSucursalesDisponibles #btnAnteriorPagina").click(function () {
                var Pagina = parseInt($("#PaginadorSucursalesDisponibles #txtPagina").val()) - 1;
                Pagina = (Pagina < 1) ? 1 : Pagina;
                $("#PaginadorSucursalesDisponibles #txtPagina").val(Pagina).change();
            });

            $("#PaginadorSucursalesDisponibles #btnSiguientePagina").click(function () {
                var Paginas = parseInt($("#PaginadorSucursalesDisponibles #lblPaginas").text());
                var Pagina = parseInt($("#PaginadorSucursalesDisponibles #txtPagina").val()) + 1;
                Pagina = (Pagina > Paginas) ? Paginas : Pagina;
                $("#PaginadorSucursalesDisponibles #txtPagina").val(Pagina).change();
            });

            $("#PaginadorSucursalesDisponibles #txtPagina").change(function () {
                ListarSucursalesDisponibles();
            }).change();

            ///////////////////////////////////////////
            //SUCURSALES ASIGNADAS
            $("#PaginadorSucursalesAsignadas #btnAnteriorPagina").click(function () {
                var Pagina = parseInt($("#PaginadorSucursalesAsignadas #txtPagina").val()) - 1;
                Pagina = (Pagina < 1) ? 1 : Pagina;
                $("#PaginadorSucursalesAsignadas #txtPagina").val(Pagina).change();
            });

            $("#PaginadorSucursalesAsignadas #btnSiguientePagina").click(function () {
                var Paginas = parseInt($("#PaginadorSucursalesAsignadas #lblPaginas").text());
                var Pagina = parseInt($("#PaginadorSucursalesAsignadas #txtPagina").val()) + 1;
                Pagina = (Pagina > Paginas) ? Paginas : Pagina;
                $("#PaginadorSucursalesAsignadas #txtPagina").val(Pagina).change();
            });

            $("#PaginadorSucursalesAsignadas #txtPagina").change(function () {
                ListarSucursalesAsignadas();
            }).change();
        });
    }
    else {
        Error("Forma alta circuito ", json.Error);
    }
}

function ListarSucursalesDisponibles() {
    var UsuarioSucursal = new Object();
    UsuarioSucursal.Pagina = parseInt($("#PaginadorSucursalesDisponibles #txtPagina").val());
    UsuarioSucursal.Columna = "S.Sucursal";
    UsuarioSucursal.Orden = "ASC";
    UsuarioSucursal.IdUsuario = $("#modalUsuarioSucursales .modal-dialog").attr('idUsuario');

    var Request = JSON.stringify(UsuarioSucursal);
    WM("_Controls/Usuario.aspx/ListarSucursalesDisponibles", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {

            $("#PaginadorSucursalesDisponibles #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorSucursalesDisponibles #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var SucursalesDisponibles = json.Datos.SucursalesDisponibles;
            $("tbody", "#tblListaSucursalesDisponibles").html('');
            for (x in SucursalesDisponibles) {
                var suc = SucursalesDisponibles[x];
                var tr = $("<tr data-id=" + suc.IdSucursal + "></tr>");
                $(tr)
                    .append($("<td class='text-left'>" + suc.Cliente + "</td>"))
                    .append($("<td class='text-left'>" + suc.Sucursal + "</td>"))
                    .append($("<td class='text-center'><input type='checkbox' class='ChkAsignar' value=" + suc.IdSucursal + "></td>"));
                $("tbody", "#tblListaSucursalesDisponibles").append(tr);
            }
        }
        else {
            Error("Listar sucursales disponibles", json.Error);
        }
    });
}

function ListarSucursalesAsignadas() {
    var UsuarioSucursal = new Object();
    UsuarioSucursal.Pagina = parseInt($("#PaginadorSucursalesAsignadas #txtPagina").val());
    UsuarioSucursal.Columna = "S.Sucursal";
    UsuarioSucursal.Orden = "ASC";
    UsuarioSucursal.IdUsuario = $("#modalUsuarioSucursales .modal-dialog").attr('idUsuario');

    var Request = JSON.stringify(UsuarioSucursal);
    WM("_Controls/Usuario.aspx/ListarSucursalesAsignadas", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        IdSucursalPredeterminada = json.IdSucursalPredeterminada;
        
        if (json.Error == "") {
           
            $("#PaginadorSucursalesAsignadas #lblPaginas").text(json.Datos.Paginador[0].NoPaginas);
            $("#PaginadorSucursalesAsignadas #lblRegistros").text(json.Datos.Paginador[0].NoRegistros);

            var SucursalesAsignadas = json.Datos.SucursalesAsignadas;
            $("tbody", "#tblListaSucursalesAsignadas").html('');
            for (x in SucursalesAsignadas) {
                predeterminado = "<img src=\"imgs/off.png\">";

                var suc = SucursalesAsignadas[x];

                if (IdSucursalPredeterminada == suc.IdSucursal) {
                    predeterminado = "<img src=\"imgs/on.png\">";
                }
                var tr = $("<tr data-id=" + suc.IdUsuarioSucursal + "></tr>");
                $(tr)
                    .append($("<td class='text-center'><div id=\"divPredeterminada\">" + predeterminado + "</div></td>"))
                    .append($("<td class='text-left'>" + suc.Cliente + "</td>"))
                    .append($("<td class='text-left'>" + suc.Sucursal + "</td>"))
                    .append($("<td class='text-center'><input type='checkbox' class='ChkDesasignar' value=" + suc.IdUsuarioSucursal + "></td>"));
                $("tbody", "#tblListaSucursalesAsignadas").append(tr);
            }
        }
        else {
            Error("Listar sucursales asignadas", json.Error);
        }
    });
}

function AsignarSucursales() {
    var FilasDisponibles = [];

    $(".ChkAsignar").each(function (index) {
        if ($(this).is(':checked')) {
            FilasDisponibles.push($(this).val());
        }
    });

    Ejecutar = true;
    errores = [];
    if (FilasDisponibles.length == 0) {
        Ejecutar = false;
        errores.push("Debe selecionar las sucursales a asignar");
    }

    if (Ejecutar) {
        var Sucursal = new Object();
        Sucursal.IdSucursalDisponible = FilasDisponibles;
        Sucursal.IdUsuario = $("#modalUsuarioSucursales .modal-dialog").attr('idUsuario');

        var Request = JSON.stringify(Sucursal);
        WM("_Controls/Usuario.aspx/AsignarSucursales", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarSucursalesDisponibles()
                ListarSucursalesAsignadas()
            }
            else {
                Error("Asignar sucursales", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function DesasignarSucursales() {
    var FilasAsignadas = [];

    $(".ChkDesasignar").each(function (index) {
        if ($(this).is(':checked')) {
            FilasAsignadas.push($(this).val());
        }
    });

    Ejecutar = true;
    errores = [];
    if (FilasAsignadas.length == 0) {
        Ejecutar = false;
        errores.push("Debe selecionar las sucursales para des asignar");
    }

    if (Ejecutar) {
        var Sucursal = new Object();
        Sucursal.pIdUsuarioSucursal = FilasAsignadas;
        Sucursal.IdUsuario = $("#modalUsuarioSucursales .modal-dialog").attr('idUsuario');;

        var Request = JSON.stringify(Sucursal);
        WM("_Controls/Usuario.aspx/DesasignarSucursales", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarSucursalesDisponibles()
                ListarSucursalesAsignadas()
            }
            else {
                Error("Des asignar sucursales", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function ObtenerProveedores(IdPerfil) {
	var Perfil = new Object();
	Perfil.IdPerfil = IdPerfil;
	var Request = JSON.stringify(Perfil);
	WM("_Controls/Usuario.aspx/ObtenerProveedores", Request, function (Respuesta) {
		var json = JSON.parse(Respuesta.d);
		COMBO("#cmbProveedor", json.Datos.Proveedores);
		if (IdPerfil == 4) {

		    $("#divProveedor").removeClass("hidden");
		}
		else {
		    $("#divProveedor").addClass("hidden");
		}
	});
}

function ObtenerPaises(IdCliente) {
    var Cliente = new Object();
    Cliente.IdCliente = IdCliente;
    var Request = JSON.stringify(Cliente);
    WM("_Controls/Usuario.aspx/ObtenerPaises", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbPais", json.Datos.Paises);
    });
}

function ObtenerEstados(IdCliente, IdPais) {
    var Estado = new Object();
    Estado.IdCliente = IdCliente;
    Estado.IdPais = IdPais;
    var Request = JSON.stringify(Estado);
    WM("_Controls/Usuario.aspx/ObtenerEstados", Request, function (Respuesta) {
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
    WM("_Controls/Usuario.aspx/ObtenerMunicipios", Request, function (Respuesta) {
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
    WM("_Controls/Usuario.aspx/ObtenerSucursales", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        COMBO("#cmbSucursal", json.Datos.Sucursales);

        LimpiarCombo("#cmbMedidor");
        LimpiarCombo("#cmbTablero");
    });
}

function SucursalPredeterminada() {
    var Usuario = new Object();
    Usuario.IdUsuario = $("#modalUsuarioSucursales .modal-dialog").attr('idUsuario');

    var Request = JSON.stringify(Usuario);
    WM("_Controls/Usuario.aspx/SucursalPredeterminada", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "")
        {
            $('#modalUsuarioSucursales').modal('hide');
        }
        else {
            Error("Sucursales asignadas", json.Error);
            return false;
            
        }
    });
}

function Predeterminada(id) {
    var Usuario = new Object();
    Usuario.IdUsuario = $("#modalUsuarioSucursales .modal-dialog").attr('idUsuario');
    Usuario.IdUsuarioSucursal = id;
    var Request = JSON.stringify(Usuario);
    WM("_Controls/Usuario.aspx/EstablecerSucursalPredeterminada", Request, function (Respuesta) {
        var json = JSON.parse(Respuesta.d);
        if (json.Error == "") {
            ListarSucursalesAsignadas();
        }
        else {
            Error("Sucursales asignadas", json.Error);
        }
    });
}

function ObtenerFormaEditarContrasena(Id) {
    var Usuario = new Object();
    Usuario.IdUsuario = Id;
    var pRequest = JSON.stringify(Usuario);
    ASPT(pRequest, "_Controls/Usuario.aspx/ObtenerFormaEditarContrasena", "_Views/tmplEditarContrasena.html", "#modalEditarContrasenaTmpl", procesarFormaEditarContrasena);
}

function procesarFormaEditarContrasena(data) {
    respuesta = data.Datos;
    if (data.Error == "") {
        modalName = 'modalEditarContrasena';
        CREARMODAL(modalName);
        $("#" + modalName).setTemplateElement(modalName+"Tmpl").processTemplate(data);
        $("#" + modalName).modal();
    }

    $("#" + modalName).on('shown.bs.modal', function () {
        
    });

}

function EditarContrasena() {
    var Usuario = new Object();
    Usuario.IdUsuario = parseInt($("#modalEditarContrasena .modal-dialog").attr("idUsuario"));
    Usuario.Password = $("#txtContrasena", "#modalEditarContrasena").val();

    Ejecutar = true;
    errores = [];

    if (Usuario.Password === "") {
        Ejecutar = false;
        errores.push("La contraseña es requerida.");
    }

    if (Ejecutar) {
        var Request = JSON.stringify(Usuario);
        WM("_Controls/Usuario.aspx/EditarContrasena", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                $("#modalEditarContrasena").modal("hide");
            }
            else {
                Error("Editar contrasena", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}

function DesactivarUsuario(IdUsuario, Baja) {

    Ejecutar = true;
    errores = [];

    if (IdUsuario === 0) {
        Ejecutar = false;
        errores.push("Usuario es requerido.");
    }

    if (Baja === "") {
        Ejecutar = false;
        errores.push("Estatus es requerido.");
    }

    if (Ejecutar) {
        var Usuario = new Object();
        Usuario.IdUsuario = IdUsuario;
        Usuario.Baja = Baja;

        var Request = JSON.stringify(Usuario);
        WM("_Controls/Usuario.aspx/DesactivarUsuario", Request, function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == "") {
                ListarUsuarios();
            }
            else {
                Error("Desactivar usuario", json.Error.replace(/u003c/gi, "<").replace(/u003e/gi, ">"));
            }
        });
    }
    else {
        ManejarArregloDeErrores(errores);
    }
}
