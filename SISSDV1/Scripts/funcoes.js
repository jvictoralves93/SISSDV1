//Evita do usuário enviar o formulário antes de preencher alguns campos obrigatórios
function Click() {
    document.addEventListener('keypress', function (e) {
        if (e.which == 13) {
            Buscar(); BuscarComputador(); BuscarGrupo();
        }
    }, false);
};


function digitando() {
    $('#salvar').attr('disabled', 'disabled');
};

//Busca os Usuários no RM
function BuscarFuncionariosRM() {
    $.ajax(
    {
        type: 'POST',
        url: 'Pesquisar',
        dataType: 'html',
        cache: false,
        async: true,
        beforeSend: function () {
            //Aqui adiciona o loader
            $("#Resultado").html("<tr><td class='text-center' colspan='11'><img src='../img/load.gif' height='60' widith='60'></td></tr>");
        },
        success: function (data) {
            $('#Resultado').html(data);
            $("#grid-basic").bootgrid();
        }
    });
};

//Busca os Usuários no AD
function Buscar() {
    var usuarios = $('#usuarios').val();
    $.ajax(
    {
        type: 'POST',
        url: 'Procurar',
        data: { usuarios },
        dataType: 'html',
        cache: false,
        async: true,
        beforeSend: function () {
            //Aqui adiciona o loader
            $("#Resultado").html("<tr><td class='text-center' colspan='4'><img src='../img/load.gif' height='60' widith='60'></td></tr>");
        },
        success: function (data) {
            $('#Resultado').html(data);
            $("#grid-basic").bootgrid({
                //formatters: {
                //    "commands": function (column, row) {
                //        return "<button id=" + row.Email + " type='button' class='btn btn-xs btn-primary command-edit item' onclick='editar();'><span class='ion-edit'></span></button> " +
                //            "<button id=" + row.Email + " type='button' class='btn btn-xs btn-warning command-edit item' onclick='detalheModal(" + $(this).attr("id") + ");'><span class='ion-information'></span></button>" +
                //            "<button id=" + row.Email + " type='button' class='btn btn-xs btn-danger command-edit item' onclick='modalreset();'><span class='ion-key'></span></button>";
                //            "<button id=" + row.Email + " type='button' class='btn btn-xs btn-danger command-edit item' onclick='verificar();'><span class='ion-key'></span></button>";
                //    }
                //}
            });
        }
    });
};
//Busca os computadores no AD
function BuscarComputador() {
    var computadores = $('#computadores').val();
    $.ajax(
    {
        type: 'POST',
        url: 'Procurar',
        data: { computadores },
        dataType: 'html',
        cache: false,
        async: true,
        beforeSend: function () {
            //Aqui adiciona o loader
            $("#Resultadocomputer").html("<tr><td class='text-center' colspan='3'><img src='../img/load.gif' height='60' widith='60'></td></tr>");
        },
        success: function (data) {
            $('#Resultadocomputer').html(data);
        }
    });
};
//Busca os grupos no AD
function BuscarGrupo() {
    var grupos = $('#grupos').val();
    $.ajax(
    {
        type: 'POST',
        url: 'Procurar',
        data: { grupos },
        dataType: 'html',
        cache: false,
        async: true,
        beforeSend: function () {
            //Aqui adiciona o loader
            $("#ResultadoGrupo").html("<tr><td class='text-center' colspan='3'><img src='../img/load.gif' height='60' widith='60'></td></tr>");
        },
        success: function (data) {
            $('#ResultadoGrupo').html(data);
        }
    });
};

//Filtra as cidades
function FiltrarCidade() {
    $.ajax({
        url: '/Users/Cidades',
        type: "POST",
        dataType: "JSON",
        success: function (cities) {
            $("#cidade").html(""); // Limpar campo antes de preencher
            $.each(cities, function (i, item) {
                $("#cidade").append(
                $('<option></option>').val(item.Cidade).html(item.Cidade));
                FiltrarUnidade();
            });
        }
    });
};
//Filtra as unidades por cidade
function FiltrarUnidade() {
    var cidade = $('#cidade').val();
    $.ajax({
        url: '/Users/Unidades',
        type: "POST",
        dataType: "JSON",
        data: { cidade },
        success: function (cities) {
            $("#unidade").html(""); // Limpar campo antes de preencher
            $.each(cities, function (i, item) {
                $("#unidade").append(
                $('<option></option>').val(item.Unidade).html(item.Unidade));
                FiltrarDepartamento();
            });
        }
    });
};
//Filtra os departamentos por unidade
function FiltrarDepartamento() {
    var cidade = $('#cidade').val();
    var unidade = $('#unidade').val();
    $.ajax({
        url: '/Users/Departamento',
        type: "POST",
        dataType: "JSON",
        data: { cidade, unidade },
        success: function (cities) {
            $("#departamento").html(""); // Limpar campo antes de preencher 
            $.each(cities, function (i, item) {
                $("#departamento").append(
                $('<option></option>').val(item.Departamento).html(item.Departamento));
            });
        }
    });
};
//Verifica se o usuário já existe
function VerificarUsuario() {
    var usuario = $('#Usuario').val();
    $.ajax({
        url: 'VerificaUser',
        type: "POST",
        dataType: "JSON",
        data: { usuario },
        success: function (data) {
            if (data == "") {
                $('#validacao').html("");
                $('#salvar').removeAttr('disabled');
            } else {
                $('#validacao').html("<p class = 'text-danger'> Este Nome de Usuário já existe </p>");
                $('#salvar').attr('disabled', 'disabled');
            }
        }
    });
};

//Verifica se o hostname já existe
function VerificarHost() {
    var hostname = $('#hostname').val();
    $.ajax({
        url: 'VerificaHostname',
        type: "POST",
        dataType: "JSON",
        data: { hostname },
        success: function (data) {
            if (data == "") {
                $('#validacao').html("");
                $('#salvar').removeAttr('disabled');
            } else {
                $('#validacao').html("<p class = 'text-danger'> Este Computador já existe </p>");
                $('#salvar').attr('disabled', 'disabled');
            }
        }
    });
};
//Busca Um tecnico
function BuscarTecnico() {
    var tecnico = $('#tecnico').val();
    $.ajax({
        url: '/Computadores/BuscaTecnico',
        type: "POST",
        dataType: "JSON",
        data: { tecnico},
        success: function (cities) {
            $('#tecnico').val(cities.NomeExibicao);
            $('#salvar').removeAttr('disabled');
        }
    });
};
//Cria um computador
$('#form-computador').submit(function CriarComputador() {
    var hostname = $('#hostname').val();
    var tecnico = $('#tecnico').val();
    var cidade = $('#cidade').val();
    var unidade = $('#unidade').val();
    var departamento = $('#departamento').val();
    $.ajax({
        url: 'CriarComputador',
        type: "POST",
        dataType: "JSON",
        data: { hostname, tecnico, cidade, unidade, departamento },
        success: function () {
            $("#modalSucesso").modal("show");
            limparCampos();
        }
    });
    return false;
});
//Cria um usuário
$('#form-user').submit(function CriarUsuario() {
    var nome = $('#nome').val();
    var sobrenome = $('#sobrenome').val();
    var username = $('#Usuario').val();
    var cargo = $('#cargo').val();
    var subdepartamento = $('#subdepartamento').val();
    var telefone = $('#telefone').val();
    var cpf = $('#cpf').val();
    var chapa = $('#chapa').val();
    var senha = $('#senha').val();
    var cidade = $('#cidade').val();
    var unidade = $('#unidade').val();
    var departamento = $('#departamento').val();
    var diainicio = $('#diainicio').val();
    var diafim = $('#diafim').val();
    var horarioinicio = $('#horarioinicio').val();
    var horariofim = $('#horariofim').val();
    $.ajax({
        url: 'CriarUsuario',
        type: "POST",
        dataType: "JSON",
        data: { nome, sobrenome, username, diainicio, diafim, horarioinicio, horariofim, cargo, subdepartamento, telefone, cpf, chapa, senha, cidade, unidade, departamento },
        success: function () {
            $("#modalSucesso").modal("show");
            limparCampos();
        }
    });
    return false;
});
//Limpa os campos
function limparCampos() {
    $('#nome').val("");
    $('#sobrenome').val("");
    $('#Usuario').val("");
    $('#cargo').val("");
    $('#subdepartamento').val("");
    $('#telefone').val("");
    $('#cpf').val("");
    $('#chapa').val("");
    $('#senha').val("");
    $('#confsenha').val("");
    $('#hostname').val("");
    $('#tecnico').val("");
};

//Detalhes
function Detalhes(resul) {
    var email = resul;
    $.ajax(
    {
        type: 'POST',
        url: 'DetalhesUser',
        data: { email },
        dataType: 'JSON',
        cache: false,
        async: true,
        beforeSend: function () {
            //Aqui adiciona o loader
            $("#nomeresult").html("<img src='../img/load.gif' height='30' widith='30'>");
            $('#emailresult').html("<img src='../img/load.gif' height='30' widith='30'>");
            $('#cargoresult').html("<img src='../img/load.gif' height='30' widith='30'>");
            $('#telefoneresult').html("<img src='../img/load.gif' height='30' widith='30'>");
            $('#gerenteresult').html("<img src='../img/load.gif' height='30' widith='30'>");
            $('#usuarioresult').html("<img src='../img/load.gif' height='30' widith='30'>");
            $('#paisresult').html("<img src='../img/load.gif' height='30' widith='30'>");
            $('#estadoresult').html("<img src='../img/load.gif' height='30' widith='30'>");
            $('#cidaderesult').html("<img src='../img/load.gif' height='30' widith='30'>");
            $('#unidaderesult').html("<img src='../img/load.gif' height='30' widith='30'>");

        },
        success: function (data) {
            $('#nomeresult').html(data.NomeExibicao);
            $('#emailresult').html(data.Email);
            $('#cargoresult').html(data.Cargo);
            $('#telefoneresult').html(data.Telefone);
            $('#gerenteresult').html(data.Gerente);
            $('#usuarioresult').html(data.Username);
            $('#paisresult').html(data.Pais);
            $('#estadoresult').html(data.Estado);
            $('#cidaderesult').html(data.Cidade);
            $('#unidaderesult').html(data.Escritorio);
            
        }
    });
};

//Resetar Senha
$('#form-resetsenha').submit(function NovaSenha() {
    var username = "joaoteste@sebsa.com.br";
    var password = "1234@mudar";
    $.ajax({
        url: 'ResetSenha',
        type: "POST",
        dataType: "JSON",
        data: { username, password },
        success: function () {
            $("#modalSucesso").modal("show");
        }
    });
    return false;
});

//Deletar Usuário
$('#form-deleteuser').submit(function DeleteUser() {
    var username = $('.resultado').attr('id');
    $.ajax({
        url: 'DeleteUser',
        type: "POST",
        dataType: "JSON",
        data: { hostname },
        success: function () {
            $("#modalSucesso").modal("show");
        }
    });
    return false;
});


//Resetar Host
$('#form-resethost').submit(function ResetHost() {
    var hostname = $('.resultado').attr('id');
    $.ajax({
        url: 'ResetHost',
        type: "POST",
        dataType: "JSON",
        data: { hostname },
        success: function () {
            $("#modalSucesso").modal("show");
        }
    });
    return false;
});

//Deletar Host
$('#form-deletehost').submit(function DeleteHost() {
    var hostname = $('.resultado').attr('id');
    $.ajax({
        url: 'DeleteHost',
        type: "POST",
        dataType: "JSON",
        data: { hostname },
        success: function () {
            $("#modalSucesso").modal("show");
        }
    });
    return false;
});