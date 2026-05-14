using tourplannerBackend.DTOs;
using tourplannerBackend.Model;

namespace tourplannerBackend.Services
{
    // Registered as Singleton so the in-memory list persists for the application lifetime.
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

        public ContactResponseDto? GetById(int id)
        {
            lock (_lock)
            {
                var contact = _contacts.FirstOrDefault(c => c.Id == id);
                return contact == null ? null : MapToDto(contact);
            }
        }

        public ContactResponseDto Create(ContactCreateDto dto)
        {
            lock (_lock)
            {
                var contact = new Contact
                {
                    Id      = _nextId++,
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

        public ContactResponseDto? Update(int id, ContactUpdateDto dto)
        {
            lock (_lock)
            {
                var contact = _contacts.FirstOrDefault(c => c.Id == id);
                if (contact == null) return null;

                if (dto.FirstName != null) contact.FirstName = dto.FirstName;
                if (dto.LastName  != null) contact.LastName  = dto.LastName;
                if (dto.Email     != null) contact.Email     = dto.Email;
                if (dto.Phone     != null) contact.Phone     = dto.Phone;
                if (dto.Address   != null) contact.Address   = dto.Address;

                return MapToDto(contact);
            }
        }

        public bool Delete(int id)
        {
            lock (_lock)
            {
                var contact = _contacts.FirstOrDefault(c => c.Id == id);
                if (contact == null) return false;
                _contacts.Remove(contact);
                return true;
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
