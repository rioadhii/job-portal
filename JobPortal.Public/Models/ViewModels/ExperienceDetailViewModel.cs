using System.ComponentModel.DataAnnotations;

namespace JobPortal.Public.Models.ViewModels;

public class ExperienceDetailViewModel
{
    public int ExperienceDetailId { get; set; }

    [MaxLength(100)]
    public string CompanyName { get; set; }

    [MaxLength(50)]
    public string Position { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? Responsibilities { get; set; }
}