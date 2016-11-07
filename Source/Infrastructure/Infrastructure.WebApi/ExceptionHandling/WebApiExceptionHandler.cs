namespace Infrastructure.WebApi.ExceptionHandling
{
    using System.Net.Http;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Http.Results;

    using Ergate.Infrastructure.AppBoot.Container;
    
    [Service(typeof(IExceptionHandler))]
    public class WebApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            base.Handle(context);

            context.Result = new BadRequestResult(context.ExceptionContext.Request);
        }
    }
}