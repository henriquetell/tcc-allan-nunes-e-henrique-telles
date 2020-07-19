var map;
function initMap() {
    let element = document.getElementById("map");
    if (element) {
        var states =
        {
              AC: { lat: -9.98, lng: -67.86 }
            , AL: { lat: -9.59, lng: -35.75 }
            , AM: { lat: -3.04, lng: -60.03 }
            , AP: { lat: 0.10, lng: -51.16 }
            , BA: { lat: -12.90, lng: -38.49 }
            , CE: { lat: -3.78, lng: -38.58 }
            , DF: { lat: -15.72, lng: -48.00 }
            , ES: { lat: -20.28, lng: -40.32 }
            , GO: { lat: -16.69, lng: -49.37 }
            , MA: { lat: -2.62, lng: -44.43 }
            , MT: { lat: -15.41, lng: -56.16 }
            , MS: { lat: -20.48, lng: -54.70 }
            , MG: { lat: -19.90, lng: -44.03 }
            , PA: { lat: -3.32, lng: -53.36 }
            , PB: { lat: -7.14, lng: -34.95 }
            , PR: { lat: -25.49, lng: -51.55 }
            , PE: { lat: -8.38, lng: -37.86 }
            , PI: { lat: -6.60, lng: -42.28 }
            , RJ: { lat: -22.25, lng: -42.66 }
            , RN: { lat: -5.81, lng: -36.59 }
            , RO: { lat: -10.83, lng: -63.34 }
            , RS: { lat: -30.17, lng: -53.50 }
            , RR: { lat: 1.99, lng: -61.33 }
            , SC: { lat: -27.45, lng: -50.95 }
            , SE: { lat: -10.57, lng: -37.45 }
            , SP: { lat: -22.19, lng: -48.79 }
            , TO: { lat: -9.46, lng: -48.26 }
        };

        map = new google.maps.Map(element, {
            center: { lat: -15.721, lng: -48.007 },
            zoom: 5
        });

        var marker, i;
        for (var prop in states) {
            let pos = states[prop];
            let state = prop;
            marker = new google.maps.Marker({ position: pos, map: map });
            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {
                    map.setZoom(8);
                    map.setCenter(pos);
                    initChart(state);
                }
            })(marker, i));
        }
    }
}

function initChart(state) {
    $.ajax({
        url: `https://citiesenvironmentalassessmentmodule.azurewebsites.net/api/RatingState?state=${state}`,
        method: 'GET',
        success: function (resp) {
            var config = {
                type: 'pie',
                data: {
                    datasets: [{
                        data: [
                            resp.avgQuality,
                            resp.avgPolitical,
                            resp.avgCrimes,
                        ],
                        backgroundColor: [
                            window.chartColors.yellow,
                            window.chartColors.green,
                            window.chartColors.blue,
                        ],
                        label: state
                    }],
                    labels: [
                        'Qualidade',
                        'Políticas',
                        'Crimes Ambientais'
                    ]
                },
                options: {
                    responsive: true
                }
            };
            var ctx = document.getElementById('chart-area').getContext('2d');
            if (window.myPie !== undefined)
                window.myPie.destroy();
            window.myPie = new Chart(ctx, config);
            $('#resultModal').modal('show');
        },
        error: function () {
            alert(err.responseJSON.error);
        }
    });
}
