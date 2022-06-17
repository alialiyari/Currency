using MagicOnion;
using Models;

namespace Rate
{
    public interface IRateService : IService<IRateService>
    {
        UnaryResult<ServiceDto<List<NodeModel>>> Find(string From, string To);
        UnaryResult<ServiceDto<List<InfoModel>>> List();
        UnaryResult<ServiceDto<long?>> Save(SaveModel saveModel);
    }
}