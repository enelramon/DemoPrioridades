using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

public partial class UserSkills
{
    [Key]
    public int UserSkillId { get; set; }

    public int? ProfileId { get; set; }

    public int? SkillId { get; set; }

    public int? SkillLevel { get; set; }

    public int? YearsOfExperience { get; set; }

    public bool IsPrimary { get; set; }

    [ForeignKey("ProfileId")]
    [InverseProperty("UserSkills")]
    public virtual UserProfiles? Profile { get; set; }

    [ForeignKey("SkillId")]
    [InverseProperty("UserSkills")]
    public virtual Skills? Skill { get; set; }
}
