using System.ComponentModel.DataAnnotations;

namespace RegistroPrioridades.Models;

public class Prioridades
{
    [Key]
    public int PrioridadId { get; set; }
    [Required(ErrorMessage = "El Campo Descripción es obligatorio")]
    public string? Descripcion { get; set; }

    public int DiasCompromiso { get; set; }

}

