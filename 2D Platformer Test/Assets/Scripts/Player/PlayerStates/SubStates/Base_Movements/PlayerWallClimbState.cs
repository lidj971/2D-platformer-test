using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState) return;
        player.SetVelocityY(playerData.wallClimbVelocity);

        if (yInput != 1)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
    }
    
    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        player.SetAnimationState(player.PLAYER_WALL_CLIMB);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
