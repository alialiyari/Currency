using MagicOnion;
using Models;

namespace Sms
{
    public interface ISmsService : IService<ISmsService>
    {
        UnaryResult<ServiceDto> Send(string To, string Content);
    }
}