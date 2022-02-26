using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        player.SetAnimationState(player.PLAYER_LANDING);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //player.SetVelocityX(playerData.movementVelocity, playerData.horizontalDamping);

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else 
            {
                player.KillVelocityX();
                if (isAnimationFinished)
                {
                    stateMachine.ChangeState(player.IdleState);
                }         
            }
        }

    }
}
