using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSolution.Data.Constants;

namespace TestSolution.WebApp.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessions = context.HttpContext.Session.GetString(SystemConstants.Token);
            if (sessions == null)
            {
                context.Result = new RedirectToActionResult(ActionName.Login, ControllerName.Userlogin, null);
            }
            base.OnActionExecuting(context);
        }
    }
}
