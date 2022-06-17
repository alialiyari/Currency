using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Threading;
using System.Threading.Tasks;

namespace Userr
{
    public class VerifyStatusSetCommand : CommandRequest<OperationResult>
    {
        public long Id { get; set; }
        public VerifyStatusEnum VerifyStatus { get; set; }
    }

    class ApproveCommandHandler : IRequestHandler<VerifyStatusSetCommand, OperationResult>
    {

        private readonly DatabaseContext databaseContext;


        public ApproveCommandHandler(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<OperationResult> Handle(VerifyStatusSetCommand request, CancellationToken cancellationToken)
        {

            var finded = await databaseContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            finded.VerifyStatus = request.VerifyStatus;


            await databaseContext.SaveChangesAsync(cancellationToken);
            return OperationResult.BuildSuccessResult("با موفقیت تائید گردید");
        }
    }
}
