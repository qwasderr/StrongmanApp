namespace StrongmanApp.Models
{
    public class ResultsTVF
    {
        public int ID { get; set; }
        public int athlete_ID {  get; set; }
        public string athlete_name { get; set; }
        public string event_name { get; set; }
        public string event_result { get; set;}
        public float event_points { get; set;}
        public float total_pts { get; set;}
    }
}
