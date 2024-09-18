using System.ComponentModel.DataAnnotations;

namespace JobPortal.Public.Models.ViewModels;

public class EducationDetailViewModel
{
    public int EducationDetailId { get; set; }

    [MaxLength(100)]
    public string SchoolName { get; set; }

    [MaxLength(50)]
    public string Degree { get; set; }

    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

}