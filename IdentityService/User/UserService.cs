using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Http;
using Models;
using System.Security.Claims;

namespace Userr
{
    public class UserrService : ServiceBase<IUserService>, IUserService
    {
        private readonly IMediator mediator;
        private readonly ClaimsPrincipal User;

        public UserrService(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            User = httpContextAccessor.HttpContext.User;
        }

        public async UnaryResult<ServiceDto<PagedResultDto<ListModel>>> List(DataSourceRequestDTO dataSource)
        {
            return (await mediator.Send(new ListViewQuery() { DataSourceRequest = dataSource })).ToServiceDto();
        }

        public async UnaryResult<ServiceDto<long?>> InsertSave(InsertSaveModel createModel)
        {
            return await mediator.Send(new InsertCommand() { SaveModel = createModel });
        }

        public async UnaryResult<ServiceDto<ViewModel>> View(long Id)
        {
            return await mediator.Send(new ViewQuery() { Id = Id });
        }

        public async UnaryResult<ServiceDto> Save(SaveModel saveModel)
        {
            return await mediator.Send(new SaveCommand() { SaveModel = saveModel });
        }

        public async UnaryResult<ServiceDto> Admin_PasswordReset(long UserId, string NewPassword)
        {
            return await mediator.Send(new ChangePasswordCommand() { UserId = UserId, IsResetPassword = true, NewPassword = NewPassword });
        }





        // برای همه ی کاربران
        public async UnaryResult<ServiceDto<ViewModel>> CurrentUserInfoView()
        {
            return await mediator.Send(new ViewQuery() { Id = User.UserId() });
        }

        public async UnaryResult<ServiceDto> CurrentUserInfoSave(SaveModel saveModel)
        {
            saveModel.Id = User.UserId().GetValueOrDefault();
            saveModel.ActiveStatus = ActiveStatusEnum.InActive;
            saveModel.VerifyStatus = VerifyStatusEnum.Requested;

            return await mediator.Send(new SaveCommand() { AllowChangeUsername = false, SaveModel = saveModel, UserId = User.UserId() });
        }

        public async UnaryResult<ServiceDto> CurrentUserChangePassword(string CurrentPassword, string NewPassword)
        {
            return await mediator.Send(new ChangePasswordCommand() { CurrentPassword = CurrentPassword, NewPassword = NewPassword, UserId = User.UserId().Value });
        }





        public async UnaryResult<ServiceDto> DeleteSave(long? Id)
        {
            return (await mediator.Send(new DeleteCommand() { Id = Id })).ToServiceDto();
        }


        public async UnaryResult<ServiceDto<RolesViewModel>> RolesView(long UserId)
        {
            return (await mediator.Send(new RolesViewQuery() { Id = UserId })).ToServiceDto();
        }

        public async UnaryResult<ServiceDto> RolesSave(RolesSaveModel saveModel)
        {
            return (await mediator.Send(new RolesSaveCommand() { SaveModel = saveModel })).ToServiceDto();
        }



















        public async UnaryResult<ServiceDto<SignInResponse>> SignIn(SignInRequest signInModel)
        {
            return await mediator.Send(new SignInCommand() { signInModel = signInModel });
        }


        public async UnaryResult<ServiceDto> SmsPassAsync(SmsPassModel smsPassModel)
        {
            return await mediator.Send(new SmsPassCommand() { Mobile = smsPassModel.Mobile, NationalCode = smsPassModel.NationalCode, SmsCode = smsPassModel.SmsCode });
        }

        public async UnaryResult<ServiceDto> SmsMobileCode(SmsPassModel smsPassModel)
        {
            return await mediator.Send(new SmsMobileCodeCommand() { NationalCode = smsPassModel.NationalCode, Mobile = smsPassModel.Mobile });
        }
    }

}