using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    //Input 
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool dashInput;
    private bool grabInput;
    //Checks
    private bool isGrounded;

    private bool isJumping;
    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private float startWallJumpCoyoteTime;

    private bool isTouchingWall;
    private bool isTouchingWallBack;

    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;

    private bool isTouchingLedge;

    private bool canRun = false;

    public override void DoChecks()
    {
        base.DoChecks();
        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;
        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();
        isTouchingLedge = player.CheckIfTouchingLedge();

        if(isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetecetedPositon(player.transform.position);
        }

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || isTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
        
        CheckJumpMultiplier();
        //state change
        if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if(isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if(jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = player.CheckIfTouchingWall();
            player.WallJumpState.DetermineWallJumpDirecton(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);

        }else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && xInput == player.FacingDirection && player.CurrentVelocity.y <= 0)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }else if(dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            player.CheckIfShouldFlip(xInput);
            canRun = true;
        }
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();


        if (player.CurrentVelocity.y > 4)
        {
            player.SetAnimationState(player.PLAYER_JUMP, 3, 0);
        }
        else if (player.CurrentVelocity.y < 4 && player.CurrentVelocity.y > 2)
        {
            player.SetAnimationState(player.PLAYER_JUMP, 3, 1);
        }
        else if (player.CurrentVelocity.y < 2 && player.CurrentVelocity.y > -2)
        {
            player.SetAnimationState(player.PLAYER_JUMP, 3, 2);
        }
        else if (player.CurrentVelocity.y < -2 && player.CurrentVelocity.y > -4)
        {
            player.SetAnimationState(player.PLAYER_FALL, 2, 0);
        }
        else if (player.CurrentVelocity.y < -4)
        {
            player.SetAnimationState(player.PLAYER_FALL, 2, 1);
        }
    }

    private void CheckJumpMultiplier()
    {
        if (!isJumping) return;
        
        if (jumpInputStop && player.CurrentVelocity.y < playerData.minJumpVelocity)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
            isJumping = false;
        }
        else if (player.CurrentVelocity.y <= 0f)
        {
            isJumping = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!canRun) return;
        player.SetVelocityX(playerData.movementVelocity, playerData.horizontalDamping);
        if(player.CurrentVelocity.y > 0)
        {
            player.SetGravityScale(playerData.gravityScale * playerData.fallGravityMultiplier);
        }
        else
        {
            player.SetGravityScale(playerData.gravityScale);
        }
    }

    private void CheckCoyoteTime()
    {
        if (!coyoteTime || Time.time <= startTime + playerData.coyoteTime) return;
        
        coyoteTime = false;
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (!wallJumpCoyoteTime || Time.time <= startWallJumpCoyoteTime + playerData.coyoteTime) return;
        wallJumpCoyoteTime = false;
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }


    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime() 
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    } 

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;


    public void SetIsJumping() => isJumping = true;

    
}

