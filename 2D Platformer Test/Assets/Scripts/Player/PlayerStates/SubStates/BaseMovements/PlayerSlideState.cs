using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerAbilityState
{
    //Determine si l'animation de StartSliding est terminer
    private bool isStartAnimationFinished;
    //Determine si l'on est encore sous un block
    private bool isTouchingCeiling;

    private float currentSlideVelocity;
    private float currentSlideTime;
    private int startFacingDirection;
    public float time;

    private int xInput;

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
        /*if (isTouchingCeiling)
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
        }*/
        player.SetAnimationState(player.PLAYER_SLIDING);
        
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
        currentSlideTime = playerData.slideTime;
        startFacingDirection = player.FacingDirection;
        currentSlideVelocity = playerData.slideVelocity;
        Debug.Log("lol");
        isStartAnimationFinished = false;       
    }

    public override void Exit()
    {
        base.Exit();

        player.KillVelocityX();
        player.SetActiveCollider(player.standingCollider);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //currentSlideVelocity = Mathf.Lerp(currentSlideVelocity,0f,currentSlideTime) ;

        xInput = player.InputHandler.NormInputX;
        currentSlideTime -= Time.deltaTime;

        if (currentSlideTime > 0 && (startFacingDirection == xInput || xInput == 0)) return;
        if (isTouchingCeiling) return;
        isAbilityDone = true;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //player.SetVelocityX(playerData.slideVelocity);
        
    }

    public bool CheckIfCanSlide()
    {
        return (player.RB.velocity.x > playerData.minVelocity || player.RB.velocity.x < -playerData.minVelocity);
    }
}
