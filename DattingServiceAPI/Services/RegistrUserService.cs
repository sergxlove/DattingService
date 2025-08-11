using DataAccess.Profiles.Postgres.Abstractions;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Services
{
    public class RegistrUserService
    {
        private readonly ILoginUsersRepository _loginUserRep;
        private readonly IUsersRepository _userRep;
        private readonly ITempLoginUsersRepository _tempLoginUserRep;
        private readonly ITransactionsWork _transactions;

        public RegistrUserService(ILoginUsersRepository loginUserRep, IUsersRepository userRep,
            ITempLoginUsersRepository tempLoginUserRep,ITransactionsWork transactions)
        {
            _loginUserRep = loginUserRep;   
            _userRep = userRep;
            _tempLoginUserRep = tempLoginUserRep;
            _transactions = transactions;
        }

        public async Task<bool> RegistrationAsync(RegistrRequest request)
        {
            try
            {
                await _transactions.BeginTransactionAsync();


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
