using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CompanyPofile.Models;
using Microsoft.AspNetCore.Hosting;
using CompanyPofile.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Net.Http.Headers;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Cache;

namespace CompanyPofile.Controllers
{
    public class HomeController : Controller
    {
        private readonly CompanyDbContext contex;
        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, CompanyDbContext contex, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this.contex = contex;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            TempData["Index"] = "";
            return View();
        }

        [Route("Kontakt")]
        public IActionResult Kontakt()
        {
            return View();
        }

        //POST /cars/create
        [Route("Kontakt")]
        [HttpPost]
        [ValidateAntiForgeryToken] //protect against CSRF attacks 
        public async Task<IActionResult> Kontakt(Inquiry inquiry)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            // OPTIONAL: Set version truncation to none, so full versions will be returned
            // By default only minor versions will be returned (e.g. X.Y)
            // for other options see VERSION_TRUNCATION_* constants in DeviceParserAbstract class
            // add using DeviceDetectorNET.Parser;
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var dd = new DeviceDetector(userAgent);

            // OPTIONAL: Set caching method
            // By default static cache is used, which works best within one php process (memory array caching)
            // To cache across requests use caching in files or memcache
            // add using DeviceDetectorNET.Cache;
            dd.SetCache(new DictionaryCache());

            // OPTIONAL: If called, GetBot() will only return true if a bot was detected  (speeds up detection a bit)
            dd.DiscardBotInformation();

            // OPTIONAL: If called, bot detection will completely be skipped (bots will be detected as regular devices then)
            dd.SkipBotDetection();

            dd.Parse();

            if (dd.IsBot())
            {
                // handle bots,spiders,crawlers,...
                var botInfo = dd.GetBot();
                inquiry.Browser = "BOT";
                inquiry.OperatingSystem = "BOT";
                inquiry.Device = "BOT";
                inquiry.Brand = "BOT";
                inquiry.Model = "BOT";
            }
            else
            {
                var clientInfo = dd.GetClient(); // holds information about browser, feed reader, media player, ...
                var osInfo = dd.GetOs();
                var device = dd.GetDeviceName();
                var brand = dd.GetBrandName();
                var model = dd.GetModel();

                inquiry.Browser = $"{clientInfo.Match.Name} ({clientInfo.Match.Version})";
                inquiry.OperatingSystem = $"{osInfo.Match.Name} ({osInfo.Match.Version}) {osInfo.Match.Platform}";
                var deviceResult = device == "" ? inquiry.Device = "N/A" : inquiry.Device = device;
                var brandResult = brand == "" ? inquiry.Brand = "N/A" : inquiry.Brand = brand;
                var modelResult = model == "" ? inquiry.Model = "N/A" : inquiry.Model = model;
            }

            //local Environment
            //OperatingSystem os = Environment.OSVersion;
            //var platform = os.Platform.ToString();
            //var version = os.Version.ToString();
            //var servicePack = os.ServicePack.ToString();

            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            inquiry.IpAddress = remoteIpAddress;

            inquiry.CreatedDate = DateTime.Now;
            inquiry.Status = "New";
            inquiry.HoursSpend = 0;
            inquiry.HourPrice = 400;
            inquiry.InvoiceNo = "N/A";
            inquiry.Payment = "N/A";
            
            
            if (ModelState.IsValid)
            {
                var inquiryId = await contex.Inquiries.FirstOrDefaultAsync(x => x.Id == inquiry.Id);
                if (inquiryId != null)
                {
                    ModelState.AddModelError("", "Nastapil bland, ponow wpis.");
                    return View();
                }

                //string attachmentName = "noimagecar.png";
                //if (inquiry.AttachmentUpload != null)
                //{
                //    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/cars");
                //    attachmentName = Guid.NewGuid().ToString() + "_" + inquiry.AttachmentUpload.FileName;
                //    string filePath = Path.Combine(uploadsDir, attachmentName);
                //    FileStream fs = new FileStream(filePath, FileMode.Create);
                //    await inquiry.AttachmentUpload.CopyToAsync(fs);
                //    fs.Close();
                //}
                //inquiry.Attachment = attachmentName;

                

                contex.Add(inquiry);
                await contex.SaveChangesAsync();

                TempData["Success"] = "Wiadomosc zostala wyslana!";

                return RedirectToAction("Kontakt");
            }

            return View();
        }


        [Route("Projekty")]
        public IActionResult Projekty()
        {
            return View();
        }
        [Route("Cenydoradzwa")]
        public IActionResult Cenydoradzwa()
        {
            return View();
        }

        [Route("Onas")]
        public IActionResult Onas()
        {
            return View();
        }

        //[Route("Treningi")]
        //public IActionResult Treningi()
        //{
        //    return View();
        //}

        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
