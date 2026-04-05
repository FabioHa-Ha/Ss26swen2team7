using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourResponseDto>>> GetAll()
        {
            var tours = await _tourService.GetAllAsync();
            return Ok(tours);
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<TourResponseDto>>> GetMyTours()
        {
            var userId = GetUserId();
            var tours = await _tourService.GetByUserIdAsync(userId);
            return Ok(tours);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TourResponseDto>> GetById(int id)
        {
            var tour = await _tourService.GetByIdAsync(id);
            return tour == null ? NotFound() : Ok(tour);
        }

        [HttpPost]
        public async Task<ActionResult<TourResponseDto>> Create([FromBody] TourCreateDto dto)
        {
            var userId = GetUserId();
            try
            {
                var created = await _tourService.CreateAsync(userId, dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TourResponseDto>> Update(int id, [FromBody] TourUpdateDto dto)
        {
            try
            {
                var updated = await _tourService.UpdateAsync(id, dto);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _tourService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim!);
        }
    }
}
