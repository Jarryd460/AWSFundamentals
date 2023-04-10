using Amazon.S3;
using Customers.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerImageController : ControllerBase
    {
        private readonly ICustomerImageService _customerImageService;

        public CustomerImageController(ICustomerImageService customerImageService)
        {
            _customerImageService = customerImageService;
        }

        [HttpPost("customers/{id:guid}/image")]
        public async Task<IActionResult> Upload([FromRoute] Guid id, [FromForm(Name = "Data")] IFormFile file)
        {
            var response = await _customerImageService.UploadImageAsync(id, file).ConfigureAwait(false);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("customers/{id:guid}/image")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var response = await _customerImageService.GetImageAsync(id).ConfigureAwait(false);
                return File(response.ResponseStream, response.Headers.ContentType);
            }
            catch (AmazonS3Exception ex) when (ex.Message is "The specified key does not exist.")
            {
                return NotFound();
            }
        }

        [HttpDelete("customers/{id:guid}/image")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
