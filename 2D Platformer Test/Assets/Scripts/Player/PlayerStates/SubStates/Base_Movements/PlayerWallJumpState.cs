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

        if (Time.time < startTime + playerData.wallJumpTime) return;
        
        isAbilityDone = true;
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        if (player.CurrentVelocity.y > 6)
        {
            player.SetAnimationState(player.PLAYER_JUMP, 3, 0);
        }
        else if (player.CurrentVelocity.y < 6 && player.CurrentVelocity.y > 2)
        {
            player.SetAnimationState(player.PLAYER_JUMP, 3, 1);
        }
        else if (player.CurrentVelocity.y < 2 && player.CurrentVelocity.y > -2)
        {
            player.SetAnimationState(player.PLAYER_JUMP, 3, 2);
        }
        else if (player.CurrentVelocity.y < -2 && player.CurrentVelocity.y > -6)
        {
            player.SetAnimationState(player.PLAYER_FALL, 2, 0);
        }
        else if (player.CurrentVelocity.y < -6)
        {
            player.SetAnimationState(player.PLAYER_FALL, 2, 1);
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
