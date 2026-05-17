using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using VLimat.Eduz.App.Common;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;
using VLimat.Eduz.Application.Features.MasterConfig.Providers;

namespace VLimat.Eduz.App.Controllers.Master
{

    [Area("Master")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class MasterConfigController  : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IProviderMasterConfigFactory _queryFactory;
        public MasterConfigController(IMediator mediator, IProviderMasterConfigFactory queryFactory)
            => (_mediator, _queryFactory) = (mediator, queryFactory);

        // POST version that matches Angular: POST api/Master/MasterConfig/GetAll/{academicId} with body { category: configuration }
        [HttpPost("GetAll/{academicId}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetAllMasterConfig([FromRoute] string academicId, [FromBody] GetAllMasterConfigRequest request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Category))
                return BadRequest("Request body must include a non-empty 'category' property.");

            // Use your secret key (move to configuration/secret store for production)
            string secretKey = "MyVerySecretKey_1234567890ABCDEF";

            try
            {
                // Decrypt the incoming values (both route and body)
                //var decryptedAcademicId = CryptoHelper.DecryptString(academicId, secretKey);
                //if (!int.TryParse(decryptedAcademicId, out var parsedAcademicId))
                //    return BadRequest("Decrypted academicId is not a valid integer.");

                var decryptedConfiguration = CryptoHelper.DecryptString(request.Category, secretKey);

                var query = _queryFactory.CreateGetAllMasterConfig(1, decryptedConfiguration);
                var result = await _mediator.Send(query, cancellationToken);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (FormatException)
            {
                return BadRequest("One of the encrypted inputs is not a valid Base-64 string.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CryptographicException)
            {
                return BadRequest("Decryption failed. Check key/format.");
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetByConfiguration(int id, CancellationToken cancellationToken)
        {
            //var query = _queryFactory.CreateEntityGetMasterConfig(Id);
            var query = _queryFactory.CreateGetMasterConfig(id);
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MasterConfigRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest("Request body is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = _queryFactory.CreateCreateMasterConfigCommand(request);
                var created = await _mediator.Send(command, cancellationToken);

                if (created == null)
                    return BadRequest("Creation failed.");

                // normalize to object for pattern matching
                object result = created!;

                switch (result)
                {
                    case int id when id > 0:
                        return CreatedAtAction(nameof(GetByConfiguration), new { id }, result);
                    case long id when id > 0:
                        return CreatedAtAction(nameof(GetByConfiguration), new { id }, result);
                    case bool b when b:
                        return Ok(result);
                    case bool b when !b:
                        return BadRequest("Creation reported failure.");
                    default:
                        return Ok(result);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CryptographicException)
            {
                return BadRequest("Decryption failed. Check key/format.");
            }
            catch (Exception)
            {
                // Consider logging the exception here.
                return StatusCode(500, "An unexpected error occurred while creating the resource.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MasterConfigRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest("Request body is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = _queryFactory.CreateUpdateMasterConfigCommand(request);
                var updated = await _mediator.Send(command, cancellationToken);

                if (updated == null)
                    return NotFound();

                object result = updated!;

                // Interpret common update handler return types:
                // - bool true  => success (204 NoContent)
                // - bool false => failure (400)
                // - int/long > 0 => affected rows => success (204)
                // - any other object => return updated object (200)
                switch (result)
                {
                    case bool b when b:
                        return NoContent();
                    case bool b when !b:
                        return BadRequest("Update reported failure.");
                    case int i when i > 0:
                        return NoContent();
                    case long l when l > 0:
                        return NoContent();
                    default:
                        return Ok(result);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CryptographicException)
            {
                return BadRequest("Decryption failed. Check key/format.");
            }
            catch (Exception)
            {
                // Consider logging the exception here.
                return StatusCode(500, "An unexpected error occurred while updating the resource.");
            }
        }

        // Simple request DTO for POST body
        public class GetAllMasterConfigRequest
        {
            public string Category { get; set; } = string.Empty;
        }
    }
}
