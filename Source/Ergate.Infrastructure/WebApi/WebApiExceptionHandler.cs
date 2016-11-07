namespace Ergate.Infrastructure.WebApi
{
    using System.Web.Http.ExceptionHandling;
    using System.Web.Http.Results;

    public class WebApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            base.Handle(context);

            context.Result = new BadRequestResult(context.ExceptionContext.Request);
        }
    }
}