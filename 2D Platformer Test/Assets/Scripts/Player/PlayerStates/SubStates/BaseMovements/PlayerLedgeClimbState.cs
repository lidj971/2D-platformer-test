using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;

    private bool isClimbing = false;
    private bool isHanging = false;
   

    protected bool jumpInput;
    protected bool isTouchingWall;
    protected bool isGrounded;


    private int xInput;
    private int yInput;

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        player.KillVelocity();
        player.transform.position = detectedPos;
        cornerPos = player.DetermineCornerPosition();
        

        startPos.Set(cornerPos.x - (player.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (player.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();
        isHanging = false;

        if (!isClimbing) return;
        player.transform.position = stopPos;
        isClimbing = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isGrounded = player.CheckIfGrounded();
        jumpInput = player.InputHandler.JumpInput;

        if (isAnimationFinished)
        {
            if (isGrounded)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
            
        }
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;

            player.KillVelocity();
            player.transform.position = startPos;

            if (xInput == player.FacingDirection && !isClimbing)
            {
                isClimbing = true;
            }
            else if (jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirecton(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
            else if (yInput == -1 && !isClimbing)
            {

                stateMachine.ChangeState(player.InAirState);
            }

        }

    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        if (xInput == player.FacingDirection)
        {
            player.SetAnimationState(player.PLAYER_LEDGE_CLIMB);
        }
        else if (player.currentAnimationState != player.PLAYER_LEDGE_CLIMB && player.currentAnimationState != player.PLAYER_IDLE)
        {
            player.SetAnimationState(player.PLAYER_LEDGE_HOLD);
        }
    }

    public void SetDetecetedPositon(Vector2 pos) => detectedPos = pos;

    
}
