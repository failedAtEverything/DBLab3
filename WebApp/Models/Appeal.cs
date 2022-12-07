using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class Appeal
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Organization { get; set; }

    public string? AppealPurpose { get; set; }

    public int ProgramId { get; set; }

    public virtual WeeklyProgramList Program { get; set; } = null!;
}
