namespace WebApp.Shared.Grpc;

public enum AdminPanelState
{
    Undefined = 0,
    Initial = 1,
    Update = 2,
    Create = 3,
    Delete = 4
}

public interface IHasAdminPanelState
{
    public AdminPanelState State { get; set; }
    public AdminPanelState PreviousState { get; set; }
}

public static class AdminPanelStateExtensions
{
    public static void PerformUpdate(this IHasAdminPanelState dto)
    {
        if (dto.State is AdminPanelState.Initial)
        {
            dto.State = AdminPanelState.Update;
        }
    }

    public static void PerformDelete(this IHasAdminPanelState dto)
    {
        dto.PreviousState = dto.State;
        dto.State = AdminPanelState.Delete;
    }

    public static void Recover(this IHasAdminPanelState dto)
    {
        dto.State = dto.PreviousState;
    }
}