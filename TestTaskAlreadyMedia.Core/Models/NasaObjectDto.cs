namespace TestTaskAlreadyMedia.Core.Models;

public class NasaObjectDto
{
    public Guid? DbId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameType { get; set; } = string.Empty;
    public string Recclass { get; set; } = string.Empty;
    public decimal Mass { get; set; }
    public string Fall { get; set; } = string.Empty;
    public DateTime Year { get; set; }
    public decimal Reclat { get; set; }
    public decimal Reclong { get; set; }
    public GeolocationDto? Geolocation { get; set; }
}

public class GeolocationDto
{
    public string Type { get; set; } = string.Empty;
    public decimal[] Coordinates { get; set; } = [];
}
