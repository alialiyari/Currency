using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Userr
{
    public class ListViewQuery : CommandRequest<OperationResult<PagedResultDto<ListModel>>>
    {
        public DataSourceRequestDTO DataSourceRequest { get; set; }
    }

    class ListViewHandler : IRequestHandler<ListViewQuery, OperationResult<PagedResultDto<ListModel>>>
    {

        private readonly DatabaseContext dbContext;


        public ListViewHandler(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResultDto<ListModel>>> Handle(ListViewQuery request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Users.AsNoTracking().Select(x => new ListModel()
            {
                Id = x.Id,
                FullName =  x.FirstName + ' ' + x.LastName,
                ActiveStatus = x.ActiveStatus,
                NationalCode = x.NationalCode,
                RegisterDate = x.RegisterDate,
                UserName = x.UserName,
                VerifyStatus = x.VerifyStatus
            }).OrderByDescending(x=> x.Id).ToPagedResult(request.DataSourceRequest);

            return OperationResult<PagedResultDto<ListModel>>.BuildSuccessResult(result);
        }
    }
}
