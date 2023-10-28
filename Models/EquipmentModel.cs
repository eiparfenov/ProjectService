namespace Models;

public class EquipmentModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    public Department? Department { get; set; }
    public Guid DepartmentId { get; set; }

    public List<Equipment>? Equipments { get; set; }
    public List<Workplace>? Workplaces { get; set; }
}