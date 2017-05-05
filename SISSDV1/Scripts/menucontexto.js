$(function () {
    $.contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
            if (key == "Resetar Senha") {
                modalreset();
            } else if (key == "Detalhes") {
                detalhes();
            } else if (key == "Excluir") {
                excluir();
            } else if (key == "Editar") {
                editar();
            }
        },
        items: {
            "Editar": { name: "Editar", icon: "edit", },
            "Resetar Senha": { name: "Resetar Senha", icon: "reset" },
            "Excluir": { name: "Excluir", icon: "delete" },
            "Detalhes": { name: "Detalhes", icon: "details" },
        }
    });

    $('.context-menu-one').on('click', function (e) {
        modalreset();
    })
});