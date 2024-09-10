using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

[Index("CategoryName", Name = "UQ__SkillCat__8517B2E0AD612525", IsUnique = true)]
public partial class SkillCategory
{
    [Key]
    public int CategoryId { get; set; }

    [StringLength(100)]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Skills> Skills { get; set; } = new List<Skills>();
}
