using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Filters;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    /// <summary>
    /// Error-handling technique demonstrated here: EXCEPTION FILTER (DomainExceptionFilter).
    ///
    /// [TypeFilter(typeof(DomainExceptionFilter))] intercepts AppException subtypes thrown by the
    /// service layer and converts them to ProblemDetails — scoped to this controller only.
    /// Unrecognised exceptions (e.g. DbException) fall through to the GlobalExceptionHandler.
    ///
    /// Compare with ContactController which relies solely on the global handler.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [TypeFilter(typeof(DomainExceptionFilter))]
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
        [ProducesResponseType(typeof(TourResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TourResponseDto>> GetById(int id)
        {
            var tour = await _tourService.GetByIdAsync(id);
            return tour == null ? NotFound() : Ok(tour);
        }

        // NotFoundException / BusinessRuleException → DomainExceptionFilter
        [HttpPost]
        [ProducesResponseType(typeof(TourResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TourResponseDto>> Create([FromBody] TourCreateDto dto)
        {
            var userId = GetUserId();
            var created = await _tourService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TourResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TourResponseDto>> Update(int id, [FromBody] TourUpdateDto dto)
        {
            var updated = await _tourService.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
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
