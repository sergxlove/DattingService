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

        public async Task<Guid> AddAsync(LoginUsers user, CancellationToken token)
        {
            return await _repository.AddAsync(user, token);
        }

        public async Task<int> DeleteAsync(string email, CancellationToken token)
        {
            return await _repository.DeleteAsync(email, token);
        }

        public async Task<LoginUsers?> GetAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetAsync(id, token);
        }
    }
}
