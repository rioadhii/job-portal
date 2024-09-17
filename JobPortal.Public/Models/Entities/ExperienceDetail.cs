using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Public.Models.Entities;

public class ExperienceDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ExperienceDetailId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Position { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public string Responsibilities { get; set; }

    public int ApplicantId { get; set; }
    public Applicant Applicant { get; set; }
}