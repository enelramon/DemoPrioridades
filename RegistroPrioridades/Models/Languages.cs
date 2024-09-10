using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

[Index("LanguageName", Name = "UQ__Language__E89C4A6AF31DE079", IsUnique = true)]
public partial class Languages
{
    [Key]
    public int LanguageId { get; set; }

    [StringLength(100)]
    public string LanguageName { get; set; } = null!;

    [InverseProperty("Language")]
    public virtual ICollection<UserLanguages> UserLanguages { get; set; } = new List<UserLanguages>();
}
