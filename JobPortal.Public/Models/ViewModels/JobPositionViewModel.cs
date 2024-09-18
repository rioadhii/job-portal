using System.ComponentModel.DataAnnotations;

namespace JobPortal.Public.Models.ViewModels;

public class JobPositionViewModel
{
    public int JobPositionId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}