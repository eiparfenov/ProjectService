namespace Models;

public class Role
{
    public Guid Id { get; set; }

    public RoleType RoleType { get; set; }
    
    public Department? MatchedDepartment { get; set; }
    public Guid? DepartmentId { get; set; }
    
    public List<User>? Users { get; set; }
}

public enum RoleType
{
    Undefined = 0,
    Student = 1,
    Employee = 2,
    Receptionist = 3,
    AdminDepartment = 4,
    SuperAdmin = 5
}