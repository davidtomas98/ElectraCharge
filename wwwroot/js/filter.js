$(document).ready(function () {
    $('#txtBusqueda').on('keyup', function () {
        filtrarTabla();
    });

    $('#filtro').on('change', function () {
        filtrarTabla();
    });

    function filtrarTabla() {
        var filtro = $('#filtro').val();
        var valor = $('#txtBusqueda').val().toLowerCase();

        $('#tablaCargadores tr').filter(function () {
            var columna = $(this).find('td').eq(filtro === 'potencia' ? 2 : 1).text().toLowerCase();
            $(this).toggle(columna.indexOf(valor) > -1);
        });
    }
});
