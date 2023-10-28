namespace Models;

public class EquipmentModelWorkplace
{
    public Guid WorkplaceId { get; set; }
    public Workplace? Workplace { get; set; }

    public Guid EquipmentModelId { get; set; }
    public EquipmentModel? EquipmentModel { get; set; }
}