using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

public partial class CompanyProfiles
{
    [Key]
    public int CompanyId { get; set; }

    public int UserId { get; set; }

    [StringLength(255)]
    public string CompanyName { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(255)]
    public string? Website { get; set; }

    [StringLength(100)]
    public string? Industry { get; set; }

    [InverseProperty("Company")]
    public virtual ICollection<JobOffers> JobOffers { get; set; } = new List<JobOffers>();

    [ForeignKey("UserId")]
    [InverseProperty("CompanyProfiles")]
    public virtual Users User { get; set; } = null!;
}
