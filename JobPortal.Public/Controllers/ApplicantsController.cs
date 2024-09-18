using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortal.Public.Models;
using JobPortal.Public.Models.Entities;
using JobPortal.Public.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JobPortal.Public.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ApplicantsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IActionResult> Index()
        {
            var applicants = await _dbContext.Applicants
                .Include(a => a.JobPosition) // Load JobPosition related data
                .ToListAsync();

            var applicantViewModels = applicants.Select(applicant => new ApplicantViewModel
            {
                ApplicantId = applicant.ApplicantId,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                PhoneNumber = applicant.PhoneNumber,
                DateOfBirth = applicant.DateOfBirth,
                JobPositionId = applicant.JobPositionId,
                JobPositionName = applicant.JobPosition.PositionName 
            }).ToList();

            return View(applicantViewModels);
        }

        public IActionResult Create()
        {
            ViewData["JobPositionId"] = new SelectList(_dbContext.JobPositions, "JobPositionId", "PositionName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicantViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save Applicant
                var applicant = new Applicant
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    JobPositionId = model.JobPositionId
                };
                _dbContext.Applicants.Add(applicant);
                await _dbContext.SaveChangesAsync();

                // Save Education Details
                if (model.EducationDetails != null)
                {
                    foreach (var eduDetail in model.EducationDetails)
                    {
                        var educationDetail = new EducationDetail
                        {
                            SchoolName = eduDetail.SchoolName,
                            Degree = eduDetail.Degree,
                            StartDate = eduDetail.StartDate,
                            EndDate = eduDetail.EndDate,
                            ApplicantId = applicant.ApplicantId
                        };
                        _dbContext.EducationDetails.Add(educationDetail);
                    }
                }

                // Save Experience Details
                if (model.ExperienceDetails != null)
                {
                    foreach (var expDetail in model.ExperienceDetails)
                    {
                        var experienceDetail = new ExperienceDetail
                        {
                            CompanyName = expDetail.CompanyName,
                            Position = expDetail.Position,
                            StartDate = expDetail.StartDate,
                            EndDate = expDetail.EndDate,
                            Responsibilities = expDetail.Responsibilities,
                            ApplicantId = applicant.ApplicantId
                        };
                        _dbContext.ExperienceDetails.Add(experienceDetail);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["JobPositionId"] = new SelectList(_dbContext.JobPositions, "JobPositionId", "PositionName",
                model.JobPositionId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get the applicant with their education and experience details
            var applicant = await _dbContext.Applicants
                .Include(a => a.EducationDetails)
                .Include(a => a.ExperienceDetails)
                .FirstOrDefaultAsync(a => a.ApplicantId == id);

            if (applicant == null)
            {
                return NotFound();
            }

            // Prepare the view model
            var viewModel = new ApplicantViewModel
            {
                ApplicantId = applicant.ApplicantId,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                DateOfBirth = applicant.DateOfBirth,
                PhoneNumber = applicant.PhoneNumber,
                JobPositionId = applicant.JobPositionId,
                EducationDetails = applicant.EducationDetails.Select(ed => new EducationDetailViewModel
                {
                    EducationDetailId = ed.EducationDetailId,
                    SchoolName = ed.SchoolName,
                    Degree = ed.Degree,
                    StartDate = ed.StartDate,
                    EndDate = ed.EndDate
                }).ToList(),
                ExperienceDetails = applicant.ExperienceDetails.Select(exp => new ExperienceDetailViewModel
                {
                    ExperienceDetailId = exp.ExperienceDetailId,
                    CompanyName = exp.CompanyName,
                    Position = exp.Position,
                    StartDate = exp.StartDate,
                    EndDate = exp.EndDate,
                    Responsibilities = exp.Responsibilities
                }).ToList()
            };

            ViewData["JobPositionId"] = new SelectList(_dbContext.JobPositions, "JobPositionId", "PositionName",
                viewModel.JobPositionId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ApplicantViewModel model)
        {
            if (id != model.ApplicantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the applicant
                    var applicant = await _dbContext.Applicants
                        .Include(a => a.EducationDetails)
                        .Include(a => a.ExperienceDetails)
                        .FirstOrDefaultAsync(a => a.ApplicantId == id);

                    if (applicant == null)
                    {
                        return NotFound();
                    }

                    // Update applicant's basic details
                    applicant.FirstName = model.FirstName;
                    applicant.LastName = model.LastName;
                    applicant.Email = model.Email;
                    applicant.DateOfBirth = model.DateOfBirth;
                    applicant.PhoneNumber = model.PhoneNumber;
                    applicant.JobPositionId = model.JobPositionId;

                    // Update Education Details
                    // Remove existing education details that were removed in the form
                    foreach (var existingEducation in applicant.EducationDetails.ToList())
                    {
                        if (!model.EducationDetails.Any(ed =>
                                ed.EducationDetailId == existingEducation.EducationDetailId))
                        {
                            _dbContext.EducationDetails.Remove(existingEducation);
                        }
                    }

                    // Update or add new education details
                    foreach (var educationModel in model.EducationDetails)
                    {
                        var existingEducation = applicant.EducationDetails
                            .FirstOrDefault(ed => ed.EducationDetailId == educationModel.EducationDetailId);

                        if (existingEducation != null)
                        {
                            // Update existing education detail
                            existingEducation.SchoolName = educationModel.SchoolName;
                            existingEducation.Degree = educationModel.Degree;
                            existingEducation.StartDate = educationModel.StartDate;
                            existingEducation.EndDate = educationModel.EndDate;
                        }
                        else
                        {
                            // Add new education detail
                            applicant.EducationDetails.Add(new EducationDetail
                            {
                                SchoolName = educationModel.SchoolName,
                                Degree = educationModel.Degree,
                                StartDate = educationModel.StartDate,
                                EndDate = educationModel.EndDate,
                                ApplicantId = applicant.ApplicantId
                            });
                        }
                    }

                    // Update Experience Details
                    // Remove existing experience details that were removed in the form
                    foreach (var existingExperience in applicant.ExperienceDetails.ToList())
                    {
                        if (!model.ExperienceDetails.Any(exp =>
                                exp.ExperienceDetailId == existingExperience.ExperienceDetailId))
                        {
                            _dbContext.ExperienceDetails.Remove(existingExperience);
                        }
                    }

                    // Update or add new experience details
                    foreach (var experienceModel in model.ExperienceDetails)
                    {
                        var existingExperience = applicant.ExperienceDetails
                            .FirstOrDefault(exp => exp.ExperienceDetailId == experienceModel.ExperienceDetailId);

                        if (existingExperience != null)
                        {
                            // Update existing experience detail
                            existingExperience.CompanyName = experienceModel.CompanyName;
                            existingExperience.Position = experienceModel.Position;
                            existingExperience.StartDate = experienceModel.StartDate;
                            existingExperience.EndDate = experienceModel.EndDate;
                            existingExperience.Responsibilities = experienceModel.Responsibilities;
                        }
                        else
                        {
                            // Add new experience detail
                            applicant.ExperienceDetails.Add(new ExperienceDetail
                            {
                                CompanyName = experienceModel.CompanyName,
                                Position = experienceModel.Position,
                                StartDate = experienceModel.StartDate,
                                EndDate = experienceModel.EndDate,
                                Responsibilities = experienceModel.Responsibilities,
                                ApplicantId = applicant.ApplicantId
                            });
                        }
                    }

                    // Save changes
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantExists(model.ApplicantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["JobPositionId"] = new SelectList(_dbContext.JobPositions, "JobPositionId", "PositionName",
                model.JobPositionId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _dbContext.Applicants
                .Include(a => a.JobPosition) // Include related data if needed
                .FirstOrDefaultAsync(m => m.ApplicantId == id);

            if (applicant == null)
            {
                return NotFound();
            }

            var applicantViewModel = new ApplicantViewModel
            {
                ApplicantId = applicant.ApplicantId,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                PhoneNumber = applicant.PhoneNumber,
                DateOfBirth = applicant.DateOfBirth,
                JobPositionId = applicant.JobPositionId,
                JobPositionName = applicant.JobPosition.PositionName // Populate Job Position Name for display
            };

            return View(applicantViewModel);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicant = await _dbContext.Applicants.FindAsync(id);

            if (applicant != null)
            {
                var educationDetails = await _dbContext.EducationDetails
                    .Where(x => x.ApplicantId == applicant.ApplicantId).ToListAsync();
                _dbContext.EducationDetails.RemoveRange(educationDetails);
                
                var experienceDetails = await _dbContext.ExperienceDetails
                    .Where(x => x.ApplicantId == applicant.ApplicantId).ToListAsync();
                _dbContext.ExperienceDetails.RemoveRange(experienceDetails);
                
                // Remove the applicant
                _dbContext.Applicants.Remove(applicant);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        
        private bool ApplicantExists(int id)
        {
            return _dbContext.Applicants.Any(a => a.ApplicantId == id);
        }
    }
}