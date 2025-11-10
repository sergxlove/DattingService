using DataAccess.Profiles.Postgres.Abstractions;
using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Services
{
    public class RegistrUserService : IRegistrUserService
    {
        private readonly ILoginUsersRepository _loginUserRep;
        private readonly IUsersRepository _userRep;
        private readonly ITempLoginUsersRepository _tempLoginUserRep;
        private readonly IInterestsRepository _interestsRep;
        private readonly ITransactionsWork _transactions;

        public RegistrUserService(ILoginUsersRepository loginUserRep, IUsersRepository userRep,
            ITempLoginUsersRepository tempLoginUserRep, IInterestsRepository interestsRep,
            ITransactionsWork transactions)
        {
            _loginUserRep = loginUserRep;
            _userRep = userRep;
            _tempLoginUserRep = tempLoginUserRep;
            _interestsRep = interestsRep;
            _transactions = transactions;
        }

        public async Task<bool> RegistrationAsync(Users user, CancellationToken token)
        {
            try
            {
                await _transactions.BeginTransactionAsync();
                LoginUsers? tempUser = await _tempLoginUserRep.GetAsync(user.Id, token);
                if (tempUser is null) throw new Exception();
                LoginUsers loginUser = LoginUsers.Create(tempUser.Id,tempUser.Email, 
                    tempUser.Password, false).Value;
                Guid result = await _loginUserRep.AddAsync(loginUser, token);
                if (result != user.Id) throw new Exception();
                result = await _userRep.AddAsync(user, token);
                if (result != user.Id) throw new Exception();
                result = await _interestsRep.AddAsync(new Interests(user.Id, Array.Empty<int>()), token);
                if (result != user.Id) throw new Exception();
                await _tempLoginUserRep.DeleteAsync(tempUser.Email, token);
                await _transactions.CommitAsync();
                return true;
            }
            catch
            {
                await _transactions.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid id, CancellationToken token)
        {
            try
            {
                await _transactions.BeginTransactionAsync();
                if(await _loginUserRep.CheckAsync(id, token)) throw new Exception();
                int result = await _interestsRep.DeleteAsync(id, token);
                if(result == 0) throw new Exception();
                result = await _userRep.DeleteAsync(id, token);
                if (result == 0) throw new Exception();
                result = await _loginUserRep.DeleteAsync(id, token);
                if (result == 0) throw new Exception();
                await _transactions.CommitAsync();
                return true;
            }
            catch
            {
                await _transactions.CommitAsync();
                return false;
            }
        }
    }
}
