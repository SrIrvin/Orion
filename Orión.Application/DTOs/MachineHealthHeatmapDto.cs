namespace Orión.Application.DTOs;

public class MachineHealthHeatmapDto
{
    public string IdMaquinaria { get; set; } = string.Empty;
    public string NombreMaquinaria { get; set; } = string.Empty;
    public List<DayHealthDto> Days { get; set; } = new();
}

public class DayHealthDto
{
    public DayOfWeek Day { get; set; }
    public int FailureCount { get; set; }
    public string ColorLevel { get; set; } = "#EBEDF0"; // Default empty (GitHub style)
}
