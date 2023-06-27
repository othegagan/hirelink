using hirelink.DbContext;
using hirelink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace hirelink.Controllers {
    [Authorize]
    public class JobsController : Controller {
        private readonly HirelinkDbContext _db;

        public JobsController(HirelinkDbContext db) {
            _db = db;
        }

        // GET: /Jobs
        [HttpGet]
        public IActionResult Jobs() {
            List<Job> jobs = new List<Job>();
            try {
                jobs = _db.GetAllJobs();
            } catch (Exception ex) {
                TempData["errorMessage"] = ex.Message;
            }
            return View(jobs);
        }


        //GET : /Jobs/5     used to retrieve candidates of a particular job using job ID
        [HttpGet("jobs/{jobID}")]
        public IActionResult Index(int jobID) {
            List<Candidate> candidates = _db.GetCandidatesByJobId(jobID);
            return View(candidates);
        }



        // GET: Jobs/Create
        [HttpGet("jobs/create")]
        public IActionResult Create() {
            return View();
        }


        // POST : Jobs/Insert
        [HttpPost]
        public IActionResult Insert(Job job) {
            if (!ModelState.IsValid) {
                TempData["errorMessage"] = "Model data is invalid";
            }
            bool result = _db.InsertJob(job);
            if (!result) {
                TempData["errorMessage"] = "Unable to create the job";
            }
            TempData["successMessage"] = "Job create successfully";

            return RedirectToAction("Jobs");
        }


        // GET : Jobs/Edit/5
        [HttpGet]
        public IActionResult Edit(int id) {
            try {
                Job job = _db.GetJobById(id);
                if (id == 0) {
                    TempData["errorMessage"] = $"Job details not found with Job ID  : {id}";
                    return RedirectToAction("Jobs");
                }
                return View(job);

            } catch (Exception ex) {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }


        //POST : Jobs/Update
        [HttpPost]
        public IActionResult Update(Job job) {
            try {
                if (!ModelState.IsValid) {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View();
                }

                bool result = _db.UpdateJobDetails(job);

                if (result) {
                    TempData["successMessage"] = "Job details updated successfully";
                    return RedirectToAction("Jobs");
                } else {
                    TempData["errorMessage"] = "Unable to update the job details";
                    return View();
                }
            } catch (Exception ex) {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }


        //GET :  Upload Candidates
        [HttpGet("jobs/upload/{jobID}")]
        public IActionResult Upload(int jobID) {
            return View(jobID);
        }


        // GET  upload candidates from excel
        [HttpPost]
        public IActionResult Upload(IFormFile uploadedExcelFile, int id) {
            string url = "/jobs/" + id;
            if (uploadedExcelFile == null || uploadedExcelFile.FileName == null) {
                // Handle the null scenario
                // For example, return a BadRequest or throw an exception
                TempData["errorMessage"] = "No file uploaded.";
                return View();
            }

            var mainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExcelFilesUploaded");
            if (!Directory.Exists(mainPath)) {
                Directory.CreateDirectory(mainPath);
            }

            var fileName = Path.GetFileNameWithoutExtension(uploadedExcelFile.FileName);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var uniqueFileName = $"{fileName}_{timestamp}{Path.GetExtension(uploadedExcelFile.FileName)}";

            var filePath = Path.Combine(mainPath, uniqueFileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
                uploadedExcelFile.CopyTo(stream);
            }

            string extension = Path.GetExtension(uniqueFileName);
            string conString = string.Empty;

            switch (extension) {
                case ".xls":
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0";
                    break;

                case ".xlsx":
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0";
                    break;
            }

            using (OleDbConnection excelConnection = new(conString)) {
                excelConnection.Open();
                DataTable? excelSchema = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                List<string>? sheetNames = new List<string>();

                foreach (DataRow row in excelSchema.Rows) {
                    string? sheetName = row["TABLE_NAME"].ToString();
                    if (sheetName.EndsWith("$"))
                        sheetNames.Add(sheetName);
                }

                foreach (string sheetName in sheetNames) {
                    string query = "SELECT * FROM [" + sheetName + "]";

                    using (OleDbCommand command = new OleDbCommand(query, excelConnection)) {
                        using (OleDbDataReader dr = command.ExecuteReader()) {

                            try {
                                while (dr.Read()) {
                                    Candidate candidate = new Candidate();
                                    candidate.JobID = id;
                                    candidate.Name = (string)dr["Name"];
                                    candidate.Phone = ((double)dr["Phone"]).ToString(); ;
                                    candidate.Email = (string)dr["Email"];
                                    candidate.Address = (string)dr["Address"];
                                    candidate.Gender = (string)dr["Gender"];
                                    candidate.Education = (string)dr["Education"];
                                    candidate.InstituteName = (string)dr["InstituteName"];
                                    candidate.PassingYear = Convert.ToInt32((double)dr["PassingYear"]);
                                    candidate.Experience = Convert.ToInt32((double)dr["Experience"]);
                                    candidate.Designation = (string)dr["Designation"];
                                    candidate.Skills = (string)dr["Skills"];
                                    candidate.CurrentCTC = Convert.ToInt32((double)dr["CurrentCTC"]);
                                    candidate.ExpectedCTC = Convert.ToInt32((double)dr["ExpectedCTC"]);
                                    candidate.PresentCompany = (string)dr["PresentCompany"];
                                    candidate.ResumeFileLink = (string)dr["ResumeFileLink"];

                                    _db.InsertCandidate(candidate);

                                }
                                TempData["successMessage"] = "Candidates imported successfully";
                                return Redirect(url); ;
                            } catch (Exception ex) {
                                TempData["errorMessage"] = ex.Message;
                                return View();
                            }
                        }
                    }
                }

                excelConnection.Close();
            }

            // Get the Referer header from the request
            string referer = Request.Headers["Referer"].ToString();
            // Redirect to the previous page
            return Redirect(referer);
        }


        [HttpPost("jobs/updatestatus")]
        public IActionResult UpdateStatus(int jobID, string newJobStatus, string updatedBy) {
            try {
                bool result = _db.UpdateJobStatus(jobID, newJobStatus, updatedBy);
                if (result) {
                    TempData["successMessage"] = $"Job {newJobStatus} successfully";
                    return RedirectToAction("Jobs");
                } else {
                    TempData["errorMessage"] = "Unable to update the job details";
                    return RedirectToAction("Jobs");
                }
            } catch (Exception ex) {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Jobs");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
