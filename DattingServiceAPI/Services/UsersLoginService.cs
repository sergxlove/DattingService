using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Services
{
    public class UsersLoginService : IUsersLoginService
    {
        public UsersLoginService(IUsersLoginRepository repository)
        {
            _repository = repository;
        }
        private readonly IUsersLoginRepository _repository;

        public async Task<bool> VerifyAsync(string username, string password)
        {
            return await _repository.VerifyAsync(username, password);
        }

        public async Task<Guid> AddAsync(UsersDataForLogin user)
        {
            return await _repository.AddAsync(user);
        }

        public async Task<int> DeleteAsync(string username)
        {
            return await _repository.DeleteAsync(username);
        }

        public async Task<bool> CheckAsync(string username)
        {
            return await _repository.CheckAsync(username);
        }

        public async Task<int> UpdatePasswordAsync(UsersDataForLogin user)
        {
            return await _repository.UpdatePasswordAsync(user);
        }
    }
}
