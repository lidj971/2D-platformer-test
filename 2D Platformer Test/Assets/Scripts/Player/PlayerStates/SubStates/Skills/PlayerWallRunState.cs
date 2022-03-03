using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRunState : PlayerAbilityState
{
    public PlayerWallRunState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetGravityScale(0f);
        
    }

    public override void Exit()
    {
        base.Exit();
        player.SetGravityScale(playerData.gravityScale);
    }

    public override void LogicUpdate()
    {
        int xInput = player.InputHandler.NormInputX;
        base.LogicUpdate();
        if (xInput == player.FacingDirection) return;
        isAbilityDone = true;
    }

    public override void PhysicsUpdate()
    {
        player.SetVelocityY(playerData.movementVelocity, playerData.horizontalDamping);
        base.PhysicsUpdate();
    }

}
