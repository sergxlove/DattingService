using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Services
{
    public class TempLoginUsersService : ITempLoginUsersService
    {
        private readonly ITempLoginUsersRepository _repository;
        public TempLoginUsersService(ITempLoginUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> AddAsync(LoginUsers user)
        {
            return await _repository.AddAsync(user);
        }

        public async Task<int> DeleteAsync(string email)
        {
            return await _repository.DeleteAsync(email);
        }

        public async Task<LoginUsers?> GetAsync(Guid id)
        {
            return await _repository.GetAsync(id);
        }
    }
}
