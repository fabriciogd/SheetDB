namespace SheetDB.Model
{
    using SheetDB.Enum;

    public class Permission
    {
        public string Email { get; set; }

        public Role Role { get; set; }

        public Type Type { get; set; }

        public Permission(string email, Role role, Type type)
        {
            this.Email = email;
            this.Role = role;
            this.Type = type;
        }
    }
}
