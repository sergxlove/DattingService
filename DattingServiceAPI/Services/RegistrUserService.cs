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
        private readonly ITransactionsWork _transactions;

        public RegistrUserService(ILoginUsersRepository loginUserRep, IUsersRepository userRep,
            ITempLoginUsersRepository tempLoginUserRep, ITransactionsWork transactions)
        {
            _loginUserRep = loginUserRep;
            _userRep = userRep;
            _tempLoginUserRep = tempLoginUserRep;
            _transactions = transactions;
        }

        public async Task<bool> RegistrationAsync(Users user, CancellationToken token)
        {
            try
            {
                await _transactions.BeginTransactionAsync();
                var tempUser = await _tempLoginUserRep.GetAsync(user.Id, token);
                if (tempUser is null) throw new Exception();
                LoginUsers loginUser = LoginUsers.Create(tempUser.Email, tempUser.Password).Value;
                var result = await _loginUserRep.AddAsync(loginUser, token);
                if (result != user.Id) throw new Exception();
                result = await _userRep.AddAsync(user, token);
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
    }
}
