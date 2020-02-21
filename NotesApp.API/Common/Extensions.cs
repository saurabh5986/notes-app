using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace NotesApp.API
{
    /// <summary>
    /// Extenion methods
    /// </summary>
    public class Extensions
    {
        /// <summary>
        /// Returns text file
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static HttpResponseMessage GetTextFile(string plainText)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            byte[] textAsBytes = Encoding.Unicode.GetBytes(plainText);

            MemoryStream stream = new MemoryStream(textAsBytes);

            httpResponseMessage.StatusCode = HttpStatusCode.OK;

            httpResponseMessage.Content = new StreamContent(stream);

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ProcessedOCR_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".txt"
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            return httpResponseMessage;
        }

        /// <summary>
        /// Accepts Base64String and save image to folder and returs path of the file
        /// </summary>
        /// <param name="imageBase64String"></param>
        /// <returns></returns>
        public static string LoadImage(string imageBase64String)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            byte[] bytes = Convert.FromBase64String(imageBase64String);

            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms);
            }

            var filename = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".jpg";

            var path = HttpContext.Current.Server.MapPath("~/uploads/image" + filename);
            File.WriteAllBytes(path, Convert.FromBase64String(imageBase64String));
            return path;
        }
    }
}