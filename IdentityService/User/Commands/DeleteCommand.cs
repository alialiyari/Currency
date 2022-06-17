using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Threading;
using System.Threading.Tasks;

namespace Userr
{
    public class DeleteCommand : CommandRequest<OperationResult>
    {
        public long? Id { get; set; }
    }

    class DeleteCommandHandler : IRequestHandler<DeleteCommand, OperationResult>
    {
        private readonly DatabaseContext dbContext;
        private readonly FileServiceApi fileServiceApi;

        public DeleteCommandHandler(DatabaseContext dbContext, FileServiceApi fileServiceApi)
        {
            this.dbContext = dbContext;
            this.fileServiceApi = fileServiceApi;
        }

        public async Task<OperationResult> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await dbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);




            await fileServiceApi.FileRemove(userEntity.ImageUrl);
            await fileServiceApi.FileRemove(userEntity.NationalCardUrl);


            
            dbContext.Users.Remove(userEntity);
            await dbContext.SaveChangesAsync(cancellationToken);
            return OperationResult.BuildSuccessResult(  "با موفقیت حذف گردید");
        }
    }
}
