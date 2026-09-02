using System.Text.Json.Serialization;

namespace BusData.Models;

// DTO = data transfer object; map json data into memory for each vehicle
public class VehicleDto
{
    [JsonPropertyName("GroundSpeed")]
    public double Speed {get; set;}

    [JsonPropertyName("Latitude")]
    public double Latitude {get; set;}

    [JsonPropertyName("Longitude")]
    public double Longitude {get; set;}

    [JsonPropertyName("Name")]
    public string VehicleName {get; set;} = string.Empty;

    [JsonPropertyName("RouteID")]
    public int RouteID {get; set;}

    [JsonPropertyName("VehicleID")]
    public int VehicleID {get; set;}
}

