$(document).ready(function () {    

    // Get Route Distance Form
    $('#get-route-distance_form').submit(function (e) {  
        var routes = GenerateRoutes();        
        var route = $('#get-route-distance_route').val().toUpperCase();

        var data = {
            routes: routes,
            route: route
        }        

        $.ajax({
            type: 'POST',
            contentType: "application/json",
            url: 'https://localhost:5001/api/v1/trains/GetRouteDistance',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (response) {
                $('.get-route-distance_response').html('<p>Distance: ' + response.distance + '</p>')                
            },
            error: function (response) {
                console.log(response);
                $('.get-route-distance_response').html('<p class="error">Error: ' + response.responseText + '</p>')
            },
        });

        e.preventDefault();
    });

    // Get Shortest Distance Between Towns Form
    $('#get-shortest-distance-between-towns-by-id_form').submit(function (e) {
        var routes = GenerateRoutes();
        var startTownId = $('#get-shortest-distance-between-towns-by-id_start-town').val().toUpperCase();
        var destinationTownId = $('#get-shortest-distance-between-towns-by-id_destination-town').val().toUpperCase();        

        var data = {
            routes: routes,
            startTownId: startTownId,
            destinationTownId: destinationTownId            
        }

        $.ajax({
            type: 'POST',
            contentType: "application/json",
            url: 'https://localhost:5001/api/v1/trains/GetShortestDistanceBetweenTownsById',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (response) {
                $('.get-shortest-distance-between-towns-by-id_response').html('<p>Routes: ' + response.distance + '</p>')
            },
            error: function (response) {
                console.log(response);
                $('.get-shortest-distance-between-towns-by-id_response').html('<p class="error">Error: ' + response.responseText + '</p>')
            },
        });

        e.preventDefault();
    });

    // Get Routes By Stops Form
    $('#get-number-0f-routes-between-towns-by-stop_form').submit(function (e) {
        var routes = GenerateRoutes();
        var startTownId = $('#get-number-0f-routes-between-towns-by-stop_start-town').val().toUpperCase();
        var destinationTownId = $('#get-number-0f-routes-between-towns-by-stop_destination-town').val().toUpperCase();
        var stopCount = $('#get-number-0f-routes-between-towns-by-stop_stop-count').val();
        var limitType = $('#get-number-0f-routes-between-towns-by-stop_limit-type').val();

        var data = {
            routes: routes,
            startTownId: startTownId,
            destinationTownId: destinationTownId,
            stopCount: stopCount,
            limitType: limitType
        }

        $.ajax({
            type: 'POST',
            contentType: "application/json",
            url: 'https://localhost:5001/api/v1/trains/GetNumberOfRoutesBetweenTownsByStop',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (response) {
                $('.get-number-0f-routes-between-towns-by-stop_response').html('<p>Routes: ' + response.numberOfStops + '</p>')
            },
            error: function (response) {
                console.log(response);
                $('.get-number-0f-routes-between-towns-by-stop_response').html('<p class="error">Error: ' + response.responseText + '</p>')
            },
        });

        e.preventDefault();
    });

    // Get Routes By Distance Form
    $('#get-number-0f-routes-between-towns-by-distance_form').submit(function (e) {
        var routes = GenerateRoutes();
        var startTownId = $('#get-number-0f-routes-between-towns-by-distance_start-town').val().toUpperCase();
        var destinationTownId = $('#get-number-0f-routes-between-towns-by-distance_destination-town').val().toUpperCase();
        var distance = $('#get-number-0f-routes-between-towns-by-distance_distance').val();
        var limitType = $('#get-number-0f-routes-between-towns-by-distance_limit-type').val();

        var data = {
            routes: routes,
            startTownId: startTownId,
            destinationTownId: destinationTownId,
            distance: distance,
            limitType: limitType
        }

        $.ajax({
            type: 'POST',
            contentType: "application/json",
            url: 'https://localhost:5001/api/v1/trains/GetNumberOfRoutesBetweenTownsByDistance',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (response) {
                $('.get-number-0f-routes-between-towns-by-distance_response').html('<p>Routes: ' + response.numberOfStops + '</p>')
            },
            error: function (response) {
                console.log(response);
                $('.get-number-0f-routes-between-towns-by-distance_response').html('<p class="error">Error: ' + response.responseText + '</p>')
            },
        });

        e.preventDefault();
    });
});

function GenerateRoutes() {
    var result = [];
    var routeData = $('#route-data').val().toUpperCase();

    if (!routeData || routeData === '') {
        $('.route-data-error').html('<p class="error">Invalid/missing route data</p>');
        return false;
    }    

    var routeArray = routeData.split(',');

    if (routeArray <= 1) {
        $('.route-data-error').html('<p class="error">Invalid/missing route data</p>');
        return false;
    }    

    if (routeArray[0].length === 3) {
        $(routeArray).each(function (index, element) {
            result.push({ "startTownId": element.trim().split("")[0], "destinationTownId": element.trim().split("")[1], "distance": element.trim().split("")[2] })
        });
    }
    else {
        $(routeArray).each(function (index, element) {
            result.push({ "startTownId": element.trim().split("|")[0], "destinationTownId": element.trim().split("|")[1], "distance": element.trim().split("|")[2] })
        });
    }

    $('.route-data-error').html('');

    return result;
}