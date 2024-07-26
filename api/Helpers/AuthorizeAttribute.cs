using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Entities.DbModels;

namespace api.Helpers
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    //{
    //    public void OnAuthorization(AuthorizationFilterContext context)
    //    {
            
    //        var user = context.HttpContext.Items["User"];
    //        if (user == null )
    //        {
    //            // not logged in
    //            context.Result = new JsonResult(new { message = "Sin Autorizacion" }) { StatusCode = StatusCodes.Status401Unauthorized };
    //        }
    //    }
    //}
}
