using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Note.Attributes;

public class AuthAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var username = session.GetString("Username");

        if (string.IsNullOrEmpty(username))
        {
            context.Result = new RedirectToRouteResult(new
            {
                controller = "User",
                action = "Login"
            });
        }
        base.OnActionExecuting(context);
    }
}