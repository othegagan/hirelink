using hirelink.DbContext;
using hirelink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace hirelink.Controllers {

    public class StatisticsController : Controller {

        private readonly HirelinkDbContext _db;

        public StatisticsController(HirelinkDbContext db) {
            _db = db;
        }

        [HttpGet("/stats")]
        public IActionResult Index() {
            Statistics stats = new Statistics();
            stats = _db.GetStatistics();
            return View(stats);
        }
    }
}