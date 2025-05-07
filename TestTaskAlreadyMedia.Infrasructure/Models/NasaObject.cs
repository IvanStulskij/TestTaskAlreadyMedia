using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTaskAlreadyMedia.Infrasructure.Models;

public class NasaObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public int NasaId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameType { get; set; } = string.Empty;
    public string Recclass { get; set; } = string.Empty;
    public decimal Mass { get; set; }
    public string Fall { get; set; } = string.Empty;
    public short Year { get; set; }
    public decimal Reclat { get; set; }
    public decimal Reclong { get; set; }
    public string? GeolocationType { get; set; } = string.Empty;
    public decimal[]? Coordinates { get; set; } = [];
}
