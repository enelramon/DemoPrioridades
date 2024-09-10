using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

public partial class JobSkillRequirements
{
    [Key]
    public int JobSkillReqId { get; set; }

    public int? JobOfferId { get; set; }

    public int? SkillId { get; set; }

    public int? RequiredLevel { get; set; }

    public bool IsRequired { get; set; }

    [ForeignKey("JobOfferId")]
    [InverseProperty("JobSkillRequirements")]
    public virtual JobOffers? JobOffer { get; set; }

    [ForeignKey("SkillId")]
    [InverseProperty("JobSkillRequirements")]
    public virtual Skills? Skill { get; set; }
}
