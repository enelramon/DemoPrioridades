using System.ComponentModel.DataAnnotations;

namespace RegistroPrioridades.Models;

public class Prioridades
{
    [Key]
    public int PrioridadId { get; set; }
    [Required(ErrorMessage = "El campo descripción es obligatorio.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El campo días es obligatorio.")]
    public int DiasCompromiso { get; set; }

}

