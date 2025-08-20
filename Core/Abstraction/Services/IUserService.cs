using Challenge.Core.DTOs.Users;

namespace Challenge.Core.Abstraction.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default);
        Task<UserDto?> GetAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct = default);
        Task UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
