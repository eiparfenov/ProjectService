namespace Models;

public class Equipment
{
    public Guid Id { get; set; }
    public string? InvNumber { get; set; }
    public string? Comment { get; set; }
    
    public EquipmentModel? EquipmentModel { get; set; }
    public Guid EquipmentModelId { get; set; }
}