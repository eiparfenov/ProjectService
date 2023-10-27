using System.Text.RegularExpressions;

namespace WebApp.Client;

public static class Routes
{
    public const string DepartmentMatching = "/{DepartmentUrl}";
    public static class Admin
    {
        public const string Route = $"{DepartmentMatching}/admin";
        public static string Index(string departmentUrl) => FillTemplate(Route, departmentUrl);

        public static class Department
        {
            public const string Route =  $"{Admin.Route}/departments";
            public static string Index(string departmentUrl) => FillTemplate(Route, departmentUrl);
        }
    }
    
    #region PathsFilling
    private static readonly Regex TemplateReplacer = new(@"{(?<arg>\w+)\:?(?<type>[^}]*)}");
    private static string FillTemplate(string path, params string[] args) =>
        TemplateReplacer.Replace(path, m => args[m.Index - 1]);
    #endregion
}