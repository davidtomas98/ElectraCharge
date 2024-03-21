 function actualizarEstadoCargador(cargadorId) {
        // Realizar una solicitud AJAX POST al controlador para actualizar el estado del cargador
        $.ajax({
            type: "POST",
            url: "/Cargadores/ActualizarEstadoCargador",
            data: { cargadorId: cargadorId },
            success: function(response) {
                console.log(response);
            },
            error: function(xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    }
