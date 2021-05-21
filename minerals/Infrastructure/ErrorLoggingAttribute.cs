using Microsoft.AspNetCore.Mvc.Filters;
using Minerals.Contexts;
using Models;
using System;

namespace Minerals.Infrastructure
{
    public class ErrorLoggingAttribute : ExceptionFilterAttribute
    {
        private DataContext dbcontext;
        public ErrorLoggingAttribute(DataContext ctx) => dbcontext = ctx;
        /// <summary> Global exception handler. writes all exceptions to the database</summary>
        public override void OnException(ExceptionContext exceptionContext)
        {
            base.OnException(exceptionContext);

            var eventLog = new EventLog
            {
                EventMessage = exceptionContext?.Exception?.Message,
                Type = "ApplicationError",
                Source = exceptionContext?.Exception.StackTrace,
                StatusCode = "Error",
                CreateDate = DateTime.Now
            };
            dbcontext.EventLogs.Add(eventLog);
            dbcontext.SaveChanges();
        }
    }
}

