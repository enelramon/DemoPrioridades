using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

public partial class Certifications
{
    [Key]
    public int CertificationId { get; set; }

    public int? ProfileId { get; set; }

    [StringLength(255)]
    public string CertificationName { get; set; } = null!;

    [StringLength(255)]
    public string? IssuingOrganization { get; set; }

    public DateOnly? IssueDate { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    [StringLength(100)]
    public string? CredentialId { get; set; }

    [StringLength(255)]
    public string? CredentialUrl { get; set; }

    [ForeignKey("ProfileId")]
    [InverseProperty("Certifications")]
    public virtual UserProfiles? Profile { get; set; }
}
