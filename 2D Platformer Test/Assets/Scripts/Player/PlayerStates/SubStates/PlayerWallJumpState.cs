using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirecition;
    
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();
        player.JumpState.ResetAmountOfJumps();
        player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirecition);
        player.CheckIfShouldFlip(wallJumpDirecition);
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        int spaceBetweenFrames;
        if (player.CurrentVelocity.y < 0)
        {
            spaceBetweenFrames = Mathf.RoundToInt(playerData.jumpVelocity / 2);
            player.SetAnimationFrame(player.PLAYER_FALL, 2, Mathf.Abs(Mathf.RoundToInt(player.CurrentVelocity.y) / spaceBetweenFrames));
        }
        else
        {
            spaceBetweenFrames = Mathf.RoundToInt(playerData.jumpVelocity / 3);
            player.SetAnimationFrame(player.PLAYER_JUMP, 3, Mathf.RoundToInt(player.CurrentVelocity.y) / spaceBetweenFrames);
        }
    }

    public void DetermineWallJumpDirecton(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirecition = -player.FacingDirection;
        }
        else
        {
            wallJumpDirecition = player.FacingDirection;
        }
    }

}
