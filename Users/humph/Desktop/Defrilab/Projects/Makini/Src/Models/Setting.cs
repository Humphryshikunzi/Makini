namespace Makini.Src.Models
{
    class Setting : BaseModel
    {
        public bool  NightModeOn { get; set; }
        public  bool  MarkettingNotificationsOn { get; set; }
        public  bool  AppNotificationsOn { get; set; }
    }
}
