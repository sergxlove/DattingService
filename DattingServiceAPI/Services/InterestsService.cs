using DattingService.Core.Models;
using Newtonsoft.Json.Linq;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Services
{
    public class InterestsService : IInterestsService
    {
        private readonly IInterestsRepository _repository;

        public InterestsService(IInterestsRepository repository)
        {
            _repository = repository;
        }

        public async Task<JArray> GetAsync(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<Guid> AddAsync(Interests interest)
        {
            return await _repository.AddAsync(interest);
        }

        public async Task<int> UpdateAsync(Interests interest)
        {
            return await _repository.UpdateAsync(interest);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> CheckAsync(Guid id)
        {
            return await _repository.CheckAsync(id);
        }
    }
}
