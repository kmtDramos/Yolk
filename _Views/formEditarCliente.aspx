<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formEditarCliente.aspx.cs" Inherits="_Views_formEditarCliente" %>
<div id="modalEditarCliente" class="modal fade" role="dialog" idCliente="<%=Id %>">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header text-center">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4 class="modal-title">Editar cliente</h4>
			</div>
			<div class="modal-body">
                <div class="container-fluid">
                    <div class="well bs-component">
                        <div class="row">
                            <legend>Datos del cliente</legend>
		                    <div class="col-md-12 form-group">
				                <strong>Cliente</strong>

                                <input type="text" id="txtCliente" class="form-control" value="<%=Cliente %>" />
			                </div>
               
                        
		                    <div class="col-md-4 form-group">
				                <strong class="control-strong">País</strong>
                                <select class="form-control input-sm" name='cmbPais' id='cmbPais'>
						            <option value="0">-Seleccionar-</option>
									<%
										foreach (Dictionary<string, object> Pais in Paises.ToList())
										{
									%><option value="<%=Pais["IdPais"] %>"<%=((IdPais == Pais["IdPais"].ToString())?" selected":"")%>><%=Pais["Pais"] %></option>
									<%
										}
									 %>
                                </select>
			                </div>
		                    <div class="col-md-4 form-group">
			                    <strong class="control-strong">Estado</strong>
				                <select class="form-control input-sm" name='cmbEstado' id='cmbEstado'>
						            <option value="0">-Seleccionar-</option>
									<%
										foreach (Dictionary<string, object> Estado in Estados.ToList())
										{
									%><option value="<%=Estado["IdEstado"] %>"<%=((IdEstado == Estado["IdEstado"].ToString())?" selected":"")%>><%=Estado["Estado"] %></option>
									<%
										}
									 %>
                                </select>
		                    </div>
		                    <div class="col-md-4 form-group">
			                    <strong class="control-strong">Municipio</strong>
				                <select class="form-control input-sm" name='cmbMunicipio' id='cmbMunicipio'>
						            <option value="0">-Seleccionar-</option>
									<%
										foreach (Dictionary<string, object> Municipio in Municipios.ToList())
										{
									%><option value="<%=Municipio["IdMunicipio"] %>"<%=((IdMunicpio == Municipio["IdMunicipio"].ToString())?" selected":"")%>><%=Municipio["Municipio"] %></option>
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
				<button type="button" class="btn btn-success" id="btnEditarCliente">Editar</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
			</div>
		</div>
	</div>
</div>