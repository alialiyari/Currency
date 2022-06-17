using Common;
using Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Userr
{
    public class ViewQuery : CommandRequest<ServiceDto<ViewModel>>
    {
        public long? Id { get; set; }
    }

    class ViewHandler : IRequestHandler<ViewQuery, ServiceDto<ViewModel>>
    {

        private readonly DatabaseContext dbContext;
        private readonly FileServiceApi fileServiceApi;

        public ViewHandler(DatabaseContext dbContext, FileServiceApi fileServiceApi)
        {
            this.dbContext = dbContext;
            this.fileServiceApi = fileServiceApi;
        }

        public async Task<ServiceDto<ViewModel>> Handle(ViewQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new ViewModel()
                {
                    ActiveStatus = x.ActiveStatus,
                    Id = x.Id,
                    Address = x.Address,

                    Email = x.Email,
                    BirthDate = x.BirthDate,


                    FirstName = x.FirstName,
                    ImageUrl = x.ImageUrl,
                    LastLoginDate = x.LastLoginDate,
                    LastName = x.LastName,
                    Mobile = x.PhoneNumber,
                    MobileConfirmed = x.PhoneNumberConfirmed,
                    NationalCardUrl = x.NationalCardUrl,
                    NationalCode = x.NationalCode,
                    PostalCode = x.PostalCode,
                    RegisterDate = x.RegisterDate,


                    ShSh = x.ShSh,
                    UserName = x.UserName,
                    VerifyStatus = x.VerifyStatus,

                    IntroducerUserId = x.IntroducerUserId,
                    IntroducerUserFullName = x.IntroducerUser.FirstName + " " + x.IntroducerUser.LastName,
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user == null) return new ServiceDto<ViewModel>() { Message = "پارامترهای ورودی اشتباه می باشد" };

            user.ImageUrl = fileServiceApi.FileUrlGenerate(user.ImageUrl);
            user.NationalCardUrl = fileServiceApi.FileUrlGenerate(user.NationalCardUrl);

            return new ServiceDto<ViewModel>() { Data = user };
        }
    }
}
