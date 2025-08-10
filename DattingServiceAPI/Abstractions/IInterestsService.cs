using DattingService.Core.Models;
using Newtonsoft.Json.Linq;

namespace ProfilesServiceAPI.Abstractions
{
    public interface IInterestsService
    {
        Task<Guid> AddAsync(Interests interest);
        Task<bool> CheckAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<JArray> GetAsync(Guid id);
        Task<int> UpdateAsync(Interests interest);
    }
}