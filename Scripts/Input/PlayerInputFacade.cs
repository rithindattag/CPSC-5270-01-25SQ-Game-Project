using Godot;

// Facade for simplifying input logic used by Player
public static class PlayerInputFacade
{
    public static bool IsMoveLeft() => Input.IsActionJustPressed("ui_left");

    public static bool IsMoveRight() => Input.IsActionJustPressed("ui_right");

    public static bool IsJump() => Input.IsActionJustPressed("ui_accept");

    public static bool IsSlide() => Input.IsActionJustPressed("ui_down");
}
