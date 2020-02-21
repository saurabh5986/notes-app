using NotesApp.API.Models;
using NotesApp.Business.Models;
using Patagames.Ocr;
using Patagames.Ocr.Enums;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NotesApp.API.Controllers
{
    /// <summary>
    /// OCR Methods
    /// </summary>
    public class OCRController : ApiController
    {
        /// <summary>
        /// Accepts base64sring and process OCR on it and returns text file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ProcessImageOCR")]
        public HttpResponseMessage ProcessImageOCR(ImageData data)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                using (var db = new NotesAppEntities())
                {
                    if (db.Images.Where(s => s.ImageBase64String == data.base64String).Any())
                    {
                        var imagedata = db.Images.Where(s => s.ImageBase64String == data.base64String).FirstOrDefault();
                        if (imagedata != null)
                        {
                            string plainText = imagedata.OCRText;
                            return Extensions.GetTextFile(plainText);
                        }
                    }
                    if (!string.IsNullOrEmpty(data.base64String))
                    {
                        var ImagePath = Extensions.LoadImage(data.base64String);

                        if (!string.IsNullOrEmpty(ImagePath))
                        {
                            var ext = Path.GetExtension(ImagePath);
                            if (ext == ".jpg" || ext == ".jpeg")
                            {
                                using (var api = OcrApi.Create())
                                {
                                    api.Init(Languages.English, HttpContext.Current.Server.MapPath("~"));
                                    string plainText = api.GetTextFromImage(ImagePath);

                                    db.Images.Add(new Image
                                    {
                                        ImageBase64String = data.base64String,
                                        OCRText = plainText,
                                        CreatedUTC = DateTime.UtcNow
                                    });
                                    db.SaveChanges();

                                    return Extensions.GetTextFile(plainText);
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please give path for jpg or jpeg images only.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Please give path for jpg or jpeg images only.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide jpg image path to process ocr.");
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        ///// <summary>
        ///// Convert Image to Base64String
        ///// </summary>
        ///// <param name="ImagePath"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("ConvertImageToBase64")]
        //public HttpResponseMessage ConvertImageToBase64(string ImagePath)
        //{
        //    try
        //    {
        //        using (System.Drawing.Image image = System.Drawing.Image.FromFile(ImagePath))
        //        {
        //            using (MemoryStream m = new MemoryStream())
        //            {
        //                image.Save(m, image.RawFormat);
        //                byte[] imageBytes = m.ToArray();
        //                string base64String = Convert.ToBase64String(imageBytes);
        //                return Request.CreateResponse(HttpStatusCode.OK, base64String);
        //                //return base64String;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
        //    }
        //}
    }
}