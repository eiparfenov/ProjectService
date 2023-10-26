namespace WebApp.Client;

public static class Routes
{
    public const string DepartmentMatching = "/{department}";
    public static class Admin
    {
        public const string AdminPanel = $"{DepartmentMatching}/admin";

        public static class Department
        {
            public const string Index =  $"{AdminPanel}/departments";
        }
    }
}