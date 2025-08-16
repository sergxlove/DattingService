using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Services
{ 
    public class UsersService : IUsersService
    {
        public UsersService(IUsersRepository repository)
        {
            _repository = repository;
        }

        private readonly IUsersRepository _repository;

        public async Task<Users?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetByIdAsync(id, token);
        }

        public async Task<Guid> AddAsync(Users user, CancellationToken token)
        {
            return await _repository.AddAsync(user, token);
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }

        public async Task<int> ActiveAsync(Guid id, CancellationToken token)
        {
            return await _repository.ActiveAsync(id, token);
        }

        public async Task<int> VerifyAsync(Guid id, CancellationToken token)
        {
            return await _repository.VerifyAsync(id, token);
        }

        public async Task<int> InactiveAsync(Guid id, CancellationToken token)
        {
            return await _repository.InactiveAsync(id, token);
        }
    }
}
