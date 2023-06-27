using System.ComponentModel.Design;
using hirelink.DbContext;
using hirelink.Models;
using hirelink.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace hirelink.Controllers {

    [Authorize]
    public class CandidateController : Controller {

        private readonly HirelinkDbContext _db;
        public static IConfiguration? Configuration { get; set; }

        private string GetConnectionString() {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("conn") ?? throw new Exception("Connection string not found.");
        }

        public CandidateController(HirelinkDbContext db) {
            _db = db;
        }

        /* -------- Below are HttpGET requests -------- */

        [HttpGet("candidate/add/{jobID}")]
        public IActionResult Add() {
            return View();
        }


        [HttpGet("/Candidate/{id}")]
        public ActionResult Index(int id) {
            Candidate candidate = _db.GetCandidateById(id);
            List<FeedbackViewModel> feedbacks = _db.GetFeedbacksByCandidateId(id);
            List<CandidateTimeLine> timeLines = _db.GetCandidateTimelineByCandidateId(id);

            CandidateDetailsViewModel candidateViewModel = new CandidateDetailsViewModel {
                Candidate = candidate,
                Feedbacks = feedbacks,
                TimeLines = timeLines
            };

            return View(candidateViewModel);
        }


        [HttpGet("/Candidate/AddFeedback/{id}")]
        public IActionResult AddFeedback(int id) {
            return View(id);
        }



        /* -------- Below are Http POST requests -------- */

        [HttpPost]
        public IActionResult Add(Candidate candidate) {
            if (!ModelState.IsValid) {
                TempData["errorMessage"] = "Invalid Details";
            }
            bool result = _db.InsertCandidate(candidate);
            if (!result) {
                TempData["errorMessage"] = "Unable to add candidate";
            }
            TempData["successMessage"] = "Candidate added successfully";
            string url = "/jobs/" + candidate.JobID;
            return Redirect(url);
        }


        [HttpPost("/Candidate/UpdateCandidateProgress")]
        public IActionResult UpdateCandidateProgress(int candidateId, string progress, string userName, string comments) {
            bool result = _db.UpdateCandidateProgress(candidateId, progress, userName, comments);
            if (!result) {
                TempData["errorMessage"] = "Unable to move the candidate";
            }
            TempData["successMessage"] = $"Candidate moved to {progress} successfully";

            return RedirectToAction("Index", new { id = candidateId });
        }


        [HttpPost("/Candidate/UpdateCandidateStatus")]
        public IActionResult UpdateCandidateStatus(int candidateId, string interview_timings, string details, string userName) {
            string newStatus = "scheduled on " + interview_timings;
            bool result = _db.UpdateCandidateStatus(candidateId, newStatus, details, userName);
            if (!result) {
                TempData["errorMessage"] = "Unable to seducle the interview";
            }
            TempData["successMessage"] = $"Interview {newStatus} successfully";

            return RedirectToAction("Index", new { id = candidateId });
        }


        [HttpPost("/Candidate/AddFeedback/{id}")]
        public IActionResult AddFeedback(int candidateId, string interviewerName, string interviewerEmail, List<string> topics, List<int> ratings, string comments, string userName) {
            // Create an instance of FeedbackViewModel and populate its properties
            var feedbackViewModel = new FeedbackViewModel {
                CandidateId = candidateId,
                InterviewerName = interviewerName,
                InterviewerEmail = interviewerEmail,
                Comments = comments,
                FeedbackDate = DateTime.Now,
                Topics = new List<FeedbackTopicViewModel>()
            };

            // Iterate through the topics and ratings lists
            for (int i = 0; i < topics.Count; i++) {
                var topicViewModel = new FeedbackTopicViewModel {
                    TopicName = topics[i],
                    TopicRating = ratings[i]
                };

                feedbackViewModel.Topics.Add(topicViewModel);
            }

            bool result = _db.InsertFeedback(feedbackViewModel, userName);

            if (!result) {
                TempData["errorMessage"] = "Unable to store interview feedback";
            }
            TempData["successMessage"] = $"Interview feedback stored successfully";

            return Redirect("/Candidate/"+candidateId);
        }


    }
}
