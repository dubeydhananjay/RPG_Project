namespace RPG.Control
{
    public interface IRaycastable
    {
        CursorType ComponentCursorType { get; }
        bool HandleRaycast(PlayerController callingController);
    }
}
