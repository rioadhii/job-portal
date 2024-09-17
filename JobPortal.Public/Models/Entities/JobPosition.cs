using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Public.Models.Entities;

public class JobPosition
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int JobPositionId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string PositionName { get; set; }
    
    public string Description { get; set; }

    public ICollection<Applicant> Applicants { get; set; }
}