using UniPass.Infrastructure.ViewModels;

namespace UniPass.Infrastructure.Contracts;

public interface IAccount
{
    Task<Operation<bool>> Login(LoginViewModel model);
    Task<ApplicationAuthenticationState> GetState();
    Task<Operation<UserModelViewModel>> GetCurrentUser();
    Task<Operation<bool>> Logout();
}