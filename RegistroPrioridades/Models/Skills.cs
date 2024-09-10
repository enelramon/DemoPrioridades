using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

[Index("SkillName", Name = "UQ__Skills__B63C657179F210A0", IsUnique = true)]
public partial class Skills
{
    [Key]
    public int SkillId { get; set; }

    [StringLength(100)]
    public string SkillName { get; set; } = null!;

    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Skills")]
    public virtual SkillCategory? Category { get; set; }

    [InverseProperty("Skill")]
    public virtual ICollection<JobSkillRequirements> JobSkillRequirements { get; set; } = new List<JobSkillRequirements>();

    [InverseProperty("Skill")]
    public virtual ICollection<UserSkills> UserSkills { get; set; } = new List<UserSkills>();
}
