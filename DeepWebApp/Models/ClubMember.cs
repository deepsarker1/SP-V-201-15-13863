namespace DeepWebApp.Models
{
    public class ClubMember
    {
        public int MemberId { get; set; }
        public string StudentId { get; set; }
        public List<Student> StudentList { get; set; }
    }
}
