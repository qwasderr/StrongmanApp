namespace StrongmanApp.Models
{
    public class AthlTVF
    {
        public int athlete_ID { get; set; }
        public string athlete_name { get; set; }
        public List<EventsTVF> Events { get; set; }
        public float? total_pts {  get; set; }
    }
}
