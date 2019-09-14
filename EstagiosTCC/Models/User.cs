namespace EstagiosTCC.Models
{
    public class User : FirebaseModel
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UrlPhoto { get; set; }

        public User()
        {
            Uid = string.Empty;
            DisplayName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            UrlPhoto = string.Empty;
        }
    }
}