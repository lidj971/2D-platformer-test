using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerAbilityState
{
    //Determine si l'animation de StartSliding est terminer
    private bool isStartAnimationFinished;
    //Determine si l'on est encore sous un block
    private bool isTouchingCeiling;

    //on appelle cette fonction lorsque StopSliding est terminer
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    //on appelle cette fonction lorsque l'animation StartSlide est finie
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
    }

    public override void Exit()
    {
        base.Exit();
        
        player.SetActiveCollider(player.standingCollider);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isTouchingCeiling || !isAnimationFinished || !isStartAnimationFinished) return;
        isAbilityDone = true;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!isStartAnimationFinished) return;
        player.SetVelocityX(playerData.slideVelocity);
    }
}
