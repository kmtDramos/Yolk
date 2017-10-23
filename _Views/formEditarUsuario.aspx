<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formEditarUsuario.aspx.cs" Inherits="_Views_formEditarUsuario" %>
<div id="modalEditarUsuario" class="modal fade" role="dialog" IdUsuario="<%=IdUsuario %>">
	<div class="modal-dialog modal-lg">
		<div class="modal-content">
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4 class="modal-title">Editar usuario</h4>
			</div>
			<div class="modal-body">
			    <div class="container-fluid">
                    <div class="well bs-component">
                        <div class="row">
                            <div class="col-md-3 form-group">					            
						        <label for="txtUsuario">*Usuario:</label>
						        <input type="text" id="txtUsuario" class="form-control" value="<%=Usuario %>" disabled />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4 form-group">
						        <label for="">*Nombre:</label>
						        <input type="text" id="txtNombre" class="form-control" value="<%=Nombre %>" />
                            </div>

                            <div class="col-md-4 form-group">
						        <label for="">*Apellido paterno:</label>
						        <input type="text" id="txtApellidoPaterno" class="form-control" value="<%=ApellidoPaterno %>" />
					        </div>
                            
                            <div class="col-md-4 form-group">
						        <label for="">Apellido materno</label>
						        <input type="text" id="txtApellidoMaterno" class="form-control" value="<%=ApellidoMaterno %>" />
                            </div>

                            <div class="col-md-4 form-group">
						        <label for="">*Correo:</label>
						        <input type="text" id="txtCorreo" class="form-control" value="<%=Correo %>" />
					        </div>

                            <div class="col-md-4 form-group">
								<label for="">*Jefe inmediato</label>
								<input type="text" id="txtJefeInmediato" class="form-control" value="hola" />
							</div>

							<div class="col-md-4 form-group">
								<label for="">Responsable sucursal.</label><br />
								<input type="checkbox" id="responsableSuc" value="hola">
							</div>

                        </div>
                        <div class="row">
                            <div class="col-md-4 form-group">
                                <label for="">*Perfil:</label>
                                <select class="form-control input-sm" name='cmbPerfil' id='cmbPerfil'>
						            <option value="0">-Seleccionar-</option>
						            <%
							            foreach (Dictionary<string, object> Perfil in Perfiles.ToList())
							            {
						            %><option value="<%=Perfil["Valor"] %>"<%=((IdPerfil == Perfil["Valor"].ToString())?" selected":"")%>><%=Perfil["Etiqueta"] %></option>
						            <%
							            }
							            %>
                                </select>
                            </div>
							 <div class="col-md-4 form-group">
                                <label for="">*Proveedor:</label>
                                <select class="form-control input-sm" name='cmbProveedor' id='cmbProveedor'>
						            <option value="0">-Seleccionar-</option>
						            <%
							            foreach (Dictionary<string, object> Proveedor in Proveedores.ToList())
							            {
						            %><option value="<%=Proveedor["Valor"] %>"<%=((IdProveedor == Proveedor["Valor"].ToString())?" selected":"")%>><%=Proveedor["Etiqueta"] %></option>
						            <%
							            }
							            %>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-success" id="btnEditarrUsuario">Editar</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
			</div>
		</div>
	</div>
</div>