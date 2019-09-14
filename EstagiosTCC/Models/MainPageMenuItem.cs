namespace EstagiosTCC.Models
{
    public enum MenuItemType
    {
        Home,
        Course,
        NewInternship,
        SearchInternship,
        MyInterships,
        MyProfile,
        About,
        Logout
    }
    public class MainPageMenuItem
    {
        public MenuItemType Uid { get; set; }
        public string Title { get; set; }
    }
}