using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    public bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingLowWall;
    protected bool isTouchingLedge;
    protected bool isTouchingWallBack;

    
    protected bool JumpInput;
    protected bool GrabInput;
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
        isTouchingWallBack = player.CheckIfTouchingWallBack();

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
        GrabInput = player.InputHandler.GrabInput;
        JumpInput = player.InputHandler.JumpInput;

        if (JumpInput)
        {
            player.WallJumpState.DetermineWallJumpDirecton(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (!isTouchingWall || (xInput != player.FacingDirection && !GrabInput))
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if (isTouchingWall && !isTouchingLedge)
        {
            player.StateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (!isTouchingWallBack && xInput == player.FacingDirection && isGrounded)
        {
            player.StateMachine.ChangeState(player.WallRunState);
        }
        else if (isGrounded && !GrabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
