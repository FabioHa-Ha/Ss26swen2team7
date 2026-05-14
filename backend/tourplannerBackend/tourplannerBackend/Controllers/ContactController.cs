using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    /// <summary>
    /// Simple Contact-Management REST API.
    /// Data is stored in an in-memory collection (no database) — data resets on application restart.
    ///
    /// Error-handling technique demonstrated here: GLOBAL EXCEPTION HANDLER.
    /// The service layer throws typed domain exceptions (NotFoundException, ConflictException, …).
    /// The GlobalExceptionHandler middleware catches them and writes RFC-7807 ProblemDetails responses.
    /// Because of that, no try/catch is needed here — the controller stays focused on happy-path logic.
    ///
    /// Model-validation errors (invalid email format, missing required fields) are handled
    /// automatically by [ApiController] → 400 Bad Request with ValidationProblemDetails.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController(IContactService contactService) : ControllerBase
    {
        // GET api/contact
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactResponseDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ContactResponseDto>> GetAll()
            => Ok(contactService.GetAll());

        // GET api/contact/{id}
        // NotFoundException → GlobalExceptionHandler → 404 ProblemDetails
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public ActionResult<ContactResponseDto> GetById(int id)
            => Ok(contactService.GetById(id));

        // POST api/contact
        // 400: DataAnnotations fail (handled by [ApiController])
        // 409: ConflictException → GlobalExceptionHandler
        [HttpPost]
        [ProducesResponseType(typeof(ContactResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public ActionResult<ContactResponseDto> Create([FromBody] ContactCreateDto dto)
        {
            var created = contactService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/contact/{id}
        // 404: NotFoundException, 409: ConflictException — both via GlobalExceptionHandler
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContactResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public ActionResult<ContactResponseDto> Update(int id, [FromBody] ContactUpdateDto dto)
            => Ok(contactService.Update(id, dto));

        // DELETE api/contact/{id}
        // 404: NotFoundException → GlobalExceptionHandler
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            contactService.Delete(id);
            return NoContent();
        }
    }
}
