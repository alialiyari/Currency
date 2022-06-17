using MediatR;
using Models;

namespace Rate
{
    public class FindQuery : IRequest<ServiceDto<List<NodeModel>>>
    {
        public string To { get; set; }
        public string From { get; set; }
    }

    class FindQueryHandler : IRequestHandler<FindQuery, ServiceDto<List<NodeModel>>>
    {
        private readonly IMediator mediator;

        public FindQueryHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<ServiceDto<List<NodeModel>>> Handle(FindQuery request, CancellationToken cancellationToken)
        {
            // برعکس ریت ها رو هم باید وارد کنند
            ServiceDto<List<Rate.InfoModel>> Rates = await mediator.Send(new Rate.ListQuery() { }, cancellationToken);

            var nodes = new List<NodeModel>();
            var rootNode = new NodeModel() { Currency = request.From };
            nodes.Add(rootNode);


            void ChildrenGenerate(NodeModel nodeModel)
            {
                foreach (var rate in Rates.Data.Where(x => x.From == nodeModel.Currency).OrderBy(x => x.Rate))
                {
                    if (IsVisited(nodeModel, rate.To)) continue;
                    var newNode = new NodeModel() { ParentNode = nodeModel, Currency = rate.To, Rate = rate.Rate, };

                    nodes.Add(newNode);
                    if (rate.To == request.To) continue;

                    ChildrenGenerate(newNode);
                }
            }
            bool IsVisited(NodeModel nodeModel, string Currency)
            {
                if (nodeModel == null) return false;
                if (nodeModel.Currency == Currency) return true;

                return IsVisited(nodeModel.ParentNode, Currency);
            }

            ChildrenGenerate(rootNode);
            return new ServiceDto<List<NodeModel>>() { Data = nodes };
        }
    }
}
