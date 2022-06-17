using MediatR;
 
namespace Common
{
    public class CommandRequest<TResponse> : IRequest<TResponse>
    {
        public bool CommitChange { get; set; } = true;
    }
}
