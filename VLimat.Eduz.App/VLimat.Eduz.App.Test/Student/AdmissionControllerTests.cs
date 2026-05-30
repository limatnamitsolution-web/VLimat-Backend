using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VLimat.Eduz.App.Controllers.Student;
using VLimat.Eduz.Application.Features.Student.Admission.DTOs;
using Xunit;

namespace VLimat.Eduz.App.Tests.Controllers.Student
{
    public class AdmissionControllerTests
    {
        [Fact]
        public async Task SaveStudentAdmission_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var controller = new AdmissionController();
            controller.ModelState.AddModelError("sess_stud_first_name", "Required");

            var request = new StudentAdmissionRequestDto();

            var result = await controller.SaveStudentAdmission(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SaveStudentAdmission_ReturnsOk_WhenModelIsValid_NoFiles()
        {
            var controller = new AdmissionController();

            var request = new StudentAdmissionRequestDto
            {
                adm_no = "ADM001"
                // no Docs and months are left as defaults
            };

            var result = await controller.SaveStudentAdmission(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task SaveStudentAdmission_ReturnsInternalServerError_WhenModelIsNull()
        {
            var controller = new AdmissionController();

            // Passing null may cause the controller to throw when it accesses model members;
            // the controller catches exceptions and returns 500, so assert that behavior.
            var result = await controller.SaveStudentAdmission(null);

            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
        }

        // Intentionally failing test: expects Ok but controller returns BadRequest due to swallowed exception in IntentionalBug
        [Fact]
        public void IntentionalBug_Test_WillFail()
        {
            var controller = new AdmissionController();

            var result = controller.IntentionalBug("trigger");

            // This assertion is intentionally incorrect to produce a failing test
            var ok = Assert.IsType<OkObjectResult>(result);
        }
    }
}
