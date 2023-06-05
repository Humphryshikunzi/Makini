namespace Makini.Src.Models
{
    /// <summary>
    /// Model for SocialProfile
    /// </summary>
    [Preserve(AllMembers = true)]
    public class Profile
    {     
        private string imagePath;  
        public string Name { get; set; } 

        public string ImagePath
        {
            get { return imagePath; }
            set { this.imagePath = value; }
        }         
    }
}