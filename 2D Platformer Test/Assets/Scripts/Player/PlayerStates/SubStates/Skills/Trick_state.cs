using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trick_state : PlayerAbilityState

{
    protected int direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = player.FacingDirection
        player.SetVelocityY(playerdata.jumpVelocity)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void AnimationUpdate()
    {

    }

    public override void LogicUpdate()
    {

    }
    public override void PhysicsUpdate()
    {
        
    }
}
