using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using AzureMediaService.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace AzureMediaService.Controllers
{
    public class EncodeMediaController : Controller
    {
        // GET: EncodeMedia
        public ActionResult Index()
        {
            return View();
        }
        
        // GET: EncodeMedia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EncodeMedia/Create
        [HttpPost]
        public ActionResult Create(MediaContentModel mediaContent)
        {
            try
            {
                var encode = new EncodeMediaService();
                encode.Execute(mediaContent);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Parse the XML error message in the Media Services response and create a new
                // exception with its content.

                ex = MediaServicesExceptionParser.Parse(ex);

                Debug.WriteLine(ex.Message);

                return RedirectToAction("Index", "Home");
            }
        }
        
        // GET: EncodeMedia/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index", "Home");
        }

        // POST: EncodeMedia/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
