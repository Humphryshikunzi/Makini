"use strict"

const signalr = (map) => {

    const route =
    {
        type: "FeatureCollection",
        features: [
            {
                type: "Feature",
                geometry: {
                    type: "LineString",
                    coordinates: [
                        [
                            36.9627,
                            -0.3992
                        ]
                    ]
                }
            }
        ]
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalRHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    async function start() {
        try {
            await connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    }
    connection.onclose(async () => {
        await start();
    });

    map.on('load', function () {
        console.log("map loaded");
        map.addSource('trace', {
            type: 'geojson', data: route
        })
        map.addLayer({
            'id': 'trace',
            'type': 'line',
            'source': 'trace',
            'point': {
                'line-color': 'yellow',
                'line-opacity': 0.75,
                'line-width': 5
            }
        })
    })

    connection.on("ReceiveLocationAsync", (location, speedid) => {

        //map.jumpTo({ 'center': [location.latitude, location.long], 'zoom': 9 });
        map.panTo([location.long, location.latitude]);

        route.features[0].geometry.coordinates.push([location.long, location.latitude])
        map.getSource('trace').setData(route);
        console.log(location)
    });

    // Start the connection.
    start();
}

window.loadmap = () => {
    mapboxgl.accessToken = 'pk.eyJ1IjoiYmVubnl0cm92YXRvIiwiYSI6ImNrZDcwdTVwbTE4amEyem8yZWdkNHN3ZmoifQ.r3Llqtnwfqqju2zfzE-fvA'
    var map = new mapboxgl.Map({
        container: 'map', // container id
        style: 'mapbox://styles/mapbox/streets-v11', // style URL
        center: [36.96274, -0.39902], // starting position [lng, lat]
        zoom: 9 // starting zoom
    });

    console.log("hit ")
    signalr(map)
}



window.testfunction = (name) => {
    console.log("this is a test")
    console.log("this is a test")
    return name;
}



