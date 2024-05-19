using UniPass.Infrastructure.ViewModels;

namespace UniPass.Infrastructure.Contracts;

public interface IAccountService
{
    Task<Operation<bool>> Login(LoginViewModel model);
    Task<ApplicationAuthenticationState> GetState();
    Task<Operation<UserViewModel>> GetCurrentUser();
    Task<Operation<bool>> Logout();
}