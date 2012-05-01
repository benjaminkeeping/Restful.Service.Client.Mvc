using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Restful.Web.Client.Errors;
using Restful.Wiretypes;

namespace Restful.Service.Client.Mvc
{
    public class BaseClient : IClient
    {
        readonly ISessionProvider _sessionProvider;

        public BaseClient(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        protected ActionResult Try(Func<ActionResult> operation, Func<IEnumerable<Error>, ActionResult> onError)
        {
            try
            {
                return operation.Invoke();
            }
            catch (HttpError er)
            {
                if (er.GetType() == typeof(Http401))
                    return RedirectToLogin();
                return onError.Invoke(er.Errors);
            }
        }

        ActionResult RedirectToLogin()
        {
            return new RedirectResult("/sign-in");
        }

        protected ActionResult Try(Func<ActionResult> operation, Func<string, ActionResult> onError)
        {
            return Try(operation, errors => onError.Invoke(errors.Count() > 0
                ? errors.ElementAt(0).Value
                : "Sorry, but an unknown error has occured"));
        }

        public ActionResult ExecuteOrRedirectToLogin(Func<ActionResult> action)
        {
            if (_sessionProvider.IsLoggedIn()) return action.Invoke();
            return RedirectToLogin();
        }
    }
}
