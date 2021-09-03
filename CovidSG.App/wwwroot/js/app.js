var map, mapMarker, user_location;
var tableService;
var dailyReport = [];

// General functions
function getDimensions() {
    var obj = {
        width: window.innerWidth,
        height: window.innerHeight
    };
    //console.log(obj);
    return obj;
};


//Map functions
function showMap (elementId, zoom) {
    var elem = document.getElementById(elementId);
    if (!elem) {
        throw new Error('No element with ID ' + elementId);
    }

    // Initialize map if needed
    if (!elem.map) {
        var SINGAPORE = [1.3649, 103.8229];
        //var zoom = 12;
        map = L.map(elementId);
        var tile_url = 'http://{s}.tile.osm.org/{z}/{x}/{y}.png';
        var token ="pk.eyJ1IjoiY2hvbmdrZiIsImEiOiJja3F1OTJtdzAwMjg3MnN0ZmZ6cngzYm9qIn0.GC3bMB1jLKwWsLsBvfzgSg"
        var layer = L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=' + token, {
            tap: false,
            maxZoom: 18,
            attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, ' +
                'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
            id: 'mapbox/streets-v11',
            tileSize: 512,
            zoomOffset: -1
        });
        map.addLayer(layer);
        map.setView(SINGAPORE, zoom)
        map.on('locationfound', onLocationFound);

        var lc = L.control.locate({
            //position: 'topright',
            setView: true,
            locateOptions: {
                enableHighAccuracy: true
            },
            //strings: {
            //    title: "Show me where I am, yo!"
            //}
        }).addTo(map);

        // request location update and set location
        lc.start()

        //var circle = L.circle([1.3649, 103.8229], {
        //    color: 'red',
        //    fillColor: '#f03',
        //    fillOpacity: 0.3,
        //    radius: 500
        //}).addTo(map);

        //map.locate({ setView: false, watch: false })
        //when set to true will force L.control.locate() to stop working
        // create control and add to map
        


        //map.locate({ setView: false, watch: true, enableHighAccuracy: true }) /* This will return map so you can do chaining */
        //    .on('locationfound', function (e) {
        //        if (mapMarker) map.removeLayer(mapMarker);
        //        mapMarker = L.marker([e.latitude, e.longitude]).bindPopup('You are here :)');
        //        map.addLayer(mapMarker);
        //    })
        //    .on('locationerror', function (e) {
        //        console.log(e);
        //        alert("Location access denied.");
        //    });  

        
        
        //var osmUrl = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
        //var osmAttrib = 'Map data © <a href="http://osm.org/copyright">OpenStreetMap</a> contributors';
        //var osm = new L.TileLayer(osmUrl, {
        //    attribution: osmAttrib,
        //    detectRetina: true
        //});

        //// please replace this with your own mapbox token!
        //var token = "pk.eyJ1IjoiY2hvbmdrZiIsImEiOiJja3F1OTJtdzAwMjg3MnN0ZmZ6cngzYm9qIn0.GC3bMB1jLKwWsLsBvfzgSg";
        //var mapboxUrl = 'https://api.mapbox.com/styles/v1/mapbox/streets-v10/tiles/{z}/{x}/{y}@2x?access_token=' + token;
        //var mapboxAttrib = 'Map data © <a href="http://osm.org/copyright">OpenStreetMap</a> contributors. Tiles from <a href="https://www.mapbox.com">Mapbox</a>.';
        //var mapbox = new L.TileLayer(mapboxUrl, {
        //    attribution: mapboxAttrib,
        //    tileSize: 512,
        //    zoomOffset: -1
        //});

        //var map = new L.Map(elementId, {
        //    tap: false, // ref https://github.com/Leaflet/Leaflet/issues/7255
        //    layers: [mapbox],
        //    center: SINGAPORE,
        //    zoom: zoom,
        //    zoomControl: true
        //});
        //L.control.locate().addTo(map);
    }            
}

function onLocationFound(e) {
    //var radius = e.accuracy / 2;
    //L.marker(e.latlng).addTo(map)
    //    .bindPopup("You are within " + radius + " meters from this point").openPopup();
    //L.circle(e.latlng, radius).addTo(map);
    //let count = 0
    //map.eachLayer((l) => {
    //    if (l instanceof L.CircleMarker)
    //        count++;
    //})
    //alert("total locations " + count);
    user_location = e;
}

function addLocation(latitude, longitude, msg) {
    var p = new L.latLng(latitude, longitude);
    var circle = L.circleMarker(p, {
        color: 'red',
        fillColor: 'red',
        fillOpacity: 1,
        radius: 5
    }).addTo(map);
    circle.bindPopup(msg);
}

function locateUser() {
    return user_location;
}

function initialiseTableService(account, sas)
{
    var tableUri = 'https://' + account + '.table.core.windows.net';
    tableService = AzureStorage.Table.createTableServiceWithSas(tableUri, sas);
    //tableService.listTablesSegmented(null, { maxResults: 200 }, function (error, results) {
    //    if (error) {
    //        alert('List table list error, please open browser console to view detailed error');
    //        console.log(error);
    //    } else {
    //        for (var i = 0, table; table = results.entries[i]; i++) {
    //            console.log(table);
    //        }
    //    }
    //});
}

function queryTable(table)
{
    dailyReport = [];
    var tableQuery = new AzureStorage.Table.TableQuery().top(10);
    tableService.queryEntities(table, tableQuery, null, function (error, results) {
        if (error) {
            alert('List table entities error, please open browser console to view detailed error');
            console.log(error);
        } else {            
            if (results.entries.length < 1) {
                console.log('Empty results...');
            }
            //return results;  
            //var objs = [];
            for (var i = 0, entity; entity = results.entries[i]; i++) {                
                //console.log(entity);
                obj =
                {
                    PartitionKey : entity.PartitionKey._,
                    RowKey : entity.RowKey._,
                    Count: entity.Count._,
                    Date: entity.Date._,
                    Name: entity.Name._,
                    Type : entity.Type._
                };                
                dailyReport.push(obj);
            }
            //var objList = {
            //    table : objs
            //};
        }
    });
}

function getTable()
{
    console.log(JSON.stringify(dailyReport));
    return (dailyReport);
}