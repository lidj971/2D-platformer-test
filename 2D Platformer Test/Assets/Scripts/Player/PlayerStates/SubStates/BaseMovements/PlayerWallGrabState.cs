﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;
    private bool slideInput;

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
        slideInput = player.InputHandler.SlideInput;

        if (isExitingState) return;
        
        HoldPosition();
        if (yInput > 0)
        {
            stateMachine.ChangeState(player.WallClimbState);
        }
        else if (!isGrounded && (yInput < 0 || !GrabInput))
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (isTouchingWall && !isTouchingLowWall && xInput != 0 && slideInput)
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
        player.KillVelocity();
    }
}
