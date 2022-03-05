using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        player.SetAnimationState(player.PLAYER_LANDING);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState) return;
        
        if (xInput != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else
        {
            player.KillVelocityX();
            if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
