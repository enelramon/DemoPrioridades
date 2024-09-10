using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

public partial class JobOffers
{
    [Key]
    public int JobOfferId { get; set; }

    public int CompanyId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? SalaryRangeMin { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? SalaryRangeMax { get; set; }

    [StringLength(10)]
    public string? Currency { get; set; }

    [StringLength(50)]
    public string? JobType { get; set; }

    public int? WorkTypeId { get; set; }

    public DateTime PostedDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("CompanyId")]
    [InverseProperty("JobOffers")]
    public virtual CompanyProfiles Company { get; set; } = null!;

    [InverseProperty("JobOffer")]
    public virtual ICollection<JobApplications> JobApplications { get; set; } = new List<JobApplications>();

    [InverseProperty("JobOffer")]
    public virtual ICollection<JobSkillRequirements> JobSkillRequirements { get; set; } = new List<JobSkillRequirements>();

    [ForeignKey("WorkTypeId")]
    [InverseProperty("JobOffers")]
    public virtual PreferredWorkType? WorkType { get; set; }
}
