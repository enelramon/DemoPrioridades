using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

[Index("WorkTypeName", Name = "UQ__Preferre__4C45DD915E616D8D", IsUnique = true)]
public partial class PreferredWorkType
{
    [Key]
    public int WorkTypeId { get; set; }

    [StringLength(50)]
    public string WorkTypeName { get; set; } = null!;

    [InverseProperty("WorkType")]
    public virtual ICollection<JobOffers> JobOffers { get; set; } = new List<JobOffers>();

    [InverseProperty("WorkType")]
    public virtual ICollection<UserProfiles> UserProfiles { get; set; } = new List<UserProfiles>();
}
