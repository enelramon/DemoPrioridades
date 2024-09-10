using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

[Index("Email", Name = "UQ__Users__A9D1053416007C50", IsUnique = true)]
public partial class Users
{
    [Key]
    public int UserId { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(50)]
    public string UserType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<CompanyProfiles> CompanyProfiles { get; set; } = new List<CompanyProfiles>();

    [InverseProperty("User")]
    public virtual ICollection<UserProfiles> UserProfiles { get; set; } = new List<UserProfiles>();
}
