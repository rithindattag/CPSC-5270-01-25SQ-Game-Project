using Godot;

public static class PlayerInputFacade
{
    public static bool IsMoveLeft() => Input.IsActionJustPressed("ui_left");

    public static bool IsMoveRight() => Input.IsActionJustPressed("ui_right");

    // public static bool IsJump() => Input.IsActionJustPressed("ui_accept");

    public static bool IsJump()
{
    if (Input.IsActionJustPressed("ui_accept"))
        GD.Print("✅ Spacebar (ui_accept) pressed");
    return Input.IsActionJustPressed("ui_accept");
}


    public static bool IsSlide() => Input.IsActionJustPressed("ui_down");
}
