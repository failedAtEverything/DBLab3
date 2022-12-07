using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<WeeklyProgramList> WeeklyProgramLists { get; } = new List<WeeklyProgramList>();
}
