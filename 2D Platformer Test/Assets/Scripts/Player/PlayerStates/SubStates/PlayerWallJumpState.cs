using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    public int wallJumpDirecition;
    
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

        player.SetAnimationState(player.PLAYER_JUMP);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //animaiton change
        if (player.CurrentVelocity.y < 0.01f)
        {
            player.SetAnimationState(player.PLAYER_JUMPTOFALL);
        }

        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
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
