<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formEditarTarifa.aspx.cs" Inherits="_Views_formEditarTarifa" %>
<div id="modalEditarTarifa" class="modal fade" role="dialog" IdTarifa="<%=Id %>">
	<div class="modal-dialog modal-lg">
		<div class="modal-content">
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4 class="modal-title">Editar tarifa</h4>
			</div>
			<div class="modal-body">
				<div class="container-fluid">
                    <div class="well bs-component">
                        <div class="row">
                            <legend>Datos de la tarifa</legend>
							<div class="col-md-3 form-group">
								<strong class="control-strong">Fuente</strong>
								<select class="form-control input-sm" id="cmbFuente" style="width:100%;">
									<option value="0">-Seleccionar-</option>
									<%
										foreach (Dictionary<string, object> Fuente in TipoFuentes.ToList())
										{
									%><option value="<%=Fuente["IdFuente"] %>"<%=((IdFuente == Fuente["IdFuente"].ToString())?" selected":"")%>><%=Fuente["Fuente"] %></option>
									<%
										}
									 %>
								</select>
							</div>
                            <div class="col-md-3 form-group">
                                <strong class="control-strong">Tipo tarifa</strong>
								<select class="form-control input-sm" id="cmbTipoTarifa" style="width:100%;">
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
                            <div class="col-md-3 form-group">
                                <strong class="control-strong">Tipo tensión</strong>
								<select class="form-control input-sm" id="cmbTipoTension" style="width:100%;">
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
                            <div class="col-md-3 form-group">
                                <strong class="control-strong">Tipo cuota</strong>
								<select class="form-control input-sm" id="cmbTipoCuota" style="width:100%;">
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
		                    <div class="col-md-3 form-group">
                                <strong class="control-strong">Región</strong>
								<select class="form-control input-sm" id="cmbRegion" style="width:100%;">
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
                            <div class="col-md-3 form-group">
                                <strong class="control-strong">Consumo Baja</strong>
                                <input type="text" name="txtConsumoBaja" class="form-control" id="txtConsumoBaja" value="<%=ConsumoBaja %>" placeholder="$0.0000" onkeypress="validate(event)" onblur="clean(event)">
			                </div>
                            <div class="col-md-3 form-group">
                                <strong class="control-strong">Consumo Media</strong>
                                <input type="text" name="txtConsumoMedia" class="form-control" id="txtConsumoMedia" value="<%=ConsumoMedia %>" placeholder="$0.0000" onkeypress="validate(event)" onblur="clean(event)">
			                </div>
                            <div class="col-md-3 form-group">
                                <strong class="control-strong">Consumo Alta</strong>
                                 <input type="text" name="txtConsumoAlta" class="form-control" id="txtConsumoAlta" value="<%=ConsumoAlta %>" placeholder="$0.0000" onkeypress="validate(event)" onblur="clean(event)">
			                </div>
                            <div class="col-md-3 form-group">
                                <strong class="control-strong">Demanda</strong>
                                 <input type="text" name="txtDemanda" class="form-control" id="txtDemanda" value="<%=Demanda %>" placeholder="$0.0000" onkeypress="validate(event)" onblur="clean(event)">
			                </div>
                          <%--  <div class="col-md-3 form-group">
                                <strong class="control-strong">Fecha</strong>
								<div id='txtFecha' class='input-group date'>
                                    <input id="inicio" type='text' class="form-control" value="<%=Fecha %>" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
			                </div>--%>
								<div>
								<div class="col-md-3 form-group">
									<strong class="control-strong">Mes</strong>
									<select class="form-control input-sm" name="cmbMes" id="cmbMes">
										<option <%if (Mes=="0"){ %> selected="selected" <%}%> value="0">-Seleccionar-</option>
										<option <%if (Mes=="1"){ %> selected="selected" <%}%>  value="1">Enero</option>
										<option <%if (Mes=="2"){ %> selected="selected" <%}%>  value="2">Febrero</option>
										<option <%if (Mes=="3"){ %> selected="selected" <%}%>  value="3">Marzo</option>
										<option <%if (Mes=="4"){ %> selected="selected" <%}%>  value="4">Abril</option>
										<option <%if (Mes=="5"){ %> selected="selected" <%}%>  value="5">Mayo</option>
										<option <%if (Mes=="6"){ %> selected="selected" <%}%>  value="6">Junio</option>
										<option <%if (Mes=="7"){ %> selected="selected" <%}%>  value="7">Julio</option>
										<option <%if (Mes=="8"){ %> selected="selected" <%}%>  value="8">Agosto</option>
										<option <%if (Mes=="9"){ %> selected="selected" <%}%>  value="9">Septiembre</option>
										<option <%if (Mes=="10"){ %> selected="selected" <%}%>  value="10">Octubre</option>
										<option <%if (Mes=="11"){ %> selected="selected" <%}%>  value="11">Noviembre</option>
										<option <%if (Mes=="12"){ %> selected="selected" <%}%>  value="12">Diciembre</option>
									</select>
								</div>
								<div class="col-md-3 form-group">
									<strong class="control-strong">Año</strong>
									<input type="text" name="txtAnio" class="form-control" id="txtAnio" value="<%=Anio%>">
								</div>
							</div>
	                    </div>                   
                    </div>
                </div>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-success" id="btnEditarTarifa">Editar</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
			</div>
		</div>

	</div>
</div>