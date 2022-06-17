using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Models;


namespace Rate
{
    public class ListQuery : IRequest<ServiceDto<List<InfoModel>>>
    {

    }

    class ListQueryHandler : IRequestHandler<ListQuery, ServiceDto<List<InfoModel>>>
    {
        private readonly IMemoryCache memoryCache;
        private readonly DatabaseContext databaseContext;

        public ListQueryHandler(IMemoryCache memoryCache, DatabaseContext databaseContext)
        {
            this.memoryCache = memoryCache;
            this.databaseContext = databaseContext;
        }
        public async Task<ServiceDto<List<InfoModel>>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            var data = await memoryCache.GetOrCreateAsync("Rates", async entry =>
            {
                entry.AbsoluteExpiration = DateTime.Now.AddYears(1);

                var data = await databaseContext.Rates.AsNoTracking()
                    .Where(x => x.FinishDate == null).OrderByDescending(x => x.Id).Select(x => new InfoModel
                    {
                        Id = x.Id,

                        To = x.To,
                        From = x.From,

                        Rate = x.Rate,

                        StartDate = x.StartDate,
                        FinishDate = x.FinishDate,


                    }).ToListAsync(cancellationToken);
                return data;
            });

            return new ServiceDto<List<InfoModel>>()
            {
                Data = data
            };
        }
    }
}
