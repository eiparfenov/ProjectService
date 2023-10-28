namespace Models;

public class Department
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string UrlTitle { get; set; } = default!;

    public List<EquipmentModel>? EquipmentModels { get; set; }
}