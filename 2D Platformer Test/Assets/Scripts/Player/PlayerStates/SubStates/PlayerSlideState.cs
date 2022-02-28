using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerGroundedState
{
    private bool isStartAnimationFinished;
    private bool isTouchingCeiling;

    
    public PlayerSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        isStartAnimationFinished = true;
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        if (isTouchingCeiling)
        {
            if (!isStartAnimationFinished)
            {
                player.SetAnimationState(player.PLAYER_START_SLIDING);
            }
            else
            {
                player.SetAnimationState(player.PLAYER_SLIDING);
            }
        }
        else
        {
            player.SetAnimationState(player.PLAYER_STOP_SLIDING);
        }
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingCeiling = player.CheckIfTouchingCeiling();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetActiveCollider(player.slidingCollider);
        isStartAnimationFinished = false;
        player.JumpState.SetCanJump(false);
    }

    public override void Exit()
    {
        base.Exit();
        
        player.SetActiveCollider(player.standingCollider);
        player.JumpState.SetCanJump(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.SetVelocityX(playerData.slideVelocity);

        if (!isTouchingCeiling && isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
