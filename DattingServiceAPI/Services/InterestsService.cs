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

        public async Task<int[]> GetAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetAsync(id, token);
        }

        public async Task<Guid> AddAsync(Interests interest, CancellationToken token)
        {
            return await _repository.AddAsync(interest, token);
        }

        public async Task<int> UpdateAsync(Interests interest, CancellationToken token)
        {
            return await _repository.UpdateAsync(interest, token);
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }

        public async Task<bool> CheckAsync(Guid id, CancellationToken token)
        {
            return await _repository.CheckAsync(id, token);
        }
    }
}
