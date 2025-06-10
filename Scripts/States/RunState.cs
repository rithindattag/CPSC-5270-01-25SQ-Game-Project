using Godot;

public class RunState : IPlayerState
{
    public void Enter(Player player)
    {
        player.PlayAnimation("Run");
    }

    public void Exit(Player player) { }

    public void Update(Player player, double delta)
    {
        if (PlayerInputFacade.IsJump())
        {
            player.ChangeState(new JumpState());
        }
        else if (PlayerInputFacade.IsSlide())
        {
            player.ChangeState(new SlideState());
        }
    }
}
