using hirelink.Controllers;
using hirelink.Models;
using hirelink.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using Candidate = hirelink.Models.Candidate;

namespace hirelink.DbContext {
    public class HirelinkDbContext {

        public HirelinkDbContext() { }

        public static IConfiguration? Configuration { get; set; }

        private string GetConnectionString() {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("conn") ?? throw new Exception("Connection string not found.");
        }




        /* ------- Below are Job post related methods  ------*/

        public List<Job> GetAllJobs() {
            List<Job> jobs = new();
            using (SqlConnection connection = new(GetConnectionString())) {
                using SqlCommand command = new("[dbo].GetAllJobs", connection);
                try {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Job job = new() {
                            JobId = (int)reader["job_id"],
                            JobTitle = (string)reader["job_title"],
                            Department = (string)reader["department"],
                            Location = (string)reader["Location"],
                            JobType = (string)reader["job_type"],
                            JobStatus = (string)reader["job_status"],
                            PostedOn = (DateTime)reader["posted_on"],
                            PostedBy = (string)reader["posted_by"],
                            ClosedOn = reader["closed_on"] != DBNull.Value ? (DateTime?)reader["closed_on"] : null,
                            ClosedBy = reader["closed_by"] != DBNull.Value ? (string)reader["closed_by"] : null,
                            CandidateCount = (int)reader["candidate_count"],
                            HiredCount = (int)reader["hired_count"],
                            RejectedCount = (int)reader["rejected_count"]
                        };

                        jobs.Add(job);
                    }
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while fetching job data: " + ex.Message);
                    throw;
                }
            }
            return jobs;
        }


        public Job GetJobById(int id) {
            Job job = new();
            using (SqlConnection connection = new(GetConnectionString())) {
                try {
                    using SqlCommand command = new("[dbo].GetJobById", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@JobID", id);

                    connection.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) {
                        job.JobId = (int)reader["job_id"];
                        job.JobTitle = (string)reader["job_title"];
                        job.Department = (string)reader["department"];
                        job.Location = (string)reader["Location"];
                        job.JobType = (string)reader["job_type"];
                        job.Skills = (string)reader["skills"];
                        job.ProjectName = (string)reader["project_name"];
                        job.ClientName = (string)reader["client_name"];
                        job.OwnerName = (string)reader["owner_name"];
                        job.NumberOfOpenings = (int)reader["no_of_openings"];
                    }
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while fetching job data: " + ex.Message);
                    throw;
                }
            }
            return job;
        }


        public bool InsertJob(Job job) {
            SqlConnection sqlConnection = new(GetConnectionString());
            using SqlConnection connection = sqlConnection;
            try {
                using SqlCommand command = new("[dbo].[InsertJob]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@JobTitle", job.JobTitle);
                command.Parameters.AddWithValue("@Department", job.Department);
                command.Parameters.AddWithValue("@Location", job.Department);
                command.Parameters.AddWithValue("@Skills", job.Skills);
                command.Parameters.AddWithValue("@JobType", job.JobType);
                command.Parameters.AddWithValue("@ClientName", job.ClientName);
                command.Parameters.AddWithValue("@ProjectName", job.ProjectName);
                command.Parameters.AddWithValue("@OwnerName", job.OwnerName);
                command.Parameters.AddWithValue("@NumberOfOpenings", job.NumberOfOpenings);
                command.Parameters.AddWithValue("@PostedBy", job.PostedBy);
                connection.Open();
                int inserted = command.ExecuteNonQuery();
                return inserted > 0;

            } catch (Exception ex) {
                Console.WriteLine("An error occurred while fetching job data: " + ex.Message);
                throw;
            }
        }


        public bool UpdateJobDetails(Job job) {
            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                try {
                    using SqlCommand command = new("[dbo].UpdateJobDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@JobId", job.JobId);
                    command.Parameters.AddWithValue("@JobTitle", job.JobTitle);
                    command.Parameters.AddWithValue("@Department", job.Department);
                    command.Parameters.AddWithValue("@Location", job.Location);
                    command.Parameters.AddWithValue("@JobType", job.JobType);
                    command.Parameters.AddWithValue("@Skills", job.Skills);
                    command.Parameters.AddWithValue("@ProjectName", job.ProjectName);
                    command.Parameters.AddWithValue("@ClientName", job.ClientName);
                    command.Parameters.AddWithValue("@OwnerName", job.OwnerName);
                    command.Parameters.AddWithValue("@NumberOfOpenings", job.NumberOfOpenings);

                    connection.Open();
                    int updated = command.ExecuteNonQuery();
                    return updated > 0;
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while updating job data: " + ex.Message);
                    throw;
                }
            }
        }


        public bool UpdateJobStatus(int jobID, string status, string updatedBy) {
            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                try {
                    using SqlCommand command = new SqlCommand("[dbo].UpdateJobStatus", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@JobID", jobID);
                    command.Parameters.AddWithValue("@NewJobStatus", status);
                    command.Parameters.AddWithValue("@UpdatedBy", updatedBy);

                    connection.Open();
                    int updated = command.ExecuteNonQuery();
                    return updated > 0;
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while updating job status: " + ex.Message);
                    throw;
                }
            }
        }


        public List<Candidate> GetCandidatesByJobId(int id) {
            List<Candidate> candidates = new List<Candidate>();
            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                try {
                    using SqlCommand command = new SqlCommand("[dbo].GetCandidatesByJobID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@JobID", id);

                    connection.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Candidate candidate = new Candidate();
                        candidate.CandidateId = (int)reader["candidate_id"];
                        candidate.JobID = (int)reader["job_id"];
                        candidate.Name = (string)reader["name"];
                        candidate.Phone = (string)reader["phone"];
                        candidate.Email = (string)reader["email"];
                        candidate.Address = (string)reader["address"];
                        candidate.Gender = (string)reader["gender"];
                        candidate.Education = (string)reader["education"];
                        candidate.InstituteName = (string)reader["institute_name"];
                        candidate.PassingYear = (int)reader["passing_year"];
                        candidate.Experience = (int)reader["experience"];
                        candidate.Designation = (string)reader["designation"];
                        candidate.Skills = (string)reader["skills"];
                        candidate.CurrentCTC = Convert.ToDecimal(reader["current_ctc"]);
                        candidate.ExpectedCTC = Convert.ToDecimal(reader["expected_ctc"]);
                        candidate.PresentCompany = (string)reader["present_company"];
                        candidate.Progress = (string)reader["progress"];
                        candidate.ResumeFileLink = (string)reader["resume_file_link"];
                        candidate.Status = reader["status"].ToString();

                        candidates.Add(candidate);
                    }
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while fetching candidates data: " + ex.Message);
                    throw;
                }
            }
            return candidates;
        }


        public Statistics? GetStatistics() {
            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                using (SqlCommand command = new SqlCommand("GetStats", connection)) {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) {
                        Statistics model = new Statistics {
                            JobsCount = Convert.ToInt32(reader["JobsCount"]),
                            CandidatesCount = Convert.ToInt32(reader["CandidatesCount"]),
                            HiredCount = Convert.ToInt32(reader["HiredCount"]),
                            RejectedCount = Convert.ToInt32(reader["RejectedCount"]),
                            OpenJobsCount = Convert.ToInt32(reader["OpenJobsCount"]),
                            ClosedJobsCount = Convert.ToInt32(reader["ClosedJobsCount"]),
                            OtherProgressCount = Convert.ToInt32(reader["OtherProgressCount"])
                        };
                        return model;
                    }
                    return null;
                }
            }
        }




        /* ------- Below are Candidate related methods ------*/

        public bool InsertCandidate(Candidate candidate) {
            using (SqlConnection connection = new(GetConnectionString())) {
                try {
                    using SqlCommand command = new("[dbo].[InsertCandidate]", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@JobID", candidate.JobID);
                    command.Parameters.AddWithValue("@Name", candidate.Name);
                    command.Parameters.AddWithValue("@Phone", candidate.Phone);
                    command.Parameters.AddWithValue("@Email", candidate.Email);
                    command.Parameters.AddWithValue("@Address", candidate.Address);
                    command.Parameters.AddWithValue("@Gender", candidate.Gender);
                    command.Parameters.AddWithValue("@Education", candidate.Education);
                    command.Parameters.AddWithValue("@InstituteName", candidate.InstituteName);
                    command.Parameters.AddWithValue("@PassingYear", candidate.PassingYear);
                    command.Parameters.AddWithValue("@Experience", candidate.Experience);
                    command.Parameters.AddWithValue("@Designation", candidate.Designation);
                    command.Parameters.AddWithValue("@Skills", candidate.Skills);
                    command.Parameters.AddWithValue("@CurrentCTC", candidate.CurrentCTC);
                    command.Parameters.AddWithValue("@ExpectedCTC", candidate.ExpectedCTC);
                    command.Parameters.AddWithValue("@PresentCompany", candidate.PresentCompany);
                    command.Parameters.AddWithValue("@ResumeFileLink", candidate.ResumeFileLink);

                    connection.Open();
                    int inserted = command.ExecuteNonQuery();
                    return inserted > 0;

                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while inserting candidte: " + ex.Message);
                    throw;
                }
            }

        }


        public bool InsertFeedback(FeedbackViewModel feedbackViewModel, string userName) {
            try {
                using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                    connection.Open();

                    // Create overall feedback command
                    SqlCommand overallFeedbackCommand = new SqlCommand("InsertFeedback", connection);
                    overallFeedbackCommand.CommandType = CommandType.StoredProcedure;

                    // Set overall feedback command parameters
                    overallFeedbackCommand.Parameters.AddWithValue("@CandidateId", feedbackViewModel.CandidateId);
                    overallFeedbackCommand.Parameters.AddWithValue("@interviewerEmail", feedbackViewModel.InterviewerEmail);
                    overallFeedbackCommand.Parameters.AddWithValue("@roundComments", feedbackViewModel.Comments);
                    overallFeedbackCommand.Parameters.AddWithValue("@interviewerName", feedbackViewModel.InterviewerName);
                    overallFeedbackCommand.Parameters.AddWithValue("@topicsJson", JsonConvert.SerializeObject(feedbackViewModel.Topics));
                    overallFeedbackCommand.Parameters.AddWithValue("@ActivityDoneBy", userName);

                    // Execute the overall feedback command
                    int updated = overallFeedbackCommand.ExecuteNonQuery();
                    return updated > 0;
                }
            } catch (Exception ex) {
                Console.WriteLine("An error occurred while updating candidate status: " + ex.Message);
                return false; // Return false in case of an error
            }
        }


        public bool InsertCandidateTimeline(CandidateTimeLine timeline) {
            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                try {
                    using SqlCommand command = new SqlCommand("dbo.InsertCandidateTimeline", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CandidateId", timeline.CandidateId);
                    command.Parameters.AddWithValue("@ActivityDoneBy", timeline.ActivityDoneBy);
                    command.Parameters.AddWithValue("@ActivityPerformed", timeline.ActivityPerformed);

                    connection.Open();
                    int updated = command.ExecuteNonQuery();
                    return updated > 0;
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while updating candidate timeline: " + ex.Message);
                    throw;
                }
            }

        }


        public Candidate GetCandidateById(int candidateID) {
            Candidate candidate = new Candidate();
            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                try {
                    using SqlCommand command = new SqlCommand("[dbo].GetCandidateByID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CandidateID", candidateID);

                    connection.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) {
                        candidate.CandidateId = (int)reader["candidate_id"];
                        candidate.JobID = (int)reader["job_id"];
                        candidate.Name = (string)reader["name"];
                        candidate.Phone = (string)reader["phone"];
                        candidate.Email = (string)reader["email"];
                        candidate.Address = (string)reader["address"];
                        candidate.Gender = (string)reader["gender"];
                        candidate.Education = (string)reader["education"];
                        candidate.InstituteName = (string)reader["institute_name"];
                        candidate.PassingYear = (int)reader["passing_year"];
                        candidate.Experience = (int)reader["experience"];
                        candidate.Designation = (string)reader["designation"];
                        candidate.Skills = (string)reader["skills"];
                        candidate.CurrentCTC = Convert.ToDecimal(reader["current_ctc"]);
                        candidate.ExpectedCTC = Convert.ToDecimal(reader["expected_ctc"]);
                        candidate.PresentCompany = (string)reader["present_company"];
                        candidate.Progress = (string)reader["progress"];
                        candidate.ResumeFileLink = (string)reader["resume_file_link"];
                        candidate.Status = reader["status"].ToString();
                        candidate.Details = reader["details"].ToString();
                        candidate.Comments = reader["comments"].ToString();

                    }
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while fetching candidate data: " + ex.Message);
                    throw;
                }
            }
            return candidate;
        }


        public List<FeedbackViewModel> GetFeedbacksByCandidateId(int candidateID) {
            List<FeedbackViewModel> feedbacks = new List<FeedbackViewModel>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                try {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[dbo].GetCandidateFeedbacks", connection)) {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CandidateId", candidateID);

                        using (SqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                FeedbackViewModel feedback = new FeedbackViewModel {
                                    FeedbackId = (int)reader["feedback_id"],
                                    Round = (string)reader["round"],
                                    InterviewerEmail = (string)reader["interviewer_email"],
                                    Comments = (string)reader["comments"],
                                    FeedbackDate = (DateTime)reader["feedback_date"],
                                    InterviewerName = (string)reader["interviewer_name"],
                                    Topics = new List<FeedbackTopicViewModel>() // Initialize the Topics list
                                };

                                feedbacks.Add(feedback);
                            }
                        }
                    }

                    // Retrieve feedback topics for each feedback
                    foreach (FeedbackViewModel feedback in feedbacks) {
                        using SqlCommand topicCommand = new("GetFeedbackTopics", connection);
                        topicCommand.CommandType = CommandType.StoredProcedure;
                        topicCommand.Parameters.AddWithValue("@FeedbackId", feedback.FeedbackId);

                        using SqlDataReader topicReader = topicCommand.ExecuteReader();
                        while (topicReader.Read()) {
                            FeedbackTopicViewModel topicViewModel = new() {
                                FeedbackTopicId = (int)topicReader["feedback_topic_id"],
                                TopicName = (string)topicReader["topic_name"],
                                TopicRating = (int)topicReader["topic_rating"]
                            };

                            feedback.Topics.Add(topicViewModel);
                        }
                    }
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while fetching candidates data: " + ex.Message);
                    throw;
                }
            }

            return feedbacks;
        }


        public List<CandidateTimeLine> GetCandidateTimelineByCandidateId(int candidateID) {
            List<CandidateTimeLine> TimeLines = new();
            using (SqlConnection connection = new(GetConnectionString())) {
                try {
                    using SqlCommand command = new("[dbo].GetCandidateTimelineByCandidateId", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CandidateId", candidateID);
                    connection.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        CandidateTimeLine timeLine = new() {
                            JobId = (int)reader["job_id"],
                            CandidateId = (int)reader["candidate_id"],
                            ActivityDoneBy = (string)reader["activity_done_by"],
                            ActivityDate = reader["activity_date"] != DBNull.Value ? (DateTime)reader["activity_date"] : null,
                            ActivityPerformed = (string)reader["activity_performed"],
                        };
                        TimeLines.Add(timeLine);
                    }
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while fetching candidates data: " + ex.Message);
                    throw;
                }
            }
            return TimeLines;
        }


        public bool UpdateCandidateProgress(int candidateId, string progress, string userName, string comments) {
            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                try {
                    using SqlCommand command = new SqlCommand("dbo.UpdateCandidateProgress", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CandidateId", candidateId);
                    command.Parameters.AddWithValue("@Progress", progress);
                    command.Parameters.AddWithValue("@ActivityDoneBy", userName);
                    command.Parameters.AddWithValue("@Comments", comments);

                    connection.Open();
                    int updated = command.ExecuteNonQuery();
                    return updated > 0;
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while updating candidate progress: " + ex.Message);
                    throw;
                }
            }
        }


        public bool UpdateCandidateStatus(int candidateId, string newStatus, string details, string userName) {
            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                try {
                    using SqlCommand command = new SqlCommand("dbo.UpdateCandidateStatus", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CandidateId", candidateId);
                    command.Parameters.AddWithValue("@Status", newStatus);
                    command.Parameters.AddWithValue("@Details", details);
                    command.Parameters.AddWithValue("@ActivityDoneBy", userName);

                    connection.Open();
                    int updated = command.ExecuteNonQuery();
                    return updated > 0;
                } catch (Exception ex) {
                    Console.WriteLine("An error occurred while updating candidate status: " + ex.Message);
                    return false;
                }
            }
        }


        public User? VerifyCredentials(string userName) {
            using (SqlConnection connection = new SqlConnection(GetConnectionString())) {
                connection.Open();

                // Check if the username exists in super_tbl
                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM super_tbl WHERE Username = @Username", connection)) {
                    checkCommand.Parameters.AddWithValue("@Username", userName);
                    int userCount = (int)checkCommand.ExecuteScalar();

                    if (userCount > 0) {
                        // Retrieve user info
                        using (SqlCommand retrieveCommand = new SqlCommand("SELECT name, password, role FROM super_tbl WHERE Username = @Username", connection)) {
                            retrieveCommand.Parameters.AddWithValue("@Username", userName);

                            using (SqlDataReader reader = retrieveCommand.ExecuteReader()) {
                                if (reader.Read()) {
                                    User user = new User();

                                    user.Name = (string)reader["name"];
                                    user.Password = (string)reader["password"];
                                    user.Role = (string)reader["role"];

                                    return user;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

    }
}

