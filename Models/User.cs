namespace Models;

public class User
{
    public Guid Id { get; set; }
    public long? VkId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public List<Role>? Roles { get; set; }
}