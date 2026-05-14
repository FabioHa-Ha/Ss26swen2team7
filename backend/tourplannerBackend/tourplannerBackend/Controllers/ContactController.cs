using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    /// <summary>
    /// Simple Contact-Management REST API.
    /// Data is stored in an in-memory collection (no database) — data resets on application restart.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET api/contact
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactResponseDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ContactResponseDto>> GetAll()
        {
            return Ok(_contactService.GetAll());
        }

        // GET api/contact/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ContactResponseDto> GetById(int id)
        {
            var contact = _contactService.GetById(id);
            return contact == null ? NotFound() : Ok(contact);
        }

        // POST api/contact
        [HttpPost]
        [ProducesResponseType(typeof(ContactResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ContactResponseDto> Create([FromBody] ContactCreateDto dto)
        {
            var created = _contactService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/contact/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContactResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ContactResponseDto> Update(int id, [FromBody] ContactUpdateDto dto)
        {
            var updated = _contactService.Update(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        // DELETE api/contact/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var deleted = _contactService.Delete(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
