using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState) return;
        player.SetVelocityY(-playerData.wallSlideVelocity);

        if (isGrounded || (GrabInput && yInput == 0))
        {
            stateMachine.ChangeState(player.WallGrabState);
        }

    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        player.SetAnimationState(player.PLAYER_WALL_SLIDE);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
