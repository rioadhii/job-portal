using System.ComponentModel.DataAnnotations;

namespace JobPortal.Public.Models.ViewModels;

public class ApplicantViewModel
{
    public int ApplicantId { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Phone]
    [StringLength(15)]
    public string PhoneNumber { get; set; }

    [Required]
    public int JobPositionId { get; set; }

    public string? JobPositionName { get; set; } // To display the name of the Job Position in the view

    public List<EducationDetailViewModel> EducationDetails { get; set; } = new List<EducationDetailViewModel>();
    
    public List<ExperienceDetailViewModel> ExperienceDetails { get; set; } = new List<ExperienceDetailViewModel>();

}