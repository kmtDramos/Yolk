/**/

$(function () {

	$("#txtUsuario, #txtPassword").keypress(function (e) {
		if (e.keyCode == 13) $("#btnEntrar").click();
	});

	$("#btnEntrar").click(function(){
		var Usuario = new Object();
		Usuario.Usuario = $("#txtUsuario").val();
		Usuario.Password = $("#txtPassword").val();
		var Request = JSON.stringify(Usuario);
		WM("_Controls/Usuario.aspx/Login",Request, function(Respuesta){
			var json = JSON.parse(Respuesta.d);
			if (json.Error != "") {
				Error("Inicio de sesión", json.Error);
			}
			else
			{
			    var splitPath = location.pathname.split("/");
			    splitPath.pop();
			    splitPath.push(json.Datos.Pagina);
			    fullPath = splitPath.join("/");
			    window.location = fullPath;
			}
		});
	});
});