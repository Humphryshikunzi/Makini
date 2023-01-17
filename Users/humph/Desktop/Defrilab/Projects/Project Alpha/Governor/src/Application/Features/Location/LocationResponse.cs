namespace _.Application.Features.Location
{
  public class LocationResponse
  {
    public string Time { get; set; }
    public string Date { get; set; } 
    public string Speed { get; set; }
    public double Latitude { get; set; }
    public double Long { get; set; }
    public string GpsCourse { get; set; }
    public string SpeedSignalStatus { get; set; }
    public string EngineON { get; set; }
    public int SpeedGovId { get; set; }
  }
}
