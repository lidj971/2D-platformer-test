using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.CheckIfShouldFlip(xInput);

        if (isExitingState) return;

        if (player.CurrentVelocity.x <= 0.1 && player.CurrentVelocity.x >= -0.1 && !isExitingState && xInput == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (isTouchingWall && !isTouchingLowWall && xInput != 0 && slideInput)
        {
            stateMachine.ChangeState(player.SlideState);
        }
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();

        if (xInput != 0)
        {
            if (!isAnimationFinished)
            {
                player.SetAnimationState(player.PLAYER_RUN_START);
            }
            else 
            {
                player.SetAnimationState(player.PLAYER_RUN);
            }
        }
        else
        {
            player.SetAnimationState(player.PLAYER_RUN_STOP);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocityX(playerData.movementVelocity,playerData.horizontalDamping);
    }
}
