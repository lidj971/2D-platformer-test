﻿using System.Collections;
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
        if (player.CurrentVelocity.y > 0.0001f)
        {
            player.SetAnimationFrame(player.PLAYER_JUMP, 3, 0);
        }        
        else 
        {
            player.SetAnimationFrame(player.PLAYER_JUMP, 3, 2);
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