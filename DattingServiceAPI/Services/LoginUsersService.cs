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

        public async Task<bool> VerifyAsync(string email, string password)
        {
            return await _repository.VerifyAsync(email, password);
        }

        public async Task<Guid> AddAsync(LoginUsers user)
        {
            return await _repository.AddAsync(user);
        }

        public async Task<int> DeleteAsync(string email)
        {
            return await _repository.DeleteAsync(email);
        }

        public async Task<bool> CheckAsync(string email)
        {
            return await _repository.CheckAsync(email);
        }

        public async Task<int> UpdatePasswordAsync(LoginUsers user)
        {
            return await _repository.UpdatePasswordAsync(user);
        }

    }
}
