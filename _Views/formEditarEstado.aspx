<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formEditarEstado.aspx.cs" Inherits="_Views_formEditarEstado" %>
<div id="modalEditarEstado" class="modal fade" role="dialog" IdEstado="<%=Id %>">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header text-center">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4 class="modal-title">Editar Estado</h4>
			</div>
			<div class="modal-body">
                <div class="container-fluid">
                    <div class="well bs-component">
                        <div class="row">
                        <legend>Datos del Estado</legend>                      
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
				                <strong>Estado</strong>
                                <input type="text" id="txtNombreEstado" class="form-control" value="<%=NombreEstado %>" />
			                </div>
	                    </div>
                    </div>                   
                </div>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-success" id="btnEditarEstado">Editar</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
			</div>
		</div>
	</div>
</div>