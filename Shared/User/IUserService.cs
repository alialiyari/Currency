using Models;
using MagicOnion;

namespace Userr
{
    public interface IUserService : IService<IUserService>
    {
        UnaryResult<ServiceDto> DeleteSave(long? Id);
        UnaryResult<ServiceDto<long?>> InsertSave(InsertSaveModel createModel);
        UnaryResult<ServiceDto<PagedResultDto<ListModel>>> List(DataSourceRequestDTO dataSource);
        UnaryResult<ServiceDto> RolesSave(RolesSaveModel saveModel);
        UnaryResult<ServiceDto<RolesViewModel>> RolesView(long UserId);
       
        UnaryResult<ServiceDto> Save(SaveModel saveModel);
        UnaryResult<ServiceDto<ViewModel>> View(long Id);
        UnaryResult<ServiceDto> CurrentUserInfoSave(SaveModel saveModel);
        UnaryResult<ServiceDto<ViewModel>> CurrentUserInfoView();
       
        UnaryResult<ServiceDto<SignInResponse>> SignIn(SignInRequest signInModel);
        UnaryResult<ServiceDto> SmsPassAsync(SmsPassModel smsPassModel);
        UnaryResult<ServiceDto> SmsMobileCode(SmsPassModel smsPassModel);
        UnaryResult<ServiceDto> CurrentUserChangePassword(string CurrentPassword, string NewPassword);
        UnaryResult<ServiceDto> Admin_PasswordReset(long UserId, string NewPassword);
    }
}