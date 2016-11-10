namespace SheetDB.Implementation
{
    using Enum;
    using SheetDB.Model;

    public interface IDatabasePermission
    {
        Permission Permission { get; set; }

        void Delete();

        IDatabasePermission Update(Role role);
    }
}
