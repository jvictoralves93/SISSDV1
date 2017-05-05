$(document).ready(function () {
    /* initialize the external events
	-----------------------------------------------------------------*/
    $('#external-events .fc-event').each(function () {
        // store data so the calendar knows to render an event upon drop
        $(this).data('event', {
            title: $.trim($(this).text()), // use the element's text as the event title
            stick: true // maintain when user navigates (see docs on the renderEvent method)
        });
        // make the event draggable using jQuery UI
        $(this).draggable({
            zIndex: 999,
            revert: true,      // will cause the event to go back to its
            revertDuration: 0  //  original position after the drag
        });
    });
    /* initialize the calendar
	-----------------------------------------------------------------*/
    var zone = "-03:00";
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay,list'
        },
        editable: true,
        droppable: true, // this allows things to be dropped onto the calendar
        drop: function () {
            // is the "remove after drop" checkbox checked?
            if ($('#drop-remove').is(':checked')) {
                // if so, remove the element from the "Draggable Events" list
                $(this).remove();
            }
        },
        events: "BuscarEventos",
        eventReceive: function (event) {
            var title = event.title;
            var start = event.start.format("YYYY-MM-DD[T]HH:MM:SS");
            $.ajax({
                url: "CriarEvento",
                type: "POST",
                dataType: 'json',
                data: { title, start },
                success: function (response) {
                    event.id = response.eventid;
                    $('#calendar').fullCalendar('updateEvent', event);
                },
                error: function (e) {
                    console.log(e.responseText);
                }
            });
            $('#calendar').fullCalendar('updateEvent', event);
        },
        eventDrop: function (event, delta, revertFunc) {
            var title = event.title;
            var start = event.start.format("YYYY-MM-DD[T]HH:MM:SS");
            u$.ajax({
                url: 'AlterarEvento',
                data: { title, start },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status != 'success')
                        revertFunc();
                },
                error: function (e) {
                    revertFunc();
                    alert('Error processing your request: ' + e.responseText);
                }
            });
        }
    });

});

