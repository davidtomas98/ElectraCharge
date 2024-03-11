$(document).ready(function () {
    var valorAnterior = ''; // Variable para almacenar el valor anterior del campo de búsqueda

    $('#txtBusqueda').keyup(function () {
        var filtro = $('#filtro').val();
        var valor = $(this).val().trim(); // Trim para eliminar espacios en blanco

        // Realizar la solicitud AJAX para filtrar uusarios si el valor cambia y no está vacío
        if (valor !== valorAnterior && valor !== "") {
            $.ajax({
                url: '/Usuarios/FiltrarUsuarios',
                type: 'GET',
                data: { filtro: filtro, valor: valor },
                success: function (data) {
                    mostrarUsuarios(data.usuarios);
                }
            });
        } else if (valor === "") { 
            obtenerListaUsuariosCompleta();
        }

        // Actualizar el valor anterior del campo de búsqueda
        valorAnterior = valor;
    });

    function obtenerListaUsuariosCompleta() {
        $.ajax({
            url: '/Usuarios/Index2',
            type: 'GET',
            data: { pagina: 1, cantidadPorPagina: 10 },
            success: function (data) {
                mostrarUsuarios(data.usuarios);
            }
        });
    }

    function mostrarUsuarios(usuarios) {
        // Limpiar la tabla de cargadores
        $('#tablaUsuarios').empty();

        // Iterar sobre los usuarios y agregarlos a la tabla
        $.each(usuarios, function (index, usuario) { var row = '<tr>' +
                '<td>' + usuario.nombre + '</td>' +
                '<td>' + usuario.apellido1 + '</td>' +
                '<td>' + usuario.apellido2 + '</td>' +
                '<td>' + usuario.correoElectronico + '</td>' +
                '<td>' +
                '<a href="/Usuarios/Editar/' + usuario.id_usuario + '" class="btn btn-outline-primary"><i class="bi bi-pencil-fill"></i></a>' +
                '<a href="/Usuarios/Eliminar/' + usuario.id_usuario + '" class="btn btn-outline-danger"><i class="bi bi-trash-fill"></i></a>' +
                '</td>' +
                '</tr>';
            $('#tablaUsuarios').append(row);
        });
    }
});
