﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool grabInput { get; private set; }
    public bool slideInput { get; private set; }

    public bool isGrounded { get; private set; }
    public bool isTouchingWall;
    public bool isTouchingLowWall { get; private set; }


    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingLowWall = player.CheckIfTouchingLowWall();
    }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetAmountOfJumps();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        JumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        slideInput = player.InputHandler.SlideInput;

        if (JumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime(); 
            stateMachine.ChangeState(player.InAirState);
        }else if(isTouchingWall && grabInput && player.StateMachine.CurrentState != player.SlideState)
        {
            if (!isTouchingLowWall)
            {
                if (xInput != 0) return;
                stateMachine.ChangeState(player.WallGrabState);
            }
            else
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
