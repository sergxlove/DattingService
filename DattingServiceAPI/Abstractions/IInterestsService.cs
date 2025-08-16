using DattingService.Core.Models;
using Newtonsoft.Json.Linq;

namespace ProfilesServiceAPI.Abstractions
{
    public interface IInterestsService
    {
        Task<Guid> AddAsync(Interests interest, CancellationToken token);
        Task<bool> CheckAsync(Guid id, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<JArray> GetAsync(Guid id, CancellationToken token);
        Task<int> UpdateAsync(Interests interest, CancellationToken token);
    }
}