using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskLabBackend.Filters
{
    public class CustomExpectionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(new
            {
                Success = false,
                Message = "An unexpected error occured",
                Error = context.Exception.Message
            })
            {
                StatusCode = 500
            };
            
            context.ExceptionHandled = true;
        }
    }
}
