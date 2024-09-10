using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace RegistroPrioridades.Models;

[Index("AvailabilityType", Name = "UQ__Availabi__AF78582AF5049A9C", IsUnique = true)]
public partial class Availability
{
    [Key]
    public int AvailabilityId { get; set; }

    [StringLength(50)]
    public string AvailabilityType { get; set; } = null!;

    [InverseProperty("Availability")]
    public virtual ICollection<UserProfiles> UserProfiles { get; set; } = new List<UserProfiles>();
}
