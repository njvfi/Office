namespace Офіс.DAL.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
    public enum Role
    {
        User,
        Member
    }
}
