using DataAccess.Photo.S3Minio.Abstractions;
using DataAccess.Profiles.Postgres.Abstractions;
using DattingService.Core.Models;
using Newtonsoft.Json.Linq;
using ProfilesServiceAPI.Abstractions;
using System.IO;

namespace ProfilesServiceAPI.Services
{
    public class PhotoMovedService : IPhotoMovedService
    {
        private readonly IPhotoRepository _photoRep;
        private readonly IUsersRepository _userRep;
        private readonly ITransactionsWork _transactions;

        public PhotoMovedService(IPhotoRepository photoRep, IUsersRepository userRep,
            ITransactionsWork transactions)
        {
            _photoRep = photoRep;
            _userRep = userRep;
            _transactions = transactions;
        }

        public async Task<bool> AddPhotoAsync(Stream photoStream, Guid idUser, string fileName,
            CancellationToken token)
        {
            try
            {
                await _transactions.BeginTransactionAsync();
                string photoId = await _photoRep.UploadFileAsync("photopr", fileName,
                        photoStream, token);
                if (photoId == string.Empty) throw new Exception();
                Users? user = await _userRep.GetByIdAsync(idUser, token);
                if (user is null) throw new Exception();
                user.AddUrlPhoto(photoId);
                int resultUpdate = await _userRep.UpdateAsync(user, token);
                if (resultUpdate == 0) throw new Exception();
                await _transactions.CommitAsync();
                return true;
            }
            catch
            {
                await _transactions.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeletePhotoAsync(Guid idUser, CancellationToken token)
        {
            try
            {
                await _transactions.BeginTransactionAsync();
                Users? user = await _userRep.GetByIdAsync(idUser, token);
                if (user is null) throw new Exception();
                if (user.PhotoURL is null) throw new Exception();
                bool result = await _photoRep.DeleteAsync("photopr",
                    user.PhotoURL[0].ToString(), token);
                if (!result) throw new Exception();
                user.RemoveUrlPhoto(user.PhotoURL[0]);
                await _userRep.UpdateAsync(user, token);
                await _transactions.CommitAsync();
                return true;
            }
            catch
            {
                await _transactions.RollbackAsync();
                return false;
            }
        }
    }
}
