using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VLimat.Eduz.App.Controllers.Master;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;
using Xunit;

namespace VLimat.Eduz.App.Tests.Controllers.Master
{
    public class MasterConfigControllerTests
    {
        // These tests exercise the controller-side validation branches that do not require
        // IMediator or IProviderMasterConfigFactory behaviour. Passing null for those deps
        // is safe because the controller checks inputs before using them.

        [Fact]
        public async Task GetAllMasterConfig_ReturnsBadRequest_WhenRequestIsNull()
        {
            var controller = new MasterConfigController(null, null);

            var result = await controller.GetAllMasterConfig("1", null, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetAllMasterConfig_ReturnsBadRequest_WhenCategoryIsEmpty()
        {
            var controller = new MasterConfigController(null, null);

            var request = new MasterConfigController.GetAllMasterConfigRequest { Category = string.Empty };

            var result = await controller.GetAllMasterConfig("1", request, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenRequestIsNull()
        {
            var controller = new MasterConfigController(null, null);

            var result = await controller.Create(null, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var controller = new MasterConfigController(null, null);
            controller.ModelState.AddModelError("Configuration", "Required");

            var request = new MasterConfigRequest();

            var result = await controller.Create(request, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenRequestIsNull()
        {
            var controller = new MasterConfigController(null, null);

            var result = await controller.Update(null, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var controller = new MasterConfigController(null, null);
            controller.ModelState.AddModelError("ConfigKey", "Required");

            var request = new MasterConfigRequest();

            var result = await controller.Update(request, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}