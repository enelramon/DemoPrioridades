using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

public partial class JobApplications
{
    [Key]
    public int ApplicationId { get; set; }

    public int JobOfferId { get; set; }

    public int ProfileId { get; set; }

    public DateTime ApplicationDate { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    public string? CoverLetter { get; set; }

    [ForeignKey("JobOfferId")]
    [InverseProperty("JobApplications")]
    public virtual JobOffers JobOffer { get; set; } = null!;

    [ForeignKey("ProfileId")]
    [InverseProperty("JobApplications")]
    public virtual UserProfiles Profile { get; set; } = null!;
}
