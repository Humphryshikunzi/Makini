let dgram = require('dgram')
var client = dgram.createSocket("udp4")
const location = [
    [36.969156, -0.414748]
]




let imei = 451282484;
var date = new Date()
const locationstrings = []

location.forEach((value) => {
    var locationstring = `b"b'GVNR,KCU 808H,${imei},0.009,${0},${1},${value[0]},${date.toDateString()},${date.toTimeString().toString()},${value[1]},${4.232},${0}"`
    locationstrings.push(locationstring)
}
)


client.connect("3030", "localhost")
locationstrings.forEach(location => {
    setInterval(() => {
        send(client, location)
    }, 3000);
})

async function send(client, location) {
    try {
        client.send(location)
    } catch (error) {
        console.log(error)
    }
}


