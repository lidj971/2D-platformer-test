using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
 
    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        player.SetAnimationState(player.PLAYER_IDLE);
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.KillVelocityX();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!player.canMove) return;

        if (isTouchingWall && !isTouchingLowWall && xInput != 0 && slideInput)
        {
            stateMachine.ChangeState(player.SlideState);
        }
        else if (xInput != 0 && !isExitingState)
        {
            //pour que le joueur ne passe pas en movestate alors qu'il va face a un mur-
            if (!isTouchingWall)
            {

                stateMachine.ChangeState(player.MoveState);
            }
            else
            {
                if (xInput != player.FacingDirection)
                {
                    stateMachine.ChangeState(player.MoveState);
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
