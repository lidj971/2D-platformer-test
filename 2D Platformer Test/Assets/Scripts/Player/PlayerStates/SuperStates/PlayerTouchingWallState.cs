﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingLowWall;
    protected bool jumpInput;
    protected bool grabInput;
    protected bool isTouchingLedge;
    protected int xInput;
    protected int yInput;
    
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string stateName) : base(player, stateMachine, playerData, stateName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingLedge = player.CheckIfTouchingLedge();
        isTouchingLowWall = player.CheckIfTouchingLowWall();

        if (isTouchingLedge || !isTouchingWall) return;
        player.LedgeClimbState.SetDetecetedPositon(player.transform.position);
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

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        grabInput = player.InputHandler.GrabInput;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput)
        {
            player.WallJumpState.DetermineWallJumpDirecton(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (!isTouchingWall || (xInput != player.FacingDirection && !grabInput))
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if(isTouchingWall && !isTouchingLedge)
        {
            player.StateMachine.ChangeState(player.LedgeClimbState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
