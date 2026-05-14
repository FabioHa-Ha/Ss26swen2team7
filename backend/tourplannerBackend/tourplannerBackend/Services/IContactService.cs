using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public interface IContactService
    {
        IEnumerable<ContactResponseDto> GetAll();

        /// <summary>Throws NotFoundException when the id does not exist.</summary>
        ContactResponseDto GetById(int id);

        /// <summary>Throws ConflictException when the email is already in use.</summary>
        ContactResponseDto Create(ContactCreateDto dto);

        /// <summary>Throws NotFoundException or ConflictException on violations.</summary>
        ContactResponseDto Update(int id, ContactUpdateDto dto);

        /// <summary>Throws NotFoundException when the id does not exist.</summary>
        void Delete(int id);
    }
}
