using System;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace AsyncLocalBug
{
    public class Global : System.Web.HttpApplication
    {
        public static AsyncLocal<string> AsyncLocalState = new AsyncLocal<string>();

        public static SynchronizationContext _last;

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.Add(new Route("foo", new MyHandler()));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            _last = SynchronizationContext.Current;
            AsyncLocalState.Value = HttpContext.Current.Request.Path;
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var path = AsyncLocalState.Value;

            if (!ReferenceEquals(SynchronizationContext.Current, _last))
            {

            }

            if (path == null)
            {
            }
        }
    }

    public class MyHandler : IHttpHandler, IRouteHandler
    {
        public bool IsReusable => false;

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var path = Global.AsyncLocalState.Value;

            if (!ReferenceEquals(SynchronizationContext.Current, Global._last))
            {

            }

            if (path == null)
            {
            }

            return this;
        }

        public void ProcessRequest(HttpContext context)
        {
            var path = Global.AsyncLocalState.Value;

            if (!ReferenceEquals(SynchronizationContext.Current, Global._last))
            {

            }

            if (path == null)
            {
            }

            context.Response.Write("Path is: " + path);
        }
    }

}