using System;
using System.Collections.Generic;

namespace Orión.Application.DTOs;

public class GlobalActivityHeatmapDto
{
    public string DateHeader { get; set; } = string.Empty; // Ej: "01/03"
    public List<CalendarDayDto> Days { get; set; } = new(); // 7 días (Dom-Sab)
}

public class CalendarDayDto
{
    public DateTime Date { get; set; }
    public string DayName { get; set; } = string.Empty;
    public int FailureCount { get; set; }
    public string ColorLevel { get; set; } = "#EBEDF0";
}
