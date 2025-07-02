using System.Text.RegularExpressions;

namespace NWRWS.Webs.Common
{
    public class MyMiddleware
    {
        RequestDelegate _next;

        bool isHomeLoad;

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
            isHomeLoad = true;
        }
        public async Task Invoke(HttpContext context)
        {
            //Console.WriteLine("Middleware");

            string mainPath = context.Request.Path.ToString();

            //Regex r = new Regex(@"^[a-zA-Z0-9-_/]+$");
            //string strPath = NWRWS.Webs.Common.Functions.ValidateHomePage(mainPath);
            //if (strPath != mainPath && (mainPath == "" || mainPath == "/"))
            //{
            //    isHomeLoad = true;
            //}
            //else if(strPath == mainPath)
            //{
            //    context.Response.Redirect(strPath, true);
            //    isHomeLoad = false;
            //    return;
            //}

            //if (r.IsMatch(mainPath) && (mainPath == "" || mainPath == "/") && isHomeLoad)
            //{
            //    string strHomePath = NWRWS.Webs.Common.Functions.GetHomePage();
            //    if (mainPath != strHomePath)
            //    {
            //        context.Response.Redirect(strHomePath, true);
            //        isHomeLoad = false;
            //        return;
            //    }
            //}
            //else
            //{
            //}

            //string strPath = NWRWS.Webs.Common.Functions.GetPathFrom(context.Request.Path.ToString());

            //// modify url (from mbudnik's answer)
            ////context.Request.Path = (mainPath);
            //if (strPath != mainPath)
            //{
            //    context.Response.Redirect(strPath, true);
            //    //context.Request.PathBase=(strPath);
            //    return;
            //}

            await _next.Invoke(context);
        }

    }
}
