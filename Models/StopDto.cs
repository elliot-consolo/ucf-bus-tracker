namespace BusData.Models;

public class StopDto
{
    public int StopId {get; set;}
    public string RouteId {get; set;} = string.Empty;
    public string StopName {get; set;} = string.Empty;
    public int SequenceOrder {get; set;}
    public double Latitude {get; set;}
    public double Longitude {get; set;}
    public double RadiusMeters {get; set;}
}