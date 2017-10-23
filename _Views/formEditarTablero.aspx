<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formEditarTablero.aspx.cs" Inherits="_Views_formEditarTablero" %>
<div id="modalEditarTablero" class="modal fade" role="dialog" IdTablero="<%=IdTablero %>">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4 class="modal-title">Editar tablero</h4>
			</div>
			<div class="modal-body">
				<div>
					<div class="form-group">
						<label for="txtUsuario">*Tablero:</label>
						<input type="text" id="txtTablero" class="form-control" value="<%=Tablero %>" />
					</div>
				</div>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-success" id="btnEditarTablero">Editar</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
			</div>
		</div>

	</div>
</div>