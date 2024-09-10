using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

public partial class WorkExperience
{
    [Key]
    public int ExperienceId { get; set; }

    public int? ProfileId { get; set; }

    [StringLength(255)]
    public string CompanyName { get; set; } = null!;

    [StringLength(255)]
    public string JobTitle { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Description { get; set; }

    [ForeignKey("ProfileId")]
    [InverseProperty("WorkExperience")]
    public virtual UserProfiles? Profile { get; set; }
}
