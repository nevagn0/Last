namespace Last.Models
{
    public class AnimalViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Type { get; set; }
        public string PassportSeria { get; set; }
        public string PassportNumber { get; set; }
        public List<string> Vaccines { get; set; } = new List<string>();
    }
}
