using DataAccess.Profiles.Postgres.Abstractions;
using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Services
{
    public class LoginUsersService : ILoginUsersService
    {
        private readonly ILoginUsersRepository _repository;
        public LoginUsersService(ILoginUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> VerifyAsync(string email, string password, CancellationToken token)
        {
            return await _repository.VerifyAsync(email, password, token);
        }

        public async Task<Guid> AddAsync(LoginUsers user, CancellationToken token)
        {
            return await _repository.AddAsync(user, token);
        }

        public async Task<int> DeleteAsync(string email, CancellationToken token)
        {
            return await _repository.DeleteAsync(email, token);
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }

        public async Task<bool> CheckAsync(string email, CancellationToken token)
        {
            return await _repository.CheckAsync(email, token);
        }

        public async Task<bool> CheckAsync(Guid id, CancellationToken token)
        {
            return await _repository.CheckAsync(id, token);
        }

        public async Task<int> UpdatePasswordAsync(LoginUsers user, CancellationToken token)
        {
            return await _repository.UpdatePasswordAsync(user, token);
        }

        public async Task<string> GetEmailAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetEmailAsync(id, token);
        }
    }
}
