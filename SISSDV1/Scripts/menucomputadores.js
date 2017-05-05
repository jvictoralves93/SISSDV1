$(function () {
    $.contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
            if (key == "Resetar Host") {
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
            "Resetar Host": { name: "Resetar Host", icon: "reset" },
            "Excluir": { name: "Excluir", icon: "delete" },
            "Detalhes": { name: "Detalhes", icon: "details" },
        }
    });
    
});