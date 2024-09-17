using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Public.Models.Entities;

public class Applicant
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ApplicantId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }

    public int JobPositionId { get; set; }
    public JobPosition JobPosition { get; set; }

    public ICollection<EducationDetail> EducationDetails { get; set; }
    public ICollection<ExperienceDetail> ExperienceDetails { get; set; }
}