let dgram = require('dgram')
let cpusNo = require('os').cpus().length
let cluster = require("cluster")
let faker = require("faker")
let v4 = require('uuid')
const { random } = require('faker')

// speed gov available in the database
const speedgov = ["12345", "12346", "12347", "12348", "12349"]

const location = [
        [36.969156, -0.414748],
        [36.992682, -0.399055],
        [37.041297, -0.439846],
        [37.071093, -0.464944],
        [37.136956, -0.484559],

        [37.169104, -0.516706],

        [37.186353, -0.557478],

        [37.200462, -0.600602],

        [37.200545, -0.627383],

        [37.196101, -0.645883],

        [37.216086, -0.666604],

        [37.243474, -0.667345],

        [37.275698, -0.744253],

        [37.262650, -0.809425],

        [37.261022, -0.810559],

        [37.230341, -0.830738],

        [37.183957, -0.925606],

        [37.117191, -0.943020],

        [37.062056, -1.032968],

        [37.022884, -1.093901],

        [36.951790, -1.166445],

        [36.848763, -1.257873],

        [36.821187, -1.297068]
]

class Location {
    constructor(lat, long, gpscourse, speedGov) {
        this.Latitude = lat
        this.Long = long
        this.Time = Date.now()
        this.GpsCourse = gpscourse
        this.EngineON = true
        this.SpeedSignalStatus = true
        this.Speed = 45
        this.SpeedGovId = speedGov
    }
}

var ownerId = v4.v1();
const fakeLocation = new Location(faker.address.latitude(), faker.address.longitude(), ownerId, speedgov[Math.floor(Math.random() * speedgov.length)])

if (cluster.isMaster) {
    cluster.fork();
  
} else {
    sendNewLocation(fakeLocation)
}

async function sendNewLocation(fakeLocation) {
    var client = dgram.createSocket("udp4")

    async function send(client, location) {
        let imei = 451282484;
        var date = new Date()
        const loc = `b"b'GVNR,KCU 808H,${imei},0.009,${0},${1},${location.Latitude},${date.toDateString()},${date.toTimeString().toString()},${location.Long},${4.232},${0}"`;
        console.log(loc)
        try {
            client.send(loc)
        } catch (error) {
            console.log(error)
        }
    }

    try {
        await client.connect("3030", "localhost")
        setInterval(async () => {
            await send(client, fakeLocation)
        }, 3000);

    } catch (error) {
        console.log(error)
    }
}