const routes = [
    { "startTownId": "A", "destinationTownId": "B", "distance": 5 },
    { "startTownId": "B", "destinationTownId": "C", "distance": 4 },
    { "startTownId": "C", "destinationTownId": "D", "distance": 8 },
    { "startTownId": "D", "destinationTownId": "C", "distance": 8 },
    { "startTownId": "D", "destinationTownId": "E", "distance": 6 },
    { "startTownId": "A", "destinationTownId": "D", "distance": 5 },
    { "startTownId": "C", "destinationTownId": "E", "distance": 2 },
    { "startTownId": "E", "destinationTownId": "B", "distance": 3 },
    { "startTownId": "A", "destinationTownId": "E", "distance": 7 }
];

$(document).ready(function () {    

    // Get Route Distance Form
    $('#get-route-distance-form').submit(function (e) {        
        var route = $('#get-route-distance-route').val();

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
                $('.get-route-distance-response').html('<p>Distance: ' + response.distance + '</p>')                
            },
            error: function (response) {
                console.log(response);
                $('.get-route-distance-response').html('<p class="error">Error: ' + response.responseText + '</p>')
            },
        });

        e.preventDefault();
    });
});