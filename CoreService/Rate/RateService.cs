using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Models;
using System.Security.Claims;

namespace Rate
{
    public class RateService : ServiceBase<IRateService>, IRateService
    {
        private readonly IMediator mediator;
        private readonly ClaimsPrincipal User;

        public RateService(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
        }

        public async UnaryResult<ServiceDto<long?>> Save(SaveModel saveModel)
        {
            return await mediator.Send(new SaveCommand()
            {
                SaveModel = saveModel,
            });
        }

        public async UnaryResult<ServiceDto<List<InfoModel>>> List()
        {
            return await mediator.Send(new ListQuery() { });
        }


        public async UnaryResult<ServiceDto<List<NodeModel>>> Find(string From, string To)
        {
            return await mediator.Send(new FindQuery() { From = From, To = To });
        }
    }

}