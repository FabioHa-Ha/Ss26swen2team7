using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public interface IContactService
    {
        IEnumerable<ContactResponseDto> GetAll();
        ContactResponseDto? GetById(int id);
        ContactResponseDto Create(ContactCreateDto dto);
        ContactResponseDto? Update(int id, ContactUpdateDto dto);
        bool Delete(int id);
    }
}
