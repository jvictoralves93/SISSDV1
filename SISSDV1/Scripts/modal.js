$('#modalNovo').on('hidden.bs.modal', function () {
    $("#modalNovo").empty();
});

//Novo Usuário
function ModalNovoUsuario() {
    $("#modalNovo").modal("show");

    var url = "NovoUsuario";
    //debugger;
    $.ajax({
        method: "POST",
        url: url,
        cache: false,
        beforeSend: function (xhr) {
            $("#modalNovo").html('<div class="loading text-center p-40" style="position: absolute;left: 50%;top: 50%;"><img src="../img/load.gif" height="80" width="80"/></div>');
        }
    })
      .done(function (html) {
          $("#modalNovo").html(html);
      })
        .fail(function () {
            $("#modalNovo").html('<p class="error" style="position: absolute;left: 50%;top: 50%;"><strong>Opa!</strong> Não foi possível abrir a página. Tente novamente mais tarde.</p>');
        });
    $("#modalNovo").modal("show");
};

$('#modalDetalhes').on('hidden.bs.modal', function () {
    $("#modalDetalhes").empty();
});

//Detalhes Usuário
function detalheModal(email) {

    var url = "DetalhesUsuario";
    //debugger;
    $.ajax({
        method: "POST",
        url: url,
        data: { email },
        cache: false,
        beforeSend: function (xhr) {
            $("#modalDetalhes").html('<div class="loading text-center p-40" style="position: absolute;left: 50%;top: 50%;"><img src="../img/load.gif" height="80" width="80"/></div>');
        }
    })
      .done(function (html) {
          $("#modalDetalhes").html(html);
          $(".load").html('');
      })
        .fail(function () {
            $("#modalDetalhes").html('<p class="error"><strong>Opa!</strong> Não foi possível abrir a página. Tente novamente mais tarde.</p>');
        });

    $("#modalDetalhes").modal("show");
};

$('#modalReset').on('hidden.bs.modal', function () {
    $("#modalReset").empty();
});

//Resetar senha
function modalreset(email) {
    var url = "ResetarSenha";
    //debugger;
    $.ajax({
        method: "POST",
        url: url,
        data: { email },
        cache: false,
        beforeSend: function (xhr) {
            $("#modalReset").html('<div class="loading text-center p-40" style="position: absolute;left: 50%;top: 50%;"><img src="../img/load.gif" height="80" width="80"/></div>');
        }
    })
      .done(function (html) {
          $("#modalReset").html(html);
      })
        .fail(function () {
            $("#modalReset").html('<p class="error" style="position: absolute;left: 50%;top: 50%;"><strong>Opa!</strong> Não foi possível abrir a página. Tente novamente mais tarde.</p>');
        });
    $("#modalReset").modal("show");
};


//Editar Usuário
function editar(email) {
    var url = "EditarUsuario";
    //debugger;
    $.ajax({
        method: "POST",
        url: url,
        data: { email },
        cache: false,
        beforeSend: function (xhr) {
            $("#modalEdit").html('<div class="loading text-center p-40" style="position: absolute;left: 50%;top: 50%;"><img src="../img/load.gif" height="80" width="80"/></div>');
        }
    })
      .done(function (html) {
          $("#modalEdit").html(html);
      })
        .fail(function () {
            $("#modalEdit").html('<p class="error"><strong>Opa!</strong> Não foi possível abrir a página. Tente novamente mais tarde.</p>');
        });

    $("#modalEdit").modal("show");
};

$('#modalEdit').on('hidden.bs.modal', function () {
    $("#modalEdit").empty();
});

//Verificar Usuário;
function verificar(chapa) {
    $("#modalVerificar").modal("show");

    var url = "Verificar";
    //debugger;
    $.ajax({
        method: "POST",
        url: url,
        data: { chapa },
        cache: false,
        beforeSend: function (xhr) {
            $("#modalVerificar").html('<div class="loading text-center p-40" style="position: absolute;left: 50%;top: 50%;"><img src="../img/load.gif" height="80" width="80"/></div>');
        }
    })
      .done(function (html) {
          $("#modalVerificar").html(html);
      })
        .fail(function () {
            $("#modalVerificar").html('<p class="error" style="position: absolute;left: 50%;top: 50%;"><strong>Opa!</strong> Não foi possível abrir a página. Tente novamente mais tarde.</p>');
        });
    $("#modalVerificar").modal("show");

};

$('#modalVerificar').on('hidden.bs.modal', function () {
    $("#modalVerificar").empty();
});



//Computadores -------------------------------------------------------------------------
//Novo Computador
$('#modalNovoComp').on('hidden.bs.modal', function () {
    $("#modalNovoComp").empty();
});

function ModalNovoComputador() {
    $("#modalNovoComp").modal("show");

    var url = "Novo";
    //debugger;
    $.ajax({
        method: "POST",
        url: url,
        cache: false,
        beforeSend: function (xhr) {
            $("#modalNovoComp").html('<div class="loading text-center p-40" style="position: absolute;left: 50%;top: 50%;"><img src="../img/load.gif" height="80" width="80"/></div>');
        }
    })
      .done(function (html) {
          $("#modalNovoComp").html(html);
      })
        .fail(function () {
            $("#modalNovoComp").html('<p class="error" style="position: absolute;left: 50%;top: 50%;"><strong>Opa!</strong> Não foi possível abrir a página. Tente novamente mais tarde.</p>');
        });
    $("#modalNovoComp").modal("show");
};

//Resetar Host
$('#modalResetComp').on('hidden.bs.modal', function () {
    $("#modalResetComp").empty();
});
function ModalResetHost(hostname) {
    var url = "Resetar";
    //debugger;
    $.ajax({
        method: "POST",
        url: url,
        data: { hostname },
        cache: false,
        beforeSend: function (xhr) {
            $("#modalResetComp").html('<div class="loading text-center p-40" style="position: absolute;left: 50%;top: 50%;"><img src="../img/load.gif" height="80" width="80"/></div>');
        }
    })
      .done(function (html) {
          $("#modalResetComp").html(html);
      })
        .fail(function () {
            $("#modalResetComp").html('<p class="error" style="position: absolute;left: 50%;top: 50%;"><strong>Opa!</strong> Não foi possível abrir a página. Tente novamente mais tarde.</p>');
        });
    $("#modalResetComp").modal("show");
};

//Excluir Host
$('#modalDeleteComp').on('hidden.bs.modal', function () {
    $("#modalDeleteComp").empty();
});

function ModalDeleteHost(hostname) {
    var url = "Deletar";
    //debugger;
    $.ajax({
        method: "POST",
        url: url,
        data: { hostname },
                cache: false,
        beforeSend: function (xhr) {
            $("#modalDeleteComp").html('<div class="loading text-center p-40" style="position: absolute;left: 50%;top: 50%;"><img src="../img/load.gif" height="80" width="80"/></div>');
        }
    })
      .done(function (html) {
          $("#modalDeleteComp").html(html);
      })
        .fail(function () {
            $("#modalDeleteComp").html('<p class="error" style="position: absolute;left: 50%;top: 50%;"><strong>Opa!</strong> Não foi possível abrir a página. Tente novamente mais tarde.</p>');
        });
    $("#modalDeleteComp").modal("show");
};


$('#modalSucesso').on('hidden.bs.modal', function () {
    $("#modalSucesso").empty();
});

//Novo Usuário
function ModalSucesso() {
    $("#modalSucesso").modal("show");

    var url = "Sucesso";
    //debugger;
    $.ajax({
        method: "POST",
        url: url,
        cache: false,
        beforeSend: function (xhr) {
            $("#modalSucesso").html('<div class="loading text-center p-40" style="position: absolute;left: 50%;top: 50%;"><img src="../img/load.gif" height="80" width="80"/></div>');
        }
    })
      .done(function (html) {
          $("#modalSucesso").html(html);
      })
        .fail(function () {
            $("#modalSucesso").html('<p class="error" style="position: absolute;left: 50%;top: 50%;"><strong>Opa!</strong> Não foi possível abrir a página. Tente novamente mais tarde.</p>');
        });
    $("#modalSucesso").modal("show");
};