namespace DiscGolfRounds.API.Areas.Courses.Requests
{
    public class NewCourseRequest
    {
        public string courseName { get; set; }
        public string variantName { get; set; }
        public List<int> holePars { get; set; }
    }
}
