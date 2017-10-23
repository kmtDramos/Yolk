<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formEditarMedidor.aspx.cs" Inherits="_Views_formEditarMedidor" %>

<div id="modalEditarMedidor" class="modal fade" role="dialog" IdMedidor="<%=IdMedidor %>">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4 class="modal-title">Editar usuario</h4>
			</div>
			<div class="modal-body">
				<div>
					<div class="form-group">
						<label for="txtUsuario">*Medidor:</label>
						<input type="text" id="txtMedidor" class="form-control" value="<%=Medidor %>" />
					</div>
				</div>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-success" id="btnEditarMedidor">Editar</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
			</div>
		</div>

	</div>
</div>