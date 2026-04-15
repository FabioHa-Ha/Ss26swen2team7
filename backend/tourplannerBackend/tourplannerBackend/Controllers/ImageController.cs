using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Model;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService imageService;

        public ImageController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ImageCreateDto imageDto)
        {
            if (imageDto.Image == null || imageDto.Image.Length == 0)
                return BadRequest("No file uploaded.");

            int newId = await imageService.CreateImage(imageDto);
            return Ok(newId);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            TourImage? image = await imageService.GetImage(id);

            if (image == null)
                return NotFound();

            return File(image.Image, image.ContentType, image.FileName);
        }
    }
}
