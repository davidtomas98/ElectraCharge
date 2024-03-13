$(document).ready(function () {
    var valorAnterior = ''; // Variable para almacenar el valor anterior del campo de búsqueda


    $('#txtBusqueda').keyup(function () {
        var filtro = $('#filtro').val();
        var valor = $(this).val().trim(); // Trim para eliminar espacios en blanco

        // Realizar la solicitud AJAX para filtrar asignaciones si el valor cambia y no está vacío
        if (valor !== valorAnterior && valor !== "") {
            $.ajax({
                url: '/Registrar/FiltrarAsignaciones',
                type: 'GET',
                data: { filtro: filtro, valor: valor  },
                success: function (data) {
                    mostrarAsignaciones(data.asignaciones);
                }
            });
        } else if (valor === "") { 
            obtenerListaAsignacionesCompleta();
        }

        // Actualizar el valor anterior del campo de búsqueda
        valorAnterior = valor;
    });

    function obtenerListaAsignacionesCompleta() {
        $.ajax({
            url: '/Registrar/Index2',
            type: 'GET',
            data: { pagina: 1 , cantidadPorPagina : 25},
            success: function (data) {
                mostrarAsignaciones(data.asignaciones);
            }
        });
    }

    

    async function mostrarAsignaciones(asignaciones) {
        // Ordenar las asignaciones por fecha
        asignaciones.sort((a, b) => {
            const dateA = new Date(a.fecha);
            const dateB = new Date(b.fecha);
            return dateB - dateA;
        });
    
        // Limpiar la tabla de asignaciones
        $('#tablaAsignaciones').empty();
    
        // Iterar sobre las asignaciones y agregarlas a la tabla
        for (const asignacion of asignaciones) {
            // Obtener el usuario asociado a esta asignación
            const response = await fetch(`/Usuarios/ObtenerUsuario?id=${asignacion.idUsuario}`);
            const usuario = await response.json();
            const nombreUsuario = usuario != null ? `${usuario.nombre} (${usuario.correoElectronico})` : "No disponible";
    
            // Obtener la fecha en formato dd/MM/yyyy
            const fechaFormateada = asignacion.fecha.substring(8, 10) + '/' + asignacion.fecha.substring(5, 7) + '/' + asignacion.fecha.substring(0, 4);
    
            // Construir la fila de la tabla
            const row = '<tr>' +
                '<td>' + asignacion.id + '</td>' +
                '<td>' + nombreUsuario + '</td>' +
                '<td>' + asignacion.idCargador + '</td>' +
                '<td>' + asignacion.tiempo + '</td>' +
                '<td>' + fechaFormateada + '</td>' +
                '</tr>';
            
            // Agregar la fila a la tabla
            $('#tablaAsignaciones').append(row);
        }
    }
});
