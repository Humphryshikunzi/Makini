namespace Makini.Src.Models
{
    public class Message : BaseModel
    {
        public  string  Date { get; set; }
        public  string  Time { get; set; }
        public  string  DateAndTime { get; set; }
        public  string  Sender { get; set; }
        public  bool  IsMine { get; set; }
        public  string  Text { get; set; }
    }
}
