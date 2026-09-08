namespace BusData.Models;

public class TransitAnalyticsDto
{
    public string TimeLabel {get; set;} = string.Empty;
    public int Hour {get; set;}
    public double? AvgDurationMinutes {get; set;}
}