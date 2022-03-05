using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//le wallrun ne depend plus de goundedState
public class PlayerWallRunState : PlayerAbilityState
{
    protected int xInput;
    protected bool GrabInput;
    protected bool JumpInput;
    protected bool isTouchingLedge;
    
    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        //wallrun state ajouté
        player.SetAnimationState(player.PLAYER_WALL_RUN);
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
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        GrabInput = player.InputHandler.GrabInput;
        JumpInput = player.InputHandler.JumpInput;
        isTouchingLedge = player.CheckIfTouchingLedge();

        //si l'on grab jump ou se retourne
        if (xInput == player.FacingDirection && !GrabInput && !JumpInput && isTouchingLedge) return;
        //on quitte le wallRun State
        isAbilityDone = true;
    }

    public override void PhysicsUpdate()
    {
        player.SetWallRunVelocity(playerData.movementVelocity, playerData.horizontalDamping);
        base.PhysicsUpdate();
    }

    

}
