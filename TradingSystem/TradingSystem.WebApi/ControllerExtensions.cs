using System;
using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace TradingSystem.WebApi
{
    public static class ControllerExtensions
    {
        public static ActionResult InternalServerError(this ControllerBase controller, object value = null)
        {
            if (controller is null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return controller.StatusCode((int)HttpStatusCode.InternalServerError, value);
        }
    }
}
