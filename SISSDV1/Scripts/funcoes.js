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
        cache: true,
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
        cache: true,
        async: true,
        beforeSend: function () {
            //Aqui adiciona o loader
            $("#Resultado").html("<tr><td class='text-center' colspan='4'><img src='../img/load.gif' height='60' widith='60'></td></tr>");
        },
        success: function (data) {
            $('#Resultado').html(data);
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
        cache: true,
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
        cache: true,
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
                $('#validacao').html("<p class='text-danger'> Este Nome de Usuário já existe </p>");
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
                $('#validacao').html("<p class='text-danger'> Este Computador já existe </p>");
                $('#salvar').attr('disabled', 'disabled');
            }
        }
    });
};
//Busca Um tecnico
//function BuscarTecnico() {
//    var tecnico = $('#tecnico').val();
//    $.ajax({
//        url: '/Computadores/BuscaTecnico',
//        type: "POST",
//        dataType: "JSON",
//        data: { tecnico},
//        success: function (cities) {
//            $('#tecnico').val(cities.NomeExibicao);
//            $('#salvar').removeAttr('disabled');
//        }
//    });
//};
