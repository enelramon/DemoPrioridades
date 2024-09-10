using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using RegistroPrioridades.Models;

namespace RegistroPrioridades.Models;

public partial class UserProfiles
{
    [Key]
    public int ProfileId { get; set; }

    public int UserId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(255)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    public string? Biography { get; set; }

    public int? YearsOfExperience { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? DesiredSalary { get; set; }

    [StringLength(10)]
    public string? Currency { get; set; }

    public int? AvailabilityId { get; set; }

    public int? WorkTypeId { get; set; }

    [StringLength(255)]
    public string? GitHubProfile { get; set; }

    [StringLength(255)]
    public string? LinkedInProfile { get; set; }

    [StringLength(255)]
    public string? PortfolioUrl { get; set; }

    [ForeignKey("AvailabilityId")]
    [InverseProperty("UserProfiles")]
    public virtual Availability? Availability { get; set; }

    [InverseProperty("Profile")]
    public virtual ICollection<Certifications> Certifications { get; set; } = new List<Certifications>();

    [InverseProperty("Profile")]
    public virtual ICollection<Education> Education { get; set; } = new List<Education>();

    [InverseProperty("Profile")]
    public virtual ICollection<JobApplications> JobApplications { get; set; } = new List<JobApplications>();

    [ForeignKey("UserId")]
    [InverseProperty("UserProfiles")]
    public virtual Users User { get; set; } = null!;

    [InverseProperty("Profile")]
    public virtual ICollection<UserLanguages> UserLanguages { get; set; } = new List<UserLanguages>();

    [InverseProperty("Profile")]
    public virtual ICollection<UserSkills> UserSkills { get; set; } = new List<UserSkills>();

    [InverseProperty("Profile")]
    public virtual ICollection<WorkExperience> WorkExperience { get; set; } = new List<WorkExperience>();

    [ForeignKey("WorkTypeId")]
    [InverseProperty("UserProfiles")]
    public virtual PreferredWorkType? WorkType { get; set; }
}
