using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class WeeklyProgramList
{
    public int Id { get; set; }

    public int? WeekNumber { get; set; }

    public int? WeekMonth { get; set; }

    public int? WeekYear { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public int ProgramId { get; set; }

    public int EmployeesId { get; set; }

    public string? Guests { get; set; }

    public virtual ICollection<Appeal> Appeals { get; } = new List<Appeal>();

    public virtual Employee Employees { get; set; } = null!;

    public virtual Program Program { get; set; } = null!;
}
