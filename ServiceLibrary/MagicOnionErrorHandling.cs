using System;
using System.Threading.Tasks;

public class MagicOnionErrorHandling : MagicOnion.Server.MagicOnionFilterAttribute
{
    public override async ValueTask Invoke(MagicOnion.Server.ServiceContext context, Func<MagicOnion.Server.ServiceContext, ValueTask> next)
    {
        try
        {
            /* on before */
            await next(context); // next
            /* on after */
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }
}