using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Repositories;

namespace ProfilesServiceAPI.Services
{ 
    public class UsersService : IUsersService
    {
        public UsersService(UsersRepository repository)
        {
            _repository = repository;
        }

        private readonly UsersRepository _repository;

        public async Task<Users?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Guid> AddAsync(Users user)
        {
            return await _repository.AddAsync(user);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<int> ActiveAsync(Guid id)
        {
            return await _repository.ActiveAsync(id);
        }

        public async Task<int> InactiveAsync(Guid id)
        {
            return await _repository.InactiveAsync(id);
        }
    }
}
