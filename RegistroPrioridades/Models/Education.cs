using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

public partial class Education
{
    [Key]
    public int EducationId { get; set; }

    public int? ProfileId { get; set; }

    [StringLength(255)]
    public string InstitutionName { get; set; } = null!;

    [StringLength(255)]
    public string? Degree { get; set; }

    [StringLength(255)]
    public string? FieldOfStudy { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [ForeignKey("ProfileId")]
    [InverseProperty("Education")]
    public virtual UserProfiles? Profile { get; set; }
}
