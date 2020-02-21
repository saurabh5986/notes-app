using System.Web;
using System.Web.WebPages;

namespace NotesApp
{
    public class RequestHelpers
    {
        public static string RequestIPAddress()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                string retVal = null;
                if (context.Request.Headers["X-Forwarded-For"] != null)
                {
                    retVal = context.Request.Headers["X-Forwarded-For"].ToString();
                }
                if (retVal.IsEmpty())
                {
                    if (context.Request.ServerVariables["REMOTE_ADDR"] != null)
                    {
                        retVal = context.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    }
                }
                if (retVal.IsEmpty())
                    retVal = "";
                return retVal;
            }
            return "";
        }
    }
}