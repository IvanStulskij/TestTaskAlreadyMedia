namespace TestTaskAlreadyMedia.Core.Models;

public class CommonSettings
{
    public string CheckNasaObjectsJobCronExpression { get; set; } = string.Empty;
    public int[] NasaObjectsRetriesDelaysInSeconds { get; set; } = [];
}
