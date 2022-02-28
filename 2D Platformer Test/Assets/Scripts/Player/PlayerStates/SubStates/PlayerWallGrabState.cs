using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;
    
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string stateName) : base(player, stateMachine, playerData, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        holdPosition = player.transform.position;

        HoldPosition();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (isExitingState) return;
        
        HoldPosition();
        if (yInput > 0)
        {
            stateMachine.ChangeState(player.WallClimbState);
        }
        else if (!isGrounded && (yInput < 0 || !grabInput))
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (isTouchingWall && !isTouchingLowWall && xInput != 0)
        {
            stateMachine.ChangeState(player.SlideState);
        }
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        player.SetAnimationState(player.PLAYER_WALL_GRAB);
    }

    private void HoldPosition()
    {
        player.transform.position = holdPosition;
        player.KillVelocityX();
        player.SetVelocityY(0f);
    }
}
