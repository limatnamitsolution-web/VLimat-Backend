using Microsoft.AspNetCore.Mvc;
using VLimat.Eduz.Application.Features.Student.Admission.DTOs;

namespace VLimat.Eduz.App.Controllers.Student
{
    [Area("Student")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class AdmissionController : ControllerBase
    {

        [HttpPost("save-student-admission")]
        public async Task<IActionResult> SaveStudentAdmission([FromForm] StudentAdmissionRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            try
            {
                // 1. Save Student Basic Details
                // var studentEntity = MapToEntity(model);
                // _context.Students.Add(studentEntity);
                // await _context.SaveChangesAsync();

                // 2. Handle File Uploads
                if (model.Docs != null && model.Docs.Count > 0)
                {
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedDocs");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    foreach (var doc in model.Docs)
                    {
                        if (doc.doc_File != null && doc.doc_File.Length > 0)
                        {
                            // Generate a unique filename or use a specific format
                            // e.g., {AdmissionNo}_{DocCode}_{OriginalFileName}
                            string fileName = $"{model.adm_no}_{doc.doc_Code}_{Guid.NewGuid()}{Path.GetExtension(doc.doc_File.FileName)}";
                            string filePath = Path.Combine(uploadFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await doc.doc_File.CopyToAsync(stream);
                            }

                            // Save file path/metadata to database linked to the student
                            // var docEntity = new StudentDocument { ... Path = filePath ... };
                            // _context.StudentDocuments.Add(docEntity);
                        }
                    }
                }

                // 3. Save Transport Months
                if (model.months != null)
                {
                    foreach (var monthId in model.months)
                    {
                        // Save student-transport-month mapping
                    }
                }

                return Ok(new { message = "Student saved successfully", studentId = model.adm_no });
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }


    }
}
