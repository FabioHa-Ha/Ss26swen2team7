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
    public class TourLogController : ControllerBase
    {
        private readonly ITourLogService _tourLogService;

        public TourLogController(ITourLogService tourLogService)
        {
            _tourLogService = tourLogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourLogResponseDto>>> GetAll()
        {
            var logs = await _tourLogService.GetAllAsync();
            return Ok(logs);
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<TourLogResponseDto>>> GetMyLogs()
        {
            var userId = GetUserId();
            var logs = await _tourLogService.GetByUserIdAsync(userId);
            return Ok(logs);
        }

        [HttpGet("tour/{tourId}")]
        public async Task<ActionResult<IEnumerable<TourLogResponseDto>>> GetByTour(int tourId)
        {
            var logs = await _tourLogService.GetByTourIdAsync(tourId);
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TourLogResponseDto>> GetById(int id)
        {
            var log = await _tourLogService.GetByIdAsync(id);
            return log == null ? NotFound() : Ok(log);
        }

        [HttpPost]
        public async Task<ActionResult<TourLogResponseDto>> Create([FromBody] TourLogCreateDto dto)
        {
            var userId = GetUserId();
            try
            {
                var created = await _tourLogService.CreateAsync(userId, dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TourLogResponseDto>> Update(int id, [FromBody] TourLogUpdateDto dto)
        {
            try
            {
                var updated = await _tourLogService.UpdateAsync(id, dto);
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
            var deleted = await _tourLogService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim!);
        }
    }
}
