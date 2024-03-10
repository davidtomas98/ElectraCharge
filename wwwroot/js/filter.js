$(document).ready(function () {
    var valorAnterior = ''; // Variable para almacenar el valor anterior del campo de búsqueda

    $('#txtBusqueda').keyup(function () {
        var filtro = $('#filtro').val();
        var valor = $(this).val().trim(); // Trim para eliminar espacios en blanco

        // Realizar la solicitud AJAX para filtrar cargadores si el valor cambia y no está vacío
        if (valor !== valorAnterior && valor !== "") {
            $.ajax({
                url: '/Cargadores/FiltrarCargadores',
                type: 'GET',
                data: { filtro: filtro, valor: valor },
                success: function (data) {
                    mostrarCargadores(data.cargadores);
                }
            });
        } else if (valor === "") { 
            obtenerListaCargadoresCompleta();
        }

        // Actualizar el valor anterior del campo de búsqueda
        valorAnterior = valor;
    });

    function obtenerListaCargadoresCompleta() {
        $.ajax({
            url: '/Cargadores/Index2',
            type: 'GET',
            data: { pagina: 1, cantidadPorPagina: 10 },
            success: function (data) {
                mostrarCargadores(data.cargadores);
            }
        });
    }

    function mostrarCargadores(cargadores) {
        // Limpiar la tabla de cargadores
        $('#tablaCargadores').empty();

        // Iterar sobre los cargadores y agregarlos a la tabla
        $.each(cargadores, function (index, cargador) {
            var estadoBadge = cargador.estado == "disponible" ? '<span class="badge rounded-pill bg-success">Disponible</span>' : '<span class="badge rounded-pill bg-danger">No disponible</span>';
            var row = '<tr>' +
                '<td>' + cargador.id + '</td>' +
                '<td>' + cargador.marca + '</td>' +
                '<td>' + cargador.potencia + '</td>' +
                '<td>' + cargador.ubicacion + '</td>' +
                '<td>' + estadoBadge + '</td>' +
                '<td>' +
                '<a href="/Cargadores/Editar/' + cargador.id_cargador + '" class="btn btn-outline-primary"><i class="bi bi-pencil-fill"></i></a>' +
                '<a href="/Cargadores/Eliminar/' + cargador.id_cargador + '" class="btn btn-outline-danger"><i class="bi bi-trash-fill"></i></a>' +
                '</td>' +
                '</tr>';
            $('#tablaCargadores').append(row);
        });
    }
});
