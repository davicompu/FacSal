﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ChrisJSherm.Filters
{
    public class RequireHttpsWebApiAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                HandleNonHttpsRequest(actionContext);
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }

        protected virtual void HandleNonHttpsRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            actionContext.Response.ReasonPhrase = "SSL Required";
        }
    }
}
