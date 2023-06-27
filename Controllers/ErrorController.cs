using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hirelink.DbContext;
using hirelink.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;

namespace project.Controllers {

    public class ErrorController : Controller {
        [Route("Error")]
        public IActionResult Index() {
            // Custom error handling logic
            return View("Error"); // Assuming you have an Error.cshtml view for displaying the error page
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode) {
            // Custom error handling logic based on different HTTP status codes
            return View("Error"); // Assuming you have an Error.cshtml view for displaying the error page
        }
    }

}
