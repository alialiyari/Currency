using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Models;

namespace Rate
{
    public class SaveCommand : IRequest<ServiceDto<long?>>
    {
        public SaveModel SaveModel { get; set; }
    }

    class SaveCommandHandler : IRequestHandler<SaveCommand, ServiceDto<long?>>
    {
        private readonly IMemoryCache memoryCache;
        private readonly DatabaseContext databaseContext;

        public SaveCommandHandler(IMemoryCache memoryCache, DatabaseContext databaseContext)
        {
            this.memoryCache = memoryCache;
            this.databaseContext = databaseContext;
        }

        public async Task<ServiceDto<long?>> Handle(SaveCommand request, CancellationToken cancellationToken)
        {
            var isBeforeSaved = await databaseContext.Rates.Where(x => x.To == request.SaveModel.To)
                .Where(x => x.From == request.SaveModel.From).Where(x => x.FinishDate == null).SingleOrDefaultAsync(cancellationToken);

            if (isBeforeSaved != null) isBeforeSaved.FinishDate = DateTime.Now;

            var newEntity = databaseContext.Rates.Add(new Rate.RateEntity()
            {
                To = request.SaveModel.To,
                From = request.SaveModel.From,


                StartDate = DateTime.Now,
                Rate = request.SaveModel.Rate,
            });

            await databaseContext.SaveChangesAsync(cancellationToken);
            memoryCache.Remove("Rates");

            return new ServiceDto<long?>() { Data = newEntity.Entity.Id, Message = "Saved successfully :-)" };
        }
    }
}
