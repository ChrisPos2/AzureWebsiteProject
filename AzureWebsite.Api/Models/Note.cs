namespace AzureWebsite.Api.Models
{
    public class Note
    {
        public Note()
        {
            CreatedOn = DateTime.UtcNow;
        }

        public Note(string title, string content) : this()
        {
            Title = title;
            Content = content;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}

//db credentials
//user:chris   pass:Pos12345