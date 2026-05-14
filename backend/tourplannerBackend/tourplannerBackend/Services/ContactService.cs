using tourplannerBackend.DTOs;
using tourplannerBackend.Exceptions;
using tourplannerBackend.Model;

namespace tourplannerBackend.Services
{
    // Registered as Singleton so the in-memory list persists for the application lifetime.
    //
    // Error-handling approach demonstrated here: SERVICE LAYER throws typed domain exceptions
    // (NotFoundException, ConflictException, BusinessRuleException) instead of returning null/bool.
    // The controller stays free of try/catch — the GlobalExceptionHandler maps exceptions to HTTP.
    public class ContactService : IContactService
    {
        private readonly List<Contact> _contacts = [];
        private int _nextId = 1;
        private readonly object _lock = new();

        public ContactService()
        {
            _contacts.Add(new Contact { Id = _nextId++, FirstName = "Max",  LastName = "Mustermann", Email = "max.mustermann@example.com", Phone = "+43 1 234 5678" });
            _contacts.Add(new Contact { Id = _nextId++, FirstName = "Anna", LastName = "Beispiel",   Email = "anna.beispiel@example.com",   Address = "Musterstraße 1, 1010 Wien" });
        }

        public IEnumerable<ContactResponseDto> GetAll()
        {
            lock (_lock)
                return _contacts.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Returns the contact or throws NotFoundException — callers don't need to handle null.
        /// </summary>
        public ContactResponseDto GetById(int id)
        {
            lock (_lock)
            {
                var contact = _contacts.FirstOrDefault(c => c.Id == id)
                    ?? throw new NotFoundException(nameof(Contact), id);
                return MapToDto(contact);
            }
        }

        /// <summary>
        /// Business rule: email must be unique across all contacts.
        /// Throws ConflictException before inserting a duplicate.
        /// </summary>
        public ContactResponseDto Create(ContactCreateDto dto)
        {
            lock (_lock)
            {
                // Business-rule validation — email uniqueness (DataAnnotations only checks format)
                if (_contacts.Any(c => c.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
                    throw new ConflictException($"A contact with email '{dto.Email}' already exists.");

                var contact = new Contact
                {
                    Id        = _nextId++,
                    FirstName = dto.FirstName,
                    LastName  = dto.LastName,
                    Email     = dto.Email,
                    Phone     = dto.Phone,
                    Address   = dto.Address
                };
                _contacts.Add(contact);
                return MapToDto(contact);
            }
        }

        /// <summary>
        /// Throws NotFoundException if the contact does not exist.
        /// Throws ConflictException if the new email is already taken by another contact.
        /// </summary>
        public ContactResponseDto Update(int id, ContactUpdateDto dto)
        {
            lock (_lock)
            {
                var contact = _contacts.FirstOrDefault(c => c.Id == id)
                    ?? throw new NotFoundException(nameof(Contact), id);

                if (dto.Email != null &&
                    _contacts.Any(c => c.Id != id && c.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
                    throw new ConflictException($"Email '{dto.Email}' is already used by another contact.");

                if (dto.FirstName != null) contact.FirstName = dto.FirstName;
                if (dto.LastName  != null) contact.LastName  = dto.LastName;
                if (dto.Email     != null) contact.Email     = dto.Email;
                if (dto.Phone     != null) contact.Phone     = dto.Phone;
                if (dto.Address   != null) contact.Address   = dto.Address;

                return MapToDto(contact);
            }
        }

        /// <summary>
        /// Throws NotFoundException instead of returning false — the caller always gets a result.
        /// </summary>
        public void Delete(int id)
        {
            lock (_lock)
            {
                var contact = _contacts.FirstOrDefault(c => c.Id == id)
                    ?? throw new NotFoundException(nameof(Contact), id);
                _contacts.Remove(contact);
            }
        }

        private static ContactResponseDto MapToDto(Contact c) => new()
        {
            Id        = c.Id,
            FirstName = c.FirstName,
            LastName  = c.LastName,
            Email     = c.Email,
            Phone     = c.Phone,
            Address   = c.Address
        };
    }
}
