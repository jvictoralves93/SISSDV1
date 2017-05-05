//Novo Usuário
$(function () {
    $(".novo").click(function () {
        $("#modalNovo").modal("show");
    });
});

//Editar Usuário;
function detalheModal(email) {
    var resul = email;
    $("#modalDetalhes").modal("show");    
    Detalhes(resul);
};

//Resetar senha
function modalreset(id) {
    $("#modalReset").modal("show");
};

//Resetar senha
function editar() {
    $("#modalEdit").modal("show");
};

//Resetar senha
function excluir() {
    $("#modalDelete").modal("show");
};

//Editar Usuário;
function verificar(email) {
    var resul = email;
    $("#modalVerificar").modal("show");
};
