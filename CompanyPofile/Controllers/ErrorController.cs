using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BoGroent.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Przepraszamy, ale ta strona nie istnieje.";
                    break;
                default:
                    ViewBag.ErrorMessage = "Przepraszamy, wystąpił problem wewnętrzny.";
                    break;
            }
            return View("NotFound");
        }
    }
}
