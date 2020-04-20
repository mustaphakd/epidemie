
/**
 * generates geo_nodes.sql for insertion into posgresSql.
 * we left out about 5 countries that were already loaded manually in the DB 
 * 4 of these countries are:
 * 
values('Ivory Coast', 'CIV', 'west'
values('Cameroon', 'CMR', 'central'
values('Democratic Republic of the Congo'
values('Republic of Congo', 'COG', 'central',

 */

const fs = require("fs");
const readln = require("readline");


const sourceFilePath = "countries_geo.json";
const destFilePath = "geo_nodes.sql";

const fileExist = fs.existsSync(sourceFilePath);

if (fileExist === false)
{
    console.log("File does not exist. Exiting.");
}


const outputFileStream = fs.createWriteStream(destFilePath);

const fileContent = fs.readFileSync(sourceFilePath, {encoding: "utf8"}); // fileStream.read();
console.log("File content: ", fileContent);
const json = JSON.parse(fileContent);

console.dir(json);
const jsonLength = json.length;

for (let  i = 0; i < jsonLength; i++)
{
    const item = json[i];
    const processedLine = processLine(item);
    console.log("lineReader.On(... writing processed line to writestream.");
    outputFileStream.write(processedLine + "\n", "utf8");
    
}

outputFileStream.end();

function processLine(line)
{
    console.log('Process line start', line);
    const geofeature = line; // JSON.parse(line);
    console.log("Process line jsonparsed line: ");
    console.dir(geofeature);

    const properties = geofeature.properties;
    console.log("Process line jsonparsed properties: ");
    console.dir(properties);
    const type = geofeature.type;
    console.log("Process line jsonparsed type: ");
    console.dir(type);
    const geometry = geofeature.geometry;
    console.log("Process line jsonparsed geometry: ");
    console.dir(geometry);

    
    if(properties === undefined || type === undefined || geometry === undefined)
    {
        console.error("Process line jsonparsed geometry - properties, type or geometry is undefined. ");
        return;
    }

    const region = properties.region;
    const name = properties.ADMIN;
    const isoName = properties.ISO_A3;

    if(region === undefined || name === undefined || isoName === undefined)
    {
        console.error("Process line jsonparsed geometry - region, name or isoName is undefined. ");
        return;
    }

    console.log("Process line jsonparsed region: ", region);
    console.log("Process line jsonparsed name: ", name);
    console.log("Process line jsonparsed isoName: ", isoName);
    const stringfiedGeometry = JSON.stringify(geometry).replace("\\", "");
    console.log("\n\nProcess line jsonparsed stringfiedGeometry: ", stringfiedGeometry);

    //double check output to make sure there is not any double single quotes at the beginning of geojson
    //https://postgis.net/docs/ST_GeomFromGeoJSON.html
    //https://postgis.net/docs/ST_AsGeoJSON.html
    let newLine = `values('${name}', '${isoName}', '${region}', ST_GeomFromGeoJSON('${stringfiedGeometry}'))`;

    console.log('Process line End');
    return newLine;

}