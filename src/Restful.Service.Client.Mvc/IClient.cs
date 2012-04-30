using System;
using System.Web.Mvc;

namespace Restful.Service.Client.Mvc
{
    public interface IClient
    {
        ActionResult ExecuteOrRedirectToLogin(Func<ActionResult> action);
    }
}