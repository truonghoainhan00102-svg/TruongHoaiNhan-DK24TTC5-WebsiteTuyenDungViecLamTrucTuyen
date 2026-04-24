using System;

namespace JobManagement.Models
{
    public class ApplicantViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CoverLetter { get; set; }
        public string CVPath { get; set; }
        public DateTime? ApplyDate { get; set; }
        public string Status { get; set; }
    }
}