using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Public.Models.Entities;

public class EducationDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EducationDetailId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string SchoolName { get; set; }
    
    [MaxLength(50)]
    public string Degree { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public int ApplicantId { get; set; }
    public Applicant Applicant { get; set; }
}