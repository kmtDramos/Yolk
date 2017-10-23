<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formEditarRegion.aspx.cs" Inherits="_Views_formEditarRegion" %>
<div id="modalEditarRegion" class="modal fade" role="dialog" IdRegion="<%=IdRegion %>">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4 class="modal-title">Editar región</h4>
			</div>
			<div class="modal-body">
				<div>
					<div class="form-group">
						<label for="lblRegion">*Región:</label>
						<input type="text" id="txtRegion" class="form-control" value="<%=Region %>" />
					</div>
				</div>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-success" id="btnEditarRegion">Editar</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
			</div>
		</div>

	</div>
</div>