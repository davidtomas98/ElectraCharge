document.addEventListener("DOMContentLoaded", function () {
    var estado = "@Model.Estado";
    var select = document.getElementById("estado");
    for (var i = 0; i < select.options.length; i++) {
        if (select.options[i].value === estado) {
            select.options[i].selected = true;
            break;
        }
    }
});
