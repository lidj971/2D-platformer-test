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


    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string stateName) : base(player, stateMachine, playerData, stateName)
    {
    }

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
        player.SetAnimationState(player.PLAYER_LEDGEHOLD);
    }

    public override void Exit()
    {
        base.Exit();
        isHanging = false;

        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        jumpInput = player.InputHandler.JumpInput;

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
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
                player.SetAnimationState(player.PLAYER_LEDGECLIMB);
            }
            else if (jumpInput)
            {
                player.WallJumpState.wallJumpDirecition = -player.FacingDirection;
                stateMachine.ChangeState(player.WallJumpState);
            }
            else if (yInput == -1 && !isClimbing)
            {

                stateMachine.ChangeState(player.InAirState);
            }

        }

    }

    public void SetDetecetedPositon(Vector2 pos) => detectedPos = pos;
}
