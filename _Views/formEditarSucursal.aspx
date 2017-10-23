<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formEditarSucursal.aspx.cs" Inherits="_Views_formEditarSucursal" %>

<div id="modalEditarSucursal" class="modal fade" role="dialog" idSucursal="<%=IdSucursal %>">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header text-center">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4 class="modal-title">Editar sucursal</h4>
			</div>
			<div class="modal-body">
                <div class="container-fluid">
                    <div class="well bs-component">
                        <div class="row">
                            <legend>Datos de la sucursal</legend>
		                    <div class="col-md-12 form-group">
				                <strong>Sucursal</strong>

                                <input type="text" id="txtSucursal" class="form-control" value="<%=Sucursal %>" />
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
									%><option value="<%=Municipio["IdMunicipio"] %>"<%=((IdMunicipio == Municipio["IdMunicipio"].ToString())?" selected":"")%>><%=Municipio["Municipio"] %></option>
									<%
										}
									 %>
                                </select>
		                    </div>
                            <legend>Datos de la tarifa</legend>
                            <div class="col-md-4 form-group">
			                    <strong class="control-strong">Tipo tarifa</strong>
				                <select class="form-control input-sm" name='cmbTipoTarifa' id='cmbTipoTarifa'>
						            <option value="0">-Seleccionar-</option>
									<%
                                        foreach (Dictionary<string, object> TipoTarifa in TipoTarifas.ToList())
										{
									%><option value="<%=TipoTarifa["IdTipoTarifa"] %>"<%=((IdTipoTarifa == TipoTarifa["IdTipoTarifa"].ToString())?" selected":"")%>><%=TipoTarifa["TipoTarifa"] %></option>
									<%
										}
									 %>
                                </select>
		                    </div>
                            <div class="col-md-4 form-group">
			                    <strong class="control-strong">Tipo tensión</strong>
				                <select class="form-control input-sm" name='cmbTipoTension' id='cmbTipoTension'>
						            <option value="0">-Seleccionar-</option>
									<%
                                        foreach (Dictionary<string, object> TipoTension in TipoTensiones.ToList())
										{
									%><option value="<%=TipoTension["IdTipoTension"] %>"<%=((IdTipoTension == TipoTension["IdTipoTension"].ToString())?" selected":"")%>><%=TipoTension["TipoTension"] %></option>
									<%
										}
									 %>
                                </select>
		                    </div>
                            <div class="col-md-4 form-group">
			                    <strong class="control-strong">Tipo cuota</strong>
				                <select class="form-control input-sm" name='cmbTipoCuota' id='cmbTipoCuota'>
						            <option value="0">-Seleccionar-</option>
									<%
                                        foreach (Dictionary<string, object> TipoCuota in TipoCuotas.ToList())
										{
									%><option value="<%=TipoCuota["IdTipoCuota"] %>"<%=((IdTipoCuota == TipoCuota["IdTipoCuota"].ToString())?" selected":"")%>><%=TipoCuota["TipoCuota"] %></option>
									<%
										}
									 %>
                                </select>
		                    </div>
                            <div class="col-md-4 form-group">
			                    <strong class="control-strong">Region</strong>
				                <select class="form-control input-sm" name='cmbRegion' id='cmbRegion'>
						            <option value="0">-Seleccionar-</option>
									<%
										foreach (Dictionary<string, object> Region in Regiones.ToList())
										{
									%><option value="<%=Region["IdRegion"] %>"<%=((IdRegion == Region["IdRegion"].ToString())?" selected":"")%>><%=Region["Region"] %></option>
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
				<button type="button" class="btn btn-success" id="btnEditarSucursal">Editar</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
			</div>
		</div>
	</div>
</div>