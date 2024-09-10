using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

public partial class UserLanguages
{
    [Key]
    public int UserLanguageId { get; set; }

    public int? ProfileId { get; set; }

    public int? LanguageId { get; set; }

    [StringLength(50)]
    public string? ProficiencyLevel { get; set; }

    [ForeignKey("LanguageId")]
    [InverseProperty("UserLanguages")]
    public virtual Languages? Language { get; set; }

    [ForeignKey("ProfileId")]
    [InverseProperty("UserLanguages")]
    public virtual UserProfiles? Profile { get; set; }
}
