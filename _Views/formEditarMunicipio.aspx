<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formEditarMunicipio.aspx.cs" Inherits="_Views_formEditarMunicipio" %>
<div id="modalEditarMunicipio" class="modal fade" role="dialog" IdMunicipio="<%=Id %>">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header text-center">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4 class="modal-title">Editar Municipio</h4>
			</div>
			<div class="modal-body">
                <div class="container-fluid">
                    <div class="well bs-component">
                        <div class="row">
                        <legend>Datos del Municipio</legend>                      
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
										foreach (Dictionary<string, object> Pais in Estados.ToList())
										{
									%><option value="<%=Pais["IdEstado"] %>"<%=((IdEstado == Pais["IdEstado"].ToString())?" selected":"")%>><%=Pais["Estado"] %></option>
									<%
										}
									 %>
                                </select>
			                </div>
		                    <div class="col-md-4 form-group">
				                <strong>Municipio</strong>
                                <input type="text" id="txtNombreMunicipio" class="form-control" value="<%=NombreMunicipio %>" />
			                </div>
	                    </div>
                    </div>                   
                </div>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-success" id="btnEditarMunicipio">Editar</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
			</div>
		</div>
	</div>
</div>