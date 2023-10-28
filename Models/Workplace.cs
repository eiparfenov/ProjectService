namespace Models;

public class Workplace
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }

    public List<EquipmentModel>? EquipmentModels { get; set; }

    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
}