using System.ComponentModel.DataAnnotations;

namespace JobPortal.Public.Models.ViewModels;

public class ApplicantViewModel
{
    public int ApplicantId { get; set; }

    [Display(Name = "First Name")]
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    [StringLength(100)]
    public string LastName { get; set; }

    [Display(Name = "Email")]
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Display(Name = "Date of Birth")]
    [Required]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Phone Number")]
    [Required]
    [Phone]
    [StringLength(15)]
    public string PhoneNumber { get; set; }

    [Display(Name = "Position")]
    [Required]
    public int JobPositionId { get; set; }

    public string? JobPositionName { get; set; } // To display the name of the Job Position in the view

    public List<EducationDetailViewModel> EducationDetails { get; set; } = new List<EducationDetailViewModel>();
    
    public List<ExperienceDetailViewModel> ExperienceDetails { get; set; } = new List<ExperienceDetailViewModel>();

}